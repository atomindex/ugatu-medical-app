using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс категории
    public class Category : Entity {

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public string Name;                     //Название категории


        //Статический конструктор
        static Category() {
            tableName = "categories";
            fields = "categories.id, categories.name";
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

        public static string GetFields() {
            return fields;
        }

        //Возвращает данные списка категорий
        public static ListData GetListData(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Category.fields + " FROM " + Category.GetTableName();
            string countSql = "SELECT count(*) as row_count FROM " + Category.GetTableName();
            
            //Создаем фильтр по статусу Удален
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Category.GetFieldName("removed"), SqlComparisonOperator.Equal);
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

        //Возвращает данные списка категорий
        public static ListData GetPatientCategoriesListData(int workerId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Category.fields +
                            " FROM " + Category.GetTableName() +
                            " JOIN patients_categories ON patients_categories.category_id = " + Category.GetFieldName("id");

            string countSql = "SELECT count(*) as row_count FROM " + Category.GetTableName() +
                             " JOIN patients_categories ON patients_categories.category_id = " + Category.GetFieldName("id");

            //Создаем фильтр по статусу Удален и идентификатору пациента
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Category.GetFieldName("removed"), SqlComparisonOperator.Equal);
            SqlFilterCondition patientFilter = new SqlFilterCondition("patients_categories.patient_id", SqlComparisonOperator.Equal);

            removedFilter.SetValue("0");
            patientFilter.SetValue(workerId.ToString());

            filterGroup.AddItem(removedFilter);
            filterGroup.AddItem(patientFilter);
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

        //Возвращает список категорий из данных списка
        public static List<Category> GetList(ListData listData) {
            //Формируем список категорий
            List<Category> list = new List<Category>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    Category category = new Category(listData.Connection);
                    category.LoadData(data);
                    list.Add(category);
                }
            }

            return list;
        }


        //Конструктор
        public Category(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT " + fields + " FROM " + tableName + " WHERE id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            LoadData(data[0]);
        }



        //Возвращает копию
        public Category Clone() {
            Category category = new Category(connection);
            category.id = id;
            category.Name = Name;
            return category;
        }

        //Сохраняет категорию
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { Name }
            );
            return id;
        }

        //Удаляет категорию
        public override void Remove() {
            remove(tableName);
        }



        //Добавляет список сотрудников предоставляющих категорию
        public int AddPatients(List<Patient> patientsList) {
            if (patientsList.Count == 0) return 0;

            string[] values = new string[patientsList.Count];
            for (int i = 0; i < patientsList.Count; i++)
                values[i] = "(" + id + ", " + patientsList[i].GetId().ToString() + ")";

            string sql = "INSERT IGNORE INTO patients_categories (category_id, patient_id) VALUES " + String.Join(",", values);
            return connection.Insert(sql);
        }

        //Удаляет список сотрудников предоставляющих категорию
        public int RemovePatients(List<Patient> patientsList) {
            if (patientsList.Count == 0) return 0;

            string[] patientsIds = new string[patientsList.Count];
            for (int i = 0; i < patientsList.Count; i++)
                patientsIds[i] = patientsList[i].GetId().ToString();
            string sql = "DELETE FROM patients_categories WHERE category_id = " + id + " AND patient_id IN " + QueryBuilder.BuildInStatement(patientsIds);
            return connection.Delete(sql);
        }



        //Загружает данные в поля
        public void LoadData(string[] data) {
            id = Int32.Parse(data[0]);
            Name = data[1];
        }

    }

}
