using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования услуги
    public partial class ServiceEditForm : EntityEditForm {

        private DBConnection connection;                //Соединение с базой

        private Service service;                        //Редактируемая услуга
        private List<Worker> serviceWorkers;            //Сотрудники предоставляющие услугу
        private List<Worker> serviceRemovedWorkers;     //Удаленные сотрудники предоставляющие услугу

        private TextBoxWrapper tbwName;                 //Поле Название услуги
        private TextBoxWrapper tbwPrice;                //Поле Цена услуги

        private ListBoxWrapper lbwWorkers;              //Поле Сотрудники предоставляющие услугу



        //Конструктор
        public ServiceEditForm(Service service) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

            lbwWorkers = new ListBoxWrapper("Сотрудники предоставляющие услугу", new ListBox());
            lbwWorkers.Dock = DockStyle.Top;
            lbwWorkers.Parent = panel;
            lbwWorkers.AddAddEvent(btnAddWorker_Event);
            lbwWorkers.AddRemoveEvent(btnRemoveService_Event);

            tbwPrice = new TextBoxWrapper("Цена", new TextBox());
            tbwPrice.Dock = DockStyle.Top;
            tbwPrice.Parent = panel;

            tbwName = new TextBoxWrapper("Название", new TextBox());
            tbwName.Dock = DockStyle.Top;
            tbwName.Parent = panel;

            //Подгружаем данные услуги
            AssignService(service);
        }



        //Привязывает услугу к форме, подгружает данные в форму
        public void AssignService(Service service) {
            connection = service.GetConnection();
            this.service = service;

            tbwName.SetValue(service.Name);
            tbwPrice.SetValue(service.Price.ToString());

            ListData workersData = Worker.GetServiceWorkersListData(service.GetId(), service.GetConnection(), 25);
            workersData.Update();
            serviceWorkers = Worker.GetList(workersData);
            serviceRemovedWorkers = new List<Worker>();

            ListBox workersList = (lbwWorkers.CtrlField as ListBox);
            workersList.SuspendLayout();
            workersList.Items.Clear();
            foreach (Worker workerService in serviceWorkers)
                workersList.Items.Add(workerService.GetFullName());
            workersList.ResumeLayout();
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            TextBoxWrapper[] requiredTextBoxes = new TextBoxWrapper[] {
                tbwName, tbwPrice
            };

            foreach (TextBoxWrapper field in requiredTextBoxes)
                if (field.GetValue().Trim().Length > 0)
                    field.HideError();
                else {
                    field.ShowError("Поле обязательно для заполнения");
                    success = false;
                }

            if (!tbwPrice.HasError()) {
                int parsedPrice;
                if (!Int32.TryParse(tbwPrice.GetValue(), out parsedPrice))
                    tbwPrice.ShowError("Неверное значение");
            }

            return success;
        }

        //Сохраняет услугу
        protected override bool Save() {
            service.Name = tbwName.GetValue();
            service.Price = Int32.Parse(tbwPrice.GetValue());

            connection.StartTransaction();

            if (service.Save() == -1)
                connection.RollbackTransaction();

            else if (service.RemoveWorkers(serviceRemovedWorkers) == -1)
                connection.RollbackTransaction();

            else if (service.AddWorkers(serviceWorkers) == -1)
                connection.RollbackTransaction();

            else if (connection.CommitTransaction())
                return true;

            return true;
        }



        //Событие клика на кнопку Добавить сотрудника
        private void btnAddWorker_Event(object sender, EventArgs e) {
            //Создаем фильтр для отсечения уже добавленных сотрудников
            SqlFilter avalaibleWorkersFilter = new SqlFilter(SqlLogicalOperator.And);
            if (serviceWorkers.Count > 0) {
                SqlFilterCondition selectedFilterCondition = new SqlFilterCondition(Worker.GetFieldName("id"), SqlComparisonOperator.NotIn);

                string[] serviceWorkersIds = new string[serviceWorkers.Count];
                for (int i = 0; i < serviceWorkers.Count; i++)
                    serviceWorkersIds[i] = serviceWorkers[i].GetId().ToString();
                selectedFilterCondition.SetValue(QueryBuilder.BuildInStatement(serviceWorkersIds));
                
                avalaibleWorkersFilter.AddItem(selectedFilterCondition);
            }

            //Создаем форму для добавления сотрудников
            ListData workersListData = Worker.GetListData(connection, 25, 0, avalaibleWorkersFilter);
            WorkerSelectForm workerSelectForm = new WorkerSelectForm(workersListData);

            if (workerSelectForm.ShowDialog() == DialogResult.OK) {
                //Добавляем выбранных сотрудников в список
                List<Worker> selectedWorkers = workerSelectForm.GetSelected();
                serviceWorkers.AddRange(selectedWorkers);

                //Добавляем выбранных сотрудников в список на форме
                ListBox lstBoxService = (lbwWorkers.CtrlField as ListBox);
                lstBoxService.SuspendLayout();
                foreach (Worker serviceWorker in selectedWorkers)
                    lstBoxService.Items.Add(serviceWorker.GetFullName());
                lstBoxService.ResumeLayout();
            }
        }

        //Событие клика на кнопку Удалить сотрудника
        private void btnRemoveService_Event(object sender, EventArgs e) {
            ListBox lstBoxService = (lbwWorkers.CtrlField as ListBox);
            serviceRemovedWorkers.Add(serviceWorkers[lstBoxService.SelectedIndex]);
            serviceWorkers.RemoveAt(lstBoxService.SelectedIndex);
            lstBoxService.Items.RemoveAt(lstBoxService.SelectedIndex);
        }

    }

}
