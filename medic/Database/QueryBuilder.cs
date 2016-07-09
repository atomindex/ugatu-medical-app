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

    }

}
