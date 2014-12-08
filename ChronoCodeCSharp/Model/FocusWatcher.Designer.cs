namespace FocusChanged.Model
{
    partial class FocusWatcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FocusWatcher));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Files = new System.Windows.Forms.ToolStripDropDownButton();
            this.Options = new System.Windows.Forms.ToolStripDropDownButton();
            this.Action = new System.Windows.Forms.ToolStripDropDownButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.statusStrip1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(635, 267);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Files,
            this.Options,
            this.Action});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(177, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Files
            // 
            this.Files.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Files.Image = ((System.Drawing.Image)(resources.GetObject("Files.Image")));
            this.Files.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Files.Name = "Files";
            this.Files.Size = new System.Drawing.Size(43, 20);
            this.Files.Text = "Files";
            this.Files.ToolTipText = "Files";
            this.Files.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // Options
            // 
            this.Options.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Options.Image = ((System.Drawing.Image)(resources.GetObject("Options.Image")));
            this.Options.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Options.Name = "Options";
            this.Options.Size = new System.Drawing.Size(62, 20);
            this.Options.Text = "Options";
            // 
            // Action
            // 
            this.Action.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Action.Image = ((System.Drawing.Image)(resources.GetObject("Action.Image")));
            this.Action.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Action.Name = "Action";
            this.Action.Size = new System.Drawing.Size(55, 20);
            this.Action.Text = "Action";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 267);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Files;
        private System.Windows.Forms.ToolStripDropDownButton Options;
        private System.Windows.Forms.ToolStripDropDownButton Action;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
    }
}