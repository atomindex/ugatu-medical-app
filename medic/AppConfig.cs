using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace medic {

    //Класс настроек приложения
    public static class AppConfig {

        public static Color BlueColor = Color.FromArgb(0, 108, 255);
        public static Color LightBlueColor = Color.FromArgb(194, 219, 255);
        public static Color OrangeColor = Color.FromArgb(255, 108, 0);
        public static Color LightOrangeColor = Color.FromArgb(255, 220, 195);

        public static string DateFormat = "dd.MM.yyyy";
        public static DateTime MinDate = new DateTime(1901, 1, 1, 0, 0, 0);

        public static string DatabaseHost = "localhost";
        public static string DatabaseName = "medic";
        public static string DatabaseUser = "root";
        public static string DatabasePassword = "";
        public static int DatabaseConnectTime = 60;

    }

}
