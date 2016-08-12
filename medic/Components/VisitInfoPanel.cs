using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {
    class VisitInfoPanel : Panel {

        private Label lblVisitDateValue;
        private Label lblPatientName;                       //Надпись Имя пациента
        private Label lblPatientSexValue;
        private Label lblPatientAgeValue;

        private TableLayoutPanel tlpServicesInfo;
        private Label lblEmptyServices;
        private Label lblServicesPrice;

        private Label lblSalesInfo;
        private TableLayoutPanel tlpSalesInfo;
        private Label lblSalesPercent;

        private Label lblTotalPrice;

        public VisitInfoPanel() {
            Dock = DockStyle.Top;
            Padding = new Padding(10, 10, 10, 10);
            Margin = new Padding(0, 15, 15, 5);
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;


            Panel pnlTotalPrice = new Panel();
            pnlTotalPrice.Dock = DockStyle.Top;
            pnlTotalPrice.Height = 38;
            pnlTotalPrice.Padding = new Padding(0, 0, 0, 10);
            pnlTotalPrice.Parent = this;

            lblTotalPrice = createLabel("0 р.");
            lblTotalPrice.Dock = DockStyle.Right;
            lblTotalPrice.BackColor = AppConfig.BlueColor;
            lblTotalPrice.ForeColor = Color.White;
            lblTotalPrice.Padding = new Padding(10, 5, 10, 5);
            lblTotalPrice.FlatStyle = FlatStyle.Standard;
            lblTotalPrice.TextAlign = ContentAlignment.MiddleCenter;
            lblTotalPrice.Font = new Font(lblTotalPrice.Font.Name, 10, FontStyle.Regular);
            lblTotalPrice.Parent = pnlTotalPrice;

            lblSalesPercent = createLabel("Общий размер скидки: 0%", FontStyle.Bold);
            lblSalesPercent.Dock = DockStyle.Top;
            lblSalesPercent.Padding = new Padding(0,0,0,20);
            lblSalesPercent.ForeColor = AppConfig.BlueColor;
            lblSalesPercent.Parent = this;

            tlpSalesInfo = createTableLayoutPanel();
            tlpSalesInfo.Parent = this;

            lblSalesInfo = createLabel("Скидки", FontStyle.Bold);
            lblSalesInfo.Dock = DockStyle.Top;
            lblSalesInfo.Padding = new Padding(0, 0, 0, 5);
            lblSalesInfo.Parent = this;



            lblServicesPrice = createLabel("Общая цена услуг: 0 р.", FontStyle.Bold);
            lblServicesPrice.Dock = DockStyle.Top;
            lblServicesPrice.Padding = new Padding(0, 0, 0, 20);
            lblServicesPrice.ForeColor = AppConfig.BlueColor;
            lblServicesPrice.Parent = this;

            lblEmptyServices = createLabel("Не добавленно ни одной услуги", FontStyle.Italic);
            lblEmptyServices.Dock = DockStyle.Top;
            lblEmptyServices.Padding = new Padding(0,0,0,5); 
            lblEmptyServices.Parent = this;

            tlpServicesInfo = createTableLayoutPanel();
            tlpServicesInfo.Parent = this;

            Label lblServicesInfo = createLabel("Услуги", FontStyle.Bold);
            lblServicesInfo.Dock = DockStyle.Top;
            lblServicesInfo.Padding = new Padding(0, 0, 0, 5);
            lblServicesInfo.Parent = this;



            TableLayoutPanel tlpUserInfo = createTableLayoutPanel();
            tlpUserInfo.RowCount = 3;
            tlpUserInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpUserInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpUserInfo.Parent = this;

            Label lblVisitDate = createLabel("Дата посещения", FontStyle.Bold);
            lblVisitDate.Margin = new Padding(0, 0, 10, 5);
            tlpUserInfo.Controls.Add(lblVisitDate, 0, 0);

            lblVisitDateValue = createLabel("");
            lblVisitDateValue.Margin = new Padding(0, 0, 0, 5);
            tlpUserInfo.Controls.Add(lblVisitDateValue, 1, 0);

            Label lblPatientSex = createLabel("Пол", FontStyle.Bold);
            lblPatientSex.Margin = new Padding(0, 0, 10, 5);
            tlpUserInfo.Controls.Add(lblPatientSex, 0, 1);

            lblPatientSexValue = createLabel("");
            lblPatientSexValue.Margin = new Padding(0, 0, 0, 5);
            tlpUserInfo.Controls.Add(lblPatientSexValue, 1, 1);

            Label lblPatientAge = createLabel("Возраст", FontStyle.Bold);
            lblPatientAge.Margin = new Padding(0, 0, 10, 5);
            tlpUserInfo.Controls.Add(lblPatientAge, 0, 2);

            lblPatientAgeValue = new Label();
            lblPatientAgeValue.Margin = new Padding(0, 0, 0, 5);
            tlpUserInfo.Controls.Add(lblPatientAgeValue, 1, 2);

            lblPatientName = createLabel("");
            lblPatientName.Dock = DockStyle.Top;
            lblPatientName.Font = new Font(lblPatientName.Font.Name, 12);
            lblPatientName.Padding = new Padding(0, 0, 0, 15);
            lblPatientName.Parent = this;
        }

        private TableLayoutPanel createTableLayoutPanel() {
            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.Dock = DockStyle.Top;
            tlp.AutoSize = true;
            tlp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlp.ColumnCount = 2;
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            return tlp;
        }

        private Label createLabel(string text, FontStyle style = FontStyle.Regular) {
            Label label = new Label();
            label.AutoSize = true;
            label.FlatStyle = FlatStyle.System;
            label.Text = text;
            if (style != FontStyle.Regular)
                label.Font = new Font(label.Font.Name, label.Font.Size, style);
            return label;
        }

        private void addLayoutRow(TableLayoutPanel tlpPanel, string labelText, string valueText, Padding margin) {
            int index = tlpPanel.RowCount;
            tlpPanel.RowCount++;

            Label label = createLabel(labelText);
            label.Dock = DockStyle.Top;
            label.MaximumSize = new Size(300, 0);
            label.Margin = margin;

            updateLabelHeight(label);
            tlpPanel.Controls.Add(label, 0, index);

            if (valueText != null) {
                Label value = createLabel(valueText);
                value.TextAlign = ContentAlignment.TopRight;
                value.Dock = DockStyle.Right;
                value.Margin = margin;
                tlpPanel.Controls.Add(value, 1, index);
            }
            
            tlpPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        private void removeLayoutRow(TableLayoutPanel tlpPanel, int index) {
            for (int i = 0; i < tlpPanel.ColumnCount; i++) {
                Control control = tlpPanel.GetControlFromPosition(i, index);
                if (control != null) {
                    tlpPanel.Controls.Remove(control);
                    control.Parent = null;
                }
            }

            tlpPanel.RowStyles.RemoveAt(index);

            for (int i = index + 1; i < tlpPanel.RowCount; i++) {
                for (int j = 0; j < tlpPanel.ColumnCount; j++) {
                    var control = tlpPanel.GetControlFromPosition(j, i);
                    if (control != null) {
                        tlpPanel.SetRow(control, i - 1);
                    }
                }
            }
            tlpPanel.RowCount = tlpPanel.RowCount - 1;
        }

        private void updateLabelHeight(Label label) {
            Graphics g = label.CreateGraphics();
            SizeF size = g.MeasureString(label.Text, label.Font, label.MaximumSize.Width);
            label.MinimumSize = new Size(label.MinimumSize.Width, (int)size.Height);
        }




        public void SetVisitDate(string date) {
            lblVisitDateValue.Text = date;
        }

        public void SetPatientName(string name) {
            lblPatientName.Text = name;
        }

        public void SetPatientSex(string sex) {
            lblPatientSexValue.Text = sex;
        }

        public void SetPatientAge(int age) {
            lblPatientAgeValue.Text = age.ToString();
        }

        public void AddService(string name, int price) {
            SuspendLayout();
            addLayoutRow(tlpServicesInfo, name, price.ToString() + " р.", Padding.Empty);
            addLayoutRow(tlpServicesInfo, "", null, new Padding(0, 3, 0, 5));
            
            Label workerLabel = (tlpServicesInfo.GetControlFromPosition(0, tlpServicesInfo.RowCount-1) as Label);
            workerLabel.ForeColor = Color.FromArgb(100,100,100);
            workerLabel.AutoSize = false;
            workerLabel.Height = 1;

            lblEmptyServices.Visible = false;

            ResumeLayout();
        }

        public void SetServiceWorkerAt(int index, string workerName) {
            Label lblServiceWorker = tlpServicesInfo.GetControlFromPosition(0, index * 2 + 1) as Label;
            lblServiceWorker.Text = workerName;
            if (workerName.Length > 0) {
                lblServiceWorker.AutoSize = true;
                updateLabelHeight(lblServiceWorker);
            } else {
                lblServiceWorker.AutoSize = false;
                lblServiceWorker.MinimumSize = new Size(lblServiceWorker.MinimumSize.Width, 0);
                lblServiceWorker.Height = 1;
            }
        }

        public void RemoveServiceAt(int index) {
            SuspendLayout();
            removeLayoutRow(tlpServicesInfo, index * 2);
            removeLayoutRow(tlpServicesInfo, index * 2);
            lblEmptyServices.Visible = tlpServicesInfo.RowCount == 0;
            ResumeLayout();
        }


        public void AddSale(string name, string salePercent) {
            SuspendLayout();
            addLayoutRow(tlpSalesInfo, name, salePercent + "%", new Padding(0, 0, 0, 5));
            lblSalesPercent.Visible = true;
            lblSalesInfo.Visible = true;
            ResumeLayout();
        }

        public void ClearSales() {
            SuspendLayout();
            for (int i = 0; i < tlpSalesInfo.ColumnCount; i++) {
                for (int j = 0; j < tlpSalesInfo.RowCount; j++) {
                    Control control = tlpSalesInfo.GetControlFromPosition(i, j);
                    if (control != null) {
                        tlpSalesInfo.Controls.Remove(control);
                        control.Parent = null;
                    }
                }
            }

            tlpSalesInfo.RowStyles.Clear();
            tlpSalesInfo.RowCount = 0;

            lblSalesPercent.Visible = false;
            lblSalesInfo.Visible = false;
            ResumeLayout();
        }

        public void SetServicesPrice(string price) {
            lblServicesPrice.Text = "Общая цена услуг: " + price + " р.";
        }

        public void SetSalesPercent(string percent, string price) {
            lblSalesPercent.Text = "Общий размер скидки: " + percent + "% (-" + price + " р.)";
        }

        public void SetTotalPrice(string totalPrice) {
            lblTotalPrice.Text = totalPrice + " р.";
        }

    }
}
