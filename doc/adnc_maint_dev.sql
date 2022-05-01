/*
SQLyog Community v13.1.6 (64 bit)
MySQL - 10.5.8-MariaDB-1:10.5.8+maria~focal : Database - adnc_maint_dev
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`adnc_maint_dev` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `adnc_maint_dev`;

-- ----------------------------
-- Table structure for syscfg
-- ----------------------------
DROP TABLE IF EXISTS `syscfg`;
CREATE TABLE `syscfg`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL,
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `value` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `description` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of syscfg
-- ----------------------------
INSERT INTO `syscfg` VALUES (1612799408001, 1600000000000, '2021-02-08 23:50:09.297676', 1600000000000, '2021-02-09 00:03:25.805429', 'app', 'adnc', '.NET微服务开发框架', 0);
INSERT INTO `syscfg` VALUES (1612800372001, 1600000000000, '2021-02-09 00:06:13.005999', NULL, NULL, 'tst', 'test', '1', 1);

-- ----------------------------
-- Table structure for sysdict
-- ----------------------------
DROP TABLE IF EXISTS `sysdict`;
CREATE TABLE `sysdict`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL,
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ordinal` int(11) NOT NULL,
  `pid` bigint(20) NOT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0,
  `value` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysdict
-- ----------------------------
INSERT INTO `sysdict` VALUES (1600000008500, 1600000000000, '2021-02-09 01:14:13.888000', 1600000000000, '2021-02-09 13:39:25.888000', '商品状态', 0, 0, 0, NULL);
INSERT INTO `sysdict` VALUES (1600000008501, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '未知', 0, 1600000008500, 0, '1000');
INSERT INTO `sysdict` VALUES (1600000008502, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '下架中', 0, 1600000008500, 0, '1008');
INSERT INTO `sysdict` VALUES (1600000008503, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '销售中', 0, 1600000008500, 0, '1016');
INSERT INTO `sysdict` VALUES (1600000008600, 1600000000000, '2021-02-09 01:14:13.888000', 1600000000000, '2021-02-09 13:39:25.888000', '订单状态', 0, 0, 0, NULL);
INSERT INTO `sysdict` VALUES (1600000008601, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '创建中', 0, 1600000008600, 0, '1000');
INSERT INTO `sysdict` VALUES (1600000008602, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '待付款', 0, 1600000008600, 0, '1008');
INSERT INTO `sysdict` VALUES (1600000008603, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '付款中', 0, 1600000008600, 0, '1016');
INSERT INTO `sysdict` VALUES (1600000008604, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '待发货', 0, 1600000008600, 0, '1040');
INSERT INTO `sysdict` VALUES (1600000008605, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '待确认', 0, 1600000008600, 0, '1048');
INSERT INTO `sysdict` VALUES (1600000008606, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '待评价', 0, 1600000008600, 0, '1056');
INSERT INTO `sysdict` VALUES (1600000008607, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '完成', 0, 1600000008600, 0, '1064');
INSERT INTO `sysdict` VALUES (1600000008608, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '取消中', 0, 1600000008600, 0, '1023');
INSERT INTO `sysdict` VALUES (1600000008609, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '已取消', 0, 1600000008600, 0, '1024');
INSERT INTO `sysdict` VALUES (1600000008610, 1600000000000, '2021-02-09 01:14:13.888000', NULL, NULL, '已删除', 0, 1600000008600, 0, '1032');

-- ----------------------------
-- Table structure for sysnotice
-- ----------------------------
DROP TABLE IF EXISTS `sysnotice`;
CREATE TABLE `sysnotice`  (
  `id` bigint(20) NOT NULL,
  `createby` bigint(20) NOT NULL,
  `createtime` datetime(6) NOT NULL,
  `modifyby` bigint(20) NULL DEFAULT NULL,
  `modifytime` datetime(6) NULL DEFAULT NULL,
  `content` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `title` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `type` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysnotice
-- ----------------------------

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;