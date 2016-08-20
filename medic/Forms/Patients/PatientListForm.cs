using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма списка пациентов
    public partial class PatientListForm : EntityListForm {

        private string[] tableColumnNames;              //Список имен колонок
        private string[] tableFields;                   //Список полей соответвующих колонкам

        private ListData listData;                      //Данные списка пациентов
        private List<Patient> patientsList;             //Список пациентов

        private SqlFilter filter;                       //Фильтр для запросов
        private SqlFilterCondition[] fullNameFilter;    //Условия фильтра

        private SqlSorter sorter;                       //Сортировка для запросов
        private SqlSorterCondition sorterItem;          //Поля сортировки

        private ToolStripLabel tlsLblPatientName;        //Подпись к полю ФИО
        private ToolStripTextBox tlsTxtPatientName;      //Поле ФИО



        //Конструктор
        public PatientListForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();

            //Инициализируем имена
            tableColumnNames = new string[] { "Фамилия", "Имя", "Отчество", "Пол", "Дата рождения" };
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
            filter.AddItem(initialListData.Filter);

            //Создаем сортировщик
            sorter = new SqlSorter();
            sorterItem = new SqlSorterCondition();
            sorter.AddItem(sorterItem);
            sorter.AddItem(initialListData.Sorter);

            //Создаем данные списка
            listData = new ListData(
                connection: initialListData.Connection,
                baseSql: initialListData.BaseSql,
                countSql: initialListData.CountSql,
                filter: filter,
                sorter: sorter,
                limit: initialListData.Limit,
                pageIndex: initialListData.PageIndex
            );


            Text = "Список пациентов";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];

            //Добавляем компоненты фильтра
            tlsLblPatientName = new ToolStripLabel("ФИО");
            tlsFilter.Items.Add(tlsLblPatientName);
            
            tlsTxtPatientName = new ToolStripTextBox();
            tlsTxtPatientName.AutoSize = false;
            tlsTxtPatientName.Width = 200;
            tlsFilter.Items.Add(tlsTxtPatientName);

            //Добавляем события
            AddAddEvent(btnAdd_Click);
            AddEditEvent(btnEdit_Click);
            AddRemoveEvent(btnRemove_Click);
            AddSearchEvent(btnSearch_Click);
            AddSortChangeEvent(tblSortChange_Event);
        }



        //Перезагрузка данных в таблицу
        protected override bool reloadData(bool resetPageIndex = false) {
            if (listData.Update(resetPageIndex ? 0 : tblPager.GetPage()) == null) {
                DialogResult = DialogResult.Abort;
                Close();
                return false;
            }

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

        //Загрузка данных пациента в строку таблицы
        private void loadDataToRow(int rowIndex, Patient patient) {
            table.Rows[rowIndex].Cells[0].Value = patient.LastName;
            table.Rows[rowIndex].Cells[1].Value = patient.FirstName;
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



        //Событие клика по кнопке Добавление пациента
        private void btnAdd_Click(object sender, EventArgs e) {
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

        //Событие клика по кнопке Редактирование пациента
        private void btnEdit_Click(object sender, EventArgs e) {
            if (patientsList.Count == 0)
                return;

            Patient patient = patientsList[table.CurrentCell.RowIndex].Clone();

            PatientEditForm patientEditForm = new PatientEditForm(patient);
            patientEditForm.Text = "Редактирование пациента";

            if (patientEditForm.ShowDialog() == DialogResult.OK) {
                loadDataToRow(table.CurrentCell.RowIndex, patient);
                patientsList[table.CurrentCell.RowIndex] = patient;
            }
        }

        //Событие клика по кнопке Удаление пациента
        private void btnRemove_Click(object sender, EventArgs e) {
            if (patientsList.Count == 0)
                return;

            Patient patient = patientsList[table.CurrentCell.RowIndex];

            if (MessageBox.Show("Вы дейсвительно хотите удалить пациента?", "Удаление пациента", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                patient.Remove();
                reloadData();
            }
        }

        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtPatientName.Text.Trim().Split(" ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fullNameFilter.Length; i++)
                fullNameFilter[i].SetValue(i < names.Length ? "\"%"+QueryBuilder.EscapeLikeString(names[i])+"%\"" : null);

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

    }

}
