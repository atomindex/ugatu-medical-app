/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50525
Source Host           : localhost:3306
Source Database       : medic

Target Server Type    : MYSQL
Target Server Version : 50525
File Encoding         : 65001

Date: 2016-08-20 20:06:20
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for categories
-- ----------------------------
DROP TABLE IF EXISTS `categories`;
CREATE TABLE `categories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `removed` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of categories
-- ----------------------------
INSERT INTO `categories` VALUES ('1', 'Студент', '0');
INSERT INTO `categories` VALUES ('2', 'Рабочий', '0');
INSERT INTO `categories` VALUES ('3', 'Инвалид', '0');

-- ----------------------------
-- Table structure for patients
-- ----------------------------
DROP TABLE IF EXISTS `patients`;
CREATE TABLE `patients` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `first_name` varchar(100) NOT NULL,
  `middle_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `sex` int(11) NOT NULL DEFAULT '-1',
  `birthday` date NOT NULL DEFAULT '1901-01-01',
  `removed` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of patients
-- ----------------------------
INSERT INTO `patients` VALUES ('1', 'Ярослава', 'Игоревна', 'Кузьмина', '1', '1963-11-23', '0');
INSERT INTO `patients` VALUES ('2', 'Давид', 'Аркадьевич', 'Белозёров', '0', '1986-05-17', '0');
INSERT INTO `patients` VALUES ('3', 'Аркадий', 'Тарасович', 'Беляев', '0', '1991-08-16', '0');
INSERT INTO `patients` VALUES ('4', 'Муза', 'Семеновна', 'Григорьева', '1', '1980-05-02', '0');
INSERT INTO `patients` VALUES ('5', 'Семён', 'Альбертович', 'Сорокин', '0', '1981-01-04', '0');
INSERT INTO `patients` VALUES ('6', 'Галя', 'Антоновна', 'Стручкова', '1', '2000-08-16', '0');
INSERT INTO `patients` VALUES ('7', 'Власта', 'Львовна', 'Холодкова', '1', '1955-06-04', '0');

-- ----------------------------
-- Table structure for patients_categories
-- ----------------------------
DROP TABLE IF EXISTS `patients_categories`;
CREATE TABLE `patients_categories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `patient_id` int(11) NOT NULL,
  `category_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `patient_id` (`patient_id`,`category_id`),
  KEY `patients_categories_category_id` (`category_id`),
  CONSTRAINT `patients_categories_category_id` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`) ON DELETE CASCADE,
  CONSTRAINT `patients_categories_patient_id` FOREIGN KEY (`patient_id`) REFERENCES `patients` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of patients_categories
-- ----------------------------
INSERT INTO `patients_categories` VALUES ('1', '1', '2');
INSERT INTO `patients_categories` VALUES ('2', '3', '1');
INSERT INTO `patients_categories` VALUES ('3', '4', '2');
INSERT INTO `patients_categories` VALUES ('5', '5', '2');
INSERT INTO `patients_categories` VALUES ('4', '5', '3');

-- ----------------------------
-- Table structure for sales
-- ----------------------------
DROP TABLE IF EXISTS `sales`;
CREATE TABLE `sales` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` text NOT NULL,
  `percent` int(11) NOT NULL,
  `cond_services_count` text NOT NULL,
  `cond_services_sum_price` text NOT NULL,
  `cond_patient_age` text NOT NULL,
  `cond_patient_sex` text NOT NULL,
  `cond_patient_category` text NOT NULL,
  `cond_visit_number` text NOT NULL,
  `cond_visit_date` text NOT NULL,
  `removed` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sales
-- ----------------------------
INSERT INTO `sales` VALUES ('1', 'Для пенсионеров', 'Для пенсионеров - мужчин', '5', '|0', '|0', '>=|60', '=|0', '|0', '|0', '|2016-08-16', '0');
INSERT INTO `sales` VALUES ('2', 'Для пенсионеров', 'Для пенсионеров - женщин', '5', '|0', '|0', '>=|55', '=|1', '|0', '|0', '|2016-08-16', '0');
INSERT INTO `sales` VALUES ('3', 'Для студентов', 'Для студентов', '2', '|0', '|0', '|0', '|-1', '=|1', '|0', '|2016-08-16', '0');

