using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма выбора списка сотрудников
    public partial class WorkerSelectForm : EntitySelectForm {

        private ListData listData;                          //Данные списка сотрудников
        private List<Worker> workersList;                   //Список сотрудников
        private Dictionary<int, Worker> selectedWorkers;    //Список выбранных сотрудников

        private SqlFilter filter;                           //Фильтр дл запросов
        private SqlFilterCondition[] fullNameFilter;        //Условия фильтра по имени
        
        private SqlSorter sorter;                           //Сортировка для запросов

        private bool lockSelectUpdate;

        private ToolStripLabel tlsLblWorkerName;       //Подпись к полю ФИО
        private ToolStripTextBox tlsTxtWorkerName;     //Поле ФИО



        //Конструктор
        public WorkerSelectForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();
            Text = "Выбор сотрудников";

            this.connection = initialListData.Connection;

            selectedWorkers = new Dictionary<int, Worker>();
           
            //Создаем фильтр по имени
            filter = new SqlFilter(SqlLogicalOperator.And);
            string fullNameFilterField = QueryBuilder.BuildConcatStatement(
                Worker.GetTableName(),
                new string[] { "first_name", "last_name", "middle_name" }
            );
            fullNameFilter = new SqlFilterCondition[] {
                new SqlFilterCondition(fullNameFilterField, SqlComparisonOperator.Like),
                new SqlFilterCondition(fullNameFilterField, SqlComparisonOperator.Like),
                new SqlFilterCondition(fullNameFilterField, SqlComparisonOperator.Like)
            };
            filter.AddItems(fullNameFilter);
            filter.AddItem(initialListData.Filter);

            //Создаем сортировщик по имени
            sorter = new SqlSorter();
            sorter.AddItems(new SqlSorterCondition[] {
                new SqlSorterCondition(Worker.GetTableName(), "first_name", SqlOrder.Asc),
                new SqlSorterCondition(Worker.GetTableName(), "last_name", SqlOrder.Asc),
                new SqlSorterCondition(Worker.GetTableName(), "middle_name", SqlOrder.Asc),
            });
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
            tlsLblWorkerName = new ToolStripLabel("ФИО");
            tlsFilter.Items.Add(tlsLblWorkerName);

            tlsTxtWorkerName = new ToolStripTextBox();
            tlsTxtWorkerName.AutoSize = false;
            tlsTxtWorkerName.Width = 200;
            tlsFilter.Items.Add(tlsTxtWorkerName);

            //Добавляем события
            AddSearchEvent(btnSearch_Click);
            AddCheckEvent(lstCheck_Event);

            //Подгружаем начальные данные
            reloadData();
        }



        //Возвращает список выбранных сотрудников
        public List<Worker> GetSelected() {
            return selectedWorkers.Values.ToList<Worker>();
        }



        //Перезагрузка данных в список
        protected override void reloadData(bool resetPageIndex = false) {
            lockSelectUpdate = true;

            listData.Update(resetPageIndex ? 0 : tblPager.GetPage());
            workersList = Worker.GetList(listData);

            list.Items.Clear();
            foreach (Worker worker in workersList)
                list.Items.Add(worker.GetFullName(), selectedWorkers.ContainsKey(worker.GetId()));

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            lockSelectUpdate = false;
        }



        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtWorkerName.Text.Trim().Split(" ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fullNameFilter.Length; i++)
                fullNameFilter[i].SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);

            reloadData(true);
        }

        //Событие изменения состояния флажков в списке

        private void lstCheck_Event(object sender, ItemCheckEventArgs e) {
            if (lockSelectUpdate) return;

            Worker worker = workersList[e.Index];

            if (e.NewValue == CheckState.Checked)
                selectedWorkers.Add(worker.GetId(), worker);
            else
                selectedWorkers.Remove(worker.GetId());
        }

    }

}
