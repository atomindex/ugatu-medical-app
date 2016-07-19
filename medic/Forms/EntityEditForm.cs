using System;
using System.Windows.Forms;

namespace medic.Forms {

    //Базовая форма редактирования сущности
    public partial class EntityEditForm : Form {

        //Конструктор
        public EntityEditForm() {
            InitializeComponent();
        }



        //Проверяет поля на корректность введенных данных
        protected virtual bool Validate() {
            return true;
        }

        //Записывает данные формы в объект сущности и сохраняет
        protected virtual bool Save() {
            return true;
        }



        //Возвращает панель для компонентов
        public Panel GetPanel() {
            return panel;
        }



        //Событие клика на кнопку Сохранить
        private void saveButton_Click(object sender, EventArgs e) {
            if (Validate()) {
                if (Save()) {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                //TODO Добавить сообщение об ошибке
            }
        }

        //Событие клика на кнопку Отмена
        private void cancelButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }

}
