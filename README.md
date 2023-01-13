# SellingwithEntityframwork
แก้ไข การเชื่อมต่อ Model ให้เป็นของตัวเอง
สร้าง Table ตามนี้

CREATE TABLE `pd_order` (
  `OD_ID` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PD_ID` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `OD_DATE` datetime DEFAULT NULL,
  `OD_QUANTITY` decimal(18,2) DEFAULT NULL,
  `PRICE` decimal(18,2) DEFAULT NULL,
  `OD_STATUS` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`OD_ID`,`PD_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;


CREATE TABLE `pd_product` (
  `PD_ID` varchar(10) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `PD_NAME` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `PD_VALUE` decimal(18,2) DEFAULT NULL,
  `PD_PRICE` decimal(18,2) DEFAULT NULL,
  `TP_ID` varchar(10) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `PD_UNIT` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`PD_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

CREATE TABLE `pd_type` (
  `TP_ID` varchar(10) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `TP_NAME` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`TP_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

CREATE TABLE `pd_user` (
  `USERNAME` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `PASSWORD` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`USERNAME`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
