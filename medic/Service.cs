using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {
    public class Service : Entity {

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public string Name;                //Имя сотрудника
        public int Price;               //Отчество сотрудника



        //Статический конструктор
        static Service() {
            tableName = "services";
            fields = "name, price";
            fieldsArray = new string[] { "name", "price" };
        }



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает список сотрудников
        public static ListData<Service> GetList(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string where_statement = filter != null ? filter.ToString("AND ", " ") : "";
            string order_statement = sorter != null ? sorter.ToString("ORDER BY ", " ") : "";
            string limit_statement = QueryBuilder.BuildLimit(limit, pageIndex);
            
            //Получаем список услуг и общее количество
            List<string[]> data = connection.Select("SELECT id, " + Service.fields + " FROM " + Service.tableName + " WHERE removed = 0 " + order_statement + where_statement + limit_statement);
            List<string[]> countData = connection.Select("SELECT count(*) as row_count FROM " + Service.tableName + " WHERE removed = 0 " + where_statement);
            int count = Int32.Parse(countData[0][0]);

            //Если номер страницы вышел за пределы, подгружаем послеюнюю страницу
            if (count > 0 && data == null)
                return GetList(connection, (int)(count / (double)limit));

            //Формируем список услуг
            List<Service> list = new List<Service>();
            if (data != null)
                for (int i = 0; i < data.Count; i++) {
                    Service service = new Service(connection);
                    service.loadData(data[i]);
                    list.Add(service);
                }

            return new ListData<Service>(list, count, limit, pageIndex);
        }



        //Конструктор
        public Service(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT id, " + fields + " FROM " + tableName + " WHERE removed = 0 AND id = " + id.ToString());
            if (data != null) return;
            loadData(data[0]);
        }



        //Сохраняет сотрудника
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { Name, Price.ToString() }
            );
            return id;
        }

        //Удаляет сотрудника
        public override void Remove() {
            remove(tableName);
        }



        //Загружает данные в поля
        private void loadData(string[] data) {
            id = Int32.Parse(data[0]);
            Name = data[1];
            Price = Int32.Parse(data[2]);
        }

    }
}
