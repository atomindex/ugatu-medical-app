using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace medic.Components {

    public delegate void ValueComboBoxWrapperRemoveClick(int index);
    public delegate void ValueComboBoxWrapperChange(int index, string key);

    public class ValueComboBoxWrapperItem {

        public Label label;
        private string key;
        private string value;

        public ComboBox comboBox;
        private string[] keys;

        public Button btnRemove;

        public ValueComboBoxWrapperItem() {
            label = new Label();
            label.Margin = new Padding(0, 0, 5, 10);
            label.Dock = DockStyle.Left;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.MinimumSize = new Size(100, 0);
            label.MaximumSize = new Size(200, 0);
            label.AutoSize = true;
            updateLabelHeight();

            comboBox = new ComboBox();
            comboBox.Margin = new Padding(0, 0, 0, 10);
            comboBox.Dock = DockStyle.Top;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            btnRemove = new Button();
            btnRemove.Margin = new Padding(5, 0, 0, 10);
            btnRemove.Dock = DockStyle.Top;
            btnRemove.Width = 21;
            btnRemove.Height = 21;
            btnRemove.BackgroundImage = Properties.Resources.Remove;
            btnRemove.BackgroundImageLayout = ImageLayout.Center;
            btnRemove.ImageAlign = ContentAlignment.MiddleCenter;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }


        public void SetValue(string key, string value) {
            this.key = key;
            this.value = value;
            label.Text = value;
        }

        public string GetComboBoxKey() {
            return comboBox.SelectedIndex > -1 ? keys[comboBox.SelectedIndex] : "";
        }

        public void SetComboBoxKey(string key) {
            for (int i = 0; i < keys.Length; i++)
                if (keys[i] == key) {
                    comboBox.SelectedIndex = i;
                    break;
                }
        }

        public void SetComboBoxValues(string[] keys, string[] values) {
            this.keys = keys;
            comboBox.Items.Clear();
            comboBox.Items.AddRange(values);
        }

        public void Remove() {
            label.Parent = null;
            comboBox.Parent = null;
            btnRemove.Parent = null;
        }

        private void updateLabelHeight() {
            Graphics g = label.CreateGraphics();
            SizeF size = g.MeasureString(label.Text, label.Font, label.MaximumSize.Width);
            label.MinimumSize = new Size(label.MinimumSize.Width, (int)size.Height);
        }
    }

    //Класс текстового поля ввода с надписями
    public class ValueComboBoxWrapper : FieldWrapper {

        private bool editable;
        private List<ValueComboBoxWrapperItem> items;

        private ToolStrip tlsListManager;        //Панель с кнопками
        private ToolStripLabel tlsLblLabel;      //Подпись к полю
        private ToolStripSeparator tlsSeparator; //Разделитель
        private ToolStripButton tlsBtnAdd;       //Кнопка добавления

        private TableLayoutPanel tlpPanel;
        private Label lblEmpty;

        private ValueComboBoxWrapperRemoveClick removeEvent;
        private ValueComboBoxWrapperChange changeEvent;

        //Конструктор
        public ValueComboBoxWrapper(string labelText, string emptyText, int maxHeight = 300) : base(null, null) {
            editable = true;
            items = new List<ValueComboBoxWrapperItem>();

            Panel scrollPanel = new Panel();
            scrollPanel.Dock = DockStyle.Top;
            scrollPanel.AutoSize = true;
            scrollPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            scrollPanel.MaximumSize = new Size(0, maxHeight);
            scrollPanel.AutoScroll = true;
            scrollPanel.BackColor = Color.White;
            scrollPanel.Padding = new Padding(10,10,10,0);
            scrollPanel.BorderStyle = BorderStyle.FixedSingle;
            scrollPanel.Parent = this;

            tlpPanel = new TableLayoutPanel();
            tlpPanel.Dock = DockStyle.Top;
            tlpPanel.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            tlpPanel.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpPanel.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            tlpPanel.AutoSize = true;
            tlpPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPanel.Visible = false;
            tlpPanel.Parent = scrollPanel;

            lblEmpty = new Label();
            lblEmpty.AutoSize = true;
            lblEmpty.Dock = DockStyle.Top;
            lblEmpty.Text = emptyText;
            lblEmpty.FlatStyle = FlatStyle.System;
            lblEmpty.Margin = Padding.Empty;
            lblEmpty.Padding = new Padding(0, 0, 0, 10);
            lblEmpty.Font = new Font(lblEmpty.Font.Name, lblEmpty.Font.Size, FontStyle.Italic);
            lblEmpty.Parent = scrollPanel;

            tlsListManager = new ToolStrip();
            tlsListManager.GripStyle = ToolStripGripStyle.Hidden;
            tlsListManager.BackColor = Color.Transparent;
            tlsListManager.Parent = this;

            tlsLblLabel = new ToolStripLabel();
            tlsLblLabel.Text = labelText;
            tlsListManager.Items.Add(tlsLblLabel);

            tlsSeparator = new ToolStripSeparator();
            tlsListManager.Items.Add(tlsSeparator);

            tlsBtnAdd = new ToolStripButton();
            tlsBtnAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tlsBtnAdd.Text = "Добавить";
            tlsBtnAdd.Image = medic.Properties.Resources.Add;
            tlsListManager.Items.Add(tlsBtnAdd);
        }

        public ValueComboBoxWrapperItem GetItem(int index) {
            return items[index];
        }

        public ValueComboBoxWrapperItem AddItem(string key, string value, string[] keys, string[] values) {
            int lastRowIndex = tlpPanel.RowCount;

            lblEmpty.Visible = false;
            tlpPanel.Visible = true;

            ValueComboBoxWrapperItem item = new ValueComboBoxWrapperItem();
            item.SetValue(key, value);
            item.SetComboBoxValues(keys, values);

            item.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            item.comboBox.Tag = lastRowIndex;
            item.btnRemove.Click += btnRemove_Click;
            item.btnRemove.Tag = lastRowIndex;
            item.btnRemove.Visible = false;
            
            items.Add(item);

            tlpPanel.RowCount = lastRowIndex + 1;
            tlpPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tlpPanel.Controls.Add(item.label, 0, lastRowIndex);
            tlpPanel.Controls.Add(item.comboBox, 1, lastRowIndex);
            tlpPanel.Controls.Add(item.btnRemove, 2, lastRowIndex);

            return item;
        }

        public void RemoveItemAt(int index) {
            SuspendLayout();

            items[index].Remove();

            tlpPanel.RowStyles.RemoveAt(index);

            for (int i = index + 1; i < tlpPanel.RowCount; i++) {
                for (int j = 0; j < tlpPanel.ColumnCount; j++) {
                    var control = tlpPanel.GetControlFromPosition(j, i);
                    if (control != null) {
                        tlpPanel.SetRow(control, i - 1);
                    }
                }
                items[i].btnRemove.Tag = i - 1;
                items[i].comboBox.Tag = i - 1;
            }
            tlpPanel.RowCount = tlpPanel.RowCount - 1;

            ResumeLayout();

            items.RemoveAt(index);

            if (items.Count == 0) {
                lblEmpty.Visible = true;
                tlpPanel.Visible = false;
            } else {
                lblEmpty.Visible = false;
                tlpPanel.Visible = true;
            }

            if (removeEvent != null)
                removeEvent.Invoke(index);
        }

        //Возвращает значение поля ввода
        public override string GetValue() {
            return "";
        }

        //Устанавливает значение поля ввода
        public override void SetValue(string value) {
    
        }


        public void SetEditable(bool enabled) {
            this.editable = enabled;
            tlsBtnAdd.Visible = tlsSeparator.Visible = enabled;
            foreach (ValueComboBoxWrapperItem item in items)
                item.btnRemove.Visible = enabled;
        }


        public void AddAddEvent(EventHandler handler) {
            tlsBtnAdd.Click += handler;
        }

        public void AddRemoveEvent(ValueComboBoxWrapperRemoveClick handler) {
            removeEvent += handler;
        }

        public void AddChangeEvent(ValueComboBoxWrapperChange handler) {
            changeEvent += handler;
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            RemoveItemAt((int)(sender as Button).Tag);
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (changeEvent == null)
                return;

            ComboBox comboBox = sender as ComboBox;
            int itemIndex = (int)comboBox.Tag;
            changeEvent.Invoke(itemIndex, items[itemIndex].GetComboBoxKey());
        }

    }

}
