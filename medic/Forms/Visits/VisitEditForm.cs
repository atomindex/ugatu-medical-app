using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using medic.Database;
using medic.Components;

namespace medic.Forms {

    //Форма редактирования сотрудника
    public partial class VisitEditForm : EntityEditForm {

        private DBConnection connection;                    //Соединение с базой
        private List<Sale> sales;

        private Visit visit;                                //Редактируемое посещение
        private List<VisitService> visitServices;
        private List<VisitService> removedVisitServices;
        private List<List<Worker>> servicesWorkers;

        private List<VisitSale> visitSales;

        private VisitInfoPanel sidePanel;
        private DatepickerWrapper dpwVisitDate;             //Поле Дата посещения
        private ValueComboBoxWrapper vcbwServices;


        //Конструктор
        public VisitEditForm(Visit visit, Patient patient = null) : base() {
            InitializeComponent();

            MinimumSize = new Size(900, 0);

            TableLayoutPanel panelWrapper = new TableLayoutPanel();
            panelWrapper.Dock = DockStyle.Top;
            panelWrapper.AutoSize = true;
            panelWrapper.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelWrapper.ColumnCount = 2;
            panelWrapper.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panelWrapper.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panelWrapper.Parent = this;
            Controls.SetChildIndex(panelWrapper, Controls.GetChildIndex(toolsPanel));

            sidePanel = new VisitInfoPanel();
            sidePanel.Margin = new Padding(0, 15, 15, 5);
            sidePanel.MinimumSize = new Size(0, 300);
            sidePanel.MaximumSize = new Size(sidePanel.MaximumSize.Width, 400);
            sidePanel.AutoScroll = true;
            sidePanel.AutoSize = true;
            sidePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelWrapper.Controls.Add(sidePanel, 1, 0);


            Panel panel = GetPanel();
            panel.Dock = DockStyle.Top;
            panel.MinimumSize = new Size(450, 30);
            panelWrapper.Controls.Add(panel, 0, 0);

            vcbwServices = new ValueComboBoxWrapper("Услуги", "Добавьте услуги", 176);
            vcbwServices.Dock = DockStyle.Top;
            vcbwServices.Parent = panel;
            vcbwServices.AddAddEvent(btnAddService_Event);
            vcbwServices.AddRemoveEvent(btnRemoveService_Event);
            vcbwServices.AddChangeEvent(comboBoxServiceWorkerChange_Event);

            DateTimePicker dtpVisitDate = new DateTimePicker();
            dpwVisitDate = new DatepickerWrapper("Дата посещения", dtpVisitDate);
            dpwVisitDate.Dock = DockStyle.Top;
            dpwVisitDate.Parent = panel;

            panel.TabIndex = 0;
            toolsPanel.TabIndex = 1;
            FormUtils.UpdateTabIndex(panel, FormUtils.UpdateTabIndex(toolsPanel, 2));

            //Подгружаем данные посещения
            if (patient == null) {
                AssignVisit(visit);
                dpwVisitDate.Visible = false;
            } else {
                AssignVisit(visit, patient);
                dtpVisitDate.ValueChanged += dtpVisitDate_ValueChanged;
            }
         
            calcPrices();
        }


        //Привязывает сотрудника к форме, подгружает данные в форму
        private void AssignVisit(Visit visit, Patient patient) {
            this.connection = visit.GetConnection();
            this.visit = visit;

            visit.VisitDate = dpwVisitDate.GetDate();
            visit.RelatedPatient = patient;
            visit.PatientSex = patient.Sex;
            visit.PatientAge = patient.GetAge();

            ListData salesListData = Sale.GetListData(connection);
            salesListData.Update();
            sales = Sale.GetList(salesListData);

            servicesWorkers = new List<List<Worker>>();
            visitServices = new List<VisitService>();
            visitSales = new List<VisitSale>();
            removedVisitServices = new List<VisitService>();

            sidePanel.SetVisitDate(dpwVisitDate.GetDate().ToString(AppConfig.DateFormat));
            sidePanel.SetPatientName(patient.GetFullName());
            sidePanel.SetPatientSex(patient.GetStringSex());
            sidePanel.SetPatientAge(patient.GetAge());

            updateSales();
        }



