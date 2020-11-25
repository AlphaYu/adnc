/*
 Navicat MySQL Data Transfer

 Source Server         : 193.112.75.77
 Source Server Type    : MariaDB
 Source Server Version : 100504
 Source Host           : 193.112.75.77:13308
 Source Schema         : adnc_cus_dev

 Target Server Type    : MariaDB
 Target Server Version : 100504
 File Encoding         : 65001

 Date: 25/11/2020 20:12:55
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for CusFinance
-- ----------------------------
DROP TABLE IF EXISTS `CusFinance`;
CREATE TABLE `CusFinance`  (
  `ID` bigint(20) NOT NULL,
  `CreateBy` bigint(20) NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL,
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Balance` decimal(18, 2) NOT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  CONSTRAINT `FK_CusFinance_Customer_ID` FOREIGN KEY (`ID`) REFERENCES `Customer` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of CusFinance
-- ----------------------------

-- ----------------------------
-- Table structure for CusTransactionLog
-- ----------------------------
DROP TABLE IF EXISTS `CusTransactionLog`;
CREATE TABLE `CusTransactionLog`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CustomerId` bigint(20) NOT NULL,
  `Account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ExchangeType` varchar(3) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ExchageStatus` varchar(3) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ChangingAmount` decimal(18, 2) NOT NULL,
  `Amount` decimal(18, 2) NOT NULL,
  `ChangedAmount` decimal(18, 2) NOT NULL,
  `Remark` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of CusTransactionLog
-- ----------------------------

-- ----------------------------
-- Table structure for Customer
-- ----------------------------
DROP TABLE IF EXISTS `Customer`;
CREATE TABLE `Customer`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL,
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Nickname` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Realname` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Customer
-- ----------------------------

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory`  (
  `MigrationId` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of __EFMigrationsHistory
-- ----------------------------
INSERT INTO `__EFMigrationsHistory` VALUES ('20201124033501_20201124_init', '3.1.9');

SET FOREIGN_KEY_CHECKS = 1;
