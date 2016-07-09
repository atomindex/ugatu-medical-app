using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {
    public abstract class Entity {

        protected DBConnection connection;      //Соединение с базой

        protected int id;                       //Идентификатор записи



        //Конструктор
        public Entity(DBConnection connection) {
            this.connection = connection;
            id = -1;
        }



        //Сохраняет запись
        public abstract int Save();

        //Удаляет запись
        public abstract void Remove();

        //Возвращает идентификатор
        public long GetId() {
            return id;
        }



        //Сохраняет запись
        protected int save(string table, string[] fields, string[] values) {
            string[] escapedValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                escapedValues[i] = QueryBuilder.EscapeString(values[i], true);

            if (id <= 0) {
                id = connection.Insert("INSERT INTO " + table + " (" + String.Join(", ", fields) + ") VALUES (" + String.Join(",", escapedValues) + ")");
            } else {
                string[] updateValues = new string[escapedValues.Length];
                for (int i = 0; i < escapedValues.Length; i++)
                    updateValues[i] = fields[i] + " = " + escapedValues[i];
                connection.Update("UPDATE " + table + " SET " + String.Join(",", updateValues) + " WHERE id = " + id);
            }
            return id;
        }

        //Удаляет запись
        protected void remove(string table) {
            connection.Update("UPDATE " + table + " SET removed = 1 WHERE id = " + id.ToString());
        }

    }
}
