using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования специальности
    public partial class SpecialtyEditForm : EntityEditForm {

        private DBConnection connection;                //Соединение с базой

        private Specialty specialty;                    //Редактируемая специальность
        private List<Worker> specialtyWorkers;          //Сотрудники имеющие специальность
        private List<Worker> specialtyRemovedWorkers;   //Удаленные сотрудники имеющие специальность

        private TextBoxWrapper tbwName;                 //Поле Название специальности

        private ListBoxWrapper lbwWorkers;              //Поле Сотрудники имеющие специальность



        //Конструктор
        public SpecialtyEditForm(Specialty specialty) : base() {
            InitializeComponent();

            connection = specialty.GetConnection();
            this.specialty = specialty;

            Panel panel = GetPanel();

            lbwWorkers = new ListBoxWrapper("Сотрудники имеющие специальность", new ListBox());
            lbwWorkers.Dock = DockStyle.Top;
            lbwWorkers.Parent = panel;
            lbwWorkers.AddAddEvent(btnAddWorker_Event);
            lbwWorkers.AddRemoveEvent(btnRemoveWorker_Event);

            tbwName = new TextBoxWrapper("Название", new TextBox());
            tbwName.Dock = DockStyle.Top;
            tbwName.Parent = panel;

            panel.TabIndex = 0;
            toolsPanel.TabIndex = 1;
            FormUtils.UpdateTabIndex(panel, FormUtils.UpdateTabIndex(toolsPanel, 2));
        }



        public DialogResult ShowDialog() {
            if (!AssignSpecialty(specialty))
                return DialogResult.Abort;
            return base.ShowDialog();
        }



        //Привязывает специальность к форме, подгружает данные в форму
        public bool AssignSpecialty(Specialty specialty) {
            tbwName.SetValue(specialty.Name);

            ListData workersData = Worker.GetSpecialtyWorkersListData(specialty.GetId(), specialty.GetConnection(), 25);
            if (workersData.Update() == null)
                return false;
            specialtyWorkers = Worker.GetList(workersData);
            specialtyRemovedWorkers = new List<Worker>();

            ListBox lstBoxWorkers = (lbwWorkers.CtrlField as ListBox);
            lstBoxWorkers.SuspendLayout();
            lstBoxWorkers.Items.Clear();
            foreach (Worker workerSpecialty in specialtyWorkers)
                lstBoxWorkers.Items.Add(workerSpecialty.GetFullName());
            lstBoxWorkers.ResumeLayout();

            return true;
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            TextBoxWrapper[] requiredTextBoxes = new TextBoxWrapper[] {
                tbwName
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

        //Сохраняет специальность
        protected override bool Save() {
            specialty.Name = tbwName.GetValue();

            connection.StartTransaction();

            if (specialty.Save() == -1)
                connection.RollbackTransaction();

            else if (specialty.RemoveWorkers(specialtyRemovedWorkers) == -1)
                connection.RollbackTransaction();

            else if (specialty.AddWorkers(specialtyWorkers) == -1)
                connection.RollbackTransaction();

            else if (connection.CommitTransaction())
                return true;

            return true;
        }



        //Событие клика на кнопку Добавить сотрудника
        private void btnAddWorker_Event(object sender, EventArgs e) {
            //Создаем фильтр для отсечения уже добавленных сотрудников
            SqlFilter avalaibleWorkersFilter = new SqlFilter(SqlLogicalOperator.And);
            if (specialtyWorkers.Count > 0) {
                SqlFilterCondition selectedFilterCondition = new SqlFilterCondition(Worker.GetFieldName("id"), SqlComparisonOperator.NotIn);

                string[] specialtyWorkersIds = new string[specialtyWorkers.Count];
                for (int i = 0; i < specialtyWorkers.Count; i++)
                    specialtyWorkersIds[i] = specialtyWorkers[i].GetId().ToString();
                selectedFilterCondition.SetValue(QueryBuilder.BuildInStatement(specialtyWorkersIds));
                
                avalaibleWorkersFilter.AddItem(selectedFilterCondition);
            }

            //Создаем форму для добавления сотрудников
            ListData workersListData = Worker.GetListData(connection, 25, 0, avalaibleWorkersFilter);
            WorkerSelectForm workerSelectForm = new WorkerSelectForm(workersListData);

            if (workerSelectForm.ShowDialog() == DialogResult.OK) {
                //Добавляем выбранных сотрудников в список
                List<Worker> selectedWorkers = workerSelectForm.GetSelected();
                specialtyWorkers.AddRange(selectedWorkers);

                //Добавляем выбранных сотрудников в список на форме
                ListBox lstBoxWorkers = (lbwWorkers.CtrlField as ListBox);
                lstBoxWorkers.SuspendLayout();
                foreach (Worker specialtyWorker in selectedWorkers)
                    lstBoxWorkers.Items.Add(specialtyWorker.GetFullName());
                lstBoxWorkers.ResumeLayout();
            }
        }

        //Событие клика на кнопку Удалить сотрудника
        private void btnRemoveWorker_Event(object sender, EventArgs e) {
            ListBox lstBoxWorkers = (lbwWorkers.CtrlField as ListBox);
            if (lstBoxWorkers.SelectedIndex == -1)
                return;

            specialtyRemovedWorkers.Add(specialtyWorkers[lstBoxWorkers.SelectedIndex]);
            specialtyWorkers.RemoveAt(lstBoxWorkers.SelectedIndex);
            lstBoxWorkers.Items.RemoveAt(lstBoxWorkers.SelectedIndex);
        }

    }

}
