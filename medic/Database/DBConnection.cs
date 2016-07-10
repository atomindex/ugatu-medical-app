using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace medic.Database {

    //Вспомогательный класс подключения к базе
    public class DBConnection {

        private MySqlConnection connection;     //Подключение к базе
        private Timer closeTimer;


        //Конструктор
        public DBConnection(string server, string database, string username, string password, int closeTimeout = 60) {
            connection = new MySqlConnection(
                "SERVER=" + server + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + username + ";" +
                "PASSWORD=" + password + ";"
            );

            closeTimer = new Timer();
            closeTimer.Interval = closeTimeout * 1000;
            closeTimer.Tick += closeTimerTick_Event;
        }



        //Соединение с базой
        public bool OpenConnection() {
            if (connection.State != ConnectionState.Closed) return true;
            
            try {
                connection.Open();
                closeTimer.Start();
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
            if (connection.State == ConnectionState.Closed) return true;

            try {
                connection.Close();
                closeTimer.Stop();
                return true;
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }



        //Функция для выполняния Insert запроса, возвращает id записи
        public int Insert(string query) {
            OpenConnection();
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteNonQuery() > 0 ? (int)command.LastInsertedId : -1;
        }

        //Функция для выполнения Update запроса, возвращает количество обновленных записей
        public int Update(string query) {
            OpenConnection();
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteNonQuery();
        }

        //Функция для выполнения Delete запроса, возвращает количество удаленных записей
        public int Delete(string query) {
            OpenConnection();
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteNonQuery();
        }

        //Функция для выполнения Select запроса, возвращает список массивов
        public List<string[]> Select(string query) {
            OpenConnection();

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



        //Автоотключение базы
        private void closeTimerTick_Event(object sender, EventArgs e) {
            CloseConnection();
        }

    }

}
