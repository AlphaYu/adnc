-- �������ݿ� adnc_cust_unittest
CREATE DATABASE IF NOT EXISTS adnc_cust_unittest CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

USE adnc_cust_unittest;
-- ����  �� adnc_cust_unittest.customer �ṹ
CREATE TABLE IF NOT EXISTS `customer` (
  `id` bigint(20) NOT NULL,
  `account` varchar(16) NOT NULL,
  `password` varchar(32) NOT NULL,
  `nickname` varchar(16) NOT NULL,
  `realname` varchar(16) NOT NULL,
  `createby` bigint(20) NOT NULL COMMENT '������',
  `createtime` datetime(6) NOT NULL COMMENT '����ʱ��/ע��ʱ��',
  `modifyby` bigint(20) DEFAULT NULL COMMENT '��������',
  `modifytime` datetime(6) DEFAULT NULL COMMENT '������ʱ��',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='�ͻ���';

-- ���ڵ�����  adnc_cust_unittest.customer �����ݣ�~0 rows (��Լ)

-- ����  �� adnc_cust_unittest.customerfinance �ṹ
CREATE TABLE IF NOT EXISTS `customerfinance` (
  `id` bigint(20) NOT NULL,
  `account` varchar(16) NOT NULL,
  `balance` decimal(18,4) NOT NULL,
  `rowversion` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `createby` bigint(20) NOT NULL COMMENT '������',
  `createtime` datetime(6) NOT NULL COMMENT '����ʱ��/ע��ʱ��',
  `modifyby` bigint(20) DEFAULT NULL COMMENT '��������',
  `modifytime` datetime(6) DEFAULT NULL COMMENT '������ʱ��',
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_customerfinance_customer_id` FOREIGN KEY (`id`) REFERENCES `customer` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='�ͻ������';

-- ���ڵ�����  adnc_cust_unittest.customerfinance �����ݣ�~0 rows (��Լ)

-- ����  �� adnc_cust_unittest.customertransactionlog �ṹ
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
  `createby` bigint(20) NOT NULL COMMENT '������',
  `createtime` datetime(6) NOT NULL COMMENT '����ʱ��/ע��ʱ��',
  PRIMARY KEY (`id`),
  KEY `ix_customertransactionlog_customerid` (`customerid`),
  CONSTRAINT `fk_customertransactionlog_customer_customerid` FOREIGN KEY (`customerid`) REFERENCES `customer` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='�ͻ�����䶯��¼';

-- ����  �� adnc_cust_unittest.project �ṹ
CREATE TABLE IF NOT EXISTS `project` (
  `id` bigint(20) NOT NULL,
  `name` VARCHAR(50) NOT NULL,
  `isdeleted` TINYINT(1) DEFAULT 0,
   `rowversion` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `createby` bigint(20) NOT NULL COMMENT '������',
  `createtime` datetime(6) NOT NULL COMMENT '����ʱ��/ע��ʱ��',
  `modifyby` bigint(20) NOT NULL COMMENT '��������',
  `modifytime` datetime(6) NOT NULL COMMENT '������ʱ��',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='���̱�';



