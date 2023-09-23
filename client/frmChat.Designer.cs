﻿namespace NeaClient
{
    partial class frmChat
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
            this.btnEditor = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createGuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinGuildFromCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.privateChatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guildSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invitesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createInviteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInviteCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlMessages = new System.Windows.Forms.Panel();
            this.pnlGuilds.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnlMessages.SuspendLayout();
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
            this.pnlGuilds.Size = new System.Drawing.Size(253, 642);
            this.pnlGuilds.TabIndex = 0;
            // 
            // tvGuilds
            // 
            this.tvGuilds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvGuilds.Location = new System.Drawing.Point(3, 32);
            this.tvGuilds.Name = "tvGuilds";
            this.tvGuilds.ShowPlusMinus = false;
            this.tvGuilds.Size = new System.Drawing.Size(247, 514);
            this.tvGuilds.TabIndex = 1;
            this.tvGuilds.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvGuilds_NodeMouseClick);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Search Guilds/Channels:";
            this.txtSearch.Size = new System.Drawing.Size(247, 23);
            this.txtSearch.TabIndex = 0;
            // 
            // txtMessageText
            // 
            this.txtMessageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessageText.Location = new System.Drawing.Point(304, 551);
            this.txtMessageText.Multiline = true;
            this.txtMessageText.Name = "txtMessageText";
            this.txtMessageText.Size = new System.Drawing.Size(677, 23);
            this.txtMessageText.TabIndex = 1;
            this.txtMessageText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessageText_KeyDown);
            // 
            // tblMessages
            // 
            this.tblMessages.AutoSize = true;
            this.tblMessages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblMessages.ColumnCount = 2;
            this.tblMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tblMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblMessages.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMessages.Location = new System.Drawing.Point(0, 0);
            this.tblMessages.Name = "tblMessages";
            this.tblMessages.RowCount = 1;
            this.tblMessages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMessages.Size = new System.Drawing.Size(793, 0);
            this.tblMessages.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(987, 550);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnEditor
            // 
            this.btnEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditor.Location = new System.Drawing.Point(271, 551);
            this.btnEditor.Name = "btnEditor";
            this.btnEditor.Size = new System.Drawing.Size(27, 23);
            this.btnEditor.TabIndex = 5;
            this.btnEditor.Text = "+";
            this.btnEditor.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.testToolStripMenuItem,
            this.privateChatsToolStripMenuItem,
            this.guildSettingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1074, 24);
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
            // accountOptionsToolStripMenuItem
            // 
            this.accountOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAccountToolStripMenuItem});
            this.accountOptionsToolStripMenuItem.Name = "accountOptionsToolStripMenuItem";
            this.accountOptionsToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.accountOptionsToolStripMenuItem.Text = "Account Options";
            // 
            // addAccountToolStripMenuItem
            // 
            this.addAccountToolStripMenuItem.Name = "addAccountToolStripMenuItem";
            this.addAccountToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.addAccountToolStripMenuItem.Text = "Add Account";
            this.addAccountToolStripMenuItem.Click += new System.EventHandler(this.addAccountToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createGuildToolStripMenuItem,
            this.joinGuildFromCodeToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.testToolStripMenuItem.Text = "Guilds";
            // 
            // createGuildToolStripMenuItem
            // 
            this.createGuildToolStripMenuItem.Name = "createGuildToolStripMenuItem";
            this.createGuildToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createGuildToolStripMenuItem.Text = "Create Guild";
            this.createGuildToolStripMenuItem.Click += new System.EventHandler(this.createGuildToolStripMenuItem_Click);
            // 
            // joinGuildFromCodeToolStripMenuItem
            // 
            this.joinGuildFromCodeToolStripMenuItem.Name = "joinGuildFromCodeToolStripMenuItem";
            this.joinGuildFromCodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.joinGuildFromCodeToolStripMenuItem.Text = "Join Guild";
            this.joinGuildFromCodeToolStripMenuItem.Click += new System.EventHandler(this.joinGuildFromCodeToolStripMenuItem_Click);
            // 
            // privateChatsToolStripMenuItem
            // 
            this.privateChatsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDMToolStripMenuItem});
            this.privateChatsToolStripMenuItem.Name = "privateChatsToolStripMenuItem";
            this.privateChatsToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.privateChatsToolStripMenuItem.Text = "Private Chats";
            // 
            // createDMToolStripMenuItem
            // 
            this.createDMToolStripMenuItem.Name = "createDMToolStripMenuItem";
            this.createDMToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createDMToolStripMenuItem.Text = "Create DM";
            // 
            // guildSettingsToolStripMenuItem
            // 
            this.guildSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listUsersToolStripMenuItem,
            this.invitesToolStripMenuItem});
            this.guildSettingsToolStripMenuItem.Name = "guildSettingsToolStripMenuItem";
            this.guildSettingsToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.guildSettingsToolStripMenuItem.Text = "Guild Settings";
            // 
            // listUsersToolStripMenuItem
            // 
            this.listUsersToolStripMenuItem.Name = "listUsersToolStripMenuItem";
            this.listUsersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.listUsersToolStripMenuItem.Text = "List Users";
            // 
            // invitesToolStripMenuItem
            // 
            this.invitesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createInviteToolStripMenuItem,
            this.viewInviteCodesToolStripMenuItem});
            this.invitesToolStripMenuItem.Name = "invitesToolStripMenuItem";
            this.invitesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.invitesToolStripMenuItem.Text = "Invites";
            // 
            // createInviteToolStripMenuItem
            // 
            this.createInviteToolStripMenuItem.Name = "createInviteToolStripMenuItem";
            this.createInviteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createInviteToolStripMenuItem.Text = "Create Invite Code";
            // 
            // viewInviteCodesToolStripMenuItem
            // 
            this.viewInviteCodesToolStripMenuItem.Name = "viewInviteCodesToolStripMenuItem";
            this.viewInviteCodesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewInviteCodesToolStripMenuItem.Text = "View Invite Codes";
            // 
            // pnlMessages
            // 
            this.pnlMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMessages.AutoScroll = true;
            this.pnlMessages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMessages.Controls.Add(this.tblMessages);
            this.pnlMessages.Location = new System.Drawing.Point(269, 27);
            this.pnlMessages.Name = "pnlMessages";
            this.pnlMessages.Size = new System.Drawing.Size(793, 518);
            this.pnlMessages.TabIndex = 7;
            // 
            // frmChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 577);
            this.Controls.Add(this.pnlMessages);
            this.Controls.Add(this.btnEditor);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessageText);
            this.Controls.Add(this.pnlGuilds);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "frmChat";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.frmChat_Load);
            this.pnlGuilds.ResumeLayout(false);
            this.pnlGuilds.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlMessages.ResumeLayout(false);
            this.pnlMessages.PerformLayout();
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
        private Button btnEditor;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem accountOptionsToolStripMenuItem;
        private ToolStripMenuItem createGuildToolStripMenuItem;
        private ToolStripMenuItem joinGuildFromCodeToolStripMenuItem;
        private ToolStripMenuItem addAccountToolStripMenuItem;
        private ToolStripMenuItem privateChatsToolStripMenuItem;
        private Panel pnlMessages;
        private ToolStripMenuItem guildSettingsToolStripMenuItem;
        private ToolStripMenuItem createDMToolStripMenuItem;
        private ToolStripMenuItem listUsersToolStripMenuItem;
        private ToolStripMenuItem invitesToolStripMenuItem;
        private ToolStripMenuItem createInviteToolStripMenuItem;
        private ToolStripMenuItem viewInviteCodesToolStripMenuItem;
    }
}