        private void AssignVisit(Visit visit) {
            this.connection = visit.GetConnection();
            this.visit = visit;

            ListData salesListData = Sale.GetListData(connection);
            salesListData.Update();
            sales = Sale.GetList(salesListData);

            servicesWorkers = new List<List<Worker>>();

            //Получаем все услуги посещения
            ListData visitServicesListData = VisitService.GetListData(visit.GetId(), visit.GetConnection());
            visitServicesListData.Update();
            visitServices = VisitService.GetList(visitServicesListData);
            removedVisitServices = new List<VisitService>();

            //Получаем все скидки посещения
            ListData visitSalesListData = VisitSale.GetListData(visit.GetId(), visit.GetConnection());
            visitSalesListData.Update();
            visitSales = VisitSale.GetList(visitSalesListData);

            //Устанавливаем данные посещения
            dpwVisitDate.SetDate(visit.VisitDate);

            //Подгружаем данные в панель
            sidePanel.SetVisitDate(visit.VisitDate.ToString(AppConfig.DateFormat));
            sidePanel.SetPatientName(visit.RelatedPatient.GetFullName());
            sidePanel.SetPatientSex(Patient.GetSex(visit.PatientSex));
            sidePanel.SetPatientAge(visit.PatientAge);


            //Загружаем услуги в форму
            vcbwServices.SetEditable(false);
            vcbwServices.SuspendLayout();
            for (int i = 0; i < visitServices.Count; i++) {
                VisitService visitService = visitServices[i];

                //Получаем всех сотрудников, предоставляющих данную услугу
                ListData serviceWorkersListData = Worker.GetServiceWorkersListData(visitService.RelatedService.GetId(), connection, 10000);
                serviceWorkersListData.Update();
                List<Worker> serviceWorkers = Worker.GetList(serviceWorkersListData);
                servicesWorkers.Add(serviceWorkers);

                //Получаем ключ - значение для сотрудников
                string[] workersKeys = new string[serviceWorkers.Count];
                string[] workersNames = new string[serviceWorkers.Count];
                int selectedIndex = -1;
                for (int j = 0; j < serviceWorkers.Count; j++) {
                    workersKeys[j] = j.ToString();
                    workersNames[j] = serviceWorkers[j].GetFullName();
                    if (serviceWorkers[j].GetId() == visitService.RelatedWorker.GetId())
                        selectedIndex = j;
                }

                //Добавляем элемент на форму
                ValueComboBoxWrapperItem item = vcbwServices.AddItem(
                    visitService.RelatedService.GetId().ToString(),
                    visitService.RelatedService.Name,
                    workersKeys,
                    workersNames
                );
                sidePanel.AddService(visitService.RelatedService.Name, visitService.RelatedService.Price);
                
                item.SetComboBoxKey(selectedIndex.ToString(), -1);
            }
            vcbwServices.ResumeLayout();

            foreach (VisitSale visitSale in visitSales)
                sidePanel.AddSale(visitSale.RelatedSale.Name, visitSale.Percent.ToString());
        }



        private void updateSales() {
            VisitData visitData = new VisitData();
            visitData.PatientAge = visit.RelatedPatient.GetAge();
            visitData.PatientSex = visit.RelatedPatient.Sex;
            visitData.PatientCategories = visit.RelatedPatient.GetCategoriesIds();
            visitData.VisitNumber = visit.RelatedPatient.GetVisitCount() + 1;
            visitData.ServicesCount = visitServices.Count;
            visitData.VisitDate = dpwVisitDate.GetDate();

            visitData.ServicesSumPrice = 0;
            foreach (VisitService service in visitServices)
                visitData.ServicesSumPrice += service.Price;

            List<Sale> suitableSales = Sale.GetSuitableSales(sales, visitData);
            visitSales.Clear();
            sidePanel.ClearSales();
            foreach (Sale sale in suitableSales) {
                VisitSale visitSale = new VisitSale(connection);
                visitSale.Percent = sale.Percent;
                visitSale.VisitId = visit.GetId();
                visitSale.RelatedSale = sale;
                visitSales.Add(visitSale);

                sidePanel.AddSale(sale.Name, sale.Percent.ToString());
            }
        }

        private void calcPrices() {
            int servicesPrice = 0;
            foreach (VisitService visitService in visitServices)
                servicesPrice += visitService.Price;
            
            int salesPercent = 0;
            foreach (VisitSale visitSale in visitSales)
                salesPercent += visitSale.Percent;
            salesPercent = Math.Min(salesPercent, 100);

            double salePrice = servicesPrice / 100.0 * salesPercent;
            double totalPrice = servicesPrice - salePrice;

            visit.Price = totalPrice;

            sidePanel.SetServicesPrice(servicesPrice.ToString());
            sidePanel.SetSalesPercent(salesPercent.ToString(), salePrice.ToString());
            sidePanel.SetTotalPrice(totalPrice.ToString());
        }

        //Проверяет кооректность введенных данных
        protected override bool Validate() {
            bool success = true;

            FieldWrapper[] requiredTextBoxes = new FieldWrapper[] {
                dpwVisitDate
            };

            foreach (FieldWrapper field in requiredTextBoxes)
                if (field.GetValue().Trim().Length > 0)
                    field.HideError();
                else {
                    field.ShowError("Поле обязательно для заполнения");
                    success = false;
                }

            vcbwServices.HideError();
            if (visitServices.Count == 0) {
                vcbwServices.ShowError("Не выбранно ни одной услуги");
                success = false;
            } else {
                for (int i = 0; i < visitServices.Count; i++) {
                    string key = vcbwServices.GetItem(i).GetComboBoxKey();
                    if (key == "") {
                        vcbwServices.ShowError("Выберите исполнителя для каждой услуги");
                        success = false;
                        break;
                    }
                }
            }

            return success;
        }

