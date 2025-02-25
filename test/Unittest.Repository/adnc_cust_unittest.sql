-- 创建数据库 adnc_cust_unittest
CREATE DATABASE IF NOT EXISTS adnc_cust_unittest CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

USE adnc_cust_unittest;
-- 导出  表 adnc_cust_unittest.customer 结构
CREATE TABLE IF NOT EXISTS `customer` (
  `id` bigint(20) NOT NULL,
  `account` varchar(16) NOT NULL,
  `password` varchar(32) NOT NULL,
  `nickname` varchar(16) NOT NULL,
  `realname` varchar(16) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户表';

-- 正在导出表  adnc_cust_unittest.customer 的数据：~0 rows (大约)

-- 导出  表 adnc_cust_unittest.customerfinance 结构
CREATE TABLE IF NOT EXISTS `customerfinance` (
  `id` bigint(20) NOT NULL,
  `account` varchar(16) NOT NULL,
  `balance` decimal(18,4) NOT NULL,
  `rowversion` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) DEFAULT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_customerfinance_customer_id` FOREIGN KEY (`id`) REFERENCES `customer` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户财务表';

-- 正在导出表  adnc_cust_unittest.customerfinance 的数据：~0 rows (大约)

-- 导出  表 adnc_cust_unittest.customertransactionlog 结构
CREATE TABLE IF NOT EXISTS `customertransactionlog` (
  `id` bigint(20) NOT NULL,
  `customerid` bigint(20) NOT NULL,
  `account` varchar(16) NOT NULL,
  `exchangetype` int(11) NOT NULL,
  `exchagestatus` int(11) NOT NULL,
  `changingamount` decimal(18,4) NOT NULL,
  `amount` decimal(18,4) NOT NULL,
  `changedamount` decimal(18,4) NOT NULL,
  `remark` varchar(64) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  PRIMARY KEY (`id`),
  KEY `ix_customertransactionlog_customerid` (`customerid`),
  CONSTRAINT `fk_customertransactionlog_customer_customerid` FOREIGN KEY (`customerid`) REFERENCES `customer` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='客户财务变动记录';

-- 导出  表 adnc_cust_unittest.project 结构
CREATE TABLE IF NOT EXISTS `project` (
  `id` bigint(20) NOT NULL,
  `name` VARCHAR(50) NOT NULL,
  `isdeleted` TINYINT(1) DEFAULT 0,
   `rowversion` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `createby` bigint(20) NOT NULL COMMENT '创建人',
  `createtime` datetime(6) NOT NULL COMMENT '创建时间/注册时间',
  `modifyby` bigint(20) NOT NULL COMMENT '最后更新人',
  `modifytime` datetime(6) NOT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工程表';



