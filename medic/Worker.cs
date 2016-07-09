using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс сотрудника
    public class Worker : Entity {

        public string FirstName;        //Имя сотрудника
        public string MiddleName;       //Отчество сотрудника
        public string LastName;         //Фамилия сотрудника
        public string Phone;            //Телефон сотрудника
        public string Address;          //Адрес сотрудника

        private static string tableName = "workers";
        private static string fields = "first_name, middle_name, last_name, phone, address";
        private static string[] fieldsArray = new string[] { "first_name", "middle_name", "last_name", "phone", "address" };



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает список сотрудников
        public static ListData<Worker> GetList(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string where_statement = filter != null ? filter.ToString("AND ", " ") : "";
            string order_statement = sorter != null ? sorter.ToString("ORDER BY ", " ") : "";
            string limit_statement = QueryBuilder.BuildLimit(limit, pageIndex);
            
            //Получаем список сотрудников и общее количество
            List<string[]> data = connection.Select("SELECT id, " + Worker.fields + " FROM " + Worker.tableName + " WHERE removed = 0 " + order_statement + where_statement + limit_statement);
            List<string[]> countData = connection.Select("SELECT count(*) as row_count FROM " + Worker.tableName + " WHERE removed = 0 " + where_statement);
            int count = Int32.Parse(countData[0][0]);

            //Если номер страницы вышел за пределы, подгружаем послеюнюю страницу
            if (count > 0 && data == null)
                return GetList(connection, (int)(count / (double)limit));

            //Формируем список сотрудников
            List<Worker> list = new List<Worker>();
            if (data != null)
                for (int i = 0; i < data.Count; i++) {
                    Worker worker = new Worker(connection);
                    worker.loadData(data[i]);
                    list.Add(worker);
                }

            return new ListData<Worker>(list, count, limit, pageIndex);
        }



        //Конструктор
        public Worker(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT id, " + fields + " FROM " + tableName + " WHERE removed = 0 AND id = " + id.ToString());
            if (data != null) return;
            loadData(data[0]);
        }



        //Сохраняет сотрудника
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { FirstName, MiddleName, LastName, Phone, Address }
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
            FirstName = data[1];
            MiddleName = data[2];
            LastName = data[3];
            Phone = data[4];
            Address = data[5];
        }
    
    }
}
