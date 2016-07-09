using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {
    public enum SqlFilterOperator { and, or, andnot, ornot };
    public enum SqlFilterConditionOperator { equals, less, larger, lessEquals, largerEquals, like };



    public abstract class SqlFilterItem {
    }

    public class SqlFilterCondition : SqlFilterItem {
        private SqlFilterConditionOperator op;
        private string stringOp;
        private string tableName;
        private string fieldName;
        private string fullFieldName;
        private string value;

        public static string RenderOperator(SqlFilterConditionOperator op) {
            switch (op) {
                case SqlFilterConditionOperator.equals: return " = ";
                case SqlFilterConditionOperator.less: return " < ";
                case SqlFilterConditionOperator.larger: return " > ";
                case SqlFilterConditionOperator.lessEquals: return " <= ";
                case SqlFilterConditionOperator.largerEquals: return " >= ";
                case SqlFilterConditionOperator.like: return " LIKE ";
            }
            return "=";
        }

        public SqlFilterCondition(string tableName, string fieldName, SqlFilterConditionOperator op) {
            this.tableName = tableName;
            this.fieldName = fieldName;
            this.fullFieldName = tableName.Length > 0 ? tableName + "." + fieldName : fieldName;
            this.op = op;
            this.stringOp = RenderOperator(op);
        }

        public void SetValue(object value) {
            if (value == null)
                this.value = null;
            else
                this.value = value.ToString();
        }

        public override string ToString() {
            if (value == null) return "";
            return fullFieldName + stringOp + value;
        } 
    }

    public class SqlFilter : SqlFilterItem {
        private SqlFilterOperator op;
        private List<SqlFilterItem> items;

        public static string RenderOperator(SqlFilterOperator op) {
            switch (op) {
                case SqlFilterOperator.and: return " AND ";
                case SqlFilterOperator.or: return " OR ";
                case SqlFilterOperator.andnot: return " AND NOT ";
                case SqlFilterOperator.ornot: return " OR NOT";
            }
            return " AND ";
        }

        public static string BuildConcatStatement(string tableName, string[] fieldsNames) {
            StringBuilder sb = new StringBuilder();

            sb.Append("CONCAT(");
            for (int i = 0; i < fieldsNames.Length; i++) {
                sb.Append(tableName);
                sb.Append(".");
                sb.Append(fieldsNames[i]);
                if (i < fieldsNames.Length - 1) sb.Append(",");
            }
            sb.Append(")");
            
            return sb.ToString();
        }

        public SqlFilter(SqlFilterOperator op) {
            this.op = op;
            items = new List<SqlFilterItem>();
        }

        public SqlFilter(SqlFilterOperator op, SqlFilterItem[] items) {
            this.op = op;
            this.items = items.ToList<SqlFilterItem>();
        }

        public void AddItem(SqlFilterItem item) {
            items.Add(item);
        }

        public void AddItems(SqlFilterItem[] items) {
            this.items.AddRange(items.ToList<SqlFilterItem>());
        }

        public override string ToString() {
            List<string> filledItems = new List<string>();
            foreach(SqlFilterItem filterItem in items) {
                string buildedFilter = filterItem.ToString();
                if (buildedFilter.Length > 0)
                    filledItems.Add(buildedFilter);
            }

            return filledItems.Count > 0 ? "(" + String.Join(RenderOperator(op), filledItems) + ")" : "";
        }

        public string ToString(string prefix, string suffix) {
            string result = ToString();
            return result.Length > 0 ? prefix + result + suffix : "";
        }
    }
}
