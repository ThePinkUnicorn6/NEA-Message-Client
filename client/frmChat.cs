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
using Newtonsoft.Json;

using System.Security.Cryptography;
namespace NeaClient
{
    public partial class frmChat : Form
    {
        List<Message> messages = new List<Message>(); // Used to store all the loaded messages of the currently active channel.
        List<Guild> guilds = new List<Guild>();
        string activeChannelID;
        List<string[]> tokens;
        int activeToken = 0; // Used to store the index of the token currently in use in the list tokens.
        User activeUser = new User(); // Used to store the information of the logged in user.
        HttpClient client;
        int lastMessageTime;
        public frmChat()
        {
            InitializeComponent();
        }
        private async void frmChat_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.ReadAllText("tokens.csv") == "") // If the tokens file has no tokens open the login page.
                {
                    addLogin();
                }
                tokens = File.ReadLines("tokens.csv").Select(x => x.Split(',')).ToList();
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
            int count = 0;
            do
            {
                UseWaitCursor = true;
                fillGuildSidebarSuccess = await fillGuildSidebar();
                UseWaitCursor = false;
                count++;
                if (count == 5)
                {
                    DialogResult retry = MessageBox.Show("Could not connect to " + tokens[activeToken][0] + ". Would you like to try again?", "Connection Error", MessageBoxButtons.YesNo);
                    if (retry == DialogResult.Yes)
                    {
                        count = 0;
                    }
                    else
                    {
                        Close();
                    }
                }
            } while (fillGuildSidebarSuccess == false && count <= 5);
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
                    successfullConnection = false;
                    addLogin(true);
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
            if (File.Exists("tokens.csv")) // Checks if a token file has been created, and readse its contents if it has.
            {
                tokens = File.ReadLines("tokens.csv").Select(x => x.Split(',')).ToList();
            }
            else
            {
                tokens = new List<string[]>();
            }
            if (!string.IsNullOrEmpty(token)) // If the login form has responded with info, write it to the file.
            {
                tokens.Add(new string[] { server, token });
                File.AppendAllText("tokens.csv", tokens[tokens.Count - 1][0] + "," + tokens[tokens.Count - 1][1] + "\r\n");
                if (removeInvalid == true) removeToken(activeToken);
                activeToken = tokens.Count - 1;
            }
            tvGuilds.Nodes.Clear();
        }
        public void removeToken(int tokenIndex)
        {
            tokens.RemoveAt(tokenIndex); // Remove the token from the list.
            File.WriteAllText("tokens.csv", ""); // Clear the file.
            for (int i = 0; i < tokens.Count; i++) // Re-write the token list to the file.
            {
                File.AppendAllText("tokens.csv", tokens[i][0] + "," + tokens[i][1] + "\r\n");
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
                Text = txtMessageText.Text,
                ChannelID = activeChannelID,
            };
            if (string.IsNullOrWhiteSpace(message.Text)) { return; } // Do nothing if text box is empty.
            byte[] key =             {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
            };
            Message plaintextMessage = message;
            try
            {
                message.Encrypt(key);
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
                response = await client.GetAsync("/api/content/sendMessage?token=" + tokens[activeToken][1] + "&channelID=" + message.ChannelID + "&messageText=" + message.Text + "&messageIV=" + Convert.ToBase64String(message.IV));
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
                    checkNewMessages();
                    messages.Add(plaintextMessage);
                    displayMessage(plaintextMessage);
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
                // This line fetches the guild id from the tags of the currently selected node. If a channel is selected, it has to get the value of the parent node.
                

                tblMessages.Controls.Clear();
                tblMessages.SuspendLayout();
                for (int i = 0; i < jsonResponseObject.Count; i++)
                {
                    Message message = new Message
                    {
                        ID = jsonResponseObject[i].ID,
                        UserName = jsonResponseObject[i].UserName,
                        ChannelID = jsonResponseObject[i].ChannelID,
                        UserID = jsonResponseObject[i].UserID,
                        Text = jsonResponseObject[i].Text,
                        Time = jsonResponseObject[i].Time,
                        IV = jsonResponseObject[i].IV
                    };
                    byte[] key = {
                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                    };

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
        private async Task checkNewMessages()
        {

        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void txtMessageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Control.ModifierKeys != Keys.Shift)
            {
                e.SuppressKeyPress = true;
                sendMessage();
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

        private void tvGuilds_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Channel channel;
            if (e.Node.Tag.GetType() != typeof(Channel)) // If the user has clicked on a guild, display the messages of the first channel in the guild.
            {
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                    return;
                }
                else
                {
                    Guild guild = (Guild)e.Node.Tag;
                    channel = guild.Channels[0];
                    e.Node.Expand();
                }
            }
            else // If the user has clicked on a channel, display it.
            {
                channel = (Channel)e.Node.Tag;
                string channelID = channel.ID;
            }
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
            string guildID = (Guild)tvGuilds.SelectedNode.Tag != null ? ((Guild)tvGuilds.SelectedNode.Tag).ID.ToString() : ((Guild)tvGuilds.SelectedNode.Parent.Tag).ID.ToString(); 
            HttpResponseMessage response = new HttpResponseMessage();
            bool successfullConnection;
            try
            {
                response = await client.GetAsync("/api/guild/createInvite?token=" + tokens[activeToken][1] + "&guildID=" + guildID);
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
            Guild activeGuild = (Guild)tvGuilds.SelectedNode.Tag != null ? (Guild)tvGuilds.SelectedNode.Tag : (Guild)tvGuilds.SelectedNode.Parent.Tag;

            if (activeGuild != null)
            {
                Form invites = new frmInvites(activeGuild, tokens, activeToken);
                invites.Show();
            }

        }
    }
}
