/*
Navicat MariaDB Data Transfer

Source Server         : 193.112.75.77
Source Server Version : 100504
Source Host           : 193.112.75.77:13308
Source Database       : adnc_maint_dev

Target Server Type    : MariaDB
Target Server Version : 100504
File Encoding         : 65001

Date: 2020-11-09 12:46:14
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of __EFMigrationsHistory
-- ----------------------------

-- ----------------------------
-- Table structure for SysCfg
-- ----------------------------
DROP TABLE IF EXISTS `SysCfg`;
CREATE TABLE `SysCfg` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `CfgDesc` text DEFAULT NULL COMMENT '备注',
  `CfgName` varchar(256) DEFAULT NULL COMMENT '参数名',
  `CfgValue` varchar(512) DEFAULT NULL COMMENT '参数值',
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=70678390889385985 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='系统参数';

-- ----------------------------
-- Records of SysCfg
-- ----------------------------
INSERT INTO `SysCfg` VALUES ('11603720275097', '23', '2020-10-26 21:51:15.121998', '23', '2020-10-26 21:51:20.001182', 'aaa', 'aaaaa', 'aaaaa', '1');
INSERT INTO `SysCfg` VALUES ('11604676749252', '23', '2020-11-06 23:32:30.413540', null, null, 'ssss', 'sss', 'sssss', '0');

-- ----------------------------
-- Table structure for SysDict
-- ----------------------------
DROP TABLE IF EXISTS `SysDict`;
CREATE TABLE `SysDict` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Num` varchar(255) DEFAULT NULL,
  `Pid` bigint(20) DEFAULT NULL,
  `Tips` varchar(255) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=31597804178960 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='字典';

-- ----------------------------
-- Records of SysDict
-- ----------------------------
INSERT INTO `SysDict` VALUES ('16', null, null, '23', '2020-10-30 12:23:19.274949', '状态', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('29', null, null, '23', '2020-08-08 22:45:05.876071', '性别', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('35', null, null, '23', '2020-08-08 14:33:43.938980', '账号状态', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('53', null, null, '23', '2020-08-13 18:29:20.247196', '证件类型', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('68', '1', '2019-01-13 14:18:21.000000', '23', '2020-08-19 10:29:39.017103', '是否', '0', '0', null, '0');

-- ----------------------------
-- Table structure for SysLoginLog
-- ----------------------------
DROP TABLE IF EXISTS `SysLoginLog`;
CREATE TABLE `SysLoginLog` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL,
  `Device` varchar(20) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Message` varchar(255) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Succeed` tinyint(1) NOT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  `Account` varchar(32) CHARACTER SET utf8mb4 DEFAULT NULL,
  `UserName` varchar(64) CHARACTER SET utf8mb4 DEFAULT NULL,
  `RemoteIpAddress` varchar(22) CHARACTER SET utf8mb4 DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of SysLoginLog
-- ----------------------------

-- ----------------------------
-- Table structure for SysNotice
-- ----------------------------
DROP TABLE IF EXISTS `SysNotice`;
CREATE TABLE `SysNotice` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Content` varchar(255) DEFAULT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='通知';

-- ----------------------------
-- Records of SysNotice
-- ----------------------------

-- ----------------------------
-- Table structure for SysTask
-- ----------------------------
DROP TABLE IF EXISTS `SysTask`;
CREATE TABLE `SysTask` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Concurrent` bit(1) DEFAULT NULL COMMENT '是否允许并发',
  `Cron` varchar(50) DEFAULT NULL COMMENT '定时规则',
  `Data` text DEFAULT NULL COMMENT '执行参数',
  `Disabled` bit(1) DEFAULT NULL COMMENT '是否禁用',
  `ExecAt` datetime(6) DEFAULT NULL,
  `ExecResult` text DEFAULT NULL COMMENT '执行结果',
  `JobClass` varchar(255) DEFAULT NULL COMMENT '执行类',
  `JobGroup` varchar(50) DEFAULT NULL COMMENT '任务组名',
  `Name` varchar(50) DEFAULT NULL COMMENT '任务名',
  `Note` varchar(255) DEFAULT NULL COMMENT '任务说明',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=70962226407804929 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='定时任务';

-- ----------------------------
-- Records of SysTask
-- ----------------------------

-- ----------------------------
-- Table structure for SysTaskLog
-- ----------------------------
DROP TABLE IF EXISTS `SysTaskLog`;
CREATE TABLE `SysTaskLog` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ExecAt` datetime(6) DEFAULT NULL,
  `ExecSuccess` bit(1) DEFAULT NULL COMMENT '执行结果（成功:1、失败:0)',
  `IdTask` bigint(20) DEFAULT NULL,
  `JobException` varchar(500) DEFAULT NULL COMMENT '抛出异常',
  `Name` varchar(50) DEFAULT NULL COMMENT '任务名',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='定时任务日志';

-- ----------------------------
-- Records of SysTaskLog
-- ----------------------------
