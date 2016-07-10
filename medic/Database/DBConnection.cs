using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Database {

    //Вспомогательный класс подключения к базе
    public class DBConnection {

        private MySqlConnection connection;     //Подключение к базе



        //Конструктор
        public DBConnection(string server, string database, string username, string password) {
            connection = new MySqlConnection(
                "SERVER=" + server + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + username + ";" +
                "PASSWORD=" + password + ";"
            );
        }



        //Соединение с базой
        public bool OpenConnection() {
            try {
                connection.Open();
                return true;
            } catch (MySqlException ex) {
                switch (ex.Number) {
                    case 0:
                        MessageBox.Show("Не удалось установить соединение с сервером");
                        break;

                    case 1045:
                        MessageBox.Show("Неверный логин или пароль");
                        break;

                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
                return false;
            }
        }

        //Отключение от базы
        public bool CloseConnection() {
            try {
                connection.Close();
                return true;
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }



        //Функция для выполняния Insert запроса, возвращает id записи
        public int Insert(string query) {
            MySqlCommand command = new MySqlCommand(query, connection);
            if (command.ExecuteNonQuery() > 0) {
                return (int)command.LastInsertedId;
            } else return -1;
        }

        //Функция для выполнения Update запроса, возвращает количество обновленных записей
        public int Update(string query) {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteNonQuery();
        }

        //Функция для выполнения Delete запроса, возвращает количество удаленных записей
        public int Delete(string query) {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteNonQuery();
        }

        //Функция для выполнения Select запроса, возвращает список массивов
        public List<string[]> Select(string query) {
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> result = new List<string[]>();

            while (reader.Read()) {
                string[] row = new string[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                    row[i] = reader.GetString(i);

                result.Add(row);
            }

            reader.Close();

            return result.Count > 0 ? result : null;
        }

    }

}
