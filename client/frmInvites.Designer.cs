namespace NeaClient
{
    partial class frmInvites
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.cbInvites = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(97, 213);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(81, 31);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(6, 213);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(81, 31);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Create New";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // cbInvites
            // 
            this.cbInvites.FormattingEnabled = true;
            this.cbInvites.ItemHeight = 15;
            this.cbInvites.Location = new System.Drawing.Point(6, 7);
            this.cbInvites.Name = "cbInvites";
            this.cbInvites.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.cbInvites.Size = new System.Drawing.Size(172, 199);
            this.cbInvites.TabIndex = 2;
            // 
            // frmInvites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 250);
            this.Controls.Add(this.cbInvites);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvites";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "frmInvites";
            this.Load += new System.EventHandler(this.frmInvites_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Button btnDelete;
        private Button btnCreate;
        private ListBox cbInvites;
    }
}