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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItemLists = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCategories = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPatients = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSpecialties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemWorkers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemServices = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSales = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemVisitLists = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemVisits = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReports = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReport1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReport2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReport3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReport4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTool = new System.Windows.Forms.ToolStrip();
            this.btnAddVitit = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnShowPatientVisits = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnAddPatient = new System.Windows.Forms.ToolStripButton();
            this.table = new System.Windows.Forms.DataGridView();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.mainMenu.SuspendLayout();
            this.mainTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLists,
            this.menuItemVisitLists,
            this.menuItemReports,
            this.menuItemHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(951, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // menuItemLists
            // 
            this.menuItemLists.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCategories,
            this.menuItemPatients,
            this.menuItemSpecialties,
            this.menuItemWorkers,
            this.menuItemServices,
            this.menuItemSales});
            this.menuItemLists.Name = "menuItemLists";
            this.menuItemLists.Size = new System.Drawing.Size(94, 20);
            this.menuItemLists.Text = "Справочники";
            // 
            // menuItemCategories
            // 
            this.menuItemCategories.Name = "menuItemCategories";
            this.menuItemCategories.Size = new System.Drawing.Size(160, 22);
            this.menuItemCategories.Text = "Категории";
            this.menuItemCategories.Click += new System.EventHandler(this.menuItemCategories_Click);
            // 
            // menuItemPatients
            // 
            this.menuItemPatients.Name = "menuItemPatients";
            this.menuItemPatients.Size = new System.Drawing.Size(160, 22);
            this.menuItemPatients.Text = "Пациенты";
            this.menuItemPatients.Click += new System.EventHandler(this.menuItemPatients_Click);
            // 
            // menuItemSpecialties
            // 
            this.menuItemSpecialties.Name = "menuItemSpecialties";
            this.menuItemSpecialties.Size = new System.Drawing.Size(160, 22);
            this.menuItemSpecialties.Text = "Специальности";
            this.menuItemSpecialties.Click += new System.EventHandler(this.menuItemSpecialties_Click);
            // 
            // menuItemWorkers
            // 
            this.menuItemWorkers.Name = "menuItemWorkers";
            this.menuItemWorkers.Size = new System.Drawing.Size(160, 22);
            this.menuItemWorkers.Text = "Сотрудники";
            this.menuItemWorkers.Click += new System.EventHandler(this.menuItemWorkers_Click);
            // 
            // menuItemServices
            // 
            this.menuItemServices.Name = "menuItemServices";
            this.menuItemServices.Size = new System.Drawing.Size(160, 22);
            this.menuItemServices.Text = "Услуги";
            this.menuItemServices.Click += new System.EventHandler(this.menuItemServices_Click);
            // 
            // menuItemSales
            // 
            this.menuItemSales.Name = "menuItemSales";
            this.menuItemSales.Size = new System.Drawing.Size(160, 22);
            this.menuItemSales.Text = "Скидки";
            this.menuItemSales.Click += new System.EventHandler(this.menuItemSales_Click);
            // 
            // menuItemVisitLists
            // 
            this.menuItemVisitLists.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemVisits});
            this.menuItemVisitLists.Name = "menuItemVisitLists";
            this.menuItemVisitLists.Size = new System.Drawing.Size(84, 20);
            this.menuItemVisitLists.Text = "Посещения";
            // 
            // menuItemVisits
            // 
            this.menuItemVisits.Name = "menuItemVisits";
            this.menuItemVisits.Size = new System.Drawing.Size(182, 22);
            this.menuItemVisits.Text = "Список посещений";
            this.menuItemVisits.Click += new System.EventHandler(this.menuItemVisits_Click);
            // 
            // menuItemReports
            // 
            this.menuItemReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReport1,
            this.menuItemReport2,
            this.menuItemReport3,
            this.menuItemReport4});
            this.menuItemReports.Name = "menuItemReports";
            this.menuItemReports.Size = new System.Drawing.Size(60, 20);
            this.menuItemReports.Text = "Отчеты";
            // 
            // menuItemReport1
            // 
            this.menuItemReport1.Name = "menuItemReport1";
            this.menuItemReport1.Size = new System.Drawing.Size(200, 22);
            this.menuItemReport1.Text = "Доход по услугам";
            this.menuItemReport1.Click += new System.EventHandler(this.menuItemReport1_Click);
            // 
            // menuItemReport2
            // 
            this.menuItemReport2.Name = "menuItemReport2";
            this.menuItemReport2.Size = new System.Drawing.Size(200, 22);
            this.menuItemReport2.Text = "Оказанные услуги";
            this.menuItemReport2.Click += new System.EventHandler(this.menuItemReport2_Click);
            // 
            // menuItemReport3
            // 
            this.menuItemReport3.Name = "menuItemReport3";
            this.menuItemReport3.Size = new System.Drawing.Size(200, 22);
            this.menuItemReport3.Text = "Посещения пациентов";
            this.menuItemReport3.Click += new System.EventHandler(this.menuItemReport3_Click);
            // 
            // menuItemReport4
            // 
            this.menuItemReport4.Name = "menuItemReport4";
            this.menuItemReport4.Size = new System.Drawing.Size(200, 22);
            this.menuItemReport4.Text = "Статистика по услугам";
            this.menuItemReport4.Click += new System.EventHandler(this.menuItemReport4_Click);
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
            this.menuItemAbout.Size = new System.Drawing.Size(152, 22);
            this.menuItemAbout.Text = "О программе";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // mainTool
            // 
            this.mainTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.mainTool.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddVitit,
            this.btnRefresh,
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
            this.mainTool.Size = new System.Drawing.Size(951, 37);
            this.mainTool.TabIndex = 2;
            this.mainTool.Text = "toolStrip1";
            // 
            // btnAddVitit
            // 
            this.btnAddVitit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.btnAddVitit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddVitit.ForeColor = System.Drawing.Color.White;
            this.btnAddVitit.Image = global::medic.Properties.Resources.Visit;
            this.btnAddVitit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddVitit.Margin = new System.Windows.Forms.Padding(0, 1, 7, 2);
            this.btnAddVitit.Name = "btnAddVitit";
            this.btnAddVitit.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnAddVitit.Size = new System.Drawing.Size(150, 28);
            this.btnAddVitit.Tag = "1";
            this.btnAddVitit.Text = "Новое посещение";
            this.btnAddVitit.Click += new System.EventHandler(this.btnAddVitit_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::medic.Properties.Resources.ReloadLarge;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(28, 28);
            this.btnRefresh.Text = "toolStripButton1";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnShowPatientVisits
            // 
            this.btnShowPatientVisits.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnShowPatientVisits.Image = global::medic.Properties.Resources.VisitsList;
            this.btnShowPatientVisits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowPatientVisits.Name = "btnShowPatientVisits";
            this.btnShowPatientVisits.Size = new System.Drawing.Size(154, 28);
            this.btnShowPatientVisits.Text = "Посещения пациента";
            this.btnShowPatientVisits.Click += new System.EventHandler(this.btnShowPatientVisits_Click);
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
            // btnSearch
            // 
            this.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSearch.Image = global::medic.Properties.Resources.SearchLarge;
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
            this.btnAddPatient.Image = global::medic.Properties.Resources.Patient;
            this.btnAddPatient.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPatient.Margin = new System.Windows.Forms.Padding(0, 1, 7, 2);
            this.btnAddPatient.Name = "btnAddPatient";
            this.btnAddPatient.Size = new System.Drawing.Size(121, 28);
            this.btnAddPatient.Text = "Новый пациент";
            this.btnAddPatient.Click += new System.EventHandler(this.btnAddPatient_Click);
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
            this.table.Size = new System.Drawing.Size(951, 348);
            this.table.TabIndex = 4;
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 30000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 409);
            this.Controls.Add(this.table);
            this.Controls.Add(this.mainTool);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
        private System.Windows.Forms.ToolStripMenuItem menuItemVisitLists;
        private System.Windows.Forms.ToolStripMenuItem menuItemVisits;
        private System.Windows.Forms.ToolStripButton btnShowPatientVisits;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.ToolStripTextBox txtBoxSearch;
        protected System.Windows.Forms.DataGridView table;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripMenuItem menuItemPatients;
        private System.Windows.Forms.ToolStripMenuItem menuItemCategories;
        private System.Windows.Forms.ToolStripMenuItem menuItemReports;
        private System.Windows.Forms.ToolStripMenuItem menuItemReport1;
        private System.Windows.Forms.ToolStripMenuItem menuItemReport2;
        private System.Windows.Forms.ToolStripMenuItem menuItemReport3;
        private System.Windows.Forms.ToolStripMenuItem menuItemReport4;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.Timer refreshTimer;
    }
}

