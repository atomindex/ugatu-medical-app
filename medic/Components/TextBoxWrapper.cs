using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {
    
    //Класс текстового поля ввода с надписями
    class TextBoxWrapper : FieldWrapper {

        //Конструктор
        public TextBoxWrapper(string labelText, TextBox field) : base(labelText, field) {}



        //Возвращает значение поля ввода
        public override string GetValue() {
            return (CtrlField as TextBox).Text;
        }

        //Устанавливает значение поля ввода
        public override void SetValue(string value) {
            (CtrlField as TextBox).Text = value;
        }

    }

}