-- ----------------------------
-- Table structure for services
-- ----------------------------
DROP TABLE IF EXISTS `services`;
CREATE TABLE `services` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `price` int(11) NOT NULL,
  `removed` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of services
-- ----------------------------
INSERT INTO `services` VALUES ('1', 'Аллерголог - первичный прием', '950', '0');
INSERT INTO `services` VALUES ('2', 'Аллерголог - повторный прием', '800', '0');
INSERT INTO `services` VALUES ('3', 'Аллерголог - кожная проба с гистамином', '160', '0');
INSERT INTO `services` VALUES ('4', 'Терапевт - первичный прием', '950', '0');
INSERT INTO `services` VALUES ('5', 'Терапевт - повторный прием', '800', '0');
INSERT INTO `services` VALUES ('6', 'Консультация гастроэнтеролога - диетолога', '3000', '0');
INSERT INTO `services` VALUES ('7', 'Гастроэтерология - нутритивная поддержка в условии кабинета озонотерапии', '940', '0');
INSERT INTO `services` VALUES ('8', 'Психотерапия - первичный прием', '2500', '0');
INSERT INTO `services` VALUES ('9', 'Психотерапия - повторный прием', '2500', '0');
INSERT INTO `services` VALUES ('10', 'Забор крови из вены', '200', '0');
INSERT INTO `services` VALUES ('11', 'Прививка', '110', '0');
INSERT INTO `services` VALUES ('12', 'Прививка от кори', '200', '0');
INSERT INTO `services` VALUES ('13', 'Осмотр врача-стоматолога', '155', '0');
INSERT INTO `services` VALUES ('14', 'Осмотр врача-уролога', '150', '0');

-- ----------------------------
-- Table structure for specialties
-- ----------------------------
DROP TABLE IF EXISTS `specialties`;
CREATE TABLE `specialties` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `removed` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of specialties
-- ----------------------------
INSERT INTO `specialties` VALUES ('1', 'Аллерголог', '0');
INSERT INTO `specialties` VALUES ('2', 'Терапевт', '0');
INSERT INTO `specialties` VALUES ('3', 'Гастроэнтеролог - диетолог', '0');
INSERT INTO `specialties` VALUES ('4', 'Психотерапевт', '0');
INSERT INTO `specialties` VALUES ('5', 'Медсестра', '0');
INSERT INTO `specialties` VALUES ('6', 'Стоматолог', '0');
INSERT INTO `specialties` VALUES ('7', 'Уролог', '0');

-- ----------------------------
-- Table structure for visits
-- ----------------------------
DROP TABLE IF EXISTS `visits`;
CREATE TABLE `visits` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `patient_id` int(11) NOT NULL,
  `visit_date` date NOT NULL,
  `patient_sex` int(11) NOT NULL,
  `patient_age` int(11) NOT NULL,
  `price` double NOT NULL,
  `removed` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of visits
-- ----------------------------
INSERT INTO `visits` VALUES ('1', '1', '2016-08-16', '1', '52', '1150', '0');
INSERT INTO `visits` VALUES ('2', '4', '2016-08-16', '1', '36', '2045', '0');
INSERT INTO `visits` VALUES ('3', '7', '2016-08-16', '1', '61', '2522.25', '0');
INSERT INTO `visits` VALUES ('4', '4', '2016-08-17', '1', '36', '800', '0');
INSERT INTO `visits` VALUES ('5', '6', '2016-08-17', '1', '16', '1250', '0');
INSERT INTO `visits` VALUES ('6', '3', '2016-08-17', '0', '25', '3528', '0');
INSERT INTO `visits` VALUES ('7', '2', '2016-08-17', '0', '30', '1110', '0');
INSERT INTO `visits` VALUES ('8', '5', '2016-08-17', '0', '35', '1150', '0');
INSERT INTO `visits` VALUES ('9', '2', '2016-08-19', '0', '30', '5610', '0');
INSERT INTO `visits` VALUES ('10', '5', '2016-08-19', '0', '35', '3600', '0');
INSERT INTO `visits` VALUES ('11', '7', '2016-08-19', '1', '61', '6887.5', '0');
INSERT INTO `visits` VALUES ('12', '6', '2016-08-20', '1', '16', '6300', '0');
INSERT INTO `visits` VALUES ('13', '3', '2016-08-20', '0', '25', '264.6', '0');
INSERT INTO `visits` VALUES ('14', '4', '2016-08-20', '1', '36', '1295', '0');

