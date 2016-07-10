using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace medic.Components {

    //Класс пейджера для таблицы
    public class TablePager : Panel {

        public int Сount;               //Общее количество записей
        public int PagesCount;          //Количество страниц
        public int PageIndex;           //Индекс текущей страницы

        private Label lblFinded;        //Надпись для отображения количества записей
        private Panel pnlPager;         //Панель пейджера
        private Label lblPagerPage;     //Надпись для отображения номера текущей страницы
        private Button btnPrevious;     //Кнопка перехода на предыдущую страницу
        private Button btnNext;         //Кнопку перехода на следующую страницу



        //Конструктор
        public TablePager() {
            Height = 35;
            Padding = new Padding(10, 5, 10, 5);

            //Инициализируем надпись общего количества найденых записей
            lblFinded = new Label();
            lblFinded.AutoSize = true;
            lblFinded.MinimumSize = new Size(0, 25);
            lblFinded.TextAlign = ContentAlignment.MiddleCenter;
            lblFinded.Dock = DockStyle.Right;
            lblFinded.Parent = this;
            
            //Иницализируем панель пейджера
            pnlPager = new Panel();
            pnlPager.AutoSize = true;
            pnlPager.Padding = new Padding(10, 0, 0, 0);
            pnlPager.Dock = DockStyle.Right;
            pnlPager.Parent = this;

            //Инициализируем надпись номера текущей страницы
            lblPagerPage = new Label();
            lblPagerPage.AutoSize = true;
            lblPagerPage.MinimumSize = new Size(0, 25);
            lblPagerPage.Text = PageIndex.ToString();
            lblPagerPage.TextAlign = ContentAlignment.MiddleCenter;
            lblPagerPage.Dock = DockStyle.Fill;
            lblPagerPage.Parent = pnlPager;

            //Инициализируем кнопку Предыдущая страница
            btnPrevious = new Button();
            btnPrevious.Text = "<";
            btnPrevious.Dock = DockStyle.Left;
            btnPrevious.Width = 18;
            btnPrevious.Parent = pnlPager;
            btnPrevious.Click += btnPrevious_Click;

            //Инициализируем кнопку Следующая страница
            btnNext = new Button();
            btnNext.Text = ">";
            btnNext.Dock = DockStyle.Right;
            btnNext.Width = 18;
            btnNext.Parent = pnlPager;
            btnNext.Click += btnNext_Click;
        }



        //Устанавливает данные пейджера
        public void SetData(int count, int pages, int page) {
            Сount = count;
            PagesCount = pages;

            lblFinded.Text = "Найдено: " + count.ToString();
            pnlPager.Visible = (pages > 1);

            SetPage(page);
        }

        //Возвращает индекс текущей страницы
        public int GetPage() {
            return PageIndex;
        }

        //Устанавливает текущую страницу
        public void SetPage(int pageIndex) {
            if (pageIndex > PagesCount - 1) 
                pageIndex = PagesCount - 1;

            if (pageIndex < 0)
                pageIndex = 0;

            PageIndex = pageIndex;

            btnPrevious.Enabled = pageIndex > 0;
            btnNext.Enabled = pageIndex < PagesCount - 1;

            lblPagerPage.Text = (pageIndex + 1).ToString() + " из " + this.PagesCount.ToString();
        }



        //Вешает событие на изменение индекса страницы с помощью кнопок
        public void AddChangeEvent(EventHandler handler) {
            btnPrevious.Click += handler;
            btnNext.Click += handler;
        }

        //Событие клика по кнопке Предыдущая страница
        private void btnPrevious_Click(object sender, EventArgs e) {
            SetPage(PageIndex - 1);
        }

        //Событие клика по кнопке Следующая страница
        private void btnNext_Click(object sender, EventArgs e) {
            SetPage(PageIndex + 1);
        }

    }

}
