using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Net;

namespace NeaClient
{
    public partial class frmChat : Form
    {
        List<Message> messages = new List<Message>(); // Used to store all the loaded messages of the currently active channel.
        List<Guild> guilds = new List<Guild>();
        int activeGuildIndex;
        string activeChannelID;
        List<string[]> tokens;
        int activeToken = 0; // Used to store the index of the token currently in use in the list tokens.
        User activeUser = new User(); // Used to store the information of the logged in user.
        HttpClient client;
        Utility utility = new Utility();
        int lastMessageTime;
        const string tokenFile = "tokens.csv";
        const string keyFile = "guildKeys.csv";
        public frmChat()
        {
            InitializeComponent();
        }
        private async void frmChat_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.ReadAllText(tokenFile) == "") // If the tokens file has no tokens open the login page.
                {
                    addLogin();
                }
                tokens = File.ReadLines(tokenFile).Select(x => x.Split(',')).ToList();
            }
            catch // If the tokens file does not exist, open the login page.
            {
                addLogin();
            }
            if (tokens.Count == 0) // If the login page has not added any tokens, the user must have closed it, so close the program.
            {
                this.Close();
                return; // Return, otherwise it starts running the next code and errors.
            }
            client = new() { BaseAddress = new Uri("http://" + tokens[activeToken][0]) };
            activeUser = new User 
            {

            };
            bool fillGuildSidebarSuccess;
            do
            {
                UseWaitCursor = true;
                fillGuildSidebarSuccess = await fillGuildSidebar();
                UseWaitCursor = false;
                if (!fillGuildSidebarSuccess)
                {
                    DialogResult retry = MessageBox.Show("Could not connect to " + tokens[activeToken][0] + ". Would you like to try again?", "Connection Error", MessageBoxButtons.YesNo);
                    if (retry != DialogResult.Yes)
                    {
                        Close();
                    }
                }
            } while (fillGuildSidebarSuccess == false);
            fulfillKeyRequests();
        }
        private static void showError(dynamic jsonResponse)
        {
            MessageBox.Show(jsonResponse.error.ToString(), "Error: " + jsonResponse.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async Task fulfillKeyRequests()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/key/listRequests?token=" + tokens[activeToken][1]);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // If any requests returned and keys exist for it encrypt and send them.
                    try
                    {
                        foreach (dynamic request in jsonResponseObject)
                        {
                            response = await client.GetAsync("/api/user/getInfo?token=" + tokens[activeToken][1] + "&userID=" + request.UserID);
                            var jsonResponseUser = await response.Content.ReadAsStringAsync();
                            dynamic jsonResponseObjectUser = JsonConvert.DeserializeObject<dynamic>(jsonResponseUser);
                            byte[] publicKey = Convert.FromBase64String((string)jsonResponseObjectUser.PublicKey);
                            Guild guild = new Guild
                            {
                                ID = (string)request.GuildID,
                            };
                            guild.GetKey();
                            if (guild.Key != null)
                            {
                                string keyCypherText;
                                using (RSA rsa = RSA.Create())
                                {
                                    rsa.ImportRSAPublicKey(publicKey, out _);
                                    keyCypherText = Convert.ToBase64String(rsa.Encrypt(guild.Key, RSAEncryptionPadding.OaepSHA256));
                                }
                                var content = new
                                {
                                    token = tokens[activeToken][1],
                                    keyCypherText = keyCypherText,
                                    guildID = guild.ID,
                                    userID = (string)request.UserID,
                                };
                                response = await client.PostAsJsonAsync("/api/guild/key/submit", content);
                                return;
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        public async Task<bool> fillGuildSidebar()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/listGuilds?token=" + tokens[activeToken][1]);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    guilds = new List<Guild>();
                    foreach (dynamic item in jsonResponseObject)
                    {
                        if (item.ContainsKey("channels"))
                        {
                            List<Channel> channels = new List<Channel>();
                            foreach (dynamic channel in item.channels)
                            {
                                channels.Add(new Channel
                                {
                                    Name = channel.channelName,
                                    ID = channel.channelID,
                                    Description = channel.channelDesc,
                                    Type = channel.channelType
                                });
                            }

                            guilds.Add(new Guild
                            {
                                Name = item["guildName"],
                                ID = item["guildID"],
                                OwnerID = item["ownerID"],
                                Description = item["guildDesc"],
                                KeyDigest = item["guildKeyDigest"],
                                Channels = channels
                            });
                        }
                    }
                    
                    tvGuilds.BeginUpdate();
                    tvGuilds.Nodes.Clear();
                    foreach (Guild guild in guilds)
                    {
                        TreeNode tempNode = new TreeNode(guild.Name);
                        tempNode.Tag = guild; // Store the guild in the node tag so it can be identified when clicked on.
                        tvGuilds.Nodes.Add(tempNode);
                        foreach (Channel channel in guild.Channels)
                        {
                            tempNode = new TreeNode(channel.Name);
                            tempNode.Tag = channel; // Store the channel in the node tag so it can be identified when clicked on.
                            tvGuilds.Nodes[guilds.IndexOf(guild)].Nodes.Add(tempNode);
                        }
                    }
                    tvGuilds.EndUpdate();
                }
                else if (jsonResponseObject.errcode.ToString() == "INVALID_TOKEN")
                {
                    addLogin(true);
                    successfullConnection = await fillGuildSidebar();
                }
                else
                {
                    showError(jsonResponseObject);
                    successfullConnection = false;
                }
            }
            return successfullConnection;
        }
        public void addLogin(bool removeInvalid = false)
        {
            User user;
            string server;
            using (var frmLogin = new frmLogin()) // Opens the login form and saves its responses.
            {
                frmLogin.ShowDialog();
                user = frmLogin.user;
                server = frmLogin.server;
                client = frmLogin.client;
            }
            if (File.Exists(tokenFile)) // Checks if a token file has been created, and readse its contents if it has.
            {
                tokens = File.ReadLines(tokenFile).Select(x => x.Split(',')).ToList();
            }
            else
            {
                tokens = new List<string[]>();
            }
            if (user != null && user.Token != null) // If the login form has responded with info, write it to the file.
            {
                tokens.Add(new string[] { server, user.Token , Convert.ToBase64String(user.PrivateKey) });
                File.AppendAllText(tokenFile, tokens[tokens.Count - 1][0] + "," + tokens[tokens.Count - 1][1] +  "," + tokens[tokens.Count - 1][2] + "\r\n");
                if (removeInvalid == true)
                {
                    removeToken(activeToken);
                    client = new() { BaseAddress = new Uri("http://" + tokens[activeToken][0]) }; // If the token was previously incorrect, the client needs to be set 
                }
                activeToken = tokens.Count - 1;
            }
            tvGuilds.Nodes.Clear();
        }
        public void removeToken(int tokenIndex)
        {
            tokens.RemoveAt(tokenIndex); // Remove the token from the list.
            File.WriteAllText(tokenFile, ""); // Clear the file.
            for (int i = 0; i < tokens.Count; i++) // Re-write the token list to the file.
            {
                File.AppendAllText(tokenFile, tokens[i][0] + "," + tokens[i][1] + "," + tokens[i][2] + "\r\n");
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            sendMessage();
        }
        private async Task sendMessage()
        {
            Message message = new Message
            {
                PlainText = txtMessageText.Text,
                ChannelID = activeChannelID,
            };
            if (string.IsNullOrWhiteSpace(message.PlainText)) { return; } // Do nothing if text box is empty.
            if (guilds[activeGuildIndex].Key == null) // If the key is not held by the guild allready, read it from the file.
            {
                guilds[activeGuildIndex].GetKey();
                if (guilds[activeGuildIndex].Key == null) // If it is not in the file, request it
                {
                    bool fetchedSuccessfully = await requestGuildKey(guilds[activeGuildIndex].ID);
                    if (fetchedSuccessfully) // If a key request has been previously made, another user might have submitted their keys, so the key should be in the keyfile.
                    {
                        guilds[activeGuildIndex].GetKey();
                    }
                    else
                    {
                        txtKeyWarning.Visible = true;
                        return;
                    }
                }
            }
            try
            {
                message.Encrypt(guilds[activeGuildIndex].Key);
            }
            catch {}
            // Send message to server
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            await displayNewMessages();
            try
            {
                var content = new 
                { 
                    token = tokens[activeToken][1],
                    channelID = message.ChannelID, 
                    messageText = message.CypherText, 
                    IV = Convert.ToBase64String(message.IV)
                };
                response = await client.PostAsJsonAsync("/api/content/sendMessage", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (response.IsSuccessStatusCode)
                {
                    message.ID = jsonResponseObject.ID;
                    message.UserID = jsonResponseObject.UserID;
                    message.UserName = jsonResponseObject.UserName;
                    message.Time = jsonResponseObject.Time;
                    messages.Add(message);
                    displayMessage(message, messages.Count);
                    txtMessageText.Text = "";
                }
                else
                {
                    showError(jsonResponseObject);
                }
            }
            else
            {
                MessageBox.Show("Could not connect to: " + tokens[activeToken][0], "Connection Error.");
            }
        }

        private async Task displayChannel(string channelID)
        {
            activeChannelID = channelID;
            tblMessages.Controls.Clear();
            txtKeyWarning.Visible = false;
            if (guilds[activeGuildIndex].Key == null) // If the key is not held by the guild allready, read it from the file.
            {
                guilds[activeGuildIndex].GetKey();
                if (guilds[activeGuildIndex].Key == null) // If it is not in the file, request it
                {
                    bool fetchedSuccessfully = await requestGuildKey(guilds[activeGuildIndex].ID);
                    if (fetchedSuccessfully) // If a key request has been previously made, another user might have submitted their keys, so the key should be in the keyfile.
                    {
                        guilds[activeGuildIndex].GetKey();
                    }
                    else
                    {
                        txtKeyWarning.Visible = true;
                        return;
                    }
                }
            }
            UseWaitCursor = true;
            messages = await fetchMessages(channelID);
            UseWaitCursor = false;
            tblMessages.SuspendLayout();
            for (int i = 0; i < messages.Count; i++)
            {
                messages[i].Decrypt(guilds[activeGuildIndex].Key);
                displayMessage(messages[i], i);
            }
            tblMessages.ResumeLayout();
            UseWaitCursor = false;
        }
        private void displayMessage(Message message, int row)
        {
            RichTextBox rtbMessageText = new RichTextBox();
            rtbMessageText.Text = message.ToString();
            rtbMessageText.ReadOnly = true;
            Size size = TextRenderer.MeasureText(rtbMessageText.Text, rtbMessageText.Font);
            rtbMessageText.Height = size.Height;
            rtbMessageText.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            rtbMessageText.BorderStyle = BorderStyle.None;
            rtbMessageText.BackColor = System.Drawing.Color.DarkOliveGreen;
            rtbMessageText.ForeColor = System.Drawing.Color.White;
            tblMessages.Controls.Add(rtbMessageText, 1, row);

            // TODO: work out how to scroll the message into view
        }
        private async Task<List<Message>> fetchMessages(string channelID, string afterMessageID = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                if (afterMessageID == null)
                {
                    response = await client.GetAsync("/api/content/getMessages?token=" + tokens[activeToken][1] + "&channelID=" + channelID);
                }
                else
                {
                    response = await client.GetAsync("/api/content/getMessages?token=" + tokens[activeToken][1] + "&channelID=" + channelID + "&afterMessageID=" + afterMessageID);
                }
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            List<dynamic> jsonResponseObject;
            try // If there is an errorcode returned from the server, it won't be in the format of a list so will cause an exeption that needs to be caught.
            {
                jsonResponseObject = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);
            }
            catch
            {
                dynamic jsonResponseError = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                showError(jsonResponseError);
                return new List<Message>();
            }
            var recievedMessages = new List<Message>();
            if (successfullConnection)
            {
                for (int i = 0; i < jsonResponseObject.Count; i++)
                {
                    byte[] IV;
                    try
                    {
                        IV = Convert.FromBase64String(jsonResponseObject[i].IV.ToString());
                    }
                    catch { IV = new byte[16]; }
                    Message message = new Message
                    {
                        ID = jsonResponseObject[i].ID,
                        UserName = jsonResponseObject[i].UserName,
                        ChannelID = jsonResponseObject[i].ChannelID,
                        UserID = jsonResponseObject[i].UserID,
                        CypherText = jsonResponseObject[i].Text,
                        Time = jsonResponseObject[i].Time,
                        IV = IV
                    };
                    recievedMessages.Add(message);
                }
            }
            else
            {
                MessageBox.Show("Could not connect to: " + tokens[activeToken][0], "Connection Error.");
            }
            return recievedMessages;
        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void txtMessageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Control.ModifierKeys != Keys.Shift)
            {
                e.SuppressKeyPress = true;
                await sendMessage();
            }
        }

        private void addAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addLogin();
        }

        private void createGuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmGuildSettings = new frmGuildSettings(tokens[activeToken]);
            frmGuildSettings.ShowDialog();
            fillGuildSidebar();
        }

        private async void tvGuilds_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            Channel channel;
            Guild guild;
            if (e.Node.Tag.GetType() != typeof(Channel)) // If the user has clicked on a guild, display the messages of the first channel in the guild.
            {
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                    return;
                }
                else
                {
                    txtKeyWarning.Visible = false;
                    guild = (Guild)e.Node.Tag;
                    channel = guild.Channels[0];
                    e.Node.Expand();
                }
            }
            else // If the user has clicked on a channel, display it.
            {
                channel = (Channel)e.Node.Tag;
                guild = (Guild)e.Node.Parent.Tag;
            }
            guilds[activeGuildIndex] = guild;
            await displayChannel(channel.ID);
        }
        private async Task joinGuild(string inviteCode)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                var content = new
                {
                    token = tokens[activeToken][1],
                    code =inviteCode
                };
                response = await client.PostAsJsonAsync("/api/guild/join", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (jsonResponseObject.ContainsKey("errcode"))
                {
                    if (jsonResponseObject.errcode == "INVALID_INVITE")
                    {
                        MessageBox.Show("Invalid code: " + inviteCode + ", please try again.");
                    }
                    else
                    {
                        showError(jsonResponseObject);
                    }
                }
                else
                {
                    fillGuildSidebar();
                }
            }
            else
            {
                MessageBox.Show("Could not connect to: " + tokens[activeToken][0], "Connection Error.");
            }
        }
        private void joinGuildFromCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inviteCode = Microsoft.VisualBasic.Interaction.InputBox("Join Guild", "Enter the invite code:");
            joinGuild(inviteCode);
        }

        private async Task createInvite(object sender, EventArgs e)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                var content = new
                {
                    token = tokens[activeToken][1],
                    guildID = guilds[activeGuildIndex].ID
                };
                response = await client.PostAsJsonAsync("/api/guild/createInvite", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (jsonResponseObject.ContainsKey("errcode"))
                {
                    showError(jsonResponseObject);
                }
            }
            else
            {
                MessageBox.Show("Could not connect to: " + tokens[activeToken][0], "Connection Error.");
            }
        }

        private void invitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (guilds[activeGuildIndex] != null)
            {
                Form invites = new frmInvites(guilds[activeGuildIndex], tokens, activeToken);
                invites.Show();
            }
        }
        public async Task<bool> requestGuildKey(string guildID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            byte[] keyCypherText;
            try
            {
                var content = new
                {
                    token = tokens[activeToken][1],
                    guildID = guildID
                };
                response = await client.PostAsJsonAsync("/api/guild/key/request", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if ((int)response.StatusCode == 200 && jsonResponseObject != null && jsonResponseObject.ContainsKey("key")) // If the client has requested the keys previously and another user has submitted the keys, 
                {
                    byte[] guildKey;
                    keyCypherText = Convert.FromBase64String((string)jsonResponseObject.key);
                    User user = new();
                    user.ReadPrivateKey(tokenFile, activeToken);
                    // Decrypt guild key
                    using (RSA rsa = RSA.Create())
                    {
                        rsa.ImportRSAPrivateKey(user.PrivateKey, out _);
                        guildKey = rsa.Decrypt(keyCypherText, RSAEncryptionPadding.OaepSHA256);
                    }
                    // Check if key is valid
                    if (Convert.ToBase64String(SHA256.HashData(guildKey)) == guilds[activeGuildIndex].KeyDigest)
                    {
                        // If the key that has been submited is correct, save it to file.
                        utility.saveKey(guildID, guildKey);
                        return true;
                    }
                }
                else if (jsonResponseObject != null && jsonResponseObject.ContainsKey("errcode"))
                {
                    showError(jsonResponseObject);
                }
            }
            else
            {
                MessageBox.Show("Could not connect to: " + tokens[activeToken][0], "Connection Error.");
            }
            return false;
        }
        public async Task displayNewMessages()
        {
            int initialMessageCount = messages.Count;
            if (messages.Count > 0)
            {
                messages.AddRange(await fetchMessages(activeChannelID, messages[messages.Count - 1].ID));
            }
            if (messages.Count > initialMessageCount)
            {
                tblMessages.SuspendLayout();
                for (int i = initialMessageCount; i < messages.Count; i++)
                {
                    messages[i].Decrypt(guilds[activeGuildIndex].Key);
                    displayMessage(messages[i], i);
                }
                tblMessages.ResumeLayout();
            }
        }
        private void tmrFulfillGuildRequests_Tick(object sender, EventArgs e)
        {
            fulfillKeyRequests();
        }

        private void tmrMessageCheck_Tick(object sender, EventArgs e)
        {
            displayNewMessages();
        }
    }
}
