/*
Navicat MariaDB Data Transfer

Source Server         : 193.112.75.77
Source Server Version : 100504
Source Host           : 193.112.75.77:13308
Source Database       : AlphaNetCore

Target Server Type    : MariaDB
Target Server Version : 100504
File Encoding         : 65001

Date: 2020-08-02 21:56:02
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
INSERT INTO `__EFMigrationsHistory` VALUES ('20200628074855_user', '3.1.5');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200701060730_user-row-version', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200701130414_userfinance', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200701130759_userfinance-2', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200703084434_userfinace-rowversion', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200703085004_userfinace-rowversion-2', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200703151703_userfinace-role', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200703161752_menu', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713071837_dict', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713135533_cfg', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713135724_cfg2', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713145331_cfg3', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713145548_cfg4', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713153549_cfg5', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713155019_cfg6', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200713164753_cfg7', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200715153942_sysoperationlog', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200717133207_sysloginlog', '3.1.0');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200728145917_idchane', '3.1.6');

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
-- Records of CMSArticle
-- ----------------------------

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
-- Records of CMSBanner
-- ----------------------------

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
-- Records of CMSChannel
-- ----------------------------

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
-- Records of CMSContacts
-- ----------------------------

-- ----------------------------
-- Table structure for EbPublished
-- ----------------------------
DROP TABLE IF EXISTS `EbPublished`;
CREATE TABLE `EbPublished` (
  `Id` bigint(20) NOT NULL,
  `Version` varchar(20) DEFAULT NULL,
  `Name` varchar(200) NOT NULL,
  `Content` longtext DEFAULT NULL,
  `Retries` int(11) DEFAULT NULL,
  `Added` datetime NOT NULL,
  `ExpiresAt` datetime DEFAULT NULL,
  `StatusName` varchar(40) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ExpiresAt` (`ExpiresAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of EbPublished
-- ----------------------------
INSERT INTO `EbPublished` VALUES ('1279701738769248256', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279701738769248256\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 17:00:44 +08:00\",\"cap-corr-id\":\"1279701738769248256\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T17:00:44.8859228+08:00\"}', '0', '2020-07-05 17:00:45', '2020-07-06 17:00:45', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279719054475735040', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279719054475735040\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:09:33 +08:00\",\"cap-corr-id\":\"1279719054475735040\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T18:09:33.2695215+08:00\"}', '3', '2020-07-05 18:09:33', '2020-07-06 18:14:35', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279719316275802112', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279719316275802112\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:10:35 +08:00\",\"cap-corr-id\":\"1279719316275802112\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T18:10:35.6928583+08:00\"}', '0', '2020-07-05 18:10:35', '2020-07-06 18:10:35', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279719777215471616', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279719777215471616\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:12:25 +08:00\",\"cap-corr-id\":\"1279719777215471616\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T18:12:25.5842836+08:00\"}', '0', '2020-07-05 18:12:25', '2020-07-06 18:12:26', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279720499409424384', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279720499409424384\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:15:17 +08:00\",\"cap-corr-id\":\"1279720499409424384\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T18:15:17.7707678+08:00\"}', '0', '2020-07-05 18:15:17', '2020-07-06 18:15:18', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279730562803011584', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279730562803011584\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:55:17 +08:00\",\"cap-corr-id\":\"1279730562803011584\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T18:55:17.0734837+08:00\"}', '0', '2020-07-05 18:55:17', '2020-07-06 18:55:17', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279739305313361920', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279739305313361920\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 19:30:01 +08:00\",\"cap-corr-id\":\"1279739305313361920\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T19:30:01.4474742+08:00\"}', '0', '2020-07-05 19:30:01', '2020-07-06 19:30:01', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279780881211392000', 'v1', 'test.show.time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279780881211392000\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 22:15:13 +08:00\",\"cap-corr-id\":\"1279780881211392000\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T22:15:13.9113101+08:00\"}', '0', '2020-07-05 22:15:14', '2020-07-06 22:15:14', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279781049311387648', 'v1', 'test.show.string', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279781049311387648\",\"cap-msg-name\":\"test.show.string\",\"cap-msg-type\":\"String\",\"cap-senttime\":\"2020/7/5 22:15:53 +08:00\",\"cap-corr-id\":\"1279781049311387648\",\"cap-corr-seq\":\"0\"},\"Value\":\"hello world\"}', '0', '2020-07-05 22:15:54', '2020-07-06 22:15:54', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279784324154392576', 'v1', 'test_show_time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279784324154392576\",\"cap-msg-name\":\"test_show_time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 22:28:54 +08:00\",\"cap-corr-id\":\"1279784324154392576\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T22:28:54.7743127+08:00\"}', '0', '2020-07-05 22:28:55', '2020-07-06 22:28:55', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279784348674293760', 'v1', 'test_show_string', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279784348674293760\",\"cap-msg-name\":\"test_show_string\",\"cap-msg-type\":\"String\",\"cap-senttime\":\"2020/7/5 22:29:00 +08:00\",\"cap-corr-id\":\"1279784348674293760\",\"cap-corr-seq\":\"0\"},\"Value\":\"hello world\"}', '0', '2020-07-05 22:29:00', '2020-07-06 22:29:00', 'Succeeded');
INSERT INTO `EbPublished` VALUES ('1279788420557942784', 'v1', 'test_show_time', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279788420557942784\",\"cap-msg-name\":\"test_show_time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 22:45:11 +08:00\",\"cap-corr-id\":\"1279788420557942784\",\"cap-corr-seq\":\"0\"},\"Value\":\"2020-07-05T22:45:11.4330931+08:00\"}', '0', '2020-07-05 22:45:11', '2020-07-06 22:45:11', 'Succeeded');

-- ----------------------------
-- Table structure for EbsReceived
-- ----------------------------
DROP TABLE IF EXISTS `EbsReceived`;
CREATE TABLE `EbsReceived` (
  `Id` bigint(20) NOT NULL,
  `Version` varchar(20) DEFAULT NULL,
  `Name` varchar(400) NOT NULL,
  `Group` varchar(200) DEFAULT NULL,
  `Content` longtext DEFAULT NULL,
  `Retries` int(11) DEFAULT NULL,
  `Added` datetime NOT NULL,
  `ExpiresAt` datetime DEFAULT NULL,
  `StatusName` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ExpiresAt` (`ExpiresAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of EbsReceived
-- ----------------------------
INSERT INTO `EbsReceived` VALUES ('1279701741176778752', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279701738769248256\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 17:00:44 +08:00\",\"cap-corr-id\":\"1279701738769248256\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T17:00:44.8859228+08:00\"}', '0', '2020-07-05 17:00:45', '2020-07-06 17:00:45', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279719317601202176', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279719316275802112\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:10:35 +08:00\",\"cap-corr-id\":\"1279719316275802112\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T18:10:35.6928583+08:00\"}', '0', '2020-07-05 18:10:36', '2020-07-06 18:10:36', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279719779606224896', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279719777215471616\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:12:25 +08:00\",\"cap-corr-id\":\"1279719777215471616\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T18:12:25.5842836+08:00\"}', '0', '2020-07-05 18:12:26', '2020-07-06 18:12:26', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279720322830278656', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279719054475735040\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:09:33 +08:00\",\"cap-corr-id\":\"1279719054475735040\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T18:09:33.2695215+08:00\"}', '0', '2020-07-05 18:14:35', '2020-07-06 18:19:17', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279720501523353600', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279720499409424384\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:15:17 +08:00\",\"cap-corr-id\":\"1279720499409424384\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T18:15:17.7707678+08:00\"}', '0', '2020-07-05 18:15:18', '2020-07-06 18:15:57', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279730563641872384', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279730562803011584\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 18:55:17 +08:00\",\"cap-corr-id\":\"1279730562803011584\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T18:55:17.0734837+08:00\"}', '0', '2020-07-05 18:55:17', '2020-07-06 19:29:54', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279739306689093632', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279739305313361920\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 19:30:01 +08:00\",\"cap-corr-id\":\"1279739305313361920\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T19:30:01.4474742+08:00\"}', '0', '2020-07-05 19:30:01', '2020-07-06 19:30:01', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279780883497287680', 'v1', 'test.show.time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279780881211392000\",\"cap-msg-name\":\"test.show.time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 22:15:13 +08:00\",\"cap-corr-id\":\"1279780881211392000\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T22:15:13.9113101+08:00\"}', '0', '2020-07-05 22:15:14', '2020-07-06 22:15:14', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279781051656003584', 'v1', 'test.show.string', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279781049311387648\",\"cap-msg-name\":\"test.show.string\",\"cap-msg-type\":\"String\",\"cap-senttime\":\"2020/7/5 22:15:53 +08:00\",\"cap-corr-id\":\"1279781049311387648\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"hello world\"}', '0', '2020-07-05 22:15:54', '2020-07-06 22:15:54', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279784327350452224', 'v1', 'test_show_time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279784324154392576\",\"cap-msg-name\":\"test_show_time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 22:28:54 +08:00\",\"cap-corr-id\":\"1279784324154392576\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T22:28:54.7743127+08:00\"}', '0', '2020-07-05 22:28:55', '2020-07-06 22:28:55', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279784349441851392', 'v1', 'test_show_string', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279784348674293760\",\"cap-msg-name\":\"test_show_string\",\"cap-msg-type\":\"String\",\"cap-senttime\":\"2020/7/5 22:29:00 +08:00\",\"cap-corr-id\":\"1279784348674293760\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"hello world\"}', '0', '2020-07-05 22:29:00', '2020-07-06 22:29:00', 'Succeeded');
INSERT INTO `EbsReceived` VALUES ('1279788422797701120', 'v1', 'test_show_time', 'cap.queue.demoebus.v1', '{\"Headers\":{\"my.header.first\":\"first\",\"my.header.second\":\"second\",\"cap-msg-id\":\"1279788420557942784\",\"cap-msg-name\":\"test_show_time\",\"cap-msg-type\":\"DateTime\",\"cap-senttime\":\"2020/7/5 22:45:11 +08:00\",\"cap-corr-id\":\"1279788420557942784\",\"cap-corr-seq\":\"0\",\"cap-msg-group\":\"cap.queue.demoebus.v1\"},\"Value\":\"2020-07-05T22:45:11.4330931+08:00\"}', '0', '2020-07-05 22:45:11', '2020-07-06 22:45:49', 'Succeeded');

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
-- Records of Message
-- ----------------------------

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
-- Records of MessageSender
-- ----------------------------

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
-- Records of MessageTemplate
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
INSERT INTO `SysCfg` VALUES ('1', null, null, '23', '2020-07-15 19:50:37.000000', '应用名称', 'system.app.name', 'systemmanager', '0');
INSERT INTO `SysCfg` VALUES ('2', null, null, '1', '2020-07-14 01:00:32.000000', '系统默认上传文件路径', 'system.file.upload.path', '/data/web-flash/runtime/upload', '0');
INSERT INTO `SysCfg` VALUES ('3', null, null, '23', '2020-07-21 18:13:47.000000', '腾讯sms接口appid', 'api.tencent.sms.appid', '1400219425', '1');
INSERT INTO `SysCfg` VALUES ('4', null, null, '23', '2020-07-21 18:10:59.000000', '腾讯sms接口appkey', 'api.tencent.sms.appkey', '5f71ed5325f3b292946530a1773e997a', '1');
INSERT INTO `SysCfg` VALUES ('5', null, null, '1', '2020-07-14 00:59:41.000000', 'xxxx', 'api.tencent.sms.sign', '需要去申请咯', '1');
INSERT INTO `SysCfg` VALUES ('200728', '23', '2020-07-28 20:56:20.000000', '23', '2020-07-28 22:45:35.000000', '', 'cosnulurl', 'http://www.baidu.com', '1');
INSERT INTO `SysCfg` VALUES ('11596036584019', '23', '2020-07-29 23:29:44.030477', '23', '2020-07-29 23:29:47.363485', 'qq', 'qqq', 'qq', '1');
INSERT INTO `SysCfg` VALUES ('70644585327628288', '1', '2020-07-13 22:36:27.000000', '1', '2020-07-14 00:48:44.000000', 'xxxx', '111111111111111', '2222222222', '1');
INSERT INTO `SysCfg` VALUES ('70678390889385984', '1', '2020-07-14 00:50:41.000000', '1', '2020-07-14 00:58:29.000000', '132', '111', '133', '1');

-- ----------------------------
-- Table structure for SysDept
-- ----------------------------
DROP TABLE IF EXISTS `SysDept`;
CREATE TABLE `SysDept` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `FullName` varchar(255) DEFAULT NULL,
  `Num` int(11) DEFAULT NULL,
  `Pid` bigint(20) DEFAULT NULL,
  `Pids` varchar(255) DEFAULT NULL,
  `SimpleName` varchar(255) DEFAULT NULL,
  `Tips` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=51595946012039 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='部门';

-- ----------------------------
-- Records of SysDept
-- ----------------------------
INSERT INTO `SysDept` VALUES ('24', null, null, '23', '2020-07-28 17:26:00.000000', '总公司', '1', '0', '[0],', '总公司', '', null);
INSERT INTO `SysDept` VALUES ('25', null, null, '23', '2020-08-02 20:54:19.992765', '开发部', '2', '24', '[0],[24],', '开发部', '', null);
INSERT INTO `SysDept` VALUES ('26', null, null, null, null, '运营部', '3', '24', '[0],[24],', '运营部', '', null);
INSERT INTO `SysDept` VALUES ('27', null, null, null, null, '战略部', '4', '24', '[0],[24],', '战略部', '', null);
INSERT INTO `SysDept` VALUES ('29', '1', '2020-07-12 15:39:15.000000', '1', '2020-07-12 22:53:10.000000', 'vue组', '1', '25', '[0],[24],[25],', 'vue组', null, null);
INSERT INTO `SysDept` VALUES ('30', '1', '2020-07-12 15:40:21.000000', null, null, 'angular组', '2', '25', '[0],[24],[25],', 'angular组', null, null);
INSERT INTO `SysDept` VALUES ('31', '1', '2020-07-12 15:42:06.000000', null, null, 'taobao组', '1', '26', '[0],[24],[26],', 'taobao组', null, null);
INSERT INTO `SysDept` VALUES ('32', '1', '2020-07-12 15:42:50.000000', null, null, '京东组', '2', '26', '[0],[24],[26],', '京东组', null, null);

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
) ENGINE=InnoDB AUTO_INCREMENT=31596374135528 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='字典';

-- ----------------------------
-- Records of SysDict
-- ----------------------------
INSERT INTO `SysDict` VALUES ('16', null, null, '23', '2020-08-02 21:12:55.869879', '状态', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('29', null, null, '23', '2020-08-02 21:15:35.406795', '性别', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('35', null, null, null, null, '账号状态', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('36', null, null, null, null, '启用', '1', '35', null, '0');
INSERT INTO `SysDict` VALUES ('37', null, null, null, null, '冻结', '2', '35', null, '0');
INSERT INTO `SysDict` VALUES ('38', null, null, null, null, '已删除', '3', '35', null, '0');
INSERT INTO `SysDict` VALUES ('53', null, null, null, null, '证件类型', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('54', null, null, null, null, '身份证', '1', '53', null, '0');
INSERT INTO `SysDict` VALUES ('55', null, null, null, null, '护照', '2', '53', null, '0');
INSERT INTO `SysDict` VALUES ('68', '1', '2019-01-13 14:18:21.000000', '23', '2020-08-02 21:12:30.501754', '是否', '0', '0', null, '0');
INSERT INTO `SysDict` VALUES ('31595945861169', '23', '2020-07-28 22:17:41.000000', null, null, 'aa', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31595945883502', '23', '2020-07-28 22:18:03.000000', null, null, 'aaaa', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31595946063336', '23', '2020-07-28 22:21:03.000000', null, null, 'ss', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31595946768610', '23', '2020-07-28 22:32:48.000000', null, null, '杀杀杀', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31595946768613', '23', '2020-07-28 22:32:48.000000', null, null, 'aaa', 'a', '31595946768610', null, '1');
INSERT INTO `SysDict` VALUES ('31595947022431', '23', '2020-07-28 22:37:02.000000', null, null, 'aa', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31595947022435', '23', '2020-07-28 22:37:02.000000', null, null, 'a', 'a', '31595947022431', null, '1');
INSERT INTO `SysDict` VALUES ('31595947031522', '23', '2020-07-28 22:37:11.000000', null, null, 'aa', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31595947031523', '23', '2020-07-28 22:37:11.000000', null, null, 'a', 'a', '31595947031522', null, '1');
INSERT INTO `SysDict` VALUES ('31595947058097', '23', '2020-07-28 22:37:38.000000', null, null, 'aa', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31595947058098', '23', '2020-07-28 22:37:38.000000', null, null, 'a', 'a', '31595947058097', null, '1');
INSERT INTO `SysDict` VALUES ('31595947058100', '23', '2020-07-28 22:37:38.000000', null, null, 'b', 'b', '31595947058097', null, '1');
INSERT INTO `SysDict` VALUES ('31595947058101', '23', '2020-07-28 22:37:38.000000', null, null, 'c', 'c', '31595947058097', null, '1');
INSERT INTO `SysDict` VALUES ('31595947058102', '23', '2020-07-28 22:37:38.000000', null, null, 'd', 'd', '31595947058097', null, '1');
INSERT INTO `SysDict` VALUES ('31595947058103', '23', '2020-07-28 22:37:38.000000', null, null, 'k', 'k', '31595947058097', null, '1');
INSERT INTO `SysDict` VALUES ('31596036561077', '23', '2020-07-29 23:29:21.099633', null, null, 'qq', '0', '0', null, '1');
INSERT INTO `SysDict` VALUES ('31596036561081', '23', '2020-07-29 23:29:21.099632', null, null, 'q', 'q', '31596036561077', null, '1');
INSERT INTO `SysDict` VALUES ('31596373950785', '23', '2020-08-02 21:12:30.790383', null, null, '0', '否', '68', null, '0');
INSERT INTO `SysDict` VALUES ('31596373950786', '23', '2020-08-02 21:12:30.790385', null, null, '1', '是', '68', null, '0');
INSERT INTO `SysDict` VALUES ('31596373976464', '23', '2020-08-02 21:12:56.466051', null, null, '显示', '1', '16', null, '0');
INSERT INTO `SysDict` VALUES ('31596374135526', '23', '2020-08-02 21:15:35.528992', null, null, '男', '1', '29', null, '0');
INSERT INTO `SysDict` VALUES ('31596374135527', '23', '2020-08-02 21:15:35.528994', null, null, '女', '3', '29', null, '0');

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
-- Records of SysFileInfo
-- ----------------------------
INSERT INTO `SysFileInfo` VALUES ('1', '1', '2019-03-18 10:34:34.000000', '1', '2019-03-18 10:34:34.000000', 'banner1.png', '7e9ebc08-b194-4f85-8997-d97ccb0d2c2d.png');
INSERT INTO `SysFileInfo` VALUES ('2', '1', '2019-03-18 10:54:04.000000', '1', '2019-03-18 10:54:04.000000', 'banner2.png', '756b9ca8-562f-4bf5-a577-190dcdd25c29.png');
INSERT INTO `SysFileInfo` VALUES ('3', '1', '2019-03-18 20:09:59.000000', '1', '2019-03-18 20:09:59.000000', 'offcial_site.png', 'b0304e2b-0ee3-4966-ac9f-a075b13d4af6.png');
INSERT INTO `SysFileInfo` VALUES ('4', '1', '2019-03-18 20:10:18.000000', '1', '2019-03-18 20:10:18.000000', 'bbs.png', '67486aa5-500c-4993-87ad-7e1fbc90ac1a.png');
INSERT INTO `SysFileInfo` VALUES ('5', '1', '2019-03-18 20:20:14.000000', '1', '2019-03-18 20:20:14.000000', 'product.png', '1f2b05e0-403a-41e0-94a2-465f0c986b78.png');
INSERT INTO `SysFileInfo` VALUES ('6', '1', '2019-03-18 20:22:09.000000', '1', '2019-03-18 20:22:09.000000', 'profile.jpg', '40ead888-14d1-4e9f-abb3-5bfb056a966a.jpg');
INSERT INTO `SysFileInfo` VALUES ('7', '1', '2019-03-20 09:05:54.000000', '1', '2019-03-20 09:05:54.000000', '2303938_1453211.png', '87b037da-b517-4007-a66e-ba7cc8cfd6ea.png');
INSERT INTO `SysFileInfo` VALUES ('8', null, null, null, null, 'login.png', '26835cc4-059e-4900-aff5-a41f2ea6a61d.png');
INSERT INTO `SysFileInfo` VALUES ('9', null, null, null, null, 'login.png', '7ec7553b-7c9e-44d9-b9c2-3d89b11cf842.png');
INSERT INTO `SysFileInfo` VALUES ('10', null, null, null, null, 'login.png', '357c4aad-19fd-4600-9fb6-e62aafa3df25.png');
INSERT INTO `SysFileInfo` VALUES ('11', null, null, null, null, 'index.png', '55dd582b-033e-440d-8e8d-c8d39d01f1bb.png');
INSERT INTO `SysFileInfo` VALUES ('12', null, null, null, null, 'login.png', '70507c07-e8bc-492f-9f0a-00bf1c23e329.png');
INSERT INTO `SysFileInfo` VALUES ('13', null, null, null, null, 'index.png', 'cd539518-d15a-4cda-a19f-251169f5d1a4.png');
INSERT INTO `SysFileInfo` VALUES ('14', null, null, null, null, 'login.png', '194c8a38-be94-483c-8875-3c62a857ead7.png');
INSERT INTO `SysFileInfo` VALUES ('15', null, null, null, null, 'index.png', '6a6cb215-d0a7-4574-a45e-5fa04dcfdf90.png');
INSERT INTO `SysFileInfo` VALUES ('16', '1', '2019-03-21 19:37:50.000000', null, null, '测试文档.doc', 'd9d77815-496f-475b-a0f8-1d6dcb86e6ab.doc');
INSERT INTO `SysFileInfo` VALUES ('17', '1', '2019-04-28 00:34:09.000000', null, null, '首页.png', 'd5aba978-f8af-45c5-b079-673decfbdf26.png');
INSERT INTO `SysFileInfo` VALUES ('18', '1', '2019-04-28 00:34:34.000000', null, null, '资讯.png', '7e07520d-5b73-4712-800b-16f88d133db2.png');
INSERT INTO `SysFileInfo` VALUES ('19', '1', '2019-04-28 00:38:32.000000', null, null, '产品服务.png', '99214651-8cb8-4488-b572-12c6aa21f30a.png');
INSERT INTO `SysFileInfo` VALUES ('20', '1', '2019-04-28 00:39:09.000000', null, null, '67486aa5-500c-4993-87ad-7e1fbc90ac1a.png', '31fdc83e-7688-41f5-b153-b6816d5dfb06.png');
INSERT INTO `SysFileInfo` VALUES ('21', '1', '2019-04-28 00:39:22.000000', null, null, '67486aa5-500c-4993-87ad-7e1fbc90ac1a.png', 'ffaf0563-3115-477b-b31d-47a4e80a75eb.png');
INSERT INTO `SysFileInfo` VALUES ('22', '1', '2019-04-28 00:39:47.000000', null, null, '7e07520d-5b73-4712-800b-16f88d133db2.png', '8928e5d4-933a-4953-9507-f60b78e3ccee.png');
INSERT INTO `SysFileInfo` VALUES ('23', null, '2019-04-28 17:34:31.000000', null, null, '756b9ca8-562f-4bf5-a577-190dcdd25c29.png', '7d64ba36-adc4-4982-9ec2-8c68db68861b.png');
INSERT INTO `SysFileInfo` VALUES ('24', null, '2019-04-28 17:39:43.000000', null, null, 'timg.jpg', '6483eb26-775c-4fe2-81bf-8dd49ac9b6b1.jpg');
INSERT INTO `SysFileInfo` VALUES ('25', '1', '2019-05-05 15:36:54.000000', null, null, 'timg.jpg', '7fe918a2-c59a-4d17-ad77-f65dd4e163bf.jpg');

-- ----------------------------
-- Table structure for SysLoginLog
-- ----------------------------
DROP TABLE IF EXISTS `SysLoginLog`;
CREATE TABLE `SysLoginLog` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateTime` datetime(6) DEFAULT NULL,
  `Message` varchar(255) DEFAULT NULL,
  `Succeed` tinyint(1) NOT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  `Account` varchar(32) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Device` varchar(20) CHARACTER SET utf8mb4 DEFAULT NULL,
  `RemoteIpAddress` varchar(22) CHARACTER SET utf8mb4 DEFAULT NULL,
  `UserName` varchar(64) CHARACTER SET utf8mb4 DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=77879750785372161 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='登录日志';

-- ----------------------------
-- Records of SysLoginLog
-- ----------------------------
INSERT INTO `SysLoginLog` VALUES ('1', '2020-07-18 00:01:58.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('71', '2019-05-10 13:17:43.000000', null, '0', '1', null, null, null, null);
INSERT INTO `SysLoginLog` VALUES ('72', '2019-05-12 13:36:56.000000', null, '0', '1', null, null, null, null);
INSERT INTO `SysLoginLog` VALUES ('72117073953820672', '2020-07-18 00:07:30.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117089967673344', '2020-07-18 00:07:34.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117100654759936', '2020-07-18 00:07:36.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117108456165376', '2020-07-18 00:07:38.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117115385155584', '2020-07-18 00:07:40.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117122167345152', '2020-07-18 00:07:41.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117128798539776', '2020-07-18 00:07:43.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117135144521728', '2020-07-18 00:07:44.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117141612138496', '2020-07-18 00:07:46.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72117147563855872', '2020-07-18 00:07:47.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72118381511315456', '2020-07-18 00:12:42.000000', '{\"StatusCode\":0,\"Error\":\"账号已锁定\"}', '0', '21', 'alphago', 'web', null, '风口旁的猪');
INSERT INTO `SysLoginLog` VALUES ('72118746516426752', '2020-07-18 00:14:09.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72119803200016384', '2020-07-18 00:18:21.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72119905448759296', '2020-07-18 00:18:45.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72119945672134656', '2020-07-18 00:18:54.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72119990211448832', '2020-07-18 00:19:05.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72120019005345792', '2020-07-18 00:19:12.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72120053025345536', '2020-07-18 00:19:20.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72120079126499328', '2020-07-18 00:19:26.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72120105533837312', '2020-07-18 00:19:33.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72120136009650176', '2020-07-18 00:19:40.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72121969318957056', '2020-07-18 00:26:57.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72122963842961408', '2020-07-18 00:30:54.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72123481248108544', '2020-07-18 00:32:57.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72123570431594496', '2020-07-18 00:33:19.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72123819401285632', '2020-07-18 00:34:18.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72124885039714304', '2020-07-18 00:38:32.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72124972298014720', '2020-07-18 00:38:53.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72125175931473920', '2020-07-18 00:39:41.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72125219212496896', '2020-07-18 00:39:52.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72125561018912768', '2020-07-18 00:41:13.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72125631676157952', '2020-07-18 00:41:30.000000', '{\"StatusCode\":0,\"Error\":\"用户名或密码错误\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72336118976221184', '2020-07-18 14:37:54.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72362298664161280', '2020-07-18 16:21:56.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72363740321943552', '2020-07-18 16:27:40.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72366547632525312', '2020-07-18 16:38:49.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72390942182739968', '2020-07-18 18:15:45.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72394072140156928', '2020-07-18 18:28:11.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72394136824713216', '2020-07-18 18:28:27.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72394273223479296', '2020-07-18 18:28:59.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72394440001589248', '2020-07-18 18:29:39.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72404937765687296', '2020-07-18 19:11:22.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72410576487321600', '2020-07-18 19:33:46.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72411131729285120', '2020-07-18 19:35:59.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72411199341465600', '2020-07-18 19:36:15.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72424385486655488', '2020-07-18 20:28:39.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72427044713140224', '2020-07-18 20:39:13.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72429240859103232', '2020-07-18 20:47:56.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72430595870953472', '2020-07-18 20:53:19.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72432473316921344', '2020-07-18 21:00:47.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72433536568791040', '2020-07-18 21:05:00.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72434322623303680', '2020-07-18 21:08:08.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72439659195666432', '2020-07-18 21:29:20.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72443497868824576', '2020-07-18 21:44:35.000000', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72443574951743488', '2020-07-18 21:44:54.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72450967680978944', '2020-07-18 22:14:16.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72451344333672448', '2020-07-18 22:15:46.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72451471437860864', '2020-07-18 22:16:16.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72451679278206976', '2020-07-18 22:17:06.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72451726514458624', '2020-07-18 22:17:17.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72452035156512768', '2020-07-18 22:18:31.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72458020143108096', '2020-07-18 22:42:18.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72726109556445184', '2020-07-19 16:27:35.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72736049910124544', '2020-07-19 17:07:05.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72738369611894784', '2020-07-19 17:16:18.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72740070494441472', '2020-07-19 17:23:04.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72740091184943104', '2020-07-19 17:23:09.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72740340234326016', '2020-07-19 17:24:08.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72740936534331392', '2020-07-19 17:26:30.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72740991098032128', '2020-07-19 17:26:43.000000', '登录成功', '1', '1', 'admin', 'web', '::1:54402', '管理员');
INSERT INTO `SysLoginLog` VALUES ('72742884457189376', '2020-07-19 17:34:15.000000', '{\"StatusCode\":404,\"Error\":\"未分配任务角色，请联系管理员\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72742982671011840', '2020-07-19 17:34:38.000000', '{\"StatusCode\":404,\"Error\":\"未分配任务角色，请联系管理员\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('72743179442589696', '2020-07-19 17:35:25.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72744599503900672', '2020-07-19 17:41:04.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72752634972475392', '2020-07-19 18:12:59.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72757027172126720', '2020-07-19 18:30:27.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72758408557760512', '2020-07-19 18:35:56.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72765343248027648', '2020-07-19 19:03:29.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72765706084683776', '2020-07-19 19:04:56.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72779111189319680', '2020-07-19 19:58:12.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72780101095395328', '2020-07-19 20:02:08.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72799700268486656', '2020-07-19 21:20:01.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72800219275857920', '2020-07-19 21:22:04.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72801978819940352', '2020-07-19 21:29:04.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72802365044035584', '2020-07-19 21:30:36.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72803710773891072', '2020-07-19 21:35:57.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72824213777551360', '2020-07-19 22:57:25.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72824223751606272', '2020-07-19 22:57:27.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72824458192228352', '2020-07-19 22:58:23.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72824848858091520', '2020-07-19 22:59:56.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72825002692579328', '2020-07-19 23:00:33.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72825053678538752', '2020-07-19 23:00:45.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72825153641385984', '2020-07-19 23:01:09.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72827277540462592', '2020-07-19 23:09:36.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72829080227155968', '2020-07-19 23:16:45.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72829990793777152', '2020-07-19 23:20:22.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72830609197764608', '2020-07-19 23:22:50.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72831031610314752', '2020-07-19 23:24:31.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72831211629842432', '2020-07-19 23:25:13.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72831250322296832', '2020-07-19 23:25:23.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72831288414965760', '2020-07-19 23:25:32.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72831486222536704', '2020-07-19 23:26:19.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72836219351994368', '2020-07-19 23:45:07.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72836416735940608', '2020-07-19 23:45:54.000000', '登录成功', '1', '23', 'alpha2008', 'web', '::1:62765', 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72838527104192512', '2020-07-19 23:54:18.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72862497740296192', '2020-07-20 01:29:33.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('72862704821473280', '2020-07-20 01:30:22.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73053557351387136', '2020-07-20 14:08:45.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73071873348472832', '2020-07-20 15:21:32.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73072158531784704', '2020-07-20 15:22:40.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73073520367767552', '2020-07-20 15:28:04.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73073719559458816', '2020-07-20 15:28:52.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73074727513624576', '2020-07-20 15:32:52.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('73076231674925056', '2020-07-20 15:38:51.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73141549071994880', '2020-07-20 19:58:24.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73448905743929344', '2020-07-21 16:19:43.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('73464650720743424', '2020-07-21 17:22:17.000000', '登录成功', '1', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('73468651218538496', '2020-07-21 17:38:11.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73486841180983296', '2020-07-21 18:50:28.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73488996558311424', '2020-07-21 18:59:02.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73564969685356544', '2020-07-22 00:00:55.000000', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('73565064518569984', '2020-07-22 00:01:18.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('73565380353855488', '2020-07-22 00:02:33.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('74239896595861504', '2020-07-23 20:42:50.000000', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('74239951461552128', '2020-07-23 20:43:03.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('74251007516872704', '2020-07-23 21:26:59.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('74510880712298496', '2020-07-24 14:39:38.000000', '{\"StatusCode\":404,\"Error\":\"用户名或密码错误\"}', '0', '15', 'alpha', 'web', null, '王大户');
INSERT INTO `SysLoginLog` VALUES ('74511113923989504', '2020-07-24 14:40:33.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('74513990335729664', '2020-07-24 14:51:59.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('74516539210076160', '2020-07-24 15:02:07.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('74520970811019264', '2020-07-24 15:19:43.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('74614182502338560', '2020-07-24 13:30:07.000000', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('74614239972691968', '2020-07-24 13:30:21.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75182907190284288', '2020-07-26 11:10:01.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75597725579743232', '2020-07-27 14:38:22.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75599391267885056', '2020-07-27 14:44:59.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75613592447225856', '2020-07-27 07:41:25.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75656058030395392', '2020-07-27 10:30:09.000000', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('75710889248034816', '2020-07-27 14:08:02.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75908639403151360', '2020-07-28 03:13:49.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75913081850564608', '2020-07-28 03:31:29.000000', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('75913138679189504', '2020-07-28 03:31:42.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75915495957729280', '2020-07-28 03:41:04.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75937938847961088', '2020-07-28 05:10:15.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75940465450553344', '2020-07-28 05:20:17.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('75998350775816192', '2020-07-28 17:10:18.000000', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('75998434049527808', '2020-07-28 17:10:38.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76000027046187008', '2020-07-28 17:16:58.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76054842312888320', '2020-07-28 20:54:47.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76075613789753344', '2020-07-28 22:17:19.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76079443256414208', '2020-07-28 22:32:32.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76080515421179904', '2020-07-28 22:36:48.000000', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76303000351674368', '2020-07-29 13:20:52.925649', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76372431467581440', '2020-07-29 09:56:46.593654', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76373087083433984', '2020-07-29 09:59:22.904291', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76455819809525760', '2020-07-29 23:28:07.924098', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76674750390341632', '2020-07-30 05:58:05.042172', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76681030198759424', '2020-07-30 06:23:02.264731', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76681724351877120', '2020-07-30 06:25:47.763806', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76682659056717824', '2020-07-30 06:29:30.615074', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76683034501451776', '2020-07-30 06:31:00.127469', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76683615332864000', '2020-07-30 06:33:18.608567', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76683624291897344', '2020-07-30 06:33:20.744639', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76683709641789440', '2020-07-30 06:33:41.093186', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76683739157106688', '2020-07-30 06:33:48.130754', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76686727686787072', '2020-07-30 06:45:40.651400', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76688871227789312', '2020-07-30 06:54:11.711630', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690396343832576', '2020-07-30 07:00:15.327443', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690557992308736', '2020-07-30 07:00:53.867040', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690563486846976', '2020-07-30 07:00:55.177542', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690566791958528', '2020-07-30 07:00:55.965239', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690568314490880', '2020-07-30 07:00:56.328682', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690711818407936', '2020-07-30 07:01:30.542095', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690718021783552', '2020-07-30 07:01:32.021812', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690721041682432', '2020-07-30 07:01:32.741635', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690724363571200', '2020-07-30 07:01:33.533016', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690727408635904', '2020-07-30 07:01:34.259809', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690730726330368', '2020-07-30 07:01:35.050842', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690733763006464', '2020-07-30 07:01:35.774257', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690737223307264', '2020-07-30 07:01:36.599756', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690743300853760', '2020-07-30 07:01:38.048586', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76690750544416768', '2020-07-30 07:01:39.775167', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76692644067151872', '2020-07-30 07:09:11.226784', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76693190299750400', '2020-07-30 07:11:21.458528', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76693704647249920', '2020-07-30 07:13:24.088465', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76695072170708992', '2020-07-30 07:18:50.131390', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76766664238174208', '2020-07-30 20:03:19.010963', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76767804132560896', '2020-07-30 20:07:50.783016', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76768332468064256', '2020-07-30 20:09:56.747957', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76769861056991232', '2020-07-30 20:16:01.192810', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76770528525946880', '2020-07-30 20:18:40.328604', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76773903581712384', '2020-07-30 20:32:05.005519', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76774193445867520', '2020-07-30 20:33:14.113898', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76800474065539072', '2020-07-30 22:17:39.905776', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76800997380460544', '2020-07-30 22:19:44.670882', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76801306936872960', '2020-07-30 22:20:58.475076', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76802365256241152', '2020-07-30 22:25:10.798045', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76804132723363840', '2020-07-30 22:32:12.194998', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76806535673679872', '2020-07-30 22:41:45.103043', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76809157004627968', '2020-07-30 22:52:10.075981', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76813738132508672', '2020-07-30 15:10:22.301372', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76819293848866816', '2020-07-30 15:32:26.887033', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76826384403664896', '2020-07-31 00:00:37.407913', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76830969981702144', '2020-07-30 16:18:50.694457', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76831338476474368', '2020-07-30 16:20:18.550816', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76831482441764864', '2020-07-30 16:20:52.874741', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('76831503375536128', '2020-07-30 16:20:57.865540', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76831656052396032', '2020-07-30 16:21:34.266709', '{\"StatusCode\":404,\"Error\":\"用户名或密码错误\"}', '0', '22', 'alpha2007', 'web', null, 'alpha');
INSERT INTO `SysLoginLog` VALUES ('76831670543716352', '2020-07-30 16:21:37.721250', '{\"StatusCode\":404,\"Error\":\"用户名或密码错误\"}', '0', '22', 'alpha2007', 'web', null, 'alpha');
INSERT INTO `SysLoginLog` VALUES ('76831698582638592', '2020-07-30 16:21:44.406280', '{\"StatusCode\":404,\"Error\":\"用户名或密码错误\"}', '0', '22', 'alpha2007', 'web', null, 'alpha');
INSERT INTO `SysLoginLog` VALUES ('76831746796163072', '2020-07-30 16:21:55.901458', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76834529872056320', '2020-07-31 00:32:59.438493', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('76952860151123968', '2020-07-31 08:23:11.575944', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '1', 'admin', 'web', null, '管理员');
INSERT INTO `SysLoginLog` VALUES ('77575172273606656', '2020-08-02 01:36:02.347690', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77800825023500288', '2020-08-02 16:32:42.155694', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77808665914118144', '2020-08-02 17:03:51.570121', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77813425400582144', '2020-08-02 17:22:46.320053', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77854854898388992', '2020-08-02 20:07:23.882466', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77855924097781760', '2020-08-02 20:11:38.799168', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77857491739873280', '2020-08-02 20:17:52.554267', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77861731518517248', '2020-08-02 20:34:43.395937', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77866382770966528', '2020-08-02 20:53:12.347034', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77869528549298176', '2020-08-02 21:05:42.352815', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77871018651291648', '2020-08-02 21:11:37.621061', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77871764344016896', '2020-08-02 21:14:35.407938', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77874415936868352', '2020-08-02 21:25:07.598290', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77874451584258048', '2020-08-02 21:25:16.095561', '{\"StatusCode\":429,\"Error\":\"账号已锁定\"}', '0', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77875002770329600', '2020-08-02 21:27:27.508792', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77875610495619072', '2020-08-02 21:29:52.401986', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77876090982502400', '2020-08-02 21:31:46.958105', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77877104615755776', '2020-08-02 21:35:48.627445', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77877172609617920', '2020-08-02 21:36:04.838518', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77879585311690752', '2020-08-02 21:45:40.072290', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');
INSERT INTO `SysLoginLog` VALUES ('77879750785372160', '2020-08-02 21:46:19.523534', '登录成功', '1', '23', 'alpha2008', 'web', null, 'alpha2008');

-- ----------------------------
-- Table structure for SysMenu
-- ----------------------------
DROP TABLE IF EXISTS `SysMenu`;
CREATE TABLE `SysMenu` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Code` varchar(32) NOT NULL COMMENT '编号',
  `Component` varchar(64) DEFAULT NULL COMMENT '組件配置',
  `Hidden` bit(1) DEFAULT NULL COMMENT '是否隐藏',
  `Icon` varchar(32) DEFAULT NULL COMMENT '图标',
  `IsMenu` bit(1) NOT NULL COMMENT '是否是菜单1:菜单,0:按钮',
  `IsOpen` bit(1) DEFAULT NULL COMMENT '是否默认打开1:是,0:否',
  `Levels` int(11) NOT NULL COMMENT '级别',
  `Name` varchar(64) NOT NULL COMMENT '名称',
  `Num` int(11) NOT NULL COMMENT '顺序',
  `PCode` varchar(64) NOT NULL COMMENT '父菜单编号',
  `PCodes` varchar(128) DEFAULT NULL COMMENT '递归父级菜单编号',
  `Status` bit(1) NOT NULL COMMENT '状态1:启用,0:禁用',
  `Tips` varchar(32) DEFAULT NULL COMMENT '鼠标悬停提示信息',
  `Url` varchar(32) DEFAULT NULL COMMENT '链接',
  PRIMARY KEY (`ID`) USING BTREE,
  UNIQUE KEY `UK_s37unj3gh67ujhk83lqva8i1t` (`Code`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=61595946041629 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='菜单';

-- ----------------------------
-- Records of SysMenu
-- ----------------------------
INSERT INTO `SysMenu` VALUES ('1', null, null, '23', '2020-07-15 19:50:17.000000', 'system', 'layout', '\0', 'system', '', null, '1', '系统管理', '1', '0', '[0],', '', null, '/system');
INSERT INTO `SysMenu` VALUES ('2', '1', '2019-07-31 22:04:30.000000', '1', '2019-03-11 22:25:38.000000', 'cms', 'layout', '\0', 'documentation', '', null, '1', 'CMS管理', '3', '0', '[0],', '', null, '/cms');
INSERT INTO `SysMenu` VALUES ('3', null, null, '23', '2020-07-15 09:37:59.000000', 'operationMgr', 'layout', '\0', 'operation', '', null, '1', '运维管理', '2', '0', '[0],', '\0', null, '/optionMgr');
INSERT INTO `SysMenu` VALUES ('4', '1', '2019-07-31 22:04:30.000000', '1', '2019-04-16 18:59:15.000000', 'user', 'views/system/user/index', '\0', 'user', '', null, '2', '用户管理', '1', 'system', '[0],[system],', '', null, '/user');
INSERT INTO `SysMenu` VALUES ('5', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userAdd', null, '\0', null, '\0', null, '3', '添加用户', '1', 'user', '[0],[system],[user],', '', null, '/user/add');
INSERT INTO `SysMenu` VALUES ('6', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userEdit', null, '\0', null, '\0', null, '3', '修改用户', '2', 'user', '[0],[system],[user],', '', null, '/user/edit');
INSERT INTO `SysMenu` VALUES ('7', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userDelete', null, '\0', null, '\0', '\0', '3', '删除用户', '3', 'user', '[0],[system],[user],', '', null, '/user/delete');
INSERT INTO `SysMenu` VALUES ('8', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userReset', null, '\0', null, '\0', '\0', '3', '重置密码', '4', 'user', '[0],[system],[user],', '', null, '/user/reset');
INSERT INTO `SysMenu` VALUES ('9', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userFreeze', null, '\0', null, '\0', '\0', '3', '冻结用户', '5', 'user', '[0],[system],[user],', '', null, '/user/freeze');
INSERT INTO `SysMenu` VALUES ('10', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userUnfreeze', null, '\0', null, '\0', '\0', '3', '解除冻结用户', '6', 'user', '[0],[system],[user],', '', null, '/user/unfreeze');
INSERT INTO `SysMenu` VALUES ('11', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userSetRole', null, '\0', null, '\0', '\0', '3', '分配角色', '7', 'user', '[0],[system],[user],', '', null, '/user/setRole');
INSERT INTO `SysMenu` VALUES ('12', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'role', 'views/system/role/index', '\0', 'peoples', '', '\0', '2', '角色管理', '2', 'system', '[0],[system],', '', null, '/role');
INSERT INTO `SysMenu` VALUES ('13', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'roleAdd', null, '\0', null, '\0', '\0', '3', '添加角色', '1', 'role', '[0],[system],[role],', '', null, '/role/add');
INSERT INTO `SysMenu` VALUES ('14', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'roleEdit', null, '\0', null, '\0', '\0', '3', '修改角色', '2', 'role', '[0],[system],[role],', '', null, '/role/edit');
INSERT INTO `SysMenu` VALUES ('15', null, null, '23', '2020-07-19 17:19:55.000000', 'roleDelete', null, '\0', null, '\0', null, '3', '删除角色', '3', 'role', '[0],[system],[role]', '', null, '/role/delete');
INSERT INTO `SysMenu` VALUES ('16', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'roleSetAuthority', null, '\0', null, '\0', '\0', '3', '配置权限', '4', 'role', '[0],[system],[role],', '', null, '/role/setAuthority');
INSERT INTO `SysMenu` VALUES ('17', null, null, '1', '2020-07-12 23:41:14.000000', 'menu', 'views/system/menu/index', '\0', 'menu', '', null, '2', '菜单管理', '3', 'system', '[0],[system],', '', null, '/menu');
INSERT INTO `SysMenu` VALUES ('18', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'menuAdd', null, '\0', null, '\0', '\0', '3', '添加菜单', '1', 'menu', '[0],[system],[menu],', '', null, '/menu/add');
INSERT INTO `SysMenu` VALUES ('19', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'menuEdit', null, '\0', null, '\0', '\0', '3', '修改菜单', '2', 'menu', '[0],[system],[menu],', '', null, '/menu/edit');
INSERT INTO `SysMenu` VALUES ('20', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'menuDelete', null, '\0', null, '\0', '\0', '3', '删除菜单', '3', 'menu', '[0],[system],[menu],', '', null, '/menu/remove');
INSERT INTO `SysMenu` VALUES ('21', null, null, '1', '2020-07-12 23:41:24.000000', 'dept', 'views/system/dept/index', '\0', 'dept', '', null, '2', '部门管理', '4', 'system', '[0],[system],', '', null, '/dept');
INSERT INTO `SysMenu` VALUES ('22', null, null, '1', '2020-07-13 00:05:37.000000', 'dict', 'views/system/dict/index', '\0', 'dict', '', null, '2', '字典管理', '5', 'system', '[0],[system],', '', null, '/dict');
INSERT INTO `SysMenu` VALUES ('23', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptEdit', null, '\0', null, '\0', null, '3', '修改部门', '1', 'dept', '[0],[system],[dept],', '', null, '/dept/update');
INSERT INTO `SysMenu` VALUES ('24', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptDelete', null, '\0', null, '\0', null, '3', '删除部门', '1', 'dept', '[0],[system],[dept],', '', null, '/dept/delete');
INSERT INTO `SysMenu` VALUES ('25', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictAdd', null, '\0', null, '\0', null, '3', '添加字典', '1', 'dict', '[0],[system],[dict],', '', null, '/dict/add');
INSERT INTO `SysMenu` VALUES ('26', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictEdit', null, '\0', null, '\0', null, '3', '修改字典', '1', 'dict', '[0],[system],[dict],', '', null, '/dict/update');
INSERT INTO `SysMenu` VALUES ('27', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictDelete', null, '\0', null, '\0', null, '3', '删除字典', '1', 'dict', '[0],[system],[dict],', '', null, '/dict/delete');
INSERT INTO `SysMenu` VALUES ('28', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptList', null, '\0', null, '\0', null, '3', '部门列表', '5', 'dept', '[0],[system],[dept],', '', null, '/dept/list');
INSERT INTO `SysMenu` VALUES ('29', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptDetail', null, '\0', null, '\0', null, '3', '部门详情', '6', 'dept', '[0],[system],[dept],', '', null, '/dept/detail');
INSERT INTO `SysMenu` VALUES ('30', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictList', null, '\0', null, '\0', null, '3', '字典列表', '5', 'dict', '[0],[system],[dict],', '', null, '/dict/list');
INSERT INTO `SysMenu` VALUES ('31', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictDetail', null, '\0', null, '\0', null, '3', '字典详情', '6', 'dict', '[0],[system],[dict],', '', null, '/dict/detail');
INSERT INTO `SysMenu` VALUES ('32', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptAdd', null, '\0', null, '\0', null, '3', '添加部门', '1', 'dept', '[0],[system],[dept],', '', null, '/dept/add');
INSERT INTO `SysMenu` VALUES ('33', null, null, '23', '2020-07-18 22:46:57.000000', 'cfg', 'views/system/cfg/index', '\0', 'cfg', '', null, '2', '参数管理', '6', 'system', '[0],[system]', '', null, '/cfg');
INSERT INTO `SysMenu` VALUES ('34', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'cfgAdd', null, '\0', null, '\0', null, '3', '添加系统参数', '1', 'cfg', '[0],[system],[cfg],', '', null, '/cfg/add');
INSERT INTO `SysMenu` VALUES ('35', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'cfgEdit', null, '\0', null, '\0', null, '3', '修改系统参数', '2', 'cfg', '[0],[system],[cfg],', '', null, '/cfg/update');
INSERT INTO `SysMenu` VALUES ('36', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'cfgDelete', null, '\0', null, '\0', null, '3', '删除系统参数', '3', 'cfg', '[0],[system],[cfg],', '', null, '/cfg/delete');
INSERT INTO `SysMenu` VALUES ('37', null, null, '23', '2020-07-18 22:47:08.000000', 'task', 'views/system/task/index', '\0', 'task', '', null, '2', '任务管理', '7', 'system', '[0],[system]', '', null, '/task');
INSERT INTO `SysMenu` VALUES ('38', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'taskAdd', null, '\0', null, '\0', null, '3', '添加任务', '1', 'task', '[0],[system],[task],', '', null, '/task/add');
INSERT INTO `SysMenu` VALUES ('39', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'taskEdit', null, '\0', null, '\0', null, '3', '修改任务', '2', 'task', '[0],[system],[task],', '', null, '/task/update');
INSERT INTO `SysMenu` VALUES ('40', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'taskDelete', null, '\0', null, '\0', null, '3', '删除任务', '3', 'task', '[0],[system],[task],', '', null, '/task/delete');
INSERT INTO `SysMenu` VALUES ('41', '1', '2019-03-11 22:29:54.000000', '1', '2019-03-11 22:29:54.000000', 'channel', 'views/cms/channel/index', '\0', 'channel', '', null, '2', '栏目管理', '1', 'cms', '[0],[cms],', '', null, '/channel');
INSERT INTO `SysMenu` VALUES ('42', '1', '2019-03-11 22:30:17.000000', '1', '2019-03-11 22:30:17.000000', 'article', 'views/cms/article/index', '\0', 'documentation', '', null, '2', '文章管理', '2', 'cms', '[0],[cms],', '', null, '/article');
INSERT INTO `SysMenu` VALUES ('43', '1', '2019-03-11 22:30:52.000000', '1', '2019-03-11 22:30:52.000000', 'banner', 'views/cms/banner/index', '\0', 'banner', '', null, '2', 'banner管理', '3', 'cms', '[0],[cms],', '', null, '/banner');
INSERT INTO `SysMenu` VALUES ('44', '1', '2019-03-18 19:45:37.000000', '1', '2019-03-18 19:45:37.000000', 'contacts', 'views/cms/contacts/index', '\0', 'contacts', '', null, '2', '邀约管理', '4', 'cms', '[0],[cms],', '', null, '/contacts');
INSERT INTO `SysMenu` VALUES ('45', '1', '2019-03-19 10:25:05.000000', '1', '2019-03-19 10:25:05.000000', 'file', 'views/cms/file/index', '\0', 'file', '', null, '2', '文件管理', '5', 'cms', '[0],[cms],', '', null, '/fileMgr');
INSERT INTO `SysMenu` VALUES ('46', '1', '2019-03-11 22:30:17.000000', '1', '2019-03-11 22:30:17.000000', 'editArticle', 'views/cms/article/edit.vue', '\0', 'articleEdit', '', null, '2', '新建文章', '1', 'cms', '[0],[cms],', '', null, '/cms/articleEdit');
INSERT INTO `SysMenu` VALUES ('47', null, null, '23', '2020-07-15 09:39:10.000000', 'taskLog', 'views/system/task/taskLog', '', 'task', '', null, '3', '任务日志', '4', 'task', '[0],[system],[task],', '', null, '/task/taskLog');
INSERT INTO `SysMenu` VALUES ('48', null, null, '23', '2020-07-18 23:16:17.000000', 'opsLog', 'views/system/opslog/index', '\0', 'log', '', null, '2', '操作日志', '9', 'system', '[0],[system]', '', null, '/opslog');
INSERT INTO `SysMenu` VALUES ('49', null, null, '23', '2020-07-19 17:42:18.000000', 'loginLog', 'views/system/loginlog/index', '\0', 'logininfor', '', null, '2', '登录日志', '8', 'system', '[0],[system]', '', null, '/loginlog');
INSERT INTO `SysMenu` VALUES ('54', null, null, '23', '2020-07-30 15:34:41.433597', 'druid', 'views/maintain/druid/index', '\0', 'monitor', '', null, '2', '性能检测', '1', 'operationMgr', '[0],[operationMgr]', '', null, '/druid');
INSERT INTO `SysMenu` VALUES ('55', null, null, '23', '2020-07-20 15:43:28.000000', 'swagger', 'views/maintain/swagger/index', '\0', 'swagger', '', null, '2', '接口文档', '2', 'operationMgr', '[0],[operationMgr]', '', null, '/swagger');
INSERT INTO `SysMenu` VALUES ('56', '1', '2019-06-10 21:26:52.000000', '1', '2019-06-10 21:26:52.000000', 'messageMgr', 'layout', '\0', 'message', '', null, '1', '消息管理', '4', '0', '[0],', '', null, '/message');
INSERT INTO `SysMenu` VALUES ('57', '1', '2019-06-10 21:27:34.000000', '1', '2019-06-10 21:27:34.000000', 'msg', 'views/message/message/index', '\0', 'message', '', null, '2', '历史消息', '1', 'messageMgr', 'messageMgr', '', null, '/history');
INSERT INTO `SysMenu` VALUES ('58', '1', '2019-06-10 21:27:56.000000', '1', '2019-06-10 21:27:56.000000', 'msgTpl', 'views/message/template/index', '\0', 'template', '', null, '2', '消息模板', '2', 'messageMgr', 'messageMgr', '', null, '/template');
INSERT INTO `SysMenu` VALUES ('59', '1', '2019-06-10 21:28:21.000000', '1', '2019-06-10 21:28:21.000000', 'msgSender', 'views/message/sender/index', '\0', 'sender', '', null, '2', '消息发送者', '3', 'messageMgr', 'messageMgr', '', null, '/sender');
INSERT INTO `SysMenu` VALUES ('60', '1', '2019-06-10 21:28:21.000000', '1', '2019-06-10 21:28:21.000000', 'msgClear', null, '\0', null, '', null, '2', '清空历史消息', '3', 'messageMgr', 'messageMgr', '', null, null);
INSERT INTO `SysMenu` VALUES ('61', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'msgTplEdit', null, '\0', null, '\0', null, '3', '编辑消息模板', '1', 'msgTpl', 'msgTpl', '', null, null);
INSERT INTO `SysMenu` VALUES ('62', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'msgTplDelete', null, '\0', null, '\0', null, '3', '删除消息模板', '2', 'msgTpl', 'msgTpl', '', null, null);
INSERT INTO `SysMenu` VALUES ('63', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'msgSenderEdit', null, '\0', null, '\0', null, '3', '编辑消息发送器', '1', 'msgSender', 'msgSender', '', null, null);
INSERT INTO `SysMenu` VALUES ('64', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'msgSenderDelete', null, '\0', null, '\0', null, '3', '删除消息发送器', '2', 'msgSender', 'msgSender', '', null, null);
INSERT INTO `SysMenu` VALUES ('65', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'fileUpload', null, '\0', null, '\0', null, '3', '上传文件', '1', 'file', '[0],[cms],[file],', '', null, null);
INSERT INTO `SysMenu` VALUES ('66', '1', '2019-07-31 21:51:33.000000', '1', '2019-07-31 21:51:33.000000', 'bannerEdit', null, '\0', null, '\0', null, '3', '编辑banner', '1', 'banner', '[0],[cms],[banner],', '', null, null);
INSERT INTO `SysMenu` VALUES ('67', '1', '2019-07-31 21:51:33.000000', '1', '2019-07-31 21:51:33.000000', 'bannerDelete', null, '\0', null, '\0', null, '3', '删除banner', '2', 'banner', '[0],[cms],[banner],', '', null, null);
INSERT INTO `SysMenu` VALUES ('68', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'channelEdit', null, '\0', null, '\0', null, '3', '编辑栏目', '1', 'channel', '[0],[cms],[channel],', '', null, null);
INSERT INTO `SysMenu` VALUES ('69', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'channelDelete', null, '\0', null, '\0', null, '3', '删除栏目', '2', 'channel', '[0],[cms],[channel],', '', null, null);
INSERT INTO `SysMenu` VALUES ('70', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deleteArticle', null, '\0', null, '\0', null, '3', '删除文章', '2', 'article', '[0],[cms],[article]', '', null, null);
INSERT INTO `SysMenu` VALUES ('71', null, null, '23', '2020-07-19 18:22:40.000000', 'nlogLog', 'views/system/nloglog/index', '\0', 'log', '', null, '2', 'Nlog日志', '10', 'system', '[0],[system]', '', null, '/nloglogs');
INSERT INTO `SysMenu` VALUES ('72', null, null, '23', '2020-08-02 21:38:54.017724', 'health', 'views/maintain/health/index', '\0', 'monitor', '', null, '2', '健康检测', '3', 'operationMgr', '[0],[operationMgr]', '', null, '/health');

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
INSERT INTO `SysNotice` VALUES ('1', '1', '2017-01-11 08:53:20.000000', '1', '2019-01-08 23:30:58.000000', '欢迎使用XXX后台管理系统', '完美世界', '10');

-- ----------------------------
-- Table structure for SysRelation
-- ----------------------------
DROP TABLE IF EXISTS `SysRelation`;
CREATE TABLE `SysRelation` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `MenuId` bigint(20) DEFAULT NULL,
  `RoleId` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `IX_SysRelation_RoleId` (`RoleId`),
  KEY `IX_SysRelation_MenuId` (`MenuId`),
  CONSTRAINT `FK_SysRelation_SysMenu_MenuId` FOREIGN KEY (`MenuId`) REFERENCES `SysMenu` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_SysRelation_SysRole_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `SysRole` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=81596376007894 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='菜单角色关系';

-- ----------------------------
-- Records of SysRelation
-- ----------------------------
INSERT INTO `SysRelation` VALUES ('81595947086863', '1', '1');
INSERT INTO `SysRelation` VALUES ('81595947086864', '4', '1');
INSERT INTO `SysRelation` VALUES ('81595947086865', '5', '1');
INSERT INTO `SysRelation` VALUES ('81595947086866', '6', '1');
INSERT INTO `SysRelation` VALUES ('81595947086867', '7', '1');
INSERT INTO `SysRelation` VALUES ('81595947086868', '8', '1');
INSERT INTO `SysRelation` VALUES ('81595947086869', '9', '1');
INSERT INTO `SysRelation` VALUES ('81595947086870', '10', '1');
INSERT INTO `SysRelation` VALUES ('81595947086871', '11', '1');
INSERT INTO `SysRelation` VALUES ('81595947086872', '12', '1');
INSERT INTO `SysRelation` VALUES ('81595947086873', '13', '1');
INSERT INTO `SysRelation` VALUES ('81595947086874', '14', '1');
INSERT INTO `SysRelation` VALUES ('81595947086875', '15', '1');
INSERT INTO `SysRelation` VALUES ('81595947086876', '16', '1');
INSERT INTO `SysRelation` VALUES ('81595947086877', '17', '1');
INSERT INTO `SysRelation` VALUES ('81595947086878', '18', '1');
INSERT INTO `SysRelation` VALUES ('81595947086879', '19', '1');
INSERT INTO `SysRelation` VALUES ('81595947086880', '20', '1');
INSERT INTO `SysRelation` VALUES ('81595947086881', '21', '1');
INSERT INTO `SysRelation` VALUES ('81595947086882', '23', '1');
INSERT INTO `SysRelation` VALUES ('81595947086883', '24', '1');
INSERT INTO `SysRelation` VALUES ('81595947086884', '28', '1');
INSERT INTO `SysRelation` VALUES ('81595947086885', '29', '1');
INSERT INTO `SysRelation` VALUES ('81595947086886', '32', '1');
INSERT INTO `SysRelation` VALUES ('81595947086887', '22', '1');
INSERT INTO `SysRelation` VALUES ('81595947086888', '25', '1');
INSERT INTO `SysRelation` VALUES ('81595947086889', '26', '1');
INSERT INTO `SysRelation` VALUES ('81595947086890', '27', '1');
INSERT INTO `SysRelation` VALUES ('81595947086891', '30', '1');
INSERT INTO `SysRelation` VALUES ('81595947086892', '31', '1');
INSERT INTO `SysRelation` VALUES ('81595947086893', '33', '1');
INSERT INTO `SysRelation` VALUES ('81595947086894', '34', '1');
INSERT INTO `SysRelation` VALUES ('81595947086895', '35', '1');
INSERT INTO `SysRelation` VALUES ('81595947086896', '36', '1');
INSERT INTO `SysRelation` VALUES ('81595947086897', '37', '1');
INSERT INTO `SysRelation` VALUES ('81595947086898', '38', '1');
INSERT INTO `SysRelation` VALUES ('81595947086899', '39', '1');
INSERT INTO `SysRelation` VALUES ('81595947086900', '40', '1');
INSERT INTO `SysRelation` VALUES ('81595947086901', '47', '1');
INSERT INTO `SysRelation` VALUES ('81595947086902', '48', '1');
INSERT INTO `SysRelation` VALUES ('81595947086903', '49', '1');
INSERT INTO `SysRelation` VALUES ('81595947086904', '2', '1');
INSERT INTO `SysRelation` VALUES ('81595947086905', '41', '1');
INSERT INTO `SysRelation` VALUES ('81595947086906', '68', '1');
INSERT INTO `SysRelation` VALUES ('81595947086907', '69', '1');
INSERT INTO `SysRelation` VALUES ('81595947086908', '42', '1');
INSERT INTO `SysRelation` VALUES ('81595947086909', '70', '1');
INSERT INTO `SysRelation` VALUES ('81595947086910', '43', '1');
INSERT INTO `SysRelation` VALUES ('81595947086911', '66', '1');
INSERT INTO `SysRelation` VALUES ('81595947086912', '67', '1');
INSERT INTO `SysRelation` VALUES ('81595947086913', '44', '1');
INSERT INTO `SysRelation` VALUES ('81595947086914', '45', '1');
INSERT INTO `SysRelation` VALUES ('81595947086915', '65', '1');
INSERT INTO `SysRelation` VALUES ('81595947086916', '46', '1');
INSERT INTO `SysRelation` VALUES ('81595947086917', '3', '1');
INSERT INTO `SysRelation` VALUES ('81595947086918', '54', '1');
INSERT INTO `SysRelation` VALUES ('81595947086919', '55', '1');
INSERT INTO `SysRelation` VALUES ('81595947086920', '72', '1');
INSERT INTO `SysRelation` VALUES ('81596376007848', '1', '2');
INSERT INTO `SysRelation` VALUES ('81596376007849', '4', '2');
INSERT INTO `SysRelation` VALUES ('81596376007850', '5', '2');
INSERT INTO `SysRelation` VALUES ('81596376007851', '6', '2');
INSERT INTO `SysRelation` VALUES ('81596376007852', '7', '2');
INSERT INTO `SysRelation` VALUES ('81596376007853', '8', '2');
INSERT INTO `SysRelation` VALUES ('81596376007854', '9', '2');
INSERT INTO `SysRelation` VALUES ('81596376007855', '10', '2');
INSERT INTO `SysRelation` VALUES ('81596376007856', '11', '2');
INSERT INTO `SysRelation` VALUES ('81596376007857', '12', '2');
INSERT INTO `SysRelation` VALUES ('81596376007858', '13', '2');
INSERT INTO `SysRelation` VALUES ('81596376007859', '14', '2');
INSERT INTO `SysRelation` VALUES ('81596376007860', '15', '2');
INSERT INTO `SysRelation` VALUES ('81596376007861', '16', '2');
INSERT INTO `SysRelation` VALUES ('81596376007862', '17', '2');
INSERT INTO `SysRelation` VALUES ('81596376007863', '18', '2');
INSERT INTO `SysRelation` VALUES ('81596376007864', '19', '2');
INSERT INTO `SysRelation` VALUES ('81596376007865', '20', '2');
INSERT INTO `SysRelation` VALUES ('81596376007866', '21', '2');
INSERT INTO `SysRelation` VALUES ('81596376007867', '23', '2');
INSERT INTO `SysRelation` VALUES ('81596376007868', '24', '2');
INSERT INTO `SysRelation` VALUES ('81596376007869', '28', '2');
INSERT INTO `SysRelation` VALUES ('81596376007870', '29', '2');
INSERT INTO `SysRelation` VALUES ('81596376007871', '32', '2');
INSERT INTO `SysRelation` VALUES ('81596376007872', '22', '2');
INSERT INTO `SysRelation` VALUES ('81596376007873', '25', '2');
INSERT INTO `SysRelation` VALUES ('81596376007874', '26', '2');
INSERT INTO `SysRelation` VALUES ('81596376007875', '27', '2');
INSERT INTO `SysRelation` VALUES ('81596376007876', '30', '2');
INSERT INTO `SysRelation` VALUES ('81596376007877', '31', '2');
INSERT INTO `SysRelation` VALUES ('81596376007878', '33', '2');
INSERT INTO `SysRelation` VALUES ('81596376007879', '34', '2');
INSERT INTO `SysRelation` VALUES ('81596376007880', '35', '2');
INSERT INTO `SysRelation` VALUES ('81596376007881', '36', '2');
INSERT INTO `SysRelation` VALUES ('81596376007882', '37', '2');
INSERT INTO `SysRelation` VALUES ('81596376007883', '38', '2');
INSERT INTO `SysRelation` VALUES ('81596376007884', '39', '2');
INSERT INTO `SysRelation` VALUES ('81596376007885', '40', '2');
INSERT INTO `SysRelation` VALUES ('81596376007886', '47', '2');
INSERT INTO `SysRelation` VALUES ('81596376007887', '48', '2');
INSERT INTO `SysRelation` VALUES ('81596376007888', '49', '2');
INSERT INTO `SysRelation` VALUES ('81596376007889', '71', '2');
INSERT INTO `SysRelation` VALUES ('81596376007890', '3', '2');
INSERT INTO `SysRelation` VALUES ('81596376007891', '54', '2');
INSERT INTO `SysRelation` VALUES ('81596376007892', '55', '2');
INSERT INTO `SysRelation` VALUES ('81596376007893', '72', '2');

-- ----------------------------
-- Table structure for SysRole
-- ----------------------------
DROP TABLE IF EXISTS `SysRole`;
CREATE TABLE `SysRole` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `DeptId` bigint(20) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Num` int(11) DEFAULT NULL,
  `Pid` bigint(20) DEFAULT NULL,
  `Tips` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=71596376206927 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='角色';

-- ----------------------------
-- Records of SysRole
-- ----------------------------
INSERT INTO `SysRole` VALUES ('1', null, null, '23', '2020-07-31 11:45:55.172088', null, '超级管理员', '1', null, 'administrator', null);
INSERT INTO `SysRole` VALUES ('2', null, null, '1', '2020-07-12 14:30:00.000000', null, '网站管理员', '2', null, 'developer', null);

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
INSERT INTO `SysTask` VALUES ('1', null, null, '23', '2020-07-15 19:50:44.000000', '\0', '0 0/30 * * * ?', '{\n\"appname\": \"web-flash\",\n\"version\":1\n}\n            \n            \n            \n            \n            \n            \n            \n            \n            \n            \n            \n            ', '\0', null, null, 'cn.enilu.flash.service.task.job.HelloJob', null, '测试任务', '任务,每30分钟执行一次');
INSERT INTO `SysTask` VALUES ('70962226407804928', '1', '2020-07-14 19:38:33.000000', null, null, '\0', 'sdfsfd', 'sfsdfsdfsdf', '\0', null, null, 'cn.enilu.flash.service.task.job.HelloJob', null, '测试惹怒', 'sfsfsd');

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

-- ----------------------------
-- Table structure for SysUser
-- ----------------------------
DROP TABLE IF EXISTS `SysUser`;
CREATE TABLE `SysUser` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Account` varchar(32) DEFAULT NULL COMMENT '账户',
  `Avatar` varchar(255) DEFAULT NULL,
  `Birthday` datetime(6) DEFAULT NULL,
  `DeptId` bigint(20) DEFAULT NULL,
  `Email` varchar(64) DEFAULT NULL COMMENT 'email',
  `Name` varchar(64) DEFAULT NULL COMMENT '姓名',
  `Password` varchar(64) DEFAULT NULL COMMENT '密码',
  `Phone` varchar(16) DEFAULT NULL COMMENT '手机号',
  `RoleId` varchar(128) DEFAULT NULL COMMENT '角色id列表，以逗号分隔',
  `Salt` varchar(16) DEFAULT NULL COMMENT '密码盐',
  `Sex` int(11) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `FK_SysUser_SysDept_DeptId` (`DeptId`),
  CONSTRAINT `FK_SysUser_SysDept_DeptId` FOREIGN KEY (`DeptId`) REFERENCES `SysDept` (`ID`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=21595947504819 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='账号';

-- ----------------------------
-- Records of SysUser
-- ----------------------------
INSERT INTO `SysUser` VALUES ('-1', '1', null, null, null, 'system', null, null, null, null, '应用系统', null, null, null, null, null, null, null);
INSERT INTO `SysUser` VALUES ('1', '1', '2016-01-29 08:49:53.000000', '23', '2020-07-21 17:47:21.000000', 'admin', null, '2017-05-05 00:00:00.000000', '27', '1alphacn@foxmail.com', '管理员', 'FB62D394677DA9592EF3C2BEDE9F1B2D', '15021222222', '1', '8pgby', '2', '2', '25');
INSERT INTO `SysUser` VALUES ('2', '1', '2018-09-13 17:21:02.000000', '2', '2020-07-03 08:18:40.000000', 'developer', null, '2017-12-31 00:00:00.000000', '25', 'ads@foxmail.com', '网站管理员', 'fac36d5616fe9ebd460691264b28ee27', '15022222222', '2,', 'vscp9', '6', '1', null);
INSERT INTO `SysUser` VALUES ('3', '1', '2020-03-03 21:43:51.000000', '0', '2020-07-01 00:12:56.000000', 'guokun', null, null, '25', 'alphacn@foxmail.com', '郭坤', '9EA2580E4BE4D8D577012AB469C75C82', null, '2,', 'vailr', '2', '1', null);
INSERT INTO `SysUser` VALUES ('4', '1', '2020-03-05 21:12:18.000000', '0', '2020-06-29 12:58:00.000000', 'test', '', '2020-03-25 00:00:00.000000', '27', 'ads@foxmail.com', '测试1', '008FB100979084A4EA9436099C38FD08', '', '2,3,', 'd6m3r', '2', '2', null);
INSERT INTO `SysUser` VALUES ('15', '2', '2020-07-03 16:46:29.000000', '1', '2020-07-11 22:48:51.000000', 'alpha', '', '2000-07-03 00:00:00.000000', '25', 'alpha@google.com', '王大户', 'D2346DE6F27B2C5FD21B7D8344DF8E95', '', null, 'xjrm1', '1', '2', null);
INSERT INTO `SysUser` VALUES ('16', '2', '2020-07-03 16:52:59.000000', '23', '2020-07-18 17:43:27.000000', 'i77cz2', '', '2000-07-03 00:00:00.000000', '25', 'alpha@google.com', '王大户', '849AAB1F2A3B5180844A5D087E043F26', '', null, 'yl004', '1', '1', null);
INSERT INTO `SysUser` VALUES ('17', '2', '2020-07-03 17:06:23.000000', '1', '2020-07-08 19:55:31.000000', 'bg68vs', '', '2000-07-03 20:46:39.000000', '25', 'alpha@google.com', '王大户', 'F6A884A9BABD7111221A7B7AAE812ECC', '', '2,3,', 'wxpnl', '1', '2', null);
INSERT INTO `SysUser` VALUES ('18', '2', '2020-07-03 17:10:31.000000', null, null, 'usdqzd', '', '2000-07-03 17:10:29.000000', '25', 'alpha@google.com', '王大户', '69B2C5BCE9CD096A2D17937E0F58F71D', '', null, 'ydkif', '1', '2', null);
INSERT INTO `SysUser` VALUES ('19', '2', '2020-07-03 17:35:06.000000', '23', '2020-07-28 08:33:34.000000', '9bn4jj', '', '2000-07-03 17:35:05.000000', '25', 'alpha@google.com', '王大户', '1FFB279BFDB59BB1DC05657F5866E655', '', '1,2', 'jy17f', '1', '1', null);
INSERT INTO `SysUser` VALUES ('20', '2', '2020-07-03 20:47:11.000000', '23', '2020-07-21 19:01:59.000000', 'fxjy89', '', '2000-07-03 00:00:00.000000', '25', 'alpha@google.com', '王大户', '7194CC5DFCC3E5551B4CF59D6613D164', '', '1,', 'uk144', '2', '1', null);
INSERT INTO `SysUser` VALUES ('21', '1', '2020-07-12 14:25:45.000000', '0', '2020-07-18 00:12:26.000000', 'alphago', null, '2015-01-01 00:00:00.000000', '25', 'alpha@gmail.com', '风口旁的猪', 'D2B4321BC4C638C65615694E93EBEE6C', '18811112222', '2,', '2292y', '1', '2', null);
INSERT INTO `SysUser` VALUES ('22', '1', '2020-07-14 19:54:35.000000', '23', '2020-08-02 21:16:45.988392', 'alpha2007', null, '2013-07-02 00:00:00.000000', '25', 'alpha@tom.com', 'alpha', '2383158AE34C1B7792E1888E009CDFC7', '18809098888', '2,', 'mlu5c', '2', '1', null);
INSERT INTO `SysUser` VALUES ('23', '1', '2020-07-14 20:07:50.000000', '23', '2020-08-02 21:16:42.750709', 'alpha2008', null, '2020-07-02 00:00:00.000000', '24', 'alpha2008@tom.com', 'alpha2008', 'C1FE6E4E238DD6812856995AEC16AD9D', null, '2,', '2mh6e', '1', '1', null);
INSERT INTO `SysUser` VALUES ('21595947128478', '23', '2020-07-28 22:38:48.000000', null, null, 'alpha99', null, '2020-07-01 00:00:00.000000', '24', 'alpha2008@tom.com', 'alpha99', '32419ADF930A6A183D219755649A8734', null, null, 'cf2od', '1', '2', null);
INSERT INTO `SysUser` VALUES ('21595947504818', '23', '2020-07-28 22:45:04.000000', null, null, 'ssdfsdfsd', null, null, '24', 'alpha2008@tom.com', 'sadasd', '7202F88E25DB7A7019DC8752ED667E42', null, null, 'vaj3n', '1', '2', null);

-- ----------------------------
-- Table structure for SysUserFinance
-- ----------------------------
DROP TABLE IF EXISTS `SysUserFinance`;
CREATE TABLE `SysUserFinance` (
  `ID` bigint(20) NOT NULL,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  `ModifyBy` bigint(20) DEFAULT NULL,
  `ModifyTime` datetime(6) DEFAULT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `RowVersion` timestamp(3) NULL DEFAULT '2000-07-01 22:33:02.559',
  PRIMARY KEY (`ID`),
  CONSTRAINT `FK_SysUserFinance_SysUser_ID` FOREIGN KEY (`ID`) REFERENCES `SysUser` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of SysUserFinance
-- ----------------------------
INSERT INTO `SysUserFinance` VALUES ('4', null, null, '0', '2020-07-01 21:53:32.000000', '410.0000', '2020-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('15', '2', '2020-07-03 16:46:30.000000', null, null, '0.0000', '2020-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('16', '2', '2020-07-03 16:53:00.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('17', '2', '2020-07-03 17:06:23.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('18', '2', '2020-07-03 17:10:31.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('19', '2', '2020-07-03 17:35:06.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('20', '2', '2020-07-03 20:47:11.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21', '1', '2020-07-12 14:25:45.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('22', '1', '2020-07-14 19:54:35.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('23', '1', '2020-07-14 20:07:50.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21595947128478', '23', '2020-07-28 22:38:48.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21595947504818', '23', '2020-07-28 22:45:04.000000', null, null, '0.0000', '2000-07-01 22:33:02.559');
