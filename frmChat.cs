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
        List<string> tokens;
        public frmChat()
        {
            InitializeComponent();
            try
            {
                tokens = File.ReadAllLines("tokens").ToList();
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
            using (var frmLogin = new frmLogin())
            {
                frmLogin.ShowDialog();
                token = frmLogin.token;
            }
            if (File.Exists(token))
            {
                tokens = File.ReadAllLines("tokens").ToList();
            }
            else
            {
                tokens = new List<string>();
            }
            tokens.Add(token);
            File.WriteAllLines("tokens", tokens);
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            sendMessage();
        }
        private void sendMessage()
        {
            if (txtMessageText.Text.Length < 1) { return; } // Do nothing if text box is empty.
            Message message = new Message()
            {
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
            rtbMessageText.Text = messages[messages.Count - 1].Text;
            rtbMessageText.ReadOnly = true;
            rtbMessageText.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            rtbMessageText.BorderStyle = BorderStyle.None;
            rtbMessageText.BackColor = System.Drawing.Color.DarkOliveGreen;
            rtbMessageText.ForeColor = System.Drawing.Color.White;
            tblMessages.Controls.Add(rtbMessageText, 1, messages.Count);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
