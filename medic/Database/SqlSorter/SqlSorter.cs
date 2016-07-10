using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {

    //Класс группы сортировок
    public class SqlSorter {

        private List<SqlSorterItem> items;      //Список элементов сортировки



        //Конструктор
        public SqlSorter() {
            items = new List<SqlSorterItem>();
        }



        //Добавление элемента в список сортировок
        public void AddItem(SqlSorterItem item) {
            items.Add(item);
        }



        //Возвращает строковое представление группы сортировок
        public override string ToString() {
            List<string> filledItems = new List<string>();

            for (int i = 0; i < items.Count; i++) {
                string buildedSort = items[i].ToString();
                if (buildedSort.Length > 0)
                    filledItems.Add(buildedSort);
            }

            return String.Join(",", filledItems);
        }

        //Возаращает строковое представление группы сортировок, с префиксом и суффиксом
        public string ToString(string prefix, string suffix) {
            string result = ToString();
            return result.Length > 0 ? prefix + result + suffix : "";
        }

    }

}
