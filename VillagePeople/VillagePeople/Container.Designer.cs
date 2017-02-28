namespace VillagePeople {
    partial class Container {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.GamePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // GamePanel
            // 
            this.GamePanel.BackColor = System.Drawing.Color.White;
            this.GamePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GamePanel.Location = new System.Drawing.Point(0, 0);
            this.GamePanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(900, 804);
            this.GamePanel.TabIndex = 0;
            this.GamePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GamePanel_Paint);
            // 
            // Container
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 804);
            this.Controls.Add(this.GamePanel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Container";
            this.Text = "Village People";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel GamePanel;
    }
}

