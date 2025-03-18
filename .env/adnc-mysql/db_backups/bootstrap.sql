-- --------------------------------------------------------
-- 主机:                           62.234.187.128
-- 服务器版本:                        10.5.8-MariaDB-1:10.5.8+maria~focal - mariadb.org binary distribution
-- 服务器操作系统:                      debian-linux-gnu
-- HeidiSQL 版本:                  12.1.0.6537
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 导出 adnc_admin_dev 的数据库结构
CREATE DATABASE IF NOT EXISTS `adnc_admin_dev` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `adnc_admin_dev`;

-- 导出  表 adnc_admin_dev.sys_config 结构
CREATE TABLE IF NOT EXISTS `sys_config` (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `key` varchar(64) NOT NULL COMMENT '参数键',
  `name` varchar(64) NOT NULL COMMENT '参数名',
  `value` varchar(128) NOT NULL COMMENT '参数值',
  `remark` varchar(128) NOT NULL COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='系统参数';

-- 正在导出表  adnc_admin_dev.sys_config 的数据：~2 rows (大约)
INSERT INTO `sys_config` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `key`, `name`, `value`, `remark`) VALUES
	(654337157616325, 653335112912901, '2025-03-14 09:24:44.641988', 653335112912901, '2025-03-14 09:24:44.642019', 'weixin-key', '微信接口Key', '343sfsdfas', '微信接口配置');

-- 导出  表 adnc_admin_dev.sys_dictionary 结构
CREATE TABLE IF NOT EXISTS `sys_dictionary` (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `code` varchar(32) NOT NULL,
  `name` varchar(32) NOT NULL,
  `remark` varchar(128) NOT NULL,
  `status` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='字典';

-- 正在导出表  adnc_admin_dev.sys_dictionary 的数据：~3 rows (大约)
INSERT INTO `sys_dictionary` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `code`, `name`, `remark`, `status`) VALUES
	(654231800692741, 653335112912901, '2025-03-14 02:16:02.842381', 653335112912901, '2025-03-14 02:16:02.847194', 'notice_type', '通知类型', '', 1),
	(654240219889733, 653335112912901, '2025-03-14 02:50:18.256197', 653335112912901, '2025-03-14 02:50:50.768281', 'notice_level', '通知级别', '', 1);

-- 导出  表 adnc_admin_dev.sys_dictionary_data 结构
CREATE TABLE IF NOT EXISTS `sys_dictionary_data` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `dictcode` varchar(32) NOT NULL,
  `label` varchar(32) NOT NULL,
  `value` varchar(32) NOT NULL,
  `tagtype` varchar(32) NOT NULL,
  `status` tinyint(1) NOT NULL,
  `ordinal` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=654241459761222 DEFAULT CHARSET=utf8mb4 COMMENT='字典数据';

-- 正在导出表  adnc_admin_dev.sys_dictionary_data 的数据：~9 rows (大约)
INSERT INTO `sys_dictionary_data` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `dictcode`, `label`, `value`, `tagtype`, `status`, `ordinal`) VALUES
	(654239258492997, 653335112912901, '2025-03-14 02:46:23.753258', 653335112912901, '2025-03-14 02:46:23.764096', 'notice_type', '系统升级', '1', 'success', 1, 1),
	(654239345217605, 653335112912901, '2025-03-14 02:46:44.662244', 653335112912901, '2025-03-14 02:46:44.662315', 'notice_type', '系统维护', '2', 'warning', 1, 2),
	(654239670661189, 653335112912901, '2025-03-14 02:48:04.130043', 653335112912901, '2025-03-14 02:48:17.928995', 'notice_type', '安全警告', '3', 'info', 1, 3),
	(654239799701573, 653335112912901, '2025-03-14 02:48:35.619266', 653335112912901, '2025-03-14 02:48:35.619300', 'notice_type', '假期通知', '4', 'primary', 1, 4),
	(654239873306693, 653335112912901, '2025-03-14 02:48:53.589871', 653335112912901, '2025-03-14 02:48:53.589908', 'notice_type', '公司新闻', '5', 'danger', 1, 5),
	(654240025727045, 653335112912901, '2025-03-14 02:49:30.801847', 653335112912901, '2025-03-14 02:49:47.775236', 'notice_type', '其他', '7', 'info', 1, 99),
	(654241282986053, 653335112912901, '2025-03-14 02:54:37.749547', 653335112912901, '2025-03-14 02:54:37.749570', 'notice_level', '高', 'H', 'danger', 1, 1),
	(654241329270853, 653335112912901, '2025-03-14 02:54:49.049727', 653335112912901, '2025-03-14 02:55:31.410027', 'notice_level', '中', 'M', 'primary', 1, 2),
	(654241459761221, 653335112912901, '2025-03-14 02:55:20.908143', 653335112912901, '2025-03-14 02:55:40.136094', 'notice_level', '低', 'L', 'info', 1, 3);

