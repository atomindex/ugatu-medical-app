using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using medic.Forms;
using medic.Database;
using medic.Components;

namespace medic {

    public partial class MainForm : Form {

        private string[] tableFields;                   //Список полей соответвующих колонкам

        private DBConnection connection;

        private Cursor tempCursor;

        private ListData listData;
        private List<Patient> patientsList;

        private SqlFilter filter;                       //Фильтр для запросов
        private SqlFilterCondition[] fullNameFilter;    //Условия фильтра

        private SqlSorter sorter;                       //Сортировка для запросов
        private SqlSorterCondition sorterItem;          //Поля сортировки

        protected TablePager tblPager;

        public MainForm() {
            InitializeComponent();

            mainTool.Renderer = new ButtonRenderer();
            btnAddVitit.MouseEnter += toolStripButton_MouseEnter;
            btnAddVitit.MouseLeave += toolStripButton_MouseLeave;

            connection = new DBConnection(
                AppConfig.DatabaseHost, 
                AppConfig.DatabaseName, 
                AppConfig.DatabaseUser, 
                AppConfig.DatabasePassword,
                AppConfig.DatabaseConnectTime
            );

            table.ColumnCount = 5;
            table.Columns[0].HeaderText = "Фамилия";
            table.Columns[1].HeaderText = "Имя";
            table.Columns[2].HeaderText = "Отчество";
            table.Columns[3].HeaderText = "Пол";
            table.Columns[4].HeaderText = "Дата рождения";

            //Создаем пейджер
            tblPager = new TablePager();
            tblPager.Parent = this;
            tblPager.Dock = DockStyle.Bottom;
            tblPager.AddChangeEvent(tblPagerPageChange_Event);


            //Инициализируем имена
            tableFields = new string[] { "last_name", "first_name", "middle_name", "sex", "birthday" };

            //Создаем фильтр по имени
            filter = new SqlFilter(SqlLogicalOperator.And);
            string fullNameFilterField = QueryBuilder.BuildConcatStatement(
                Patient.GetTableName(),
                new string[] { "last_name", "first_name", "middle_name" }
            );
            fullNameFilter = new SqlFilterCondition[] {
                new SqlFilterCondition(fullNameFilterField, SqlComparisonOperator.Like),
                new SqlFilterCondition(fullNameFilterField, SqlComparisonOperator.Like),
                new SqlFilterCondition(fullNameFilterField, SqlComparisonOperator.Like)
            };
            filter.AddItems(fullNameFilter);

            //Создаем сортировщик
            sorter = new SqlSorter();
            sorterItem = new SqlSorterCondition();
            sorter.AddItem(sorterItem);

            listData = Patient.GetListData(connection, AppConfig.BaseLimit, 0, filter, sorter);

            btnSearch.Click += btnSearch_Click;

            reloadData();
        }

        //Перезагрузка данных в таблицу
        protected bool reloadData(bool resetPageIndex = false) {
            if (listData.Update(resetPageIndex ? 0 : tblPager.GetPage()) == null) {
                refreshTimer.Enabled = false;
                return false;
            }
            refreshTimer.Enabled = true;

            patientsList = Patient.GetList(listData);

            if (patientsList.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);
            
            return true;
        }

        //Очистка таблицы
        protected static void ClearTable(DataGridView table) {
            for (int i = 0; i < table.RowCount; i++) {
                table.Rows[i].DefaultCellStyle.BackColor = Color.White;
                for (int j = 0; j < table.ColumnCount; j++)
                    table.Rows[i].Cells[j].Value = "";
            }
        }

        //Очистка цвета таблицы
        protected static void ClearTableColor(DataGridView table) {
            for (int i = 0; i < table.RowCount; i++)
                table.Rows[i].DefaultCellStyle.BackColor = Color.White;
        }

        //Загрузка данных пациента в строку таблицы
        private void loadDataToRow(int rowIndex, Patient patient) {
            table.Rows[rowIndex].Cells[0].Value = patient.FirstName;
            table.Rows[rowIndex].Cells[1].Value = patient.LastName;
            table.Rows[rowIndex].Cells[2].Value = patient.MiddleName;
            table.Rows[rowIndex].Cells[3].Value = patient.GetStringSex();
            table.Rows[rowIndex].Cells[4].Value = patient.Birthday.ToString(AppConfig.DateFormat);
        }

        //Загрузка данных пациентов в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.List.Count;
            for (int i = 0; i < listData.List.Count; i++)
                loadDataToRow(i, patientsList[i]);
        }



        private void menuItemWorkers_Click(object sender, EventArgs e) {
            ListData listData = Worker.GetListData(connection, AppConfig.BaseLimit);
            WorkerListForm workersListForm = new WorkerListForm(listData);
            workersListForm.ShowDialog();
        }

        private void menuItemServices_Click(object sender, EventArgs e) {
            ListData listData = Service.GetListData(connection, AppConfig.BaseLimit);
            ServiceListForm servicesListForm = new ServiceListForm(listData);
            servicesListForm.ShowDialog();
        }

        private void menuItemSpecialties_Click(object sender, EventArgs e) {
            ListData listData = Specialty.GetListData(connection, AppConfig.BaseLimit);
            SpecialtyListForm specialtiesListForm = new SpecialtyListForm(listData);
            specialtiesListForm.ShowDialog();
        }

