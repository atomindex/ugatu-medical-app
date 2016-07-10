using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic {
    public class ListData<T> {

        public List<T> list;        //Список данных
        public int count;           //Общее количество данных
        public int limit;           //Ограничение по количеству
        public int offset;          //Количество пропущенных записей
        public int pageIndex;       //Текущая страница
        public int pages;           //Количество страниц

        //Конструктор
        public ListData() { }

        //Конструктор
        public ListData(List<T> list, int count, int limit, int pageIndex) {
            this.list = list;
            this.count = count;
            this.limit = limit;
            this.offset = limit > 0 ? limit * pageIndex : 0;
            this.pageIndex = pageIndex;
            this.pages = limit > 0 ? (int)Math.Ceiling(count / (double)limit) : 0;
        }

    }

}
