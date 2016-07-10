using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace medic.Components {
    
    //Класс поля ввода
    public abstract partial class FieldWrapper : Panel {

        private bool hasError;

        public Label LblLabel;          //Подпись
        public Control CtrlField;       //Поле ввода
        public Label LblError;          //Ошибка



        //Конструктор
        public FieldWrapper(string labelText, Control field) {
            InitializeComponent();

            hasError = false;

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(0, 5, 0, 5);

            //Создаем надпись для вывода ошибок
            LblError = new Label();
            LblError.Dock = DockStyle.Top;
            LblError.AutoSize = true;
            LblError.ForeColor = Color.OrangeRed;
            LblError.Padding = new Padding(0, 2, 0, 0);
            LblError.Visible = false;
            LblError.Parent = this;

            //Помещаем компонент ввода на панель
            CtrlField = field;
            CtrlField.Parent = this;
            CtrlField.Dock = DockStyle.Top;

            //Создаем подпись
            LblLabel = new Label();
            LblLabel.Text = labelText;
            LblLabel.Dock = DockStyle.Top;
            LblLabel.AutoSize = true;
            LblLabel.Padding = new Padding(0, 0, 0, 5);
            LblLabel.Parent = this;
        }



        //Возвращает true, если есть ли ошибка в поле
        public bool HasError() {
            return hasError;
        }

        //Показывает ошибку
        public void ShowError(string errorText) {
            LblError.Text = errorText;
            LblError.Visible = true;
            hasError = true;
        }

        //Скрывает ошибку
        public void HideError() {
            LblError.Text = "";
            LblError.Visible = false;
            hasError = false;
        }



        //Возвращает значение поля ввода
        public abstract string GetValue();

        //Установливает значение для поля ввода
        public abstract void SetValue(string value);

    }

}
