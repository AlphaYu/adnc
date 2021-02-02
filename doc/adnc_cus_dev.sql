/*
SQLyog Community v13.1.6 (64 bit)
MySQL - 10.5.8-MariaDB-1:10.5.8+maria~focal : Database - adnc_cus_dev
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`adnc_cus_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `adnc_cus_dev`;

/*Table structure for table `CusFinance` */

DROP TABLE IF EXISTS `CusFinance`;

CREATE TABLE `CusFinance` (
  `Id` bigint(20) NOT NULL DEFAULT 0,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Account` varchar(16) NOT NULL,
  `Balance` decimal(18,2) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  CONSTRAINT `FK_CusFinance_Customer_Id` FOREIGN KEY (`Id`) REFERENCES `Customer` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

/*Data for the table `CusFinance` */

/*Table structure for table `Customer` */

DROP TABLE IF EXISTS `Customer`;

CREATE TABLE `Customer` (
  `Id` bigint(20) NOT NULL DEFAULT 0,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Account` varchar(16) NOT NULL,
  `Nickname` varchar(16) NOT NULL,
  `Realname` varchar(16) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

/*Data for the table `Customer` */

insert  into `Customer`(`Id`,`CreateBy`,`CreateTime`,`ModifyBy`,`ModifyTime`,`Account`,`Nickname`,`Realname`) values 
(144487932374421504,1600000000000,'2021-02-02 17:03:27.790833',1600000000000,'2021-02-02 17:03:27.790838','alpha2008','测试','测试用户');

/*Table structure for table `CusTransactionLog` */

DROP TABLE IF EXISTS `CusTransactionLog`;

CREATE TABLE `CusTransactionLog` (
  `Id` bigint(20) NOT NULL DEFAULT 0,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `CustomerId` bigint(20) NOT NULL,
  `Account` varchar(16) DEFAULT NULL,
  `ExchangeType` varchar(3) DEFAULT NULL,
  `ExchageStatus` varchar(3) DEFAULT NULL,
  `ChangingAmount` decimal(18,2) NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `ChangedAmount` decimal(18,2) NOT NULL,
  `Remark` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC;

/*Data for the table `CusTransactionLog` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
