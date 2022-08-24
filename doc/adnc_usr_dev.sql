/*
SQLyog Community v13.1.6 (64 bit)
MySQL - 10.5.8-MariaDB-1:10.5.8+maria~focal : Database - adnc_usr_dev
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`adnc_usr_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `adnc_usr_dev`;

-- ----------------------------
-- Table structure for sysdept
-- ----------------------------
DROP TABLE IF EXISTS `sysdept`;
CREATE TABLE `sysdept`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `fullname` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `pid` bigint(20) NULL DEFAULT NULL,
  `pids` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `simplename` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `tips` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `version` int(11) NULL DEFAULT NULL,
  `ordinal` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '部门' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysdept
-- ----------------------------
INSERT INTO `sysdept` VALUES (1600000001000, 1600000000000, '2020-11-24 01:22:27.000000', 1600000000000, '2020-11-25 18:30:11.883723', '总公司', 0, '[0],', '总公司', NULL, NULL, 0);
INSERT INTO `sysdept` VALUES (1606155294001, 1600000000000, '2020-11-24 02:14:54.675010', 1600000000000, '2021-02-08 22:51:06.109324', '财务部', 1600000001000, '[0],[1600000001000],', '财务部', NULL, NULL, 2);
INSERT INTO `sysdept` VALUES (1606155335002, 1600000000000, '2020-11-24 02:15:35.476720', 1600000000000, '2021-02-08 22:46:38.866773', '研发部', 1600000001000, '[0],[1600000001000],', '研发部', NULL, NULL, 1);
INSERT INTO `sysdept` VALUES (1606155393003, 1600000000000, '2020-11-24 02:16:33.336059', 1600000000000, '2021-02-08 23:40:42.477252', 'csharp组', 1606155335002, '[0],[1600000001000],[1606155335002],', 'csharp组', NULL, NULL, 6);
INSERT INTO `sysdept` VALUES (1606155436004, 1600000000000, '2021-02-02 12:45:35.079665', 1600000000000, '2021-02-08 23:19:00.342879', 'go组', 1606155335002, '[0],[1600000001000],[1606155335002],', 'go组', NULL, NULL, 3);
INSERT INTO `sysdept` VALUES (1612796969001, 1600000000000, '2021-02-08 23:09:29.757253', 1600000000000, '2021-02-08 23:17:57.831845', '测试部', 1600000001000, '[0],[1600000001000],', '测试部', NULL, NULL, 1);
INSERT INTO `sysdept` VALUES (1612797557001, 1600000000000, '2021-02-08 23:19:17.938562', NULL, NULL, 'java组', 1606155335002, '[0],[1600000001000],[1606155335002],', 'java组', NULL, NULL, 1);
INSERT INTO `sysdept` VALUES (1616044463001, 1600000000000, '2021-03-18 13:14:23.793179', NULL, NULL, '云南知轮汽车科技有限公司', 1600000001000, '[0],[1600000001000],', '知轮科技', NULL, NULL, 1);

-- ----------------------------
-- Table structure for sysmenu
-- ----------------------------
DROP TABLE IF EXISTS `sysmenu`;
CREATE TABLE `sysmenu`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `code` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `component` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `hidden` bit(1) NULL DEFAULT NULL COMMENT '是否隐藏',
  `icon` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ismenu` bit(1) NOT NULL COMMENT '是否是菜单1:菜单,0:按钮',
  `isopen` bit(1) NULL DEFAULT NULL COMMENT '是否默认打开1:是,0:否',
  `levels` int(11) NOT NULL COMMENT '级别',
  `name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `pcode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `pcodes` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `status` bit(1) NOT NULL COMMENT '状态1:启用,0:禁用',
  `tips` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '鼠标悬停提示信息',
  `url` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ordinal` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_s37unj3gh67ujhk83lqva8i1t`(`code`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '菜单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysmenu
-- ----------------------------
INSERT INTO `sysmenu` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `code`, `component`, `hidden`, `icon`, `ismenu`, `isopen`, `levels`, `name`, `pcode`, `pcodes`, `status`, `tips`, `url`, `ordinal`) VALUES
	(1600000000001, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2021-02-08 23:40:04.403976', 'usr', 'layout', b'0', 'peoples', b'1', NULL, 1, '用户中心', '0', '[0],', b'1', NULL, '/usr', 0),
	(1600000000003, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2021-02-08 23:39:40.812082', 'maintain', 'layout', b'0', 'operation', b'1', NULL, 1, '运维中心', '0', '[0],', b'1', NULL, '/maintain', 1),
	(1600000000004, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-11-25 00:21:11.459114', 'user', 'views/usr/user/index', b'0', 'user', b'1', NULL, 2, '用户管理', 'usr', '[0],[usr]', b'1', NULL, '/user', 0),
	(1600000000005, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-11-25 00:21:23.738379', 'userAdd', NULL, b'0', '', b'0', NULL, 3, '添加用户', 'user', '[0],[usr][user]', b'1', NULL, '/user/add', 0),
	(1600000000006, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-11-25 00:21:44.038549', 'userEdit', NULL, b'0', '', b'0', NULL, 3, '修改用户', 'user', '[0],[usr][user]', b'1', NULL, '/user/edit', 0),
	(1600000000007, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'userDelete', NULL, b'0', NULL, b'0', b'0', 3, '删除用户', 'user', '[0],[usr],[user],', b'1', NULL, '/user/delete', 0),
	(1600000000008, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'userReset', NULL, b'0', NULL, b'0', b'0', 3, '重置密码', 'user', '[0],[usr],[user],', b'1', NULL, '/user/reset', 0),
	(1600000000009, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-11-25 00:21:48.244064', 'userFreeze', NULL, b'0', '', b'0', NULL, 3, '冻结用户', 'user', '[0],[usr][user]', b'1', NULL, '/user/freeze', 0),
	(1600000000010, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'userUnfreeze', NULL, b'0', NULL, b'0', b'0', 3, '解除冻结用户', 'user', '[0],[usr],[user],', b'1', NULL, '/user/unfreeze', 0),
	(1600000000011, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'userSetRole', NULL, b'0', NULL, b'0', b'0', 3, '分配角色', 'user', '[0],[usr],[user],', b'1', NULL, '/user/setRole', 0),
	(1600000000012, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'role', 'views/usr/role/index', b'0', 'people', b'1', NULL, 2, '角色管理', 'usr', '[0],[usr]', b'1', NULL, '/role', 0),
	(1600000000013, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'roleAdd', NULL, b'0', NULL, b'0', b'0', 3, '添加角色', 'role', '[0],[usr],[role],', b'1', NULL, '/role/add', 0),
	(1600000000014, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'roleEdit', NULL, b'0', NULL, b'0', b'0', 3, '修改角色', 'role', '[0],[usr],[role],', b'1', NULL, '/role/edit', 0),
	(1600000000015, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'roleDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除角色', 'role', '[0],[usr],[role]', b'1', NULL, '/role/delete', 0),
	(1600000000016, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'roleSetAuthority', NULL, b'0', NULL, b'0', b'0', 3, '配置权限', 'role', '[0],[usr],[role],', b'1', NULL, '/role/setAuthority', 0),
	(1600000000017, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'menu', 'views/usr/menu/index', b'0', 'menu', b'1', NULL, 2, '菜单管理', 'usr', '[0],[usr]', b'1', NULL, '/menu', 0),
	(1600000000018, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'menuAdd', NULL, b'0', NULL, b'0', b'0', 3, '添加菜单', 'menu', '[0],[usr],[menu],', b'1', NULL, '/menu/add', 0),
	(1600000000019, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'menuEdit', NULL, b'0', NULL, b'0', b'0', 3, '修改菜单', 'menu', '[0],[usr],[menu],', b'1', NULL, '/menu/edit', 0),
	(1600000000020, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'menuDelete', NULL, b'0', NULL, b'0', b'0', 3, '删除菜单', 'menu', '[0],[usr],[menu],', b'1', NULL, '/menu/remove', 0),
	(1600000000021, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'dept', 'views/usr/dept/index', b'0', 'dept', b'1', NULL, 2, '部门管理', 'usr', '[0],[usr],', b'1', NULL, '/dept', 0),
	(1600000000022, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'dict', 'views/maintain/dict/index', b'0', 'dict', b'1', NULL, 2, '字典管理', 'maintain', '[0],[maintain],', b'1', NULL, '/dict', 0),
	(1600000000023, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'deptEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改部门', 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/update', 0),
	(1600000000024, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'deptDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除部门', 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/delete', 0),
	(1600000000025, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2022-08-15 11:03:42.351551', 'dictAdd', NULL, b'0', '', b'0', NULL, 3, '添加字典', 'dict', '[0],[maintain],[dict]', b'1', NULL, '/dict/add', 0),
	(1600000000026, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'dictEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改字典', 'dict', '[0],[maintain],[dict],', b'1', NULL, '/dict/update', 0),
	(1600000000027, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'dictDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除字典', 'dict', '[0],[maintain],[dict],', b'1', NULL, '/dict/delete', 0),
	(1600000000028, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'deptList', NULL, b'0', NULL, b'0', NULL, 3, '部门列表', 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/list', 0),
	(1600000000030, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'dictList', NULL, b'0', NULL, b'0', NULL, 3, '字典列表', 'dict', '[0],[maintain],[dict],', b'1', NULL, '/dict/list', 0),
	(1600000000032, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'deptAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加部门', 'dept', '[0],[usr],[dept],', b'1', NULL, '/dept/add', 0),
	(1600000000033, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'cfg', 'views/maintain/cfg/index', b'0', 'cfg', b'1', NULL, 2, '参数管理', 'maintain', '[0],[maintain]', b'1', NULL, '/cfg', 0),
	(1600000000034, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'cfgAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加系统参数', 'cfg', '[0],[maintain],[cfg],', b'1', NULL, '/cfg/add', 0),
	(1600000000035, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'cfgEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改系统参数', 'cfg', '[0],[maintain],[cfg],', b'1', NULL, '/cfg/update', 0),
	(1600000000036, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'cfgDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除系统参数', 'cfg', '[0],[maintain],[cfg],', b'1', NULL, '/cfg/delete', 0),
	(1600000000037, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2022-08-15 11:04:10.021053', 'task', 'views/maintain/task/index', b'1', 'task', b'1', NULL, 2, '任务管理', 'maintain', '[0],[maintain]', b'0', NULL, '/task', 0),
	(1600000000038, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'taskAdd', NULL, b'0', NULL, b'0', NULL, 3, '添加任务', 'task', '[0],[maintain],[task],', b'1', NULL, '/task/add', 0),
	(1600000000039, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'taskEdit', NULL, b'0', NULL, b'0', NULL, 3, '修改任务', 'task', '[0],[maintain],[task],', b'1', NULL, '/task/update', 0),
	(1600000000040, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'taskDelete', NULL, b'0', NULL, b'0', NULL, 3, '删除任务', 'task', '[0],[maintain],[task],', b'1', NULL, '/task/delete', 0),
	(1600000000047, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'taskLog', 'views/maintain/task/taskLog', b'1', 'task', b'1', NULL, 3, '任务日志', 'task', '[0],[maintain],[task],', b'1', NULL, '/task/taskLog', 0),
	(1600000000048, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'opsLog', 'views/maintain/opslog/index', b'0', 'log', b'1', NULL, 2, '操作日志', 'maintain', '[0],[maintain]', b'1', NULL, '/opslog', 0),
	(1600000000049, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'loginLog', 'views/maintain/loginlog/index', b'0', 'logininfor', b'1', NULL, 2, '登录日志', 'maintain', '[0],[maintain]', b'1', NULL, '/loginlog', 0),
	(1600000000054, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2022-08-15 11:07:15.272200', 'druid', 'layout', b'0', 'link', b'1', NULL, 2, '性能检测', 'maintain', '[0],[maintain]', b'1', NULL, 'http://skywalking.aspdotnetcore.net', 0),
	(1600000000055, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-11-25 19:30:04.982175', 'swagger', 'views/maintain/swagger/index', b'0', 'swagger', b'1', NULL, 2, '接口文档', 'maintain', '[0],[maintain]', b'1', NULL, '/swagger', 0),
	(1600000000071, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2022-08-15 11:07:03.454784', 'nlogLog', 'layout', b'0', 'logininfor', b'1', NULL, 2, 'Nlog日志', 'maintain', '[0],[maintain]', b'1', NULL, 'http://loki.aspdotnetcore.net', 0),
	(1600000000072, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2022-08-15 11:05:35.278971', 'health', 'layout', b'0', 'monitor', b'1', NULL, 2, '健康检测', 'maintain', '[0],[maintain]', b'1', NULL, 'http://prometheus.aspdotnetcore.net', 0),
	(1600000000073, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'menuList', NULL, b'0', '', b'0', NULL, 3, '菜单列表', 'menu', '[0],[usr][menu]', b'1', NULL, '/menu/list', 0),
	(1600000000074, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2021-02-08 23:29:43.628024', 'roleList', NULL, b'0', '', b'0', NULL, 3, '角色列表', 'role', '[0],[usr][role]', b'1', NULL, '/role/list', 1),
	(1600000000075, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'userList', NULL, b'0', '', b'0', NULL, 3, '用户列表', 'user', '[0],[usr][user]', b'1', NULL, 'user/list', 0),
	(1600000000076, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'cfgList', NULL, b'0', '', b'0', NULL, 3, '系统参数列表', 'cfg', '[0],[maintain][cfg]', b'1', NULL, '/cfg/list', 0),
	(1600000000077, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2020-08-08 08:08:08.888888', 'taskList', NULL, b'0', NULL, b'0', NULL, 3, '任务列表', 'task', '[0],[maintain][task]', b'1', NULL, '/task/list', 0),
	(1600000000078, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2022-08-15 11:05:57.235325', 'eventBus', 'layout', b'0', 'server', b'1', NULL, 2, 'EventBus', 'maintain', '[0],[maintain]', b'1', NULL, 'http://114.132.157.167:8888/cus/cap/', 0),
	(324234353984645, 1600000000000, '2022-08-24 14:52:59.958191', NULL, NULL, 'cus', 'layout', b'1', 'wechat', b'1', NULL, 1, '客户中心', '0', '[0],', b'0', NULL, '/cus', 1),
	(324234861724805, 1600000000000, '2022-08-24 14:55:03.809432', 1600000000000, '2022-08-24 14:58:10.931952', 'customer', 'views/usr/user/index', b'1', 'peoples', b'1', NULL, 2, '客户管理', 'cus', '[0],[cus]', b'1', NULL, '/customer', 1),
	(324235389097093, 1600000000000, '2022-08-24 14:57:12.562108', 1600000000000, '2022-08-24 14:58:54.985811', 'customerList', NULL, b'0', '', b'0', NULL, 3, '客户列表', 'customer', '[0],[cus][customer]', b'1', NULL, '/customer/list', 1),
	(324236388873349, 1600000000000, '2022-08-24 15:01:16.647958', 1600000000000, '2022-08-24 15:02:54.797116', 'customerRecharge', '', b'0', 'contacts', b'0', NULL, 3, '充值', 'customer', '[0],[cus][customer]', b'1', NULL, '/customer/recharge', 1);
-- ----------------------------
-- Table structure for sysrelation
-- ----------------------------
DROP TABLE IF EXISTS `sysrelation`;
CREATE TABLE `sysrelation`  (
  `id` bigint(20) NOT NULL,
  `menuid` bigint(20) NULL DEFAULT NULL,
  `roleid` bigint(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `ix_sysrelation_roleid`(`roleid`) USING BTREE,
  INDEX `ix_sysrelation_menuid`(`menuid`) USING BTREE,
  CONSTRAINT `fk_sysrelation_sysmenu_menuid` FOREIGN KEY (`menuid`) REFERENCES `sysmenu` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `fk_sysrelation_sysrole_roleid` FOREIGN KEY (`roleid`) REFERENCES `sysrole` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '菜单角色关系' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysrelation
-- ----------------------------
INSERT INTO `sysrelation` VALUES (1606193510001, 1600000000001, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510002, 1600000000004, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510003, 1600000000005, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510004, 1600000000006, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510005, 1600000000007, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510006, 1600000000008, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510007, 1600000000009, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510008, 1600000000010, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510009, 1600000000011, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510010, 1600000000075, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510011, 1600000000012, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510012, 1600000000013, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510013, 1600000000014, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510014, 1600000000015, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510015, 1600000000016, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510016, 1600000000074, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510017, 1600000000017, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510018, 1600000000018, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510019, 1600000000019, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510020, 1600000000020, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510021, 1600000000073, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510022, 1600000000021, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510023, 1600000000023, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510024, 1600000000024, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510025, 1600000000028, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510026, 1600000000032, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510027, 1600000000003, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510028, 1600000000022, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510029, 1600000000025, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510030, 1600000000026, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510031, 1600000000027, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510032, 1600000000030, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510033, 1600000000033, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510034, 1600000000034, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510035, 1600000000035, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510036, 1600000000036, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510037, 1600000000076, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510038, 1600000000037, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510039, 1600000000038, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510040, 1600000000039, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510041, 1600000000040, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510042, 1600000000047, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510043, 1600000000077, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510044, 1600000000048, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510045, 1600000000049, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510046, 1600000000054, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510047, 1600000000055, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510048, 1600000000071, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510049, 1600000000072, 1600000000010);
INSERT INTO `sysrelation` VALUES (1606193510050, 1600000000078, 1600000000010);
INSERT INTO `sysrelation` VALUES (1607193510049, 324234353984645, 1600000000010);
INSERT INTO `sysrelation` VALUES (1607193510050, 324234861724805, 1600000000010);
INSERT INTO `sysrelation` VALUES (1607193510051, 324235389097093, 1600000000010);
INSERT INTO `sysrelation` VALUES (1607193510052, 324236388873349, 1600000000010);
INSERT INTO `sysrelation` VALUES (1610294626091, 1600000000003, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626092, 1600000000022, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626093, 1600000000025, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626094, 1600000000026, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626095, 1600000000027, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626096, 1600000000030, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626097, 1600000000033, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626098, 1600000000034, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626099, 1600000000035, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626100, 1600000000036, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626101, 1600000000076, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626102, 1600000000037, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626103, 1600000000038, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626104, 1600000000039, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626105, 1600000000040, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626106, 1600000000047, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626107, 1600000000077, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626108, 1600000000048, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626109, 1600000000049, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626110, 1600000000054, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626111, 1600000000055, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626112, 1600000000071, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626113, 1600000000072, 1606156061057);
INSERT INTO `sysrelation` VALUES (1610294626114, 1600000000078, 1606156061057);

-- ----------------------------
-- Table structure for sysrole
-- ----------------------------
DROP TABLE IF EXISTS `sysrole`;
CREATE TABLE `sysrole`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `deptid` bigint(20) NULL DEFAULT NULL,
  `name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `pid` bigint(20) NULL DEFAULT NULL,
  `tips` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `version` int(11) NULL DEFAULT NULL,
  `ordinal` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysrole
-- ----------------------------
INSERT INTO `sysrole` VALUES (1600000000010, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2021-02-08 23:39:16.907337', NULL, '系统管理员', NULL, 'administrator', NULL, 0);
INSERT INTO `sysrole` VALUES (1606156061057, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2021-02-08 22:44:29.701775', NULL, '只读用户', NULL, 'readonly', NULL, 1);
INSERT INTO `sysrole` VALUES (1615989759001, 1600000000000, '2020-08-08 08:08:08.888888', NULL, NULL, NULL, 'aaaa', NULL, 'bbbb', NULL, 1);

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `account` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `avatar` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `birthday` datetime(6) NULL DEFAULT NULL,
  `deptid` bigint(20) NULL DEFAULT NULL,
  `email` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `phone` varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `salt` varchar(6) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `sex` int(11) NULL DEFAULT NULL,
  `status` int(11) NULL DEFAULT NULL,
  `version` int(11) NULL DEFAULT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0,
  `roleids` varchar(72) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `fk_sysuser_sysdept_deptid`(`deptid`) USING BTREE,
  CONSTRAINT `fk_sysuser_sysdept_deptid` FOREIGN KEY (`deptid`) REFERENCES `sysdept` (`id`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '账号' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysuser
-- ----------------------------
INSERT INTO `sysuser` VALUES (1600000000000, 1600000000000, '2020-08-08 08:08:08.888888', 1600000000000, '2021-02-08 19:36:59.797137', 'alpha2008', NULL, '2020-11-04 00:00:00.000000', 1600000001000, 'alpha2008@tom.com', '余小猫', 'E2CD3261D6C9C4BCBBCC807CFF64417A', '18898658888', '2mh6e', 1, 1, NULL, 0, '1600000000010,1606156061057');
INSERT INTO `sysuser` VALUES (1606291099001, 1600000000000, '2020-11-25 15:58:20.255014', 1600000000000, '2021-02-08 23:41:01.034853', 'adncgo2', NULL, '2020-11-25 00:00:00.000000', 1606155393003, 'beta2009@tom.com', '余二猫', 'A9B7CDA2D9001025FC02C40AF6A80D4E', '18987656789', '880qx', 2, 1, NULL, 0, '1606156061057');
INSERT INTO `sysuser` VALUES (1606293242002, 1600000000000, '2020-11-25 16:34:03.074970', 1600000000000, '2021-02-08 23:20:03.098985', 'adncgo3', NULL, '2020-11-25 00:00:00.000000', 1606155436004, 'beta2009@tom.com', '余三猫', 'B273093C82A8E58C4E6E9673A8062092', '18898737334', 'p110y', 1, 1, NULL, 0, '1600000000010,1606156061057');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;