using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace medic.Components {

    class ListBoxWrapper : FieldWrapper {

        private ToolStrip tlsListManager;
        private ToolStripLabel tlsLblLabel;
        private ToolStripButton tlsBtnAdd;
        private ToolStripButton tlsBtnRemove;
       


        public ListBoxWrapper(string labelText, ListBox field) : base(labelText, field) {
            field.Height = 60;
            LblLabel.Visible = false;

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



        public override string GetValue() {
            return "";
        }

        public override void SetValue(string value) {
            throw new NotImplementedException();
        }



        public void AddAddEvent(EventHandler handler) {
            tlsBtnAdd.Click += handler;
        }

        public void AddRemoveEvent(EventHandler handler) {
            tlsBtnRemove.Click += handler;
        }
    }

}