-- 导出  表 adnc_admin_dev.sys_eventtracker 结构
CREATE TABLE IF NOT EXISTS `sys_eventtracker` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `eventid` bigint(20) NOT NULL,
  `trackername` varchar(50) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_sys_eventtracker_eventid_trackername` (`eventid`,`trackername`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='事件跟踪/处理信息';

-- 正在导出表  adnc_admin_dev.sys_eventtracker 的数据：~0 rows (大约)

-- 导出  表 adnc_admin_dev.sys_menu 结构
CREATE TABLE IF NOT EXISTS `sys_menu` (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `parentid` bigint(20) NOT NULL COMMENT '父菜单Id',
  `parentids` varchar(128) NOT NULL COMMENT '父菜单Id组合',
  `name` varchar(32) NOT NULL COMMENT '名称',
  `perm` varchar(32) NOT NULL COMMENT '权限编码',
  `routename` varchar(64) NOT NULL COMMENT '路由名称',
  `routepath` varchar(64) NOT NULL COMMENT '路由路径',
  `type` varchar(16) NOT NULL COMMENT '菜单类型',
  `component` varchar(64) NOT NULL COMMENT '組件配置',
  `visible` tinyint(1) NOT NULL COMMENT '是否显示',
  `redirect` varchar(128) NOT NULL COMMENT '跳转路由路径',
  `icon` varchar(32) NOT NULL COMMENT '图标',
  `keepalive` tinyint(1) NOT NULL COMMENT '是否开启页面缓存',
  `alwaysshow` tinyint(1) NOT NULL COMMENT '只有一个子路由是否始终显示',
  `params` varchar(128) NOT NULL COMMENT '路由参数',
  `ordinal` int(11) NOT NULL COMMENT '序号',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='菜单';

-- 正在导出表  adnc_admin_dev.sys_menu 的数据：~45 rows (大约)
INSERT INTO `sys_menu` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `parentid`, `parentids`, `name`, `perm`, `routename`, `routepath`, `type`, `component`, `visible`, `redirect`, `icon`, `keepalive`, `alwaysshow`, `params`, `ordinal`) VALUES
	(653342080185029, 1000000000000, '2025-03-11 13:55:46.028479', 653337659433029, '2025-03-14 16:30:12.233442', 0, '[0]', '系统管理', '', '', '/system', 'CATALOG', 'Layout', 1, '/system/user', 'role', 1, 1, '', 1),
	(653342584296133, 1000000000000, '2025-03-11 13:57:49.016162', 653335112912901, '2025-03-14 09:10:34.425720', 653342080185029, '[0][653342080185029]', '菜单管理', '', 'Menu', 'menu', 'MENU', 'system/menu/index', 1, '', 'menu', 1, 0, '', 4),
	(653343418946245, 1000000000000, '2025-03-11 14:01:12.781334', 653335112912901, '2025-03-12 02:18:57.594467', 653342584296133, '[0][653342080185029][653342584296133]', '菜单新增', 'menu-create', '', '', 'BUTTON', '', 1, '', '', 1, 1, '', 1),
	(653389651534725, 653335112912901, '2025-03-11 17:09:20.259624', 653335112912901, '2025-03-12 02:18:51.063178', 653342584296133, '[0][653342080185029][653342584296133]', '菜单修改', 'menu-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '111=111', 2),
	(653524636169093, 653335112912901, '2025-03-12 02:18:35.485594', 653335112912901, '2025-03-12 02:18:35.490303', 653342584296133, '[0][653342080185029][653342584296133]', '菜单删除', 'menu-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3),
	(653525628846981, 653335112912901, '2025-03-12 02:22:37.641550', 653335112912901, '2025-03-12 17:18:54.181204', 653342080185029, '[0][653342080185029]', '角色管理', '', 'Role', 'role', 'MENU', 'system/role/index', 1, '', 'el-icon-Trophy', 1, 0, '', 2),
	(653525880882053, 653335112912901, '2025-03-12 02:23:39.176354', 653335112912901, '2025-03-12 02:23:39.176387', 653525628846981, '[0][653342080185029][653525628846981]', '角色新增', 'role-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(653525982651269, 653335112912901, '2025-03-12 02:24:04.021653', 653335112912901, '2025-03-12 02:24:04.021694', 653525628846981, '[0][653342080185029][653525628846981]', '角色修改', 'role-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2),
	(653526164857733, 653335112912901, '2025-03-12 02:24:48.546878', 653335112912901, '2025-03-12 02:24:48.546926', 653525628846981, '[0][653342080185029][653525628846981]', '角色删除', 'role-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3),
	(653526703580037, 653335112912901, '2025-03-12 02:27:00.032542', 653335112912901, '2025-03-15 01:31:28.153432', 653525628846981, '[0][653342080185029][653525628846981]', '角色查询', 'role-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4),
	(653688386045317, 653335112912901, '2025-03-12 13:24:53.568100', 653335112912901, '2025-03-12 13:24:53.572926', 653342080185029, '[0][653342080185029]', '机构管理', '', 'Dept', 'dept', 'MENU', 'system/dept/index', 1, '', 'el-icon-CoffeeCup', 1, 0, '', 1),
	(653688962147717, 653335112912901, '2025-03-12 13:27:13.929212', 653335112912901, '2025-03-12 13:27:13.929284', 653688386045317, '[0][653342080185029][653688386045317]', '机构新增', 'org-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(653689048724869, 653335112912901, '2025-03-12 13:27:35.065392', 653335112912901, '2025-03-12 13:29:55.810715', 653688386045317, '[0][653342080185029][653688386045317]', '机构修改', 'org-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2),
	(653689299236229, 653335112912901, '2025-03-12 13:28:36.228932', 653335112912901, '2025-03-12 13:30:04.669707', 653688386045317, '[0][653342080185029][653688386045317]', '机构删除', 'org-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3),
	(653689402680709, 653335112912901, '2025-03-12 13:29:01.481851', 653335112912901, '2025-03-15 01:30:38.237695', 653688386045317, '[0][653342080185029][653688386045317]', '机构查询', 'org-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4),
	(653689923114373, 653335112912901, '2025-03-12 13:31:08.537851', 653335112912901, '2025-03-15 01:34:27.281727', 653342584296133, '[0][653342080185029][653342584296133]', '菜单查询', 'menu-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4),
	(653745302407109, 653335112912901, '2025-03-12 17:16:29.199235', 653335112912901, '2025-03-12 17:18:47.314183', 653342080185029, '[0][653342080185029]', '用户管理', '', 'User', 'user', 'MENU', 'system/user/index', 1, '', 'el-icon-Avatar', 1, 0, '', 3),
	(653745429395397, 653335112912901, '2025-03-12 17:16:59.877507', 653335112912901, '2025-03-12 17:16:59.877621', 653745302407109, '[0][653342080185029][653745302407109]', '用户新增', 'user-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(653745520240581, 653335112912901, '2025-03-12 17:17:22.056285', 653335112912901, '2025-03-12 17:17:22.056480', 653745302407109, '[0][653342080185029][653745302407109]', '用户修改', 'user-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2),
	(653756020922437, 653335112912901, '2025-03-12 18:00:05.906431', 653335112912901, '2025-03-15 01:32:11.120967', 653745302407109, '[0][653342080185029][653745302407109]', '用户查询', 'user-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4),
	(653787841784005, 653335112912901, '2025-03-12 20:09:34.640689', 653335112912901, '2025-03-12 20:09:34.644133', 653745302407109, '[0][653342080185029][653745302407109]', '重置密码', 'user-reset-password', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 5),
	(653804678302149, 653335112912901, '2025-03-12 21:18:05.045862', 653335112912901, '2025-03-12 21:18:05.050174', 653745302407109, '[0][653342080185029][653745302407109]', '用户删除', 'user-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 6),
	(653805800937925, 653335112912901, '2025-03-12 21:22:39.031803', 653335112912901, '2025-03-12 21:22:39.031846', 653745302407109, '[0][653342080185029][653745302407109]', '用户导入', 'user-import', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 7),
	(653805917993413, 653335112912901, '2025-03-12 21:23:07.610400', 653335112912901, '2025-03-12 21:23:07.610542', 653745302407109, '[0][653342080185029][653745302407109]', '用户导出', 'user-export', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 8),
	(654225897013253, 653335112912901, '2025-03-14 01:52:01.907929', 653335112912901, '2025-03-14 01:53:03.674597', 653342080185029, '[0][653342080185029]', '字典管理', '', 'Dict', 'dict', 'MENU', 'system/dict/index', 1, '', 'el-icon-Discount', 1, 0, '', 5),
	(654226073489413, 653335112912901, '2025-03-14 01:52:44.618434', 653335112912901, '2025-03-14 01:52:44.618516', 654225897013253, '[0][654225897013253]', '字典新增', 'dict-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(654226273665029, 653335112912901, '2025-03-14 01:53:33.486091', 653335112912901, '2025-03-14 01:54:08.179769', 654225897013253, '[0][653342080185029][654225897013253]', '字典修改', 'dict-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2),
	(654226371764229, 653335112912901, '2025-03-14 01:53:57.431053', 653335112912901, '2025-03-14 01:53:57.431184', 654225897013253, '[0][653342080185029][654225897013253]', '字典删除', 'dict-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3),
	(654226547773445, 653335112912901, '2025-03-14 01:54:40.402966', 653335112912901, '2025-03-15 01:34:59.384276', 654225897013253, '[0][653342080185029][654225897013253]', '字典查询', 'dict-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4),
	(654227048943621, 653335112912901, '2025-03-14 01:56:42.757506', 653335112912901, '2025-03-14 03:00:46.258079', 653342080185029, '[0][653342080185029]', '字典数据', '', 'DictData', 'dict-data', 'MENU', 'system/dict/data', 0, '', 'el-icon-CollectionTag', 1, 0, '', 6),
	(654227246395397, 653335112912901, '2025-03-14 01:57:30.967744', 653335112912901, '2025-03-14 01:57:30.967803', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据新增', 'dictdata-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(654227348602885, 653335112912901, '2025-03-14 01:57:55.923311', 653335112912901, '2025-03-14 01:57:55.923356', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据修改', 'dictdata-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2),
	(654227455537157, 653335112912901, '2025-03-14 01:58:22.024924', 653335112912901, '2025-03-14 01:58:22.024962', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据删除', 'dictdata-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3),
	(654227565006853, 653335112912901, '2025-03-14 01:58:48.754711', 653335112912901, '2025-03-15 01:35:45.770369', 654227048943621, '[0][653342080185029][654227048943621]', '字典数据查询', 'dictdata-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4),
	(654243405697093, 653335112912901, '2025-03-14 03:03:16.107668', 653335112912901, '2025-03-14 03:03:16.107695', 653342080185029, '[0][653342080185029]', '系统配置', '', 'Config', 'config', 'MENU', 'system/config/index', 1, '', 'el-icon-Setting', 1, 0, '', 7),
	(654332804104837, 653335112912901, '2025-03-14 09:07:02.281041', 653335112912901, '2025-03-14 09:07:02.289492', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置新增', 'sysconfig-create', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(654332952756869, 653335112912901, '2025-03-14 09:07:38.205504', 653335112912901, '2025-03-14 09:07:47.036752', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置修改', 'sysconfig-update', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 2),
	(654333157950085, 653335112912901, '2025-03-14 09:08:28.289495', 653335112912901, '2025-03-14 09:08:28.289582', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置删除', 'sysconfig-delete', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 3),
	(654333310177925, 653335112912901, '2025-03-14 09:09:05.451614', 653335112912901, '2025-03-15 01:36:15.355682', 654243405697093, '[0][653342080185029][654243405697093]', '系统配置查询', 'sysconfig-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 4),
	(654442132821701, 653337659433029, '2025-03-14 16:31:53.539063', 653335112912901, '2025-03-14 17:17:17.138145', 0, '[0]', '运维管理', '', '', '/maint', 'CATALOG', 'Layout', 1, '', 'el-icon-Opportunity', 1, 1, '', 2),
	(654442722764485, 653337659433029, '2025-03-14 16:34:17.507347', 653337659433029, '2025-03-14 16:34:17.507427', 654442132821701, '[0][654442132821701]', '操作日志', '', 'OperateLog', 'operatelog', 'MENU', 'maint/log/index', 1, '', 'el-icon-Search', 1, 0, '', 1),
	(654455068768133, 653335112912901, '2025-03-14 17:24:32.167647', 653335112912901, '2025-03-15 01:36:42.106666', 654442722764485, '[0][654442132821701][654442722764485]', '操作日志查询', 'operationlog-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(654523403131077, 653335112912901, '2025-03-14 22:02:35.101176', 653335112912901, '2025-03-14 22:02:35.112004', 654442132821701, '[0][654442132821701]', '登录日志', '', 'LoginLog', 'loginlog', 'MENU', 'maint/log/loginlog', 1, '', 'el-icon-ChatLineRound', 1, 0, '', 1),
	(654523795179717, 653335112912901, '2025-03-14 22:04:10.598917', 653335112912901, '2025-03-15 01:37:04.449633', 654523403131077, '[0][654442132821701][654523403131077]', '登录日志查询', 'loginlog-search', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 1),
	(654788688007877, 653335112912901, '2025-03-15 16:02:03.031386', 653335112912901, '2025-03-15 16:02:03.039044', 653525628846981, '[0][653342080185029][653525628846981]', '角色权限设置', 'role-setperms', '', '', 'BUTTON', '', 1, '', '', 1, 0, '', 5);

-- 导出  表 adnc_admin_dev.sys_organization 结构
CREATE TABLE IF NOT EXISTS `sys_organization` (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `parentid` bigint(20) NOT NULL,
  `parentids` varchar(128) NOT NULL,
  `code` varchar(16) NOT NULL,
  `name` varchar(32) NOT NULL,
  `status` tinyint(1) NOT NULL,
  `ordinal` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='部门';

-- 正在导出表  adnc_admin_dev.sys_organization 的数据：~7 rows (大约)
INSERT INTO `sys_organization` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `parentid`, `parentids`, `code`, `name`, `status`, `ordinal`) VALUES
	(653725132137989, 653335112912901, '2025-03-12 15:54:24.703512', 653335112912901, '2025-03-12 15:54:24.709316', 0, '[0]', 'microsoft', '微软中国有限公司', 1, 1),
	(653726253618757, 653335112912901, '2025-03-12 15:58:58.512981', 653335112912901, '2025-03-12 15:58:58.517780', 653725132137989, '[0]', 'office', 'office事业部', 1, 1),
	(653726372808261, 653335112912901, '2025-03-12 15:59:27.369192', 653335112912901, '2025-03-12 15:59:27.369244', 653725132137989, '[0]', 'database', '数据库事业部', 1, 2),
	(653726516561477, 653335112912901, '2025-03-12 16:00:02.464592', 653335112912901, '2025-03-12 16:10:30.737262', 653725132137989, '[0]', 'tools', '开发工具事业部', 1, 3),
	(653729175238277, 653335112912901, '2025-03-12 16:10:51.633050', 653335112912901, '2025-03-12 16:10:51.633839', 653726516561477, '[0]', 'vscode', 'vscode', 1, 1),
	(653729291642501, 653335112912901, '2025-03-12 16:11:19.988466', 653335112912901, '2025-03-13 13:59:53.811160', 653726516561477, '[0]', 'visualstudio', 'visualstudio', 0, 3);

-- 导出  表 adnc_admin_dev.sys_role 结构
CREATE TABLE IF NOT EXISTS `sys_role` (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `name` varchar(32) NOT NULL,
  `code` varchar(32) NOT NULL,
  `datascope` int(11) NOT NULL,
  `status` tinyint(1) NOT NULL,
  `ordinal` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='角色';

-- 正在导出表  adnc_admin_dev.sys_role 的数据：~3 rows (大约)
INSERT INTO `sys_role` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `name`, `code`, `datascope`, `status`, `ordinal`) VALUES
	(653344679641925, 1000000000000, '2025-03-11 14:06:20.618864', 653335112912901, '2025-03-12 11:45:34.554147', '系统管理员', 'administrator', 0, 1, 1),
	(653682250118405, 653335112912901, '2025-03-12 12:59:55.116604', 653335112912901, '2025-03-15 01:41:00.311004', '访问者', 'customer', 2, 1, 1);

-- 导出  表 adnc_admin_dev.sys_role_menu_relation 结构
CREATE TABLE IF NOT EXISTS `sys_role_menu_relation` (
  `id` bigint(20) NOT NULL,
  `menuid` bigint(20) NOT NULL,
  `roleid` bigint(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='菜单角色关系';

-- 正在导出表  adnc_admin_dev.sys_role_menu_relation 的数据：~50 rows (大约)
INSERT INTO `sys_role_menu_relation` (`id`, `menuid`, `roleid`) VALUES
	(654524019480773, 653342080185029, 653344679641925),
	(654524019525829, 653688386045317, 653344679641925),
	(654524019525830, 653688962147717, 653344679641925),
	(654524019525831, 653689048724869, 653344679641925),
	(654524019525832, 653689299236229, 653344679641925),
	(654524019525833, 653689402680709, 653344679641925),
	(654524019525834, 653525628846981, 653344679641925),
	(654524019525835, 653525880882053, 653344679641925),
	(654524019525836, 653525982651269, 653344679641925),
	(654524019525837, 653526164857733, 653344679641925),
	(654524019525838, 653526703580037, 653344679641925),
	(654524019529925, 653745302407109, 653344679641925),
	(654524019529926, 653745429395397, 653344679641925),
	(654524019529927, 653745520240581, 653344679641925),
	(654524019529928, 653756020922437, 653344679641925),
	(654524019529929, 653787841784005, 653344679641925),
	(654524019529930, 653804678302149, 653344679641925),
	(654524019529931, 653805800937925, 653344679641925),
	(654524019529932, 653805917993413, 653344679641925),
	(654524019529933, 653342584296133, 653344679641925),
	(654524019529934, 653343418946245, 653344679641925),
	(654524019529935, 653389651534725, 653344679641925),
	(654524019529936, 653524636169093, 653344679641925),
	(654524019529937, 653689923114373, 653344679641925),
	(654524019529938, 654225897013253, 653344679641925),
	(654524019529939, 654226073489413, 653344679641925),
	(654524019529940, 654226273665029, 653344679641925),
	(654524019529941, 654226371764229, 653344679641925),
	(654524019529942, 654226547773445, 653344679641925),
	(654524019529943, 654227048943621, 653344679641925),
	(654524019529944, 654227246395397, 653344679641925),
	(654524019529945, 654227348602885, 653344679641925),
	(654524019529946, 654227455537157, 653344679641925),
	(654524019529947, 654227565006853, 653344679641925),
	(654524019529948, 654243405697093, 653344679641925),
	(654524019529949, 654332804104837, 653344679641925),
	(654524019529950, 654332952756869, 653344679641925),
	(654524019529951, 654333157950085, 653344679641925),
	(654524019529952, 654333310177925, 653344679641925),
	(654524019529953, 654442132821701, 653344679641925),
	(654524019529954, 654442722764485, 653344679641925),
	(654524019529955, 654455068768133, 653344679641925),
	(654524019529956, 654523403131077, 653344679641925),
	(654524019529957, 654523795179717, 653344679641925),
	(654788688007870, 654788688007877, 653344679641925),
	(654804815708229, 654442132821701, 653682250118405),
	(654804816343109, 654442722764485, 653682250118405),
	(654804816347205, 654455068768133, 653682250118405),
	(654804816347206, 654523403131077, 653682250118405),
	(654804816347207, 654523795179717, 653682250118405);

-- 导出  表 adnc_admin_dev.sys_role_user_relation 结构
CREATE TABLE IF NOT EXISTS `sys_role_user_relation` (
  `id` bigint(20) NOT NULL,
  `userid` bigint(20) NOT NULL,
  `roleid` bigint(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户角色关系';

-- 正在导出表  adnc_admin_dev.sys_role_user_relation 的数据：~2 rows (大约)
INSERT INTO `sys_role_user_relation` (`id`, `userid`, `roleid`) VALUES
	(654766446428229, 653337659433029, 653682250118405),
	(654772338585669, 653335112912901, 653344679641925);

-- 导出  表 adnc_admin_dev.sys_user 结构
CREATE TABLE IF NOT EXISTS `sys_user` (
  `id` bigint(20) NOT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '删除标识',
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `account` varchar(32) NOT NULL COMMENT '账号',
  `avatar` varchar(128) NOT NULL COMMENT '头像路径',
  `birthday` datetime(6) DEFAULT NULL COMMENT '生日',
  `deptid` bigint(20) NOT NULL COMMENT '部门Id',
  `email` varchar(32) NOT NULL COMMENT 'email',
  `name` varchar(32) NOT NULL COMMENT '姓名',
  `password` varchar(32) NOT NULL COMMENT '密码',
  `mobile` varchar(11) NOT NULL COMMENT '手机号',
  `salt` varchar(6) NOT NULL COMMENT '密码盐',
  `gender` int(11) NOT NULL COMMENT '性别',
  `status` tinyint(1) NOT NULL COMMENT '状态',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='管理员';

-- 正在导出表  adnc_admin_dev.sys_user 的数据：~5 rows (大约)
INSERT INTO `sys_user` (`id`, `isdeleted`, `createby`, `createtime`, `modifyby`, `modifytime`, `account`, `avatar`, `birthday`, `deptid`, `email`, `name`, `password`, `mobile`, `salt`, `gender`, `status`) VALUES
	(653335112912901, 0, 1000000000000, '2025-03-11 13:27:25.038287', 653335112912901, '2025-03-15 15:03:36.000000', 'alpha2008', '', '2025-03-11 14:11:08.062000', 653725132137989, 'user@example.com', '余大猫', '3B6791E9AB14DFF278A3C2AF4B97F1E9', '', '846vm', 1, 1),
	(653337659433029, 0, 1000000000000, '2025-03-11 13:37:46.724327', 653335112912901, '2025-03-14 11:17:44.000000', 'alpha2009', '', '2025-03-11 13:24:22.360000', 653726253618757, 'user@example.com', '余大猫', 'DE325BC108FD5B42D45500C3720F6628', '19964946688', '7vkvf', 1, 1),
	(653838533808773, 1, 653335112912901, '2025-03-12 23:35:50.884954', 653335112912901, '2025-03-12 23:36:58.000000', 'test1', '', NULL, 653725132137989, 'alphacn@foxmail.com', '测试用户', '86A961C57306BAC569B9FBB4EC6BA638', '', 'xfgiq', 0, 1),
	(653838713631365, 1, 653335112912901, '2025-03-12 23:36:34.421153', 653335112912901, '2025-03-12 23:36:58.000000', 'test2', '', NULL, 653726253618757, 'alphacn@foxmail.com', '测试用户2', '0C54EFB2BAF6F1A13E2F39195D6AA8EB', '18898666555', 'so32e', 2, 1),
	(653838923670149, 1, 653335112912901, '2025-03-12 23:37:25.701911', 653335112912901, '2025-03-12 23:37:29.000000', 'test1', '', NULL, 653726253618757, 'alphacn@foxmail.com', '测试用户1', '2A04D26E4CA62451402A1AC6F7515667', '', '3j8mi', 0, 0);

-- 导出  表 adnc_admin_dev.__efmigrationshistory 结构
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `migrationid` varchar(150) NOT NULL,
  `productversion` varchar(32) NOT NULL,
  PRIMARY KEY (`migrationid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 正在导出表  adnc_admin_dev.__efmigrationshistory 的数据：~8 rows (大约)
