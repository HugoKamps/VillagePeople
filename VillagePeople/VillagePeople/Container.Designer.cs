﻿using System.ComponentModel;
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
            this.SuspendLayout();
            // 
            // GamePanel
            // 
            this.GamePanel.BackColor = System.Drawing.Color.White;
            this.GamePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GamePanel.Location = new System.Drawing.Point(12, 22);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(1096, 605);
            this.GamePanel.TabIndex = 0;
            this.GamePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GamePanel_Paint);
            this.GamePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GamePanel_MouseClick);
            // 
            // lblVelocity
            // 
            this.lblVelocity.AutoSize = true;
            this.lblVelocity.Location = new System.Drawing.Point(1, -1);
            this.lblVelocity.Name = "lblVelocity";
            this.lblVelocity.Size = new System.Drawing.Size(28, 13);
            this.lblVelocity.TabIndex = 2;
            this.lblVelocity.Text = "(0,0)";
            // 
            // cbUpdate
            // 
            this.cbUpdate.AutoSize = true;
            this.cbUpdate.Location = new System.Drawing.Point(35, -1);
            this.cbUpdate.Name = "cbUpdate";
            this.cbUpdate.Size = new System.Drawing.Size(86, 17);
            this.cbUpdate.TabIndex = 1;
            this.cbUpdate.Text = "Auto Update";
            this.cbUpdate.UseVisualStyleBackColor = true;
            this.cbUpdate.CheckedChanged += new System.EventHandler(this.cbUpdate_CheckedChanged);
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.Location = new System.Drawing.Point(117, -1);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(79, 17);
            this.cbDebug.TabIndex = 0;
            this.cbDebug.Text = "Debug Info";
            this.cbDebug.UseVisualStyleBackColor = true;
            this.cbDebug.CheckedChanged += new System.EventHandler(this.cbDebug_CheckedChanged);
            // 
            // resourcesLabel
            // 
            this.resourcesLabel.AutoSize = true;
            this.resourcesLabel.Location = new System.Drawing.Point(421, 3);
            this.resourcesLabel.Name = "resourcesLabel";
            this.resourcesLabel.Size = new System.Drawing.Size(58, 13);
            this.resourcesLabel.TabIndex = 3;
            this.resourcesLabel.Text = "Resources";
            // 
            // cbDebugText
            // 
            this.cbDebugText.AutoSize = true;
            this.cbDebugText.Location = new System.Drawing.Point(191, -1);
            this.cbDebugText.Name = "cbDebugText";
            this.cbDebugText.Size = new System.Drawing.Size(78, 17);
            this.cbDebugText.TabIndex = 4;
            this.cbDebugText.Text = "Debug text";
            this.cbDebugText.UseVisualStyleBackColor = true;
            this.cbDebugText.CheckedChanged += new System.EventHandler(this.cbDebugText_CheckedChanged);
            // 
            // Container
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 639);
            this.Controls.Add(this.cbDebugText);
            this.Controls.Add(this.resourcesLabel);
            this.Controls.Add(this.cbDebug);
            this.Controls.Add(this.cbUpdate);
            this.Controls.Add(this.lblVelocity);
            this.Controls.Add(this.GamePanel);
            this.DoubleBuffered = true;
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
    }
}

