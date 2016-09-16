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
        private NumericBoxWrapper nbwPrice;             //Поле Цена услуги

        private ListBoxWrapper lbwWorkers;              //Поле Сотрудники предоставляющие услугу



        //Конструктор
        public ServiceEditForm(Service service) : base() {
            InitializeComponent();

            connection = service.GetConnection();
            this.service = service;

            Panel panel = GetPanel();

            lbwWorkers = new ListBoxWrapper("Сотрудники предоставляющие услугу", new ListBox());
            lbwWorkers.Dock = DockStyle.Top;
            lbwWorkers.Parent = panel;
            lbwWorkers.AddAddEvent(btnAddWorker_Event);
            lbwWorkers.AddRemoveEvent(btnRemoveWorker_Event);

            nbwPrice = new NumericBoxWrapper("Цена", new NumericUpDown(), 1);
            nbwPrice.Dock = DockStyle.Top;
            nbwPrice.Parent = panel;

            tbwName = new TextBoxWrapper("Название", new TextBox());
            tbwName.Dock = DockStyle.Top;
            tbwName.Parent = panel;

            panel.TabIndex = 0;
            toolsPanel.TabIndex = 1;
            FormUtils.UpdateTabIndex(panel, FormUtils.UpdateTabIndex(toolsPanel, 2));
        }



        public DialogResult ShowDialog() {
            if (!AssignService(service))
                return DialogResult.Abort;
            return base.ShowDialog();
        }



        //Привязывает услугу к форме, подгружает данные в форму
        public bool AssignService(Service service) {
            tbwName.SetValue(service.Name);
            nbwPrice.SetValue(service.Price.ToString());

            ListData workersData = Worker.GetServiceWorkersListData(service.GetId(), service.GetConnection(), 25);
            if (workersData.Update() == null)
                return false;
            serviceWorkers = Worker.GetList(workersData);
            serviceRemovedWorkers = new List<Worker>();

            ListBox lstBoxWorkers = (lbwWorkers.CtrlField as ListBox);
            lstBoxWorkers.SuspendLayout();
            lstBoxWorkers.Items.Clear();
            foreach (Worker workerService in serviceWorkers)
                lstBoxWorkers.Items.Add(workerService.GetFullName());
            lstBoxWorkers.ResumeLayout();

            return true;
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            FieldWrapper[] requiredTextBoxes = new FieldWrapper[] {
                tbwName, nbwPrice
            };

            foreach (FieldWrapper field in requiredTextBoxes)
                if (field.GetValue().Trim().Length > 0)
                    field.HideError();
                else {
                    field.ShowError("Поле обязательно для заполнения");
                    success = false;
                }

            return success;
        }

        //Сохраняет услугу
        protected override bool Save() {
            service.Name = tbwName.GetValue();
            service.Price = Int32.Parse(nbwPrice.GetValue());

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
            ListData workersListData = Worker.GetListData(connection, AppConfig.BaseLimit, 0, avalaibleWorkersFilter);
            WorkerSelectForm workerSelectForm = new WorkerSelectForm(workersListData);

            if (workerSelectForm.ShowDialog() == DialogResult.OK) {
                //Добавляем выбранных сотрудников в список
                List<Worker> selectedWorkers = workerSelectForm.GetSelected();
                serviceWorkers.AddRange(selectedWorkers);

                //Добавляем выбранных сотрудников в список на форме
                ListBox lstBoxWorkers = (lbwWorkers.CtrlField as ListBox);
                lstBoxWorkers.SuspendLayout();
                foreach (Worker serviceWorker in selectedWorkers)
                    lstBoxWorkers.Items.Add(serviceWorker.GetFullName());
                lstBoxWorkers.ResumeLayout();
            }
        }

        //Событие клика на кнопку Удалить сотрудника
        private void btnRemoveWorker_Event(object sender, EventArgs e) {
            ListBox lstBoxWorkers = (lbwWorkers.CtrlField as ListBox);
            if (lstBoxWorkers.SelectedIndex == -1)
                return;

            serviceRemovedWorkers.Add(serviceWorkers[lstBoxWorkers.SelectedIndex]);
            serviceWorkers.RemoveAt(lstBoxWorkers.SelectedIndex);
            lstBoxWorkers.Items.RemoveAt(lstBoxWorkers.SelectedIndex);
        }

    }

}
