namespace NeaClient
{
    partial class frmGuildUsers
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
            this.btnKick = new System.Windows.Forms.Button();
            this.btnTogglePerms = new System.Windows.Forms.Button();
            this.tblUsers = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnKick
            // 
            this.btnKick.Location = new System.Drawing.Point(97, 213);
            this.btnKick.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size(81, 31);
            this.btnKick.TabIndex = 1;
            this.btnKick.Text = "Kick";
            this.btnKick.UseVisualStyleBackColor = true;
            this.btnKick.Click += new System.EventHandler(this.btnKick_Click);
            // 
            // btnTogglePerms
            // 
            this.btnTogglePerms.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTogglePerms.Location = new System.Drawing.Point(6, 213);
            this.btnTogglePerms.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnTogglePerms.Name = "btnTogglePerms";
            this.btnTogglePerms.Size = new System.Drawing.Size(87, 31);
            this.btnTogglePerms.TabIndex = 1;
            this.btnTogglePerms.Text = "Toggle perms";
            this.btnTogglePerms.UseVisualStyleBackColor = true;
            this.btnTogglePerms.Click += new System.EventHandler(this.btnTogglePerms_Click);
            // 
            // tblUsers
            // 
            this.tblUsers.AutoScroll = true;
            this.tblUsers.AutoSize = true;
            this.tblUsers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblUsers.ColumnCount = 2;
            this.tblUsers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblUsers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblUsers.Location = new System.Drawing.Point(0, 0);
            this.tblUsers.Name = "tblUsers";
            this.tblUsers.RowCount = 1;
            this.tblUsers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblUsers.Size = new System.Drawing.Size(172, 0);
            this.tblUsers.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tblUsers);
            this.panel1.Location = new System.Drawing.Point(6, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 206);
            this.panel1.TabIndex = 3;
            // 
            // frmGuildUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 251);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnTogglePerms);
            this.Controls.Add(this.btnKick);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGuildUsers";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Users";
            this.Load += new System.EventHandler(this.frmInvites_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Button btnKick;
        private Button btnTogglePerms;
        private TableLayoutPanel tblUsers;
        private Panel panel1;
    }
}