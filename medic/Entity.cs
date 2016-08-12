using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    class EntityTransaction {
        public Entity entity;
        public int id;

        public EntityTransaction(Entity entity, int id) {
            this.entity = entity;
            this.id = id;
        }
    }

    //Базовый класс сущности
    public abstract class Entity {

        private static List<EntityTransaction> transactionObjects;

        protected DBConnection connection;      //Соединение с базой

        protected int id;                       //Идентификатор записи


        public static void StartTransaction() {
            transactionObjects = new List<EntityTransaction>();
        }

        public static void CommitTransaction() {
            transactionObjects.Clear();
            transactionObjects = null;
        }

        public static void RollbackTransaction() {
            foreach (EntityTransaction et in transactionObjects)
                et.entity.id = et.id;
            transactionObjects.Clear();
            transactionObjects = null;
        }

        //Конструктор
        public Entity(DBConnection connection) {
            this.connection = connection;
            id = -1;
        }


        //Возвращает соединение с базой
        public DBConnection GetConnection() {
            return connection;
        }

        //Возвращает идентификатор записи
        public int GetId() {
            return id;
        }



        //Сохраняет запись
        public abstract int Save();

        //Удаляет запись
        public abstract void Remove();



        //Сохраняет запись
        protected int save(string table, string[] fields, string[] values) {
            string[] escapedValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                escapedValues[i] = QueryBuilder.EscapeString(values[i], true);

            if (id <= 0) {
                if (transactionObjects != null)
                    transactionObjects.Add(new EntityTransaction(this, id));
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
        protected int remove(string table) {
            int result = connection.Update("UPDATE " + table + " SET removed = 1 WHERE id = " + id.ToString());
            return result;
        }

        protected int removePermanently(string table) {
            int result = connection.Delete("DELETE FROM " + table + " WHERE id = " + id.ToString());
            if (result > 0) {
                if (transactionObjects != null)
                    transactionObjects.Add(new EntityTransaction(this, id));
                id = -1;
            }
            return result;
        }
    }

}
