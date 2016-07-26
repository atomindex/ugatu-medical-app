using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования сотрудника
    public partial class PatientEditForm : EntityEditForm {

        private DBConnection connection;                    //Соединение с базой

        private Patient patient;                              //Редактируемый сотрудник

        private TextBoxWrapper tbwFirstName;                //Поле Имя
        private TextBoxWrapper tbwLastName;                 //Поле Фамилия
        private TextBoxWrapper tbwMiddleName;               //Поле Отчество
        private TextBoxWrapper tbwSex;                      //Поле Пол
        private TextBoxWrapper tbwBirthday;                 //Поле Дата рождения



        //Конструктор
        public PatientEditForm(Patient patient) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

            tbwBirthday = new TextBoxWrapper("Дата рождения", new TextBox());
            tbwBirthday.Dock = DockStyle.Top;
            tbwBirthday.Parent = panel;

            tbwSex = new TextBoxWrapper("Пол", new TextBox());
            tbwSex.Dock = DockStyle.Top;
            tbwSex.Parent = panel;

            tbwMiddleName = new TextBoxWrapper("Отчество", new TextBox());
            tbwMiddleName.Dock = DockStyle.Top;
            tbwMiddleName.Parent = panel;

            tbwFirstName = new TextBoxWrapper("Имя", new TextBox());
            tbwFirstName.Dock = DockStyle.Top;
            tbwFirstName.Parent = panel;

            tbwLastName = new TextBoxWrapper("Фамилия", new TextBox());
            tbwLastName.Dock = DockStyle.Top;
            tbwLastName.Parent = panel;

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
            tbwSex.SetValue(patient.Sex);
            tbwBirthday.SetValue(patient.Birthday);
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            TextBoxWrapper[] requiredTextBoxes = new TextBoxWrapper[] {
                tbwFirstName, tbwLastName, tbwMiddleName, tbwSex, tbwBirthday
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
            patient.Birthday = tbwBirthday.GetValue();
            patient.Sex = tbwSex.GetValue();

            return patient.Save() != -1;
        }

    }

}