        //Сохраняет сотрудника
        protected override bool Save() {
            for (int i = 0; i < visitServices.Count; i++) {
                int index = Int32.Parse(vcbwServices.GetItem(i).GetComboBoxKey());
                visitServices[i].RelatedWorker = servicesWorkers[i][index];
            }

            connection.StartTransaction();

            if (visit.Save() == -1)
                connection.RollbackTransaction();

            else {

                foreach (VisitService visitService in visitServices) {
                    visitService.VisitId = visit.GetId();

                    if (visitService.Save() == -1) {
                        connection.RollbackTransaction();
                        return false;
                    }
                }

                foreach (VisitSale visitSale in visitSales) {
                    visitSale.VisitId = visit.GetId();

                    if (visitSale.Save() == -1) {
                        connection.RollbackTransaction();
                        return false;
                    }
                }

                if (connection.CommitTransaction())
                    return true;
            }

            return false;
        }



       







        //Событие клика на кнопку Добавить услугу
        private void btnAddService_Event(object sender, EventArgs e) {
            //Создаем фильтр для отсечения уже добавленных услуг
            SqlFilter selectedFilter = new SqlFilter(SqlLogicalOperator.And);
            if (visitServices.Count > 0) {
                SqlFilterCondition selectedFilterCondition = new SqlFilterCondition(Service.GetFieldName("id"), SqlComparisonOperator.NotIn);

                string[] visitSpecialtiesIds = new string[visitServices.Count];
                for (int i = 0; i < visitServices.Count; i++)
                    visitSpecialtiesIds[i] = visitServices[i].RelatedService.GetId().ToString();
                selectedFilterCondition.SetValue(QueryBuilder.BuildInStatement(visitSpecialtiesIds));

                selectedFilter.AddItem(selectedFilterCondition);
            }

            //Создаем формы для добавления услуг
            ListData servicesListData = Service.GetListData(connection, 25, 0, selectedFilter);
            ServiceSelectForm serviceSelectForm = new ServiceSelectForm(servicesListData);

            if (serviceSelectForm.ShowDialog() == DialogResult.OK) {
                //Получаем выбранные услуги
                List<Service> selectedServices = serviceSelectForm.GetSelected();
                
                //Создаем, на основе услуг, список услуг посещения 
                List<VisitService> selectedVisitServices = new List<VisitService>();
                foreach (Service service in selectedServices) {
                    VisitService visitService = new VisitService(service.GetConnection());
                    visitService.Price = service.Price;
                    visitService.RelatedService = service;
                    selectedVisitServices.Add(visitService);
                }
                visitServices.AddRange(selectedVisitServices);

                //Добавляем выбранные услуги в список на форме
                vcbwServices.SuspendLayout();
                foreach (VisitService visitService in selectedVisitServices) {
                    //Получаем сотрудников предоставляющих услугу
                    ListData serviceWorkersListData = Worker.GetServiceWorkersListData(visitService.RelatedService.GetId(), connection);
                    serviceWorkersListData.Update();
                    List<Worker> serviceWorkers = Worker.GetList(serviceWorkersListData);
                    servicesWorkers.Add(serviceWorkers);

                    //Получаем ключ - значение для сотрудников
                    string[] workersKeys = new string[serviceWorkers.Count];
                    string[] workersNames = new string[serviceWorkers.Count];
                    for (int i = 0; i < serviceWorkers.Count; i++) {
                        workersKeys[i] = i.ToString();
                        workersNames[i] = serviceWorkers[i].GetFullName();
                    }

                    //Добавляем элемент на форму
                    vcbwServices.AddItem(
                        visitService.RelatedService.GetId().ToString(),
                        visitService.RelatedService.Name,
                        workersKeys,
                        workersNames
                    );

                    sidePanel.AddService(visitService.RelatedService.Name, visitService.RelatedService.Price);
                }
                vcbwServices.ResumeLayout();

                updateSales();
                calcPrices();
            }
        }


        private void comboBoxServiceWorkerChange_Event(int index, string key) {
            int workerIndex = Int32.Parse(key);
            visitServices[index].RelatedWorker = servicesWorkers[index][workerIndex];
            sidePanel.SetServiceWorkerAt(index, visitServices[index].RelatedWorker.GetFullName());
        }
     
        private void btnRemoveService_Event(int index) {
            if (visitServices[index].GetId() > 0)
                removedVisitServices.Add(visitServices[index]);
            visitServices.RemoveAt(index);
            servicesWorkers.RemoveAt(index);
            sidePanel.RemoveServiceAt(index);
            updateSales();
            calcPrices();
        }


        void dtpVisitDate_ValueChanged(object sender, EventArgs e) {
            DateTimePicker dtpVisitDate = sender as DateTimePicker;

            visit.VisitDate = dtpVisitDate.Value;

            sidePanel.SetVisitDate(dtpVisitDate.Value.ToString(AppConfig.DateFormat));
            updateSales();
            calcPrices();
        }
    }

}
