using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {
    
    //Класс текстового поля для ввода числа
    class NumericBoxWrapper : FieldWrapper {

        //Конструктор
        public NumericBoxWrapper(string labelText, NumericUpDown field, int minValue = -1, int maxValue = -1) : base(labelText, field) {
            field.Minimum = minValue > -1 ? minValue : Int32.MinValue;
            field.Maximum = maxValue > -1 ? maxValue : Int32.MaxValue;
        }



        //Возвращает значение поля ввода
        public override string GetValue() {
            return (CtrlField as NumericUpDown).Value.ToString();
        }

        //Устанавливает значение поля ввода
        public override void SetValue(string value) {
            (CtrlField as NumericUpDown).Value = Int32.Parse(value);
        }

        //Устанавливает значение поля ввода
        public void SetValue(int value) {
            (CtrlField as NumericUpDown).Value = value;
        }

    }

}
