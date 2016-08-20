using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма выбора списка услуг
    public partial class ServiceSelectForm : EntitySelectForm {

        private ListData listData;                          //Данные списка услуг
        private List<Service> servicesList;                 //Список услуг
        private Dictionary<int, Service> selectedServices;  //Список выбранных услуг

        private SqlFilter filter;                           //Фильтр для запросов
        private SqlFilter nameFilterGroup;                  //Условия фильтра

        private SqlSorter sorter;                           //Сортировка для запросов

        private bool lockSelectUpdate;                      //Блокировка события изменения состояния флажков
        
        private ToolStripLabel tlsLblServiceName;           //Подпись к полю название
        private ToolStripTextBox tlsTxtServiceName;         //Поле Название



        //Конструктор
        public ServiceSelectForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();
            Text = "Выбор услуг";

            selectedServices = new Dictionary<int, Service>();

            //Создаем фильтр по названию
            filter = new SqlFilter(SqlLogicalOperator.And);
            nameFilterGroup = new SqlFilter(SqlLogicalOperator.And);
            filter.AddItem(nameFilterGroup);
            filter.AddItem(initialListData.Filter);

            //Создаем сортировщик по названию
            sorter = new SqlSorter();
            sorter.AddItem(new SqlSorterCondition(Service.GetTableName(), "name", SqlOrder.Asc));
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
            tlsLblServiceName = new ToolStripLabel("Название");
            tlsFilter.Items.Add(tlsLblServiceName);

            tlsTxtServiceName = new ToolStripTextBox();
            tlsTxtServiceName.AutoSize = false;
            tlsTxtServiceName.Width = 200;
            tlsFilter.Items.Add(tlsTxtServiceName);

            //Добавляем события
            AddSearchEvent(btnSearch_Click);
            AddCheckEvent(lstCheck_Event);
        }



        //Возвращает список выбранных услуг
        public List<Service> GetSelected() {
            return selectedServices.Values.ToList<Service>();
        }



        //Перезагрузка данных в список
        protected override bool reloadData(bool resetPageIndex = false) {
            lockSelectUpdate = true;

            if (listData.Update(resetPageIndex ? 0 : tblPager.GetPage()) == null) {
                DialogResult = DialogResult.Abort;
                lockSelectUpdate = false;
                Close();
                return false;
            }
            servicesList = Service.GetList(listData);

            list.Items.Clear();
            foreach (Service service in servicesList)
                list.Items.Add(service.Name, selectedServices.ContainsKey(service.GetId()));

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            lockSelectUpdate = false;

            return true;
        }



        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtServiceName.Text.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Создаем условия для фильтра по имени 
            nameFilterGroup.Clear();
            for (int i = 0; i < names.Length; i++) {
                SqlFilterCondition nameFilter = new SqlFilterCondition(Service.GetFieldName("name"), SqlComparisonOperator.Like);
                nameFilter.SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);
                nameFilterGroup.AddItem(nameFilter);
            }

            reloadData(true);
        }

        //Событие изменения состояния флажков в списке
        private void lstCheck_Event(object sender, ItemCheckEventArgs e) {
            if (lockSelectUpdate) return;

            Service service = servicesList[e.Index];

            if (e.NewValue == CheckState.Checked)
                selectedServices.Add(service.GetId(), service);
            else
                selectedServices.Remove(service.GetId());
        }

    }

}
