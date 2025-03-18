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
CREATE DATABASE IF NOT EXISTS `adnc_syslog_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
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
