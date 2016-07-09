using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {
    public class Patient : Entity {

        public string FirstName;
        public string MiddleName;
        public string LastName;
        public int Sex;
        public long Birthday;

        private static string tableName = "patients";
        private static string fields = "first_name, middle_name, last_name, sex, birthday";
        private static string[] fieldsArray = new string[] { "first_name", "middle_name", "last_name", "sex", "birthday" };



        //Возвращает список пациентов
        public static List<Patient> GetList(DBConnection connection) {
            List<Patient> result = new List<Patient>();

            List<string[]> data = connection.Select("SELECT " + Patient.fields + " FROM " + Patient.tableName + " WHERE removed = 0");
            if (data == null) return result;

            for (int i = 0; i < data.Count; i++) {
                Patient patient = new Patient(connection);
                patient.loadData(data[i]);
                result.Add(patient);
            }

            return result;
        }



        //Конструктор
        public Patient(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT " + fields + " FROM " + tableName + " WHERE removed = 0 AND id = " + id.ToString());
            if (data != null) return;
            loadData(data[0]);
        }



        //Сохраняет пациента
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { FirstName, MiddleName, LastName, Sex.ToString(), Birthday.ToString() }
            );
            return id;
        }

        //Удаляет пациента
        public override void Remove() {
            remove(tableName);
        }



        //Загружает данные в поля
        private void loadData(string[] data) {
            FirstName = data[0];
            MiddleName = data[1];
            LastName = data[2];
            Sex = Int32.Parse(data[3]);
            Birthday = Int64.Parse(data[4]);
        }
    
    }
}
