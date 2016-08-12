using medic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic {
    class VisitSale : Entity {

        private static int saleFieldsCount;
        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public int VisitId;
        public Sale RelatedSale;
        public int Percent;


        //Статический конструктор
        static VisitSale() {
            tableName = "visits_sales";
            fields = "visits_sales.id, visits_sales.sale_percent, visits_sales.visit_id, " + Sale.GetFields();
            fieldsArray = new string[] { "sale_percent", "visit_id", "sale_id" };

            saleFieldsCount = Sale.GetFields().Split(new char[] { ',' }).Length;
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
        public static ListData GetListData(int visitId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + VisitSale.fields +
                            " FROM " + VisitSale.GetTableName() + 
                            " JOIN " + Sale.GetTableName() + " ON " + Sale.GetFieldName("id") + " = " + GetFieldName("sale_id");

            string countSql = "SELECT count(*) as row_count FROM " + VisitSale.GetTableName() +
                             " JOIN " + Sale.GetTableName() + " ON " + Sale.GetFieldName("id") + " = " + GetFieldName("sale_id");

            //Создаем фильтр по идентификатору идентификатору посещения
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition visitFilter = new SqlFilterCondition(GetFieldName("visit_id"), SqlComparisonOperator.Equal);
            visitFilter.SetValue(visitId.ToString());

            filterGroup.AddItem(visitFilter);
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
        public static List<VisitSale> GetList(ListData listData) {
            //Формируем список скидок
            List<VisitSale> list = new List<VisitSale>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    VisitSale visitSale = new VisitSale(listData.Connection);
                    visitSale.loadData(data);
                    list.Add(visitSale);
                }
            }
            return list;
        }


        //Конструктор
        public VisitSale(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT " + fields + " FROM " + tableName + " WHERE id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            loadData(data[0]);
        }



        //Возвращает копию
        public VisitSale Clone() {
            VisitSale visitSale = new VisitSale(connection);
            visitSale.id = id;
            visitSale.Percent = Percent;
            visitSale.VisitId = VisitId;
            visitSale.RelatedSale = RelatedSale == null ? null : RelatedSale.Clone();
            return visitSale;
        }

        //Сохраняет скидку
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { Percent.ToString(), VisitId.ToString(), RelatedSale.GetId().ToString() }
            );
            return id;
        }

        //Удаляет скидку
        public override void Remove() {
            connection.Delete("DELETE FROM " + GetTableName() + " WHERE id = " + id.ToString());
        }



        //Загружает данные в поля
        private void loadData(string[] data) {
            string[] saleData = new string[saleFieldsCount];
            Array.Copy(data, 3, saleData, 0, saleFieldsCount);

            id = Int32.Parse(data[0]);
            Percent = Int32.Parse(data[1]);
            VisitId = Int32.Parse(data[2]);
            RelatedSale = new Sale(connection);
            RelatedSale.LoadData(saleData);
        }

    }

}
