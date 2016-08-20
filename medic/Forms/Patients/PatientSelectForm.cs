using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using medic.Database;

namespace medic.Forms {

    //Форма выбора списка пациентов
    public partial class PatientSelectForm : EntitySelectForm {

        private ListData listData;                           //Данные списка пациентов
        private List<Patient> workersList;                   //Список пациентов
        private Dictionary<int, Patient> selectedPatients;   //Список выбранных пациентов

        private SqlFilter filter;                           //Фильтр дл запросов
        private SqlFilterCondition[] fullNameFilter;        //Условия фильтра по имени
        
        private SqlSorter sorter;                           //Сортировка для запросов

        private bool lockSelectUpdate;

        private ToolStripLabel tlsLblPatientName;       //Подпись к полю ФИО
        private ToolStripTextBox tlsTxtPatientName;     //Поле ФИО



        //Конструктор
        public PatientSelectForm(ListData initialListData) : base(initialListData.Connection) {
            InitializeComponent();
            Text = "Выбор пациентов";

            this.connection = initialListData.Connection;

            selectedPatients = new Dictionary<int, Patient>();
           
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

            //Создаем сортировщик по имени
            sorter = new SqlSorter();
            sorter.AddItems(new SqlSorterCondition[] {
                new SqlSorterCondition(Patient.GetTableName(), "first_name", SqlOrder.Asc),
                new SqlSorterCondition(Patient.GetTableName(), "last_name", SqlOrder.Asc),
                new SqlSorterCondition(Patient.GetTableName(), "middle_name", SqlOrder.Asc),
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
            tlsLblPatientName = new ToolStripLabel("ФИО");
            tlsFilter.Items.Add(tlsLblPatientName);

            tlsTxtPatientName = new ToolStripTextBox();
            tlsTxtPatientName.AutoSize = false;
            tlsTxtPatientName.Width = 200;
            tlsFilter.Items.Add(tlsTxtPatientName);

            //Добавляем события
            AddSearchEvent(btnSearch_Click);
            AddCheckEvent(lstCheck_Event);

            //Подгружаем начальные данные
            reloadData();
        }



        //Возвращает список выбранных пациентов
        public List<Patient> GetSelected() {
            return selectedPatients.Values.ToList<Patient>();
        }



        //Перезагрузка данных в список
        protected override void reloadData(bool resetPageIndex = false) {
            lockSelectUpdate = true;

            listData.Update(resetPageIndex ? 0 : tblPager.GetPage());
            workersList = Patient.GetList(listData);

            list.Items.Clear();
            foreach (Patient worker in workersList)
                list.Items.Add(worker.GetFullName(), selectedPatients.ContainsKey(worker.GetId()));

            tblPager.SetData(listData.Count, listData.Pages, listData.PageIndex);

            lockSelectUpdate = false;
        }



        //Событие клика по кнопке Найти
        private void btnSearch_Click(object sender, EventArgs e) {
            string[] names = tlsTxtPatientName.Text.Trim().Split(" ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < fullNameFilter.Length; i++)
                fullNameFilter[i].SetValue(i < names.Length ? "\"%" + QueryBuilder.EscapeLikeString(names[i]) + "%\"" : null);

            reloadData(true);
        }

        //Событие изменения состояния флажков в списке
        private void lstCheck_Event(object sender, ItemCheckEventArgs e) {
            if (lockSelectUpdate) return;

            Patient worker = workersList[e.Index];

            if (e.NewValue == CheckState.Checked)
                selectedPatients.Add(worker.GetId(), worker);
            else
                selectedPatients.Remove(worker.GetId());
        }

    }

}
