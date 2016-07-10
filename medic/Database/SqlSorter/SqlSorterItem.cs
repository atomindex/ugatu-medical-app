using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {

    //Класс сортировки по единичному значению 
    public class SqlSorterItem {

        private string tableName;           //Имя таблицы поля
        private string fieldName;           //Имя поля
        private string fullFieldName;       //Полное имя поле (с таблицей)
        private SqlOrder order;             //Порядок сортировки
        private string stringOrder;         //Строковое представление порядка сортировки

        

        //Конструктор
        public SqlSorterItem() {
            SetField("");
            SetOrder(SqlOrder.None);
        }

        //Конструктор
        public SqlSorterItem(string tableName, string fieldName, SqlOrder order) {
            SetField(tableName, fieldName);
            SetOrder(order);
        }



        //Установливает поле для сортировки
        public void SetField(string tableName, string fieldName) {
            this.tableName = tableName;
            this.fieldName = fieldName;
            this.fullFieldName = fieldName.Length > 0 ? tableName + "." + fieldName : "";
        }

        //Установливает поле для сортировки
        public void SetField(string fieldName) {
            this.tableName = "";
            this.fieldName = fieldName;
            this.fullFieldName = fieldName;
        }

        //Устанавливает порядок сортировки
        public void SetOrder(SqlOrder order) {
            this.order = order;
            this.stringOrder = " " + QueryBuilder.ConvertOrder(order);
        }



        //Возвращает строковое представление сортировки
        public override string ToString() {
            if (order != SqlOrder.None && fullFieldName.Length > 0)
                return fullFieldName + stringOrder;
            else
                return "";
        }

    }
}
