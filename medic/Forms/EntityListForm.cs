using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using medic.Components;
using medic.Database;

namespace medic.Forms {

    //Форма списка сущностей
    public partial class EntityListForm : Form {

        protected DBConnection connection;    //Подключение к базе

        protected TablePager tblPager;           //Пейджер таблицы



        //Очистка таблицы
        protected static void ClearTable(DataGridView table) {
            for (int i = 0; i < table.RowCount; i++) {
                table.Rows[i].DefaultCellStyle.BackColor = Color.White;
                for (int j = 0; j < table.ColumnCount; j++)
                    table.Rows[i].Cells[j].Value = "";
           
            
            }
        }

        protected static void ClearTableColor(DataGridView table) {
            for (int i = 0; i < table.RowCount; i++)
                table.Rows[i].DefaultCellStyle.BackColor = Color.White;
        }



        //Конструктор
        public EntityListForm(DBConnection connection) {
            InitializeComponent();

            this.connection = connection;

            //Создаем пейджер
            tblPager = new TablePager();
            tblPager.Parent = this;
            tblPager.Dock = DockStyle.Bottom;
        }



        //Отключение сортировки по полям
        protected void DisableSort() {
            for (int i = 0; i < table.ColumnCount; i++)
                table.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
        }



        //Добавление события на клик по кнопке добавления
        public void AddAddEvent(EventHandler handler) {
            btnAdd.Click += handler;        
        }

        //Добавление события на клик по кнопке редактирования
        public void AddEditEvent(EventHandler handler) {
            btnEdit.Click += handler;
        }

        //Добавление события на клик по кнопке удаления
        public void AddRemoveEvent(EventHandler handler) {
            btnRemove.Click += handler;
        }

        public void AddSearchEvent(EventHandler handler) {
            btnSearch.Click += handler;
        } 

        //Добавление события на изменение страницы
        public void AddPageChangeEvent(EventHandler handler) {
            tblPager.AddChangeEvent(handler);
        }
    
    }

}
