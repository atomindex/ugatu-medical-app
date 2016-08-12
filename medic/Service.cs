using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс услуги
    public class Service : Entity {

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public string Name;                     //Название услуги
        public int Price;                       //Цена услуги



        //Статический конструктор
        static Service() {
            tableName = "services";
            fields = "services.id, services.name, services.price";
            fieldsArray = new string[] { "name", "price" };
        }



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает имя поля с таблицей
        public static string GetFieldName(string field) {
            return tableName + "." + field;
        }

        public static string GetFields() {
            return fields;
        }

        //Возвращает данные списка услуг
        public static ListData GetListData(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Service.fields + " FROM " + Service.GetTableName();
            string countSql = "SELECT count(*) as row_count FROM " + Service.GetTableName();
            
            //Создаем фильтр по статусу Удален
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Service.GetFieldName("removed"), SqlComparisonOperator.Equal);
            removedFilter.SetValue("0");
            filterGroup.AddItem(removedFilter);
            filterGroup.AddItem(filter);
            
            return new ListData(
                connection: connection,
                baseSql: baseSql,
                countSql: countSql,
                filter: filterGroup,
                sorter: sorter,
                limit: limit,
                pageIndex: pageIndex
            );
        }

        //Возвращает данные списка услуг
        public static ListData GetWorkerServicesListData(int workerId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Service.fields +
                            " FROM " + Service.GetTableName() +
                            " JOIN workers_services ON workers_services.service_id = " + Service.GetFieldName("id");

            string countSql = "SELECT count(*) as row_count FROM " + Service.GetTableName() +
                             " JOIN workers_services ON workers_services.service_id = " + Service.GetFieldName("id");

            //Создаем фильтр по статусу Удален и идентификатору сотрудника
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Service.GetFieldName("removed"), SqlComparisonOperator.Equal);
            SqlFilterCondition workerFilter = new SqlFilterCondition("workers_services.worker_id", SqlComparisonOperator.Equal);
            
            removedFilter.SetValue("0");
            workerFilter.SetValue(workerId.ToString());

            filterGroup.AddItem(removedFilter);
            filterGroup.AddItem(workerFilter);
            filterGroup.AddItem(filter);

            return new ListData(
                connection: connection,
                baseSql: baseSql,
                countSql: countSql,
                filter: filterGroup,
                sorter: sorter,
                limit: limit,
                pageIndex: pageIndex
            );
        }



        //Возвращает список услуг из данных списка
        public static List<Service> GetList(ListData listData) {
            //Формируем список услуг
            List<Service> list = new List<Service>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    Service service = new Service(listData.Connection);
                    service.LoadData(data);
                    list.Add(service);
                }
            }

            return list;
        }


        //Конструктор
        public Service(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT " + fields + " FROM " + tableName + " WHERE id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            LoadData(data[0]);
        }



        //Возвращает копию
        public Service Clone() {
            Service service = new Service(connection);
            service.id = id;
            service.Name = Name;
            service.Price = Price;
            return service;
        }

        //Сохраняет услугу
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { Name, Price.ToString() }
            );
            return id;
        }

        //Удаляет услугу
        public override void Remove() {
            remove(tableName);
        }



        //Добавляет список сотрудников предоставляющих услугу
        public int AddWorkers(List<Worker> workersList) {
            if (workersList.Count == 0) return 0;

            string[] values = new string[workersList.Count];
            for (int i = 0; i < workersList.Count; i++)
                values[i] = "(" + id + ", " + workersList[i].GetId().ToString() + ")";

            string sql = "INSERT IGNORE INTO workers_services (service_id, worker_id) VALUES " + String.Join(",", values);
            return connection.Insert(sql);
        }

        //Удаляет список сотрудников предоставляющих услугу
        public int RemoveWorkers(List<Worker> workersList) {
            if (workersList.Count == 0) return 0;

            string[] workersIds = new string[workersList.Count];
            for (int i = 0; i < workersList.Count; i++)
                workersIds[i] = workersList[i].GetId().ToString();
            string sql = "DELETE FROM workers_services WHERE service_id = " + id + " AND worker_id IN " + QueryBuilder.BuildInStatement(workersIds);
            return connection.Delete(sql);
        }



        //Загружает данные в поля
        public void LoadData(string[] data) {
            id = Int32.Parse(data[0]);
            Name = data[1];
            Price = Int32.Parse(data[2]);
        }

    }

}
