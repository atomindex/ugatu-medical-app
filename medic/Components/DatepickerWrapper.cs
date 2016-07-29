using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {
    
    //Класс выбора даты с надписями
    class DatepickerWrapper : FieldWrapper {

        //Конструктор
        public DatepickerWrapper(string labelText, DateTimePicker field) : base(labelText, field) {
            field.Format = DateTimePickerFormat.Custom;
            field.CustomFormat = AppConfig.DateFormat;

            DateTime now = DateTime.Now;
            field.MinDate = AppConfig.MinDate;
            field.MaxDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }



        //Возвращает значение поля ввода
        public override string GetValue() {
            return (CtrlField as DateTimePicker).Value.ToString(AppConfig.DateFormat);
        }

        //Возвращает значение поля ввода
        public DateTime GetDate() {
            return (CtrlField as DateTimePicker).Value;
        }

        //Устанавливает значение поля ввода
        public override void SetValue(string value) {
            DateTimePicker dtp = CtrlField as DateTimePicker;
            DateTime result;

            if (value != null && value.Length > 0 && DateTime.TryParseExact(value, AppConfig.DateFormat, null, System.Globalization.DateTimeStyles.None, out result))
                dtp.Value = result;
            else
                dtp.Value = DateTime.Now;
        }

        //Устанавливает значение поля ввода
        public void SetDate(DateTime value) {
            DateTimePicker dtp = CtrlField as DateTimePicker;

            if (value < dtp.MinDate)
                dtp.Value = DateTime.Now < dtp.MinDate ? dtp.MinDate : DateTime.Now;
            else if (value > dtp.MaxDate)
                dtp.Value = dtp.MaxDate;
            else 
                dtp.Value = value;
        }

    }

}
