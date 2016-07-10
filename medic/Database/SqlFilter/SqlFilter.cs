using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {

    //Класс группы условий фильтра
    public class SqlFilter : SqlFilterItem {

        private SqlLogicalOperator op;           //Логический оператор между условиями
        private string stringOp;                 //Строковое представление логического оператора

        private List<SqlFilterItem> items;      //Список элементов фильтра (другие группы или условия)


        //Конструктор
        public SqlFilter(SqlLogicalOperator op) {
            this.op = op;
            this.stringOp = " " + QueryBuilder.ConvertLogicalOperator(op) + " ";
            items = new List<SqlFilterItem>();
        }

        //Конструктор
        public SqlFilter(SqlLogicalOperator op, SqlFilterItem[] items) {
            this.op = op;
            this.stringOp = " " + QueryBuilder.ConvertLogicalOperator(op) + " ";
            this.items = items.ToList<SqlFilterItem>();
        }



        //Добавляет элемент в список условий
        public void AddItem(SqlFilterItem item) {
            items.Add(item);
        }

        //Добавляет массив элементов в список условий
        public void AddItems(SqlFilterItem[] items) {
            this.items.AddRange(items.ToList<SqlFilterItem>());
        }



        //Возвращает строковое представление условий 
        public override string ToString() {
            List<string> filledItems = new List<string>();
            foreach(SqlFilterItem filterItem in items) {
                string buildedFilter = filterItem.ToString();
                if (buildedFilter.Length > 0)
                    filledItems.Add(buildedFilter);
            }

            return filledItems.Count > 0 ? "(" + String.Join(stringOp, filledItems) + ")" : "";
        }

        //Возвращает строковое представление условий, с префиксом и суффиксом
        public string ToString(string prefix, string suffix) {
            string result = ToString();
            return result.Length > 0 ? prefix + result + suffix : "";
        }

    }

}
