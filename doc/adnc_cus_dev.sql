/*
Navicat MariaDB Data Transfer

Source Server         : 193.112.75.77
Source Server Version : 100504
Source Host           : 193.112.75.77:13308
Source Database       : adnc_cus_dev

Target Server Type    : MariaDB
Target Server Version : 100504
File Encoding         : 65001

Date: 2020-11-09 12:48:07
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
-- Records of __EFMigrationsHistory
-- ----------------------------

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
-- Records of CusFinance
-- ----------------------------
INSERT INTO `CusFinance` VALUES ('108684515530117120', '23', '2020-10-26 21:53:28.214149', '0', '2020-10-26 22:26:18.670169', 'alpha', '414.00');

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
) ENGINE=InnoDB AUTO_INCREMENT=108684515530117121 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of Customer
-- ----------------------------
INSERT INTO `Customer` VALUES ('108684515530117120', '23', '2020-10-26 21:53:28.053480', null, null, 'alpha', 'alpha', 'alpha');

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
) ENGINE=InnoDB AUTO_INCREMENT=108684825703092225 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of CusTransactionLog
-- ----------------------------
INSERT INTO `CusTransactionLog` VALUES ('108684775623102464', '23', '2020-10-26 21:54:29.865121', 'alpha', '100', '20', '0.00', '101.00', '101.00', '', '0');
INSERT INTO `CusTransactionLog` VALUES ('108684789619494912', '23', '2020-10-26 21:54:33.185242', 'alpha', '100', '20', '101.00', '101.00', '202.00', '', '0');
INSERT INTO `CusTransactionLog` VALUES ('108684808409976832', '23', '2020-10-26 21:54:37.672477', 'alpha', '100', '20', '202.00', '104.00', '306.00', '', '0');
INSERT INTO `CusTransactionLog` VALUES ('108684825703092224', '23', '2020-10-26 21:54:41.794151', 'alpha', '100', '20', '306.00', '108.00', '414.00', '', '0');
