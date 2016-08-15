using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма выбора списка категорий
    public partial class CategorySelectForm : EntitySelectForm {

        private ListData listData;                                   //Данные списка категорий
        private List<Category> categoriesList;                       //Список категорий
        private Dictionary<int, Category> selectedPatientCategories; //Список выбранных категорий

        private SqlFilter filter;                                   //Фильтр для запросов
        private SqlFilter nameFilterGroup;                          //Условия фильтра

        private SqlSorter sorter;                                   //Сортировка для запросов

        private bool lockSelectUpdate;                              //Блокировка события изменения состояния флажков
        
        private ToolStripLabel tlsLblPatientCategoryName;           //Подпись к полю название
        private ToolStripTextBox tlsTxtPatientCategoryName;         //Поле Название



        //Конструктор
        public CategorySelectForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();
            Text = "Выбор категорий";

            selectedPatientCategories = new Dictionary<int, Category>();

            //Создаем фильтр по названию
            filter = new SqlFilter(SqlLogicalOperator.And);
            nameFilterGroup = new SqlFilter(SqlLogicalOperator.And);
            filter.AddItem(nameFilterGroup);
            filter.AddItem(initialListData.Filter);

            //Создаем сортировщик по названию
            sorter = new SqlSorter();
            sorter.AddItem(new SqlSorterCondition(Category.GetTableName(), "name", SqlOrder.Asc));
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


            //Добавляем компоненты фильтра
            tlsLblPatientCategoryName = new ToolStripLabel("Название");
            tlsFilter.Items.Add(tlsLblPatientCategoryName);

            tlsTxtPatientCategoryName = new ToolStripTextBox();
            tlsTxtPatientCategoryName.AutoSize = false;
            tlsTxtPatientCategoryName.Width = 200;
            tlsFilter.Items.Add(tlsTxtPatientCategoryName);

            //Добавляем события
            AddSearchEvent(btnSearch_Click);
            AddCheckEvent(lstCheck_Event);

            //Подгружаем начальные данные
            reloadData();
        }



        //Возвращает список выбранных категорий
        public List<Category> GetSelected() {
            return selectedPatientCategories.Values.ToList<Category>();
        }



        //Перезагрузка данных в список
        protected override void reloadData(bool resetPageIndex = false) {
            lockSelectUpdate = true;

            listData.Update(resetPageIndex ? 0 : tblPager.GetPage());
            categoriesList = Category.GetList(listData);

            list.Items.Clear();
            foreach (Category service in categoriesList)
                list.Items.Add(service.Name, selectedPatientCategories.ContainsKey(service.GetId()));

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            lockSelectUpdate = false;
        }



        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtPatientCategoryName.Text.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Создаем условия для фильтра по имени 
            nameFilterGroup.Clear();
            for (int i = 0; i < names.Length; i++) {
                SqlFilterCondition nameFilter = new SqlFilterCondition(Category.GetFieldName("name"), SqlComparisonOperator.Like);
                nameFilter.SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);
                nameFilterGroup.AddItem(nameFilter);
            }

            reloadData(true);
        }

        //Событие изменения состояния флажков в списке
        private void lstCheck_Event(object sender, ItemCheckEventArgs e) {
            if (lockSelectUpdate) return;

            Category service = categoriesList[e.Index];

            if (e.NewValue == CheckState.Checked)
                selectedPatientCategories.Add(service.GetId(), service);
            else
                selectedPatientCategories.Remove(service.GetId());
        }

    }

}
