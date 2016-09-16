using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace medic {

    //Класс настроек приложения
    public static class AppConfig {

        public static Color BlueColor = Color.FromArgb(0, 108, 255);
        public static Color LightBlueColor = Color.FromArgb(194, 219, 255);
        public static Color OrangeColor = Color.FromArgb(255, 108, 0);
        public static Color LightOrangeColor = Color.FromArgb(255, 220, 195);

        public static string DatabaseDateFormat = "yyyy-MM-dd";
        public static string DateFormat = "dd.MM.yyyy";
        public static DateTime MinDate = new DateTime(1901, 1, 1, 0, 0, 0);

        public static string DatabaseHost = "";
        public static string DatabaseName = "";
        public static string DatabaseUser = "";
        public static string DatabasePassword = "";
        public static int DatabaseConnectTime = 0;

        public static int BaseLimit = 30;

        static AppConfig() {

            try {
                //Загружаем файл настроек
                XmlDocument doc = new XmlDocument();
                doc.Load(Application.StartupPath + "\\medic.xml");

                //Получаем настройки подключения базы данных
                XmlNodeList databases = doc.GetElementsByTagName("database");
                XmlNode database = databases[0];

                DatabaseHost = database.Attributes.GetNamedItem("host").Value;
                DatabaseName = database.Attributes.GetNamedItem("dbname").Value;
                DatabaseUser = database.Attributes.GetNamedItem("user").Value;
                DatabasePassword = database.Attributes.GetNamedItem("password").Value;
                DatabaseConnectTime = Int32.Parse(database.Attributes.GetNamedItem("duration").Value);
            } catch (Exception e) {
                MessageBox.Show("Ошибка загрузки настроек приложения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

    }

}
