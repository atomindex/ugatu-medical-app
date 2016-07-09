using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {

    public enum SqlSorterOrder { none, asc, desc };

    public class SqlSorterItem {

        private string tableName;
        private string fieldName;
        private string fullFieldName;
        private SqlSorterOrder order;

        public static string RenderOrder(SqlSorterOrder order) {
            switch (order) {
                case SqlSorterOrder.asc: return "ASC";
                case SqlSorterOrder.desc: return "DESC";
                case SqlSorterOrder.none: return "";
            }
            return "ASC";
        }

        public SqlSorterItem() {
            SetField("", "");
            SetOrder(SqlSorterOrder.none);
        }

        public SqlSorterItem(string tableName, string fieldName, SqlSorterOrder order) {
            SetField(tableName, fieldName);
            SetOrder(order);
        }

        public void SetField(string tableName, string fieldName) {
            this.tableName = tableName;
            this.fieldName = fieldName;

            if (tableName.Length > 0 && fieldName.Length > 0)
                this.fullFieldName = tableName + "." + fieldName;
            else if (fieldName.Length > 0)
                this.fullFieldName = fieldName;
            else 
                this.fullFieldName = "";
        }

        public void SetOrder(SqlSorterOrder order) {
            this.order = order;
        }

        public override string ToString() {
            if (order != SqlSorterOrder.none && fullFieldName.Length > 0)
                return fullFieldName + " " + RenderOrder(order);
            else
                return "";
        }

    }

    public class SqlSorter {
        private List<SqlSorterItem> items;

        public SqlSorter() {
            items = new List<SqlSorterItem>();
        }

        public void AddItem(SqlSorterItem item) {
            items.Add(item);
        }

        public override string ToString() {
            List<string> filledItems = new List<string>();

            for (int i = 0; i < items.Count; i++) {
                string buildedSort = items[i].ToString();
                if (buildedSort.Length > 0)
                    filledItems.Add(buildedSort);
            }

            return String.Join(",", filledItems);
        }

        public string ToString(string prefix, string suffix) {
            string result = ToString();
            return result.Length > 0 ? prefix + result + suffix : "";
        }

    }

}
