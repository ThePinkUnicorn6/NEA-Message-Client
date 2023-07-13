using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NeaClient
{
    public partial class frmChat : Form
    {
        List<Message> messages = new List<Message>();
        List<Guild> guilds = new List<Guild>();
        List<string[]> tokens;
        int activeToken = 0;
        User activeUser = new User();
        HttpClient client;
        public frmChat()
        {
            InitializeComponent();
            try
            {
                tokens = File.ReadLines("tokens.csv").Select(x => x.Split(',')).ToList();
            }
            catch // If the tokens file does not exist, open the login page.
            {
                addLogin();
            }
            if (tokens.Count == 0)
            {
                Close();
            }
            else
            {
                if (tokens[0][0].Length < 1) // If the tokens file has no tokens, open the login page.
                {
                    addLogin();
                }
            }
        }
        private async void frmChat_Load(object sender, EventArgs e)
        {
            client = new() { BaseAddress = new Uri("http://" + tokens[activeToken][0]) };
            bool fillGuildSidebarSuccess;
            int count = 0;
            do
            {
                fillGuildSidebarSuccess = await fillGuildSidebar();
                count++;
            } while (fillGuildSidebarSuccess == false && count <= 5);
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
                MessageBox.Show("Could not connect to " + tokens[activeToken][0]);
                successfullConnection = false;
            }
            if (successfullConnection)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResponseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        guilds = new List<Guild>();
                        foreach (dynamic item in jsonResponseObject)
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
                    catch 
                    {

                    }
                    
                    tvGuilds.BeginUpdate();
                    tvGuilds.Nodes.Clear();
                    foreach (Guild guild in guilds)
                    {
                        tvGuilds.Nodes.Add(new TreeNode (guild.Name));
                        foreach (Channel channel in guild.Channels)
                        {
                            tvGuilds.Nodes[guilds.IndexOf(guild)].Nodes.Add(new TreeNode (channel.Name));
                        }
                    }
                    tvGuilds.EndUpdate();
                }
                else if (jsonResponseObject.errcode.ToString() == "INVALID_TOKEN")
                {
                    successfullConnection = false;
                    addLogin();
                }
                else
                {
                    MessageBox.Show(jsonResponseObject.error.ToString(), "Error: " + jsonResponseObject.errcode.ToString());
                    successfullConnection = false;
                }
            }
            return successfullConnection;
        }
        public void addLogin()
        {
            string token;
            string server;
            using (var frmLogin = new frmLogin()) // Opens the login form and saves its responses.
            {
                frmLogin.ShowDialog();
                token = frmLogin.token;
                server = frmLogin.server;
                this.client = frmLogin.client;
                frmLogin.Dispose();
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
                activeToken = tokens.Count - 1;
            }
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
        private void sendMessage()
        {
            if (string.IsNullOrWhiteSpace(txtMessageText.Text)) { return; } // Do nothing if text box is empty.
            Message message = new Message()
            {
                UserName = activeUser.UserName,
                UserID = activeUser.UserID,
                Text = txtMessageText.Text
            };
            // Send message to server.

            //if (jsonResponseObject.MessageID is not null)
            //{
            //    displayMessage(message);
            //    txtMessageText.Text = "";
            //}

        }
        private void displayMessage(Message message)
        {
            messages.Add(message);
            RichTextBox rtbMessageText = new RichTextBox();
            rtbMessageText.Text = messages[messages.Count - 1].ComposeString();
            rtbMessageText.ReadOnly = true;
            Size size = TextRenderer.MeasureText(rtbMessageText.Text, rtbMessageText.Font);
            rtbMessageText.Height = size.Height;
            rtbMessageText.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            rtbMessageText.BorderStyle = BorderStyle.None;
            rtbMessageText.BackColor = System.Drawing.Color.DarkOliveGreen;
            rtbMessageText.ForeColor = System.Drawing.Color.White;
            bool autoScroll = true;
            if (tblMessages.VerticalScroll.Value != tblMessages.VerticalScroll.Maximum) // If the user has scrolled up to look at a specific message, dont scroll back down to see new messages.
            {
                autoScroll = false;
            }
            tblMessages.Controls.Add(rtbMessageText, 1, messages.Count);
            if (autoScroll == true)
            {
                tblMessages.VerticalScroll.Value = tblMessages.VerticalScroll.Maximum;
            }
        }
        private void sendRequest(string URL)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txtMessageText_KeyDown(object sender, KeyEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void createGuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmGuildSettings = new frmGuildSettings(tokens[activeToken]);
            frmGuildSettings.ShowDialog();
            fillGuildSidebar();
        }
    }
}
