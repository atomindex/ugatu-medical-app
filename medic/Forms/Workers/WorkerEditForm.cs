using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using medic.Components;

namespace medic.Forms {
    public partial class WorkerEditForm : EntityEditForm {
        Worker worker;

        TextBoxWrapper firstName;
        TextBoxWrapper lastName;
        TextBoxWrapper middleName;
        TextBoxWrapper phone;
        TextBoxWrapper address;



        public WorkerEditForm(Worker worker) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

            address = new TextBoxWrapper("Адрес", new TextBox());
            address.Dock = DockStyle.Top;
            address.Parent = panel;

            phone = new TextBoxWrapper("Телефон", new TextBox());
            phone.Dock = DockStyle.Top;
            phone.Parent = panel;

            middleName = new TextBoxWrapper("Отчество", new TextBox());
            middleName.Dock = DockStyle.Top;
            middleName.Parent = panel;

            lastName = new TextBoxWrapper("Фамилия", new TextBox());
            lastName.Dock = DockStyle.Top;
            lastName.Parent = panel;

            firstName = new TextBoxWrapper("Имя", new TextBox());
            firstName.Dock = DockStyle.Top;
            firstName.Parent = panel;

            AssignWorker(worker);
        }

        public void AssignWorker(Worker worker) {
            this.worker = worker;

            firstName.SetValue(worker.FirstName);
            lastName.SetValue(worker.LastName);
            middleName.SetValue(worker.MiddleName);
            phone.SetValue(worker.Phone);
            address.SetValue(worker.Address);
        }

        protected override bool Validate() {
            bool success = true;

            TextBoxWrapper[] requiredTextBoxes = new TextBoxWrapper[] {
                firstName, lastName, middleName, phone, address
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

        protected override void Save() {
            worker.FirstName = firstName.GetValue();
            worker.MiddleName = middleName.GetValue();
            worker.LastName = lastName.GetValue();
            worker.Address = address.GetValue();
            worker.Phone = phone.GetValue();

            worker.Save();
        }
    }
}
