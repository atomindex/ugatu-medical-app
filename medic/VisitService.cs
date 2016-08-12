using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using medic.Database;

namespace medic {

    //Класс услуги
    public class VisitService : Entity {

        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки
        private static int serviceFieldsCount;
        private static int workerFieldsCount;

        public int Price;                       //Цена услуги
        public int VisitId;
        public Service RelatedService;
        public Worker RelatedWorker;


        //Статический конструктор
        static VisitService() {
            tableName = "visits_services";
            fields = "visits_services.id, visits_services.price, visits_services.visit_id, " + Service.GetFields() + ", " + Worker.GetFields();
            fieldsArray = new string[] { "price", "visit_id", "service_id", "worker_id" };

            serviceFieldsCount = Service.GetFields().Split(new char[] { ',' }).Length;
            workerFieldsCount = Worker.GetFields().Split(new string[] { ", " }, StringSplitOptions.None).Length;
        }



        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает имя поля с таблицей
        public static string GetFieldName(string field) {
            return tableName + "." + field;
        }



        //Возвращает данные списка услуг
        public static ListData GetListData(int visitId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + VisitService.fields +
                            " FROM " + VisitService.GetTableName() + 
                            " JOIN " + Service.GetTableName() + " ON " + Service.GetFieldName("id") + " = " + GetFieldName("service_id") +
                            " JOIN " + Worker.GetTableName() + " ON " + Worker.GetFieldName("id") + " = " + GetFieldName("worker_id");

            string countSql = "SELECT count(*) as row_count FROM " + VisitService.GetTableName() +
                             " JOIN " + Service.GetTableName() + " ON " + Service.GetFieldName("id") + " = " + GetFieldName("service_id") +
                             " JOIN " + Worker.GetTableName() + " ON " + Worker.GetFieldName("id") + " = " + GetFieldName("worker_id");

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


        //Возвращает список услуг из данных списка
        public static List<VisitService> GetList(ListData listData) {
            //Формируем список услуг
            List<VisitService> list = new List<VisitService>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    VisitService visitService = new VisitService(listData.Connection);
                    visitService.loadData(data);
                    list.Add(visitService);
                }
            }
            return list;
        }


        //Конструктор
        public VisitService(DBConnection connection, int id = 0) : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT " + fields + " FROM " + tableName + " WHERE id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            loadData(data[0]);
        }



        //Возвращает копию
        public VisitService Clone() {
            VisitService visitService = new VisitService(connection);
            visitService.id = id;
            visitService.Price = Price;
            visitService.VisitId = VisitId;
            visitService.RelatedService = RelatedService == null ? null : RelatedService.Clone();
            visitService.RelatedWorker = RelatedWorker == null ? null : RelatedWorker.Clone();
            return visitService;
        }

        //Сохраняет услугу
        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { Price.ToString(), VisitId.ToString(), RelatedService.GetId().ToString(), RelatedWorker.GetId().ToString() }
            );
            return id;
        }

        //Удаляет услугу
        public override void Remove() {
            connection.Delete("DELETE FROM " + GetTableName() + " WHERE id = " + id.ToString());

        }



        //Загружает данные в поля
        private void loadData(string[] data) {
            string[] serviceData = new string[serviceFieldsCount];
            Array.Copy(data, 3, serviceData, 0, serviceFieldsCount);

            string[] workerData = new string[workerFieldsCount];
            Array.Copy(data, serviceFieldsCount + 3, workerData, 0, workerFieldsCount);

            id = Int32.Parse(data[0]);
            Price = Int32.Parse(data[1]);
            VisitId = Int32.Parse(data[2]);
            RelatedService = new Service(connection);
            RelatedService.LoadData(serviceData);
            RelatedWorker = new Worker(connection);
            RelatedWorker.LoadData(workerData);
        }

    }

}
