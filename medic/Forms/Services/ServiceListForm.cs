using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма списка услуг
    public partial class ServiceListForm : EntityListForm {

        private string[] tableColumnNames;              //Список имен колонок
        private string[] tableFields;                   //Список полей соответвующих колонкам

        private ListData listData;                      //Данные списка услуг
        private List<Service> servicesList;             //Список услуг

        private SqlFilter filter;                       //Фильтр для запросов
        private SqlFilter nameFilterGroup;              //Условия фильтра

        private SqlSorter sorter;                       //Сортировка для запросов
        private SqlSorterCondition sorterItem;          //Поля сортировки

        private ToolStripLabel tlsLblServiceName;       //Подпись к полю название
        private ToolStripTextBox tlsTxtServiceName;     //Поле Название



        //Конструктор
        public ServiceListForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();

            //Инициализируем имена
            tableColumnNames = new string[] { "Название", "Цена" };
            tableFields = new string[] { "name", "price" };

            //Создаем фильтр по названию
            filter = new SqlFilter(SqlLogicalOperator.And);
            nameFilterGroup = new SqlFilter(SqlLogicalOperator.And);
            filter.AddItem(nameFilterGroup);
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



            Text = "Список услуг";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];

            //Добавляем компоненты фильтра
            tlsLblServiceName = new ToolStripLabel("Название");
            tlsFilter.Items.Add(tlsLblServiceName);
            
            tlsTxtServiceName = new ToolStripTextBox();
            tlsTxtServiceName.AutoSize = false;
            tlsTxtServiceName.Width = 200;
            tlsFilter.Items.Add(tlsTxtServiceName);

            //Добавляем события
            AddAddEvent(btnAdd_Click);
            AddEditEvent(btnEdit_Click);
            AddRemoveEvent(btnRemove_Click);
            AddSearchEvent(btnSearch_Click);
            AddSortChangeEvent(tblSortChange_Event);

            //Подгружаем начальные данные
            reloadData();
        }



        //Перезагрузка данных в таблицу
        protected override void reloadData(bool resetPageIndex = false) {
            listData.Update(resetPageIndex ? 0 : tblPager.GetPage());
            servicesList = Service.GetList(listData);

            if (servicesList.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);
        }

        //Загрузка данных услуги в строку таблицы
        private void loadDataToRow(int rowIndex, Service service) {
            table.Rows[rowIndex].Cells[0].Value = service.Name;
            table.Rows[rowIndex].Cells[1].Value = service.Price.ToString();
        }

        //Загрузка данных услуг в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.List.Count;
            for (int i = 0; i < listData.List.Count; i++)
                loadDataToRow(i, servicesList[i]);
        }



        //Событие клика по кнопке Добавление услуги
        private void btnAdd_Click(object sender, EventArgs e) {
            Service service = new Service(connection);

            ServiceEditForm serviceEditForm = new ServiceEditForm(service);
            serviceEditForm.Text = "Добавление новой услуги";

            if (serviceEditForm.ShowDialog() == DialogResult.OK) {
                servicesList.Insert(0, service);
                table.Rows.Insert(0, 1);
                table.Rows[0].DefaultCellStyle.BackColor = AppConfig.LightOrangeColor;
                loadDataToRow(0, service);
            }
        }

        //Событие клика по кнопке Редактирование сотрудника
        private void btnEdit_Click(object sender, EventArgs e) {
            Service service = servicesList[table.CurrentCell.RowIndex].Clone();

            ServiceEditForm serviceEditForm = new ServiceEditForm(service);
            serviceEditForm.Text = "Редактирование услуги";

            if (serviceEditForm.ShowDialog() == DialogResult.OK) {
                loadDataToRow(table.CurrentCell.RowIndex, service);
                servicesList[table.CurrentCell.RowIndex] = service;
            }
        }

        //Событие клика по кнопке Удаление сотрудника
        private void btnRemove_Click(object sender, EventArgs e) {
            int index = table.CurrentCell.RowIndex;
            Service service = servicesList[index];

            if (MessageBox.Show("Вы дейсвительно хотите удалить услугу?", "Удаление услуги", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                service.Remove();
                reloadData();
            }
        }

        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtServiceName.Text.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Создаем и добавляем условия для фильтра по имени 
            nameFilterGroup.Clear();
            for (int i = 0; i < names.Length; i++) {
                SqlFilterCondition nameFilter = new SqlFilterCondition(Service.GetFieldName("name"), SqlComparisonOperator.Like);
                nameFilter.SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);
                nameFilterGroup.AddItem(nameFilter);
            }

            reloadData(true);
        }

        //Событие сортировки таблицы по полю
        private void tblSortChange_Event(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;

            sorterItem.SetField(Service.GetTableName(), tableFields[e.ColumnIndex]);

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
