-- --------------------------------------------------------
-- 主机:                           62.234.187.128
-- 服务器版本:                        11.7.2-MariaDB-ubu2404 - mariadb.org binary distribution
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


-- 导出 adnc_cust 的数据库结构
CREATE DATABASE IF NOT EXISTS `adnc_cust` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci */;
USE `adnc_cust`;

-- 导出  表 adnc_cust.cap.published 结构
CREATE TABLE IF NOT EXISTS `cap.published` (
  `Id` bigint(20) NOT NULL,
  `Version` varchar(20) DEFAULT NULL,
  `Name` varchar(200) NOT NULL,
  `Content` longtext DEFAULT NULL,
  `Retries` int(11) DEFAULT NULL,
  `Added` datetime NOT NULL,
  `ExpiresAt` datetime DEFAULT NULL,
  `StatusName` varchar(40) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Version_ExpiresAt_StatusName` (`Version`,`ExpiresAt`,`StatusName`),
  KEY `IX_ExpiresAt_StatusName` (`ExpiresAt`,`StatusName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 正在导出表  adnc_cust.cap.published 的数据：~1 rows (大约)
INSERT INTO `cap.published` (`Id`, `Version`, `Name`, `Content`, `Retries`, `Added`, `ExpiresAt`, `StatusName`) VALUES
	(6757258586250821633, '1.0.0.0', 'CustomerRechargedEvent', '{"Headers":{"cap-callback-name":null,"cap-msg-id":"6757258586250821633","cap-corr-id":"6757258586250821633","cap-corr-seq":"0","cap-msg-name":"CustomerRechargedEvent","cap-msg-type":"CustomerRechargedEvent","cap-senttime":"03/23/2025 19:26:26"},"Value":{"CustomerId":657670039692293,"Amount":10,"TransactionLogId":657670078731269,"Id":657670078923781,"OccurredDate":"2025-03-23T19:26:26.1327549+08:00","EventSource":"RechargeAsync","EventTarget":""}}', 0, '2025-03-23 19:26:26', '2025-03-24 19:26:26', 'Succeeded');

-- 导出  表 adnc_cust.cap.received 结构
CREATE TABLE IF NOT EXISTS `cap.received` (
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
  KEY `IX_Version_ExpiresAt_StatusName` (`Version`,`ExpiresAt`,`StatusName`),
  KEY `IX_ExpiresAt_StatusName` (`ExpiresAt`,`StatusName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 正在导出表  adnc_cust.cap.received 的数据：~1 rows (大约)
INSERT INTO `cap.received` (`Id`, `Version`, `Name`, `Group`, `Content`, `Retries`, `Added`, `ExpiresAt`, `StatusName`) VALUES
	(6757258586250821634, '1.0.0.0', 'CustomerRechargedEvent', 'cap.cust-api.stag.1.0.0.0', '{"Headers":{"cap-callback-name":null,"cap-corr-id":"6757258586250821633","cap-corr-seq":"0","cap-msg-id":"6757258586250821633","cap-msg-name":"CustomerRechargedEvent","cap-msg-type":"CustomerRechargedEvent","cap-senttime":"03/23/2025 19:26:26","cap-msg-group":"cap.cust-api.stag.1.0.0.0","cap-exec-instance-id":"15ee6ffbf008"},"Value":{"CustomerId":657670039692293,"Amount":10,"TransactionLogId":657670078731269,"Id":657670078923781,"OccurredDate":"2025-03-23T19:26:26.1327549+08:00","EventSource":"RechargeAsync","EventTarget":""}}', 0, '2025-03-23 19:26:26', '2025-03-24 19:26:26', 'Succeeded');

-- 导出  表 adnc_cust.cust_customer 结构
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci COMMENT='客户表';

-- 正在导出表  adnc_cust.cust_customer 的数据：~1 rows (大约)
INSERT INTO `cust_customer` (`id`, `createby`, `createtime`, `modifyby`, `modifytime`, `account`, `password`, `nickname`, `realname`) VALUES
	(657670039692293, 653335112912901, '2025-03-23 19:26:16.726129', 653335112912901, '2025-03-23 19:26:16.728960', 'beta2008', '', '王二狗', '王二');

-- 导出  表 adnc_cust.cust_eventtracker 结构
CREATE TABLE IF NOT EXISTS `cust_eventtracker` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `eventid` bigint(20) NOT NULL,
  `trackername` varchar(50) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_cust_eventtracker_eventid_trackername` (`eventid`,`trackername`)
) ENGINE=InnoDB AUTO_INCREMENT=657670079996934 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci COMMENT='事件跟踪/处理信息';

-- 正在导出表  adnc_cust.cust_eventtracker 的数据：~1 rows (大约)
INSERT INTO `cust_eventtracker` (`id`, `eventid`, `trackername`, `createby`, `createtime`) VALUES
	(657670079996933, 657670078923781, 'HandleCustomerRechargedEvent', 1000000000000, '2025-03-23 19:26:26.407535');

-- 导出  表 adnc_cust.cust_finance 结构
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci COMMENT='客户财务表';

-- 正在导出表  adnc_cust.cust_finance 的数据：~1 rows (大约)
INSERT INTO `cust_finance` (`id`, `rowversion`, `createby`, `createtime`, `modifyby`, `modifytime`, `account`, `balance`) VALUES
	(657670039692293, '2025-03-23 11:26:26.370077', 653335112912901, '2025-03-23 19:26:16.726174', 1000000000000, '2025-03-23 19:26:26.360461', 'beta2008', 10.0000);

-- 导出  表 adnc_cust.cust_transactionlog 结构
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci COMMENT='客户财务变动记录';

-- 正在导出表  adnc_cust.cust_transactionlog 的数据：~1 rows (大约)
INSERT INTO `cust_transactionlog` (`id`, `createby`, `createtime`, `customerid`, `account`, `exchangetype`, `exchagestatus`, `changingamount`, `amount`, `changedamount`, `remark`) VALUES
	(657670078731269, 653335112912901, '2025-03-23 19:26:26.118544', 657670039692293, 'beta2008', 8000, 2008, 0.0000, 10.0000, 10.0000, '');

-- 导出  表 adnc_cust.__EFMigrationsHistory 结构
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
  `migrationid` varchar(150) NOT NULL,
  `productversion` varchar(32) NOT NULL,
  PRIMARY KEY (`migrationid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 正在导出表  adnc_cust.__EFMigrationsHistory 的数据：~2 rows (大约)
INSERT INTO `__EFMigrationsHistory` (`migrationid`, `productversion`) VALUES
	('20250317152707_Init20250317', '8.0.13'),
	('20250323102232_Update25032301', '8.0.13');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