        private void menuItemSales_Click(object sender, EventArgs e) {
            ListData listData = Sale.GetListData(connection, 25);
            SaleListForm saleListForm = new SaleListForm(listData);
            saleListForm.ShowDialog();
        }

        private void menuItemPatients_Click(object sender, EventArgs e) {
            ListData listData = Patient.GetListData(connection, AppConfig.BaseLimit);
            PatientListForm patientListForm = new PatientListForm(listData);
            patientListForm.ShowDialog();
        }

        private void menuItemCategories_Click(object sender, EventArgs e) {
            ListData listData = Category.GetListData(connection, AppConfig.BaseLimit);
            CategoryListForm categoryListForm = new CategoryListForm(listData);
            categoryListForm.ShowDialog();
        }


        private void toolStripButton_MouseEnter(object sender, EventArgs e) {
            if (tempCursor == null) {
                tempCursor = this.Cursor;
                this.Cursor = Cursors.Hand;
            }
        }

        private void toolStripButton_MouseLeave(object sender, EventArgs e) {
            this.Cursor = tempCursor;
            tempCursor = null;
        }



        //Событие измения страницы таблицы
        private void tblPagerPageChange_Event(object sender, EventArgs e) {
            reloadData();
        }

        private void btnAddPatient_Click(object sender, EventArgs e) {
            Patient patient = new Patient(connection);

            PatientEditForm patientEditForm = new PatientEditForm(patient);
            patientEditForm.Text = "Добавление нового пациента";

            if (patientEditForm.ShowDialog() == DialogResult.OK) {
                if (patientsList.Count > 0)
                    table.Rows.Insert(0, 1);

                table.Rows[0].DefaultCellStyle.BackColor = AppConfig.LightOrangeColor;
                loadDataToRow(0, patient);

                patientsList.Insert(0, patient);
            }
        }


        private void btnAddVitit_Click(object sender, EventArgs e) {
            if (patientsList.Count == 0)
                return;

            Patient patient = patientsList[table.CurrentCell.RowIndex];
            Visit visit = new Visit(connection);

            VisitEditForm visitEditForm = new VisitEditForm(visit, patient);
            visitEditForm.Text = "Добавление нового посещения";
            visitEditForm.ShowDialog();
        }

        private void menuItemVisits_Click(object sender, EventArgs e) {
            ListData visitsListData = Visit.GetListData(connection, 25);
            visitsListData.Update();
            VisitListForm visitsListForm = new VisitListForm(visitsListData);
            visitsListForm.ShowDialog();
        }


        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = txtBoxSearch.Text.Trim().Split(" ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fullNameFilter.Length; i++)
                fullNameFilter[i].SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);

            reloadData(true);
        }

        //Событие сортировки таблицы по полю
        private void tblSortChange_Event(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;

            sorterItem.SetField(Patient.GetTableName(), tableFields[e.ColumnIndex]);

            switch (headerCell.SortGlyphDirection) {
                case SortOrder.Ascending:
                    sorterItem.SetOrder(SqlOrder.Asc);
                    break;
                case SortOrder.Descending:
                    sorterItem.SetOrder(SqlOrder.Desc);
                    break;
                case SortOrder.None:
                    sorterItem.SetOrder(SqlOrder.None);
                    break;
            }

            reloadData(true);
        }

        private void btnShowPatientVisits_Click(object sender, EventArgs e) {
            if (patientsList.Count == 0)
                return;

            Patient patient = patientsList[table.CurrentCell.RowIndex];
            ListData visitsListData = Visit.GetPatientListData(patient.GetId(), connection, 5);
            VisitListForm visitListForm = new VisitListForm(visitsListData);
            visitListForm.ShowDialog();
        }

        private void menuItemReport1_Click(object sender, EventArgs e) {
            ServicesProfitReport report = new ServicesProfitReport(connection);
            report.ShowDialog();
        }

        private void menuItemReport2_Click(object sender, EventArgs e) {
            ListData listData = Worker.GetListData(connection, 25);
            WorkerSelectForm workerSelectForm = new WorkerSelectForm(listData);

            if (workerSelectForm.ShowDialog() == DialogResult.OK) {
                List<Worker> workers = workerSelectForm.GetSelected();
                if (workers.Count == 0)
                    return;
                WorkerServicesReport workerServicesReport = new WorkerServicesReport(connection, workers);
                workerServicesReport.ShowDialog();
            }
        }

        private void menuItemReport3_Click(object sender, EventArgs e) {
            ListData listData = Patient.GetListData(connection, 25);
            PatientSelectForm patientSelectForm = new PatientSelectForm(listData);

            if (patientSelectForm.ShowDialog() == DialogResult.OK) {
                List<Patient> patients = patientSelectForm.GetSelected();
                if (patients.Count == 0)
                    return;
                PatientsVisitsReport patientsVisitsReport = new PatientsVisitsReport(connection, patients);
                patientsVisitsReport.ShowDialog();
            }
        }

        private void menuItemReport4_Click(object sender, EventArgs e) {
            ServicesReport servicesReport = new ServicesReport(connection);
            servicesReport.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            if (reloadData() && !refreshTimer.Enabled)
                    refreshTimer.Enabled = true;
        }

        private void refreshTimer_Tick(object sender, EventArgs e) {
            reloadData();
        }

        private void menuItemAbout_Click(object sender, EventArgs e) {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void menuItemExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

    }

}
