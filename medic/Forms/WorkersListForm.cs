using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма списка сотрудников
    public partial class WorkersListForm : EntityListForm {

        private string[] tableColumnNames = new string[] { "Имя", "Фамилия", "Отчество", "Телефон", "Адрес" };
        private string[] tableFields = new string[] { "first_name", "last_name", "middle_name", "phone", "address" };

        private ListData<Worker> listData;      //Список сотрудников

        private SqlFilter filter;
        private SqlFilterCondition[] fullNameFilter;

        private SqlSorter sorter;
        private SqlSorterItem sorterItem;

        private ToolStripLabel tlsLblWorkerName;
        private ToolStripTextBox tlsTxtWorkerName;

        //Конструктор
        public WorkersListForm(DBConnection connection, ListData<Worker> initialListData) : base(connection) {
            InitializeComponent();

            Text = "Список работников";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];
            DisableSort();

            //Добавляем компоненты фильтра
            tlsLblWorkerName = new ToolStripLabel("ФИО");
            tlsTxtWorkerName = new ToolStripTextBox();
            tlsFilter.Items.Add(tlsLblWorkerName);
            tlsFilter.Items.Add(tlsTxtWorkerName);

            //Добавляем события
            AddAddEvent(btnAdd_Click);
            AddEditEvent(btnEdit_Click);
            AddRemoveEvent(btnRemove_Click);
            AddSearchEvent(btnSearch_Click);
            AddPageChangeEvent(btnPageChange_Event);
            AddSortChangeEvent(tblSortChange_Event);


            //Создаем фильтр
            filter = new SqlFilter(SqlFilterOperator.and);

            //Создаем условия для фильтра по имени 
            string fullNameFilterField = SqlFilter.BuildConcatStatement(
                Worker.GetTableName(), 
                new string[] {"first_name", "last_name", "middle_name"}
            );
            fullNameFilter = new SqlFilterCondition[] {
                new SqlFilterCondition("", fullNameFilterField, SqlFilterConditionOperator.like),
                new SqlFilterCondition("", fullNameFilterField, SqlFilterConditionOperator.like),
                new SqlFilterCondition("", fullNameFilterField, SqlFilterConditionOperator.like)
            };

            //Добавляем условия в фильтр
            filter.AddItems(fullNameFilter);

            //Добавляем сортировку
            sorter = new SqlSorter();
            sorterItem = new SqlSorterItem();
            sorter.AddItem(sorterItem);

            //Подгружаем начальные данные
            LoadData(initialListData);
        }



        //Подгрузка данных в таблицу и пейджер
        public void LoadData(ListData<Worker> workersListData) {
            listData = workersListData;

            if (listData.list == null) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(workersListData.count, workersListData.pages, workersListData.pageIndex);
        }



        //Перезагрузка
        private void reloadData() {
            listData = Worker.GetList(connection, listData.limit, tblPager.GetPage(), filter, sorter);
            LoadData(listData);
        }

        //Загрузка данных сотрудника в строку таблицы
        private void loadDataToRow(int rowIndex, Worker worker) {
            table.Rows[rowIndex].Cells[0].Value = worker.FirstName;
            table.Rows[rowIndex].Cells[1].Value = worker.LastName;
            table.Rows[rowIndex].Cells[2].Value = worker.MiddleName;
            table.Rows[rowIndex].Cells[3].Value = worker.Phone;
            table.Rows[rowIndex].Cells[4].Value = worker.Address;
        }

        //Загрузка данных сотрудников в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.list.Count;
            for (int i = 0; i < listData.list.Count; i++)
                loadDataToRow(i, listData.list[i]);
        }



        //Событие клика по кнопке Добавление сотрудника
        private void btnAdd_Click(object sender, EventArgs e) {
            Worker worker = new Worker(connection);

            WorkerEditForm workerEditForm = new WorkerEditForm(worker);
            workerEditForm.Text = "Добавление нового сотрудника";

            if (workerEditForm.ShowDialog() == DialogResult.OK) {
                listData.list.Insert(0, worker);
                table.Rows.Insert(0, 1);
                table.Rows[0].DefaultCellStyle.BackColor = AppConfig.LightOrangeColor;
                loadDataToRow(0, worker);
            }
        }

        //Событие клика по кнопке Редактирование сотрудника
        private void btnEdit_Click(object sender, EventArgs e) {
            Worker worker = listData.list[table.CurrentCell.RowIndex];

            WorkerEditForm workerEditForm = new WorkerEditForm(worker);
            workerEditForm.Text = "Редактирование сотрудника";

            if (workerEditForm.ShowDialog() == DialogResult.OK)
                loadDataToRow(table.CurrentCell.RowIndex, worker);
        }

        //Событие клика по кнопке Удаление сотрудника
        private void btnRemove_Click(object sender, EventArgs e) {
            int index = table.CurrentCell.RowIndex;
            Worker worker = listData.list[index];

            if (MessageBox.Show("Вы дейсвительно хотите удалить сотрудника?", "Удаление сотрудника", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                worker.Remove();
                reloadData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtWorkerName.Text.Trim().Split(" ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fullNameFilter.Length; i++)
                fullNameFilter[i].SetValue(i < names.Length ? "\"%"+QueryBuilder.EscapeLikeString(names[i])+"%\"" : null);

            reloadData();
        }

        //Событие измения страницы таблицы
        private void btnPageChange_Event(object sender, EventArgs e) {
            reloadData();
        }

        private void tblSortChange_Event(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;

            sorterItem.SetField(Worker.GetTableName(), tableFields[e.ColumnIndex]);

            switch (headerCell.SortGlyphDirection) {
                case SortOrder.Ascending: 
                    sorterItem.SetOrder(SqlSorterOrder.asc);
                    break;
                case SortOrder.Descending:
                    sorterItem.SetOrder(SqlSorterOrder.desc);
                    break;
                case SortOrder.None:
                    sorterItem.SetOrder(SqlSorterOrder.none);
                    break;
            }

            reloadData();
        }
    }
}
