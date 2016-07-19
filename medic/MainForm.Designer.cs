namespace medic {
    partial class mainForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItemLists = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemWorkers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemServices = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSpecialties = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLists,
            this.menuItemHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(658, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // menuItemLists
            // 
            this.menuItemLists.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemWorkers,
            this.menuItemServices,
            this.menuItemSpecialties});
            this.menuItemLists.Name = "menuItemLists";
            this.menuItemLists.Size = new System.Drawing.Size(94, 20);
            this.menuItemLists.Text = "Справочники";
            // 
            // menuItemWorkers
            // 
            this.menuItemWorkers.Name = "menuItemWorkers";
            this.menuItemWorkers.Size = new System.Drawing.Size(152, 22);
            this.menuItemWorkers.Text = "Сотрудники";
            this.menuItemWorkers.Click += new System.EventHandler(this.menuItemWorkers_Click);
            // 
            // menuItemServices
            // 
            this.menuItemServices.Name = "menuItemServices";
            this.menuItemServices.Size = new System.Drawing.Size(152, 22);
            this.menuItemServices.Text = "Услуги";
            this.menuItemServices.Click += new System.EventHandler(this.menuItemServices_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(68, 20);
            this.menuItemHelp.Text = "Помощь";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(149, 22);
            this.menuItemAbout.Text = "О программе";
            // 
            // menuItemSpecialties
            // 
            this.menuItemSpecialties.Name = "menuItemSpecialties";
            this.menuItemSpecialties.Size = new System.Drawing.Size(152, 22);
            this.menuItemSpecialties.Text = "Должности";
            this.menuItemSpecialties.Click += new System.EventHandler(this.menuItemSpecialties_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 409);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "mainForm";
            this.Text = "Medical";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemLists;
        private System.Windows.Forms.ToolStripMenuItem menuItemWorkers;
        private System.Windows.Forms.ToolStripMenuItem menuItemServices;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuItemSpecialties;
    }
}

