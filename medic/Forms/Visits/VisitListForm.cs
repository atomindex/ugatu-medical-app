using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма списка посещений
    public partial class VisitListForm : EntityListForm {

        private string[] tableColumnNames;              //Список имен колонок
        private string[] tableFields;                   //Список полей соответвующих колонкам

        private ListData listData;                      //Данные списка посещений
        private List<Visit> visitsList;                 //Список посещений

        private SqlFilter filter;                       //Фильтр для запросов
        private SqlFilterCondition[] fullNameFilter;    //Условия фильтра

        private SqlSorter sorter;                       //Сортировка для запросов
        private SqlSorterCondition sorterItem;          //Поля сортировки

        private ToolStripLabel tlsLblVisitName;        //Подпись к полю ФИО
        private ToolStripTextBox tlsTxtVisitName;      //Поле ФИО



        //Конструктор
        public VisitListForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();

            btnAdd.Visible = false;

            //Инициализируем имена
            tableColumnNames = new string[] { "ФИО", "Дата посещения", "Цена" };
            tableFields = new string[] { "CONCAT(visits.last_name, ' ', visits.first_name, ' ', visit.middle_name)", "visits.visit_date", "visits.price" };

            //Создаем фильтр по имени
            filter = new SqlFilter(SqlLogicalOperator.And);
            string fullNameFilterField = QueryBuilder.BuildConcatStatement(
                Patient.GetTableName(),
                new string[] { "first_name", "last_name", "middle_name" }
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


            Text = "Список посещений";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];

            //Добавляем компоненты фильтра
            tlsLblVisitName = new ToolStripLabel("ФИО");
            tlsFilter.Items.Add(tlsLblVisitName);
            
            tlsTxtVisitName = new ToolStripTextBox();
            tlsTxtVisitName.AutoSize = false;
            tlsTxtVisitName.Width = 200;
            tlsFilter.Items.Add(tlsTxtVisitName);
            
            //Добавляем события
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
            visitsList = Visit.GetList(listData);

            if (visitsList.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);
        }

        //Загрузка данных посещения в строку таблицы
        private void loadDataToRow(int rowIndex, Visit visit) {
            table.Rows[rowIndex].Cells[0].Value = visit.RelatedPatient.GetFullName();
            table.Rows[rowIndex].Cells[1].Value = visit.VisitDate.ToString(AppConfig.DateFormat);
            table.Rows[rowIndex].Cells[2].Value = visit.Price.ToString(); 
        }

        //Загрузка данных посещений в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.List.Count;
            for (int i = 0; i < listData.List.Count; i++)
                loadDataToRow(i, visitsList[i]);
        }



        //Событие клика по кнопке Редактирование посещения
        private void btnEdit_Click(object sender, EventArgs e) {
            if (visitsList.Count == 0)
                return;

            Visit visit = visitsList[table.CurrentCell.RowIndex].Clone();

            VisitEditForm visitEditForm = new VisitEditForm(visit);
            visitEditForm.Text = "Редактирование посещения";

            if (visitEditForm.ShowDialog() == DialogResult.OK) {
                loadDataToRow(table.CurrentCell.RowIndex, visit);
                visitsList[table.CurrentCell.RowIndex] = visit;
            }
        }

        //Событие клика по кнопке Удаление посещения
        private void btnRemove_Click(object sender, EventArgs e) {
            if (visitsList.Count == 0)
                return;

            Visit visit = visitsList[table.CurrentCell.RowIndex];

            if (MessageBox.Show("Вы дейсвительно хотите удалить посещение?", "Удаление посещения", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                visit.Remove();
                reloadData();
            }
        }

        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtVisitName.Text.Trim().Split(" ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fullNameFilter.Length; i++)
                fullNameFilter[i].SetValue(i < names.Length ? "\"%"+QueryBuilder.EscapeLikeString(names[i])+"%\"" : null);

            reloadData(true);
        }

        //Событие сортировки таблицы по полю
        private void tblSortChange_Event(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;

            sorterItem.SetField(Visit.GetTableName(), tableFields[e.ColumnIndex]);

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
