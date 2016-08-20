using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {
    public class QueryBuilder {

        //Экранирует специальные символы
        public static string EscapeString(string value, bool addQuotes = false) {
            StringBuilder sb = baseEscape(value);

            if (addQuotes) {
                sb.Insert(0, "\"");
                sb.Append("\"");
            }

            return sb.ToString();
        }

        //Экранирует специальные символы для like
        public static string EscapeLikeString(string value, bool addQuotes = false) {
            StringBuilder sb = baseEscape(value);
            sb.Replace("%", "\\%");

            if (addQuotes) {
                sb.Insert(0, "\"");
                sb.Append("\"");
            }

            return sb.ToString();
        }

        //Экранирует кавычки и слэши
        private static StringBuilder baseEscape(string value) {
            StringBuilder sb = new StringBuilder(value);

            sb.Replace("\\", "\\\\");
            sb.Replace("\"", "\\\"");
            sb.Replace("'", "\\'");

            return sb;
        }



        //Возвращает sql для LIMIT и OFFSET
        public static string BuildLimit(int limit, int pageIndex) {
            if (limit <= 0) return "";
            return "LIMIT " + limit.ToString() + " OFFSET " + (limit * pageIndex).ToString();
        }

        //Возвращает sql для concat полей, с разделителем в виде пробела
        public static string BuildConcatStatement(string tableName, string[] fieldsNames) {
            StringBuilder sb = new StringBuilder();

            sb.Append("CONCAT(");
            for (int i = 0; i < fieldsNames.Length; i++) {
                sb.Append(tableName);
                sb.Append(".");
                sb.Append(fieldsNames[i]);
                if (i < fieldsNames.Length - 1) sb.Append(",\" \",");
            }
            sb.Append(")");

            return sb.ToString();
        }

        //Возвращает sql для in условия
        public static string BuildInStatement(string[] values) {
            return values != null && values.Length > 0 ? "(" + String.Join(",", values) + ")" : null;
        }



        //Возвращает строковое представление логического оператора
        public static string ConvertLogicalOperator(SqlLogicalOperator op) {
            switch (op) {
                case SqlLogicalOperator.And: return "AND";
                case SqlLogicalOperator.Or: return "OR";
                case SqlLogicalOperator.AndNot: return "AND NOT";
                case SqlLogicalOperator.OrNot: return "OR NOT";
            }
            return "AND";
        }

        //Возвращает строковое представление оператора сравнения
        public static string ConvertComparisonOperator(SqlComparisonOperator op) {
            switch (op) {
                case SqlComparisonOperator.Equal: return "=";
                case SqlComparisonOperator.NotEqual: return "<>";
                case SqlComparisonOperator.Less: return "<";
                case SqlComparisonOperator.Larger: return ">";
                case SqlComparisonOperator.LessEqual: return "<=";
                case SqlComparisonOperator.LargerEqual: return ">=";
                case SqlComparisonOperator.Like: return "LIKE";
                case SqlComparisonOperator.NotLike: return "NOT LIKE";
                case SqlComparisonOperator.In: return "IN";
                case SqlComparisonOperator.NotIn: return "NOT IN";
                case SqlComparisonOperator.Is: return "IS";
                case SqlComparisonOperator.NotIs: return "NOT IS";
            }
            return "=";
        }

        //Возвращает строковое представление порядка сортировки
        public static string ConvertOrder(SqlOrder order) {
            switch (order) {
                case SqlOrder.Asc: return "ASC";
                case SqlOrder.Desc: return "DESC";
                case SqlOrder.None: return "";
            }
            return "";
        }

    }

}
