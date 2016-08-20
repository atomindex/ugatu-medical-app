using System;
using System.Collections.Generic;
using System.Windows.Forms;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования категории
    public partial class CategoryEditForm : EntityEditForm {

        private DBConnection connection;                //Соединение с базой

        private Category category;                      //Редактируемая категория

        private TextBoxWrapper tbwName;                 //Поле Название категории


        //Конструктор
        public CategoryEditForm(Category category) : base() {
            InitializeComponent();

            connection = category.GetConnection();
            this.category = category;

            Panel panel = GetPanel();

            tbwName = new TextBoxWrapper("Название", new TextBox());
            tbwName.Dock = DockStyle.Top;
            tbwName.Parent = panel;

            panel.TabIndex = 0;
            toolsPanel.TabIndex = 1;
            FormUtils.UpdateTabIndex(panel, FormUtils.UpdateTabIndex(toolsPanel, 2));

            //Подгружаем данные категории
            AssignCategory(category);
        }



        //Привязывает категорию к форме, подгружает данные в форму
        public void AssignCategory(Category category) {
            tbwName.SetValue(category.Name);
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            FieldWrapper[] requiredTextBoxes = new FieldWrapper[] {
                tbwName
            };

            foreach (FieldWrapper field in requiredTextBoxes)
                if (field.GetValue().Trim().Length > 0)
                    field.HideError();
                else {
                    field.ShowError("Поле обязательно для заполнения");
                    success = false;
                }

            return success;
        }

        //Сохраняет категорию
        protected override bool Save() {
            category.Name = tbwName.GetValue();
            return category.Save() != -1;
        }

    }

}
