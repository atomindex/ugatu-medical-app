using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace medic.Components {

    //Класс списка с кнопками добавления и удаления, и надписями
    class ListBoxWrapper : FieldWrapper {

        private ToolStrip tlsListManager;       //Панель с кнопками
        private ToolStripLabel tlsLblLabel;     //Подпись к полю
        private ToolStripButton tlsBtnAdd;      //Кнопка добавления
        private ToolStripButton tlsBtnRemove;   //Кнопка удаления
       


        //Конструктор
        public ListBoxWrapper(string labelText, ListBox field) : base(null, field) {
            field.Height = 60;

            tlsListManager = new ToolStrip();
            tlsListManager.GripStyle = ToolStripGripStyle.Hidden;
            tlsListManager.BackColor = Color.Transparent;
            tlsListManager.Parent = this;

            tlsLblLabel = new ToolStripLabel();
            tlsLblLabel.Text = labelText;
            tlsListManager.Items.Add(tlsLblLabel);

            tlsListManager.Items.Add(new ToolStripSeparator());
            
            tlsBtnAdd = new ToolStripButton();
            tlsBtnAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tlsBtnAdd.Text = "Добавить";
            tlsBtnAdd.Image = medic.Properties.Resources.Add;
            tlsListManager.Items.Add(tlsBtnAdd);

            tlsBtnRemove = new ToolStripButton();
            tlsBtnRemove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tlsBtnRemove.Text = "Удалить";
            tlsBtnRemove.Image = medic.Properties.Resources.Delete;
            tlsListManager.Items.Add(tlsBtnRemove);
        }



        //Не используется
        public override string GetValue() {
            return "";
        }

        //Не используется
        public override void SetValue(string value) {

        }



        //Добавляет событие на кнопку Добавить 
        public void AddAddEvent(EventHandler handler) {
            tlsBtnAdd.Click += handler;
        }

        //Добавляет событие на кнопку Удалить
        public void AddRemoveEvent(EventHandler handler) {
            tlsBtnRemove.Click += handler;
        }

    }

}
