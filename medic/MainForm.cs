using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using medic.Forms;
using medic.Database;

namespace medic {

    public partial class mainForm : Form {
        DBConnection connection;

        public mainForm() {
            InitializeComponent();

            connection = new DBConnection("localhost", "medic", "root", "");
        }

        private void menuItemWorkers_Click(object sender, EventArgs e) {
            ListData<Worker> listData = Worker.GetList(connection, 25);
            WorkersListForm workersListForm = new WorkersListForm(connection, listData);
            workersListForm.Show();
        }

        private void menuItemServices_Click(object sender, EventArgs e) {
            ListData<Service> listData = Service.GetList(connection, 25);
            ServicesListForm servicesListForm = new ServicesListForm(connection, listData);
            servicesListForm.Show();
        }

    }

}
