using System;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Базовая форма выбора сущностей
    public partial class EntitySelectForm : Form {

        protected DBConnection connection;              //Соединение с базой

        protected TablePager tblPager;                  //Пейджер списка



        //Конструктор
        public EntitySelectForm(DBConnection connection) {
            InitializeComponent();

            this.connection = connection;

            tblPager = new TablePager();
            tblPager.Dock = DockStyle.Bottom;
            tblPager.Parent = listPanel;

            AddPageChangeEvent(tblPagerPageChange_Event);
        }



        public DialogResult ShowDialog() {
            if (!reloadData())
                return DialogResult.Abort;

            return base.ShowDialog();
        }



        //Перезагружкат данные в список
        protected virtual bool reloadData(bool resetPageIndex = false) { return true; }



        //Добавление события на клик по кнопке поиск
        public void AddSearchEvent(EventHandler handler) {
            btnSearch.Click += handler;
        } 

        //Добавление события на изменение страницы
        public void AddPageChangeEvent(EventHandler handler) {
            tblPager.AddChangeEvent(handler);
        }

        //Добавление события изменения состояния флажков
        public void AddCheckEvent(ItemCheckEventHandler handler) {
            list.ItemCheck += handler;
        }



        //Событие измения страницы списка
        private void tblPagerPageChange_Event(object sender, EventArgs e) {
            reloadData();
        }

        //Событие клика по кнопке Добавить
        private void addButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        //Событие клика по кнопке Отмена
        private void cancelButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    
    }

}
