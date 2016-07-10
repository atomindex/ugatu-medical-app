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
    public partial class ServiceEditForm : EntityEditForm {
        Service service;

        TextBoxWrapper name;        //Название услуги
        TextBoxWrapper price;       //Цена услуги



        public ServiceEditForm(Service service) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

            price = new TextBoxWrapper("Цена", new TextBox());
            price.Dock = DockStyle.Top;
            price.Parent = panel;

            name = new TextBoxWrapper("Название", new TextBox());
            name.Dock = DockStyle.Top;
            name.Parent = panel;

            AssignService(service);
        }

        public void AssignService(Service service) {
            this.service = service;

            name.SetValue(service.Name);
            price.SetValue(service.Price.ToString());
        }

        protected override bool Validate() {
            bool success = true;

            TextBoxWrapper[] requiredTextBoxes = new TextBoxWrapper[] {
                name, price
            };

            foreach (TextBoxWrapper field in requiredTextBoxes)
                if (field.GetValue().Trim().Length > 0)
                    field.HideError();
                else {
                    field.ShowError("Поле обязательно для заполнения");
                    success = false;
                }

            if (!price.HasError()) {
                int parsedPrice;
                if (!Int32.TryParse(price.GetValue(), out parsedPrice))
                    price.ShowError("Неверное значение");
            }

            return success;
        }

        protected override void Save() {
            service.Name = name.GetValue();
            service.Price = Int32.Parse(price.GetValue());

            service.Save();
        }
    }
}
