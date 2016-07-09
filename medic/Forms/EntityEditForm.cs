using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Forms {
    public partial class EntityEditForm : Form {
        public EntityEditForm() {
            InitializeComponent();
        }

        protected virtual bool Validate() {
            return true;
        }

        protected virtual void Save() { }

        public Panel GetPanel() {
            return panel;
        }

        private void saveButton_Click(object sender, EventArgs e) {
            if (Validate()) {
                Save();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
