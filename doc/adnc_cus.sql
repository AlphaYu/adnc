/*
Navicat MariaDB Data Transfer

Source Server         : 193.112.75.77
Source Server Version : 100504
Source Host           : 193.112.75.77:13308
Source Database       : adnc_cus

Target Server Type    : MariaDB
Target Server Version : 100504
File Encoding         : 65001

Date: 2020-10-15 14:54:19
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for CusFinance
-- ----------------------------
DROP TABLE IF EXISTS `CusFinance`;
CREATE TABLE `CusFinance` (
  `ID` bigint(20) NOT NULL,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Account` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
  `Balance` decimal(18,2) NOT NULL,
  PRIMARY KEY (`ID`),
  CONSTRAINT `FK_CusFinance_Customer_ID` FOREIGN KEY (`ID`) REFERENCES `Customer` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for Customer
-- ----------------------------
DROP TABLE IF EXISTS `Customer`;
CREATE TABLE `Customer` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Account` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
  `Nickname` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
  `Realname` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=104367721055129601 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for CusTransactionLog
-- ----------------------------
DROP TABLE IF EXISTS `CusTransactionLog`;
CREATE TABLE `CusTransactionLog` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `Account` varchar(32) CHARACTER SET utf8mb4 DEFAULT NULL,
  `ExchangeType` varchar(3) CHARACTER SET utf8mb4 DEFAULT NULL,
  `ExchageStatus` varchar(3) CHARACTER SET utf8mb4 DEFAULT NULL,
  `ChangingAmount` decimal(18,2) NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `ChangedAmount` decimal(18,2) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8mb4 DEFAULT NULL,
  `CustomerId` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=103946664171147265 DEFAULT CHARSET=latin1;
