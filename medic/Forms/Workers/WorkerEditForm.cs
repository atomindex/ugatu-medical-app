using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования сотрудника
    public partial class WorkerEditForm : EntityEditForm {

        private DBConnection connection;                    //Соединение с базой

        private Worker worker;                              //Редактируемый сотрудник

        private List<Specialty> workerSpecialties;          //Специальности сотрудника
        private List<Specialty> workerRemovedSpecialties;   //Удаленные специальности сотрудника

        private List<Service> workerServices;               //Услуги предоставляемые сотрудником
        private List<Service> workerRemovedServices;        //Удаленные услуги предоставляемые сотрудником

        private TextBoxWrapper tbwFirstName;                //Поле Имя
        private TextBoxWrapper tbwLastName;                 //Поле Фамилия
        private TextBoxWrapper tbwMiddleName;               //Поле Отчество
        private TextBoxWrapper tbwPhone;                    //Поле Телефон
        private TextBoxWrapper tbwAddress;                  //Поле Адрес

        private ListBoxWrapper lbwSpecialties;              //Поле Специальности
        private ListBoxWrapper lbwServices;                 //Поле Предоставляемые услуги



        //Конструктор
        public WorkerEditForm(Worker worker) : base() {
            InitializeComponent();

            connection = worker.GetConnection();
            this.worker = worker;

            Panel panel = GetPanel();

            lbwServices = new ListBoxWrapper("Предоставляемые услуги", new ListBox());
            lbwServices.Dock = DockStyle.Top;
            lbwServices.Parent = panel;
            lbwServices.AddAddEvent(btnAddService_Event);
            lbwServices.AddRemoveEvent(btnRemoveService_Event);

            lbwSpecialties = new ListBoxWrapper("Специальности", new ListBox());
            lbwSpecialties.Dock = DockStyle.Top;
            lbwSpecialties.Parent = panel;
            lbwSpecialties.AddAddEvent(btnAddSpecialty_Event);
            lbwSpecialties.AddRemoveEvent(btnRemoveSpecialty_Event);

            tbwAddress = new TextBoxWrapper("Адрес", new TextBox());
            tbwAddress.Dock = DockStyle.Top;
            tbwAddress.Parent = panel;

            tbwPhone = new TextBoxWrapper("Телефон", new TextBox());
            tbwPhone.Dock = DockStyle.Top;
            tbwPhone.Parent = panel;

            tbwMiddleName = new TextBoxWrapper("Отчество", new TextBox());
            tbwMiddleName.Dock = DockStyle.Top;
            tbwMiddleName.Parent = panel;

            tbwFirstName = new TextBoxWrapper("Имя", new TextBox());
            tbwFirstName.Dock = DockStyle.Top;
            tbwFirstName.Parent = panel;

            tbwLastName = new TextBoxWrapper("Фамилия", new TextBox());
            tbwLastName.Dock = DockStyle.Top;
            tbwLastName.Parent = panel;

            panel.TabIndex = 0;
            toolsPanel.TabIndex = 1;
            FormUtils.UpdateTabIndex(panel, FormUtils.UpdateTabIndex(toolsPanel, 2));
        }



        public DialogResult ShowDialog() {
            if (!AssignWorker(worker))
                return DialogResult.Abort;
            return base.ShowDialog();
        }



        //Привязывает сотрудника к форме, подгружает данные в форму
        public bool AssignWorker(Worker worker) {
            tbwFirstName.SetValue(worker.FirstName);
            tbwLastName.SetValue(worker.LastName);
            tbwMiddleName.SetValue(worker.MiddleName);
            tbwPhone.SetValue(worker.Phone);
            tbwAddress.SetValue(worker.Address);

            ListData specialtiesData = Specialty.GetWorkerSpecialtiesListData(worker.GetId(), worker.GetConnection());
            if (specialtiesData.Update() == null)
                return false;
            workerSpecialties = Specialty.GetList(specialtiesData);
            workerRemovedSpecialties = new List<Specialty>();

            ListData servicesData = Service.GetWorkerServicesListData(worker.GetId(), worker.GetConnection());
            if (servicesData.Update() == null)
                return false;
            workerServices = Service.GetList(servicesData);
            workerRemovedServices = new List<Service>();

            //Подгружаем данные 
            ListBox lstBoxSpecialties = (lbwSpecialties.CtrlField as ListBox);
            lstBoxSpecialties.SuspendLayout();
            lstBoxSpecialties.Items.Clear();
            foreach (Specialty workerSpecialty in workerSpecialties)
                lstBoxSpecialties.Items.Add(workerSpecialty.Name);
            lstBoxSpecialties.ResumeLayout();

            ListBox lstBoxServices = (lbwServices.CtrlField as ListBox);
            lstBoxServices.SuspendLayout();
            lstBoxServices.Items.Clear();
            foreach(Service workerService in workerServices)
                lstBoxServices.Items.Add(workerService.Name);
            lstBoxServices.ResumeLayout();

            return true;
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

            else if (worker.RemoveSpecialties(workerRemovedSpecialties) == -1)
                connection.RollbackTransaction();

            else if (worker.AddSpecialties(workerSpecialties) == -1)
                connection.RollbackTransaction();

            else if (connection.CommitTransaction())
                return true;

            return false;
        }


        //Событие клика на кнопку Добавить специальность
        private void btnAddSpecialty_Event(object sender, EventArgs e) {
            //Создаем фильтр для отсечения уже добавленных услуг
            SqlFilter selectedFilter = new SqlFilter(SqlLogicalOperator.And);
            if (workerSpecialties.Count > 0) {
                SqlFilterCondition selectedFilterCondition = new SqlFilterCondition(Specialty.GetFieldName("id"), SqlComparisonOperator.NotIn);

                string[] workerSpecialtiesIds = new string[workerSpecialties.Count];
                for (int i = 0; i < workerSpecialties.Count; i++)
                    workerSpecialtiesIds[i] = workerSpecialties[i].GetId().ToString();
                selectedFilterCondition.SetValue(QueryBuilder.BuildInStatement(workerSpecialtiesIds));

                selectedFilter.AddItem(selectedFilterCondition);
            }

            //Создаем формы для добавления услуг
            ListData specialtiesListData = Specialty.GetListData(connection, AppConfig.BaseLimit, 0, selectedFilter);
            SpecialtySelectForm specialtySelectForm = new SpecialtySelectForm(specialtiesListData);

            if (specialtySelectForm.ShowDialog() == DialogResult.OK) {
                //Добавление выбранных услуг в список
                List<Specialty> selectedSpetialties = specialtySelectForm.GetSelected();
                workerSpecialties.AddRange(selectedSpetialties);

                //Добавляем выбранных услуг в список на форме
                ListBox lstBoxSpecialties = (lbwSpecialties.CtrlField as ListBox);
                lstBoxSpecialties.SuspendLayout();
                foreach (Specialty workerSpecialty in selectedSpetialties)
                    lstBoxSpecialties.Items.Add(workerSpecialty.Name);
                lstBoxSpecialties.ResumeLayout();
            }
        }

        //Событие клика на кнопку Удалить специальность
        private void btnRemoveSpecialty_Event(object sender, EventArgs e) {
            ListBox lstBoxSpecialties = (lbwSpecialties.CtrlField as ListBox);
            if (lstBoxSpecialties.SelectedIndex == -1)
                return;

            workerRemovedSpecialties.Add(workerSpecialties[lstBoxSpecialties.SelectedIndex]);
            workerSpecialties.RemoveAt(lstBoxSpecialties.SelectedIndex);
            lstBoxSpecialties.Items.RemoveAt(lstBoxSpecialties.SelectedIndex);
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
            ListData servicesListData = Service.GetListData(connection, AppConfig.BaseLimit, 0, selectedFilter);
            ServiceSelectForm serviceSelectForm = new ServiceSelectForm(servicesListData);

            if (serviceSelectForm.ShowDialog() == DialogResult.OK) {
                //Добавление выбранных услуг в список
                List<Service> selectedServices = serviceSelectForm.GetSelected();
                workerServices.AddRange(selectedServices);

                //Добавляем выбранных услуг в список на форме
                ListBox lstBoxServices = (lbwServices.CtrlField as ListBox);
                lstBoxServices.SuspendLayout();
                foreach(Service workerService in selectedServices)
                    lstBoxServices.Items.Add(workerService.Name);
                lstBoxServices.ResumeLayout();
            }
        }

        //Событие клика на кнопку Удалить услугу
        private void btnRemoveService_Event(object sender, EventArgs e) {
            ListBox lstBoxServices = (lbwServices.CtrlField as ListBox);
            if (lstBoxServices.SelectedIndex == -1)
                return;

            workerRemovedServices.Add(workerServices[lstBoxServices.SelectedIndex]);
            workerServices.RemoveAt(lstBoxServices.SelectedIndex);
            lstBoxServices.Items.RemoveAt(lstBoxServices.SelectedIndex);
        }

    }

}
