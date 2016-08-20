namespace medic {
    partial class AboutForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.logoPanel = new System.Windows.Forms.Panel();
            this.tlpInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblDeveloperLabel = new System.Windows.Forms.Label();
            this.lblContactsLabel = new System.Windows.Forms.Label();
            this.lblContactsValue = new System.Windows.Forms.Label();
            this.toolsPanel = new System.Windows.Forms.Panel();
            this.toolsAligner = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.lblVersionLabel = new System.Windows.Forms.Label();
            this.lblVersionValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDeveloperValue = new System.Windows.Forms.LinkLabel();
            this.tlpInfo.SuspendLayout();
            this.toolsPanel.SuspendLayout();
            this.toolsAligner.SuspendLayout();
            this.SuspendLayout();
            // 
            // logoPanel
            // 
            this.logoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.logoPanel.BackgroundImage = global::medic.Properties.Resources.Logo;
            this.logoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoPanel.Location = new System.Drawing.Point(0, 0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(424, 105);
            this.logoPanel.TabIndex = 0;
            // 
            // tlpInfo
            // 
            this.tlpInfo.ColumnCount = 2;
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpInfo.Controls.Add(this.lblDeveloperValue, 1, 0);
            this.tlpInfo.Controls.Add(this.lblDeveloperLabel, 0, 0);
            this.tlpInfo.Controls.Add(this.lblContactsLabel, 0, 1);
            this.tlpInfo.Controls.Add(this.lblContactsValue, 1, 1);
            this.tlpInfo.Controls.Add(this.lblVersionLabel, 0, 2);
            this.tlpInfo.Controls.Add(this.lblVersionValue, 1, 2);
            this.tlpInfo.Location = new System.Drawing.Point(90, 130);
            this.tlpInfo.Name = "tlpInfo";
            this.tlpInfo.RowCount = 3;
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpInfo.Size = new System.Drawing.Size(245, 60);
            this.tlpInfo.TabIndex = 1;
            // 
            // lblDeveloperLabel
            // 
            this.lblDeveloperLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDeveloperLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDeveloperLabel.Location = new System.Drawing.Point(0, 0);
            this.lblDeveloperLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lblDeveloperLabel.Name = "lblDeveloperLabel";
            this.lblDeveloperLabel.Size = new System.Drawing.Size(122, 20);
            this.lblDeveloperLabel.TabIndex = 0;
            this.lblDeveloperLabel.Text = "Разработчик";
            this.lblDeveloperLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContactsLabel
            // 
            this.lblContactsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContactsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblContactsLabel.Location = new System.Drawing.Point(0, 20);
            this.lblContactsLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lblContactsLabel.Name = "lblContactsLabel";
            this.lblContactsLabel.Size = new System.Drawing.Size(122, 20);
            this.lblContactsLabel.TabIndex = 2;
            this.lblContactsLabel.Text = "E-mail";
            this.lblContactsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContactsValue
            // 
            this.lblContactsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContactsValue.Location = new System.Drawing.Point(122, 20);
            this.lblContactsValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblContactsValue.Name = "lblContactsValue";
            this.lblContactsValue.Size = new System.Drawing.Size(123, 20);
            this.lblContactsValue.TabIndex = 3;
            this.lblContactsValue.Text = "atomindex@gmail.com";
            this.lblContactsValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolsPanel
            // 
            this.toolsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.toolsPanel.Controls.Add(this.label1);
            this.toolsPanel.Controls.Add(this.toolsAligner);
            this.toolsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolsPanel.Location = new System.Drawing.Point(0, 215);
            this.toolsPanel.Name = "toolsPanel";
            this.toolsPanel.Size = new System.Drawing.Size(424, 50);
            this.toolsPanel.TabIndex = 2;
            // 
            // toolsAligner
            // 
            this.toolsAligner.Controls.Add(this.closeButton);
            this.toolsAligner.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolsAligner.Location = new System.Drawing.Point(334, 0);
            this.toolsAligner.Name = "toolsAligner";
            this.toolsAligner.Size = new System.Drawing.Size(90, 50);
            this.toolsAligner.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(0, 13);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Закрыть";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // lblVersionLabel
            // 
            this.lblVersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVersionLabel.Location = new System.Drawing.Point(0, 40);
            this.lblVersionLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lblVersionLabel.Name = "lblVersionLabel";
            this.lblVersionLabel.Size = new System.Drawing.Size(122, 20);
            this.lblVersionLabel.TabIndex = 4;
            this.lblVersionLabel.Text = "Версия";
            this.lblVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersionValue
            // 
            this.lblVersionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVersionValue.Location = new System.Drawing.Point(122, 40);
            this.lblVersionValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblVersionValue.Name = "lblVersionValue";
            this.lblVersionValue.Size = new System.Drawing.Size(123, 20);
            this.lblVersionValue.TabIndex = 5;
            this.lblVersionValue.Text = "1.0.0";
            this.lblVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "© Eugene Egorov, 2016";
            // 
            // lblDeveloperValue
            // 
            this.lblDeveloperValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDeveloperValue.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblDeveloperValue.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.lblDeveloperValue.Location = new System.Drawing.Point(122, 0);
            this.lblDeveloperValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblDeveloperValue.Name = "lblDeveloperValue";
            this.lblDeveloperValue.Size = new System.Drawing.Size(123, 20);
            this.lblDeveloperValue.TabIndex = 3;
            this.lblDeveloperValue.TabStop = true;
            this.lblDeveloperValue.Text = "Егоров Евгений";
            this.lblDeveloperValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDeveloperValue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDeveloperValue_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(424, 265);
            this.Controls.Add(this.toolsPanel);
            this.Controls.Add(this.tlpInfo);
            this.Controls.Add(this.logoPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "О программе";
            this.tlpInfo.ResumeLayout(false);
            this.toolsPanel.ResumeLayout(false);
            this.toolsPanel.PerformLayout();
            this.toolsAligner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.TableLayoutPanel tlpInfo;
        private System.Windows.Forms.Label lblDeveloperLabel;
        private System.Windows.Forms.Label lblContactsLabel;
        private System.Windows.Forms.Label lblContactsValue;
        protected System.Windows.Forms.Panel toolsPanel;
        private System.Windows.Forms.Panel toolsAligner;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label lblVersionLabel;
        private System.Windows.Forms.Label lblVersionValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lblDeveloperValue;
    }
}