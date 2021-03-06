﻿using medic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic {
    public class Visit : Entity {

        private static int patientFieldsCount;
        private static string tableName;        //Имя таблицы
        private static string fields;           //Поля таблицы для выборки
        private static string[] fieldsArray;    //Массив полей для вставки

        public Patient RelatedPatient;
        public DateTime VisitDate;
        public int PatientSex;
        public int PatientAge;
        public double Price;


        //Статический конструктор
        static Visit() {
            tableName = "visits";
            fields = "visits.id, DATE_FORMAT(visits.visit_date,'%Y-%m-%d'), visits.patient_sex, visits.patient_age, visits.price, " + Patient.GetFields();
            fieldsArray = new string[] { "patient_id", "visit_date", "patient_sex", "patient_age", "price" };

            patientFieldsCount = Patient.GetFields().Split(new string[] { ", " }, StringSplitOptions.None).Length;
        }


        //Возвращает имя таблицы
        public static string GetTableName() {
            return tableName;
        }

        //Возвращает имя поля с таблицей
        public static string GetFieldName(string field) {
            return tableName + "." + field;
        }

        //Возвращает данные списка посещений
        public static ListData GetListData(DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {
            string baseSql = "SELECT " + Visit.fields + " FROM " + Visit.GetTableName() +
                            " JOIN patients ON patients.id = visits.patient_id";
            string countSql = "SELECT count(*) as row_count FROM " + Visit.GetTableName() +
                             " JOIN patients ON patients.id = visits.patient_id"; ;

            //Создаем фильтр по статусу Удален
            SqlFilter filterGroup = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition removedFilter = new SqlFilterCondition(Visit.GetFieldName("removed"), SqlComparisonOperator.Equal);
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

        //Возвращат данные списка посещений, указанного пациента
        public static ListData GetPatientListData(int patientId, DBConnection connection, int limit = 0, int pageIndex = 0, SqlFilter filter = null, SqlSorter sorter = null) {

            SqlFilter patientFilter = new SqlFilter(SqlLogicalOperator.And);
            SqlFilterCondition patientFilterCondition = new SqlFilterCondition(Visit.GetFieldName("patient_id"), SqlComparisonOperator.Equal);
            patientFilterCondition.SetValue(patientId);
            patientFilter.AddItem(patientFilterCondition);
            patientFilter.AddItem(filter);

            return GetListData(connection, limit, pageIndex, patientFilter, sorter);
        }

        //Возвращает список посещений из данных списка
        public static List<Visit> GetList(ListData listData) {
            //Формируем список сотрудников
            List<Visit> list = new List<Visit>();
            if (listData.List != null) {
                foreach (string[] data in listData.List) {
                    Visit visit = new Visit(listData.Connection);
                    visit.loadData(data);
                    list.Add(visit);
                }
            }

            return list;
        }


        public Visit(DBConnection connection, int id = 0)
            : base(connection) {
            if (id <= 0) return;
            List<string[]> data = connection.Select("SELECT " + fields + " FROM " + tableName + 
                                                   " JOIN patients ON patients.id = visits.patient_id" +
                                                   " WHERE id = " + id.ToString());
            if (data.Count == 0) {
                this.id = -1;
                return;
            }
            loadData(data[0]);
        }

        //Возвращает копию
        public Visit Clone() {
            Visit visit = new Visit(connection);
            visit.id = id;
            visit.VisitDate = VisitDate;
            visit.PatientAge = PatientAge;
            visit.PatientSex = PatientSex;
            visit.RelatedPatient = RelatedPatient.Clone();
            return visit;
        }


        public override int Save() {
            id = save(
                tableName, fieldsArray,
                new string[] { 
                    RelatedPatient.GetId().ToString(), 
                    VisitDate.ToString(AppConfig.DatabaseDateFormat), 
                    PatientSex.ToString(), 
                    PatientAge.ToString(),
                    Price.ToString()
                }
            );
            return id;
        }

        //Удаляет посещение
        public override void Remove() {
            remove(tableName);
        }


        //Загружает данные в поля
        private void loadData(string[] data) {
            string[] patientData = new string[patientFieldsCount];
            Array.Copy(data, 5, patientData, 0, patientFieldsCount);

            id = Int32.Parse(data[0]);
            VisitDate = DateTime.ParseExact(data[1], AppConfig.DatabaseDateFormat, null);
            PatientSex = Int32.Parse(data[2]);
            PatientAge = Int32.Parse(data[3]);
            Price = Double.Parse(data[4]);

            RelatedPatient = new Patient(connection);
            RelatedPatient.LoadData(patientData);
        }
    }
}
