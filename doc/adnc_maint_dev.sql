/*
 Navicat MySQL Data Transfer

 Source Server         : 193.112.75.77
 Source Server Type    : MariaDB
 Source Server Version : 100504
 Source Host           : 193.112.75.77:13308
 Source Schema         : adnc_maint_dev

 Target Server Type    : MariaDB
 Target Server Version : 100504
 File Encoding         : 65001

 Date: 25/11/2020 20:12:29
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for SysCfg
-- ----------------------------
DROP TABLE IF EXISTS `SysCfg`;
CREATE TABLE `SysCfg`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `CfgDesc` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CfgName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CfgValue` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1606303844002 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '系统参数' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysCfg
-- ----------------------------
INSERT INTO `SysCfg` VALUES (1606203371016, 1600000000000, '2020-11-24 15:36:11.262574', NULL, NULL, '', 'appname', 'adnc', 1);
INSERT INTO `SysCfg` VALUES (1606303844001, NULL, NULL, 1600000000000, '2020-11-25 19:35:49.168030', '系统名称', 'app', 'adnc', 0);

-- ----------------------------
-- Table structure for SysDict
-- ----------------------------
DROP TABLE IF EXISTS `SysDict`;
CREATE TABLE `SysDict`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Num` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Pid` bigint(20) NULL DEFAULT NULL,
  `Tips` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1606305946010 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '字典' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysDict
-- ----------------------------
INSERT INTO `SysDict` VALUES (1606304212002, 1600000000000, '2020-11-25 19:36:52.130272', 1600000000000, '2020-11-25 20:05:46.936175', '性别', '0', 0, NULL, 0);
INSERT INTO `SysDict` VALUES (1606304212003, 1600000000000, '2020-11-25 19:36:52.130269', NULL, NULL, '男', '1', 1606304212002, NULL, 1);
INSERT INTO `SysDict` VALUES (1606304212004, 1600000000000, '2020-11-25 19:36:52.130272', NULL, NULL, '女', '2', 1606304212002, NULL, 1);

-- ----------------------------
-- Table structure for SysLoginLog
-- ----------------------------
DROP TABLE IF EXISTS `SysLoginLog`;
CREATE TABLE `SysLoginLog`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL,
  `Device` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Message` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Succeed` tinyint(1) NOT NULL,
  `UserId` bigint(20) NULL DEFAULT NULL,
  `Account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UserName` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `RemoteIpAddress` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 119530005997948929 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysLoginLog
-- ----------------------------

-- ----------------------------
-- Table structure for SysNotice
-- ----------------------------
DROP TABLE IF EXISTS `SysNotice`;
CREATE TABLE `SysNotice`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL,
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Content` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Type` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '通知' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysNotice
-- ----------------------------
INSERT INTO `SysNotice` VALUES (1, 1, '2020-01-11 08:53:20.000000', 1, '2019-01-08 23:30:58.000000', 'Adnc是一个轻量级的.Net Core微服务开发框架', '新时代的.NET', 10);
-- ----------------------------
-- Table structure for SysTask
-- ----------------------------
DROP TABLE IF EXISTS `SysTask`;
CREATE TABLE `SysTask`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Concurrent` bit(1) NULL DEFAULT NULL COMMENT '是否允许并发',
  `Cron` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '定时规则',
  `Data` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '执行参数',
  `Disabled` bit(1) NULL DEFAULT NULL COMMENT '是否禁用',
  `ExecAt` datetime(6) NULL DEFAULT NULL,
  `ExecResult` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '执行结果',
  `JobClass` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '执行类',
  `JobGroup` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '任务组名',
  `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '任务名',
  `Note` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '任务说明',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 70962226407804929 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '定时任务' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysTask
-- ----------------------------

-- ----------------------------
-- Table structure for SysTaskLog
-- ----------------------------
DROP TABLE IF EXISTS `SysTaskLog`;
CREATE TABLE `SysTaskLog`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ExecAt` datetime(6) NULL DEFAULT NULL,
  `ExecSuccess` bit(1) NULL DEFAULT NULL COMMENT '执行结果（成功:1、失败:0)',
  `IdTask` bigint(20) NULL DEFAULT NULL,
  `JobException` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '抛出异常',
  `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '任务名',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '定时任务日志' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysTaskLog
-- ----------------------------

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory`  (
  `MigrationId` varchar(95) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of __EFMigrationsHistory
-- ----------------------------
INSERT INTO `__EFMigrationsHistory` VALUES ('20201014072217_2020101401', '3.1.9');
INSERT INTO `__EFMigrationsHistory` VALUES ('20201014152042_2020101402', '3.1.9');
INSERT INTO `__EFMigrationsHistory` VALUES ('20201123161154_20201123_01', '3.1.9');
INSERT INTO `__EFMigrationsHistory` VALUES ('20201124032950_20201124_1', '3.1.9');

SET FOREIGN_KEY_CHECKS = 1;
