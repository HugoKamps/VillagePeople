using System.ComponentModel;
using System.Windows.Forms;

namespace VillagePeople {
    partial class Container {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.lblVelocity = new System.Windows.Forms.Label();
            this.cbUpdate = new System.Windows.Forms.CheckBox();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            this.resourcesLabel = new System.Windows.Forms.Label();
            this.cbDebugText = new System.Windows.Forms.CheckBox();
            this.addSheepButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GamePanel
            // 
            this.GamePanel.BackColor = System.Drawing.Color.White;
            this.GamePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GamePanel.Location = new System.Drawing.Point(0, 36);
            this.GamePanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(1067, 738);
            this.GamePanel.TabIndex = 0;
            this.GamePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GamePanel_Paint);
            this.GamePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GamePanel_MouseClick);
            // 
            // lblVelocity
            // 
            this.lblVelocity.AutoSize = true;
            this.lblVelocity.Location = new System.Drawing.Point(1013, 11);
            this.lblVelocity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVelocity.Name = "lblVelocity";
            this.lblVelocity.Size = new System.Drawing.Size(38, 17);
            this.lblVelocity.TabIndex = 2;
            this.lblVelocity.Text = "(0,0)";
            // 
            // cbUpdate
            // 
            this.cbUpdate.AutoSize = true;
            this.cbUpdate.Location = new System.Drawing.Point(85, 7);
            this.cbUpdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUpdate.Name = "cbUpdate";
            this.cbUpdate.Size = new System.Drawing.Size(109, 21);
            this.cbUpdate.TabIndex = 1;
            this.cbUpdate.Text = "Auto Update";
            this.cbUpdate.UseVisualStyleBackColor = true;
            this.cbUpdate.CheckedChanged += new System.EventHandler(this.cbUpdate_CheckedChanged);
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.Location = new System.Drawing.Point(208, 7);
            this.cbDebug.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(99, 21);
            this.cbDebug.TabIndex = 0;
            this.cbDebug.Text = "Debug Info";
            this.cbDebug.UseVisualStyleBackColor = true;
            this.cbDebug.CheckedChanged += new System.EventHandler(this.cbDebug_CheckedChanged);
            // 
            // resourcesLabel
            // 
            this.resourcesLabel.AutoSize = true;
            this.resourcesLabel.Location = new System.Drawing.Point(481, 9);
            this.resourcesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.resourcesLabel.Name = "resourcesLabel";
            this.resourcesLabel.Size = new System.Drawing.Size(76, 17);
            this.resourcesLabel.TabIndex = 3;
            this.resourcesLabel.Text = "Resources";
            // 
            // cbDebugText
            // 
            this.cbDebugText.AutoSize = true;
            this.cbDebugText.Location = new System.Drawing.Point(328, 7);
            this.cbDebugText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDebugText.Name = "cbDebugText";
            this.cbDebugText.Size = new System.Drawing.Size(98, 21);
            this.cbDebugText.TabIndex = 4;
            this.cbDebugText.Text = "Debug text";
            this.cbDebugText.UseVisualStyleBackColor = true;
            this.cbDebugText.CheckedChanged += new System.EventHandler(this.cbDebugText_CheckedChanged);
            // 
            // addSheepButton
            // 
            this.addSheepButton.Location = new System.Drawing.Point(822, 8);
            this.addSheepButton.Name = "addSheepButton";
            this.addSheepButton.Size = new System.Drawing.Size(150, 23);
            this.addSheepButton.TabIndex = 5;
            this.addSheepButton.Text = "Add sheep";
            this.addSheepButton.UseVisualStyleBackColor = true;
            this.addSheepButton.Click += new System.EventHandler(this.addSheepButton_Click);
            // 
            // Container
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 774);
            this.Controls.Add(this.addSheepButton);
            this.Controls.Add(this.GamePanel);
            this.Controls.Add(this.lblVelocity);
            this.Controls.Add(this.cbDebugText);
            this.Controls.Add(this.cbUpdate);
            this.Controls.Add(this.cbDebug);
            this.Controls.Add(this.resourcesLabel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Container";
            this.Text = "Village People";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel GamePanel;
        private CheckBox cbDebug;
        private CheckBox cbUpdate;
        private Label lblVelocity;
        private Label resourcesLabel;
        private CheckBox cbDebugText;
        private Button addSheepButton;
    }
}

