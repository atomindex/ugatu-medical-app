using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования сотрудника
    public partial class WorkerEditForm : EntityEditForm {

        private DBConnection connection;                //Соединение с базой

        private Worker worker;                          //Редактируемый сотрудник
        private List<Service> workerServices;           //Услуги предоставляемые сотрудником
        private List<Service> workerRemovedServices;    //Удаленные услуги предоставляемые сотрудником

        private TextBoxWrapper tbwFirstName;            //Поле Имя
        private TextBoxWrapper tbwLastName;             //Поле Фамилия
        private TextBoxWrapper tbwMiddleName;           //Поле Отчество
        private TextBoxWrapper tbwPhone;                //Поле Телефон
        private TextBoxWrapper tbwAddress;              //Поле Адрес

        private ListBoxWrapper lbwServices;             //Поле Предоставляемые услуги



        //Конструктор
        public WorkerEditForm(Worker worker) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

            lbwServices = new ListBoxWrapper("Предоставляемые услуги", new ListBox());
            lbwServices.Dock = DockStyle.Top;
            lbwServices.Parent = panel;
            lbwServices.AddAddEvent(btnAddService_Event);
            lbwServices.AddRemoveEvent(btnRemoveService_Event);

            tbwAddress = new TextBoxWrapper("Адрес", new TextBox());
            tbwAddress.Dock = DockStyle.Top;
            tbwAddress.Parent = panel;

            tbwPhone = new TextBoxWrapper("Телефон", new TextBox());
            tbwPhone.Dock = DockStyle.Top;
            tbwPhone.Parent = panel;

            tbwMiddleName = new TextBoxWrapper("Отчество", new TextBox());
            tbwMiddleName.Dock = DockStyle.Top;
            tbwMiddleName.Parent = panel;

            tbwLastName = new TextBoxWrapper("Фамилия", new TextBox());
            tbwLastName.Dock = DockStyle.Top;
            tbwLastName.Parent = panel;

            tbwFirstName = new TextBoxWrapper("Имя", new TextBox());
            tbwFirstName.Dock = DockStyle.Top;
            tbwFirstName.Parent = panel;

            //Подгружаем данные сотрудника
            AssignWorker(worker);
        }



        //Привязывает сотрудника к форме, подгружает данные в форму
        public void AssignWorker(Worker worker) {
            this.connection = worker.GetConnection();
            this.worker = worker;

            tbwFirstName.SetValue(worker.FirstName);
            tbwLastName.SetValue(worker.LastName);
            tbwMiddleName.SetValue(worker.MiddleName);
            tbwPhone.SetValue(worker.Phone);
            tbwAddress.SetValue(worker.Address);

            ListData servicesData = Service.GetWorkerServicesListData(worker.GetId(), worker.GetConnection());
            servicesData.Update();
            workerServices = Service.GetList(servicesData);
            workerRemovedServices = new List<Service>(); 

            ListBox serviceList = (lbwServices.CtrlField as ListBox);
            serviceList.SuspendLayout();
            serviceList.Items.Clear();
            foreach(Service workerService in workerServices)
                serviceList.Items.Add(workerService.Name);
            serviceList.ResumeLayout();
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            TextBoxWrapper[] requiredTextBoxes = new TextBoxWrapper[] {
                tbwFirstName, tbwLastName, tbwMiddleName, tbwPhone, tbwAddress
            };

            foreach (TextBoxWrapper field in requiredTextBoxes)
                if (field.GetValue().Trim().Length > 0)
                    field.HideError();
                else {
                    field.ShowError("Поле обязательно для заполнения");
                    success = false;
                }

            return success;
        }

        //Сохраняет сотрудника
        protected override bool Save() {
            worker.FirstName = tbwFirstName.GetValue();
            worker.MiddleName = tbwMiddleName.GetValue();
            worker.LastName = tbwLastName.GetValue();
            worker.Address = tbwAddress.GetValue();
            worker.Phone = tbwPhone.GetValue();

            connection.StartTransaction();

            if (worker.Save() == -1)
                connection.RollbackTransaction();

            else if (worker.RemoveServices(workerRemovedServices) == -1)
                connection.RollbackTransaction();

            else if (worker.AddServices(workerServices) == -1)
                connection.RollbackTransaction();

            else if (connection.CommitTransaction())
                return true;

            return false;
        }

        

        //Событие клика на кнопку Добавить услугу
        private void btnAddService_Event(object sender, EventArgs e) {
            //Создаем фильтр для отсечения уже добавленных услуг
            SqlFilter selectedFilter = new SqlFilter(SqlLogicalOperator.And);
            if (workerServices.Count > 0) {
                SqlFilterCondition selectedFilterCondition = new SqlFilterCondition(Service.GetFieldName("id"), SqlComparisonOperator.NotIn);
                
                string[] workerServicesIds = new string[workerServices.Count];
                for (int i = 0; i < workerServices.Count; i++)
                    workerServicesIds[i] = workerServices[i].GetId().ToString();
                selectedFilterCondition.SetValue(QueryBuilder.BuildInStatement(workerServicesIds));
                
                selectedFilter.AddItem(selectedFilterCondition);
            }

            //Создаем формы для добавления услуг
            ListData servicesListData = Service.GetListData(connection, 25, 0, selectedFilter);
            ServiceSelectForm serviceSelectForm = new ServiceSelectForm(servicesListData);

            if (serviceSelectForm.ShowDialog() == DialogResult.OK) {
                //Добавление выбранных услуг в список
                List<Service> selectedServices = serviceSelectForm.GetSelected();
                workerServices.AddRange(selectedServices);

                //Добавляем выбранных услуг в список на форме
                ListBox lstBoxService = (lbwServices.CtrlField as ListBox);
                lstBoxService.SuspendLayout();
                foreach(Service workerService in selectedServices)
                    lstBoxService.Items.Add(workerService.Name);
                lstBoxService.ResumeLayout();
            }
        }

        //Событие клика на кнопку Удалить услугу
        private void btnRemoveService_Event(object sender, EventArgs e) {
            ListBox lstBoxService = (lbwServices.CtrlField as ListBox);
            workerRemovedServices.Add(workerServices[lstBoxService.SelectedIndex]);
            workerServices.RemoveAt(lstBoxService.SelectedIndex);
            lstBoxService.Items.RemoveAt(lstBoxService.SelectedIndex);
        }

    }

}
