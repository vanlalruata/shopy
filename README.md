# shopy

Pre-Required Software
* Visual Studio 2015 or above
* mysql-5.6.36-win32
* mysql-connector-net-8.0.11

# Database

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for customertb
-- ----------------------------
CREATE TABLE IF NOT EXISTS `customertb`  (
  `invoiceid` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Invoice ID',
  `prefix` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Invoice Prefix',
  `invoicedate` date NULL DEFAULT NULL COMMENT 'Invoice Date',
  `customername` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Full Customer Name',
  `customercontact` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Customer Contact',
  `customeraddress` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Customer Email Address',
  PRIMARY KEY (`invoiceid`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 00001 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for invoicerec
-- ----------------------------
CREATE TABLE IF NOT EXISTS `invoicerec`  (
  `invoiceid` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `invoicedate` date NULL DEFAULT NULL,
  `subtotal` decimal(25, 2) NULL DEFAULT NULL,
  `discount` decimal(25, 2) NULL DEFAULT NULL,
  `nettotal` decimal(25, 2) NULL DEFAULT NULL,
  `paidamount` decimal(25, 2) NULL DEFAULT NULL,
  `returnamount` decimal(25, 2) NULL DEFAULT NULL,
  `status` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for invoicetb
-- ----------------------------
CREATE TABLE IF NOT EXISTS `invoicetb`  (
  `invoiceid` varchar(225) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL COMMENT 'Invoice ID',
  `partname` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Particular',
  `partquantity` decimal(25, 0) NULL DEFAULT NULL COMMENT 'Particular Quantity',
  `partrate` decimal(25, 2) NULL DEFAULT NULL COMMENT 'Particular Rate',
  `parttax` decimal(25, 2) NULL DEFAULT NULL COMMENT 'Particular Tax',
  `parttaxtype` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Particular Tax Type',
  `parttaxamount` decimal(25, 2) NULL DEFAULT NULL COMMENT 'Particular Tax Amount',
  `partnetprice` decimal(10, 2) NULL DEFAULT NULL COMMENT 'Final Amount'
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for particulartb
-- ----------------------------
CREATE TABLE IF NOT EXISTS `particulartb`  (
  `partid` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Particular ID',
  `partname` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Particular Name',
  `partrate` decimal(25, 2) NULL DEFAULT NULL COMMENT 'Particular Rate',
  `parttax` decimal(25, 6) NULL DEFAULT NULL COMMENT 'Particular Tax %',
  `parttaxtype` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Particular Tax Type',
  `taxamount` decimal(25, 2) NULL DEFAULT NULL COMMENT 'Particular Tax Amount',
  `totalamount` decimal(25, 2) NULL DEFAULT NULL COMMENT 'Particular Net Total',
  PRIMARY KEY (`partid`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for usertb
-- ----------------------------
CREATE TABLE IF NOT EXISTS `usertb`  (
  `userid` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL COMMENT 'User ID',
  `password` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `fullname` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `usertype` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `email` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  PRIMARY KEY (`userid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

SET FOREIGN_KEY_CHECKS = 1;
