using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {

    //Элемент фильтра, представляющий единичное условие
    public class SqlFilterCondition : SqlFilterItem {

        private SqlComparisonOperator op;       //Оператор сравнения
        private string stringOp;                //Строковое представление оператора сравнения
        private string tableName;               //Имя таблицы поля
        private string fieldName;               //Имя поля
        private string fullFieldName;           //Полное имя поля (с таблицей)
        private object value;                   //Значение c которым сравнивается поле

        

        //Конструктор
        public SqlFilterCondition(string tableName, string fieldName, SqlComparisonOperator op) {
            this.tableName = tableName;
            this.fieldName = fieldName;
            this.op = op;

            this.fullFieldName = tableName + "." + fieldName;
            this.stringOp = " " + QueryBuilder.ConvertComparisonOperator(op) + " ";
        }

        //Конструктор
        public SqlFilterCondition(string fieldName, SqlComparisonOperator op) {
            this.tableName = "";
            this.fieldName = fieldName;
            this.op = op;

            this.fullFieldName = fieldName;
            this.stringOp = " " + QueryBuilder.ConvertComparisonOperator(op) + " ";
        }


        //Устанавливает значение с которым будет сравниваться поле
        public void SetValue(object value) {
            this.value = value == null ? null : value.ToString();
        }



        //Возвращает строковое представление условия
        public override string ToString() {
            return value != null ? fullFieldName + stringOp + value : "";
        }

    }

}
