/*
Navicat MariaDB Data Transfer

Source Server         : 193.112.75.77
Source Server Version : 100504
Source Host           : 193.112.75.77:13308
Source Database       : adnc_maint

Target Server Type    : MariaDB
Target Server Version : 100504
File Encoding         : 65001

Date: 2020-10-15 14:54:40
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
-- Table structure for CMSArticle
-- ----------------------------
DROP TABLE IF EXISTS `CMSArticle`;
CREATE TABLE `CMSArticle` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间/注册时间',
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `Author` varchar(64) DEFAULT NULL COMMENT '作者',
  `Content` text DEFAULT NULL COMMENT '内容',
  `IdChannel` bigint(20) NOT NULL COMMENT '栏目id',
  `Img` varchar(64) DEFAULT NULL COMMENT '文章题图ID',
  `Title` varchar(128) DEFAULT NULL COMMENT '标题',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='文章';

-- ----------------------------
-- Table structure for CMSBanner
-- ----------------------------
DROP TABLE IF EXISTS `CMSBanner`;
CREATE TABLE `CMSBanner` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间/注册时间',
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `IdFile` bigint(20) DEFAULT NULL COMMENT 'banner图id',
  `Title` varchar(64) DEFAULT NULL COMMENT '标题',
  `Type` varchar(32) DEFAULT NULL COMMENT '类型',
  `Url` varchar(128) DEFAULT NULL COMMENT '点击banner跳转到url',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='文章';

-- ----------------------------
-- Table structure for CMSChannel
-- ----------------------------
DROP TABLE IF EXISTS `CMSChannel`;
CREATE TABLE `CMSChannel` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间/注册时间',
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `Code` varchar(64) DEFAULT NULL COMMENT '编码',
  `Name` varchar(64) DEFAULT NULL COMMENT '名称',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='文章栏目';

-- ----------------------------
-- Table structure for CMSContacts
-- ----------------------------
DROP TABLE IF EXISTS `CMSContacts`;
CREATE TABLE `CMSContacts` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间/注册时间',
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `Email` varchar(32) DEFAULT NULL COMMENT '电子邮箱',
  `Mobile` varchar(64) DEFAULT NULL COMMENT '联系电话',
  `Remark` varchar(128) DEFAULT NULL COMMENT '备注',
  `UserName` varchar(64) DEFAULT NULL COMMENT '邀约人名称',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='邀约信息';

-- ----------------------------
-- Table structure for Message
-- ----------------------------
DROP TABLE IF EXISTS `Message`;
CREATE TABLE `Message` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间/注册时间',
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `Content` text DEFAULT NULL COMMENT '消息内容',
  `Receiver` varchar(64) DEFAULT NULL COMMENT '接收者',
  `State` varchar(32) DEFAULT NULL COMMENT '消息类型,0:初始,1:成功,2:失败',
  `TplCode` varchar(32) DEFAULT NULL COMMENT '模板编码',
  `Type` varchar(32) DEFAULT NULL COMMENT '消息类型,0:短信,1:邮件',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='历史消息';

-- ----------------------------
-- Table structure for MessageSender
-- ----------------------------
DROP TABLE IF EXISTS `MessageSender`;
CREATE TABLE `MessageSender` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间/注册时间',
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `ClassName` varchar(64) DEFAULT NULL COMMENT '发送类',
  `Name` varchar(64) DEFAULT NULL COMMENT '名称',
  `TplCode` varchar(64) DEFAULT NULL COMMENT '短信运营商模板编号',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='消息发送者';

-- ----------------------------
-- Table structure for MessageTemplate
-- ----------------------------
DROP TABLE IF EXISTS `MessageTemplate`;
CREATE TABLE `MessageTemplate` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间/注册时间',
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `Code` varchar(32) DEFAULT NULL COMMENT '编号',
  `Cond` varchar(32) DEFAULT NULL COMMENT '发送条件',
  `Content` text DEFAULT NULL COMMENT '内容',
  `IDMessageSender` bigint(20) NOT NULL COMMENT '发送者id',
  `Title` varchar(64) DEFAULT NULL COMMENT '标题',
  `Type` varchar(32) DEFAULT NULL COMMENT '消息类型,0:短信,1:邮件',
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `FK942sadqk5x9kbrwil0psyek3n` (`IDMessageSender`) USING BTREE,
  CONSTRAINT `FK942sadqk5x9kbrwil0psyek3n` FOREIGN KEY (`IDMessageSender`) REFERENCES `MessageSender` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='消息模板';

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
-- Table structure for SysFileInfo
-- ----------------------------
DROP TABLE IF EXISTS `SysFileInfo`;
CREATE TABLE `SysFileInfo` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `OriginalFileName` varchar(255) DEFAULT NULL,
  `RealFileName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='文件';

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
) ENGINE=InnoDB AUTO_INCREMENT=104533136481849345 DEFAULT CHARSET=utf8;

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
