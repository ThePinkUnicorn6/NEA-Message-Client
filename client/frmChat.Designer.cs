namespace NeaClient
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
            this.components = new System.ComponentModel.Container();
            this.pnlGuilds = new System.Windows.Forms.Panel();
            this.tvGuilds = new System.Windows.Forms.TreeView();
            this.txtMessageText = new System.Windows.Forms.TextBox();
            this.tblMessages = new System.Windows.Forms.TableLayoutPanel();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnEditor = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadProfilePicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPrivateKeyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createGuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinGuildFromCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leaveGuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guildSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invitesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editGuildStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offlineIndicator = new System.Windows.Forms.ToolStripMenuItem();
            this.channelSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.newChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeChannelPermissions = new System.Windows.Forms.ToolStripMenuItem();
            this.ownerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anyoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlMessages = new System.Windows.Forms.Panel();
            this.txtGuildMessage = new System.Windows.Forms.TextBox();
            this.txtKeyWarning = new System.Windows.Forms.TextBox();
            this.tmrFulfillGuildRequests = new System.Windows.Forms.Timer(this.components);
            this.tmrMessageCheck = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnViewOlderMessages = new System.Windows.Forms.Button();
            this.btnViewNewerMessages = new System.Windows.Forms.Button();
            this.btnJumpToPresent = new System.Windows.Forms.Button();
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
            this.pnlGuilds.Location = new System.Drawing.Point(22, 58);
            this.pnlGuilds.Margin = new System.Windows.Forms.Padding(6);
            this.pnlGuilds.Name = "pnlGuilds";
            this.pnlGuilds.Size = new System.Drawing.Size(470, 1370);
            this.pnlGuilds.TabIndex = 0;
            // 
            // tvGuilds
            // 
            this.tvGuilds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvGuilds.Location = new System.Drawing.Point(6, 6);
            this.tvGuilds.Margin = new System.Windows.Forms.Padding(6);
            this.tvGuilds.Name = "tvGuilds";
            this.tvGuilds.ShowPlusMinus = false;
            this.tvGuilds.Size = new System.Drawing.Size(455, 1154);
            this.tvGuilds.TabIndex = 1;
            this.tvGuilds.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvGuilds_NodeMouseClick);
            // 
            // txtMessageText
            // 
            this.txtMessageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessageText.Location = new System.Drawing.Point(565, 1175);
            this.txtMessageText.Margin = new System.Windows.Forms.Padding(6);
            this.txtMessageText.Multiline = true;
            this.txtMessageText.Name = "txtMessageText";
            this.txtMessageText.Size = new System.Drawing.Size(1254, 45);
            this.txtMessageText.TabIndex = 1;
            this.txtMessageText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessageText_KeyDown);
            // 
            // tblMessages
            // 
            this.tblMessages.AutoSize = true;
            this.tblMessages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblMessages.ColumnCount = 2;
            this.tblMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tblMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblMessages.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMessages.Location = new System.Drawing.Point(0, 0);
            this.tblMessages.Margin = new System.Windows.Forms.Padding(6);
            this.tblMessages.Name = "tblMessages";
            this.tblMessages.RowCount = 1;
            this.tblMessages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMessages.Size = new System.Drawing.Size(1473, 0);
            this.tblMessages.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(1833, 1173);
            this.btnSend.Margin = new System.Windows.Forms.Padding(6);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(139, 49);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnEditor
            // 
            this.btnEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditor.Location = new System.Drawing.Point(503, 1175);
            this.btnEditor.Margin = new System.Windows.Forms.Padding(6);
            this.btnEditor.Name = "btnEditor";
            this.btnEditor.Size = new System.Drawing.Size(50, 49);
            this.btnEditor.TabIndex = 5;
            this.btnEditor.Text = "+";
            this.btnEditor.UseVisualStyleBackColor = true;
            this.btnEditor.Click += new System.EventHandler(this.btnEditor_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.testToolStripMenuItem,
            this.guildSettingsToolStripMenuItem,
            this.offlineIndicator,
            this.channelSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(11, 4, 0, 4);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1995, 44);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountOptionsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(120, 36);
            this.fileToolStripMenuItem.Text = "Settings";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // accountOptionsToolStripMenuItem
            // 
            this.accountOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadProfilePicToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.copyPrivateKeyToClipboardToolStripMenuItem});
            this.accountOptionsToolStripMenuItem.Name = "accountOptionsToolStripMenuItem";
            this.accountOptionsToolStripMenuItem.Size = new System.Drawing.Size(325, 44);
            this.accountOptionsToolStripMenuItem.Text = "Account Options";
            // 
            // uploadProfilePicToolStripMenuItem
            // 
            this.uploadProfilePicToolStripMenuItem.Name = "uploadProfilePicToolStripMenuItem";
            this.uploadProfilePicToolStripMenuItem.Size = new System.Drawing.Size(466, 44);
            this.uploadProfilePicToolStripMenuItem.Text = "Upload ProfilePic";
            this.uploadProfilePicToolStripMenuItem.Click += new System.EventHandler(this.uploadProfilePicToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(466, 44);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // copyPrivateKeyToClipboardToolStripMenuItem
            // 
            this.copyPrivateKeyToClipboardToolStripMenuItem.Name = "copyPrivateKeyToClipboardToolStripMenuItem";
            this.copyPrivateKeyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(466, 44);
            this.copyPrivateKeyToClipboardToolStripMenuItem.Text = "Copy Private Key to Clipboard";
            this.copyPrivateKeyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyPrivateKeyToClipboardToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createGuildToolStripMenuItem,
            this.joinGuildFromCodeToolStripMenuItem,
            this.leaveGuildToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(100, 36);
            this.testToolStripMenuItem.Text = "Guilds";
            // 
            // createGuildToolStripMenuItem
            // 
            this.createGuildToolStripMenuItem.Name = "createGuildToolStripMenuItem";
            this.createGuildToolStripMenuItem.Size = new System.Drawing.Size(279, 44);
            this.createGuildToolStripMenuItem.Text = "Create Guild";
            this.createGuildToolStripMenuItem.Click += new System.EventHandler(this.createGuildToolStripMenuItem_Click);
            // 
            // joinGuildFromCodeToolStripMenuItem
            // 
            this.joinGuildFromCodeToolStripMenuItem.Name = "joinGuildFromCodeToolStripMenuItem";
            this.joinGuildFromCodeToolStripMenuItem.Size = new System.Drawing.Size(279, 44);
            this.joinGuildFromCodeToolStripMenuItem.Text = "Join Guild";
            this.joinGuildFromCodeToolStripMenuItem.Click += new System.EventHandler(this.joinGuildFromCodeToolStripMenuItem_Click);
            // 
            // leaveGuildToolStripMenuItem
            // 
            this.leaveGuildToolStripMenuItem.Name = "leaveGuildToolStripMenuItem";
            this.leaveGuildToolStripMenuItem.Size = new System.Drawing.Size(279, 44);
            this.leaveGuildToolStripMenuItem.Text = "Leave Guild";
            this.leaveGuildToolStripMenuItem.Click += new System.EventHandler(this.leaveGuildToolStripMenuItem_Click);
            // 
            // guildSettingsToolStripMenuItem
            // 
            this.guildSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listUsersToolStripMenuItem,
            this.invitesToolStripMenuItem,
            this.editGuildStripMenuItem,
            this.deleteGuildToolStripMenuItem,
            this.viewDescriptionToolStripMenuItem});
            this.guildSettingsToolStripMenuItem.Name = "guildSettingsToolStripMenuItem";
            this.guildSettingsToolStripMenuItem.Size = new System.Drawing.Size(183, 36);
            this.guildSettingsToolStripMenuItem.Text = "Guild Settings";
            // 
            // listUsersToolStripMenuItem
            // 
            this.listUsersToolStripMenuItem.Name = "listUsersToolStripMenuItem";
            this.listUsersToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.listUsersToolStripMenuItem.Text = "List Users";
            this.listUsersToolStripMenuItem.Click += new System.EventHandler(this.listUsersToolStripMenuItem_Click);
            // 
            // invitesToolStripMenuItem
            // 
            this.invitesToolStripMenuItem.Name = "invitesToolStripMenuItem";
            this.invitesToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.invitesToolStripMenuItem.Text = "Manage Invites";
            this.invitesToolStripMenuItem.Click += new System.EventHandler(this.invitesToolStripMenuItem_Click);
            // 
            // editGuildStripMenuItem
            // 
            this.editGuildStripMenuItem.Name = "editGuildStripMenuItem";
            this.editGuildStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.editGuildStripMenuItem.Text = "Edit Guild";
            this.editGuildStripMenuItem.Click += new System.EventHandler(this.editGuildStripMenuItem_Click);
            // 
            // deleteGuildToolStripMenuItem
            // 
            this.deleteGuildToolStripMenuItem.Name = "deleteGuildToolStripMenuItem";
            this.deleteGuildToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.deleteGuildToolStripMenuItem.Text = "Delete Guild";
            this.deleteGuildToolStripMenuItem.Click += new System.EventHandler(this.deleteGuildToolStripMenuItem_Click);
            // 
            // viewDescriptionToolStripMenuItem
            // 
            this.viewDescriptionToolStripMenuItem.Name = "viewDescriptionToolStripMenuItem";
            this.viewDescriptionToolStripMenuItem.Size = new System.Drawing.Size(326, 44);
            this.viewDescriptionToolStripMenuItem.Text = "View Description";
            this.viewDescriptionToolStripMenuItem.Click += new System.EventHandler(this.viewDescriptionToolStripMenuItem_Click);
            // 
            // offlineIndicator
            // 
            this.offlineIndicator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.offlineIndicator.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.offlineIndicator.ForeColor = System.Drawing.Color.IndianRed;
            this.offlineIndicator.Name = "offlineIndicator";
            this.offlineIndicator.Size = new System.Drawing.Size(139, 40);
            this.offlineIndicator.Text = "OFFLINE";
            this.offlineIndicator.Visible = false;
            // 
            // channelSettings
            // 
            this.channelSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newChannelToolStripMenuItem,
            this.renameChannelToolStripMenuItem,
            this.deleteChannelToolStripMenuItem,
            this.changeChannelPermissions});
            this.channelSettings.Name = "channelSettings";
            this.channelSettings.Size = new System.Drawing.Size(215, 36);
            this.channelSettings.Text = "Channel Settings";
            // 
            // newChannelToolStripMenuItem
            // 
            this.newChannelToolStripMenuItem.Name = "newChannelToolStripMenuItem";
            this.newChannelToolStripMenuItem.Size = new System.Drawing.Size(467, 44);
            this.newChannelToolStripMenuItem.Text = "New Channel";
            this.newChannelToolStripMenuItem.Click += new System.EventHandler(this.newChannelToolStripMenuItem_Click);
            // 
            // renameChannelToolStripMenuItem
            // 
            this.renameChannelToolStripMenuItem.Name = "renameChannelToolStripMenuItem";
            this.renameChannelToolStripMenuItem.Size = new System.Drawing.Size(467, 44);
            this.renameChannelToolStripMenuItem.Text = "Rename Channel";
            this.renameChannelToolStripMenuItem.Click += new System.EventHandler(this.renameChannelToolStripMenuItem_Click);
            // 
            // deleteChannelToolStripMenuItem
            // 
            this.deleteChannelToolStripMenuItem.Name = "deleteChannelToolStripMenuItem";
            this.deleteChannelToolStripMenuItem.Size = new System.Drawing.Size(467, 44);
            this.deleteChannelToolStripMenuItem.Text = "Delete Channel";
            this.deleteChannelToolStripMenuItem.Click += new System.EventHandler(this.deleteChannelToolStripMenuItem_Click);
            // 
            // changeChannelPermissions
            // 
            this.changeChannelPermissions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ownerToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.anyoneToolStripMenuItem});
            this.changeChannelPermissions.Name = "changeChannelPermissions";
            this.changeChannelPermissions.Size = new System.Drawing.Size(467, 44);
            this.changeChannelPermissions.Text = "Channel Message Permissions";
            // 
            // ownerToolStripMenuItem
            // 
            this.ownerToolStripMenuItem.Name = "ownerToolStripMenuItem";
            this.ownerToolStripMenuItem.Size = new System.Drawing.Size(229, 44);
            this.ownerToolStripMenuItem.Text = "Owner";
            this.ownerToolStripMenuItem.Click += new System.EventHandler(this.ownerToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(229, 44);
            this.adminToolStripMenuItem.Text = "Admin";
            this.adminToolStripMenuItem.Click += new System.EventHandler(this.adminToolStripMenuItem_Click);
            // 
            // anyoneToolStripMenuItem
            // 
            this.anyoneToolStripMenuItem.Name = "anyoneToolStripMenuItem";
            this.anyoneToolStripMenuItem.Size = new System.Drawing.Size(229, 44);
            this.anyoneToolStripMenuItem.Text = "Anyone";
            this.anyoneToolStripMenuItem.Click += new System.EventHandler(this.anyoneToolStripMenuItem_Click);
            // 
            // pnlMessages
            // 
            this.pnlMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMessages.AutoScroll = true;
            this.pnlMessages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMessages.Controls.Add(this.txtGuildMessage);
            this.pnlMessages.Controls.Add(this.txtKeyWarning);
            this.pnlMessages.Controls.Add(this.tblMessages);
            this.pnlMessages.Location = new System.Drawing.Point(500, 126);
            this.pnlMessages.Margin = new System.Windows.Forms.Padding(6);
            this.pnlMessages.Name = "pnlMessages";
            this.pnlMessages.Size = new System.Drawing.Size(1473, 1037);
            this.pnlMessages.TabIndex = 7;
            // 
            // txtGuildMessage
            // 
            this.txtGuildMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuildMessage.BackColor = System.Drawing.SystemColors.Info;
            this.txtGuildMessage.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGuildMessage.Location = new System.Drawing.Point(507, 343);
            this.txtGuildMessage.Margin = new System.Windows.Forms.Padding(6);
            this.txtGuildMessage.Multiline = true;
            this.txtGuildMessage.Name = "txtGuildMessage";
            this.txtGuildMessage.Size = new System.Drawing.Size(511, 335);
            this.txtGuildMessage.TabIndex = 4;
            this.txtGuildMessage.Text = "You are not in any guilds, try joining one by opening the guilds menu and enterin" +
    "g an invite code.";
            this.txtGuildMessage.Visible = false;
            // 
            // txtKeyWarning
            // 
            this.txtKeyWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeyWarning.BackColor = System.Drawing.SystemColors.Info;
            this.txtKeyWarning.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtKeyWarning.Location = new System.Drawing.Point(507, 343);
            this.txtKeyWarning.Margin = new System.Windows.Forms.Padding(6);
            this.txtKeyWarning.Multiline = true;
            this.txtKeyWarning.Name = "txtKeyWarning";
            this.txtKeyWarning.Size = new System.Drawing.Size(511, 335);
            this.txtKeyWarning.TabIndex = 3;
            this.txtKeyWarning.Text = "You do not have the keys required to read messages in this guild. Please come bac" +
    "k later once another user has shared theirs.";
            this.txtKeyWarning.Visible = false;
            // 
            // tmrFulfillGuildRequests
            // 
            this.tmrFulfillGuildRequests.Enabled = true;
            this.tmrFulfillGuildRequests.Interval = 120000;
            this.tmrFulfillGuildRequests.Tick += new System.EventHandler(this.tmrFulfillGuildRequests_Tick);
            // 
            // tmrMessageCheck
            // 
            this.tmrMessageCheck.Enabled = true;
            this.tmrMessageCheck.Interval = 1000;
            this.tmrMessageCheck.Tick += new System.EventHandler(this.tmrMessageCheck_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnViewOlderMessages
            // 
            this.btnViewOlderMessages.Location = new System.Drawing.Point(500, 64);
            this.btnViewOlderMessages.Margin = new System.Windows.Forms.Padding(6);
            this.btnViewOlderMessages.Name = "btnViewOlderMessages";
            this.btnViewOlderMessages.Size = new System.Drawing.Size(279, 49);
            this.btnViewOlderMessages.TabIndex = 8;
            this.btnViewOlderMessages.Text = "<- View Older Messages";
            this.btnViewOlderMessages.UseVisualStyleBackColor = true;
            this.btnViewOlderMessages.Click += new System.EventHandler(this.btnViewOlderMessages_Click);
            // 
            // btnViewNewerMessages
            // 
            this.btnViewNewerMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewNewerMessages.Location = new System.Drawing.Point(1423, 64);
            this.btnViewNewerMessages.Margin = new System.Windows.Forms.Padding(6);
            this.btnViewNewerMessages.Name = "btnViewNewerMessages";
            this.btnViewNewerMessages.Size = new System.Drawing.Size(293, 49);
            this.btnViewNewerMessages.TabIndex = 8;
            this.btnViewNewerMessages.Text = "View Newer Messages ->";
            this.btnViewNewerMessages.UseVisualStyleBackColor = true;
            this.btnViewNewerMessages.Click += new System.EventHandler(this.btnViewNewerMessages_Click);
            // 
            // btnJumpToPresent
            // 
            this.btnJumpToPresent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJumpToPresent.Location = new System.Drawing.Point(1727, 64);
            this.btnJumpToPresent.Margin = new System.Windows.Forms.Padding(6);
            this.btnJumpToPresent.Name = "btnJumpToPresent";
            this.btnJumpToPresent.Size = new System.Drawing.Size(245, 49);
            this.btnJumpToPresent.TabIndex = 8;
            this.btnJumpToPresent.Text = "Jump To Present >>";
            this.btnJumpToPresent.UseVisualStyleBackColor = true;
            this.btnJumpToPresent.Click += new System.EventHandler(this.btnJumpToPresent_Click);
            // 
            // frmChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1995, 1231);
            this.Controls.Add(this.btnJumpToPresent);
            this.Controls.Add(this.btnViewNewerMessages);
            this.Controls.Add(this.btnViewOlderMessages);
            this.Controls.Add(this.pnlMessages);
            this.Controls.Add(this.btnEditor);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessageText);
            this.Controls.Add(this.pnlGuilds);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(1623, 877);
            this.Name = "frmChat";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.frmChat_Load);
            this.pnlGuilds.ResumeLayout(false);
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
        private Panel pnlMessages;
        private ToolStripMenuItem guildSettingsToolStripMenuItem;
        private ToolStripMenuItem listUsersToolStripMenuItem;
        private ToolStripMenuItem invitesToolStripMenuItem;
        private ToolStripMenuItem createInviteToolStripMenuItem;
        private ToolStripMenuItem viewInviteCodesToolStripMenuItem;
        private TextBox txtKeyWarning;
        private System.Windows.Forms.Timer tmrFulfillGuildRequests;
        private System.Windows.Forms.Timer tmrMessageCheck;
        private ToolStripMenuItem offlineIndicator;
        private OpenFileDialog openFileDialog1;
        private ToolStripMenuItem editGuildToolStripMenuItem;
        private ToolStripMenuItem deleteGuildToolStripMenuItem;
        private ToolStripMenuItem editGuildStripMenuItem;
        private ToolStripMenuItem viewDescriptionToolStripMenuItem;
        private ToolStripMenuItem channelSettings;
        private ToolStripMenuItem newChannelToolStripMenuItem;
        private ToolStripMenuItem renameChannelToolStripMenuItem;
        private ToolStripMenuItem deleteChannelToolStripMenuItem;
        private ToolStripMenuItem changeChannelPermissions;
        private ToolStripMenuItem ownerToolStripMenuItem;
        private ToolStripMenuItem adminToolStripMenuItem;
        private ToolStripMenuItem anyoneToolStripMenuItem;
        private Button btnViewOlderMessages;
        private Button btnViewNewerMessages;
        private Button btnJumpToPresent;
        private ToolStripMenuItem uploadProfilePicToolStripMenuItem;
        private TextBox txtGuildMessage;
        private ToolStripMenuItem leaveGuildToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private ToolStripMenuItem copyPrivateKeyToClipboardToolStripMenuItem;
    }
}