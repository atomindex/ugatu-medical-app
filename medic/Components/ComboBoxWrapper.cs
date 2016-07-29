using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {
    
    //Класс выпадающего списка с надписями
    class ComboBoxWrapper : FieldWrapper {

        private string[] keysList;
        private string[] valuesList;

        //Конструктор
        public ComboBoxWrapper(string labelText, ComboBox field, string[] keysList, string[] valuesList) : base(labelText, field) {
            this.keysList = keysList;
            this.valuesList = valuesList;

            field.DropDownStyle = ComboBoxStyle.DropDownList;
            field.Items.AddRange(valuesList);
            field.SelectedIndex = 0;
        }



        //Возвращает значение поля ввода
        public override string GetValue() {
            return keysList[(CtrlField as ComboBox).SelectedIndex];
        }

        //Устанавливает значение поля ввода
        public override void SetValue(string keyValue) {
            for (int i = 0; i < keysList.Length; i++)
                if (keysList[i] == keyValue) {
                    (CtrlField as ComboBox).SelectedIndex = i;
                    break;
                }
        }

    }

}
