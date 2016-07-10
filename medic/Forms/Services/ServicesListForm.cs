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

    //Форма списка услуг
    public partial class ServicesListForm : EntityListForm {

        private string[] tableColumnNames;             //Список имен колонок
        private string[] tableFields;                  //Список полей соответвующих колонкам

        private ListData<Service> listData;             //Список услуг

        private SqlFilter filter;                      //Фильтр для запросов

        private SqlSorter sorter;                      //Сортировка для запросов
        private SqlSorterItem sorterItem;              //Поля сортировки

        private ToolStripLabel tlsLblWorkerName;       //Подпись к полю название
        private ToolStripTextBox tlsTxtWorkerName;     //Поле Название



        //Конструктор
        public ServicesListForm(DBConnection connection, ListData<Service> initialListData) : base(connection) {
            InitializeComponent();

            //Инициализируем имена
            tableColumnNames = new string[] { "Название", "Цена" };
            tableFields = new string[] { "name", "price" };

            //Добавляем сортировку
            sorter = new SqlSorter();
            sorterItem = new SqlSorterItem();
            sorter.AddItem(sorterItem);



            Text = "Список услуг";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];

            //Добавляем компоненты фильтра
            tlsLblWorkerName = new ToolStripLabel("Название");
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
            AddPageChangeEvent(btnPageChange_Event);
            AddSortChangeEvent(tblSortChange_Event);

            //Подгружаем начальные данные
            LoadData(initialListData);
        }



        //Подгрузка данных в таблицу и пейджер
        public void LoadData(ListData<Service> servicesListData) {
            listData = servicesListData;

            if (listData.list.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(servicesListData.count, servicesListData.pages, servicesListData.pageIndex);
        }



        //Перезагрузка
        private void reloadData(bool resetPageIndex = false) {
            listData = Service.GetList(connection, listData.limit, resetPageIndex ? 0 : tblPager.GetPage(), filter, sorter);
            LoadData(listData);
        }

        //Загрузка данных сотрудника в строку таблицы
        private void loadDataToRow(int rowIndex, Service service) {
            table.Rows[rowIndex].Cells[0].Value = service.Name;
            table.Rows[rowIndex].Cells[1].Value = service.Price.ToString();
        }

        //Загрузка данных сотрудников в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.list.Count;
            for (int i = 0; i < listData.list.Count; i++)
                loadDataToRow(i, listData.list[i]);
        }



        //Событие клика по кнопке Добавление сотрудника
        private void btnAdd_Click(object sender, EventArgs e) {
            Service service = new Service(connection);

            ServiceEditForm serviceEditForm = new ServiceEditForm(service);
            serviceEditForm.Text = "Добавление новой услуги";

            if (serviceEditForm.ShowDialog() == DialogResult.OK) {
                listData.list.Insert(0, service);
                table.Rows.Insert(0, 1);
                table.Rows[0].DefaultCellStyle.BackColor = AppConfig.LightOrangeColor;
                loadDataToRow(0, service);
            }
        }

        //Событие клика по кнопке Редактирование сотрудника
        private void btnEdit_Click(object sender, EventArgs e) {
            Service service = listData.list[table.CurrentCell.RowIndex];

            ServiceEditForm serviceEditForm = new ServiceEditForm(service);
            serviceEditForm.Text = "Редактирование услуги";

            if (serviceEditForm.ShowDialog() == DialogResult.OK)
                loadDataToRow(table.CurrentCell.RowIndex, service);
        }

        //Событие клика по кнопке Удаление сотрудника
        private void btnRemove_Click(object sender, EventArgs e) {
            int index = table.CurrentCell.RowIndex;
            Service service = listData.list[index];

            if (MessageBox.Show("Вы дейсвительно хотите удалить услугу?", "Удаление услуги", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                service.Remove();
                reloadData();
            }
        }

        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtWorkerName.Text.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (names.Length > 0) {
                //Создаем фильтр
                filter = new SqlFilter(SqlLogicalOperator.And);

                //Создаем условия для фильтра по имени 
                for (int i = 0; i < names.Length; i++) {
                    SqlFilterCondition nameFilter = new SqlFilterCondition(Service.GetTableName(), "name", SqlComparisonOperator.Like);
                    nameFilter.SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);
                    filter.AddItem(nameFilter);
                }
            } else
                filter = null;

            reloadData(true);
        }

        //Событие измения страницы таблицы
        private void btnPageChange_Event(object sender, EventArgs e) {
            reloadData();
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
