using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма списка скидок
    public partial class SaleListForm : EntityListForm {

        private string[] tableColumnNames;              //Список имен колонок
        private string[] tableFields;                   //Список полей соответвующих колонкам

        private ListData listData;                      //Данные списка скидок
        private List<Sale> salesList;                   //Список скидок

        private SqlFilter filter;                       //Фильтр для запросов
        private SqlFilter nameFilterGroup;              //Условия фильтра

        private SqlSorter sorter;                       //Сортировка для запросов
        private SqlSorterCondition sorterItem;          //Поля сортировки

        private ToolStripLabel tlsLblSaleName;          //Подпись к полю название
        private ToolStripTextBox tlsTxtSaleName;        //Поле Название



        //Конструктор
        public SaleListForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();

            //Инициализируем имена
            tableColumnNames = new string[] { "Название", "Процент" };
            tableFields = new string[] { "name", "percent" };

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



            Text = "Список скидок";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];

            //Добавляем компоненты фильтра
            tlsLblSaleName = new ToolStripLabel("Название");
            tlsFilter.Items.Add(tlsLblSaleName);
            
            tlsTxtSaleName = new ToolStripTextBox();
            tlsTxtSaleName.AutoSize = false;
            tlsTxtSaleName.Width = 200;
            tlsFilter.Items.Add(tlsTxtSaleName);

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
            salesList = Sale.GetList(listData);

            if (salesList.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            return true;
        }

        //Загрузка данных скидки в строку таблицы
        private void loadDataToRow(int rowIndex, Sale sale) {
            table.Rows[rowIndex].Cells[0].Value = sale.Description;
            table.Rows[rowIndex].Cells[1].Value = sale.Percent.ToString();
        }

        //Загрузка данных скидок в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.List.Count;
            for (int i = 0; i < listData.List.Count; i++)
                loadDataToRow(i, salesList[i]);
        }



        //Событие клика по кнопке Добавление скидки
        private void btnAdd_Click(object sender, EventArgs e) {
            Sale sale = new Sale(connection);

            SaleEditForm saleEditForm = new SaleEditForm(sale);
            saleEditForm.Text = "Добавление новой скидки";

            if (saleEditForm.ShowDialog() == DialogResult.OK) {
                if (salesList.Count > 0)
                    table.Rows.Insert(0, 1);
                
                table.Rows[0].DefaultCellStyle.BackColor = AppConfig.LightOrangeColor;
                loadDataToRow(0, sale);

                salesList.Insert(0, sale);
            }
        }

        //Событие клика по кнопке Редактирование сотрудника
        private void btnEdit_Click(object sender, EventArgs e) {
            if (salesList.Count == 0)
                return;

            Sale sale = salesList[table.CurrentCell.RowIndex].Clone();

            SaleEditForm saleEditForm = new SaleEditForm(sale);
            saleEditForm.Text = "Редактирование скидки";

            if (saleEditForm.ShowDialog() == DialogResult.OK) {
                loadDataToRow(table.CurrentCell.RowIndex, sale);
                salesList[table.CurrentCell.RowIndex] = sale;
            }
        }

        //Событие клика по кнопке Удаление сотрудника
        private void btnRemove_Click(object sender, EventArgs e) {
            if (salesList.Count == 0)
                return;

            Sale sale = salesList[table.CurrentCell.RowIndex];

            if (MessageBox.Show("Вы дейсвительно хотите удалить скидку?", "Удаление скидки", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                sale.Remove();
                reloadData();
            }
        }

        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtSaleName.Text.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Создаем и добавляем условия для фильтра по имени 
            nameFilterGroup.Clear();
            for (int i = 0; i < names.Length; i++) {
                SqlFilterCondition nameFilter = new SqlFilterCondition(Sale.GetFieldName("name"), SqlComparisonOperator.Like);
                nameFilter.SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);
                nameFilterGroup.AddItem(nameFilter);
            }

            reloadData(true);
        }

        //Событие сортировки таблицы по полю
        private void tblSortChange_Event(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;

            sorterItem.SetField(Sale.GetTableName(), tableFields[e.ColumnIndex]);

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
