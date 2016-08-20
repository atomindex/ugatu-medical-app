using medic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Forms {
    public class ServicesProfitReport : ReportForm {

        private DBConnection connection;

        private DateTimePicker dtpDateFrom;
        private DateTimePicker dtpDateTo;

        public ServicesProfitReport(DBConnection connection) {
            Text = "Доход по услугам";

            this.connection = connection;

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
        }

        protected override bool reload() {
            List<string[]> data = Report.GetServicesProfitList(connection, dtpDateFrom.Value, dtpDateTo.Value);
            if (data == null) {
                Rollback();
                return false;
            }

            StartDocument();
            NewHeader("Доход по услугам", "за период с " + dtpDateFrom.Value.ToString(AppConfig.DateFormat) + " до " + dtpDateTo.Value.ToString(AppConfig.DateFormat));

            StartTable(new string[] { "Услуга", "Доход, р." });

            double totalProfit = 0;
            foreach (string[] row in data) {
                NewTableRow(row, 2);
                totalProfit += Double.Parse(row[1]);
            }

            EndTable();
            NewSummary("Общий доход", totalProfit.ToString() + " р.");
            EndDocument();

            return true;
        }

        private void btnApply_Click(object sender, EventArgs e) {
            reload();
        }
    }
}
