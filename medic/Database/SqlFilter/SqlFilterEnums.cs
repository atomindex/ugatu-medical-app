using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic.Database {

    //Перечисление логических операторов
    public enum SqlLogicalOperator { And, Or, AndNot, OrNot };

    //Перечисление операторов сравнения
    public enum SqlComparisonOperator { Equal, Less, Larger, LessEqual, LargerEqual, Like };

}
