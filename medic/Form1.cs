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
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            DBConnection connection = new DBConnection("localhost", "medic", "root", "");
            connection.OpenConnection();

            ListData<Worker> listData = Worker.GetList(connection, 25);

            WorkersListForm workerListForm = new WorkersListForm(connection, listData);
            workerListForm.Show();

            return;

            Worker worker = new Worker(connection);
            worker.FirstName = "Иван";
            worker.LastName = "Иванов";
            worker.MiddleName = "Иванович";
            worker.Phone = "+7-(964)-955-30-19";
            worker.Address = "Россия, Башкортастан, Стерлитамак, Худайбердина 99-64";
            worker.Save();

            MessageBox.Show(worker.GetId().ToString());
        }

   
    }
}
