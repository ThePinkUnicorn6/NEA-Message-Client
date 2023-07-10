using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeaClient
{
    public partial class frmChat : Form
    {
        List<Message> messages = new List<Message>();
        List<string[]> tokens;
        int activeToken = 0;
        User activeUser = new User();
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
            finally
            {
                if (tokens.Count < 1) // If the tokens file has no tokens, open the login page.
                {
                    addLogin();
                }
            }

        }
        public void addLogin()
        {
            string token;
            string server;
            using (var frmLogin = new frmLogin())
            {
                frmLogin.ShowDialog();
                token = frmLogin.token;
                server = frmLogin.server;
            }
            if (File.Exists("tokens.csv"))
            {
                tokens = File.ReadLines("tokens.csv").Select(x => x.Split(',')).ToList();
            }
            else
            {
                tokens = new List<string[]>();
            }
            tokens.Add(new string[] {server, token});

            File.AppendAllText("tokens.csv", tokens[tokens.Count - 1][0] + "," + tokens[tokens.Count - 1][1] + "\r\n");
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            sendMessage();
        }
        private void sendMessage()
        {
            if (txtMessageText.Text.Length < 1) { return; } // Do nothing if text box is empty.
            string userName = "";
            Message message = new Message()
            {
                UserName = "",
                Text = txtMessageText.Text
            };
            // Send message to server.
            displayMessage(message);
            txtMessageText.Text = "";
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
        }
    }
}
