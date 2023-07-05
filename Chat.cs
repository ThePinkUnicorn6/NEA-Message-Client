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
    public partial class Chat : Form
    {
        List<Message> Messages = new List<Message>();
        public Chat()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            sendMessage();
        }
        private void sendMessage()
        {
            Messages.Add(new Message()
            {
                Text = txtMessageText.Text
            });
            RichTextBox rtbMessageText = new RichTextBox();
            rtbMessageText.Text = Messages[Messages.Count - 1].Text;
            rtbMessageText.ReadOnly = true;
            rtbMessageText.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            rtbMessageText.BorderStyle = BorderStyle.None;
            rtbMessageText.BackColor = System.Drawing.Color.DarkOliveGreen;
            rtbMessageText.ForeColor = System.Drawing.Color.White;
            tblMessages.Controls.Add(rtbMessageText, 1, Messages.Count);
            txtMessageText.Text = "";
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
