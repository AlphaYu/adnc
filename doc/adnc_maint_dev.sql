-- --------------------------------------------------------
-- 主机:                           114.132.157.167
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

-- 导出  表 adnc_maint_dev.sys_config 结构
CREATE TABLE IF NOT EXISTS `sys_config` (
  `id` bigint(20) NOT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0,
  `name` varchar(64) NOT NULL COMMENT '参数名',
  `value` varchar(128) NOT NULL COMMENT '参数值',
  `description` varchar(256) NOT NULL COMMENT '备注',
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='系统参数';

-- 正在导出表  adnc_maint_dev.sys_config 的数据：~1 rows (大约)
INSERT INTO `sys_config` (`id`, `isdeleted`, `name`, `value`, `description`, `createby`, `createtime`, `modifyby`, `modifytime`) VALUES
	(349713920067013, 0, 'adnc', 'aspdotnetcore.net', '', 1600000000000, '2022-11-04 14:49:37.094930', 1600000000000, '2022-12-29 18:24:11.287761');

-- 导出  表 adnc_maint_dev.sys_dictionary 结构
CREATE TABLE IF NOT EXISTS `sys_dictionary` (
  `id` bigint(20) NOT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0,
  `name` varchar(64) NOT NULL,
  `ordinal` int(11) NOT NULL,
  `pid` bigint(20) NOT NULL,
  `value` varchar(16) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='字典';

-- 正在导出表  adnc_maint_dev.sys_dictionary 的数据：~22 rows (大约)
INSERT INTO `sys_dictionary` (`id`, `isdeleted`, `name`, `ordinal`, `pid`, `value`, `createby`, `createtime`, `modifyby`, `modifytime`) VALUES
	(1600000008500, 0, '商品状态', 0, 0, '', 1600000000000, '2021-02-09 01:14:13.888000', 1600000000000, '2022-11-04 14:49:12.308456'),
	(1600000008501, 1, '销售中', 0, 1600000008500, '1000', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008502, 1, '下架中', 0, 1600000008500, '1008', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008600, 0, '订单状态', 0, 0, '', 1600000000000, '2021-02-09 01:14:13.888000', 1600000000000, '2021-02-09 13:39:25.888000'),
	(1600000008601, 0, '创建中', 0, 1600000008600, '1000', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008602, 0, '待付款', 0, 1600000008600, '1008', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008603, 0, '付款中', 0, 1600000008600, '1016', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008604, 0, '待发货', 0, 1600000008600, '1040', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008605, 0, '待确认', 0, 1600000008600, '1048', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008606, 0, '待评价', 0, 1600000008600, '1056', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008607, 0, '完成', 0, 1600000008600, '1064', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008608, 0, '取消中', 0, 1600000008600, '1023', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008609, 0, '已取消', 0, 1600000008600, '1024', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(1600000008610, 0, '已删除', 0, 1600000008600, '1032', 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL),
	(349713813259717, 1, '销售中', 0, 1600000008500, '1000', 1600000000000, '2022-11-04 14:49:11.455529', 1600000000000, '2022-11-04 14:49:11.455856'),
	(349713813259718, 1, '下架中', 0, 1600000008500, '1008', 1600000000000, '2022-11-04 14:49:11.455557', 1600000000000, '2022-11-04 14:49:11.455859'),
	(349713818781125, 0, '销售中', 0, 1600000008500, '1000', 1600000000000, '2022-11-04 14:49:12.502198', 1600000000000, '2022-11-04 14:49:12.502237'),
	(349713818781126, 0, '下架中', 0, 1600000008500, '1008', 1600000000000, '2022-11-04 14:49:12.502199', 1600000000000, '2022-11-04 14:49:12.502238'),
	(349713839523269, 1, 'test', 0, 0, '', 1600000000000, '2022-11-04 14:49:17.372902', 1600000000000, '2022-11-04 14:49:29.207544'),
	(349713868817861, 1, '', 1, 349713839523269, '11', 1600000000000, '2022-11-04 14:49:24.704619', 1600000000000, '2022-11-04 14:49:24.704635'),
	(349713887999429, 1, '111', 1, 349713839523269, '11', 1600000000000, '2022-11-04 14:49:29.381272', 1600000000000, '2022-11-04 14:49:29.381414'),
	(369230717660613, 1, 'oo', 0, 0, '', 1600000000000, '2022-12-29 18:23:40.271444', 1600000000000, '2022-12-29 18:23:40.273440');

-- 导出  表 adnc_maint_dev.sys_eventtracker 结构
CREATE TABLE IF NOT EXISTS `sys_eventtracker` (
  `id` bigint(20) NOT NULL,
  `eventid` bigint(20) NOT NULL,
  `trackername` varchar(50) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_sys_eventtracker_eventid_trackername` (`eventid`,`trackername`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='事件跟踪/处理信息';

-- 正在导出表  adnc_maint_dev.sys_eventtracker 的数据：~0 rows (大约)

-- 导出  表 adnc_maint_dev.sys_notice 结构
CREATE TABLE IF NOT EXISTS `sys_notice` (
  `id` bigint(20) NOT NULL,
  `content` varchar(255) NOT NULL,
  `title` varchar(64) NOT NULL,
  `type` int(11) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='通知';

-- 正在导出表  adnc_maint_dev.sys_notice 的数据：~0 rows (大约)

-- 导出  表 adnc_maint_dev.__efmigrationshistory 结构
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `migrationid` varchar(150) NOT NULL,
  `productversion` varchar(32) NOT NULL,
  PRIMARY KEY (`migrationid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 正在导出表  adnc_maint_dev.__efmigrationshistory 的数据：~2 rows (大约)
INSERT INTO `__efmigrationshistory` (`migrationid`, `productversion`) VALUES
	('20221220081838_Init2022122001', '6.0.6'),
	('20221224034518_v1.0.0', '6.0.6');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