-- ----------------------------
-- Table structure for visits_sales
-- ----------------------------
DROP TABLE IF EXISTS `visits_sales`;
CREATE TABLE `visits_sales` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `visit_id` int(11) NOT NULL,
  `sale_id` int(11) NOT NULL,
  `sale_percent` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `visit_id` (`visit_id`,`sale_id`),
  KEY `visits_sales_sale_id` (`sale_id`),
  CONSTRAINT `visits_sales_sale_id` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`id`) ON DELETE CASCADE,
  CONSTRAINT `visits_sales_visit_id` FOREIGN KEY (`visit_id`) REFERENCES `visits` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of visits_sales
-- ----------------------------
INSERT INTO `visits_sales` VALUES ('1', '3', '2', '5');
INSERT INTO `visits_sales` VALUES ('2', '6', '3', '2');
INSERT INTO `visits_sales` VALUES ('3', '11', '2', '5');
INSERT INTO `visits_sales` VALUES ('4', '13', '3', '2');

-- ----------------------------
-- Table structure for visits_services
-- ----------------------------
DROP TABLE IF EXISTS `visits_services`;
CREATE TABLE `visits_services` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `visit_id` int(11) NOT NULL,
  `service_id` int(11) NOT NULL,
  `worker_id` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `visit_id` (`visit_id`,`service_id`,`worker_id`),
  KEY `visits_services_worker_id` (`worker_id`),
  KEY `visits_services_service_id` (`service_id`),
  CONSTRAINT `visits_services_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE CASCADE,
  CONSTRAINT `visits_services_visit_id` FOREIGN KEY (`visit_id`) REFERENCES `visits` (`id`) ON DELETE CASCADE,
  CONSTRAINT `visits_services_worker_id` FOREIGN KEY (`worker_id`) REFERENCES `workers` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of visits_services
-- ----------------------------
INSERT INTO `visits_services` VALUES ('1', '1', '1', '1', '950');
INSERT INTO `visits_services` VALUES ('2', '1', '10', '6', '200');
INSERT INTO `visits_services` VALUES ('3', '2', '4', '8', '950');
INSERT INTO `visits_services` VALUES ('4', '2', '7', '3', '940');
INSERT INTO `visits_services` VALUES ('5', '2', '13', '10', '155');
INSERT INTO `visits_services` VALUES ('6', '3', '13', '10', '155');
INSERT INTO `visits_services` VALUES ('7', '3', '8', '5', '2500');
INSERT INTO `visits_services` VALUES ('8', '4', '5', '8', '800');
INSERT INTO `visits_services` VALUES ('9', '5', '7', '4', '940');
INSERT INTO `visits_services` VALUES ('10', '5', '10', '6', '200');
INSERT INTO `visits_services` VALUES ('11', '5', '11', '6', '110');
INSERT INTO `visits_services` VALUES ('12', '6', '4', '8', '950');
INSERT INTO `visits_services` VALUES ('13', '6', '8', '5', '2500');
INSERT INTO `visits_services` VALUES ('14', '6', '14', '11', '150');
INSERT INTO `visits_services` VALUES ('15', '7', '3', '1', '160');
INSERT INTO `visits_services` VALUES ('16', '7', '1', '1', '950');
INSERT INTO `visits_services` VALUES ('17', '8', '1', '2', '950');
INSERT INTO `visits_services` VALUES ('18', '8', '12', '6', '200');
INSERT INTO `visits_services` VALUES ('19', '9', '6', '4', '3000');
INSERT INTO `visits_services` VALUES ('20', '9', '11', '6', '110');
INSERT INTO `visits_services` VALUES ('21', '9', '9', '5', '2500');
INSERT INTO `visits_services` VALUES ('22', '10', '1', '1', '950');
INSERT INTO `visits_services` VALUES ('23', '10', '14', '11', '150');
INSERT INTO `visits_services` VALUES ('24', '10', '9', '5', '2500');
INSERT INTO `visits_services` VALUES ('25', '11', '2', '2', '800');
INSERT INTO `visits_services` VALUES ('26', '11', '6', '4', '3000');
INSERT INTO `visits_services` VALUES ('27', '11', '8', '5', '2500');
INSERT INTO `visits_services` VALUES ('28', '11', '4', '7', '950');
INSERT INTO `visits_services` VALUES ('29', '12', '2', '2', '800');
INSERT INTO `visits_services` VALUES ('30', '12', '6', '4', '3000');
INSERT INTO `visits_services` VALUES ('31', '12', '8', '5', '2500');
INSERT INTO `visits_services` VALUES ('32', '13', '3', '1', '160');
INSERT INTO `visits_services` VALUES ('33', '13', '11', '6', '110');
INSERT INTO `visits_services` VALUES ('34', '14', '12', '6', '200');
INSERT INTO `visits_services` VALUES ('35', '14', '13', '10', '155');
INSERT INTO `visits_services` VALUES ('36', '14', '7', '3', '940');

-- ----------------------------
-- Table structure for workers
-- ----------------------------
DROP TABLE IF EXISTS `workers`;
CREATE TABLE `workers` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `first_name` varchar(100) NOT NULL,
  `middle_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `phone` varchar(50) NOT NULL,
  `address` text NOT NULL,
  `removed` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of workers
