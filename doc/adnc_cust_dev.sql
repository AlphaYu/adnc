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
CREATE DATABASE IF NOT EXISTS `adnc_cust_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
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
