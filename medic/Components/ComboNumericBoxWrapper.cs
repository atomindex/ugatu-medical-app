using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {
    
    //Класс выпадающих списков с текстовым полем ввода и надписями
    public class ComboNumericBoxWrapper : FieldWrapper {

        private ComboBox cmbList;       //Выпадающий список



        //Конструктор
        public ComboNumericBoxWrapper(string labelText, string[] comboItems, NumericUpDown field, int minValue = -1, int maxValue = -1) : base(labelText, field) {
            field.Minimum = minValue > -1 ? minValue : Int32.MinValue;
            field.Maximum = maxValue > -1 ? maxValue : Int32.MaxValue;
            
            cmbList = new ComboBox();
            cmbList.Items.AddRange(comboItems);
            cmbList.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbList.Dock = DockStyle.Top;

            TableLayoutPanel tblLPWrapperLayout = new TableLayoutPanel();
            tblLPWrapperLayout.RowCount = 1;
            tblLPWrapperLayout.ColumnCount = 2;
            tblLPWrapperLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tblLPWrapperLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tblLPWrapperLayout.AutoSize = true;
            tblLPWrapperLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblLPWrapperLayout.Dock = DockStyle.Top;

            tblLPWrapperLayout.Controls.Add(field, 1, 0);
            tblLPWrapperLayout.Controls.Add(cmbList, 0, 0);

            tblLPWrapperLayout.Parent = this;
            Controls.SetChildIndex(tblLPWrapperLayout, Controls.GetChildIndex(LblLabel));
        }



        //Возвращает индекс выбранного элемента списка
        public int GetComboIndex() {
            return cmbList.SelectedIndex;
        }

        //Устанавливает выбранный элемент списка по индекса
        public void SetComboIndex(int value) {
            cmbList.SelectedIndex = value;
        }

        //Устанавливает выбранный элемент списка по значение
        public void SetComboValue(string value) {
            for (int i = 0; i < cmbList.Items.Count; i++) {
                if (cmbList.GetItemText(cmbList.Items[i]) == value) {
                    cmbList.SelectedIndex = i;
                    break;
                }
            }
        }



        //Возвращает значение поля ввода
        public override string GetValue() {
            return (CtrlField as NumericUpDown).Value.ToString();
        }

        //Устанавливает значение поля ввода
        public override void SetValue(string value) {
            NumericUpDown nud = (CtrlField as NumericUpDown);
            nud.Value = Math.Min(nud.Maximum, Math.Max(nud.Minimum, Int32.Parse(value)));
        }

        //Устанавливает значение поля ввода
        public void SetValue(int value) {
            NumericUpDown nud = (CtrlField as NumericUpDown);
            nud.Value = Math.Min(nud.Maximum, Math.Max(nud.Minimum, value));
        }

    }

}
