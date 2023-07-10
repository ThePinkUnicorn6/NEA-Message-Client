namespace NeaClient
{
    partial class frmGuildSettings
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
            this.lblGuildName = new System.Windows.Forms.Label();
            this.txtGuildName = new System.Windows.Forms.TextBox();
            this.txtGuildDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblGuildName
            // 
            this.lblGuildName.AutoSize = true;
            this.lblGuildName.Location = new System.Drawing.Point(12, 9);
            this.lblGuildName.Name = "lblGuildName";
            this.lblGuildName.Size = new System.Drawing.Size(73, 15);
            this.lblGuildName.TabIndex = 0;
            this.lblGuildName.Text = "Guild Name:";
            // 
            // txtGuildName
            // 
            this.txtGuildName.Location = new System.Drawing.Point(12, 27);
            this.txtGuildName.Name = "txtGuildName";
            this.txtGuildName.Size = new System.Drawing.Size(192, 23);
            this.txtGuildName.TabIndex = 1;
            // 
            // txtGuildDescription
            // 
            this.txtGuildDescription.Location = new System.Drawing.Point(13, 71);
            this.txtGuildDescription.Multiline = true;
            this.txtGuildDescription.Name = "txtGuildDescription";
            this.txtGuildDescription.Size = new System.Drawing.Size(191, 100);
            this.txtGuildDescription.TabIndex = 2;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(13, 53);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(101, 15);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Guild Description:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(129, 177);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmGuildSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 217);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtGuildDescription);
            this.Controls.Add(this.txtGuildName);
            this.Controls.Add(this.lblGuildName);
            this.Name = "frmGuildSettings";
            this.Text = "Guild Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblGuildName;
        private TextBox txtGuildName;
        private TextBox txtGuildDescription;
        private Label lblDescription;
        private Button btnSave;
    }
}