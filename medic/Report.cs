using medic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace medic {

    public static class Report {

        public static List<string[]> GetServicesProfitList(DBConnection connection, DateTime dateFrom, DateTime dateTo) {
            return connection.Select(
                "SELECT sc.name, COALESCE((" +
	                "SELECT SUM(visits_services.price) FROM visits_services" + 
	               " JOIN visits ON visits.id = visits_services.visit_id" + 
                   " WHERE visits_services.service_id = sc.id AND" + 
				         " visits.removed = 0 AND" + 
				         " visits.visit_date BETWEEN '" + dateFrom.ToString(AppConfig.DatabaseDateFormat) + " 00:00:00' AND '" + dateTo.ToString(AppConfig.DatabaseDateFormat) + " 23:59:59'" + 
	               " GROUP BY visits_services.service_id" + 
                "), 0) as profit, sc.id FROM services as sc"
            );
        }

        public static List<string[]> GetWorkerServicesList(int workerId, DBConnection connection, DateTime dateFrom, DateTime dateTo) {
            return connection.Select(
                "SELECT DATE_FORMAT(visits.visit_date,'%d.%m.%Y'), COALESCE((" +
                    "SELECT GROUP_CONCAT(categories.name SEPARATOR ', ') FROM patients_categories" +
                   " JOIN categories ON categories.id = patients_categories.category_id" + 
                   " WHERE patients_categories.patient_id = patients.id" +
                   " GROUP BY patients_categories.patient_id" +
                "), '-'), CONCAT(patients.last_name, ' ', patients.first_name, ' ', patients.middle_name), services.name, visits_services.price, " +
               " workers.id as worker_id, CONCAT(workers.last_name, ' ', workers.first_name, ' ', workers.middle_name)" +
               " FROM visits_services" +
               " JOIN visits ON visits.id = visits_services.visit_id" + 
               " JOIN patients ON patients.id = visits.patient_id" + 
               " JOIN services ON services.id = visits_services.service_id" +
               " JOIN workers ON workers.id = visits_services.worker_id" +
               " WHERE visits_services.worker_id = " + workerId.ToString() + 
                     " AND visits.visit_date BETWEEN '" + dateFrom.ToString(AppConfig.DatabaseDateFormat) + " 00:00:00' AND '" + dateTo.ToString(AppConfig.DatabaseDateFormat) + " 23:59:59'" +
               " ORDER BY visits.visit_date ASC"
            );
        }

        public static List<string[]> GetPatientVisitsList(int patientId, DBConnection connection) {
            return connection.Select(
                "SELECT DATE_FORMAT(visits.visit_date, '%d.%m.%Y'), " +
                      " services_info.services_names," +
			          " services_info.services_count," +
			          " services_info.services_total_price," +
			          " COALESCE((SELECT SUM(sale_percent) FROM visits_sales WHERE visit_id = visits.id), 0)," +
		              " visits.price as sale_price" +
               " FROM visits" +
               " JOIN (" +
		            " SELECT visits_services.visit_id," +
				           " GROUP_CONCAT(services.name SEPARATOR ', \n') as services_names," +
					       " COUNT(*) as services_count," +
					       " SUM(visits_services.price) as services_total_price" +
		            " FROM visits_services" +
		            " JOIN services ON services.id = visits_services.service_id" +
		            " WHERE visits_services.visit_id IN (" +
			            "SELECT id FROM visits WHERE patient_id = " + patientId.ToString() +
		             ")" +
		            " GROUP BY visits_services.visit_id" +
               " ) services_info ON services_info.visit_id = visits.id" +
               " ORDER BY visits.visit_date ASC"
            );
        }

        public static List<string[]> GetServicesList(DBConnection connection, DateTime date) {
            return connection.Select(
                "SELECT services.name," +
                      " COUNT(*) as services_count," +
                      " SUM(visits_services.price)," +
                      " workers.id as worker_id," +
                      " CONCAT(workers.last_name, ' ', workers.first_name, ' ', workers.middle_name) as worker_name" +
                " FROM workers" +
                " JOIN visits_services ON visits_services.worker_id = workers.id" +
                " JOIN visits ON visits.id = visits_services.visit_id" +
                " JOIN services ON services.id = visits_services.service_id" +
                " WHERE DATE(visits.visit_date) = '" + date.ToString(AppConfig.DatabaseDateFormat) + "'" +
                " GROUP BY services.id, workers.id" +
                " ORDER BY workers.id ASC"
            );
        }
    }

}
