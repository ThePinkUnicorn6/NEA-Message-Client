namespace NeaClient
{
    partial class Chat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlGuilds = new System.Windows.Forms.Panel();
            this.tvGuilds = new System.Windows.Forms.TreeView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtMessageText = new System.Windows.Forms.TextBox();
            this.tblMessages = new System.Windows.Forms.TableLayoutPanel();
            this.btnSend = new System.Windows.Forms.Button();
            this.pnlMessages = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createGuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlGuilds.SuspendLayout();
            this.pnlMessages.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGuilds
            // 
            this.pnlGuilds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlGuilds.Controls.Add(this.tvGuilds);
            this.pnlGuilds.Controls.Add(this.txtSearch);
            this.pnlGuilds.Location = new System.Drawing.Point(12, 27);
            this.pnlGuilds.Name = "pnlGuilds";
            this.pnlGuilds.Size = new System.Drawing.Size(253, 626);
            this.pnlGuilds.TabIndex = 0;
            // 
            // tvGuilds
            // 
            this.tvGuilds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvGuilds.Location = new System.Drawing.Point(3, 32);
            this.tvGuilds.Name = "tvGuilds";
            this.tvGuilds.Size = new System.Drawing.Size(247, 594);
            this.tvGuilds.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(247, 23);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "Search Guilds/Channels:";
            // 
            // txtMessageText
            // 
            this.txtMessageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessageText.Location = new System.Drawing.Point(301, 630);
            this.txtMessageText.Multiline = true;
            this.txtMessageText.Name = "txtMessageText";
            this.txtMessageText.Size = new System.Drawing.Size(911, 23);
            this.txtMessageText.TabIndex = 1;
            // 
            // tblMessages
            // 
            this.tblMessages.AutoSize = true;
            this.tblMessages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblMessages.ColumnCount = 2;
            this.tblMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tblMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMessages.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMessages.Location = new System.Drawing.Point(0, 0);
            this.tblMessages.Name = "tblMessages";
            this.tblMessages.RowCount = 1;
            this.tblMessages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMessages.Size = new System.Drawing.Size(1025, 0);
            this.tblMessages.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(1218, 630);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // pnlMessages
            // 
            this.pnlMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMessages.AutoScroll = true;
            this.pnlMessages.Controls.Add(this.tblMessages);
            this.pnlMessages.Location = new System.Drawing.Point(268, 27);
            this.pnlMessages.Name = "pnlMessages";
            this.pnlMessages.Size = new System.Drawing.Size(1025, 597);
            this.pnlMessages.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(271, 630);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1305, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountOptionsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.fileToolStripMenuItem.Text = "Settings";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createGuildToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.testToolStripMenuItem.Text = "Guild";
            // 
            // accountOptionsToolStripMenuItem
            // 
            this.accountOptionsToolStripMenuItem.Name = "accountOptionsToolStripMenuItem";
            this.accountOptionsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.accountOptionsToolStripMenuItem.Text = "Account options";
            // 
            // createGuildToolStripMenuItem
            // 
            this.createGuildToolStripMenuItem.Name = "createGuildToolStripMenuItem";
            this.createGuildToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createGuildToolStripMenuItem.Text = "Create Guild";
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 665);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pnlMessages);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessageText);
            this.Controls.Add(this.pnlGuilds);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Chat";
            this.Text = "Home";
            this.pnlGuilds.ResumeLayout(false);
            this.pnlGuilds.PerformLayout();
            this.pnlMessages.ResumeLayout(false);
            this.pnlMessages.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnlGuilds;
        private TreeView tvGuilds;
        private TextBox txtSearch;
        private TextBox txtMessageText;
        private TableLayoutPanel tblMessages;
        private Button btnSend;
        private Panel pnlMessages;
        private Button button1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem accountOptionsToolStripMenuItem;
        private ToolStripMenuItem createGuildToolStripMenuItem;
    }
}