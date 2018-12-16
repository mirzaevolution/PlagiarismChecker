namespace Plagiarism.ServiceRunner.Installer
{
    partial class FormServiceRunner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServiceRunner));
            this.ButtonInstall = new System.Windows.Forms.Button();
            this.ButtonUninstall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonInstall
            // 
            this.ButtonInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonInstall.Location = new System.Drawing.Point(78, 29);
            this.ButtonInstall.Name = "ButtonInstall";
            this.ButtonInstall.Size = new System.Drawing.Size(124, 43);
            this.ButtonInstall.TabIndex = 0;
            this.ButtonInstall.Text = "Install";
            this.ButtonInstall.UseVisualStyleBackColor = true;
            this.ButtonInstall.Click += new System.EventHandler(this.ButtonInstall_Click);
            // 
            // ButtonUninstall
            // 
            this.ButtonUninstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonUninstall.Location = new System.Drawing.Point(78, 95);
            this.ButtonUninstall.Name = "ButtonUninstall";
            this.ButtonUninstall.Size = new System.Drawing.Size(124, 43);
            this.ButtonUninstall.TabIndex = 1;
            this.ButtonUninstall.Text = "Uninstall";
            this.ButtonUninstall.UseVisualStyleBackColor = true;
            this.ButtonUninstall.Click += new System.EventHandler(this.ButtonUninstall_Click);
            // 
            // FormServiceRunner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 175);
            this.Controls.Add(this.ButtonUninstall);
            this.Controls.Add(this.ButtonInstall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormServiceRunner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Runner";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonInstall;
        private System.Windows.Forms.Button ButtonUninstall;
    }
}

