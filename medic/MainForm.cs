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

            connection = new DBConnection(
                AppConfig.DatabaseHost, 
                AppConfig.DatabaseName, 
                AppConfig.DatabaseUser, 
                AppConfig.DatabasePassword,
                AppConfig.DatabaseConnectTime
            );
        }

        private void menuItemWorkers_Click(object sender, EventArgs e) {
            ListData listData = Worker.GetListData(connection, 2);
            WorkerListForm workersListForm = new WorkerListForm(listData);
            workersListForm.Show();
        }

        private void menuItemServices_Click(object sender, EventArgs e) {
            ListData listData = Service.GetListData(connection, 25);
            ServiceListForm servicesListForm = new ServiceListForm(listData);
            servicesListForm.Show();
        }

        private void menuItemSpecialties_Click(object sender, EventArgs e) {
            ListData listData = Specialty.GetListData(connection, 25);
            SpecialtyListForm specialtiesListForm = new SpecialtyListForm(listData);
            specialtiesListForm.Show();
        }

    }

}
