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
namespace NeaClient
{
    public partial class frmChat : Form
    {
        List<Message> messages = new List<Message>(); // Used to store all the loaded messages of the currently active channel.
        List<Guild> guilds = new List<Guild>();
        Guild activeGuild;
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
        }
        private static void showError(dynamic jsonResponse)
        {
            MessageBox.Show(jsonResponse.error.ToString(), "Error: " + jsonResponse.errcode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
            string token;
            string server;
            using (var frmLogin = new frmLogin()) // Opens the login form and saves its responses.
            {
                frmLogin.ShowDialog();
                token = frmLogin.token;
                server = frmLogin.server;
                this.client = frmLogin.client;
            }
            if (File.Exists(tokenFile)) // Checks if a token file has been created, and readse its contents if it has.
            {
                tokens = File.ReadLines(tokenFile).Select(x => x.Split(',')).ToList();
            }
            else
            {
                tokens = new List<string[]>();
            }
            if (!string.IsNullOrEmpty(token)) // If the login form has responded with info, write it to the file.
            {
                tokens.Add(new string[] { server, token });
                File.AppendAllText(tokenFile, tokens[tokens.Count - 1][0] + "," + tokens[tokens.Count - 1][1] + "\r\n");
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
                File.AppendAllText(tokenFile, tokens[i][0] + "," + tokens[i][1] + "\r\n");
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
            
            List <string[]> keys = utility.getKeys(); // Read keys from file
            List <string> keyGuilds = new List<string> { }; 
            for (int i = 0; i < keys.Count; i++)
            {
                keyGuilds.Add(keys[i][0]); // Create list of just the guilds to be used to search through
            }
            

            utility.binarySearch(keyGuilds.ToArray(), activeGuild.ID, out bool found, out int keyIndex);
            if (!found)
            {
                utility.requestGuildKey(activeGuild.ID);
            }
            try
            {
                message.Encrypt(Convert.FromBase64String(keys[keyIndex][1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            // Send message to server
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                var request = new { token = tokens[activeToken][1], channelID = message.ChannelID, messageText = message.CypherText, IV = Convert.ToBase64String(message.IV) };
                var contentData = JsonContent.Create(request);
                
                response = await client.PostAsync("/api/content/sendMessage", contentData);
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
                    message.ID = jsonResponseObject.MessageID;
                    message.UserID = jsonResponseObject.UserID;
                    message.UserName = jsonResponseObject.UserName;
                    utility.checkNewMessages();
                    messages.Add(message);
                    displayMessage(message);
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
            tblMessages.Controls.Clear();
            txtKeyWarning.Visible = false;
            int keyIndex;
            bool found;
            List<string[]> keys = utility.getKeys();
            if (keys.Count == 0)
            {
                txtKeyWarning.Visible = true;
                utility.requestGuildKey(activeGuild.ID);
                return;
            }
            else
            {
                List<string> keyGuilds = new();
                for (int i = 0; i < keys.Count; i++)
                {
                    keyGuilds.Add(keys[i][0]); // Converts the keys and guilds array to an array of just guilds to search for the place to insert to.
                }
                Utility utility = new();
                utility.binarySearch(keyGuilds.ToArray(), activeGuild.ID, out found, out keyIndex);
            }
            if (!found)
            {
                txtKeyWarning.Visible = true;
                utility.requestGuildKey(activeGuild.ID);
                return;
            }
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            UseWaitCursor = true;
            try
            {
                response = await client.GetAsync("/api/content/getMessages?token=" + tokens[activeToken][1] + "&channelID=" + channelID);
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
            catch (JsonSerializationException ex)
            {
                dynamic jsonResponseError = JsonConvert.DeserializeObject<dynamic> (jsonResponse);
                showError(jsonResponseError);
                UseWaitCursor = false;
                return;
            }
            messages = new List<Message>();
            if (successfullConnection)
            {
                
                tblMessages.SuspendLayout();
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
                    byte[] key = Convert.FromBase64String(keys[keyIndex][1]);

                    message.Decrypt(key);
                    messages.Add(message);
                    displayMessage(message);
                }
                tblMessages.ResumeLayout();
                activeChannelID = channelID;
                UseWaitCursor = false;
            }
            else
            {
                MessageBox.Show("Could not connect to: " + tokens[activeToken][0], "Connection Error.");
            }
        }
        private void displayMessage(Message message)
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
            tblMessages.Controls.Add(rtbMessageText, 1, messages.Count);

            // TODO: work out how to scroll the message into view
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
            activeGuild = guild;
            displayChannel(channel.ID);
        }
        private async Task joinGuild(string inviteCode)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/join?token=" + tokens[activeToken][1] + "&code=" + inviteCode);
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
            // This line fetches the guild id from the tags of the currently selected node. If a channel is selected, it has to get the value of the parent node.
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/createInvite?token=" + tokens[activeToken][1] + "&guildID=" + activeGuild.ID);
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
            // This line fetches the guild id from the tags of the currently selected node. If a channel is selected, it has to get the value of the parent node.
            Guild activeGuild = (Guild)tvGuilds.SelectedNode.Tag ?? (Guild)tvGuilds.SelectedNode.Parent.Tag;

            if (activeGuild != null)
            {
                Form invites = new frmInvites(activeGuild, tokens, activeToken);
                invites.Show();
            }
        }
    }
}
