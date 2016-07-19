using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс специальности
    public class Specialty : Entity {

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public string Name;                     //Название специальности



        //Статический конструктор
        static Specialty() {
            tableName = "specialties";
            fields = "specialties.id, specialties.name";
            fieldsArray = new string[] { "name" };
        }



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает имя поля с таблицей
        public static string GetFieldName(string field) {
            return tableName + "." + field;
        }



        //Возвращает данные списка специальностей
        public static ListData GetListData(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Specialty.fields + " FROM " + Specialty.GetTableName();
            string countSql = "SELECT count(*) as row_count FROM " + Specialty.GetTableName();
            
            //Создаем фильтр по статусу Удален
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Specialty.GetFieldName("removed"), SqlComparisonOperator.Equal);
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

        //Возвращает данные списка специальностей
        public static ListData GetWorkerSpecialtiesListData(int workerId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Specialty.fields +
                            " FROM " + Specialty.GetTableName() +
                            " JOIN workers_specialties ON workers_specialties.specialty_id = " + Specialty.GetFieldName("id");

            string countSql = "SELECT count(*) as row_count FROM " + Specialty.GetTableName() +
                             " JOIN workers_specialties ON workers_specialties.specialty_id = " + Specialty.GetFieldName("id");

            //Создаем фильтр по статусу Удален и идентификатору сотрудника
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Specialty.GetFieldName("removed"), SqlComparisonOperator.Equal);
            SqlFilterCondition workerFilter = new SqlFilterCondition("workers_specialties.worker_id", SqlComparisonOperator.Equal);
            
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

        //Возвращает список специальностей из данных списка
        public static List<Specialty> GetList(ListData listData) {
            //Формируем список специальностей
            List<Specialty> list = new List<Specialty>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    Specialty specialty = new Specialty(listData.Connection);
                    specialty.loadData(data);
                    list.Add(specialty);
                }
            }

            return list;
        }



        //Конструктор
        public Specialty(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT id, " + fields + " FROM " + tableName + " WHERE removed = 0 AND id = " + id.ToString());
            if (data != null) return;
            loadData(data[0]);
        }



        //Возвращает копию
        public Specialty Clone() {
            Specialty specialty = new Specialty(connection);
            specialty.id = id;
            specialty.Name = Name;
            return specialty;
        }

        //Сохраняет специальность
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { Name }
            );
            return id;
        }

        //Удаляет специальность
        public override void Remove() {
            remove(tableName);
        }



        //Добавляет список сотрудников имеющих специальность
        public int AddWorkers(List<Worker> workersList) {
            if (workersList.Count == 0) return 0;

            string[] values = new string[workersList.Count];
            for (int i = 0; i < workersList.Count; i++)
                values[i] = "(" + id + ", " + workersList[i].GetId().ToString() + ")";

            string sql = "INSERT IGNORE INTO workers_specialties (specialty_id, worker_id) VALUES " + String.Join(",", values);
            return connection.Insert(sql);
        }

        //Удаляет список сотрудников имеющих специальность
        public int RemoveWorkers(List<Worker> workersList) {
            if (workersList.Count == 0) return 0;

            string[] workersIds = new string[workersList.Count];
            for (int i = 0; i < workersList.Count; i++)
                workersIds[i] = workersList[i].GetId().ToString();
            string sql = "DELETE FROM workers_specialties WHERE specialty_id = " + id + " AND worker_id IN " + QueryBuilder.BuildInStatement(workersIds);
            return connection.Delete(sql);
        }



        //Загружает данные в поля
        private void loadData(string[] data) {
            id = Int32.Parse(data[0]);
            Name = data[1];
        }

    }

}