-- ----------------------------
INSERT INTO `workers` VALUES ('1', 'Прокофий', 'Григорьевич', 'Сергеев', '8 (925) 940-74-44', '452494, г. Варнавино, ул. Бебеля, дом 79, квартира 126', '0');
INSERT INTO `workers` VALUES ('2', 'Вероника', 'Федоровна', 'Тарская', '8 (913) 368-21-65', '690982, г. Домбаровский, ул. Байдукова, дом 8, квартира 161', '0');
INSERT INTO `workers` VALUES ('3', 'Савелий', 'Макарович', 'Никифоров', '8 (949) 197-77-96', '601025, г. Волот, ул. Дальняя, дом 23, квартира 213', '0');
INSERT INTO `workers` VALUES ('4', 'Клавдия', 'Игоревна', 'Дмитриева', '8 (975) 734-44-93', '109649, г. Чкаловск, ул. Александра Завидова, дом 48, квартира 210', '0');
INSERT INTO `workers` VALUES ('5', 'Самуил', 'Филиппович', 'Чаурин', '8 (978) 170-54-34', '646007, г. Новочеркасск, ул. Барвихинская, дом 50, квартира 119', '0');
INSERT INTO `workers` VALUES ('6', 'Алиса', 'Егоровна', 'Морозова', '8 (960) 278-42-90', '626030, г. Спас-Клепики, ул. Хрущёва, дом 33, квартира 284', '0');
INSERT INTO `workers` VALUES ('7', 'Анисья', 'Сергеевна', 'Лютова', '8 (923) 494-25-51', '111727, г. Яя, ул. Александра Завидова, дом 48, квартира 15', '0');
INSERT INTO `workers` VALUES ('8', 'Харлампий', 'Максович', 'Волков', '8 (904) 654-51-24', '607051, г. Горняк, ул. Багрицкого, дом 53, квартира 124', '0');
INSERT INTO `workers` VALUES ('9', 'Иннокентий', 'Степанович', 'Скачков', '8 (952) 739-36-63', '683982, г. Октябрьский, ул. Заречная, дом 14, квартира 149', '0');
INSERT INTO `workers` VALUES ('10', 'Михей', 'Святославович', 'Мартынов', '8 (905) 335-75-81', '102067, г. Толбазы, ул. Барклая, дом 92, квартира 159', '0');
INSERT INTO `workers` VALUES ('11', 'Бруно', 'Григорьевич', 'Сидоров', '8 (967) 675-51-20', '665749, г. Убинское, ул. Академическая, дом 69, квартира 22', '0');

