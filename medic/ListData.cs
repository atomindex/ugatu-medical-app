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

        //Список
        public List<string[]> List {
            get;
            private set;
        }



        //Конструктор
        public ListData(DBConnection connection, string baseSql, string countSql, SqlFilter filter, SqlSorter sorter, int limit, int pageIndex) {
            this.Connection = connection;

            this.BaseSql = baseSql;
            this.CountSql = countSql;

            this.Filter = filter;
            this.Sorter = sorter;

            this.Limit = limit;
            this.PageIndex = pageIndex;
        }



        //Обновления списка
        public List<string[]> Update(int pageIndex = 0) {
            PageIndex = pageIndex;

            string where_statement = Filter != null ? Filter.ToString(" WHERE ", " ") : "";
            string order_statement = Sorter != null ? Sorter.ToString(" ORDER BY ", " ") : "";
            string limit_statement = QueryBuilder.BuildLimit(Limit, pageIndex);

            List = Connection.Select(BaseSql + where_statement + order_statement + limit_statement);
            List<string[]> countResult = Connection.Select(CountSql + where_statement);
            Count = Int32.Parse(countResult[0][0]);

            //Если номер страницы вышел за пределы, подгружаем послеюнюю страницу
            if (Count > 0 && List.Count == 0) {
                pageIndex = (int)(Count / (double)Limit);
                limit_statement = QueryBuilder.BuildLimit(Limit, pageIndex);
                List = Connection.Select(BaseSql + where_statement + order_statement + limit_statement);
            }

            Pages = Limit > 0 ? (int)Math.Ceiling(Count / (double)Limit) : 0;

            return List;
        }

    }

}
