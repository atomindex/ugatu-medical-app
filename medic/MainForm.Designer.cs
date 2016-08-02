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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItemLists = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemWorkers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemServices = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSpecialties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSales = new System.Windows.Forms.ToolStripMenuItem();
            this.визитыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокВизитовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTool = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.table = new System.Windows.Forms.DataGridView();
            this.btnAddVitit = new System.Windows.Forms.ToolStripButton();
            this.btnShowPatientVisits = new System.Windows.Forms.ToolStripButton();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnAddPatient = new System.Windows.Forms.ToolStripButton();
            this.menuItemPatients = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.mainTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLists,
            this.визитыToolStripMenuItem,
            this.menuItemHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(830, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // menuItemLists
            // 
            this.menuItemLists.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemWorkers,
            this.menuItemServices,
            this.menuItemSpecialties,
            this.menuItemSales,
            this.menuItemPatients});
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
            // menuItemSpecialties
            // 
            this.menuItemSpecialties.Name = "menuItemSpecialties";
            this.menuItemSpecialties.Size = new System.Drawing.Size(152, 22);
            this.menuItemSpecialties.Text = "Должности";
            this.menuItemSpecialties.Click += new System.EventHandler(this.menuItemSpecialties_Click);
            // 
            // menuItemSales
            // 
            this.menuItemSales.Name = "menuItemSales";
            this.menuItemSales.Size = new System.Drawing.Size(152, 22);
            this.menuItemSales.Text = "Скидки";
            this.menuItemSales.Click += new System.EventHandler(this.menuItemSales_Click);
            // 
            // визитыToolStripMenuItem
            // 
            this.визитыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.списокВизитовToolStripMenuItem});
            this.визитыToolStripMenuItem.Name = "визитыToolStripMenuItem";
            this.визитыToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.визитыToolStripMenuItem.Text = "Визиты";
            // 
            // списокВизитовToolStripMenuItem
            // 
            this.списокВизитовToolStripMenuItem.Name = "списокВизитовToolStripMenuItem";
            this.списокВизитовToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.списокВизитовToolStripMenuItem.Text = "Список визитов";
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
            // mainTool
            // 
            this.mainTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.mainTool.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddVitit,
            this.btnShowPatientVisits,
            this.toolStripSeparator1,
            this.lblSearch,
            this.txtBoxSearch,
            this.btnSearch,
            this.btnAddPatient});
            this.mainTool.Location = new System.Drawing.Point(0, 24);
            this.mainTool.Name = "mainTool";
            this.mainTool.Padding = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.mainTool.ShowItemToolTips = false;
            this.mainTool.Size = new System.Drawing.Size(830, 37);
            this.mainTool.TabIndex = 2;
            this.mainTool.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // lblSearch
            // 
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(42, 28);
            this.lblSearch.Text = "Поиск";
            // 
            // txtBoxSearch
            // 
            this.txtBoxSearch.Name = "txtBoxSearch";
            this.txtBoxSearch.Size = new System.Drawing.Size(180, 31);
            // 
            // table
            // 
            this.table.AllowUserToAddRows = false;
            this.table.AllowUserToDeleteRows = false;
            this.table.AllowUserToResizeRows = false;
            this.table.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.table.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.table.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.table.DefaultCellStyle = dataGridViewCellStyle2;
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.table.Location = new System.Drawing.Point(0, 61);
            this.table.MultiSelect = false;
            this.table.Name = "table";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.table.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.table.RowHeadersVisible = false;
            this.table.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.table.Size = new System.Drawing.Size(830, 348);
            this.table.TabIndex = 4;
            // 
            // btnAddVitit
            // 
            this.btnAddVitit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.btnAddVitit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddVitit.ForeColor = System.Drawing.Color.White;
            this.btnAddVitit.Image = global::medic.Properties.Resources.visit;
            this.btnAddVitit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddVitit.Margin = new System.Windows.Forms.Padding(0, 1, 7, 2);
            this.btnAddVitit.Name = "btnAddVitit";
            this.btnAddVitit.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnAddVitit.Size = new System.Drawing.Size(121, 28);
            this.btnAddVitit.Tag = "1";
            this.btnAddVitit.Text = "Новый визит";
            // 
            // btnShowPatientVisits
            // 
            this.btnShowPatientVisits.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnShowPatientVisits.Image = global::medic.Properties.Resources.visit_list;
            this.btnShowPatientVisits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowPatientVisits.Name = "btnShowPatientVisits";
            this.btnShowPatientVisits.Size = new System.Drawing.Size(129, 28);
            this.btnShowPatientVisits.Text = "Визиты пациента";
            // 
            // btnSearch
            // 
            this.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSearch.Image = global::medic.Properties.Resources.search_large;
            this.btnSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(28, 28);
            this.btnSearch.Text = "toolStripButton4";
            this.btnSearch.ToolTipText = "Поиск";
            // 
            // btnAddPatient
            // 
            this.btnAddPatient.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnAddPatient.Image = global::medic.Properties.Resources.patient;
            this.btnAddPatient.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPatient.Margin = new System.Windows.Forms.Padding(0, 1, 7, 2);
            this.btnAddPatient.Name = "btnAddPatient";
            this.btnAddPatient.Size = new System.Drawing.Size(121, 28);
            this.btnAddPatient.Text = "Новый пациент";
            this.btnAddPatient.Click += new System.EventHandler(this.btnAddPatient_Click);
            // 
            // menuItemPatients
            // 
            this.menuItemPatients.Name = "menuItemPatients";
            this.menuItemPatients.Size = new System.Drawing.Size(152, 22);
            this.menuItemPatients.Text = "Пациенты";
            this.menuItemPatients.Click += new System.EventHandler(this.menuItemPatients_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 409);
            this.Controls.Add(this.table);
            this.Controls.Add(this.mainTool);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "mainForm";
            this.Text = "Medical";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainTool.ResumeLayout(false);
            this.mainTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem menuItemSales;
        private System.Windows.Forms.ToolStrip mainTool;
        private System.Windows.Forms.ToolStripButton btnAddVitit;
        private System.Windows.Forms.ToolStripButton btnAddPatient;
        private System.Windows.Forms.ToolStripMenuItem визитыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокВизитовToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnShowPatientVisits;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.ToolStripTextBox txtBoxSearch;
        protected System.Windows.Forms.DataGridView table;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripMenuItem menuItemPatients;
    }
}

