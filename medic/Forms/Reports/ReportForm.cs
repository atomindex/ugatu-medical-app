using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace medic.Forms {
    public partial class ReportForm : Form {
        private StringBuilder html;

        public ReportForm() {
            InitializeComponent();
            html = new StringBuilder();
        }

        public void StartDocument() {
            html.Clear();
            html.Append("<!DOCTYPE html><html><head><style>" +
                        " html { margin: 0; padding: 0; }" +
                        " body { font: 14px Arial; padding: 0 15px 15px 15px; margin: 0; } " +
                        " h1 { text-align: center; line-height: 1; margin: 0 0 5px 0; } " +
                        " .report-header { margin: 0 -15px 30px -15px; padding: 15px; background: rgb(250,250,250); border-bottom: 1px solid silver; } " +
                        " .report-description { text-align: center; font-size: 12px; }" +
                        " table { width: 100%; border-spacing: 0; border-collapse: collapse; margin-bottom: 10px; } " +
                        " table th { background: rgb(240,240,240); text-align: left; }; " +
                        " table th, table td { padding: 7px 10px; border: 1px solid black; vertical-align: top; } " +
                        " .summary-wrapper { overflow: hidden; margin-bottom: 45px; }" +
                        " .summary { float: left; background: rgb(0, 108, 255); padding: 5px; color: white; } " +
                        " .summary-wrapper:last-child { margin-bottom: 0 } " +
                        " .text-empty { color: rgb(90,90,90); font-style: italic; margin-bottom: 45px; } " +
                        " .text-empty:last-child { margin-bottom: 0 } " +
                        "</style></head><body>");
        }

        public void EndDocument() {
            html.Append("</body></html>");

            if (wb.Document == null)
                wb.DocumentText = html.ToString();
            else {
                wb.Document.OpenNew(true);
                wb.Document.Write(html.ToString());
            }
                
        }

        public void NewHeader(string title, string subtitle) {
            html.Append("<div class='report-header'>");
            html.Append("<h1>");
            html.Append(HttpUtility.HtmlEncode(title));
            html.Append("</h1>");
            html.Append("<div class='report-description'>");
            html.Append(HttpUtility.HtmlEncode(subtitle));
            html.Append("</div>");
            html.Append("</div>");
        }

        public void StartTable(string[] titles, double[] widths = null) {
            html.Append("<table>");

            html.Append("<tr>");
            int i = 0;
            foreach (string title in titles) {
                html.Append(widths == null ? "<th>" : "<th style='width: " + widths[i % widths.Length].ToString() + "%'>");
                html.Append(HttpUtility.HtmlEncode(title));
                html.Append("</th>");
                i++;
            }
            html.Append("</tr>");
        }

        public void NewTableRow(string[] cells, int cellsCount = -1, double[] widths = null) {
            if (cellsCount < 0) cellsCount = cells.Length;

            html.Append("<tr>");
            for (int i = 0; i < cellsCount; i++) {
                html.Append(widths == null ? "<td>" : "<td style='width: "+widths[i%widths.Length].ToString()+"%'>");
                html.Append(HttpUtility.HtmlEncode(cells[i]));
                html.Append("</td>");
            }
            html.Append("</tr>");
        }

        public void EndTable() {
            html.Append("</table>");
        }

        public void NewTitle(string title) {
            html.Append("<h2>");
            html.Append(HttpUtility.HtmlEncode(title));
            html.Append("</h2>");
        }

        public void NewSubtitle(string subtitle) {
            html.Append("<h3>");
            html.Append(HttpUtility.HtmlEncode(subtitle));
            html.Append("</h3>");
        }

        public void NewSummary(string title, string value) {
            html.Append("<div class='summary-wrapper'>");
            html.Append("<div class='summary'>");
            html.Append("<span class='title'>");
            html.Append(HttpUtility.HtmlEncode(title));
            html.Append(": </span><span class='value'>");
            html.Append(HttpUtility.HtmlEncode(value));
            html.Append("</span>");
            html.Append("</div>");
            html.Append("</div>");
        }

        public void NewEmptyText(string text) {
            html.Append("<div class='text-empty'>");
            html.Append(HttpUtility.HtmlEncode(text));
            html.Append("</div>");
        }

        public void AddApplyEvent(EventHandler handler) {
            btnApply.Click += handler;
        }
    }
}
