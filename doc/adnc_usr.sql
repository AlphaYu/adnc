/*
Navicat MariaDB Data Transfer

Source Server         : 193.112.75.77
Source Server Version : 100504
Source Host           : 193.112.75.77:13308
Source Database       : adnc_usr

Target Server Type    : MariaDB
Target Server Version : 100504
File Encoding         : 65001

Date: 2020-10-15 14:20:47
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
INSERT INTO `__EFMigrationsHistory` VALUES ('20201014033346_2020101401', '3.1.6');
INSERT INTO `__EFMigrationsHistory` VALUES ('20201014084639_2020101401', '3.1.6');
INSERT INTO `__EFMigrationsHistory` VALUES ('20201015060655_2020101501', '3.1.6');

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
) ENGINE=InnoDB AUTO_INCREMENT=51597502789177 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='部门';

-- ----------------------------
-- Records of SysDept
-- ----------------------------
INSERT INTO `SysDept` VALUES ('24', null, null, '23', '2020-10-15 00:06:02.248355', '总公司', '1', '0', '[0],', '总公司', '', null);
INSERT INTO `SysDept` VALUES ('25', null, null, '23', '2020-08-02 20:54:19.992765', '开发部', '2', '24', '[0],[24],', '开发部', '', null);
INSERT INTO `SysDept` VALUES ('26', null, null, '23', '2020-10-14 18:03:00.388570', '运营部', '3', '26', '[0],[24],[26],', '运营部', '', null);
INSERT INTO `SysDept` VALUES ('27', null, null, '23', '2020-10-15 12:50:13.755615', '战略部', '4', '24', '[0],[24],', '战略部', '', null);
INSERT INTO `SysDept` VALUES ('29', '1', '2020-07-12 15:39:15.000000', '23', '2020-10-14 18:02:49.669772', 'vue组', '1', '25', '[0],[24],[25],', 'vue组', null, null);
INSERT INTO `SysDept` VALUES ('30', '1', '2020-07-12 15:40:21.000000', '23', '2020-08-16 10:11:54.627637', 'angular组', '2', '25', '[0],[24],[25],', 'angular组', null, null);
INSERT INTO `SysDept` VALUES ('32', '1', '2020-07-12 15:42:50.000000', null, null, '京东组', '2', '26', '[0],[24],[26],', '京东组', null, null);
INSERT INTO `SysDept` VALUES ('51597484892959', '23', '2020-08-15 17:48:12.966067', '23', '2020-08-15 22:45:00.972929', '京东1部', '1', '32', '[0],[24],[26],[32],', '京东1部', null, null);
INSERT INTO `SysDept` VALUES ('51597502789176', '23', '2020-08-15 22:46:47.187375', null, null, '京东2部', '2', '32', '[0],[24],[26],[32],', '京东2部', null, null);

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
  `Url` varchar(64) CHARACTER SET utf8mb4 DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  UNIQUE KEY `UK_s37unj3gh67ujhk83lqva8i1t` (`Code`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=61595946041629 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='菜单';

-- ----------------------------
-- Records of SysMenu
-- ----------------------------
INSERT INTO `SysMenu` VALUES ('1', null, null, '23', '2020-07-15 19:50:17.000000', 'usr', 'layout', '\0', 'system', '', null, '1', '用户中心', '1', '0', '[0],', '', null, '/usr');
INSERT INTO `SysMenu` VALUES ('3', null, null, '23', '2020-08-15 14:20:03.106725', 'maintain', 'layout', '\0', 'operation', '', null, '1', '运维中心', '2', '0', '[0],', '', null, '/maintain');
INSERT INTO `SysMenu` VALUES ('4', null, null, '23', '2020-10-15 13:00:18.178612', 'user', 'views/usr/user/index', '\0', 'user', '', null, '2', '用户管理', '1', 'usr', '[0],[usr]', '', null, '/user');
INSERT INTO `SysMenu` VALUES ('5', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userAdd', null, '\0', null, '\0', null, '3', '添加用户', '1', 'user', '[0],[usr],[user],', '', null, '/user/add');
INSERT INTO `SysMenu` VALUES ('6', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userEdit', null, '\0', null, '\0', null, '3', '修改用户', '2', 'user', '[0],[usr],[user],', '', null, '/user/edit');
INSERT INTO `SysMenu` VALUES ('7', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userDelete', null, '\0', null, '\0', '\0', '3', '删除用户', '3', 'user', '[0],[usr],[user],', '', null, '/user/delete');
INSERT INTO `SysMenu` VALUES ('8', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userReset', null, '\0', null, '\0', '\0', '3', '重置密码', '4', 'user', '[0],[usr],[user],', '', null, '/user/reset');
INSERT INTO `SysMenu` VALUES ('9', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userFreeze', null, '\0', null, '\0', '\0', '3', '冻结用户', '5', 'user', '[0],[usr],[user],', '', null, '/user/freeze');
INSERT INTO `SysMenu` VALUES ('10', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userUnfreeze', null, '\0', null, '\0', '\0', '3', '解除冻结用户', '6', 'user', '[0],[usr],[user],', '', null, '/user/unfreeze');
INSERT INTO `SysMenu` VALUES ('11', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'userSetRole', null, '\0', null, '\0', '\0', '3', '分配角色', '7', 'user', '[0],[usr],[user],', '', null, '/user/setRole');
INSERT INTO `SysMenu` VALUES ('12', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'role', 'views/usr/role/index', '\0', 'peoples', '', '\0', '2', '角色管理', '2', 'usr', '[0],[usr],', '', null, '/role');
INSERT INTO `SysMenu` VALUES ('13', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'roleAdd', null, '\0', null, '\0', '\0', '3', '添加角色', '1', 'role', '[0],[usr],[role],', '', null, '/role/add');
INSERT INTO `SysMenu` VALUES ('14', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'roleEdit', null, '\0', null, '\0', '\0', '3', '修改角色', '2', 'role', '[0],[usr],[role],', '', null, '/role/edit');
INSERT INTO `SysMenu` VALUES ('15', null, null, '23', '2020-07-19 17:19:55.000000', 'roleDelete', null, '\0', null, '\0', null, '3', '删除角色', '3', 'role', '[0],[usr],[role]', '', null, '/role/delete');
INSERT INTO `SysMenu` VALUES ('16', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'roleSetAuthority', null, '\0', null, '\0', '\0', '3', '配置权限', '4', 'role', '[0],[usr],[role],', '', null, '/role/setAuthority');
INSERT INTO `SysMenu` VALUES ('17', null, null, '23', '2020-10-15 13:00:21.453285', 'menu', 'views/usr/menu/index', '\0', 'menu', '', null, '2', '菜单管理', '3', 'usr', '[0],[usr]', '', null, '/menu');
INSERT INTO `SysMenu` VALUES ('18', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'menuAdd', null, '\0', null, '\0', '\0', '3', '添加菜单', '1', 'menu', '[0],[usr],[menu],', '', null, '/menu/add');
INSERT INTO `SysMenu` VALUES ('19', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'menuEdit', null, '\0', null, '\0', '\0', '3', '修改菜单', '2', 'menu', '[0],[usr],[menu],', '', null, '/menu/edit');
INSERT INTO `SysMenu` VALUES ('20', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'menuDelete', null, '\0', null, '\0', '\0', '3', '删除菜单', '3', 'menu', '[0],[usr],[menu],', '', null, '/menu/remove');
INSERT INTO `SysMenu` VALUES ('21', null, null, '1', '2020-07-12 23:41:24.000000', 'dept', 'views/usr/dept/index', '\0', 'dept', '', null, '2', '部门管理', '4', 'usr', '[0],[usr],', '', null, '/dept');
INSERT INTO `SysMenu` VALUES ('22', null, null, '1', '2020-07-13 00:05:37.000000', 'dict', 'views/maintain/dict/index', '\0', 'dict', '', null, '2', '字典管理', '5', 'maintain', '[0],[maintain],', '', null, '/dict');
INSERT INTO `SysMenu` VALUES ('23', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptEdit', null, '\0', null, '\0', null, '3', '修改部门', '1', 'dept', '[0],[usr],[dept],', '', null, '/dept/update');
INSERT INTO `SysMenu` VALUES ('24', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptDelete', null, '\0', null, '\0', null, '3', '删除部门', '1', 'dept', '[0],[usr],[dept],', '', null, '/dept/delete');
INSERT INTO `SysMenu` VALUES ('25', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictAdd', null, '\0', null, '\0', null, '3', '添加字典', '1', 'dict', '[0],[maintain],[dict],', '', null, '/dict/add');
INSERT INTO `SysMenu` VALUES ('26', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictEdit', null, '\0', null, '\0', null, '3', '修改字典', '1', 'dict', '[0],[maintain],[dict],', '', null, '/dict/update');
INSERT INTO `SysMenu` VALUES ('27', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictDelete', null, '\0', null, '\0', null, '3', '删除字典', '1', 'dict', '[0],[maintain],[dict],', '', null, '/dict/delete');
INSERT INTO `SysMenu` VALUES ('28', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptList', null, '\0', null, '\0', null, '3', '部门列表', '5', 'dept', '[0],[usr],[dept],', '', null, '/dept/list');
INSERT INTO `SysMenu` VALUES ('30', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'dictList', null, '\0', null, '\0', null, '3', '字典列表', '5', 'dict', '[0],[maintain],[dict],', '', null, '/dict/list');
INSERT INTO `SysMenu` VALUES ('32', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'deptAdd', null, '\0', null, '\0', null, '3', '添加部门', '1', 'dept', '[0],[usr],[dept],', '', null, '/dept/add');
INSERT INTO `SysMenu` VALUES ('33', null, null, '23', '2020-10-14 23:29:37.235137', 'cfg', 'views/maintain/cfg/index', '\0', 'cfg', '', null, '2', '参数管理', '6', 'maintain', '[0],[maintain]', '', null, '/cfg');
INSERT INTO `SysMenu` VALUES ('34', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'cfgAdd', null, '\0', null, '\0', null, '3', '添加系统参数', '1', 'cfg', '[0],[maintain],[cfg],', '', null, '/cfg/add');
INSERT INTO `SysMenu` VALUES ('35', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'cfgEdit', null, '\0', null, '\0', null, '3', '修改系统参数', '2', 'cfg', '[0],[maintain],[cfg],', '', null, '/cfg/update');
INSERT INTO `SysMenu` VALUES ('36', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'cfgDelete', null, '\0', null, '\0', null, '3', '删除系统参数', '3', 'cfg', '[0],[maintain],[cfg],', '', null, '/cfg/delete');
INSERT INTO `SysMenu` VALUES ('37', null, null, '23', '2020-07-18 22:47:08.000000', 'task', 'views/maintain/task/index', '\0', 'task', '', null, '2', '任务管理', '7', 'maintain', '[0],[maintain]', '', null, '/task');
INSERT INTO `SysMenu` VALUES ('38', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'taskAdd', null, '\0', null, '\0', null, '3', '添加任务', '1', 'task', '[0],[maintain],[task],', '', null, '/task/add');
INSERT INTO `SysMenu` VALUES ('39', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'taskEdit', null, '\0', null, '\0', null, '3', '修改任务', '2', 'task', '[0],[maintain],[task],', '', null, '/task/update');
INSERT INTO `SysMenu` VALUES ('40', '1', '2019-07-31 22:04:30.000000', '1', '2019-07-31 22:04:30.000000', 'taskDelete', null, '\0', null, '\0', null, '3', '删除任务', '3', 'task', '[0],[maintain],[task],', '', null, '/task/delete');
INSERT INTO `SysMenu` VALUES ('47', null, null, '23', '2020-07-15 09:39:10.000000', 'taskLog', 'views/maintain/task/taskLog', '', 'task', '', null, '3', '任务日志', '4', 'task', '[0],[maintain],[task],', '', null, '/task/taskLog');
INSERT INTO `SysMenu` VALUES ('48', null, null, '23', '2020-10-15 13:00:29.485761', 'opsLog', 'views/maintain/opslog/index', '\0', 'log', '', null, '2', '操作日志', '9', 'maintain', '[0],[maintain]', '', null, '/opslog');
INSERT INTO `SysMenu` VALUES ('49', null, null, '23', '2020-07-19 17:42:18.000000', 'loginLog', 'views/maintain/loginlog/index', '\0', 'logininfor', '', null, '2', '登录日志', '8', 'maintain', '[0],[maintain]', '', null, '/loginlog');
INSERT INTO `SysMenu` VALUES ('54', null, null, '23', '2020-10-15 13:48:20.495472', 'druid', 'layout', '\0', 'link', '', null, '2', '性能检测', '1', 'maintain', '[0],[maintain]', '', null, 'http://193.112.75.77:18886');
INSERT INTO `SysMenu` VALUES ('55', null, null, '23', '2020-07-20 15:43:28.000000', 'swagger', 'views/maintain/swagger/index', '\0', 'swagger', '', null, '2', '接口文档', '2', 'maintain', '[0],[maintain]', '', null, '/swagger');
INSERT INTO `SysMenu` VALUES ('71', null, null, '23', '2020-07-19 18:22:40.000000', 'nlogLog', 'views/maintain/nloglog/index', '\0', 'log', '', null, '2', 'Nlog日志', '10', 'maintain', '[0],[maintain]', '', null, '/nloglogs');
INSERT INTO `SysMenu` VALUES ('72', null, null, '23', '2020-10-15 14:08:04.302624', 'health', 'layout', '\0', 'monitor', '', null, '2', '健康检测', '3', 'maintain', '[0],[maintain]', '', null, 'http://193.112.75.77:8666/healthchecks-ui');

-- ----------------------------
-- Table structure for SysRelation
-- ----------------------------
DROP TABLE IF EXISTS `SysRelation`;
CREATE TABLE `SysRelation` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `MenuId` bigint(20) DEFAULT NULL,
  `RoleId` bigint(20) DEFAULT NULL,
  `CreateBy` bigint(20) DEFAULT NULL,
  `CreateTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `IX_SysRelation_RoleId` (`RoleId`),
  KEY `IX_SysRelation_MenuId` (`MenuId`),
  CONSTRAINT `FK_SysRelation_SysMenu_MenuId` FOREIGN KEY (`MenuId`) REFERENCES `SysMenu` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_SysRelation_SysRole_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `SysRole` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=81602737364100 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='菜单角色关系';

-- ----------------------------
-- Records of SysRelation
-- ----------------------------
INSERT INTO `SysRelation` VALUES ('81597313189661', '1', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189662', '4', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189663', '5', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189664', '6', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189665', '7', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189666', '8', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189667', '9', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189668', '10', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189669', '11', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189670', '12', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189671', '13', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189672', '14', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189673', '15', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189674', '16', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189675', '17', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189676', '18', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189677', '19', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189678', '20', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189679', '21', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189680', '23', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189681', '24', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189682', '28', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189684', '32', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189685', '22', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189686', '25', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189687', '26', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189688', '27', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189689', '30', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189691', '33', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189692', '34', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189693', '35', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189694', '36', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189695', '37', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189696', '38', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189697', '39', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189698', '40', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189699', '47', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189700', '48', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189701', '49', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189702', '71', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189703', '3', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189704', '54', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189705', '55', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81597313189706', '72', '2', null, null);
INSERT INTO `SysRelation` VALUES ('81602737364055', '1', '1', '23', '2020-10-15 12:49:24.076606');
INSERT INTO `SysRelation` VALUES ('81602737364056', '4', '1', '23', '2020-10-15 12:49:24.076607');
INSERT INTO `SysRelation` VALUES ('81602737364057', '5', '1', '23', '2020-10-15 12:49:24.076608');
INSERT INTO `SysRelation` VALUES ('81602737364058', '6', '1', '23', '2020-10-15 12:49:24.076608');
INSERT INTO `SysRelation` VALUES ('81602737364059', '7', '1', '23', '2020-10-15 12:49:24.076608');
INSERT INTO `SysRelation` VALUES ('81602737364060', '8', '1', '23', '2020-10-15 12:49:24.076608');
INSERT INTO `SysRelation` VALUES ('81602737364061', '9', '1', '23', '2020-10-15 12:49:24.076609');
INSERT INTO `SysRelation` VALUES ('81602737364062', '10', '1', '23', '2020-10-15 12:49:24.076609');
INSERT INTO `SysRelation` VALUES ('81602737364063', '11', '1', '23', '2020-10-15 12:49:24.076609');
INSERT INTO `SysRelation` VALUES ('81602737364064', '12', '1', '23', '2020-10-15 12:49:24.076609');
INSERT INTO `SysRelation` VALUES ('81602737364065', '13', '1', '23', '2020-10-15 12:49:24.076609');
INSERT INTO `SysRelation` VALUES ('81602737364066', '14', '1', '23', '2020-10-15 12:49:24.076610');
INSERT INTO `SysRelation` VALUES ('81602737364067', '15', '1', '23', '2020-10-15 12:49:24.076610');
INSERT INTO `SysRelation` VALUES ('81602737364068', '16', '1', '23', '2020-10-15 12:49:24.076610');
INSERT INTO `SysRelation` VALUES ('81602737364069', '17', '1', '23', '2020-10-15 12:49:24.076610');
INSERT INTO `SysRelation` VALUES ('81602737364070', '18', '1', '23', '2020-10-15 12:49:24.076611');
INSERT INTO `SysRelation` VALUES ('81602737364071', '19', '1', '23', '2020-10-15 12:49:24.076611');
INSERT INTO `SysRelation` VALUES ('81602737364072', '20', '1', '23', '2020-10-15 12:49:24.076611');
INSERT INTO `SysRelation` VALUES ('81602737364073', '21', '1', '23', '2020-10-15 12:49:24.076611');
INSERT INTO `SysRelation` VALUES ('81602737364074', '23', '1', '23', '2020-10-15 12:49:24.076612');
INSERT INTO `SysRelation` VALUES ('81602737364075', '24', '1', '23', '2020-10-15 12:49:24.076612');
INSERT INTO `SysRelation` VALUES ('81602737364076', '28', '1', '23', '2020-10-15 12:49:24.076612');
INSERT INTO `SysRelation` VALUES ('81602737364078', '32', '1', '23', '2020-10-15 12:49:24.076612');
INSERT INTO `SysRelation` VALUES ('81602737364079', '3', '1', '23', '2020-10-15 12:49:24.076613');
INSERT INTO `SysRelation` VALUES ('81602737364080', '22', '1', '23', '2020-10-15 12:49:24.076613');
INSERT INTO `SysRelation` VALUES ('81602737364081', '25', '1', '23', '2020-10-15 12:49:24.076613');
INSERT INTO `SysRelation` VALUES ('81602737364082', '26', '1', '23', '2020-10-15 12:49:24.076613');
INSERT INTO `SysRelation` VALUES ('81602737364083', '27', '1', '23', '2020-10-15 12:49:24.076614');
INSERT INTO `SysRelation` VALUES ('81602737364084', '30', '1', '23', '2020-10-15 12:49:24.076614');
INSERT INTO `SysRelation` VALUES ('81602737364086', '33', '1', '23', '2020-10-15 12:49:24.076614');
INSERT INTO `SysRelation` VALUES ('81602737364087', '34', '1', '23', '2020-10-15 12:49:24.076615');
INSERT INTO `SysRelation` VALUES ('81602737364088', '35', '1', '23', '2020-10-15 12:49:24.076615');
INSERT INTO `SysRelation` VALUES ('81602737364089', '36', '1', '23', '2020-10-15 12:49:24.076615');
INSERT INTO `SysRelation` VALUES ('81602737364090', '37', '1', '23', '2020-10-15 12:49:24.076615');
INSERT INTO `SysRelation` VALUES ('81602737364091', '38', '1', '23', '2020-10-15 12:49:24.076615');
INSERT INTO `SysRelation` VALUES ('81602737364092', '39', '1', '23', '2020-10-15 12:49:24.076616');
INSERT INTO `SysRelation` VALUES ('81602737364093', '40', '1', '23', '2020-10-15 12:49:24.076616');
INSERT INTO `SysRelation` VALUES ('81602737364094', '47', '1', '23', '2020-10-15 12:49:24.076616');
INSERT INTO `SysRelation` VALUES ('81602737364095', '48', '1', '23', '2020-10-15 12:49:24.076616');
INSERT INTO `SysRelation` VALUES ('81602737364096', '49', '1', '23', '2020-10-15 12:49:24.076617');
INSERT INTO `SysRelation` VALUES ('81602737364097', '54', '1', '23', '2020-10-15 12:49:24.076617');
INSERT INTO `SysRelation` VALUES ('81602737364098', '55', '1', '23', '2020-10-15 12:49:24.076617');
INSERT INTO `SysRelation` VALUES ('81602737364099', '72', '1', '23', '2020-10-15 12:49:24.076617');

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
) ENGINE=InnoDB AUTO_INCREMENT=71602669674647 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='角色';

-- ----------------------------
-- Records of SysRole
-- ----------------------------
INSERT INTO `SysRole` VALUES ('1', null, null, '23', '2020-10-15 12:49:21.108692', null, '超级管理员', '1', null, 'administrator', null);
INSERT INTO `SysRole` VALUES ('2', null, null, '23', '2020-08-15 22:02:53.399427', null, '网站管理员', '2', null, 'developer', null);

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
) ENGINE=InnoDB AUTO_INCREMENT=21602737309785 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='账号';

-- ----------------------------
-- Records of SysUser
-- ----------------------------
INSERT INTO `SysUser` VALUES ('-1', '1', null, null, null, 'system', null, null, null, null, '应用系统', null, null, null, null, null, null, null);
INSERT INTO `SysUser` VALUES ('1', '1', '2016-01-29 08:49:53.000000', '0', '2020-08-19 12:04:52.201990', 'admin', null, '2017-05-05 00:00:00.000000', '27', '1alphacn@foxmail.com', '管理员', 'FB62D394677DA9592EF3C2BEDE9F1B2D', '15021222222', '1', '8pgby', '2', '2', '25');
INSERT INTO `SysUser` VALUES ('2', '1', '2018-09-13 17:21:02.000000', '2', '2020-07-03 08:18:40.000000', 'developer', null, '2017-12-31 00:00:00.000000', '25', 'ads@foxmail.com', '网站管理员', 'fac36d5616fe9ebd460691264b28ee27', '15022222222', '2,', 'vscp9', '6', '2', null);
INSERT INTO `SysUser` VALUES ('3', '1', '2020-03-03 21:43:51.000000', '0', '2020-07-01 00:12:56.000000', 'guokun', null, null, '25', 'alphacn@foxmail.com', '郭坤', '9EA2580E4BE4D8D577012AB469C75C82', null, '2,', 'vailr', '2', '2', null);
INSERT INTO `SysUser` VALUES ('4', '1', '2020-03-05 21:12:18.000000', '0', '2020-06-29 12:58:00.000000', 'test', '', '2020-03-25 00:00:00.000000', '27', 'ads@foxmail.com', '测试1', '008FB100979084A4EA9436099C38FD08', '', '2,3,', 'd6m3r', '2', '2', null);
INSERT INTO `SysUser` VALUES ('15', '2', '2020-07-03 16:46:29.000000', '23', '2020-08-06 20:28:46.594684', 'alpha', '', '2020-08-06 00:00:00.000000', '25', 'alpha@google.com', '王大户', 'D2346DE6F27B2C5FD21B7D8344DF8E95', '1898766663', null, 'xjrm1', '1', '1', null);
INSERT INTO `SysUser` VALUES ('16', '2', '2020-07-03 16:52:59.000000', '23', '2020-07-18 17:43:27.000000', 'i77cz2', '', '2000-07-03 00:00:00.000000', '25', 'alpha@google.com', '王大户', '849AAB1F2A3B5180844A5D087E043F26', '', null, 'yl004', '1', '2', null);
INSERT INTO `SysUser` VALUES ('17', '2', '2020-07-03 17:06:23.000000', '1', '2020-07-08 19:55:31.000000', 'bg68vs', '', '2000-07-03 20:46:39.000000', '25', 'alpha@google.com', '王大户', 'F6A884A9BABD7111221A7B7AAE812ECC', '', '2,3,', 'wxpnl', '1', '2', null);
INSERT INTO `SysUser` VALUES ('18', '2', '2020-07-03 17:10:31.000000', null, null, 'usdqzd', '', '2000-07-03 17:10:29.000000', '25', 'alpha@google.com', '王大户', '69B2C5BCE9CD096A2D17937E0F58F71D', '', null, 'ydkif', '1', '1', null);
INSERT INTO `SysUser` VALUES ('19', '2', '2020-07-03 17:35:06.000000', '23', '2020-07-28 08:33:34.000000', '9bn4jj', '', '2000-07-03 17:35:05.000000', '25', 'alpha@google.com', '王大户', '1FFB279BFDB59BB1DC05657F5866E655', '', '1,2', 'jy17f', '1', '1', null);
INSERT INTO `SysUser` VALUES ('20', '2', '2020-07-03 20:47:11.000000', '23', '2020-07-21 19:01:59.000000', 'fxjy89', '', '2000-07-03 00:00:00.000000', '25', 'alpha@google.com', '王大户', '7194CC5DFCC3E5551B4CF59D6613D164', '', '1,', 'uk144', '2', '1', null);
INSERT INTO `SysUser` VALUES ('21', '1', '2020-07-12 14:25:45.000000', '23', '2020-08-06 20:37:42.713860', 'alphago', null, '2015-01-01 00:00:00.000000', '25', 'alpha@gmail.com', '风口旁的猪', 'D2B4321BC4C638C65615694E93EBEE6C', '18811112222', '2,', '2292y', '1', '2', null);
INSERT INTO `SysUser` VALUES ('22', '1', '2020-07-14 19:54:35.000000', '23', '2020-08-04 17:24:22.007489', 'alpha2007', null, '2013-07-02 00:00:00.000000', '25', 'alpha@tom.com', 'alpha', '2383158AE34C1B7792E1888E009CDFC7', '18809098888', '2,', 'mlu5c', '2', '1', null);
INSERT INTO `SysUser` VALUES ('23', '1', '2020-07-14 20:07:50.000000', '23', '2020-10-14 01:33:34.239990', 'alpha2008', null, '2020-07-02 00:00:00.000000', '24', 'alpha2008@tom.com', 'alpha2008', 'C1FE6E4E238DD6812856995AEC16AD9D', '18898776677', '2,', '2mh6e', '1', '1', null);
INSERT INTO `SysUser` VALUES ('21595947128478', '23', '2020-07-28 22:38:48.000000', '23', '2020-08-13 13:04:39.101667', 'alpha99', null, '2020-07-01 00:00:00.000000', '24', 'alpha2008@tom.com', 'alpha99', '32419ADF930A6A183D219755649A8734', null, null, 'cf2od', '1', '1', null);
INSERT INTO `SysUser` VALUES ('21595947504818', '23', '2020-07-28 22:45:04.000000', '23', '2020-08-19 10:29:23.548829', 'ssdfsdfsd', null, '2020-08-08 00:00:00.000000', '24', 'beta2009@tom.com', 'beta2009', '7202F88E25DB7A7019DC8752ED667E42', '18898767654', null, 'vaj3n', '1', '2', null);
INSERT INTO `SysUser` VALUES ('21596788202803', '23', '2020-08-07 16:16:56.221250', '23', '2020-08-13 13:09:54.668155', 'alphatest1', null, '2020-08-14 00:00:00.000000', '24', 'alpha2008@tom.com', 'alphatest1', '59F389D460AF72B10BF81C9A93CF1542', '1887898987', null, 'gf39o', '2', '1', null);
INSERT INTO `SysUser` VALUES ('21596877187370', '23', '2020-08-08 16:59:47.660294', '23', '2020-08-13 13:04:46.835807', 'beta2008', null, '2020-08-04 00:00:00.000000', '24', 'beta2008@tom.com', 'beta2008', '4B148704C8BAFF62F53E1A644490FA53', '18898767876', null, '8hy01', '1', '1', null);
INSERT INTO `SysUser` VALUES ('21596878878203', '23', '2020-08-08 17:27:58.328567', '23', '2020-08-13 13:09:43.438161', 'beta2009', null, '2020-08-08 00:00:00.000000', '30', 'beta2009@tom.com', 'beta2009', '172AFECF0EA842ABE353D81EBB25721C', '18898767655', '2', 'fvwjj', '1', '1', null);
INSERT INTO `SysUser` VALUES ('21597297886851', '23', '2020-08-13 13:51:26.970212', '23', '2020-08-13 18:54:33.241716', 'alpha9999', null, '2020-08-13 00:00:00.000000', '24', 'alpha9999@tom.com', 'alpha9999', '0130C87BAF664D86E85AAB7C9B3694EF', '18898736764', '1', 'y7d29', '1', '1', null);
INSERT INTO `SysUser` VALUES ('21602669563293', '23', '2020-10-14 17:59:24.133251', '23', '2020-10-15 12:20:35.799110', 'maxlapha', null, '2020-10-15 00:00:00.000000', '29', 'alpha2008@tom.com', '麦克斯', '28F1CE5ACD1F021FFD29C427882C99DC', '18878655434', '1,2', '9j5sr', '1', '2', null);
INSERT INTO `SysUser` VALUES ('21602737309784', '23', '2020-10-15 12:48:30.386191', '23', '2020-10-15 12:48:59.867579', 'alpha999999', null, '2020-10-07 00:00:00.000000', '27', 'alpha2008', 'alpha9999', 'F5B24896A0758AF0EAF6377D36E7E32B', '18878766545', '2', '1r7xm', '1', '2', null);

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
INSERT INTO `SysUserFinance` VALUES ('4', null, null, '0', '2020-07-01 21:53:32.000000', '0.0000', '2020-07-01 22:33:02.559');
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
INSERT INTO `SysUserFinance` VALUES ('21596788202803', '23', '2020-08-07 16:16:58.595712', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21596877187370', '23', '2020-08-08 16:59:47.660311', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21596878878203', '23', '2020-08-08 17:27:58.328570', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21597297886851', '23', '2020-08-13 13:51:27.031687', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21602669563293', '23', '2020-10-14 17:59:24.379157', null, null, '0.0000', '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES ('21602737309784', '23', '2020-10-15 12:48:30.521155', null, null, '0.0000', '2000-07-01 22:33:02.559');
