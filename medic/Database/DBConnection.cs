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
        private bool transactionStarted;


        //Конструктор
        public DBConnection(string server, string database, string username, string password, int closeTimeout = 60) {
            connection = new MySqlConnection(
                "SERVER=" + server + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + username + ";" +
                "PASSWORD=" + password + ";" +
                "Convert Zero Datetime=True;Allow Zero Datetime=False;"
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

            if (transactionStarted) return false;

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
            if (!OpenConnection()) return -1;

            try {
                MySqlCommand command = new MySqlCommand(query, connection);
                int rowsCount = command.ExecuteNonQuery();
                return rowsCount > 0 ? (int)command.LastInsertedId : 0;
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        //Функция для выполнения Update запроса, возвращает количество обновленных записей
        public int Update(string query) {
            if (!OpenConnection()) return -1;

            try {
                MySqlCommand command = new MySqlCommand(query, connection);
                int rowsCount = command.ExecuteNonQuery();
                return rowsCount;
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        //Функция для выполнения Delete запроса, возвращает количество удаленных записей
        public int Delete(string query) {
            if (!OpenConnection()) return -1;

            try {
                MySqlCommand command = new MySqlCommand(query, connection);
                int rowsCount = command.ExecuteNonQuery();
                return rowsCount;
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        //Функция для выполнения Select запроса, возвращает список массивов с данными
        public List<string[]> Select(string query) {
            if (!OpenConnection()) return null;

            MySqlDataReader reader;
            try {
                MySqlCommand command = new MySqlCommand(query, connection);
                reader = command.ExecuteReader();
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
            
            List<string[]> result = new List<string[]>();

            while (reader.Read()) {
                string[] row = new string[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                    row[i] = reader.GetString(i);

                result.Add(row);
            }

            reader.Close();

            return result;
        }



        public bool StartTransaction() {
            if (transactionStarted) return false;
            if (!OpenConnection()) return false;

            try {
                MySqlCommand command = new MySqlCommand("SET AUTOCOMMIT = 0; START TRANSACTION;", connection);
                command.ExecuteNonQuery();
                transactionStarted = true;
                return true;
            } catch (MySqlException ex) {
                transactionStarted = false;
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool CommitTransaction() {
            if (!transactionStarted) return false;
            if (!OpenConnection()) return false;

            try {
                MySqlCommand command = new MySqlCommand("COMMIT; SET AUTOCOMMIT = 1;", connection);
                command.ExecuteNonQuery();
                transactionStarted = false;
                return true;
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool RollbackTransaction() {
            if (!transactionStarted) return false;
            if (!OpenConnection()) return false;

            try {
                MySqlCommand command = new MySqlCommand("ROLLBACK; SET AUTOCOMMIT = 1;", connection);
                command.ExecuteNonQuery();
                transactionStarted = false;
                return true;
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        //Автоотключение базы
        private void closeTimerTick_Event(object sender, EventArgs e) {
            CloseConnection();
        }

    }

}
