using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс пациента
    public class Patient : Entity {

        public static string[] SexKeys;
        public static string[] SexValues;

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public string FirstName;                //Имя пациента
        public string MiddleName;               //Отчество пациента
        public string LastName;                 //Фамилия пациента
        public int Sex;                         //Пол пациента
        public DateTime Birthday;               //Дата рождения пациента



        //Статический конструктор
        static Patient() {
            SexKeys = new string[] { "-1", "0", "1" };
            SexValues = new string[] { "Не выбрано", "Мужчина", "Женщина" };

            tableName = "patients";
            fields = "patients.id, patients.first_name, patients.middle_name, patients.last_name, patients.sex, DATE_FORMAT(patients.birthday,'%Y-%m-%d')";
            fieldsArray = new string[] { "first_name", "middle_name", "last_name", "sex", "birthday" };
        }



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает имя поля с таблицей
        public static string GetFieldName(string field) {
            return tableName + "." + field;
        }

        //Возвращает список полей
        public static string GetFields() {
            return fields;
        }

        //Возвращает название пола по индексу
        public static string GetSex(int sex) {
            int index = Array.IndexOf(SexKeys, sex.ToString());
            return SexValues[index > -1 ? index : 0];
        }



        //Возвращает данные списка пациентов
        public static ListData GetListData(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Patient.fields + " FROM " + Patient.GetTableName();
            string countSql = "SELECT count(*) as row_count FROM " + Patient.GetTableName();

            //Создаем фильтр по статусу Удален
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Patient.GetFieldName("removed"), SqlComparisonOperator.Equal);
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

        //Возвращает список пациентов из данных списка
        public static List<Patient> GetList(ListData listData) {
            //Формируем список пациентов
            List<Patient> list = new List<Patient>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    Patient patient = new Patient(listData.Connection);
                    patient.LoadData(data);
                    list.Add(patient);
                }
            }

            return list;
        }



        //Конструктор
        public Patient(DBConnection connection, int id = 0) : base(connection) {
            Sex = -1;

            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT " + fields + " FROM " + tableName + " WHERE id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            LoadData(data[0]);
        }



        //Возвращает копию
        public Patient Clone() {
            Patient patient = new Patient(connection);
            patient.id = id;
            patient.FirstName = FirstName;
            patient.MiddleName = MiddleName;
            patient.LastName = LastName;
            patient.Sex = Sex;
            patient.Birthday = Birthday;
            return patient;
        }

        //Возвращает полное имя пациента
        public string GetFullName() {
            string fullName = "";

            if (FirstName.Length > 0) fullName += FirstName + " ";
            if (LastName.Length > 0) fullName += LastName + " ";
            if (MiddleName.Length > 0) fullName += MiddleName + " ";

            return fullName.TrimEnd();
        }


        public string GetStringSex() {
            return GetSex(Sex);
        }

        public int GetAge() {
            DateTime now = DateTime.Now;
            int years = now.Year - Birthday.Year;
            if (now.Month < Birthday.Month || now.Month == Birthday.Month && now.Day < Birthday.Day)
                years--;
            return years;
        }

        public int GetVisitCount() {
            string sql = "SELECT COUNT(*) FROM visits WHERE removed = 0 AND patient_id = " + id.ToString();
            List<string[]> result = connection.Select(sql);
            return Int32.Parse(result[0][0]);
        }

        public int[] GetCategoriesIds() {
            string sql = "SELECT patients_categories.category_id FROM patients_categories" +
                        " JOIN categories ON categories.id = patients_categories.category_id " + 
                        " WHERE categories.removed = 0 AND patient_id = " + id.ToString();
            List<string[]> result = connection.Select(sql);

            int[] ids = new int[result.Count];
            for (int i = 0; i < result.Count; i++)
                ids[i] = Int32.Parse(result[i][0]);

            return ids;
        }


        //Сохраняет пациента
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { FirstName, MiddleName, LastName, Sex.ToString(), Birthday.ToString(AppConfig.DatabaseDateFormat) }
            );
            return id;
        }

        //Удаляет пациента
        public override void Remove() {
            remove(tableName);
        }



        //Добавляет список категорий
        public int AddCategories(List<Category> categoriesList) {
            if (categoriesList.Count == 0) return 0;

            string[] values = new string[categoriesList.Count];
            for (int i = 0; i < categoriesList.Count; i++)
                values[i] = "(" + id + ", " + categoriesList[i].GetId().ToString() + ")";

            string sql = "INSERT IGNORE INTO patients_categories (patient_id, category_id) VALUES " + String.Join(",", values);
            return connection.Insert(sql);
        }

        //Удаляет список категорий
        public int RemoveCategories(List<Category> categoriesList) {
            if (categoriesList.Count == 0) return 0;

            string[] categoriesIds = new string[categoriesList.Count];
            for (int i = 0; i < categoriesList.Count; i++)
                categoriesIds[i] = categoriesList[i].GetId().ToString();
            string sql = "DELETE FROM patients_categories WHERE patient_id = " + id + " AND category_id IN " + QueryBuilder.BuildInStatement(categoriesIds);
            return connection.Delete(sql);
        }



        //Загружает данные в поля
        public void LoadData(string[] data) {
            id = Int32.Parse(data[0]);
            FirstName = data[1];
            MiddleName = data[2];
            LastName = data[3];
            Sex = Int32.Parse(data[4]);
            Birthday = DateTime.ParseExact(data[5], AppConfig.DatabaseDateFormat, null);
        }
    
    }

}
