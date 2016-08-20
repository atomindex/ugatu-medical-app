using medic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Forms {
    public class ServicesReport : ReportForm {

        private DBConnection connection;
        private string[] titles;
        private double[] widths; 

        private DateTimePicker dtpDate;

        public ServicesReport(DBConnection connection) {
            Text = "Оказанные услуги";

            this.connection = connection;
            titles = new string[] { "Название", "Количество", "Стоимость, р" };
            widths = new double[] { 70, 15, 15 };

            tlsFilter.Items.Add(new ToolStripLabel("дата"));
            dtpDate = new DateTimePicker();
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = AppConfig.DateFormat;
            dtpDate.Width = 110;
            dtpDate.Value = DateTime.Now;
            tlsFilter.Items.Add(new ToolStripControlHost(dtpDate));

            tlsFilter.Items.Add(btnApply);
            AddApplyEvent(btnApply_Click);

            reload();
        }

        private void reload() {
            List<string[]> data = Report.GetServicesList(connection, dtpDate.Value);

            StartDocument();
            NewHeader("Оказанные услуги", "за " + dtpDate.Value.ToString(AppConfig.DateFormat));

            string prevWorker = "";
            double totalPrice = 0;
            double totalWorkerPrice = 0;
            for (int i = 0; i < data.Count; i++) {
                string[] row = data[i];
                double price = Double.Parse(row[2]);
                row[2] = price.ToString();

                if (prevWorker != row[3]) {
                    if (i > 0) {
                        EndTable();
                        NewSummary("Итого", totalWorkerPrice.ToString());
                        totalWorkerPrice = 0;
                    }
                    NewSubtitle(row[4]);
                    StartTable(titles, widths);
                }

                NewTableRow(row, 3, widths);
                prevWorker = row[3];

                totalPrice += price;
                totalWorkerPrice += price;
            }

            EndTable();
            NewSummary("Итого", totalWorkerPrice.ToString());

            NewSummary("Общий доход", totalPrice.ToString());

            EndDocument();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            reload();
        }

    }

}
