using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {
    
    //Класс выпадающих списков с текстовым полем ввода и надписями
    public class ComboComboBoxWrapper : FieldWrapper {

        private string[] rightComboKeys;    //Ключи второго списка

        private ComboBox cmbList;           //Выпадающий список



        //Конструктор
        public ComboComboBoxWrapper(string labelText, string[] leftComboItems, string[] rightComboKeys, string[] rightComboValues, ComboBox field) : base(labelText, field) {
            this.rightComboKeys = rightComboKeys;
            
            field.DropDownStyle = ComboBoxStyle.DropDownList;
            field.Items.AddRange(rightComboValues);
            
            cmbList = new ComboBox();
            cmbList.Items.AddRange(leftComboItems);
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

        //Устанавливает выбранный элемент списка по значению
        public void SetComboValue(string value, int defaultIndex = 0) {
            for (int i = 0; i < cmbList.Items.Count; i++) {
                if (cmbList.GetItemText(cmbList.Items[i]) == value) {
                    cmbList.SelectedIndex = i;
                    return;
                }
            }
            cmbList.SelectedIndex = defaultIndex;
        }



        //Возвращает значение поля ввода
        public override string GetValue() {
            ComboBox comboBox = CtrlField as ComboBox;
            return comboBox.SelectedIndex > -1 ? rightComboKeys[comboBox.SelectedIndex] : "";
        }

        //Устанавливает значение поля ввода
        public override void SetValue(string key) {
            ComboBox comboBox = CtrlField as ComboBox;
            for (int i = 0; i < rightComboKeys.Length; i++)
                if (rightComboKeys[i] == key) {
                    comboBox.SelectedIndex = i;
                    return;
                }
            comboBox.SelectedIndex = 0;
        }

        //Устанавливает значение поля ввода
        public void SetValue(string key, int defaultIndex) {
            ComboBox comboBox = CtrlField as ComboBox;
            for (int i = 0; i < rightComboKeys.Length; i++)
                if (rightComboKeys[i] == key) {
                    comboBox.SelectedIndex = i;
                    return;
                }
            comboBox.SelectedIndex = defaultIndex;
        }

        public int GetIndex() {
            ComboBox comboBox = CtrlField as ComboBox;
            return comboBox.SelectedIndex;
        }

        public int SetIndex(int index) {
            ComboBox comboBox = CtrlField as ComboBox;
            return comboBox.SelectedIndex = index;
        }

    }

}
