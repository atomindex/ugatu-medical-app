using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования сотрудника
    public partial class PatientEditForm : EntityEditForm {

        private DBConnection connection;                    //Соединение с базой

        private Patient patient;                            //Редактируемый сотрудник

        private TextBoxWrapper tbwFirstName;                //Поле Имя
        private TextBoxWrapper tbwLastName;                 //Поле Фамилия
        private TextBoxWrapper tbwMiddleName;               //Поле Отчество
        private ComboBoxWrapper cbwSex;                     //Поле Пол
        private DatepickerWrapper dpwBirthday;              //Поле Дата рождения



        //Конструктор
        public PatientEditForm(Patient patient) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

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

            //Подгружаем данные сотрудника
            AssignPatient(patient);
        }



        //Привязывает сотрудника к форме, подгружает данные в форму
        public void AssignPatient(Patient patient) {
            this.connection = patient.GetConnection();
            this.patient = patient;

            tbwFirstName.SetValue(patient.FirstName);
            tbwLastName.SetValue(patient.LastName);
            tbwMiddleName.SetValue(patient.MiddleName);
            cbwSex.SetValue(patient.Sex);
            dpwBirthday.SetDate(patient.Birthday);
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            FieldWrapper[] requiredTextBoxes = new FieldWrapper[] {
                tbwFirstName, tbwLastName, tbwMiddleName, cbwSex, dpwBirthday
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
            patient.FirstName = tbwFirstName.GetValue();
            patient.MiddleName = tbwMiddleName.GetValue();
            patient.LastName = tbwLastName.GetValue();
            patient.Birthday = dpwBirthday.GetDate();
            patient.Sex = cbwSex.GetValue();

            return patient.Save() != -1;
        }

    }

}
