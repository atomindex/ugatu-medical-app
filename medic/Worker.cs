using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс сотрудника
    public class Worker : Entity {

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public string FirstName;                //Имя сотрудника
        public string MiddleName;               //Отчество сотрудника
        public string LastName;                 //Фамилия сотрудника
        public string Phone;                    //Телефон сотрудника
        public string Address;                  //Адрес сотрудника



        //Статический конструктор
        static Worker() {
            tableName = "workers";
            fields = "workers.id, workers.first_name, workers.middle_name, workers.last_name, workers.phone, workers.address";
            fieldsArray = new string[] { "first_name", "middle_name", "last_name", "phone", "address" };
        }



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает имя поля с таблицей
        public static string GetFieldName(string field) {
            return tableName + "." + field;
        }



        //Возвращает данные списка сотрудников
        public static ListData GetListData(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Worker.fields + " FROM " + Worker.GetTableName();
            string countSql = "SELECT count(*) as row_count FROM " + Worker.GetTableName();

            //Создаем фильтр по статусу Удален
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Worker.GetFieldName("removed"), SqlComparisonOperator.Equal);
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

        //Возвращает данные списка сотрудников имеющие указанную специальность
        public static ListData GetSpecialtyWorkersListData(int specialtyId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Worker.fields +
                            " FROM " + Worker.GetTableName() +
                            " JOIN workers_specialties ON workers_specialties.worker_id = " + Worker.GetFieldName("id");

            string countSql = "SELECT count(*) as row_count FROM " + Worker.GetTableName() +
                             " JOIN workers_specialties ON workers_specialties.worker_id = " + Worker.GetFieldName("id");

            //Создаем фильтр по статусу Удален и идентификатору сервиса
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Worker.GetFieldName("removed"), SqlComparisonOperator.Equal);
            SqlFilterCondition serviceFilter = new SqlFilterCondition("workers_specialties.specialty_id", SqlComparisonOperator.Equal);

            removedFilter.SetValue("0");
            serviceFilter.SetValue(specialtyId.ToString());

            filterGroup.AddItem(removedFilter);
            filterGroup.AddItem(serviceFilter);
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

        //Возвращает данные списка сотрудников предоставляющих указанную услугу
        public static ListData GetServiceWorkersListData(int serviceId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Worker.fields +
                            " FROM " + Worker.GetTableName() +
                            " JOIN workers_services ON workers_services.worker_id = " + Worker.GetFieldName("id");
            
            string countSql = "SELECT count(*) as row_count FROM " + Worker.GetTableName() +
                             " JOIN workers_services ON workers_services.worker_id = " + Worker.GetFieldName("id");

            //Создаем фильтр по статусу Удален и идентификатору сервиса
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Worker.GetFieldName("removed"), SqlComparisonOperator.Equal);
            SqlFilterCondition serviceFilter = new SqlFilterCondition("workers_services.service_id", SqlComparisonOperator.Equal);
            
            removedFilter.SetValue("0");
            serviceFilter.SetValue(serviceId.ToString());

            filterGroup.AddItem(removedFilter);
            filterGroup.AddItem(serviceFilter);
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

        //Возвращает список сотрудников из данных списка
        public static List<Worker> GetList(ListData listData) {
            //Формируем список сотрудников
            List<Worker> list = new List<Worker>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    Worker worker = new Worker(listData.Connection);
                    worker.loadData(data);
                    list.Add(worker);
                }
            }

            return list;
        }



        //Конструктор
        public Worker(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT id, " + fields + " FROM " + tableName + " WHERE removed = 0 AND id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            loadData(data[0]);
        }



        //Возвращает копию
        public Worker Clone() {
            Worker worker = new Worker(connection);
            worker.id = id;
            worker.FirstName = FirstName;
            worker.MiddleName = MiddleName;
            worker.LastName = LastName;
            worker.Phone = Phone;
            worker.Address = Address;
            return worker;
        }

        //Возвращает полное имя сотрудника
        public string GetFullName() {
            string fullName = "";

            if (FirstName.Length > 0) fullName += FirstName + " ";
            if (LastName.Length > 0) fullName += LastName + " ";
            if (MiddleName.Length > 0) fullName += MiddleName + " ";

            return fullName.TrimEnd();
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



        //Добавляет список предоставляемых услуг
        public int AddServices(List<Service> servicesList) {
            if (servicesList.Count == 0) return 0;

            string[] values = new string[servicesList.Count];
            for (int i = 0; i < servicesList.Count; i++)
                values[i] = "(" + id + ", " + servicesList[i].GetId().ToString() + ")";

            string sql = "INSERT IGNORE INTO workers_services (worker_id, service_id) VALUES " + String.Join(",", values);
            return connection.Insert(sql);
        }

        //Удаляет список предоставляемых услуг
        public int RemoveServices(List<Service> servicesList) {
            if (servicesList.Count == 0) return 0;

            string[] servicesIds = new string[servicesList.Count];
            for (int i = 0; i < servicesList.Count; i++)
                servicesIds[i] = servicesList[i].GetId().ToString();
            string sql = "DELETE FROM workers_services WHERE worker_id = " + id + " AND service_id IN " + QueryBuilder.BuildInStatement(servicesIds);
            return connection.Delete(sql);
        }



        //Добавляет список специальностей сотрудника
        public int AddSpecialties(List<Specialty> specialtiesList) {
            if (specialtiesList.Count == 0) return 0;

            string[] values = new string[specialtiesList.Count];
            for (int i = 0; i < specialtiesList.Count; i++)
                values[i] = "(" + id + ", " + specialtiesList[i].GetId().ToString() + ")";

            string sql = "INSERT IGNORE INTO workers_specialties (worker_id, specialty_id) VALUES " + String.Join(",", values);
            return connection.Insert(sql);
        }

        //Удаляет список специальностей
        public int RemoveSpecialties(List<Specialty> specialtiesList) {
            if (specialtiesList.Count == 0) return 0;

            string[] specialtiesIds = new string[specialtiesList.Count];
            for (int i = 0; i < specialtiesList.Count; i++)
                specialtiesIds[i] = specialtiesList[i].GetId().ToString();
            string sql = "DELETE FROM workers_specialties WHERE worker_id = " + id + " AND specialty_id IN " + QueryBuilder.BuildInStatement(specialtiesIds);
            return connection.Delete(sql);
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
