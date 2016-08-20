using medic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Forms {
    public class PatientsVisitsReport : ReportForm {

        private DBConnection connection;
        private List<Patient> patients;
        private string[] titles;
        private double[] widths; 

        public PatientsVisitsReport(DBConnection connection, List<Patient> patients) {
            Text = "Посещения пациентов";

            this.connection = connection;
            this.patients = patients;
            titles = new string[] { "Дата посещения", "Услуги", "Количество", "Цена", "Скидка", "Цена со скидкой" };
            widths = new double[] { 10, 50, 10, 10, 10, 10 };

            tlsFilter.Visible = false;

            reload();
        }

        private void reload() {
            

            StartDocument();
            NewHeader("Посещения пациентов", "");

            foreach (Patient patient in patients) {
                List<string[]> data = Report.GetPatientVisitsList(patient.GetId(), connection);
                NewSubtitle(patient.GetFullName());

                StartTable(titles, widths);

                double totalProfit = 0;
                foreach (string[] row in data) {
                    NewTableRow(row, 6);
                    totalProfit += Double.Parse(row[5]);
                }

                EndTable();

                NewSummary("Стоимось всех посещений", totalProfit.ToString() + " р.");
            }

            EndDocument();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            reload();
        }
    }
}
