/*
 Navicat MySQL Data Transfer

 Source Server         : 193.112.75.77
 Source Server Type    : MariaDB
 Source Server Version : 100504
 Source Host           : 193.112.75.77:13308
 Source Schema         : adnc_usr_dev

 Target Server Type    : MariaDB
 Target Server Version : 100504
 File Encoding         : 65001

 Date: 18/11/2020 23:50:34
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for SysDept
-- ----------------------------
DROP TABLE IF EXISTS `SysDept`;
CREATE TABLE `SysDept`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `FullName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Num` int(11) NULL DEFAULT NULL,
  `Pid` bigint(20) NULL DEFAULT NULL,
  `Pids` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `SimpleName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Tips` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Version` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 51604765917208 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '部门' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysDept
-- ----------------------------
INSERT INTO `SysDept` VALUES (24, NULL, NULL, 23, '2020-10-15 00:06:02.248355', '总公司', 1, 0, '[0],', '总公司', '', NULL);
INSERT INTO `SysDept` VALUES (25, NULL, NULL, 23, '2020-11-16 21:37:56.861472', '研发部', 2, 24, '[0],[24],', '研发部', '', NULL);
INSERT INTO `SysDept` VALUES (26, NULL, NULL, 23, '2020-10-14 18:03:00.388570', '运营部', 3, 24, '[0],[24],', '运营部', '', NULL);
INSERT INTO `SysDept` VALUES (29, 1, '2020-07-12 15:39:15.000000', 23, '2020-10-14 18:02:49.669772', 'vue组', 1, 25, '[0],[24],[25],', 'vue组', NULL, NULL);
INSERT INTO `SysDept` VALUES (30, 1, '2020-07-12 15:40:21.000000', 23, '2020-08-16 10:11:54.627637', 'angular组', 2, 25, '[0],[24],[25],', 'angular组', NULL, NULL);
INSERT INTO `SysDept` VALUES (32, 1, '2020-07-12 15:42:50.000000', NULL, NULL, '京东组', 2, 26, '[0],[24],[26],', '京东组', NULL, NULL);
INSERT INTO `SysDept` VALUES (1605533015001, 23, '2020-11-16 21:23:35.641954', NULL, NULL, '京东2部', 4, 32, '[0],[24],[26],[32],', '京东2部', NULL, NULL);
INSERT INTO `SysDept` VALUES (51597484892959, 23, '2020-08-15 17:48:12.966067', 23, '2020-11-16 21:23:48.217002', '京东1部', 1, 32, '[0],[24],[26],[32],', '京东1部', NULL, NULL);
INSERT INTO `SysDept` VALUES (51604751944969, 23, '2020-11-07 20:25:53.382576', 23, '2020-11-07 23:35:03.091737', '财务部', 1, 24, '[0],[24],', '财务部', NULL, NULL);

-- ----------------------------
-- Table structure for SysMenu
-- ----------------------------
DROP TABLE IF EXISTS `SysMenu`;
CREATE TABLE `SysMenu`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Code` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '编号',
  `Component` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '組件配置',
  `Hidden` bit(1) NULL DEFAULT NULL COMMENT '是否隐藏',
  `Icon` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '图标',
  `IsMenu` bit(1) NOT NULL COMMENT '是否是菜单1:菜单,0:按钮',
  `IsOpen` bit(1) NULL DEFAULT NULL COMMENT '是否默认打开1:是,0:否',
  `Levels` int(11) NOT NULL COMMENT '级别',
  `Name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '名称',
  `Num` int(11) NOT NULL COMMENT '顺序',
  `PCode` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '父菜单编号',
  `PCodes` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '递归父级菜单编号',
  `Status` bit(1) NOT NULL COMMENT '状态1:启用,0:禁用',
  `Tips` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '鼠标悬停提示信息',
  `Url` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  UNIQUE INDEX `UK_s37unj3gh67ujhk83lqva8i1t`(`Code`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 61605679534227 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '菜单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysMenu
-- ----------------------------
INSERT INTO `SysMenu` VALUES (1, NULL, NULL, 23, '2020-11-07 23:33:15.057952', 'usr', 'layout', b'0', 'peoples', b'1', NULL, 1, '用户中心', 1, '0', '[0],', b'1', NULL, '/usr');
INSERT INTO `SysMenu` VALUES (3, NULL, NULL, 23, '2020-11-07 23:36:36.954733', 'maintain', 'layout', b'0', 'operation', b'1', NULL, 1, '运维中心', 2, '0', '[0],', b'1', NULL, '/maintain');
INSERT INTO `SysMenu` VALUES (4, NULL, NULL, 23, '2020-11-16 21:24:02.124249', 'user', 'views/usr/user/index', b'0', 'user', b'1', NULL, 2, '用户管理', 1, 'usr', '[0],[usr]', b'1', NULL, '/user');
INSERT INTO `SysMenu` VALUES (5, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'userAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加用户', 1, 'user', '[0],[usr],[user],', b'1', NULL, '/user/add');
INSERT INTO `SysMenu` VALUES (6, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'userEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改用户', 2, 'user', '[0],[usr],[user],', b'1', NULL, '/user/edit');
INSERT INTO `SysMenu` VALUES (7, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'userDelete', NULL, b'0', NULL, b'0', b'0', 3, '删除用户', 3, 'user', '[0],[usr],[user],', b'1', NULL, '/user/delete');
INSERT INTO `SysMenu` VALUES (8, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'userReset', NULL, b'0', NULL, b'0', b'0', 3, '重置密码', 4, 'user', '[0],[usr],[user],', b'1', NULL, '/user/reset');
INSERT INTO `SysMenu` VALUES (9, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'userFreeze', NULL, b'0', NULL, b'0', b'0', 3, '冻结用户', 5, 'user', '[0],[usr],[user],', b'1', NULL, '/user/freeze');
INSERT INTO `SysMenu` VALUES (10, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'userUnfreeze', NULL, b'0', NULL, b'0', b'0', 3, '解除冻结用户', 6, 'user', '[0],[usr],[user],', b'1', NULL, '/user/unfreeze');
INSERT INTO `SysMenu` VALUES (11, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'userSetRole', NULL, b'0', NULL, b'0', b'0', 3, '分配角色', 7, 'user', '[0],[usr],[user],', b'1', NULL, '/user/setRole');
INSERT INTO `SysMenu` VALUES (12, NULL, NULL, 23, '2020-11-07 20:42:46.145607', 'role', 'views/usr/role/index', b'0', 'people', b'1', NULL, 2, '角色管理', 2, 'usr', '[0],[usr]', b'1', NULL, '/role');
INSERT INTO `SysMenu` VALUES (13, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'roleAdd', NULL, b'0', NULL, b'0', b'0', 3, '添加角色', 1, 'role', '[0],[usr],[role],', b'1', NULL, '/role/add');
INSERT INTO `SysMenu` VALUES (14, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'roleEdit', NULL, b'0', NULL, b'0', b'0', 3, '修改角色', 2, 'role', '[0],[usr],[role],', b'1', NULL, '/role/edit');
INSERT INTO `SysMenu` VALUES (15, NULL, NULL, 23, '2020-07-19 17:19:55.000000', 'roleDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除角色', 3, 'role', '[0],[usr],[role]', b'1', NULL, '/role/delete');
INSERT INTO `SysMenu` VALUES (16, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'roleSetAuthority', NULL, b'0', NULL, b'0', b'0', 3, '配置权限', 4, 'role', '[0],[usr],[role],', b'1', NULL, '/role/setAuthority');
INSERT INTO `SysMenu` VALUES (17, NULL, NULL, 23, '2020-10-15 13:00:21.453285', 'menu', 'views/usr/menu/index', b'0', 'menu', b'1', NULL, 2, '菜单管理', 3, 'usr', '[0],[usr]', b'1', NULL, '/menu');
INSERT INTO `SysMenu` VALUES (18, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'menuAdd', NULL, b'0', NULL, b'0', b'0', 3, '添加菜单', 1, 'menu', '[0],[usr],[menu],', b'1', NULL, '/menu/add');
INSERT INTO `SysMenu` VALUES (19, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'menuEdit', NULL, b'0', NULL, b'0', b'0', 3, '修改菜单', 2, 'menu', '[0],[usr],[menu],', b'1', NULL, '/menu/edit');
INSERT INTO `SysMenu` VALUES (20, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'menuDelete', NULL, b'0', NULL, b'0', b'0', 3, '删除菜单', 3, 'menu', '[0],[usr],[menu],', b'1', NULL, '/menu/remove');
INSERT INTO `SysMenu` VALUES (21, NULL, NULL, 1, '2020-07-12 23:41:24.000000', 'dept', 'views/usr/dept/index', b'0', 'dept', b'1', NULL, 2, '部门管理', 4, 'usr', '[0],[usr],', b'1', NULL, '/dept');
INSERT INTO `SysMenu` VALUES (22, NULL, NULL, 1, '2020-07-13 00:05:37.000000', 'dict', 'views/maintain/dict/index', b'0', 'dict', b'1', NULL, 2, '字典管理', 5, 'maintain', '[0],[maintain],', b'1', NULL, '/dict');
INSERT INTO `SysMenu` VALUES (23, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'deptEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改部门', 1, 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/update');
INSERT INTO `SysMenu` VALUES (24, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'deptDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除部门', 1, 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/delete');
INSERT INTO `SysMenu` VALUES (25, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'dictAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加字典', 1, 'dict', '[0],[maintain],[dict],', b'1', NULL, '/dict/add');
INSERT INTO `SysMenu` VALUES (26, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'dictEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改字典', 1, 'dict', '[0],[maintain],[dict],', b'1', NULL, '/dict/update');
INSERT INTO `SysMenu` VALUES (27, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'dictDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除字典', 1, 'dict', '[0],[maintain],[dict],', b'1', NULL, '/dict/delete');
INSERT INTO `SysMenu` VALUES (28, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'deptList', NULL, b'0', NULL, b'0', NULL, 3, '部门列表', 5, 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/list');
INSERT INTO `SysMenu` VALUES (30, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'dictList', NULL, b'0', NULL, b'0', NULL, 3, '字典列表', 5, 'dict', '[0],[maintain],[dict],', b'1', NULL, '/dict/list');
INSERT INTO `SysMenu` VALUES (32, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'deptAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加部门', 1, 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/add');
INSERT INTO `SysMenu` VALUES (33, NULL, NULL, 23, '2020-10-14 23:29:37.235137', 'cfg', 'views/maintain/cfg/index', b'0', 'cfg', b'1', NULL, 2, '参数管理', 6, 'maintain', '[0],[maintain]', b'1', NULL, '/cfg');
INSERT INTO `SysMenu` VALUES (34, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'cfgAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加系统参数', 1, 'cfg', '[0],[maintain],[cfg],', b'1', NULL, '/cfg/add');
INSERT INTO `SysMenu` VALUES (35, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'cfgEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改系统参数', 2, 'cfg', '[0],[maintain],[cfg],', b'1', NULL, '/cfg/update');
INSERT INTO `SysMenu` VALUES (36, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'cfgDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除系统参数', 3, 'cfg', '[0],[maintain],[cfg],', b'1', NULL, '/cfg/delete');
INSERT INTO `SysMenu` VALUES (37, NULL, NULL, 23, '2020-11-07 23:45:44.439678', 'task', 'views/maintain/task/index', b'0', 'task', b'1', NULL, 2, '任务管理', 7, 'maintain', '[0],[maintain]', b'1', NULL, '/task');
INSERT INTO `SysMenu` VALUES (38, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'taskAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加任务', 1, 'task', '[0],[maintain],[task],', b'1', NULL, '/task/add');
INSERT INTO `SysMenu` VALUES (39, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'taskEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改任务', 2, 'task', '[0],[maintain],[task],', b'1', NULL, '/task/update');
INSERT INTO `SysMenu` VALUES (40, 1, '2019-07-31 22:04:30.000000', 1, '2019-07-31 22:04:30.000000', 'taskDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除任务', 3, 'task', '[0],[maintain],[task],', b'1', NULL, '/task/delete');
INSERT INTO `SysMenu` VALUES (47, NULL, NULL, 23, '2020-07-15 09:39:10.000000', 'taskLog', 'views/maintain/task/taskLog', b'1', 'task', b'1', NULL, 3, '任务日志', 4, 'task', '[0],[maintain],[task],', b'1', NULL, '/task/taskLog');
INSERT INTO `SysMenu` VALUES (48, NULL, NULL, 23, '2020-10-15 13:00:29.485761', 'opsLog', 'views/maintain/opslog/index', b'0', 'log', b'1', NULL, 2, '操作日志', 9, 'maintain', '[0],[maintain]', b'1', NULL, '/opslog');
INSERT INTO `SysMenu` VALUES (49, NULL, NULL, 23, '2020-11-07 23:35:40.885413', 'loginLog', 'views/maintain/loginlog/index', b'0', 'logininfor', b'1', NULL, 2, '登录日志', 8, 'maintain', '[0],[maintain]', b'1', NULL, '/loginlog');
INSERT INTO `SysMenu` VALUES (54, NULL, NULL, 23, '2020-10-15 13:48:20.495472', 'druid', 'layout', b'0', 'link', b'1', NULL, 2, '性能检测', 1, 'maintain', '[0],[maintain]', b'1', NULL, 'http://193.112.75.77:18886');
INSERT INTO `SysMenu` VALUES (55, NULL, NULL, 23, '2020-07-20 15:43:28.000000', 'swagger', 'views/maintain/swagger/index', b'0', 'swagger', b'1', NULL, 2, '接口文档', 2, 'maintain', '[0],[maintain]', b'1', NULL, '/swagger');
INSERT INTO `SysMenu` VALUES (71, NULL, NULL, 23, '2020-11-16 21:24:10.185553', 'nlogLog', 'views/maintain/nloglog/index', b'0', 'log', b'1', NULL, 2, 'Nlog日志', 10, 'maintain', '[0],[maintain]', b'1', NULL, '/nloglogs');
INSERT INTO `SysMenu` VALUES (72, NULL, NULL, 23, '2020-10-15 14:08:04.302624', 'health', 'layout', b'0', 'monitor', b'1', NULL, 2, '健康检测', 3, 'maintain', '[0],[maintain]', b'1', NULL, 'http://193.112.75.77:8666/healthchecks-ui');
INSERT INTO `SysMenu` VALUES (73, 23, '2020-11-18 13:56:21.000000', 23, '2020-11-18 13:56:21.000000', 'menuList', NULL, b'0', '', b'0', NULL, 3, '菜单列表', 4, 'menu', '[0],[usr][menu]', b'1', NULL, '/menu/list');
INSERT INTO `SysMenu` VALUES (74, 23, '2020-11-18 13:56:21.000000', 23, '2020-11-18 13:56:21.000000', 'roleList', NULL, b'0', NULL, b'0', NULL, 3, '角色列表', 5, 'role', '[0],[usr],[role]', b'1', NULL, '/role/list');
INSERT INTO `SysMenu` VALUES (75, 23, '2020-11-18 13:56:21.000000', 23, '2020-11-18 13:56:21.000000', 'userList', NULL, b'0', '', b'0', NULL, 3, '用户列表', 8, 'user', '[0],[usr][user]', b'1', NULL, 'user/list');
INSERT INTO `SysMenu` VALUES (76, 23, '2020-11-18 13:56:21.000000', 23, '2020-11-18 13:56:21.000000', 'cfgList', NULL, b'0', '', b'0', NULL, 3, '系统参数列表', 4, 'cfg', '[0],[maintain][cfg]', b'1', NULL, '/cfg/list');
INSERT INTO `SysMenu` VALUES (77, 23, '2020-11-18 13:56:21.000000', 23, '2020-11-18 13:56:21.000000', 'taskList', NULL, b'0', NULL, b'0', NULL, 3, '任务列表', 4, 'task', '[0],[maintain][task]', b'1', NULL, '/task/list');
INSERT INTO `SysMenu` VALUES (1605711424050, 23, '2020-11-18 22:57:04.147762', NULL, NULL, 'eventBus', 'layout', NULL, 'monitor', b'1', NULL, 2, 'EventBus', 4, 'maintain', '[0],[maintain]', b'1', NULL, 'http://193.112.75.77:8888/cus/cap/');

-- ----------------------------
-- Table structure for SysRelation
-- ----------------------------
DROP TABLE IF EXISTS `SysRelation`;
CREATE TABLE `SysRelation`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `MenuId` bigint(20) NULL DEFAULT NULL,
  `RoleId` bigint(20) NULL DEFAULT NULL,
  `CreateBy` bigint(20) NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_SysRelation_RoleId`(`RoleId`) USING BTREE,
  INDEX `IX_SysRelation_MenuId`(`MenuId`) USING BTREE,
  CONSTRAINT `FK_SysRelation_SysMenu_MenuId` FOREIGN KEY (`MenuId`) REFERENCES `SysMenu` (`ID`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_SysRelation_SysRole_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `SysRole` (`ID`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 81604693659533 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '菜单角色关系' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysRelation
-- ----------------------------
INSERT INTO `SysRelation` VALUES (1605711439051, 1, 2, 23, '2020-11-18 22:57:19.745977');
INSERT INTO `SysRelation` VALUES (1605711439052, 4, 2, 23, '2020-11-18 22:57:19.745981');
INSERT INTO `SysRelation` VALUES (1605711439053, 5, 2, 23, '2020-11-18 22:57:19.745981');
INSERT INTO `SysRelation` VALUES (1605711439054, 6, 2, 23, '2020-11-18 22:57:19.745982');
INSERT INTO `SysRelation` VALUES (1605711439055, 7, 2, 23, '2020-11-18 22:57:19.745982');
INSERT INTO `SysRelation` VALUES (1605711439056, 8, 2, 23, '2020-11-18 22:57:19.745983');
INSERT INTO `SysRelation` VALUES (1605711439057, 9, 2, 23, '2020-11-18 22:57:19.745983');
INSERT INTO `SysRelation` VALUES (1605711439058, 10, 2, 23, '2020-11-18 22:57:19.745984');
INSERT INTO `SysRelation` VALUES (1605711439059, 11, 2, 23, '2020-11-18 22:57:19.745984');
INSERT INTO `SysRelation` VALUES (1605711439060, 75, 2, 23, '2020-11-18 22:57:19.745985');
INSERT INTO `SysRelation` VALUES (1605711439061, 12, 2, 23, '2020-11-18 22:57:19.745985');
INSERT INTO `SysRelation` VALUES (1605711439062, 13, 2, 23, '2020-11-18 22:57:19.745986');
INSERT INTO `SysRelation` VALUES (1605711439063, 14, 2, 23, '2020-11-18 22:57:19.745986');
INSERT INTO `SysRelation` VALUES (1605711439064, 15, 2, 23, '2020-11-18 22:57:19.745987');
INSERT INTO `SysRelation` VALUES (1605711439065, 16, 2, 23, '2020-11-18 22:57:19.745987');
INSERT INTO `SysRelation` VALUES (1605711439066, 74, 2, 23, '2020-11-18 22:57:19.745988');
INSERT INTO `SysRelation` VALUES (1605711439067, 17, 2, 23, '2020-11-18 22:57:19.745988');
INSERT INTO `SysRelation` VALUES (1605711439068, 18, 2, 23, '2020-11-18 22:57:19.745989');
INSERT INTO `SysRelation` VALUES (1605711439069, 19, 2, 23, '2020-11-18 22:57:19.745989');
INSERT INTO `SysRelation` VALUES (1605711439070, 20, 2, 23, '2020-11-18 22:57:19.745989');
INSERT INTO `SysRelation` VALUES (1605711439071, 73, 2, 23, '2020-11-18 22:57:19.745990');
INSERT INTO `SysRelation` VALUES (1605711439072, 21, 2, 23, '2020-11-18 22:57:19.745990');
INSERT INTO `SysRelation` VALUES (1605711439073, 23, 2, 23, '2020-11-18 22:57:19.745991');
INSERT INTO `SysRelation` VALUES (1605711439074, 24, 2, 23, '2020-11-18 22:57:19.745991');
INSERT INTO `SysRelation` VALUES (1605711439075, 28, 2, 23, '2020-11-18 22:57:19.745992');
INSERT INTO `SysRelation` VALUES (1605711439076, 32, 2, 23, '2020-11-18 22:57:19.745992');
INSERT INTO `SysRelation` VALUES (1605711439077, 3, 2, 23, '2020-11-18 22:57:19.745992');
INSERT INTO `SysRelation` VALUES (1605711439078, 22, 2, 23, '2020-11-18 22:57:19.745993');
INSERT INTO `SysRelation` VALUES (1605711439079, 25, 2, 23, '2020-11-18 22:57:19.745993');
INSERT INTO `SysRelation` VALUES (1605711439080, 26, 2, 23, '2020-11-18 22:57:19.745994');
INSERT INTO `SysRelation` VALUES (1605711439081, 27, 2, 23, '2020-11-18 22:57:19.745994');
INSERT INTO `SysRelation` VALUES (1605711439082, 30, 2, 23, '2020-11-18 22:57:19.745995');
INSERT INTO `SysRelation` VALUES (1605711439083, 33, 2, 23, '2020-11-18 22:57:19.745995');
INSERT INTO `SysRelation` VALUES (1605711439084, 34, 2, 23, '2020-11-18 22:57:19.745996');
INSERT INTO `SysRelation` VALUES (1605711439085, 35, 2, 23, '2020-11-18 22:57:19.745996');
INSERT INTO `SysRelation` VALUES (1605711439086, 36, 2, 23, '2020-11-18 22:57:19.745997');
INSERT INTO `SysRelation` VALUES (1605711439087, 76, 2, 23, '2020-11-18 22:57:19.745997');
INSERT INTO `SysRelation` VALUES (1605711439088, 37, 2, 23, '2020-11-18 22:57:19.745998');
INSERT INTO `SysRelation` VALUES (1605711439089, 38, 2, 23, '2020-11-18 22:57:19.745998');
INSERT INTO `SysRelation` VALUES (1605711439090, 39, 2, 23, '2020-11-18 22:57:19.745999');
INSERT INTO `SysRelation` VALUES (1605711439091, 40, 2, 23, '2020-11-18 22:57:19.745999');
INSERT INTO `SysRelation` VALUES (1605711439092, 47, 2, 23, '2020-11-18 22:57:19.746000');
INSERT INTO `SysRelation` VALUES (1605711439093, 77, 2, 23, '2020-11-18 22:57:19.746000');
INSERT INTO `SysRelation` VALUES (1605711439094, 48, 2, 23, '2020-11-18 22:57:19.746001');
INSERT INTO `SysRelation` VALUES (1605711439095, 49, 2, 23, '2020-11-18 22:57:19.746001');
INSERT INTO `SysRelation` VALUES (1605711439096, 54, 2, 23, '2020-11-18 22:57:19.746001');
INSERT INTO `SysRelation` VALUES (1605711439097, 55, 2, 23, '2020-11-18 22:57:19.746002');
INSERT INTO `SysRelation` VALUES (1605711439098, 71, 2, 23, '2020-11-18 22:57:19.746002');
INSERT INTO `SysRelation` VALUES (1605711439099, 72, 2, 23, '2020-11-18 22:57:19.746003');
INSERT INTO `SysRelation` VALUES (1605711439100, 1605711424050, 2, 23, '2020-11-18 22:57:19.746003');
INSERT INTO `SysRelation` VALUES (1605711450101, 1, 1, 23, '2020-11-18 22:57:30.049511');
INSERT INTO `SysRelation` VALUES (1605711450102, 4, 1, 23, '2020-11-18 22:57:30.049515');
INSERT INTO `SysRelation` VALUES (1605711450103, 5, 1, 23, '2020-11-18 22:57:30.049527');
INSERT INTO `SysRelation` VALUES (1605711450104, 6, 1, 23, '2020-11-18 22:57:30.049528');
INSERT INTO `SysRelation` VALUES (1605711450105, 7, 1, 23, '2020-11-18 22:57:30.049529');
INSERT INTO `SysRelation` VALUES (1605711450106, 8, 1, 23, '2020-11-18 22:57:30.049529');
INSERT INTO `SysRelation` VALUES (1605711450107, 9, 1, 23, '2020-11-18 22:57:30.049530');
INSERT INTO `SysRelation` VALUES (1605711450108, 10, 1, 23, '2020-11-18 22:57:30.049530');
INSERT INTO `SysRelation` VALUES (1605711450109, 11, 1, 23, '2020-11-18 22:57:30.049530');
INSERT INTO `SysRelation` VALUES (1605711450110, 75, 1, 23, '2020-11-18 22:57:30.049531');
INSERT INTO `SysRelation` VALUES (1605711450111, 12, 1, 23, '2020-11-18 22:57:30.049532');
INSERT INTO `SysRelation` VALUES (1605711450112, 13, 1, 23, '2020-11-18 22:57:30.049532');
INSERT INTO `SysRelation` VALUES (1605711450113, 14, 1, 23, '2020-11-18 22:57:30.049533');
INSERT INTO `SysRelation` VALUES (1605711450114, 15, 1, 23, '2020-11-18 22:57:30.049533');
INSERT INTO `SysRelation` VALUES (1605711450115, 16, 1, 23, '2020-11-18 22:57:30.049534');
INSERT INTO `SysRelation` VALUES (1605711450116, 74, 1, 23, '2020-11-18 22:57:30.049534');
INSERT INTO `SysRelation` VALUES (1605711450117, 17, 1, 23, '2020-11-18 22:57:30.049534');
INSERT INTO `SysRelation` VALUES (1605711450118, 18, 1, 23, '2020-11-18 22:57:30.049535');
INSERT INTO `SysRelation` VALUES (1605711450119, 19, 1, 23, '2020-11-18 22:57:30.049536');
INSERT INTO `SysRelation` VALUES (1605711450120, 20, 1, 23, '2020-11-18 22:57:30.049536');
INSERT INTO `SysRelation` VALUES (1605711450121, 73, 1, 23, '2020-11-18 22:57:30.049536');
INSERT INTO `SysRelation` VALUES (1605711450122, 21, 1, 23, '2020-11-18 22:57:30.049537');
INSERT INTO `SysRelation` VALUES (1605711450123, 23, 1, 23, '2020-11-18 22:57:30.049537');
INSERT INTO `SysRelation` VALUES (1605711450124, 24, 1, 23, '2020-11-18 22:57:30.049538');
INSERT INTO `SysRelation` VALUES (1605711450125, 28, 1, 23, '2020-11-18 22:57:30.049538');
INSERT INTO `SysRelation` VALUES (1605711450126, 32, 1, 23, '2020-11-18 22:57:30.049539');
INSERT INTO `SysRelation` VALUES (1605711450127, 3, 1, 23, '2020-11-18 22:57:30.049539');
INSERT INTO `SysRelation` VALUES (1605711450128, 22, 1, 23, '2020-11-18 22:57:30.049540');
INSERT INTO `SysRelation` VALUES (1605711450129, 25, 1, 23, '2020-11-18 22:57:30.049540');
INSERT INTO `SysRelation` VALUES (1605711450130, 26, 1, 23, '2020-11-18 22:57:30.049540');
INSERT INTO `SysRelation` VALUES (1605711450131, 27, 1, 23, '2020-11-18 22:57:30.049541');
INSERT INTO `SysRelation` VALUES (1605711450132, 30, 1, 23, '2020-11-18 22:57:30.049541');
INSERT INTO `SysRelation` VALUES (1605711450133, 33, 1, 23, '2020-11-18 22:57:30.049542');
INSERT INTO `SysRelation` VALUES (1605711450134, 34, 1, 23, '2020-11-18 22:57:30.049542');
INSERT INTO `SysRelation` VALUES (1605711450135, 35, 1, 23, '2020-11-18 22:57:30.049543');
INSERT INTO `SysRelation` VALUES (1605711450136, 36, 1, 23, '2020-11-18 22:57:30.049543');
INSERT INTO `SysRelation` VALUES (1605711450137, 76, 1, 23, '2020-11-18 22:57:30.049544');
INSERT INTO `SysRelation` VALUES (1605711450138, 37, 1, 23, '2020-11-18 22:57:30.049544');
INSERT INTO `SysRelation` VALUES (1605711450139, 38, 1, 23, '2020-11-18 22:57:30.049544');
INSERT INTO `SysRelation` VALUES (1605711450140, 39, 1, 23, '2020-11-18 22:57:30.049545');
INSERT INTO `SysRelation` VALUES (1605711450141, 40, 1, 23, '2020-11-18 22:57:30.049545');
INSERT INTO `SysRelation` VALUES (1605711450142, 47, 1, 23, '2020-11-18 22:57:30.049548');
INSERT INTO `SysRelation` VALUES (1605711450143, 77, 1, 23, '2020-11-18 22:57:30.049549');
INSERT INTO `SysRelation` VALUES (1605711450144, 48, 1, 23, '2020-11-18 22:57:30.049549');
INSERT INTO `SysRelation` VALUES (1605711450145, 49, 1, 23, '2020-11-18 22:57:30.049550');
INSERT INTO `SysRelation` VALUES (1605711450146, 54, 1, 23, '2020-11-18 22:57:30.049550');
INSERT INTO `SysRelation` VALUES (1605711450147, 55, 1, 23, '2020-11-18 22:57:30.049551');
INSERT INTO `SysRelation` VALUES (1605711450148, 71, 1, 23, '2020-11-18 22:57:30.049551');
INSERT INTO `SysRelation` VALUES (1605711450149, 72, 1, 23, '2020-11-18 22:57:30.049552');
INSERT INTO `SysRelation` VALUES (1605711450150, 1605711424050, 1, 23, '2020-11-18 22:57:30.049552');
INSERT INTO `SysRelation` VALUES (1605711699152, 1, 1605711599151, 23, '2020-11-18 23:01:39.848242');
INSERT INTO `SysRelation` VALUES (1605711699153, 4, 1605711599151, 23, '2020-11-18 23:01:39.848245');
INSERT INTO `SysRelation` VALUES (1605711699154, 75, 1605711599151, 23, '2020-11-18 23:01:39.848245');
INSERT INTO `SysRelation` VALUES (1605711699155, 12, 1605711599151, 23, '2020-11-18 23:01:39.848246');
INSERT INTO `SysRelation` VALUES (1605711699156, 74, 1605711599151, 23, '2020-11-18 23:01:39.848246');
INSERT INTO `SysRelation` VALUES (1605711699157, 17, 1605711599151, 23, '2020-11-18 23:01:39.848247');
INSERT INTO `SysRelation` VALUES (1605711699158, 73, 1605711599151, 23, '2020-11-18 23:01:39.848247');
INSERT INTO `SysRelation` VALUES (1605711699159, 21, 1605711599151, 23, '2020-11-18 23:01:39.848247');
INSERT INTO `SysRelation` VALUES (1605711699160, 28, 1605711599151, 23, '2020-11-18 23:01:39.848248');
INSERT INTO `SysRelation` VALUES (1605711699161, 3, 1605711599151, 23, '2020-11-18 23:01:39.848248');
INSERT INTO `SysRelation` VALUES (1605711699162, 22, 1605711599151, 23, '2020-11-18 23:01:39.848249');
INSERT INTO `SysRelation` VALUES (1605711699163, 30, 1605711599151, 23, '2020-11-18 23:01:39.848249');
INSERT INTO `SysRelation` VALUES (1605711699164, 33, 1605711599151, 23, '2020-11-18 23:01:39.848249');
INSERT INTO `SysRelation` VALUES (1605711699165, 76, 1605711599151, 23, '2020-11-18 23:01:39.848250');
INSERT INTO `SysRelation` VALUES (1605711699166, 37, 1605711599151, 23, '2020-11-18 23:01:39.848250');
INSERT INTO `SysRelation` VALUES (1605711699167, 47, 1605711599151, 23, '2020-11-18 23:01:39.848250');
INSERT INTO `SysRelation` VALUES (1605711699168, 77, 1605711599151, 23, '2020-11-18 23:01:39.848251');
INSERT INTO `SysRelation` VALUES (1605711699169, 48, 1605711599151, 23, '2020-11-18 23:01:39.848251');
INSERT INTO `SysRelation` VALUES (1605711699170, 49, 1605711599151, 23, '2020-11-18 23:01:39.848252');
INSERT INTO `SysRelation` VALUES (1605711699171, 54, 1605711599151, 23, '2020-11-18 23:01:39.848252');
INSERT INTO `SysRelation` VALUES (1605711699172, 55, 1605711599151, 23, '2020-11-18 23:01:39.848252');
INSERT INTO `SysRelation` VALUES (1605711699173, 71, 1605711599151, 23, '2020-11-18 23:01:39.848253');
INSERT INTO `SysRelation` VALUES (1605711699174, 72, 1605711599151, 23, '2020-11-18 23:01:39.848253');
INSERT INTO `SysRelation` VALUES (1605711699175, 1605711424050, 1605711599151, 23, '2020-11-18 23:01:39.848253');

-- ----------------------------
-- Table structure for SysRole
-- ----------------------------
DROP TABLE IF EXISTS `SysRole`;
CREATE TABLE `SysRole`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `DeptId` bigint(20) NULL DEFAULT NULL,
  `Name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Num` int(11) NULL DEFAULT NULL,
  `Pid` bigint(20) NULL DEFAULT NULL,
  `Tips` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Version` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 71604896042898 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysRole
-- ----------------------------
INSERT INTO `SysRole` VALUES (1, NULL, NULL, 23, '2020-11-15 21:58:02.985380', NULL, '超级管理员', 1, NULL, 'administrator', NULL);
INSERT INTO `SysRole` VALUES (2, NULL, NULL, 23, '2020-11-15 21:58:05.769862', NULL, '网站管理员', 2, NULL, 'developer', NULL);
INSERT INTO `SysRole` VALUES (1605711599151, NULL, NULL, 23, '2020-11-18 23:00:11.526559', NULL, '只读管理员', 3, NULL, 'readonlier', NULL);

-- ----------------------------
-- Table structure for SysUser
-- ----------------------------
DROP TABLE IF EXISTS `SysUser`;
CREATE TABLE `SysUser`  (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreateBy` bigint(20) NULL DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Account` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '账户',
  `Avatar` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Birthday` datetime(6) NULL DEFAULT NULL,
  `DeptId` bigint(20) NULL DEFAULT NULL,
  `Email` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'email',
  `Name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '姓名',
  `Password` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '密码',
  `Phone` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '手机号',
  `RoleId` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色id列表，以逗号分隔',
  `Salt` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '密码盐',
  `Sex` int(11) NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `Version` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `FK_SysUser_SysDept_DeptId`(`DeptId`) USING BTREE,
  CONSTRAINT `FK_SysUser_SysDept_DeptId` FOREIGN KEY (`DeptId`) REFERENCES `SysDept` (`ID`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 21604896186451 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '账号' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysUser
-- ----------------------------
INSERT INTO `SysUser` VALUES (-1, 1, NULL, NULL, NULL, 'system', NULL, NULL, NULL, NULL, '应用系统', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `SysUser` VALUES (1, 1, '2016-01-29 08:49:53.000000', 23, '2020-11-09 12:39:55.034495', 'admin', NULL, '2017-05-05 00:00:00.000000', NULL, '1alphacn@foxmail.com', '管理员', 'FB62D394677DA9592EF3C2BEDE9F1B2D', '15021222222', '1', '8pgby', 2, 1, 25);
INSERT INTO `SysUser` VALUES (2, 1, '2018-09-13 17:21:02.000000', 2, '2020-07-03 08:18:40.000000', 'developer', NULL, '2017-12-31 00:00:00.000000', NULL, 'ads@foxmail.com', '网站管理员', 'fac36d5616fe9ebd460691264b28ee27', '15022222222', '2,', 'vscp9', 6, 2, NULL);
INSERT INTO `SysUser` VALUES (3, 1, '2020-03-03 21:43:51.000000', 0, '2020-07-01 00:12:56.000000', 'guokun', NULL, NULL, NULL, 'alphacn@foxmail.com', '郭坤', '9EA2580E4BE4D8D577012AB469C75C82', NULL, '2,', 'vailr', 2, 2, NULL);
INSERT INTO `SysUser` VALUES (4, 1, '2020-03-05 21:12:18.000000', 0, '2020-06-29 12:58:00.000000', 'test', '', '2020-03-25 00:00:00.000000', NULL, 'ads@foxmail.com', '测试1', '008FB100979084A4EA9436099C38FD08', '', '2,3,', 'd6m3r', 2, 2, NULL);
INSERT INTO `SysUser` VALUES (15, 2, '2020-07-03 16:46:29.000000', 23, '2020-08-06 20:28:46.594684', 'alpha', '', '2020-08-06 00:00:00.000000', NULL, 'alpha@google.com', '王大户', 'D2346DE6F27B2C5FD21B7D8344DF8E95', '1898766663', NULL, 'xjrm1', 1, 1, NULL);
INSERT INTO `SysUser` VALUES (16, 2, '2020-07-03 16:52:59.000000', 23, '2020-07-18 17:43:27.000000', 'i77cz2', '', '2000-07-03 00:00:00.000000', NULL, 'alpha@google.com', '王大户', '849AAB1F2A3B5180844A5D087E043F26', '', NULL, 'yl004', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (17, 2, '2020-07-03 17:06:23.000000', 1, '2020-07-08 19:55:31.000000', 'bg68vs', '', '2000-07-03 20:46:39.000000', NULL, 'alpha@google.com', '王大户', 'F6A884A9BABD7111221A7B7AAE812ECC', '', '2,3,', 'wxpnl', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (18, 2, '2020-07-03 17:10:31.000000', NULL, NULL, 'usdqzd', '', '2000-07-03 17:10:29.000000', NULL, 'alpha@google.com', '王大户', '69B2C5BCE9CD096A2D17937E0F58F71D', '', NULL, 'ydkif', 1, 1, NULL);
INSERT INTO `SysUser` VALUES (19, 2, '2020-07-03 17:35:06.000000', 23, '2020-07-28 08:33:34.000000', '9bn4jj', '', '2000-07-03 17:35:05.000000', NULL, 'alpha@google.com', '王大户', '1FFB279BFDB59BB1DC05657F5866E655', '', '1,2', 'jy17f', 1, 1, NULL);
INSERT INTO `SysUser` VALUES (20, 2, '2020-07-03 20:47:11.000000', 23, '2020-07-21 19:01:59.000000', 'fxjy89', '', '2000-07-03 00:00:00.000000', NULL, 'alpha@google.com', '王大户', '7194CC5DFCC3E5551B4CF59D6613D164', '', '1,', 'uk144', 2, 1, NULL);
INSERT INTO `SysUser` VALUES (21, 1, '2020-07-12 14:25:45.000000', 23, '2020-08-06 20:37:42.713860', 'alphago', NULL, '2015-01-01 00:00:00.000000', NULL, 'alpha@gmail.com', '风口旁的猪', 'D2B4321BC4C638C65615694E93EBEE6C', '18811112222', '2,', '2292y', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (22, 1, '2020-07-14 19:54:35.000000', 23, '2020-08-04 17:24:22.007489', 'alpha2007', NULL, '2013-07-02 00:00:00.000000', NULL, 'alpha@tom.com', 'alpha', '2383158AE34C1B7792E1888E009CDFC7', '18809098888', '2,', 'mlu5c', 2, 1, NULL);
INSERT INTO `SysUser` VALUES (23, 1, '2020-07-14 20:07:50.000000', 23, '2020-10-14 01:33:34.239990', 'alpha2008', NULL, '2020-07-02 00:00:00.000000', NULL, 'alpha2008@tom.com', 'alpha2008', 'C1FE6E4E238DD6812856995AEC16AD9D', '18898776677', '2,', '2mh6e', 1, 1, NULL);
INSERT INTO `SysUser` VALUES (1605711780176, 23, '2020-11-18 23:03:00.884999', 23, '2020-11-18 23:04:17.666150', 'readonly', NULL, '2020-11-18 00:00:00.000000', 51604751944969, 'readonly@gmail.com', '只能读', '23CB93FE5B25FECDCD006C0C425BBCAB', '18898659000', '1605711599151', 'jcir0', 1, 1, NULL);
INSERT INTO `SysUser` VALUES (21595947128478, 23, '2020-07-28 22:38:48.000000', 23, '2020-08-13 13:04:39.101667', 'alpha99', NULL, '2020-07-01 00:00:00.000000', NULL, 'alpha2008@tom.com', 'alpha99', '32419ADF930A6A183D219755649A8734', NULL, NULL, 'cf2od', 1, 1, NULL);
INSERT INTO `SysUser` VALUES (21595947504818, 23, '2020-07-28 22:45:04.000000', 23, '2020-08-19 10:29:23.548829', 'ssdfsdfsd', NULL, '2020-08-08 00:00:00.000000', NULL, 'beta2009@tom.com', 'beta2009', '7202F88E25DB7A7019DC8752ED667E42', '18898767654', NULL, 'vaj3n', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (21596788202803, 23, '2020-08-07 16:16:56.221250', 23, '2020-08-13 13:09:54.668155', 'alphatest1', NULL, '2020-08-14 00:00:00.000000', NULL, 'alpha2008@tom.com', 'alphatest1', '59F389D460AF72B10BF81C9A93CF1542', '1887898987', NULL, 'gf39o', 2, 1, NULL);
INSERT INTO `SysUser` VALUES (21596877187370, 23, '2020-08-08 16:59:47.660294', 23, '2020-08-13 13:04:46.835807', 'beta2008', NULL, '2020-08-04 00:00:00.000000', NULL, 'beta2008@tom.com', 'beta2008', '4B148704C8BAFF62F53E1A644490FA53', '18898767876', NULL, '8hy01', 1, 1, NULL);
INSERT INTO `SysUser` VALUES (21596878878203, 23, '2020-08-08 17:27:58.328567', 23, '2020-11-09 12:31:32.547382', 'beta2009', NULL, '2020-08-08 00:00:00.000000', NULL, 'beta2009@tom.com', 'beta2009', '172AFECF0EA842ABE353D81EBB25721C', '18898767655', '2', 'fvwjj', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (21597297886851, 23, '2020-08-13 13:51:26.970212', 23, '2020-11-09 12:31:31.186242', 'alpha9999', NULL, '2020-08-13 00:00:00.000000', NULL, 'alpha9999@tom.com', 'alpha9999', '0130C87BAF664D86E85AAB7C9B3694EF', '18898736764', '1', 'y7d29', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (21602669563293, 23, '2020-10-14 17:59:24.133251', 23, '2020-10-15 12:20:35.799110', 'maxlapha', NULL, '2020-10-15 00:00:00.000000', NULL, 'alpha2008@tom.com', '麦克斯', '28F1CE5ACD1F021FFD29C427882C99DC', '18878655434', '1,2', '9j5sr', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (21602737309784, 23, '2020-10-15 12:48:30.386191', 23, '2020-11-16 21:37:18.131497', 'alpha999999', NULL, '2020-10-07 00:00:00.000000', 24, 'alpha2008', 'alpha9999', 'F5B24896A0758AF0EAF6377D36E7E32B', '18878766545', '2', '1r7xm', 1, 2, NULL);
INSERT INTO `SysUser` VALUES (21604896186450, 23, '2020-11-09 12:29:46.604637', 23, '2020-11-15 21:56:11.771931', 'adnc', NULL, '2020-11-04 00:00:00.000000', 25, 'adnc@aspdotnetcore.net', '显示用户', '4EC13BD18AE7A1B040E3042C2F6758FC', '18898659999', '2', 'hxqz9', 1, 2, NULL);

-- ----------------------------
-- Table structure for SysUserFinance
-- ----------------------------
DROP TABLE IF EXISTS `SysUserFinance`;
CREATE TABLE `SysUserFinance`  (
  `ID` bigint(20) NOT NULL,
  `CreateBy` bigint(20) NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `ModifyBy` bigint(20) NULL DEFAULT NULL,
  `ModifyTime` datetime(6) NULL DEFAULT NULL,
  `Amount` decimal(18, 4) NOT NULL,
  `RowVersion` timestamp(3) NULL DEFAULT '2000-07-01 22:33:02.559',
  PRIMARY KEY (`ID`) USING BTREE,
  CONSTRAINT `FK_SysUserFinance_SysUser_ID` FOREIGN KEY (`ID`) REFERENCES `SysUser` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of SysUserFinance
-- ----------------------------
INSERT INTO `SysUserFinance` VALUES (4, NULL, NULL, 0, '2020-07-01 21:53:32.000000', 0.0000, '2020-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (15, 2, '2020-07-03 16:46:30.000000', NULL, NULL, 0.0000, '2020-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (16, 2, '2020-07-03 16:53:00.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (17, 2, '2020-07-03 17:06:23.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (18, 2, '2020-07-03 17:10:31.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (19, 2, '2020-07-03 17:35:06.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (20, 2, '2020-07-03 20:47:11.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21, 1, '2020-07-12 14:25:45.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (22, 1, '2020-07-14 19:54:35.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (23, 1, '2020-07-14 20:07:50.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (1605711780176, 23, '2020-11-18 23:03:00.968567', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21595947128478, 23, '2020-07-28 22:38:48.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21595947504818, 23, '2020-07-28 22:45:04.000000', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21596788202803, 23, '2020-08-07 16:16:58.595712', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21596877187370, 23, '2020-08-08 16:59:47.660311', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21596878878203, 23, '2020-08-08 17:27:58.328570', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21597297886851, 23, '2020-08-13 13:51:27.031687', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21602669563293, 23, '2020-10-14 17:59:24.379157', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21602737309784, 23, '2020-10-15 12:48:30.521155', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
INSERT INTO `SysUserFinance` VALUES (21604896186450, 23, '2020-11-09 12:29:46.676510', NULL, NULL, 0.0000, '2000-07-01 22:33:02.559');
