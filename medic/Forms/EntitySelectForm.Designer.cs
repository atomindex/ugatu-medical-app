namespace medic.Forms {
    partial class EntitySelectForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntitySelectForm));
            this.toolsPanel = new System.Windows.Forms.Panel();
            this.toolsAligner = new System.Windows.Forms.Panel();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.listPanel = new System.Windows.Forms.Panel();
            this.list = new System.Windows.Forms.CheckedListBox();
            this.tlsFilter = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.toolsPanel.SuspendLayout();
            this.toolsAligner.SuspendLayout();
            this.listPanel.SuspendLayout();
            this.tlsFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolsPanel
            // 
            this.toolsPanel.Controls.Add(this.toolsAligner);
            this.toolsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolsPanel.Location = new System.Drawing.Point(0, 309);
            this.toolsPanel.Name = "toolsPanel";
            this.toolsPanel.Size = new System.Drawing.Size(468, 50);
            this.toolsPanel.TabIndex = 1;
            // 
            // toolsAligner
            // 
            this.toolsAligner.Controls.Add(this.addButton);
            this.toolsAligner.Controls.Add(this.cancelButton);
            this.toolsAligner.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolsAligner.Location = new System.Drawing.Point(298, 0);
            this.toolsAligner.Name = "toolsAligner";
            this.toolsAligner.Size = new System.Drawing.Size(170, 50);
            this.toolsAligner.TabIndex = 0;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(0, 13);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Добавить";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
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
            // listPanel
            // 
            this.listPanel.Controls.Add(this.list);
            this.listPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPanel.Location = new System.Drawing.Point(0, 37);
            this.listPanel.Name = "listPanel";
            this.listPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 5);
            this.listPanel.Size = new System.Drawing.Size(468, 272);
            this.listPanel.TabIndex = 2;
            // 
            // list
            // 
            this.list.CheckOnClick = true;
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.FormattingEnabled = true;
            this.list.Location = new System.Drawing.Point(10, 10);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(448, 257);
            this.list.TabIndex = 3;
            // 
            // tlsFilter
            // 
            this.tlsFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.tlsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.btnSearch});
            this.tlsFilter.Location = new System.Drawing.Point(0, 0);
            this.tlsFilter.Name = "tlsFilter";
            this.tlsFilter.Padding = new System.Windows.Forms.Padding(0, 7, 7, 7);
            this.tlsFilter.Size = new System.Drawing.Size(468, 37);
            this.tlsFilter.TabIndex = 5;
            this.tlsFilter.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 20);
            this.toolStripLabel1.Text = "Поиск";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // btnSearch
            // 
            this.btnSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(64, 20);
            this.btnSearch.Text = " Найти";
            // 
            // EntitySelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 359);
            this.Controls.Add(this.listPanel);
            this.Controls.Add(this.tlsFilter);
            this.Controls.Add(this.toolsPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EntitySelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EntitySelectForm";
            this.toolsPanel.ResumeLayout(false);
            this.toolsAligner.ResumeLayout(false);
            this.listPanel.ResumeLayout(false);
            this.tlsFilter.ResumeLayout(false);
            this.tlsFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel toolsPanel;
        private System.Windows.Forms.Panel toolsAligner;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel listPanel;
        protected System.Windows.Forms.CheckedListBox list;
        protected System.Windows.Forms.ToolStrip tlsFilter;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSearch;
    }
}