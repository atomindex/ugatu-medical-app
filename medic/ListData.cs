using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс для хранения информации о select запросе и для его выполнения
    public class ListData {

        //Соединение с базой
        public DBConnection Connection {
            get;
            private set;
        }

        //Sql для получения данных (без where)
        public string BaseSql {
            get;
            private set;
        }

        //Sql для подсчета данных (без where)
        public string CountSql {
            get;
            private set;
        }

        //Фильтр
        public SqlFilter Filter {
            get;
            private set;
        }

        //Сортировщик
        public SqlSorter Sorter {
            get;
            private set;
        }

        //Ограничение по количеству
        public int Limit {
            get;
            private set;
        }

        //Общее количество данных
        public int Count {
            get;
            private set;
        }        
   
        //Количество страниц
        public int Pages {
            get;
            private set;
        }           

        //Текущая страница
        public int PageIndex {          
            get;
            private set;
        }

        //Группировка
        public string GroupBy {
            get;
            private set;
        }

        //Список
        public List<string[]> List {
            get;
            private set;
        }



        //Конструктор
        public ListData(DBConnection connection, string baseSql, string countSql, SqlFilter filter, SqlSorter sorter, int limit, int pageIndex, string groupBy = "") {
            this.Connection = connection;

            this.BaseSql = baseSql;
            this.CountSql = countSql;

            this.Filter = filter;
            this.Sorter = sorter;
            this.GroupBy = groupBy;

            this.Limit = limit;
            this.PageIndex = pageIndex;
        }



        //Обновления списка
        public List<string[]> Update(int pageIndex = 0) {
            string where_statement = Filter != null ? Filter.ToString(" WHERE ", " ") : "";
            string order_statement = Sorter != null ? Sorter.ToString(" ORDER BY ", " ") : "";
            string limit_statement = QueryBuilder.BuildLimit(Limit, pageIndex);
            string groupby_statement = GroupBy.Length > 0 ? " GROUP BY " + GroupBy + " " : "";

            List<string[]> newList = Connection.Select(BaseSql + where_statement + groupby_statement + order_statement + limit_statement);
            if (newList == null)
                return null;
            
            List<string[]> countResult = Connection.Select(CountSql + where_statement);
            if (countResult == null)
                return null;
            int count = Int32.Parse(countResult[0][0]);


            //Если номер страницы вышел за пределы, подгружаем послеюнюю страницу
            if (count > 0 && newList.Count == 0) {
                pageIndex = (int)(count / (double)Limit);
                limit_statement = QueryBuilder.BuildLimit(Limit, pageIndex);
                newList = Connection.Select(BaseSql + where_statement + order_statement + limit_statement);
                if (newList == null)
                    return null;
            }

            List = newList;
            PageIndex = pageIndex;
            Count = count;
            Pages = Limit > 0 ? (int)Math.Ceiling(Count / (double)Limit) : 0;

            return List;
        }

    }

}
