using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    public class SaleCondition {

        public string name;
        public string op;
        public string value;

        public SaleCondition(string name, string op = "", string value = "") {
            this.name = name;
            this.op = op;
            this.value = value;
        }

        public SaleCondition(string name, string conditionData) {
            this.name = name;
            SetCondition(conditionData);
        }

        public bool Compare(string comparedValue) {
            if (value == null || value.Length == 0 || op == null || op.Length == 0)
                return true;

            switch (op) {

                case "=":
                    return value == comparedValue;

                case "<>":
                    return value != comparedValue;

                case ">":
                    return Int32.Parse(value) > Int32.Parse(comparedValue);

                case "<":
                    return Int32.Parse(value) < Int32.Parse(comparedValue);

                case ">=":
                    return Int32.Parse(value) >= Int32.Parse(comparedValue);

                case "<=":
                    return Int32.Parse(value) <= Int32.Parse(comparedValue);

                case "%":
                    return Int32.Parse(comparedValue) % Int32.Parse(value) == 0;

                case "!%":
                    return Int32.Parse(comparedValue) % Int32.Parse(value) != 0;

            }

            return false;
        }

        public SaleCondition Clone() {
            return new SaleCondition(name, op, value);
        }

        public void SetCondition(string conditionData) {
            string[] conditionTokens = conditionData.Split(new string[] { "|" }, 2, StringSplitOptions.RemoveEmptyEntries);
            this.name = name;
            op = conditionTokens.Length > 0 ? conditionTokens[0] : "";
            value = conditionTokens.Length > 1 ? conditionTokens[1] : "";
        }

        public override string ToString() {
 	        return op + "|" + value;
        }

    }

    public class VisitData {
        public string ServicesCount;
        public string ServicesSumPrice;
        public string PatientAge;
        public string PatientSex;
        public string VisitNumber;
        public string VisitDate;
    }

    //Класс услуги
    public class Sale : Entity {

        public static string[] Operators = new string[] { "=", "<>", ">", "<", ">=", "<=", "%", "!%" };

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки
        private static string[] conditionsFieldsArray; //Массив полей с условиями

        public string Name;                     //Название скидки
        public string Description;              //Описание скидки
        public int Percent;                     //Процент скидки

        public SaleCondition CondServicesCount;
        public SaleCondition CondServicesSumPrice;
        public SaleCondition CondPatientAge;
        public SaleCondition CondPatientSex;
        public SaleCondition CondVisitNumber;
        public SaleCondition CondVisitDate;



        //Статический конструктор
        static Sale() {
            tableName = "sales";
            fieldsArray = new string[] { 
                "name", 
                "description", 
                "percent" 
            };
            conditionsFieldsArray = new string[] { 
                "cond_services_count", 
                "cond_services_sum_price", 
                "cond_patient_age", 
                "cond_patient_sex", 
                "cond_visit_number", 
                "cond_visit_date" 
            };
            
            StringBuilder sb = new StringBuilder();
            sb.Append("sales.id, sales.name, sales.description, sales.percent, ");
            for (int i = 0; i < conditionsFieldsArray.Length; i++) {
                sb.Append(Sale.GetFieldName(conditionsFieldsArray[i]));
                if (i < conditionsFieldsArray.Length - 1)
                    sb.Append(", ");
            }
            fields = sb.ToString();
        }



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает имя поля с таблицей
        public static string GetFieldName(string field) {
            return tableName + "." + field;
        }



        //Возвращает данные списка скидок
        public static ListData GetListData(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Sale.fields + " FROM " + Sale.GetTableName();
            string countSql = "SELECT count(*) as row_count FROM " + Sale.GetTableName();
            
            //Создаем фильтр по статусу Удален
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Sale.GetFieldName("removed"), SqlComparisonOperator.Equal);
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

        //Возвращает список скидок из данных списка
        public static List<Sale> GetList(ListData listData) {
            //Формируем список скидок
            List<Sale> list = new List<Sale>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    Sale sale = new Sale(listData.Connection);
                    sale.loadData(data);
                    list.Add(sale);
                }
            }

            return list;
        }



        //Конструктор
        public Sale(DBConnection connection, int id = 0) : base(connection) {
            CondServicesCount = new SaleCondition("cond_services_count");
            CondServicesSumPrice = new SaleCondition("cond_services_sum_price");
            CondPatientAge = new SaleCondition("cond_patient_age");
            CondPatientSex = new SaleCondition("cond_patient_sex");
            CondVisitNumber = new SaleCondition("cond_visit_number");
            CondVisitDate = new SaleCondition("cond_visit_date");

            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT id, " + fields + " FROM " + tableName + " WHERE removed = 0 AND id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }

            loadData(data[0]);
        }



        //Возвращает копию
        public Sale Clone() {
            Sale sale = new Sale(connection);
            sale.id = id;
            sale.Name = Name;
            sale.Description = Description;
            sale.Percent = Percent;

            sale.CondServicesCount = CondServicesCount.Clone();
            sale.CondServicesSumPrice = CondServicesSumPrice.Clone();
            sale.CondPatientAge = CondPatientAge.Clone();
            sale.CondPatientSex = CondPatientSex.Clone();
            sale.CondVisitNumber = CondVisitNumber.Clone();
            sale.CondVisitDate = CondVisitDate.Clone();

            return sale;
        }

        //Сохраняет скидку
        public override int Save() {
            id = save(
                tableName, 
                fieldsArray.Concat(conditionsFieldsArray).ToArray(),
                new string[] {
                    Name,
                    Description,
                    Percent.ToString(),
                    CondServicesCount.ToString(),
                    CondServicesSumPrice.ToString(),
                    CondPatientAge.ToString(),
                    CondPatientSex.ToString(),
                    CondVisitNumber.ToString(),
                    CondVisitDate.ToString()
                }
            );

            return id;
        }

        //Удаляет скидку
        public override void Remove() {
            remove(tableName);
        }



        //Загружает данные в поля
        private void loadData(string[] data) {
            id = Int32.Parse(data[0]);

            Name = data[1];
            Description = data[2];
            Percent = Int32.Parse(data[3]);

            CondServicesCount.SetCondition(data[4]);
            CondServicesSumPrice.SetCondition(data[5]);
            CondPatientAge.SetCondition(data[6]);
            CondPatientSex.SetCondition(data[7]);
            CondVisitNumber.SetCondition(data[8]);
            CondVisitDate.SetCondition(data[9]);
        }

    }

}
