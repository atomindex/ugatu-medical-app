using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования скидки
    public partial class SaleEditForm : EntityEditForm {

        private DBConnection connection;                    //Соединение с базой

        private Sale sale;                                  //Редактируемая скидка

        private TextBoxWrapper tbwName;                     //Поле Название скидки
        private TextBoxWrapper tbwDescription;              //Поле Описание скидки
        private NumericBoxWrapper nbwPercent;               //Поле Процент скидки

        private ComboTextBoxWrapper ctbwCondVisitNumber;



        //Конструктор
        public SaleEditForm(Sale sale) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

            ctbwCondVisitNumber = new ComboTextBoxWrapper("Количество посещений", Sale.Operators, new TextBox());
            ctbwCondVisitNumber.Dock = DockStyle.Top;
            ctbwCondVisitNumber.Parent = panel;

            nbwPercent = new NumericBoxWrapper("Процент скидки", new NumericUpDown(), 1, 100);
            nbwPercent.Dock = DockStyle.Top;
            nbwPercent.Parent = panel;

            tbwDescription = new TextBoxWrapper("Описание", new TextBox());
            tbwDescription.Dock = DockStyle.Top;
            tbwDescription.Parent = panel;

            tbwName = new TextBoxWrapper("Название", new TextBox());
            tbwName.Dock = DockStyle.Top;
            tbwName.Parent = panel;

            panel.TabIndex = 0;
            toolsPanel.TabIndex = 1;
            FormUtils.UpdateTabIndex(panel, FormUtils.UpdateTabIndex(toolsPanel, 2));

            //Подгружаем данные скидки
            AssignService(sale);
        }



        //Привязывает услугу к форме, подгружает данные в форму
        public void AssignService(Sale sale) {
            connection = sale.GetConnection();
            this.sale = sale;

            tbwName.SetValue(sale.Name);
            tbwDescription.SetValue(sale.Description);
            nbwPercent.SetValue(sale.Percent.ToString());

            ctbwCondVisitNumber.SetComboValue(sale.CondVisitNumber.op);
            ctbwCondVisitNumber.SetValue(sale.CondVisitNumber.value);            
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            FieldWrapper[] requiredTextBoxes = new FieldWrapper[] {
                tbwName, tbwDescription, nbwPercent
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
            sale.Name = tbwName.GetValue();
            sale.Description = tbwDescription.GetValue();
            sale.Percent = Int32.Parse(nbwPercent.GetValue());

            sale.CondVisitNumber.op = Sale.Operators[ctbwCondVisitNumber.GetComboIndex()];
            sale.CondVisitNumber.value = ctbwCondVisitNumber.GetValue();

            return sale.Save() != -1;
        }

    }

}
