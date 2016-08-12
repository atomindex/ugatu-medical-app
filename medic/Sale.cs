using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    public abstract class SaleCondition {

        public string name;
        public string op;

        public abstract bool Compare(object comparedValue);

        public abstract SaleCondition Clone();

        public abstract void SetCondition(string conditionData);
    }

    public class SaleConditionInt : SaleCondition {
        public static string[] Operators = new string[] { "", "=", "<>", ">", "<", ">=", "<=", "%", "!%" };
        public static string[] OperatorsForArray = new string[] { "", "=", "<>" };

        public int value;

        public SaleConditionInt(string name, string op = "", int value = 0) {
            this.name = name;
            this.op = op;
            this.value = value;
        }

        public override bool Compare(object comparedValue) {
            if (op == null || op.Length == 0)
                return true;

            int visitValue = (int)comparedValue;

            switch (op) {

                case "=":
                    return visitValue == value;

                case "<>":
                    return visitValue != value;

                case ">":
                    return visitValue > value;

                case "<":
                    return visitValue < value;

                case ">=":
                    return visitValue >= value;

                case "<=":
                    return visitValue <= value;

                case "%":
                    return visitValue % value == 0;

                case "!%":
                    return visitValue % value != 0;

            }

            return false;
        }

        public bool Compare(int[] comparedValue) {
            if (op == null || op.Length == 0)
                return true;

            switch (op) {

                case "=":
                    return Array.Exists<int>(comparedValue, element => element == value);

                case "<>":
                    return !Array.Exists<int>(comparedValue, element => element == value);

            }

            return false;
        }

        public override SaleCondition Clone() {
            return new SaleConditionInt(name, op, value);
        }

        public override void SetCondition(string conditionData) {
            string[] conditionTokens = conditionData.Split(new string[] { "|" }, 2, StringSplitOptions.None);
            op = conditionTokens.Length > 0 ? conditionTokens[0] : "";
            value = conditionTokens.Length > 1 ? Int32.Parse(conditionTokens[1]) : 0;
        }

        public override string ToString() {
            return op + "|" + value.ToString();
        }
    }


    public class SaleConditionDatetime : SaleCondition {

        public static string[] Operators = new string[] { "", "=", "<>", ">", "<", ">=", "<=" };

        public DateTime value;

        public SaleConditionDatetime(string name) {
            this.name = name;
            this.op = "";
            this.value = DateTime.MinValue;
        }

        public SaleConditionDatetime(string name, string op, DateTime value) {
            this.name = name;
            this.op = "";
            this.value = value;
        }

        public override bool Compare(object comparedValue) {
            if (op == null || op.Length == 0)
                return true;

            DateTime? visitValue = (DateTime?)comparedValue;

            switch (op) {
                case "=":
                    return visitValue == value;

                case "<>":
                    return visitValue != value;

                case ">":
                    return visitValue > value;

                case "<":
                    return visitValue < value;

                case ">=":
                    return visitValue >= value;

                case "<=":
                    return visitValue <= value;
            }

            return false;
        }

        public override SaleCondition Clone() {
            return new SaleConditionDatetime(name, op, value);
        }

        public override void SetCondition(string conditionData) {
            string[] conditionTokens = conditionData.Split(new string[] { "|" }, 2, StringSplitOptions.None);
            op = conditionTokens.Length > 0 ? conditionTokens[0] : "";
            value = conditionTokens.Length > 1 ? DateTime.ParseExact(conditionTokens[1], AppConfig.DatabaseDateFormat, null) : DateTime.MinValue;
        }

        public override string ToString() {
            return op + "|" + value.ToString(AppConfig.DatabaseDateFormat);
        }
    }


    public class VisitData {
        public int ServicesCount;
        public int ServicesSumPrice;
        public int PatientAge;
        public int PatientSex;
        public int[] PatientCategories;
        public int VisitNumber;
        public DateTime VisitDate;
    }

    //Класс услуги
    public class Sale : Entity {

        public static string[] Operators = new string[] { "=", "<>", ">", "<", ">=", "<=", "%", "!%" };

        private static string tableName;                //Имя таблицы
        private static string fields;                   //Поля таблицы для выборки
        private static string[] fieldsArray;            //Массив полей для вставки
        private static string[] conditionsFieldsArray;  //Массив полей с условиями

        public string Name;                             //Название скидки
        public string Description;                      //Описание скидки
        public int Percent;                             //Процент скидки

        public SaleConditionInt CondServicesCount;
        public SaleConditionInt CondServicesSumPrice;
        public SaleConditionInt CondPatientAge;
        public SaleConditionInt CondPatientSex;
        public SaleConditionInt CondPatientCategory;
        public SaleConditionInt CondVisitNumber;
        public SaleConditionDatetime CondVisitDate;



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
                "cond_patient_category",
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

        public static string GetFields() {
            return fields;
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
                    sale.LoadData(data);
                    list.Add(sale);
                }
            }

            return list;
        }


        public static List<Sale> GetSuitableSales(List<Sale> salesList, VisitData visitData) {
            List<Sale> suitableSales = new List<Sale>();

            foreach (Sale sale in salesList) {

                if (
                    sale.CondServicesCount.Compare(visitData.ServicesCount) &&
                    sale.CondServicesSumPrice.Compare(visitData.ServicesSumPrice) &&
                    sale.CondPatientAge.Compare(visitData.PatientAge) &&
                    sale.CondPatientSex.Compare(visitData.PatientSex) &&
                    sale.CondPatientCategory.Compare(visitData.PatientCategories) &&
                    sale.CondVisitNumber.Compare(visitData.VisitNumber) &&
                    sale.CondVisitDate.Compare(visitData.VisitDate)
                ) suitableSales.Add(sale);

            }

            return suitableSales;
        }


        //Конструктор
        public Sale(DBConnection connection, int id = 0) : base(connection) {
            CondServicesCount = new SaleConditionInt("cond_services_count");
            CondServicesSumPrice = new SaleConditionInt("cond_services_sum_price");
            CondPatientAge = new SaleConditionInt("cond_patient_age");
            CondPatientSex = new SaleConditionInt("cond_patient_sex", "", -1);
            CondPatientCategory = new SaleConditionInt("cond_patient_category");
            CondVisitNumber = new SaleConditionInt("cond_visit_number");
            CondVisitDate = new SaleConditionDatetime("cond_visit_date");

            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT id, " + fields + " FROM " + tableName + " WHERE id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }

            LoadData(data[0]);
        }



        //Возвращает копию
        public Sale Clone() {
            Sale sale = new Sale(connection);
            sale.id = id;
            sale.Name = Name;
            sale.Description = Description;
            sale.Percent = Percent;

            sale.CondServicesCount = (SaleConditionInt)CondServicesCount.Clone();
            sale.CondServicesSumPrice = (SaleConditionInt)CondServicesSumPrice.Clone();
            sale.CondPatientCategory = (SaleConditionInt)CondPatientCategory.Clone();
            sale.CondPatientAge = (SaleConditionInt)CondPatientAge.Clone();
            sale.CondPatientSex = (SaleConditionInt)CondPatientSex.Clone();
            sale.CondVisitNumber = (SaleConditionInt)CondVisitNumber.Clone();
            sale.CondVisitDate = (SaleConditionDatetime)CondVisitDate.Clone();

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
                    CondPatientCategory.ToString(),
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


        public int AddSales(List<Sale> saleList) {
            if (saleList.Count == 0) return 0;

            string[] values = new string[saleList.Count];
            for (int i = 0; i < saleList.Count; i++)
                values[i] = "(" + id.ToString() + "," + saleList[i].GetId().ToString() + "," + saleList[i].Percent.ToString() + ")";

            string sql = "INSERT IGNORE INTO visit_sales (visit_id, sale_id, sale_percent) VALUES " + String.Join(",", values);
            return connection.Insert(sql); ;
        }

        public int RemoveSales(List<Sale> saleList) {
            if (saleList.Count == 0) return 0;

            string[] salesIds = new string[saleList.Count];
            for (int i = 0; i < saleList.Count; i++)
                salesIds[i] = saleList[i].GetId().ToString();

            string sql = "DELETE FROM visit_sales WHERE visit_id = " + id.ToString() + " AND sale_id IN (" + String.Join(",", salesIds) + ")";
            return connection.Delete(sql);
        }


        //Загружает данные в поля
        public void LoadData(string[] data) {
            id = Int32.Parse(data[0]);

            Name = data[1];
            Description = data[2];
            Percent = Int32.Parse(data[3]);

            CondServicesCount.SetCondition(data[4]);
            CondServicesSumPrice.SetCondition(data[5]);
            CondPatientAge.SetCondition(data[6]);
            CondPatientSex.SetCondition(data[7]);
            CondPatientCategory.SetCondition(data[8]);
            CondVisitNumber.SetCondition(data[9]);
            CondVisitDate.SetCondition(data[10]);
        }

    }

}
