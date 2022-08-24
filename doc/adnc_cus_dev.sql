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

-- ----------------------------
-- Table structure for customer
-- ----------------------------
DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL,
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `nickname` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `realname` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for customerfinance
-- ----------------------------
DROP TABLE IF EXISTS `customerfinance`;
CREATE TABLE `customerfinance`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL,
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `balance` decimal(18, 4) NOT NULL,
  `rowversion` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`id`) USING BTREE,
  CONSTRAINT `fk_customerfinance_customer_id` FOREIGN KEY (`id`) REFERENCES `customer` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for customertransactionlog
-- ----------------------------
DROP TABLE IF EXISTS `customertransactionlog`;
CREATE TABLE `customertransactionlog`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `customerid` bigint(20) NOT NULL,
  `account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `exchangetype` int(11) NOT NULL,
  `exchagestatus` int(11) NOT NULL,
  `changingamount` decimal(18, 4) NOT NULL,
  `amount` decimal(18, 4) NOT NULL,
  `changedamount` decimal(18, 4) NOT NULL,
  `remark` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `ix_customertransactionlog_customerid`(`customerid`) USING BTREE,
  CONSTRAINT `fk_customertransactionlog_customer_customerid` FOREIGN KEY (`customerid`) REFERENCES `customer` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

DROP TABLE IF EXISTS `eventtracker`;
CREATE TABLE `eventtracker` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `eventid` bigint(20) NOT NULL,
  `trackername` varchar(50) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ROW_FORMAT=DYNAMIC COMMENT='事件跟踪/处理信息';

CREATE UNIQUE INDEX UK_EventId_TrackerName ON EventTracker(EventId, TrackerName);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;