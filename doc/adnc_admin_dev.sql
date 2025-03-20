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
CREATE DATABASE IF NOT EXISTS `adnc_admin_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
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

-- 数据导出被取消选择。

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

-- 数据导出被取消选择。

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
) ENGINE=InnoDB AUTO_INCREMENT=656246815661638 DEFAULT CHARSET=utf8mb4 COMMENT='字典数据';

-- 数据导出被取消选择。

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

-- 数据导出被取消选择。

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

-- 数据导出被取消选择。

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

-- 数据导出被取消选择。

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

-- 数据导出被取消选择。

-- 导出  表 adnc_admin_dev.sys_role_menu_relation 结构
CREATE TABLE IF NOT EXISTS `sys_role_menu_relation` (
  `id` bigint(20) NOT NULL,
  `menuid` bigint(20) NOT NULL,
  `roleid` bigint(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='菜单角色关系';

-- 数据导出被取消选择。

-- 导出  表 adnc_admin_dev.sys_role_user_relation 结构
CREATE TABLE IF NOT EXISTS `sys_role_user_relation` (
  `id` bigint(20) NOT NULL,
  `userid` bigint(20) NOT NULL,
  `roleid` bigint(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户角色关系';

-- 数据导出被取消选择。

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

-- 数据导出被取消选择。

-- 导出  表 adnc_admin_dev.__efmigrationshistory 结构
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `migrationid` varchar(150) NOT NULL,
  `productversion` varchar(32) NOT NULL,
  PRIMARY KEY (`migrationid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 数据导出被取消选择。

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
