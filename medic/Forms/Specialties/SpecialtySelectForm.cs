using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма выбора списка специальностей
    public partial class SpecialtySelectForm : EntitySelectForm {

        private ListData listData;                               //Данные списка специальностей
        private List<Specialty> specialtiesList;                 //Список специальностей
        private Dictionary<int, Specialty> selectedSpecialties;  //Список выбранных специальностей

        private SqlFilter filter;                                //Фильтр для запросов
        private SqlFilter nameFilterGroup;                       //Условия фильтра

        private SqlSorter sorter;                                //Сортировка для запросов

        private bool lockSelectUpdate;                           //Блокировка события изменения состояния флажков
        
        private ToolStripLabel tlsLblSpecialtyName;              //Подпись к полю название
        private ToolStripTextBox tlsTxtSpecialtyName;            //Поле Название



        //Конструктор
        public SpecialtySelectForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();

            selectedSpecialties = new Dictionary<int, Specialty>();

            //Создаем фильтр по названию
            filter = new SqlFilter(SqlLogicalOperator.And);
            nameFilterGroup = new SqlFilter(SqlLogicalOperator.And);
            filter.AddItem(nameFilterGroup);
            filter.AddItem(initialListData.Filter);

            //Создаем сортировщик по названию
            sorter = new SqlSorter();
            sorter.AddItem(new SqlSorterCondition(Specialty.GetTableName(), "name", SqlOrder.Asc));
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
            tlsLblSpecialtyName = new ToolStripLabel("Название");
            tlsFilter.Items.Add(tlsLblSpecialtyName);

            tlsTxtSpecialtyName = new ToolStripTextBox();
            tlsTxtSpecialtyName.AutoSize = false;
            tlsTxtSpecialtyName.Width = 200;
            tlsFilter.Items.Add(tlsTxtSpecialtyName);

            //Добавляем события
            AddSearchEvent(btnSearch_Click);
            AddCheckEvent(lstCheck_Event);

            //Подгружаем начальные данные
            reloadData();
        }



        //Возвращает список выбранных специальностей
        public List<Specialty> GetSelected() {
            return selectedSpecialties.Values.ToList<Specialty>();
        }



        //Перезагрузка данных в список
        protected override void reloadData(bool resetPageIndex = false) {
            lockSelectUpdate = true;

            listData.Update(resetPageIndex ? 0 : tblPager.GetPage());
            specialtiesList = Specialty.GetList(listData);

            list.Items.Clear();
            foreach (Specialty specialty in specialtiesList)
                list.Items.Add(specialty.Name, selectedSpecialties.ContainsKey(specialty.GetId()));

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            lockSelectUpdate = false;
        }



        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtSpecialtyName.Text.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Создаем условия для фильтра по имени 
            nameFilterGroup.Clear();
            for (int i = 0; i < names.Length; i++) {
                SqlFilterCondition nameFilter = new SqlFilterCondition(Specialty.GetFieldName("name"), SqlComparisonOperator.Like);
                nameFilter.SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);
                nameFilterGroup.AddItem(nameFilter);
            }

            reloadData(true);
        }

        //Событие изменения состояния флажков в списке
        private void lstCheck_Event(object sender, ItemCheckEventArgs e) {
            if (lockSelectUpdate) return;

            Specialty specialty = specialtiesList[e.Index];

            if (e.NewValue == CheckState.Checked)
                selectedSpecialties.Add(specialty.GetId(), specialty);
            else
                selectedSpecialties.Remove(specialty.GetId());
        }

    }

}
