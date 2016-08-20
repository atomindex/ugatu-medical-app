using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма списка сотрудников
    public partial class WorkerListForm : EntityListForm {

        private string[] tableColumnNames;              //Список имен колонок
        private string[] tableFields;                   //Список полей соответвующих колонкам

        private ListData listData;                      //Данные списка сотрудников
        private List<Worker> workersList;               //Список сотрудников

        private SqlFilter filter;                       //Фильтр для запросов
        private SqlFilterCondition[] fullNameFilter;    //Условия фильтра

        private SqlSorter sorter;                       //Сортировка для запросов
        private SqlSorterCondition sorterItem;          //Поля сортировки

        private ToolStripLabel tlsLblWorkerName;        //Подпись к полю ФИО
        private ToolStripTextBox tlsTxtWorkerName;      //Поле ФИО



        //Конструктор
        public WorkerListForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();

            //Инициализируем имена
            tableColumnNames = new string[] { "Фамилия", "Имя", "Отчество", "Телефон", "Адрес" };
            tableFields = new string[] { "last_name", "first_name", "middle_name", "phone", "address" };

            //Создаем фильтр по имени
            filter = new SqlFilter(SqlLogicalOperator.And);
            string fullNameFilterField = QueryBuilder.BuildConcatStatement(
                Worker.GetTableName(),
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


            Text = "Список сотрудников";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];

            //Добавляем компоненты фильтра
            tlsLblWorkerName = new ToolStripLabel("ФИО");
            tlsFilter.Items.Add(tlsLblWorkerName);
            
            tlsTxtWorkerName = new ToolStripTextBox();
            tlsTxtWorkerName.AutoSize = false;
            tlsTxtWorkerName.Width = 200;
            tlsFilter.Items.Add(tlsTxtWorkerName);

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
            workersList = Worker.GetList(listData);

            if (workersList.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            return true;
        }

        //Загрузка данных сотрудника в строку таблицы
        private void loadDataToRow(int rowIndex, Worker worker) {
            table.Rows[rowIndex].Cells[0].Value = worker.LastName;
            table.Rows[rowIndex].Cells[1].Value = worker.FirstName;
            table.Rows[rowIndex].Cells[2].Value = worker.MiddleName;
            table.Rows[rowIndex].Cells[3].Value = worker.Phone;
            table.Rows[rowIndex].Cells[4].Value = worker.Address;
        }

        //Загрузка данных сотрудников в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.List.Count;
            for (int i = 0; i < listData.List.Count; i++)
                loadDataToRow(i, workersList[i]);
        }



        //Событие клика по кнопке Добавление сотрудника
        private void btnAdd_Click(object sender, EventArgs e) {
            Worker worker = new Worker(connection);

            WorkerEditForm workerEditForm = new WorkerEditForm(worker);
            workerEditForm.Text = "Добавление нового сотрудника";

            if (workerEditForm.ShowDialog() == DialogResult.OK) {
                if (workersList.Count > 0) 
                    table.Rows.Insert(0, 1);
                
                table.Rows[0].DefaultCellStyle.BackColor = AppConfig.LightOrangeColor;
                loadDataToRow(0, worker);

                workersList.Insert(0, worker);
            }
        }

        //Событие клика по кнопке Редактирование сотрудника
        private void btnEdit_Click(object sender, EventArgs e) {
            if (workersList.Count == 0)
                return;

            Worker worker = workersList[table.CurrentCell.RowIndex].Clone();

            WorkerEditForm workerEditForm = new WorkerEditForm(worker);
            workerEditForm.Text = "Редактирование сотрудника";

            if (workerEditForm.ShowDialog() == DialogResult.OK) {
                loadDataToRow(table.CurrentCell.RowIndex, worker);
                workersList[table.CurrentCell.RowIndex] = worker;
            }
        }

        //Событие клика по кнопке Удаление сотрудника
        private void btnRemove_Click(object sender, EventArgs e) {
            if (workersList.Count == 0)
                return;

            Worker worker = workersList[table.CurrentCell.RowIndex];

            if (MessageBox.Show("Вы дейсвительно хотите удалить сотрудника?", "Удаление сотрудника", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                worker.Remove();
                reloadData();
            }
        }

        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtWorkerName.Text.Trim().Split(" ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fullNameFilter.Length; i++)
                fullNameFilter[i].SetValue(i < names.Length ? "\"%"+QueryBuilder.EscapeLikeString(names[i])+"%\"" : null);

            reloadData(true);
        }

        //Событие сортировки таблицы по полю
        private void tblSortChange_Event(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;

            sorterItem.SetField(Worker.GetTableName(), tableFields[e.ColumnIndex]);

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
