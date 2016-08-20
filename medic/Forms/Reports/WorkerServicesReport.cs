using medic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Forms {
    public class WorkerServicesReport : ReportForm {

        private DBConnection connection;
        private List<Worker> workers;
        private string[] titles;
        private double[] widths; 

        private DateTimePicker dtpDateFrom;
        private DateTimePicker dtpDateTo;

        public WorkerServicesReport(DBConnection connection, List<Worker> workers) {
            Text = "Оказанные услуги";

            this.connection = connection;
            this.workers = workers;
            titles = new string[] { "Дата", "Категория пациента", "ФИО пациента", "Услуга", "Стоимость услуги, р." };
            widths = new double[] { 10, 15, 25, 25, 15 };

            tlsFilter.Items.Add(new ToolStripLabel("от"));
            dtpDateFrom = new DateTimePicker();
            dtpDateFrom.Format = DateTimePickerFormat.Custom;
            dtpDateFrom.CustomFormat = AppConfig.DateFormat;
            dtpDateFrom.Width = 110;
            DateTime now = DateTime.Now;
            dtpDateFrom.Value = new DateTime(now.Year, now.Month, 1);
            tlsFilter.Items.Add(new ToolStripControlHost(dtpDateFrom));

            tlsFilter.Items.Add(new ToolStripLabel("до"));
            dtpDateTo = new DateTimePicker();
            dtpDateTo.Format = DateTimePickerFormat.Custom;
            dtpDateTo.CustomFormat = AppConfig.DateFormat;
            dtpDateTo.Width = 110;
            tlsFilter.Items.Add(new ToolStripControlHost(dtpDateTo));

            tlsFilter.Items.Add(btnApply);
            AddApplyEvent(btnApply_Click);

            reload();
        }

        private void reload() {
            StartDocument();
            NewHeader("Оказанные услуги", "за период с " + dtpDateFrom.Value.ToString(AppConfig.DateFormat) + " до " + dtpDateTo.Value.ToString(AppConfig.DateFormat));

            foreach (Worker worker in workers) {
                List<string[]> data = Report.GetWorkerServicesList(worker.GetId(), connection, dtpDateFrom.Value, dtpDateTo.Value);
                NewSubtitle(worker.GetFullName());

                if (data.Count > 0) {
                    StartTable(titles, widths);

                    double totalProfit = 0;
                    foreach (string[] row in data) {
                        NewTableRow(row, 5, widths);
                        totalProfit += Double.Parse(row[4]);
                    }
                    EndTable();
                    NewSummary("Общий доход", totalProfit.ToString() + " р.");
                } else
                    NewEmptyText("Не оказанно ни одной услуги");
            }

            EndDocument();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            reload();
        }

    }

}