-- ----------------------------
-- Table structure for workers_services
-- ----------------------------
DROP TABLE IF EXISTS `workers_services`;
CREATE TABLE `workers_services` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `worker_id` int(11) NOT NULL,
  `service_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `worker_id` (`worker_id`,`service_id`),
  KEY `workers_services_service_id` (`service_id`),
  CONSTRAINT `workers_services_service_id` FOREIGN KEY (`service_id`) REFERENCES `services` (`id`) ON DELETE CASCADE,
  CONSTRAINT `workers_services_worker_id` FOREIGN KEY (`worker_id`) REFERENCES `workers` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of workers_services
-- ----------------------------
INSERT INTO `workers_services` VALUES ('2', '1', '1');
INSERT INTO `workers_services` VALUES ('3', '1', '2');
INSERT INTO `workers_services` VALUES ('1', '1', '3');
INSERT INTO `workers_services` VALUES ('4', '2', '1');
INSERT INTO `workers_services` VALUES ('5', '2', '2');
INSERT INTO `workers_services` VALUES ('7', '3', '6');
INSERT INTO `workers_services` VALUES ('6', '3', '7');
INSERT INTO `workers_services` VALUES ('9', '4', '6');
INSERT INTO `workers_services` VALUES ('8', '4', '7');
INSERT INTO `workers_services` VALUES ('10', '5', '8');
INSERT INTO `workers_services` VALUES ('11', '5', '9');
INSERT INTO `workers_services` VALUES ('12', '6', '10');
INSERT INTO `workers_services` VALUES ('13', '6', '11');
INSERT INTO `workers_services` VALUES ('14', '6', '12');
INSERT INTO `workers_services` VALUES ('16', '7', '4');
INSERT INTO `workers_services` VALUES ('15', '7', '5');
INSERT INTO `workers_services` VALUES ('17', '8', '4');
INSERT INTO `workers_services` VALUES ('18', '8', '5');
INSERT INTO `workers_services` VALUES ('19', '9', '13');
INSERT INTO `workers_services` VALUES ('20', '10', '13');
INSERT INTO `workers_services` VALUES ('21', '11', '14');

-- ----------------------------
-- Table structure for workers_specialties
-- ----------------------------
DROP TABLE IF EXISTS `workers_specialties`;
CREATE TABLE `workers_specialties` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `worker_id` int(11) NOT NULL,
  `specialty_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `worker_id` (`worker_id`,`specialty_id`),
  KEY `workers_specialties_specialty_id` (`specialty_id`),
  CONSTRAINT `workers_specialties_specialty_id` FOREIGN KEY (`specialty_id`) REFERENCES `specialties` (`id`) ON DELETE CASCADE,
  CONSTRAINT `workers_specialties_worker_id` FOREIGN KEY (`worker_id`) REFERENCES `workers` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of workers_specialties
-- ----------------------------
INSERT INTO `workers_specialties` VALUES ('1', '1', '1');
INSERT INTO `workers_specialties` VALUES ('2', '2', '1');
INSERT INTO `workers_specialties` VALUES ('3', '3', '3');
INSERT INTO `workers_specialties` VALUES ('4', '4', '3');
INSERT INTO `workers_specialties` VALUES ('5', '5', '4');
INSERT INTO `workers_specialties` VALUES ('6', '6', '5');
INSERT INTO `workers_specialties` VALUES ('7', '7', '2');
INSERT INTO `workers_specialties` VALUES ('8', '8', '2');
INSERT INTO `workers_specialties` VALUES ('9', '9', '6');
INSERT INTO `workers_specialties` VALUES ('10', '10', '6');
INSERT INTO `workers_specialties` VALUES ('11', '11', '7');
