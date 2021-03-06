﻿namespace medic.Forms {
    partial class EntityEditForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityEditForm));
            this.toolsPanel = new System.Windows.Forms.Panel();
            this.toolsAligner = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.toolsPanel.SuspendLayout();
            this.toolsAligner.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolsPanel
            // 
            this.toolsPanel.Controls.Add(this.toolsAligner);
            this.toolsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolsPanel.Location = new System.Drawing.Point(0, 311);
            this.toolsPanel.Name = "toolsPanel";
            this.toolsPanel.Size = new System.Drawing.Size(400, 50);
            this.toolsPanel.TabIndex = 0;
            // 
            // toolsAligner
            // 
            this.toolsAligner.Controls.Add(this.cancelButton);
            this.toolsAligner.Controls.Add(this.saveButton);
            this.toolsAligner.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolsAligner.Location = new System.Drawing.Point(225, 0);
            this.toolsAligner.Name = "toolsAligner";
            this.toolsAligner.Size = new System.Drawing.Size(175, 50);
            this.toolsAligner.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(85, 13);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(0, 13);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // panel
            // 
            this.panel.AutoSize = true;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.MinimumSize = new System.Drawing.Size(400, 30);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 0);
            this.panel.Size = new System.Drawing.Size(400, 311);
            this.panel.TabIndex = 1;
            // 
            // EntityEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(400, 361);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.toolsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EntityEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EntityEditForm";
            this.toolsPanel.ResumeLayout(false);
            this.toolsAligner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel toolsAligner;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel panel;
        protected System.Windows.Forms.Panel toolsPanel;
    }
}