INSERT INTO `__efmigrationshistory` (`migrationid`, `productversion`) VALUES
	('20250311045441_Init20250311', '8.0.13'),
	('20250312052404_Update-2025031201', '8.0.13'),
	('20250312122420_Update-2025031202', '8.0.13'),
	('20250312140312_Update-2025031203', '8.0.13'),
	('20250313164005_Update2025031401', '8.0.13'),
	('20250316133304_Update2025031601', '8.0.13'),
	('20250317152859_Init20250317', '8.0.13'),
	('20250317153239_Update2025031702', '8.0.13');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
-- --------------------------------------------------------
-- 主机:                           62.234.187.128
-- 服务器版本:                        10.5.8-MariaDB-1:10.5.8+maria~focal - mariadb.org binary distribution
-- 服务器操作系统:                      debian-linux-gnu
-- HeidiSQL 版本:                  12.1.0.6537
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 导出 adnc_cust_dev 的数据库结构
CREATE DATABASE IF NOT EXISTS `adnc_cust_dev` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `adnc_cust_dev`;

-- 导出  表 adnc_cust_dev.cust_customer 结构
CREATE TABLE IF NOT EXISTS `cust_customer` (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `account` varchar(16) NOT NULL,
  `password` varchar(32) NOT NULL,
  `nickname` varchar(16) NOT NULL,
  `realname` varchar(16) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户表';

-- 正在导出表  adnc_cust_dev.cust_customer 的数据：~0 rows (大约)

-- 导出  表 adnc_cust_dev.cust_eventtracker 结构
CREATE TABLE IF NOT EXISTS `cust_eventtracker` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `eventid` bigint(20) NOT NULL,
  `trackername` varchar(50) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_cust_eventtracker_eventid_trackername` (`eventid`,`trackername`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='事件跟踪/处理信息';

-- 正在导出表  adnc_cust_dev.cust_eventtracker 的数据：~0 rows (大约)

-- 导出  表 adnc_cust_dev.cust_finance 结构
CREATE TABLE IF NOT EXISTS `cust_finance` (
  `id` bigint(20) NOT NULL,
  `rowversion` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  `account` varchar(16) NOT NULL,
  `balance` decimal(18,4) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_cust_finance_cust_customer_id` FOREIGN KEY (`id`) REFERENCES `cust_customer` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户财务表';

-- 正在导出表  adnc_cust_dev.cust_finance 的数据：~0 rows (大约)

-- 导出  表 adnc_cust_dev.cust_transactionlog 结构
CREATE TABLE IF NOT EXISTS `cust_transactionlog` (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `customerid` bigint(20) NOT NULL,
  `account` varchar(16) NOT NULL,
  `exchangetype` int(11) NOT NULL,
  `exchagestatus` int(11) NOT NULL,
  `changingamount` decimal(18,4) NOT NULL,
  `amount` decimal(18,4) NOT NULL,
  `changedamount` decimal(18,4) NOT NULL,
  `remark` varchar(64) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_cust_transactionlog_customerid` (`customerid`),
  CONSTRAINT `fk_cust_transactionlog_cust_customer_customerid` FOREIGN KEY (`customerid`) REFERENCES `cust_customer` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户财务变动记录';

-- 正在导出表  adnc_cust_dev.cust_transactionlog 的数据：~0 rows (大约)

-- 导出  表 adnc_cust_dev.__efmigrationshistory 结构
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `migrationid` varchar(150) NOT NULL,
  `productversion` varchar(32) NOT NULL,
  PRIMARY KEY (`migrationid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 正在导出表  adnc_cust_dev.__efmigrationshistory 的数据：~1 rows (大约)
INSERT INTO `__efmigrationshistory` (`migrationid`, `productversion`) VALUES
	('20250317152707_Init20250317', '8.0.13');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
-- --------------------------------------------------------
-- 主机:                           62.234.187.128
-- 服务器版本:                        10.5.8-MariaDB-1:10.5.8+maria~focal - mariadb.org binary distribution
-- 服务器操作系统:                      debian-linux-gnu
-- HeidiSQL 版本:                  12.1.0.6537
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 导出 adnc_maint_dev 的数据库结构
CREATE DATABASE IF NOT EXISTS `adnc_maint_dev` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `adnc_maint_dev`;

-- 导出  表 adnc_maint_dev.sys_eventtracker 结构
CREATE TABLE IF NOT EXISTS `sys_eventtracker` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `eventid` bigint(20) NOT NULL,
  `trackername` varchar(50) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_sys_eventtracker_eventid_trackername` (`eventid`,`trackername`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='事件跟踪/处理信息';

-- 正在导出表  adnc_maint_dev.sys_eventtracker 的数据：~0 rows (大约)

-- 导出  表 adnc_maint_dev.__efmigrationshistory 结构
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `migrationid` varchar(150) NOT NULL,
  `productversion` varchar(32) NOT NULL,
  PRIMARY KEY (`migrationid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 正在导出表  adnc_maint_dev.__efmigrationshistory 的数据：~1 rows (大约)
INSERT INTO `__efmigrationshistory` (`migrationid`, `productversion`) VALUES
	('20250317152538_Init20250317', '8.0.13');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
-- --------------------------------------------------------
-- 主机:                           62.234.187.128
-- 服务器版本:                        10.5.8-MariaDB-1:10.5.8+maria~focal - mariadb.org binary distribution
-- 服务器操作系统:                      debian-linux-gnu
-- HeidiSQL 版本:                  12.1.0.6537
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 导出 adnc_syslog_dev 的数据库结构
CREATE DATABASE IF NOT EXISTS `adnc_syslog_dev` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `adnc_syslog_dev`;

-- 导出  表 adnc_syslog_dev.login_log 结构
CREATE TABLE IF NOT EXISTS `login_log` (
  `Id` bigint(20) NOT NULL,
  `Device` varchar(32) NOT NULL,
  `Message` varchar(128) NOT NULL,
  `Succeed` tinyint(1) NOT NULL,
  `StatusCode` int(11) NOT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  `Account` varchar(32) NOT NULL,
  `Name` varchar(32) NOT NULL,
  `RemoteIpAddress` varchar(16) NOT NULL,
  `ExecutionTime` int(11) NOT NULL DEFAULT 0,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 数据导出被取消选择。

-- 导出  表 adnc_syslog_dev.operation_log 结构
CREATE TABLE IF NOT EXISTS `operation_log` (
  `Id` bigint(20) NOT NULL,
  `ClassName` varchar(255) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `LogName` varchar(64) NOT NULL,
  `LogType` varchar(32) NOT NULL,
  `Message` varchar(1024) NOT NULL,
  `Method` varchar(64) NOT NULL,
  `Succeed` tinyint(1) NOT NULL,
  `UserId` bigint(20) NOT NULL,
  `Account` varchar(24) NOT NULL,
  `Name` varchar(24) NOT NULL,
  `RemoteIpAddress` varchar(16) NOT NULL,
  `ExecutionTime` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 数据导出被取消选择。

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
