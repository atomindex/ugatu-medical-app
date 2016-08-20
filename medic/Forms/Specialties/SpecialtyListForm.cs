using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма списка специальностей
    public partial class SpecialtyListForm : EntityListForm {

        private string[] tableColumnNames;                //Список имен колонок
        private string[] tableFields;                     //Список полей соответвующих колонкам

        private ListData listData;                        //Данные списка специальностей
        private List<Specialty> specialtiesList;          //Список специальностей

        private SqlFilter filter;                         //Фильтр для запросов
        private SqlFilter nameFilterGroup;                //Условия фильтра

        private SqlSorter sorter;                         //Сортировка для запросов
        private SqlSorterCondition sorterItem;            //Поля сортировки

        private ToolStripLabel tlsLblSpecialtyName;       //Подпись к полю название
        private ToolStripTextBox tlsTxtSpecialtyName;     //Поле Название



        //Конструктор
        public SpecialtyListForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();

            //Инициализируем имена
            tableColumnNames = new string[] { "Название" };
            tableFields = new string[] { "name" };

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



            Text = "Список специальностей";

            //Инициализируем столбцы таблицы
            table.ColumnCount = tableColumnNames.Length;
            for (int i = 0; i < tableColumnNames.Length; i++)
                table.Columns[i].HeaderText = tableColumnNames[i];

            //Добавляем компоненты фильтра
            tlsLblSpecialtyName = new ToolStripLabel("Название");
            tlsFilter.Items.Add(tlsLblSpecialtyName);
            
            tlsTxtSpecialtyName = new ToolStripTextBox();
            tlsTxtSpecialtyName.AutoSize = false;
            tlsTxtSpecialtyName.Width = 200;
            tlsFilter.Items.Add(tlsTxtSpecialtyName);

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
            specialtiesList = Specialty.GetList(listData);

            if (specialtiesList.Count == 0) {
                table.RowCount = 1;
                ClearTable(table);
            } else {
                loadDataToTable();
                ClearTableColor(table);
            }

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            return true;
        }

        //Загрузка данных специальности в строку таблицы
        private void loadDataToRow(int rowIndex, Specialty specialty) {
            table.Rows[rowIndex].Cells[0].Value = specialty.Name;
        }

        //Загрузка данных специальностей в таблицу
        private void loadDataToTable() {
            table.RowCount = listData.List.Count;
            for (int i = 0; i < listData.List.Count; i++)
                loadDataToRow(i, specialtiesList[i]);
        }



        //Событие клика по кнопке Добавление специальности
        private void btnAdd_Click(object sender, EventArgs e) {
            Specialty specialty = new Specialty(connection);

            SpecialtyEditForm specialtyEditForm = new SpecialtyEditForm(specialty);
            specialtyEditForm.Text = "Добавление новой специальности";

            if (specialtyEditForm.ShowDialog() == DialogResult.OK) {
                if (specialtiesList.Count > 0)
                    table.Rows.Insert(0, 1);
                
                table.Rows[0].DefaultCellStyle.BackColor = AppConfig.LightOrangeColor;
                loadDataToRow(0, specialty);

                specialtiesList.Insert(0, specialty);
            }
        }

        //Событие клика по кнопке Редактирование сотрудника
        private void btnEdit_Click(object sender, EventArgs e) {
            if (specialtiesList.Count == 0)
                return;

            Specialty specialty = specialtiesList[table.CurrentCell.RowIndex].Clone();

            SpecialtyEditForm specialtyEditForm = new SpecialtyEditForm(specialty);
            specialtyEditForm.Text = "Редактирование специальности";

            if (specialtyEditForm.ShowDialog() == DialogResult.OK) {
                loadDataToRow(table.CurrentCell.RowIndex, specialty);
                specialtiesList[table.CurrentCell.RowIndex] = specialty;
            }
        }

        //Событие клика по кнопке Удаление сотрудника
        private void btnRemove_Click(object sender, EventArgs e) {
            if (specialtiesList.Count == 0)
                return;

            Specialty specialty = specialtiesList[table.CurrentCell.RowIndex];

            if (MessageBox.Show("Вы дейсвительно хотите удалить специальность?", "Удаление специальности", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                specialty.Remove();
                reloadData();
            }
        }

        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtSpecialtyName.Text.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Создаем и добавляем условия для фильтра по имени 
            nameFilterGroup.Clear();
            for (int i = 0; i < names.Length; i++) {
                SqlFilterCondition nameFilter = new SqlFilterCondition(Specialty.GetFieldName("name"), SqlComparisonOperator.Like);
                nameFilter.SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);
                nameFilterGroup.AddItem(nameFilter);
            }

            reloadData(true);
        }

        //Событие сортировки таблицы по полю
        private void tblSortChange_Event(object sender, DataGridViewCellMouseEventArgs e) {
            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;

            sorterItem.SetField(Specialty.GetTableName(), tableFields[e.ColumnIndex]);

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
