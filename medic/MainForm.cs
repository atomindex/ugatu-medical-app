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

    public partial class mainForm : Form {
        DBConnection connection;

        private Cursor tempCursor;

        private ListData listData;
        private List<Patient> patientsList;

        protected TablePager tblPager;

        public mainForm() {
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
            table.Columns[0].HeaderText = "Имя";
            table.Columns[1].HeaderText = "Фамилия";
            table.Columns[2].HeaderText = "Отчество";
            table.Columns[3].HeaderText = "Пол";
            table.Columns[4].HeaderText = "Дата рождения";

            //Создаем пейджер
            tblPager = new TablePager();
            tblPager.Parent = this;
            tblPager.Dock = DockStyle.Bottom;

            tblPager.AddChangeEvent(tblPagerPageChange_Event);

            listData = Patient.GetListData(connection, 25);
            reloadData();
        }

        //Перезагрузка данных в таблицу
        protected void reloadData(bool resetPageIndex = false) {
            listData.Update(resetPageIndex ? 0 : tblPager.GetPage());
            patientsList = Patient.GetList(listData);

            if (patientsList.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);
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
            table.Rows[rowIndex].Cells[3].Value = patient.Sex;
            table.Rows[rowIndex].Cells[4].Value = patient.Birthday.ToString(AppConfig.DateFormat);
        }

        //Загрузка данных пациентов в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.List.Count;
            for (int i = 0; i < listData.List.Count; i++)
                loadDataToRow(i, patientsList[i]);
        }



        private void menuItemWorkers_Click(object sender, EventArgs e) {
            ListData listData = Worker.GetListData(connection, 2);
            WorkerListForm workersListForm = new WorkerListForm(listData);
            workersListForm.Show();
        }

        private void menuItemServices_Click(object sender, EventArgs e) {
            ListData listData = Service.GetListData(connection, 25);
            ServiceListForm servicesListForm = new ServiceListForm(listData);
            servicesListForm.Show();
        }

        private void menuItemSpecialties_Click(object sender, EventArgs e) {
            ListData listData = Specialty.GetListData(connection, 25);
            SpecialtyListForm specialtiesListForm = new SpecialtyListForm(listData);
            specialtiesListForm.Show();
        }

        private void menuItemSales_Click(object sender, EventArgs e) {
            ListData listData = Sale.GetListData(connection, 25);
            SaleListForm saleListForm = new SaleListForm(listData);
            saleListForm.Show();
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

    }

}
