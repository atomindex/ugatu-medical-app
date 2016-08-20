using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования пациента
    public partial class PatientEditForm : EntityEditForm {

        private DBConnection connection;                    //Соединение с базой

        private Patient patient;                            //Редактируемый пациент

        private List<Category> patientCategories;          //Категории пациента
        private List<Category> patientRemovedCategories;   //Удаленные категории пациента

        private TextBoxWrapper tbwFirstName;                //Поле Имя
        private TextBoxWrapper tbwLastName;                 //Поле Фамилия
        private TextBoxWrapper tbwMiddleName;               //Поле Отчество
        private ComboBoxWrapper cbwSex;                     //Поле Пол
        private DatepickerWrapper dpwBirthday;              //Поле Дата рождения

        private ListBoxWrapper lbwCategories;               //Поле Категории

        //Конструктор
        public PatientEditForm(Patient patient) : base() {
            InitializeComponent();

            connection = patient.GetConnection();
            this.patient = patient;

            Panel panel = GetPanel();

            lbwCategories = new ListBoxWrapper("Категории", new ListBox());
            lbwCategories.Dock = DockStyle.Top;
            lbwCategories.Parent = panel;
            lbwCategories.AddAddEvent(btnAddCategory_Event);
            lbwCategories.AddRemoveEvent(btnRemoveCategory_Event);

            dpwBirthday = new DatepickerWrapper("Дата рождения", new DateTimePicker());
            dpwBirthday.Dock = DockStyle.Top;
            dpwBirthday.Parent = panel;

            cbwSex = new ComboBoxWrapper("Пол", new ComboBox(), Patient.SexKeys, Patient.SexValues);
            cbwSex.Dock = DockStyle.Top;
            cbwSex.Parent = panel;

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
            if (!AssignPatient(patient))
                return DialogResult.Abort;
            return base.ShowDialog();
        }



        //Привязывает пациента к форме, подгружает данные в форму
        public bool AssignPatient(Patient patient) {
            tbwFirstName.SetValue(patient.FirstName);
            tbwLastName.SetValue(patient.LastName);
            tbwMiddleName.SetValue(patient.MiddleName);
            cbwSex.SetValue(patient.Sex.ToString());
            dpwBirthday.SetDate(patient.Birthday);

            ListData categoriesData = Category.GetPatientCategoriesListData(patient.GetId(), patient.GetConnection());
            if (categoriesData.Update() == null)
                return false;
            patientCategories = Category.GetList(categoriesData);
            patientRemovedCategories = new List<Category>();

            //Подгружаем данные 
            ListBox lstBoxSpecialties = (lbwCategories.CtrlField as ListBox);
            lstBoxSpecialties.SuspendLayout();
            lstBoxSpecialties.Items.Clear();
            foreach (Category patientCategory in patientCategories)
                lstBoxSpecialties.Items.Add(patientCategory.Name);
            lstBoxSpecialties.ResumeLayout();

            return true;
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            FieldWrapper[] requiredTextBoxes = new FieldWrapper[] {
                tbwFirstName, tbwLastName, tbwMiddleName, dpwBirthday
            };

            foreach (FieldWrapper field in requiredTextBoxes)
                if (field.GetValue().Trim().Length > 0)
                    field.HideError();
                else {
                    field.ShowError("Поле обязательно для заполнения");
                    success = false;
                }

            if (cbwSex.GetValue() != "-1")
                cbwSex.HideError();
            else {
                cbwSex.ShowError("Поле обязательно для заполнения");
                success = false;
            }

            return success;
        }

        //Сохраняет пациента
        protected override bool Save() {
            patient.FirstName = tbwFirstName.GetValue();
            patient.MiddleName = tbwMiddleName.GetValue();
            patient.LastName = tbwLastName.GetValue();
            patient.Birthday = dpwBirthday.GetDate();
            patient.Sex = Int32.Parse(cbwSex.GetValue());

            connection.StartTransaction();

            if (patient.Save() == -1)
                connection.RollbackTransaction();

            else if (patient.RemoveCategories(patientRemovedCategories) == -1)
                connection.RollbackTransaction();

            else if (patient.AddCategories(patientCategories) == -1)
                connection.RollbackTransaction();

            else if (connection.CommitTransaction())
                return true;

            return false;
        }


        //Событие клика на кнопку Добавить категорию
        private void btnAddCategory_Event(object sender, EventArgs e) {
            //Создаем фильтр для отсечения уже добавленных услуг
            SqlFilter selectedFilter = new SqlFilter(SqlLogicalOperator.And);
            if (patientCategories.Count > 0) {
                SqlFilterCondition selectedFilterCondition = new SqlFilterCondition(Category.GetFieldName("id"), SqlComparisonOperator.NotIn);

                string[] patientCategoriesIds = new string[patientCategories.Count];
                for (int i = 0; i < patientCategories.Count; i++)
                    patientCategoriesIds[i] = patientCategories[i].GetId().ToString();
                selectedFilterCondition.SetValue(QueryBuilder.BuildInStatement(patientCategoriesIds));

                selectedFilter.AddItem(selectedFilterCondition);
            }

            //Создаем формы для добавления услуг
            ListData categoriesListData = Category.GetListData(connection, 25, 0, selectedFilter);
            CategorySelectForm specialtySelectForm = new CategorySelectForm(categoriesListData);

            if (specialtySelectForm.ShowDialog() == DialogResult.OK) {
                //Добавление выбранных услуг в список
                List<Category> selectedSpetialties = specialtySelectForm.GetSelected();
                patientCategories.AddRange(selectedSpetialties);

                //Добавляем выбранных услуг в список на форме
                ListBox lstBoxCategories = (lbwCategories.CtrlField as ListBox);
                lstBoxCategories.SuspendLayout();
                foreach (Category patientCategory in selectedSpetialties)
                    lstBoxCategories.Items.Add(patientCategory.Name);
                lstBoxCategories.ResumeLayout();
            }
        }

        //Событие клика на кнопку Удалить категорию
        private void btnRemoveCategory_Event(object sender, EventArgs e) {
            ListBox lstBoxCategories = (lbwCategories.CtrlField as ListBox);
            if (lstBoxCategories.SelectedIndex == -1)
                return;

            patientRemovedCategories.Add(patientCategories[lstBoxCategories.SelectedIndex]);
            patientCategories.RemoveAt(lstBoxCategories.SelectedIndex);
            lstBoxCategories.Items.RemoveAt(lstBoxCategories.SelectedIndex);
        }

    }

}
