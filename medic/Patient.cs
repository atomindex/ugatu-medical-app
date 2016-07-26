using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс пациента
    public class Patient : Entity {

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public string FirstName;                //Имя пациента
        public string MiddleName;               //Отчество пациента
        public string LastName;                 //Фамилия пациента
        public string Sex;                      //Пол пациента
        public string Birthday;                 //Дата рождения пациента



        //Статический конструктор
        static Patient() {
            tableName = "patients";
            fields = "patients.id, patients.first_name, patients.middle_name, patients.last_name, patients.sex, patients.birthday";
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
                    patient.loadData(data);
                    list.Add(patient);
                }
            }

            return list;
        }



        //Конструктор
        public Patient(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT id, " + fields + " FROM " + tableName + " WHERE removed = 0 AND id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            loadData(data[0]);
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

        //Сохраняет пациента
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { FirstName, MiddleName, LastName, Sex, Birthday }
            );
            return id;
        }

        //Удаляет пациента
        public override void Remove() {
            remove(tableName);
        }



        //Загружает данные в поля
        private void loadData(string[] data) {
            id = Int32.Parse(data[0]);
            FirstName = data[1];
            MiddleName = data[2];
            LastName = data[3];
            Sex = data[4];
            Birthday = data[5];
        }
    
    }

}
