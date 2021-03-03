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

/*Table structure for table `Customer` */

DROP TABLE IF EXISTS `Customer`;

CREATE TABLE `Customer` (
  `Id` bigint(20) NOT NULL,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Account` varchar(16) NOT NULL,
  `Nickname` varchar(16) NOT NULL,
  `Realname` varchar(16) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*Data for the table `Customer` */

insert  into `Customer`(`Id`,`CreateBy`,`CreateTime`,`ModifyBy`,`ModifyTime`,`Account`,`Nickname`,`Realname`) values 
(154951941552738304,1600000000000,'2021-03-03 14:03:41.915499',NULL,NULL,'alpha3000','阿尔法','机器人'),
(154959990543749120,1600000000000,'2021-03-03 14:35:41.060569',NULL,NULL,'alpha3001','阿尔法1','机器人1'),
(154963538312564736,1600000000000,'2021-03-03 14:49:46.901587',NULL,NULL,'alpha6000','阿尔法6','阿铁打'),
(154964826672730112,1600000000000,'2021-03-03 14:54:54.049573',NULL,NULL,'alpha7000','阿尔法7','阿铁打');

/*Table structure for table `CustomerFinance` */

DROP TABLE IF EXISTS `CustomerFinance`;

CREATE TABLE `CustomerFinance` (
  `Id` bigint(20) NOT NULL,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Account` varchar(16) NOT NULL,
  `Balance` decimal(18,4) NOT NULL,
  `RowVersion` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`Id`),
  CONSTRAINT `FK_CustomerFinance_Customer_Id` FOREIGN KEY (`Id`) REFERENCES `Customer` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*Data for the table `CustomerFinance` */

insert  into `CustomerFinance`(`Id`,`CreateBy`,`CreateTime`,`ModifyBy`,`ModifyTime`,`Account`,`Balance`,`RowVersion`) values 
(154951941552738304,1600000000000,'2021-03-03 14:03:41.915606',NULL,NULL,'alpha3000',0.0000,'2021-03-03 14:03:42.646598'),
(154959990543749120,1600000000000,'2021-03-03 14:35:41.060684',0,'2021-03-03 14:35:58.475628','alpha3001',1000.0000,'2021-03-03 14:35:58.892853'),
(154963538312564736,1600000000000,'2021-03-03 14:49:46.901708',0,'2021-03-03 14:57:53.391433','alpha6000',888.0000,'2021-03-03 14:57:53.797326'),
(154964826672730112,1600000000000,'2021-03-03 14:54:54.049702',0,'2021-03-03 14:55:09.728292','alpha7000',888.0000,'2021-03-03 14:55:10.142105');

/*Table structure for table `CustomerTransactionLog` */

DROP TABLE IF EXISTS `CustomerTransactionLog`;

CREATE TABLE `CustomerTransactionLog` (
  `Id` bigint(20) NOT NULL,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `CustomerId` bigint(20) NOT NULL,
  `Account` varchar(16) NOT NULL,
  `ExchangeType` int(11) NOT NULL,
  `ExchageStatus` int(11) NOT NULL,
  `ChangingAmount` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `ChangedAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CustomerTransactionLog_CustomerId` (`CustomerId`),
  CONSTRAINT `FK_CustomerTransactionLog_Customer_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `Customer` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*Data for the table `CustomerTransactionLog` */

insert  into `CustomerTransactionLog`(`Id`,`CreateBy`,`CreateTime`,`CustomerId`,`Account`,`ExchangeType`,`ExchageStatus`,`ChangingAmount`,`Amount`,`ChangedAmount`,`Remark`) values 
(154960061616230400,1600000000000,'2021-03-03 14:35:57.792557',154959990543749120,'alpha3001',8000,2008,0.0000,1000.0000,1000.0000,''),
(154964890296127488,1600000000000,'2021-03-03 14:55:09.036228',154964826672730112,'alpha7000',8000,2008,0.0000,888.0000,888.0000,''),
(154965577457340416,1600000000000,'2021-03-03 14:57:52.850437',154963538312564736,'alpha6000',8000,2008,0.0000,888.0000,888.0000,'');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
