namespace FocusChanged.view
{
    partial class FormEditBanFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditBanFile));
            this.listBanProcess = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBanProcess
            // 
            this.listBanProcess.FormattingEnabled = true;
            this.listBanProcess.Location = new System.Drawing.Point(4, 3);
            this.listBanProcess.Name = "listBanProcess";
            this.listBanProcess.Size = new System.Drawing.Size(553, 238);
            this.listBanProcess.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(482, 243);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Remove";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnRemoveBan_Click);
            // 
            // FormEditBanFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 278);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBanProcess);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEditBanFile";
            this.Text = "FormEditBanFile";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBanProcess;
        private System.Windows.Forms.Button button1;
    }
}