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
        private readonly SemaphoreSlim modifyMsgListSS = new SemaphoreSlim(1, 1); // Used to make sure only one task is running at once that can modify the message list, otherwise it can change at the same time another task is modifying it leading to problems
        List<Message> messages = new List<Message>(); // Used to store all the loaded messages of the currently active channel.
        List<Guild> guilds = new List<Guild>();
        int activeGuildIndex = -1;
        string activeChannelID;
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
            hideMessageControls(true);
            List<string[]> tokens = new List<string[]>{};

            if (!File.Exists(tokenFile) || File.ReadAllText(tokenFile) == "") // If the tokens file has no tokens open the login page.
            {
                addLogin();
            }
            tokens = File.ReadLines(tokenFile).Select(x => x.Split(',')).ToList();
            activeUser = new User
            {
                Token = tokens[0][1],
                ServerURL = tokens[0][0]
            };
            if (string.IsNullOrEmpty(activeUser.Token)) // If the login page has not added any tokens, the user must have closed it, so close the program.
            {
                this.Close();
                return; // Return, otherwise it starts running the next code and errors.
            }
            client = new() { BaseAddress = new Uri("http://" + activeUser.ServerURL) };

            bool fillGuildSidebarSuccess;
            do
            {
                UseWaitCursor = true;
                fillGuildSidebarSuccess = await fillGuildSidebar();
                UseWaitCursor = false;
                if (!fillGuildSidebarSuccess)
                {
                    menuStrip1.Items["offlineIndicator"].Visible = true;
                    DialogResult retry = MessageBox.Show("Could not connect to " + activeUser.ServerURL + ". Would you like to try again?", "Connection Error", MessageBoxButtons.YesNo);
                    if (retry != DialogResult.Yes)
                    {
                        Close();
                    }
                }
            } while (fillGuildSidebarSuccess == false);
            activeUser = await fetchUserInfo(token: activeUser.Token);
            if (string.IsNullOrEmpty(activeUser.ServerURL))
            {
                activeUser.ServerURL = tokens[0][0];
            }
            this.Text = "Home - " + activeUser.Name;
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
                response = await client.GetAsync("/api/guild/key/listRequests?token=" + activeUser.Token);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // If any requests returned and keys exist for it encrypt and send them.
                    try
                    {
                        foreach (dynamic request in jsonResponseObject)
                        {
                            response = await client.GetAsync("/api/user/getInfo?token=" + activeUser.Token + "&userID=" + request.UserID);
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
                                    token = activeUser.Token,
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
                response = await client.GetAsync("/api/guild/listGuilds?token=" + activeUser.Token);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
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
                    if (guilds.Count == 0)
                    {
                        txtGuildMessage.Show();
                    }
                    else
                    {
                        txtGuildMessage.Hide();
                    }
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
        public async void addLogin(bool removeInvalid = false)
        {
            List<string[]> tokens;
            User user;
            string server;
            using (var frmLogin = new frmLogin()) // Opens the login form and saves its responses.
            {
                frmLogin.ShowDialog();
                user = frmLogin.user;
                server = frmLogin.server;
                client = frmLogin.client;
            }
            Clipboard.SetText(Convert.ToBase64String(user.PrivateKey));
            MessageBox.Show("Here is your private key, you will need it if you want to be able to read messages after loging in again so store it somewhere safe. It has been added to your clipboard to paste somewher. \n" + Convert.ToBase64String(user.PrivateKey));
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
                tokens.Add(new string[] { server, user.Token, Convert.ToBase64String(user.PrivateKey) });

                if (removeInvalid == true)
                {
                    File.WriteAllText(tokenFile, ""); // Clear the file.
                    for (int i = 0; i < tokens.Count; i++) // Re-write the token list to the file.
                    {
                        if (tokens[i][1] != activeUser.Token)
                        {
                            File.AppendAllText(tokenFile, tokens[i][0] + "," + tokens[i][1] + "," + tokens[i][2] + "\r\n");
                            tokens = File.ReadLines(tokenFile).Select(x => x.Split(',')).ToList();
                        }
                    }
                    client = new() { BaseAddress = new Uri("http://" + server) }; // If the token was previously incorrect, the client needs to be set 
                }
                else
                {
                    File.AppendAllText(tokenFile, tokens[tokens.Count - 1][0] + "," + tokens[tokens.Count - 1][1] + "," + tokens[tokens.Count - 1][2] + "\r\n");

                }
                activeUser = new User();
                activeUser.ServerURL = server;
                activeUser.Token = user.Token;
            }
            tvGuilds.Nodes.Clear();
        }
        public async Task<User> fetchUserInfo(string userID = null, string token = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            User user = new User();
            if (token == null)
            {
                token = activeUser.Token;
            }
            else if (userID == null)
            {
                try
                {
                    response = await client.GetAsync("/api/account/userID?token=" + token);
                    successfullConnection = true;
                }
                catch
                {
                    successfullConnection = false;
                    menuStrip1.Items["offlineIndicator"].Visible = true;
                }
                if (successfullConnection)
                {
                    menuStrip1.Items["offlineIndicator"].Visible = false;
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        userID = jsonResponseObject.userID;
                    }
                    else
                    {
                        showError(jsonResponse);
                        return user;
                    }
                }
            }
            try
            {
                response = await client.GetAsync("/api/user/getInfo?token=" + token + "&userID=" + userID);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
                menuStrip1.Items["offlineIndicator"].Visible = true;
            }
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    user = JsonConvert.DeserializeObject<User>(jsonResponse);
                    user.Token = token;
                    user.ReadPrivateKey(tokenFile);
                }
                else
                {
                    showError(jsonResponse);
                }
            }
            return user;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await modifyMsgListSS.WaitAsync();
            try
            {
                await sendMessage(txtMessageText.Text, 1);
            }
            finally
            {
                modifyMsgListSS.Release();
            }
        }
        private async Task sendMessage(string messageContent, int type)
        {
            await displayNewerMessages();
            Message message = new Message
            {
                Type = type,
                Content = messageContent,
                ChannelID = activeChannelID,
            };
            if (string.IsNullOrWhiteSpace(message.Content)) { return; } // Do nothing if text box is empty.
            if (activeChannelID == null) { return; }
            if (guilds[activeGuildIndex].Key == null) // If the key is not held by the guild allready, read it from the file.
            {
                guilds[activeGuildIndex].GetKey();
                if (guilds[activeGuildIndex].Key == null) // If it is not in the file, request it
                {
                    bool fetchedSuccessfully = await utility.requestGuildKey(guilds[activeGuildIndex].ID, activeUser, guilds[activeGuildIndex].KeyDigest);
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
            try
            {
                var content = new 
                { 
                    token = activeUser.Token,
                    type = type,
                    channelID = message.ChannelID, 
                    content = message.CypherText, 
                    IV = Convert.ToBase64String(message.IV)
                };
                response = await client.PostAsJsonAsync("/api/content/sendMessage", content);
                successfullConnection = true;
            }
            catch (Exception ex)
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
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
                    if (type == 1) txtMessageText.Text = "";
                }
                else
                {
                    showError(jsonResponseObject);
                }
            }
            else if (!menuStrip1.Items["offlineIndicator"].Visible)
            {
                menuStrip1.Items["offlineIndicator"].Visible = true;
                MessageBox.Show("Could not connect to: " + activeUser.ServerURL, "Connection Error.");
            }
        }
        public async Task sendImageMessage(string channelID, string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            if (info.Length > 20000000)
            {
                MessageBox.Show("File too big!!!");
                return;
            }
            byte[] imageBytes = File.ReadAllBytes(filePath);
            await modifyMsgListSS.WaitAsync();
            try
            {
                await sendMessage(Convert.ToBase64String(imageBytes), 2);
            }
            finally
            {
                modifyMsgListSS.Release();
            }
        }
        private async Task displayChannel(string channelID)
        {
            showMessageControls();
            activeChannelID = channelID;
            tblMessages.Controls.Clear();
            txtKeyWarning.Visible = false;
            if (guilds[activeGuildIndex].Key == null) // If the key is not held by the guild allready, read it from the file.
            {
                guilds[activeGuildIndex].GetKey();
                if (guilds[activeGuildIndex].Key == null) // If it is not in the file, request it
                {
                    bool fetchedSuccessfully = await utility.requestGuildKey(guilds[activeGuildIndex].ID, activeUser, guilds[activeGuildIndex].KeyDigest);
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
        private async void displayMessage(Message message, int row)
        {
            byte[] profilePic = await fetchProfilePic(message.UserID);
            if (profilePic.Count() > 0) displayImage(profilePic, 0, row);
            if (message.Type == 1) displayText(message.ToString(), row);
            else if (message.Type == 2) displayImage(message.Content, 1, row);

            // TODO: work out how to scroll the message into view
        }
        private void displayText(string messageText, int row)
        {
            RichTextBox rtbMessageText = new RichTextBox();
            rtbMessageText.Text = messageText;
            rtbMessageText.ReadOnly = true;
            Size size = TextRenderer.MeasureText(rtbMessageText.Text, rtbMessageText.Font);
            rtbMessageText.Height = size.Height;
            rtbMessageText.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            rtbMessageText.BorderStyle = BorderStyle.None;
            rtbMessageText.BackColor = System.Drawing.Color.DarkOliveGreen;
            rtbMessageText.ForeColor = System.Drawing.Color.White;
            tblMessages.Controls.Add(rtbMessageText, 1, row);
        }
        private async Task<byte[]> fetchProfilePic(string userID)
        {
            const string picCacheDir = "./ProfilePicCache/";
            string picCacheFile = picCacheDir + userID;
            byte[] profilePic = Array.Empty<byte>();
            // Check disk for picture, if not found fetch it from server
            if (!Directory.Exists(picCacheDir)) Directory.CreateDirectory(picCacheDir);
            // If cached pfp is not older than an hour, read it from disk else fetch it again in case it has changed
            if (File.Exists(picCacheFile) && (DateTime.Now - File.GetLastWriteTime(picCacheFile)).TotalHours < 1)
            {
                profilePic = File.ReadAllBytes(picCacheFile);
            }
            else
            {
                // Fetch pfp from server and save it to cache
                User user = await fetchUserInfo(userID);
                try
                {
                    profilePic = Convert.FromBase64String(user.Picture);
                    if (profilePic.Length > 0) File.WriteAllBytes(picCacheFile, profilePic);
                }
                catch { };
            }
            return profilePic;
        }
        private void displayImage(string imageBase64, int collumn, int row)
        {
            byte[] imageBytes = Convert.FromBase64String(imageBase64);
            displayImage(imageBytes, collumn, row);
        }
        private void displayImage(byte[] imageBytes, int collumn, int row)
        {
            PictureBox image = new();
            try
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    image.Image = (Image.FromStream(ms));
                }
                image.SizeMode = PictureBoxSizeMode.Zoom;
                tblMessages.Controls.Add(image, collumn, row);
            }
            catch { };
        }
        private async Task<List<Message>> fetchMessages(string channelID, string afterMessageID = null, string beforeMessageID = null)
        {
            var recievedMessages = new List<Message>();
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                if (afterMessageID != null)
                {
                    response = await client.GetAsync("/api/content/getMessages?token=" + activeUser.Token + "&channelID=" + channelID + "&afterMessageID=" + afterMessageID);
                }
                else if (beforeMessageID != null)
                {
                    response = await client.GetAsync("/api/content/getMessages?token=" + activeUser.Token + "&channelID=" + channelID + "&beforeMessageID=" + beforeMessageID);
                }
                else
                {
                    response = await client.GetAsync("/api/content/getMessages?token=" + activeUser.Token + "&channelID=" + channelID);
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
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
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
                        Type = jsonResponseObject[i].Type,
                        CypherText = jsonResponseObject[i].Content,
                        Time = jsonResponseObject[i].Time,
                        IV = IV
                    };
                    recievedMessages.Add(message);
                }
            }
            else
            {
                if (!menuStrip1.Items["offlineIndicator"].Visible) // If the offline indicator is not visible
                {
                    menuStrip1.Items["offlineIndicator"].Visible = true;
                    MessageBox.Show("Could not connect to: " + activeUser.ServerURL, "Connection Error.");
                }
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
                await modifyMsgListSS.WaitAsync();
                try
                {
                    await sendMessage(txtMessageText.Text, 1);
                }
                finally 
                {
                    modifyMsgListSS.Release();
                }
            }
        }
        private void createGuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmGuildSettings = new frmGuildSettings(activeUser);
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
            activeGuildIndex = guilds.FindIndex(g => g.ID == guild.ID);
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
                    token = activeUser.Token,
                    code = inviteCode
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
                menuStrip1.Items["offlineIndicator"].Visible = false;
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
                    utility.requestGuildKey(guilds[activeGuildIndex].ID, activeUser, guilds[activeGuildIndex].KeyDigest);
                }
            }
            else
            {
                if (!menuStrip1.Items["offlineIndicator"].Visible) 
                {
                    MessageBox.Show("Could not connect to: " + activeUser.ServerURL, "Connection Error.");
                }
            }
        }
        private async void joinGuildFromCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inviteCode = Microsoft.VisualBasic.Interaction.InputBox("Join Guild", "Enter the invite code:");
            if (!string.IsNullOrEmpty(inviteCode)) await joinGuild(inviteCode);
            fillGuildSidebar();
        }

        private async Task createInvite(object sender, EventArgs e)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                var content = new
                {
                    token = activeUser.Token,
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
                menuStrip1.Items["offlineIndicator"].Visible = false;
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (jsonResponseObject.ContainsKey("errcode"))
                {
                    showError(jsonResponseObject);
                }
            }
            else
            {
 
                if (!menuStrip1.Items["offlineIndicator"].Visible)
                {
                    MessageBox.Show("Could not connect to: " + activeUser.ServerURL, "Connection Error.");
                }
            }
        }

        private void invitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activeGuildIndex != -1)
            {
                Form invites = new frmInvites(guilds[activeGuildIndex], activeUser);
                invites.Show();
            }
        }
        private void listUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activeGuildIndex != -1)
            {
                Form guildUsers = new frmGuildUsers(guilds[activeGuildIndex], activeUser);
                guildUsers.Show();
            }
        }
        
        public async Task displayNewerMessages(bool clearOld = false)
        {
            List<Message> newerMessages;
            string afterMessageID = messages.Count > 0 ? messages[messages.Count - 1].ID : null;
            newerMessages = await fetchMessages(activeChannelID, afterMessageID: afterMessageID); // Fetches the next 50 messages after the last currently displayed one

            int i;
            tblMessages.SuspendLayout();
            if (newerMessages.Count > 0 && clearOld)
            {
                i = 0;
                tblMessages.Controls.Clear();
                messages = newerMessages;
            }
            else
            {
                i = messages.Count - 1;
                messages.AddRange(newerMessages);
            }
            for (i = i; i < messages.Count - 1; i++)
            {
                messages[i].Decrypt(guilds[activeGuildIndex].Key);
                displayMessage(messages[i], i);
            }
            tblMessages.ResumeLayout();
        }
        public async Task displayOlderMessages()
        {
            if (messages.Count <= 0) return; // If there are no messages in the active channel return nothing.
            List<Message> olderMessages = await fetchMessages(activeChannelID, beforeMessageID: messages.First().ID); // Fetches the previous 50 messages before the currently displayed one.
            if (olderMessages.Count > 0)
            {
                tmrMessageCheck.Stop();
                hideMessageControls();
                messages = olderMessages;
                tblMessages.Controls.Clear();
                tblMessages.SuspendLayout();
                for (int i = 0; i < messages.Count; i++)
                {
                    messages[i].Decrypt(guilds[activeGuildIndex].Key);
                    displayMessage(messages[i], i);
                }
                tblMessages.ResumeLayout();
            }
        }
        public void hideMessageControls(bool includeNavButtons = false)
        {
            txtMessageText.Hide();
            btnEditor.Hide();
            btnSend.Hide();
            if (includeNavButtons)
            {
                btnJumpToPresent.Hide();
                btnViewNewerMessages.Hide();
                btnViewOlderMessages.Hide();
            }
        }
        public void showMessageControls()
        {
            txtMessageText.Show();
            btnEditor.Show();
            btnSend.Show();
            btnJumpToPresent.Show();
            btnViewNewerMessages.Show();
            btnViewOlderMessages.Show();
        }
        private void tmrFulfillGuildRequests_Tick(object sender, EventArgs e)
        {
            fulfillKeyRequests();
        }

        private async void tmrMessageCheck_Tick(object sender, EventArgs e)
        {
            //if (activeChannelID != null)
            //{
            //    await modifyMsgListSS.WaitAsync();
            //    try
            //    {
            //        await displayNewerMessages();
            //    }
            //    finally
            //    {
            //        modifyMsgListSS.Release();
            //    }
            //}
        }

        private async void btnEditor_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif";
            if (activeChannelID != null && openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sendImageMessage(activeChannelID, openFileDialog1.FileName);
            }
        }

        private void deleteGuildToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editGuildStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activeGuildIndex != -1)
            {
                Form frmGuildSettings = new frmGuildSettings(activeUser, guilds[activeGuildIndex].ID);
                frmGuildSettings.ShowDialog();
                fillGuildSidebar();
            }

        }

        private void viewDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(guilds[activeGuildIndex].Description);
        }

        private async void newChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string channelName = Microsoft.VisualBasic.Interaction.InputBox("Enter channel name:");
            if (!string.IsNullOrEmpty(channelName)) await createChannel(channelName);
            fillGuildSidebar();
        }
        private async Task createChannel(string channelName)
        {
            if (activeGuildIndex == -1) return;
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                var content = new
                {
                    token = activeUser.Token,
                    channelName = channelName,
                    guildID = guilds[activeGuildIndex].ID
                };
                response = await client.PostAsJsonAsync("/api/guild/channel/create", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (jsonResponse != "")
                {
                    dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (jsonResponseObject.ContainsKey("errcode"))
                    {
                        if (jsonResponseObject.errcode == "CHANNEL_EXISTS")
                        {
                            MessageBox.Show("A channel with the name " + channelName + " already exists..");
                        }
                        else
                        {
                            showError(jsonResponseObject);
                        }
                    }
                }
            }
            else
            {
                if (!menuStrip1.Items["offlineIndicator"].Visible)
                {
                    menuStrip1.Items["offlineIndicator"].Visible = true;
                    MessageBox.Show("Could not connect to: " + activeUser.ServerURL, "Connection Error.");
                }
            }
        }

        private void uploadProfilePicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                setProfilePic(openFileDialog1.FileName);
            }
        }
        private async Task setProfilePic(string path)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            string imageBase64 = Convert.ToBase64String(File.ReadAllBytes(path));
            var content = new
            {
                image = imageBase64,
                token = activeUser.Token
            };
            try
            {
                response = await client.PostAsJsonAsync("/api/account/setPicture", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (jsonResponseObject.ContainsKey("errcode"))
                {
                    showError(jsonResponseObject);
                }
                else
                {
                    File.Delete("./ProfilePicCache/" + activeUser.ID);
                }
            }
            else
            {
                if (!menuStrip1.Items["offlineIndicator"].Visible)
                {
                    menuStrip1.Items["offlineIndicator"].Visible = true;
                    MessageBox.Show("Could not connect to: " + activeUser.ServerURL, "Connection Error.");
                }
            }
        }

        private async void btnViewOlderMessages_Click(object sender, EventArgs e)
        {
            await modifyMsgListSS.WaitAsync();
            try
            {
                displayOlderMessages();
            }
            finally
            {
                modifyMsgListSS.Release();
            }
        }

        private async void btnJumpToPresent_Click(object sender, EventArgs e)
        {
            await modifyMsgListSS.WaitAsync();
            try
            {
                displayChannel(activeChannelID);
            }
            finally
            {
                modifyMsgListSS.Release();
            }
            
        }

        private async void btnViewNewerMessages_Click(object sender, EventArgs e)
        {
            await modifyMsgListSS.WaitAsync();
            try
            {
                await displayNewerMessages(true);
            }
            finally
            {
                modifyMsgListSS.Release();
            }
        }

        private async void renameChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activeChannelID == null) return;
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                var content = new
                {
                    token = activeUser.Token,
                    channelID = activeChannelID,
                    channelName = Microsoft.VisualBasic.Interaction.InputBox("Rename channel", "Enter new name:")
                };
                response = await client.PostAsJsonAsync("/api/guild/channel/rename", content);
                successfullConnection = true;
            }
            catch
            {
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                menuStrip1.Items["offlineIndicator"].Visible = false;
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (jsonResponseObject != null && jsonResponseObject.ContainsKey("errcode"))
                {
                    showError(jsonResponseObject);
                }
                else
                {
                    fillGuildSidebar();
                }
            }
            else
            {
                if (!menuStrip1.Items["offlineIndicator"].Visible)
                {
                    MessageBox.Show("Could not connect to: " + activeUser.ServerURL, "Connection Error.");
                }
            }
        }
    }
}
