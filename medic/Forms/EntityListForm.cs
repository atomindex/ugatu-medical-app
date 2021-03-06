﻿using System;
using System.Drawing;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма списка сущностей
    public partial class EntityListForm : Form {

        protected DBConnection connection;    //Подключение к базе

        private int sortedColumnIndex;        //Индекс колонки с сортировкой

        protected TablePager tblPager;        //Пейджер таблицы


        public DialogResult ShowDialog() {
            if (reloadData())
                return base.ShowDialog();
            else
                return DialogResult.Abort;
        }


        //Очистка таблицы
        protected static void ClearTable(DataGridView table) {
            for (int i = 0; i < table.RowCount; i++) {
                table.Rows[i].DefaultCellStyle.BackColor = Color.White;
                for (int j = 0; j < table.ColumnCount; j++)
                    table.Rows[i].Cells[j].Value = "";
            }
        }

        //Очистка цвета таблицы
        protected static void ClearTableColor(DataGridView table) {
            for (int i = 0; i < table.RowCount; i++)
                table.Rows[i].DefaultCellStyle.BackColor = Color.White;
        }



        //Конструктор
        public EntityListForm(DBConnection connection) {
            InitializeComponent();

            this.connection = connection;
            sortedColumnIndex = -1;

            //Создаем пейджер
            tblPager = new TablePager();
            tblPager.Parent = this;
            tblPager.Dock = DockStyle.Bottom;

            AddPageChangeEvent(tblPagerPageChange_Event);
        }



        //Перезагружкает данные в таблицу
        protected virtual bool reloadData(bool resetPageIndex = false) { return true; }



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

        //Добавление события на клик по кнопке поиск
        public void AddSearchEvent(EventHandler handler) {
            btnSearch.Click += handler;
        } 

        //Добавление события на изменение страницы
        public void AddPageChangeEvent(EventHandler handler) {
            tblPager.AddChangeEvent(handler);
        }

        //Добавление события изменения колонки и типа сортировки
        public void AddSortChangeEvent(DataGridViewCellMouseEventHandler handler) {
            table.ColumnHeaderMouseClick += handler;
        }



        //Событие измения страницы таблицы
        private void tblPagerPageChange_Event(object sender, EventArgs e) {
            reloadData();
        }

        //Событие клика по ячейке в шапке таблицы
        private void table_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (sortedColumnIndex != e.ColumnIndex && sortedColumnIndex >= 0)
                table.Columns[sortedColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.None;
            sortedColumnIndex = e.ColumnIndex;

            DataGridViewColumnHeaderCell headerCell = table.Columns[e.ColumnIndex].HeaderCell;
            switch (headerCell.SortGlyphDirection) {
                case SortOrder.None: 
                    headerCell.SortGlyphDirection = SortOrder.Ascending;
                    break;
                case SortOrder.Ascending:
                    headerCell.SortGlyphDirection = SortOrder.Descending;
                    break;
                case SortOrder.Descending:
                    sortedColumnIndex = -1;
                    headerCell.SortGlyphDirection = SortOrder.None;
                    break;
            }
          
        }

        //Событие добавления колонки
        private void table_ColumnAdded(object sender, DataGridViewColumnEventArgs e) {
            e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
        }
    
    }

}
