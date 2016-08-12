using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования скидки
    public partial class SaleEditForm : EntityEditForm {

        private DBConnection connection;                            //Соединение с базой

        private Sale sale;                                          //Редактируемая скидка

        private TextBoxWrapper tbwName;                             //Поле Название скидки
        private TextBoxWrapper tbwDescription;                      //Поле Описание скидки
        private NumericBoxWrapper nbwPercent;                       //Поле Процент скидки

        public ComboNumericBoxWrapper ctbwCondServicesCount;        //Поле Количество услуг
        public ComboNumericBoxWrapper ctbwCondServicesSumPrice;     //Поле Общая цена услуг
        public ComboNumericBoxWrapper ctbwCondPatientAge;           //Поле Возраст пациента
        public ComboNumericBoxWrapper ctbwCondPatientSex;           //Поле Пол пациента
        public ComboNumericBoxWrapper ctbwCondVisitNumber;          //Поле Количество посещений
        public ComboDatetimeWrapper ctbwCondVisitDate;              //Поле Дата посещения


        //Конструктор
        public SaleEditForm(Sale sale) : base() {
            InitializeComponent();

            Panel panel = GetPanel();

            //Создаем поле Дата посещения
            ctbwCondVisitDate = new ComboDatetimeWrapper("Дата посещения", SaleConditionDatetime.Operators, new DateTimePicker());
            ctbwCondVisitDate.Dock = DockStyle.Top;
            ctbwCondVisitDate.Parent = panel;

            //Создаем поле Количество посещений
            ctbwCondVisitNumber = new ComboNumericBoxWrapper("Количество посещений", SaleConditionInt.Operators, new NumericUpDown());
            ctbwCondVisitNumber.Dock = DockStyle.Top;
            ctbwCondVisitNumber.Parent = panel;

            //Создаем поле  Пол пациента
            ctbwCondPatientSex = new ComboNumericBoxWrapper("Пол пациента", SaleConditionInt.Operators, new NumericUpDown());
            ctbwCondPatientSex.Dock = DockStyle.Top;
            ctbwCondPatientSex.Parent = panel;

            //Создаем поле Возраст пациента
            ctbwCondPatientAge = new ComboNumericBoxWrapper("Возраст пациента", SaleConditionInt.Operators, new NumericUpDown());
            ctbwCondPatientAge.Dock = DockStyle.Top;
            ctbwCondPatientAge.Parent = panel;

            //Создаем поле Общая цена услуг
            ctbwCondServicesSumPrice = new ComboNumericBoxWrapper("Общая цена услуг", SaleConditionInt.Operators, new NumericUpDown());
            ctbwCondServicesSumPrice.Dock = DockStyle.Top;
            ctbwCondServicesSumPrice.Parent = panel;

            //Создаем поле Количество услуг
            ctbwCondServicesCount = new ComboNumericBoxWrapper("Количество услуг", SaleConditionInt.Operators, new NumericUpDown());
            ctbwCondServicesCount.Dock = DockStyle.Top;
            ctbwCondServicesCount.Parent = panel;


            //Создаем поле Процент скидки
            nbwPercent = new NumericBoxWrapper("Процент скидки", new NumericUpDown(), 1, 100);
            nbwPercent.Dock = DockStyle.Top;
            nbwPercent.Parent = panel;

            //Создаем поле Описание
            tbwDescription = new TextBoxWrapper("Описание", new TextBox());
            tbwDescription.Dock = DockStyle.Top;
            tbwDescription.Parent = panel;

            //Создаем поле Название
            tbwName = new TextBoxWrapper("Название", new TextBox());
            tbwName.Dock = DockStyle.Top;
            tbwName.Parent = panel;

            //Устанавливаем tabindex
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

            //Подгружаем основные данные скидки в форму
            tbwName.SetValue(sale.Name);
            tbwDescription.SetValue(sale.Description);
            nbwPercent.SetValue(sale.Percent.ToString());

         
            //Подгружаем количество услуг в форму
            ctbwCondServicesCount.SetComboValue(sale.CondServicesCount.op);
            ctbwCondServicesCount.SetValue(sale.CondServicesCount.value.ToString());

            //Подгружаем общую цену услуг в форму
            ctbwCondServicesSumPrice.SetComboValue(sale.CondServicesSumPrice.op);
            ctbwCondServicesSumPrice.SetValue(sale.CondServicesSumPrice.value.ToString());

            //Подгружаем возраст пациента в форму
            ctbwCondPatientAge.SetComboValue(sale.CondPatientAge.op);
            ctbwCondPatientAge.SetValue(sale.CondPatientAge.value.ToString());

            //Подгружаем пол пациента в форму
            ctbwCondPatientSex.SetComboValue(sale.CondPatientSex.op);
            ctbwCondPatientSex.SetValue(sale.CondPatientSex.value.ToString());

            //Подгружаем количество посещений в форму
            ctbwCondVisitNumber.SetComboValue(sale.CondVisitNumber.op);
            ctbwCondVisitNumber.SetValue(sale.CondVisitNumber.value.ToString());

            //Подгружаем дату посещения в форму
            ctbwCondVisitDate.SetComboValue(sale.CondVisitDate.op);
            ctbwCondVisitDate.SetDate(sale.CondVisitDate.value);
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

            sale.CondVisitNumber.op = SaleConditionInt.Operators[ctbwCondVisitNumber.GetComboIndex()];
            sale.CondVisitNumber.value = Int32.Parse(ctbwCondVisitNumber.GetValue());

            //Количество услуг
            sale.CondServicesCount.op = SaleConditionInt.Operators[ctbwCondServicesCount.GetComboIndex()];
            sale.CondServicesCount.value = Int32.Parse(ctbwCondServicesCount.GetValue());

            //Общая цена услуг
            sale.CondServicesSumPrice.op = SaleConditionInt.Operators[ctbwCondServicesSumPrice.GetComboIndex()]; ;
            sale.CondServicesSumPrice.value = Int32.Parse(ctbwCondServicesSumPrice.GetValue());
            
            //Возраст пациента
            sale.CondPatientAge.op = SaleConditionInt.Operators[ctbwCondPatientAge.GetComboIndex()]; ;
            sale.CondPatientAge.value = Int32.Parse(ctbwCondPatientAge.GetValue());

            //Пол пациента
            sale.CondPatientSex.op = SaleConditionInt.Operators[ctbwCondPatientSex.GetComboIndex()]; ;
            sale.CondPatientSex.value = Int32.Parse(ctbwCondPatientSex.GetValue());

            //Количество посещений
            sale.CondVisitNumber.op = SaleConditionInt.Operators[ctbwCondVisitNumber.GetComboIndex()]; ;
            sale.CondVisitNumber.value = Int32.Parse(ctbwCondVisitNumber.GetValue());

            //Дата посещения
            sale.CondVisitDate.op = SaleConditionDatetime.Operators[ctbwCondVisitDate.GetComboIndex()]; ;
            sale.CondVisitDate.value = ctbwCondVisitDate.GetDate();

            return sale.Save() != -1;
        }

    }

}
