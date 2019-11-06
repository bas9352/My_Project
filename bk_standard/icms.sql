-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 06, 2019 at 04:30 AM
-- Server version: 10.4.8-MariaDB
-- PHP Version: 7.3.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `icms`
--

-- --------------------------------------------------------

--
-- Table structure for table `admin`
--

CREATE TABLE `admin` (
  `admin_id` int(11) NOT NULL,
  `admin_username` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `admin_password` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `admin_name` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `admin_lastname` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `group_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `admin`
--

INSERT INTO `admin` (`admin_id`, `admin_username`, `admin_password`, `admin_name`, `admin_lastname`, `group_id`) VALUES
(1, 'admin', 'xGylcWycXNhe/k++55ZLYA==', '', '', 1);

-- --------------------------------------------------------

--
-- Table structure for table `bonus`
--

CREATE TABLE `bonus` (
  `bonus_id` int(5) NOT NULL,
  `bonus_top_up` double DEFAULT NULL,
  `bonus_point` double DEFAULT NULL,
  `bonus_c_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `bonus_status` enum('false','true') COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `computer`
--

CREATE TABLE `computer` (
  `pc_id` int(5) NOT NULL,
  `pc_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `pc_ip_address` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `pc_mac_address` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `computer`
--

INSERT INTO `computer` (`pc_id`, `pc_name`, `pc_ip_address`, `pc_mac_address`) VALUES
(1, 'pc1', '192.168.137.1', 'F4:06:69:C1:78:8E'),
(2, 'pc1', '192.168.137.1', 'F4:06:69:C1:78:8E');

-- --------------------------------------------------------

--
-- Table structure for table `coupon`
--

CREATE TABLE `coupon` (
  `coupon_id` int(5) NOT NULL,
  `coupon_username` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `coupon_password` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `coupon_c_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `coupon_s_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `coupon_e_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `coupon_create_by` int(5) DEFAULT NULL,
  `op_c_id` int(5) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `coupon`
--

INSERT INTO `coupon` (`coupon_id`, `coupon_username`, `coupon_password`, `coupon_c_date`, `coupon_s_date`, `coupon_e_date`, `coupon_create_by`, `op_c_id`) VALUES
(1, 'test', 'test', NULL, NULL, NULL, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `coupon_top_up`
--

CREATE TABLE `coupon_top_up` (
  `ct_id` int(5) NOT NULL,
  `ct_coupon_id` int(5) DEFAULT NULL,
  `ct_by` int(5) DEFAULT NULL,
  `ct_ordinal` int(10) DEFAULT NULL,
  `ct_real_amount` double DEFAULT NULL,
  `ct_free_amount` double DEFAULT NULL,
  `ct_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `coupon_top_up`
--

INSERT INTO `coupon_top_up` (`ct_id`, `ct_coupon_id`, `ct_by`, `ct_ordinal`, `ct_real_amount`, `ct_free_amount`, `ct_date`) VALUES
(1, 1, 1, 1, 10, NULL, NULL),
(2, 1001, 1, 1, 10, NULL, NULL),
(3, 1001, 1, 2, 10, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `member`
--

CREATE TABLE `member` (
  `member_id` int(5) NOT NULL,
  `member_username` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_password` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_name` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_nickname` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_lastname` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_birthday` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_address` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_id_card` varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_tel` varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_email` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_c_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_s_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_e_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_credit_limit` double DEFAULT NULL,
  `member_create_by` int(5) DEFAULT NULL,
  `group_id` int(5) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `member`
--

INSERT INTO `member` (`member_id`, `member_username`, `member_password`, `member_name`, `member_nickname`, `member_lastname`, `member_birthday`, `member_address`, `member_id_card`, `member_tel`, `member_email`, `member_c_date`, `member_s_date`, `member_e_date`, `member_credit_limit`, `member_create_by`, `group_id`) VALUES
(1, 'bas9352', 'xGylcWycXNhe/k++55ZLYA==', 'aaaa', 'aaaa', 'aaaa', NULL, NULL, NULL, NULL, NULL, '2030-10-15 02:00:00', NULL, NULL, NULL, 1, 3),
(2, 'bas14223', 'xGylcWycXNhe/k++55ZLYA==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2030-10-16 02:00:00', NULL, NULL, NULL, 1, 3);

-- --------------------------------------------------------

--
-- Table structure for table `member_top_up`
--

CREATE TABLE `member_top_up` (
  `mt_id` int(5) NOT NULL,
  `mt_member_id` int(5) DEFAULT NULL,
  `mt_by` int(5) DEFAULT NULL,
  `mt_ordinal` int(10) DEFAULT NULL,
  `mt_real_amount` double DEFAULT NULL,
  `mt_free_amount` double DEFAULT NULL,
  `mt_debt_amount` double DEFAULT NULL,
  `mt_pay_debt` double DEFAULT NULL,
  `mt_bonus_id` int(5) DEFAULT NULL,
  `mt_promotion_id` int(5) DEFAULT NULL,
  `mt_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `member_top_up`
--

INSERT INTO `member_top_up` (`mt_id`, `mt_member_id`, `mt_by`, `mt_ordinal`, `mt_real_amount`, `mt_free_amount`, `mt_debt_amount`, `mt_pay_debt`, `mt_bonus_id`, `mt_promotion_id`, `mt_date`) VALUES
(7, 1, 1, 1, 1000, NULL, NULL, NULL, NULL, NULL, '2019-10-15 00:00:00'),
(8, 1, 1, 2, 1000, NULL, NULL, NULL, NULL, NULL, '2019-10-16 00:00:00'),
(9, 2, 1, 1, 1000, NULL, NULL, NULL, NULL, NULL, '2019-10-17 00:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `online`
--

CREATE TABLE `online` (
  `online_id` int(5) NOT NULL,
  `online_pc_id` int(5) DEFAULT NULL,
  `online_member_id` int(5) DEFAULT NULL,
  `online_coupon_id` int(5) DEFAULT NULL,
  `online_status` enum('offline','available','online') COLLATE utf8_unicode_ci DEFAULT NULL,
  `online_ordinal` int(10) DEFAULT NULL,
  `online_s_datetime` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `online_e_datetime` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `online_use_real_amount` double DEFAULT NULL,
  `online_use_free_amount` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `online`
--

INSERT INTO `online` (`online_id`, `online_pc_id`, `online_member_id`, `online_coupon_id`, `online_status`, `online_ordinal`, `online_s_datetime`, `online_e_datetime`, `online_use_real_amount`, `online_use_free_amount`) VALUES
(1, 1, NULL, NULL, 'available', 1, '2019-10-15 04:54:04', NULL, NULL, NULL),
(2, 1, NULL, NULL, 'available', 2, '2019-10-15 04:54:43', NULL, NULL, NULL),
(3, 1, NULL, NULL, 'available', 3, '2019-10-15 04:58:21', NULL, NULL, NULL),
(4, 1, NULL, NULL, 'available', 4, '2019-10-15 04:59:08', NULL, NULL, NULL),
(5, 1, 1, NULL, 'online', 5, '2019-10-15 04:59:16', NULL, NULL, NULL),
(7, 2, NULL, NULL, 'online', 1, '2019-10-15 05:02:17', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `option_coupon`
--

CREATE TABLE `option_coupon` (
  `op_c_id` int(5) NOT NULL,
  `op_c_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `op_c_real_amount` double DEFAULT NULL,
  `op_c_free_amount` double DEFAULT NULL,
  `op_c_s_date` enum('false','true') COLLATE utf8_unicode_ci DEFAULT NULL,
  `op_c_e_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `group_id` int(5) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `option_coupon`
--

INSERT INTO `option_coupon` (`op_c_id`, `op_c_name`, `op_c_real_amount`, `op_c_free_amount`, `op_c_s_date`, `op_c_e_date`, `group_id`) VALUES
(1, 'project', 10, 10, '', '7', 4);

-- --------------------------------------------------------

--
-- Table structure for table `promotion`
--

CREATE TABLE `promotion` (
  `promo_id` int(5) NOT NULL,
  `promo_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `promo_rate_point` double DEFAULT NULL,
  `promo_rate` double DEFAULT NULL,
  `promo_c_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `promo_status` enum('false','true') COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `staff`
--

CREATE TABLE `staff` (
  `staff_id` int(5) NOT NULL,
  `staff_username` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_password` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_name` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_nickname` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_lastname` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_birthday` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_address` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_id_card` varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_tel` varchar(20) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_email` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_c_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_s_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_e_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `group_id` int(5) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `staff`
--

INSERT INTO `staff` (`staff_id`, `staff_username`, `staff_password`, `staff_name`, `staff_nickname`, `staff_lastname`, `staff_birthday`, `staff_address`, `staff_id_card`, `staff_tel`, `staff_email`, `staff_c_date`, `staff_s_date`, `staff_e_date`, `group_id`) VALUES
(1, 'admin', 'xGylcWycXNhe/k++55ZLYA==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1),
(2, '584235041', 'xGylcWycXNhe/k++55ZLYA==', 'ธรณ์ธันย์', 'บาส', 'อ่อนนวล', '1996-12-29', '343 หมู่4 ทุ่งหมอ สะเดา สงขลา', '1909801117648', '0612812641', '584235041@gmail.com', '1999-01-01 00:00:00', NULL, '2000-01-01 00:00:00', 2);

-- --------------------------------------------------------

--
-- Table structure for table `type`
--

CREATE TABLE `type` (
  `type_id` int(5) NOT NULL,
  `type_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `type`
--

INSERT INTO `type` (`type_id`, `type_name`) VALUES
(1, 'admin'),
(2, 'staff'),
(3, 'member'),
(4, 'coupon');

-- --------------------------------------------------------

--
-- Table structure for table `user_group`
--

CREATE TABLE `user_group` (
  `group_id` int(5) NOT NULL,
  `group_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `type_id` int(5) DEFAULT NULL,
  `group_rate` double DEFAULT NULL,
  `group_bonus_status` enum('false','true') COLLATE utf8_unicode_ci DEFAULT NULL,
  `group_c_date` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `user_group`
--

INSERT INTO `user_group` (`group_id`, `group_name`, `type_id`, `group_rate`, `group_bonus_status`, `group_c_date`) VALUES
(1, 'admin', 1, NULL, 'false', NULL),
(2, 'staff', 2, NULL, 'false', NULL),
(3, 'member', 3, 10, 'false', NULL),
(4, 'coupon', 4, 10, 'false', NULL);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_all_customer`
-- (See below for the actual view)
--
CREATE TABLE `v_all_customer` (
`v_member_id` int(11)
,`v_coupon_id` int(11)
,`v_all_create_by` int(11)
,`v_all_username` varchar(50)
,`v_all_password` varchar(50)
,`v_all_c_date` varchar(50)
,`v_all_s_date` varchar(50)
,`v_all_e_date` varchar(50)
,`v_op_c_id` int(11)
,`v_all_group_id` int(11)
,`v_all_group_name` varchar(50)
,`v_all_group_rate` double
,`v_group_bonus_status` varchar(5)
,`v_all_type_id` int(11)
,`v_all_type_name` varchar(50)
,`v_member_name` varchar(100)
,`v_member_nickname` varchar(50)
,`v_member_lastname` varchar(100)
,`v_member_birthday` varchar(50)
,`v_member_address` varchar(255)
,`v_member_id_card` varchar(20)
,`v_member_tel` varchar(20)
,`v_member_email` varchar(100)
,`v_member_credit_limit` double
,`v_all_ordinal_last` int(11)
,`v_all_total_real_amount` double
,`v_all_total_free_amount` double
,`v_total_debt_amount` double
,`v_total_pay_debt` double
,`v_remaining_debt_amount` double
,`v_get_bonus` bigint(21)
,`v_total_bonus_amount` double
,`v_get_promotion` bigint(21)
,`v_total_promo_amount` double
,`v_total_use_bonus_amount` double
,`v_remaining_bonus_amount` double
,`v_total_real_top_up` double
,`v_total_free_top_up` double
,`v_all_total_top_up` double
,`v_all_use_real_amount` double
,`v_all_use_free_amount` double
,`v_all_total_use_amount` double
,`v_all_remaining_real_amount` double
,`v_all_remaining_free_amount` double
,`v_all_remaining_amount` double
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_all_user`
-- (See below for the actual view)
--
CREATE TABLE `v_all_user` (
`v_staff_id` int(11)
,`v_member_id` int(11)
,`v_coupon_id` int(11)
,`v_all_username` varchar(50)
,`v_all_password` varchar(50)
,`v_all_c_date` varchar(50)
,`v_all_s_date` varchar(50)
,`v_all_e_date` varchar(50)
,`v_all_create_by` int(11)
,`v_op_c_id` int(11)
,`v_all_group_id` int(11)
,`v_all_group_name` varchar(50)
,`v_all_group_rate` double
,`v_all_type_id` int(11)
,`v_all_type_name` varchar(50)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_computer`
-- (See below for the actual view)
--
CREATE TABLE `v_computer` (
`v_pc_id` int(5)
,`v_pc_name` varchar(50)
,`v_ip_address` varchar(50)
,`v_mac_address` varchar(50)
,`v_online_id` int(5)
,`v_online_member_id` int(5)
,`v_online_coupon_id` int(5)
,`v_online_status` enum('offline','available','online')
,`v_online_ordinal` int(10)
,`v_online_s_datetime` varchar(50)
,`v_online_e_datetime` varchar(50)
,`v_online_use_real_amount_last` double
,`v_online_use_free_amount_last` double
,`v_online_total_use_amount_last` double
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_computer_status`
-- (See below for the actual view)
--
CREATE TABLE `v_computer_status` (
`v_pc_id` int(5)
,`v_pc_name` varchar(50)
,`v_ip_address` varchar(50)
,`v_mac_address` varchar(50)
,`v_online_id` int(5)
,`v_online_ordinal` int(10)
,`v_online_member_id` int(5)
,`v_online_coupon_id` int(5)
,`v_online_status` enum('offline','available','online')
,`v_online_s_datetime` varchar(50)
,`v_online_s_date` varchar(50)
,`v_online_s_time` varchar(50)
,`v_online_e_datetime` varchar(50)
,`v_online_e_date` varchar(50)
,`v_online_e_time` varchar(50)
,`v_online_use_real_amount_last` double
,`v_online_use_free_amount_last` double
,`v_online_total_use_amount_last` double
,`v_member_id` int(11)
,`v_coupon_id` int(11)
,`v_all_create_by` int(11)
,`v_all_username` varchar(50)
,`v_all_password` varchar(50)
,`v_all_c_date` varchar(50)
,`v_all_s_date` varchar(50)
,`v_all_e_date` varchar(50)
,`v_op_c_id` int(11)
,`v_all_group_id` int(11)
,`v_all_group_name` varchar(50)
,`v_all_group_rate` double
,`v_group_bonus_status` varchar(5)
,`v_all_type_id` int(11)
,`v_all_type_name` varchar(50)
,`v_member_name` varchar(100)
,`v_member_nickname` varchar(50)
,`v_member_lastname` varchar(100)
,`v_member_birthday` varchar(50)
,`v_member_address` varchar(255)
,`v_member_id_card` varchar(20)
,`v_member_tel` varchar(20)
,`v_member_email` varchar(100)
,`v_member_credit_limit` double
,`v_all_ordinal_last` int(11)
,`v_all_total_real_amount` double
,`v_all_total_free_amount` double
,`v_total_debt_amount` double
,`v_total_pay_debt` double
,`v_get_bonus` bigint(21)
,`v_total_bonus_amount` double
,`v_get_promotion` bigint(21)
,`v_total_promo_amount` double
,`v_total_use_bonus_amount` double
,`v_remaining_bonus_amount` double
,`v_total_real_top_up` double
,`v_total_free_top_up` double
,`v_all_total_top_up` double
,`v_all_use_real_amount` double
,`v_all_use_free_amount` double
,`v_all_total_use_amount` double
,`v_all_remaining_real_amount` double
,`v_all_remaining_free_amount` double
,`v_all_remaining_amount` double
,`v_all_hr` double(17,0)
,`v_all_mn` double(17,0)
,`v_all_use_hr` double(17,0)
,`v_all_use_mn` double(17,0)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_coupon_top_up`
-- (See below for the actual view)
--
CREATE TABLE `v_coupon_top_up` (
`v_ct_coupon_id` int(5)
,`v_ct_ordinal_last` int(10)
,`v_total_real_amount` double
,`v_total_free_amount` double
,`v_total_top_up` double
,`v_total_use_real_amount` double
,`v_total_use_free_amount` double
,`v_total_use_amount` double
,`v_remaining_real_amount` double
,`v_remaining_free_amount` double
,`v_total_remaining_amount` double
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_ct_ordinal`
-- (See below for the actual view)
--
CREATE TABLE `v_ct_ordinal` (
`v_coupon_id` int(5)
,`v_last_ordinal` int(10)
,`v_last_date` varchar(50)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_member_top_up`
-- (See below for the actual view)
--
CREATE TABLE `v_member_top_up` (
`v_mt_member_id` int(5)
,`v_mt_ordinal_last` int(10)
,`v_total_real_amount` double
,`v_total_free_amount` double
,`v_total_debt_amount` double
,`v_total_pay_debt` double
,`v_get_bonus` bigint(21)
,`v_total_bonus_amount` double
,`v_get_promotion` bigint(21)
,`v_total_promo_amount` double
,`v_total_use_bonus_amount` double
,`v_remaining_bonus_amount` double
,`v_total_real_top_up` double
,`v_total_free_top_up` double
,`v_total_top_up` double
,`v_total_use_real_amount` double
,`v_total_use_free_amount` double
,`v_total_use_amount` double
,`v_remaining_real_amount` double
,`v_remaining_free_amount` double
,`v_total_remaining_amount` double
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_mt_ordinal`
-- (See below for the actual view)
--
CREATE TABLE `v_mt_ordinal` (
`v_member_id` int(5)
,`v_last_ordinal` int(10)
,`v_last_date` varchar(50)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_online_history`
-- (See below for the actual view)
--
CREATE TABLE `v_online_history` (
`v_online_id` int(5)
,`v_online_pc_id` int(5)
,`v_online_pc_name` varchar(50)
,`v_online_pc_ip_address` varchar(50)
,`v_online_pc_mac_address` varchar(50)
,`v_online_member_id` int(5)
,`v_online_coupon_id` int(5)
,`v_online_status` enum('offline','available','online')
,`v_online_ordinal` int(10)
,`v_online_s_datetime` varchar(50)
,`v_online_s_date` varchar(50)
,`v_online_s_time` varchar(50)
,`v_online_e_datetime` varchar(50)
,`v_online_e_date` varchar(50)
,`v_online_e_time` varchar(50)
,`v_online_use_real_amount` double
,`v_online_use_free_amount` double
,`v_online_total_use_amount` double
,`v_all_hr` double(17,0)
,`v_all_mn` double(17,0)
,`v_member_id` int(11)
,`v_coupon_id` int(11)
,`v_all_create_by` int(11)
,`v_all_username` varchar(50)
,`v_all_password` varchar(50)
,`v_all_c_date` varchar(50)
,`v_all_s_date` varchar(50)
,`v_all_e_date` varchar(50)
,`v_op_c_id` int(11)
,`v_all_group_id` int(11)
,`v_all_group_name` varchar(50)
,`v_all_group_rate` double
,`v_all_type_id` int(11)
,`v_all_type_name` varchar(50)
,`v_member_name` varchar(100)
,`v_member_nickname` varchar(50)
,`v_member_lastname` varchar(100)
,`v_member_birthday` varchar(50)
,`v_member_address` varchar(255)
,`v_member_id_card` varchar(20)
,`v_member_tel` varchar(20)
,`v_member_email` varchar(100)
,`v_member_credit_limit` double
,`v_all_ordinal_last` int(11)
,`v_all_total_real_amount` double
,`v_all_total_free_amount` double
,`v_total_debt_amount` double
,`v_total_pay_debt` double
,`v_get_bonus` bigint(21)
,`v_total_bonus_amount` double
,`v_get_promotion` bigint(21)
,`v_total_promo_amount` double
,`v_total_use_bonus_amount` double
,`v_remaining_bonus_amount` double
,`v_total_real_top_up` double
,`v_total_free_top_up` double
,`v_all_total_top_up` double
,`v_all_use_real_amount` double
,`v_all_use_free_amount` double
,`v_all_total_use_amount` double
,`v_all_remaining_real_amount` double
,`v_all_remaining_free_amount` double
,`v_all_remaining_amount` double
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `v_online_ordinal`
-- (See below for the actual view)
--
CREATE TABLE `v_online_ordinal` (
`v_pc_id` int(5)
,`v_last_ordinal` int(10)
,`v_last_datetime` varchar(50)
);

-- --------------------------------------------------------

--
-- Structure for view `v_all_customer`
--
DROP TABLE IF EXISTS `v_all_customer`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_all_customer`  AS  select `member`.`member_id` AS `v_member_id`,`coupon`.`coupon_id` AS `v_coupon_id`,`member`.`member_create_by` AS `v_all_create_by`,`member`.`member_username` AS `v_all_username`,`member`.`member_password` AS `v_all_password`,`member`.`member_c_date` AS `v_all_c_date`,`member`.`member_s_date` AS `v_all_s_date`,`member`.`member_e_date` AS `v_all_e_date`,`coupon`.`op_c_id` AS `v_op_c_id`,`member`.`group_id` AS `v_all_group_id`,`user_group`.`group_name` AS `v_all_group_name`,`user_group`.`group_rate` AS `v_all_group_rate`,`user_group`.`group_bonus_status` AS `v_group_bonus_status`,`type`.`type_id` AS `v_all_type_id`,`type`.`type_name` AS `v_all_type_name`,`member`.`member_name` AS `v_member_name`,`member`.`member_nickname` AS `v_member_nickname`,`member`.`member_lastname` AS `v_member_lastname`,`member`.`member_birthday` AS `v_member_birthday`,`member`.`member_address` AS `v_member_address`,`member`.`member_id_card` AS `v_member_id_card`,`member`.`member_tel` AS `v_member_tel`,`member`.`member_email` AS `v_member_email`,`member`.`member_credit_limit` AS `v_member_credit_limit`,`v_member_top_up`.`v_mt_ordinal_last` AS `v_all_ordinal_last`,`v_member_top_up`.`v_total_real_amount` AS `v_all_total_real_amount`,`v_member_top_up`.`v_total_free_amount` AS `v_all_total_free_amount`,`v_member_top_up`.`v_total_debt_amount` AS `v_total_debt_amount`,`v_member_top_up`.`v_total_pay_debt` AS `v_total_pay_debt`,coalesce(`v_member_top_up`.`v_total_debt_amount`,0) - coalesce(`v_member_top_up`.`v_total_pay_debt`,0) AS `v_remaining_debt_amount`,`v_member_top_up`.`v_get_bonus` AS `v_get_bonus`,`v_member_top_up`.`v_total_bonus_amount` AS `v_total_bonus_amount`,`v_member_top_up`.`v_get_promotion` AS `v_get_promotion`,`v_member_top_up`.`v_total_promo_amount` AS `v_total_promo_amount`,`v_member_top_up`.`v_total_use_bonus_amount` AS `v_total_use_bonus_amount`,`v_member_top_up`.`v_remaining_bonus_amount` AS `v_remaining_bonus_amount`,`v_member_top_up`.`v_total_real_top_up` AS `v_total_real_top_up`,`v_member_top_up`.`v_total_free_top_up` AS `v_total_free_top_up`,`v_member_top_up`.`v_total_top_up` AS `v_all_total_top_up`,`v_member_top_up`.`v_total_use_real_amount` AS `v_all_use_real_amount`,`v_member_top_up`.`v_total_use_free_amount` AS `v_all_use_free_amount`,`v_member_top_up`.`v_total_use_amount` AS `v_all_total_use_amount`,`v_member_top_up`.`v_remaining_real_amount` AS `v_all_remaining_real_amount`,`v_member_top_up`.`v_remaining_free_amount` AS `v_all_remaining_free_amount`,`v_member_top_up`.`v_total_remaining_amount` AS `v_all_remaining_amount` from ((((((`member` left join `coupon` on(`coupon`.`coupon_id` = (`member`.`member_id` = 0))) left join `option_coupon` on(`option_coupon`.`op_c_id` = `coupon`.`op_c_id`)) left join `user_group` on(`user_group`.`group_id` = `member`.`group_id`)) left join `type` on(`type`.`type_id` = `user_group`.`type_id`)) left join `v_member_top_up` on(`v_member_top_up`.`v_mt_member_id` = `member`.`member_id`)) left join `v_coupon_top_up` on(`v_coupon_top_up`.`v_ct_coupon_id` = `coupon`.`coupon_id`)) union select `member`.`member_id` AS `v_member_id`,`coupon`.`coupon_id` AS `v_coupon_id`,`member`.`member_create_by` AS `v_all_create_by`,`coupon`.`coupon_username` AS `v_all_username`,`coupon`.`coupon_password` AS `v_all_password`,`coupon`.`coupon_c_date` AS `v_all_c_date`,`coupon`.`coupon_c_date` AS `v_all_s_date`,`coupon`.`coupon_c_date` AS `v_all_e_date`,`coupon`.`op_c_id` AS `v_op_c_id`,`option_coupon`.`group_id` AS `v_all_group_id`,`user_group`.`group_name` AS `v_all_group_name`,`user_group`.`group_rate` AS `v_all_group_rate`,`user_group`.`group_bonus_status` AS `v_group_bonus_status`,`type`.`type_id` AS `v_all_type_id`,`type`.`type_name` AS `v_all_type_name`,`member`.`member_name` AS `v_member_name`,`member`.`member_nickname` AS `v_member_nickname`,`member`.`member_lastname` AS `v_member_lastname`,`member`.`member_birthday` AS `v_member_birthday`,`member`.`member_address` AS `v_member_address`,`member`.`member_id_card` AS `v_member_id_card`,`member`.`member_tel` AS `v_member_tel`,`member`.`member_email` AS `v_member_email`,`member`.`member_credit_limit` AS `v_member_credit_limit`,`v_coupon_top_up`.`v_ct_ordinal_last` AS `v_ct_ordinal_last`,`v_coupon_top_up`.`v_total_real_amount` AS `v_total_real_amount`,`v_coupon_top_up`.`v_total_free_amount` AS `v_total_free_amount`,`v_member_top_up`.`v_total_debt_amount` AS `v_total_debt_amount`,`v_member_top_up`.`v_total_pay_debt` AS `v_total_pay_debt`,coalesce(`v_member_top_up`.`v_total_debt_amount`,0) - coalesce(`v_member_top_up`.`v_total_pay_debt`,0) AS `v_remaining_debt_amount`,`v_member_top_up`.`v_get_bonus` AS `v_get_bonus`,`v_member_top_up`.`v_total_bonus_amount` AS `v_total_bonus_amount`,`v_member_top_up`.`v_get_promotion` AS `v_get_promotion`,`v_member_top_up`.`v_total_promo_amount` AS `v_total_promo_amount`,`v_member_top_up`.`v_total_use_bonus_amount` AS `v_total_use_bonus_amount`,`v_member_top_up`.`v_remaining_bonus_amount` AS `v_remaining_bonus_amount`,`v_member_top_up`.`v_total_real_top_up` AS `v_total_real_top_up`,`v_member_top_up`.`v_total_free_top_up` AS `v_total_free_top_up`,`v_coupon_top_up`.`v_total_top_up` AS `v_total_top_up`,`v_coupon_top_up`.`v_total_use_real_amount` AS `v_total_use_real_amount`,`v_coupon_top_up`.`v_total_use_free_amount` AS `v_total_use_free_amount`,`v_coupon_top_up`.`v_total_use_amount` AS `v_total_use_amount`,`v_coupon_top_up`.`v_remaining_real_amount` AS `v_remaining_real_amount`,`v_coupon_top_up`.`v_remaining_free_amount` AS `v_remaining_free_amount`,`v_coupon_top_up`.`v_total_remaining_amount` AS `v_total_remaining_amount` from ((((((`coupon` left join `member` on(`member`.`member_id` = (`coupon`.`coupon_id` = 0))) left join `option_coupon` on(`option_coupon`.`op_c_id` = `coupon`.`op_c_id`)) left join `user_group` on(`user_group`.`group_id` = `option_coupon`.`group_id`)) left join `type` on(`type`.`type_id` = `user_group`.`type_id`)) left join `v_coupon_top_up` on(`v_coupon_top_up`.`v_ct_coupon_id` = `coupon`.`coupon_id`)) left join `v_member_top_up` on(`v_member_top_up`.`v_mt_member_id` = `member`.`member_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `v_all_user`
--
DROP TABLE IF EXISTS `v_all_user`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_all_user`  AS  select `staff`.`staff_id` AS `v_staff_id`,`v_all_customer`.`v_member_id` AS `v_member_id`,`v_all_customer`.`v_coupon_id` AS `v_coupon_id`,`staff`.`staff_username` AS `v_all_username`,`staff`.`staff_password` AS `v_all_password`,`staff`.`staff_c_date` AS `v_all_c_date`,`staff`.`staff_s_date` AS `v_all_s_date`,`staff`.`staff_e_date` AS `v_all_e_date`,`v_all_customer`.`v_all_create_by` AS `v_all_create_by`,`v_all_customer`.`v_op_c_id` AS `v_op_c_id`,`staff`.`group_id` AS `v_all_group_id`,`user_group`.`group_name` AS `v_all_group_name`,`user_group`.`group_rate` AS `v_all_group_rate`,`type`.`type_id` AS `v_all_type_id`,`type`.`type_name` AS `v_all_type_name` from (((`staff` left join `v_all_customer` on(`v_all_customer`.`v_member_id` <> `staff`.`staff_id` and `v_all_customer`.`v_coupon_id` <> `staff`.`staff_id`)) left join `user_group` on(`user_group`.`group_id` = `staff`.`group_id`)) left join `type` on(`type`.`type_id` = `user_group`.`type_id`)) union all select `staff`.`staff_id` AS `v_staff_id`,`v_all_customer`.`v_member_id` AS `v_member_id`,`v_all_customer`.`v_coupon_id` AS `v_coupon_id`,`v_all_customer`.`v_all_username` AS `v_all_username`,`v_all_customer`.`v_all_password` AS `v_all_password`,`v_all_customer`.`v_all_c_date` AS `v_all_c_date`,`v_all_customer`.`v_all_s_date` AS `v_all_s_date`,`v_all_customer`.`v_all_e_date` AS `v_all_e_date`,`v_all_customer`.`v_all_create_by` AS `v_all_create_by`,`v_all_customer`.`v_op_c_id` AS `v_op_c_id`,`v_all_customer`.`v_all_group_id` AS `v_all_group_id`,`v_all_customer`.`v_all_group_name` AS `v_all_group_name`,`v_all_customer`.`v_all_group_rate` AS `v_all_group_rate`,`v_all_customer`.`v_all_type_id` AS `v_all_type_id`,`v_all_customer`.`v_all_type_name` AS `v_all_type_name` from (`v_all_customer` left join `staff` on(`staff`.`staff_id` <> `v_all_customer`.`v_member_id` and `staff`.`staff_id` <> `v_all_customer`.`v_coupon_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `v_computer`
--
DROP TABLE IF EXISTS `v_computer`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_computer`  AS  select `online`.`online_pc_id` AS `v_pc_id`,`computer`.`pc_name` AS `v_pc_name`,`computer`.`pc_ip_address` AS `v_ip_address`,`computer`.`pc_mac_address` AS `v_mac_address`,`online`.`online_id` AS `v_online_id`,`online`.`online_member_id` AS `v_online_member_id`,`online`.`online_coupon_id` AS `v_online_coupon_id`,`online`.`online_status` AS `v_online_status`,`online`.`online_ordinal` AS `v_online_ordinal`,`online`.`online_s_datetime` AS `v_online_s_datetime`,`online`.`online_e_datetime` AS `v_online_e_datetime`,`online`.`online_use_real_amount` AS `v_online_use_real_amount_last`,`online`.`online_use_free_amount` AS `v_online_use_free_amount_last`,coalesce(`online`.`online_use_real_amount`,0) + coalesce(`online`.`online_use_free_amount`,0) AS `v_online_total_use_amount_last` from (`v_online_ordinal` left join (`online` left join `computer` on(`online`.`online_pc_id` = `computer`.`pc_id`)) on(`v_online_ordinal`.`v_pc_id` = `online`.`online_pc_id` and `v_online_ordinal`.`v_last_ordinal` = `online`.`online_ordinal`)) ;

-- --------------------------------------------------------

--
-- Structure for view `v_computer_status`
--
DROP TABLE IF EXISTS `v_computer_status`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_computer_status`  AS  select `v_computer`.`v_pc_id` AS `v_pc_id`,`v_computer`.`v_pc_name` AS `v_pc_name`,`v_computer`.`v_ip_address` AS `v_ip_address`,`v_computer`.`v_mac_address` AS `v_mac_address`,`v_computer`.`v_online_id` AS `v_online_id`,`v_computer`.`v_online_ordinal` AS `v_online_ordinal`,`v_computer`.`v_online_member_id` AS `v_online_member_id`,`v_computer`.`v_online_coupon_id` AS `v_online_coupon_id`,`v_computer`.`v_online_status` AS `v_online_status`,`v_computer`.`v_online_s_datetime` AS `v_online_s_datetime`,substring_index(`v_computer`.`v_online_s_datetime`,' ',1) AS `v_online_s_date`,substring_index(`v_computer`.`v_online_s_datetime`,' ',-1) AS `v_online_s_time`,`v_computer`.`v_online_e_datetime` AS `v_online_e_datetime`,substring_index(`v_computer`.`v_online_e_datetime`,' ',1) AS `v_online_e_date`,substring_index(`v_computer`.`v_online_e_datetime`,' ',-1) AS `v_online_e_time`,`v_computer`.`v_online_use_real_amount_last` AS `v_online_use_real_amount_last`,`v_computer`.`v_online_use_free_amount_last` AS `v_online_use_free_amount_last`,`v_computer`.`v_online_total_use_amount_last` AS `v_online_total_use_amount_last`,`v_all_customer`.`v_member_id` AS `v_member_id`,`v_all_customer`.`v_coupon_id` AS `v_coupon_id`,`v_all_customer`.`v_all_create_by` AS `v_all_create_by`,`v_all_customer`.`v_all_username` AS `v_all_username`,`v_all_customer`.`v_all_password` AS `v_all_password`,`v_all_customer`.`v_all_c_date` AS `v_all_c_date`,`v_all_customer`.`v_all_s_date` AS `v_all_s_date`,`v_all_customer`.`v_all_e_date` AS `v_all_e_date`,`v_all_customer`.`v_op_c_id` AS `v_op_c_id`,`v_all_customer`.`v_all_group_id` AS `v_all_group_id`,`v_all_customer`.`v_all_group_name` AS `v_all_group_name`,`v_all_customer`.`v_all_group_rate` AS `v_all_group_rate`,`v_all_customer`.`v_group_bonus_status` AS `v_group_bonus_status`,`v_all_customer`.`v_all_type_id` AS `v_all_type_id`,`v_all_customer`.`v_all_type_name` AS `v_all_type_name`,`v_all_customer`.`v_member_name` AS `v_member_name`,`v_all_customer`.`v_member_nickname` AS `v_member_nickname`,`v_all_customer`.`v_member_lastname` AS `v_member_lastname`,`v_all_customer`.`v_member_birthday` AS `v_member_birthday`,`v_all_customer`.`v_member_address` AS `v_member_address`,`v_all_customer`.`v_member_id_card` AS `v_member_id_card`,`v_all_customer`.`v_member_tel` AS `v_member_tel`,`v_all_customer`.`v_member_email` AS `v_member_email`,`v_all_customer`.`v_member_credit_limit` AS `v_member_credit_limit`,`v_all_customer`.`v_all_ordinal_last` AS `v_all_ordinal_last`,`v_all_customer`.`v_all_total_real_amount` AS `v_all_total_real_amount`,`v_all_customer`.`v_all_total_free_amount` AS `v_all_total_free_amount`,`v_all_customer`.`v_total_debt_amount` AS `v_total_debt_amount`,`v_all_customer`.`v_total_pay_debt` AS `v_total_pay_debt`,`v_all_customer`.`v_get_bonus` AS `v_get_bonus`,`v_all_customer`.`v_total_bonus_amount` AS `v_total_bonus_amount`,`v_all_customer`.`v_get_promotion` AS `v_get_promotion`,`v_all_customer`.`v_total_promo_amount` AS `v_total_promo_amount`,`v_all_customer`.`v_total_use_bonus_amount` AS `v_total_use_bonus_amount`,`v_all_customer`.`v_remaining_bonus_amount` AS `v_remaining_bonus_amount`,`v_all_customer`.`v_total_real_top_up` AS `v_total_real_top_up`,`v_all_customer`.`v_total_free_top_up` AS `v_total_free_top_up`,`v_all_customer`.`v_all_total_top_up` AS `v_all_total_top_up`,`v_all_customer`.`v_all_use_real_amount` AS `v_all_use_real_amount`,`v_all_customer`.`v_all_use_free_amount` AS `v_all_use_free_amount`,`v_all_customer`.`v_all_total_use_amount` AS `v_all_total_use_amount`,`v_all_customer`.`v_all_remaining_real_amount` AS `v_all_remaining_real_amount`,`v_all_customer`.`v_all_remaining_free_amount` AS `v_all_remaining_free_amount`,`v_all_customer`.`v_all_remaining_amount` AS `v_all_remaining_amount`,floor(3600 / `v_all_customer`.`v_all_group_rate` * `v_all_customer`.`v_all_remaining_amount` / 3600) AS `v_all_hr`,round(3600 / `v_all_customer`.`v_all_group_rate` * `v_all_customer`.`v_all_remaining_amount` / 60 MOD 60,0) AS `v_all_mn`,floor(3600 / `v_all_customer`.`v_all_group_rate` * `v_all_customer`.`v_all_total_use_amount` / 3600) AS `v_all_use_hr`,round(3600 / `v_all_customer`.`v_all_group_rate` * `v_all_customer`.`v_all_total_use_amount` / 60 MOD 60,0) AS `v_all_use_mn` from (`v_computer` left join `v_all_customer` on(`v_all_customer`.`v_member_id` = `v_computer`.`v_online_member_id` or `v_all_customer`.`v_coupon_id` = `v_computer`.`v_online_coupon_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `v_coupon_top_up`
--
DROP TABLE IF EXISTS `v_coupon_top_up`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_coupon_top_up`  AS  select `coupon_top_up`.`ct_coupon_id` AS `v_ct_coupon_id`,`coupon_top_up`.`ct_ordinal` AS `v_ct_ordinal_last`,coalesce(sum(`coupon_top_up`.`ct_real_amount`),0) AS `v_total_real_amount`,coalesce(sum(`coupon_top_up`.`ct_free_amount`),0) AS `v_total_free_amount`,coalesce(sum(`coupon_top_up`.`ct_real_amount`),0) + coalesce(sum(`coupon_top_up`.`ct_free_amount`),0) AS `v_total_top_up`,coalesce(sum(`online`.`online_use_real_amount`),0) AS `v_total_use_real_amount`,coalesce(sum(`online`.`online_use_free_amount`),0) AS `v_total_use_free_amount`,coalesce(sum(`online`.`online_use_real_amount`),0) + coalesce(sum(`online`.`online_use_free_amount`),0) AS `v_total_use_amount`,coalesce(sum(`coupon_top_up`.`ct_real_amount`),0) - coalesce(sum(`online`.`online_use_real_amount`),0) AS `v_remaining_real_amount`,coalesce(sum(`coupon_top_up`.`ct_free_amount`),0) - coalesce(sum(`online`.`online_use_free_amount`),0) AS `v_remaining_free_amount`,coalesce(sum(`coupon_top_up`.`ct_real_amount`),0) - coalesce(sum(`online`.`online_use_real_amount`),0) + (coalesce(sum(`coupon_top_up`.`ct_free_amount`),0) - coalesce(sum(`online`.`online_use_free_amount`),0)) AS `v_total_remaining_amount` from ((`coupon_top_up` join `v_ct_ordinal` on(`v_ct_ordinal`.`v_coupon_id` = `coupon_top_up`.`ct_coupon_id` and `v_ct_ordinal`.`v_last_ordinal` = `coupon_top_up`.`ct_ordinal`)) left join `online` on(`online`.`online_coupon_id` = `coupon_top_up`.`ct_coupon_id`)) group by `coupon_top_up`.`ct_coupon_id` ;

-- --------------------------------------------------------

--
-- Structure for view `v_ct_ordinal`
--
DROP TABLE IF EXISTS `v_ct_ordinal`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_ct_ordinal`  AS  select `coupon_top_up`.`ct_coupon_id` AS `v_coupon_id`,max(`coupon_top_up`.`ct_ordinal`) AS `v_last_ordinal`,max(`coupon_top_up`.`ct_date`) AS `v_last_date` from `coupon_top_up` group by `coupon_top_up`.`ct_coupon_id` ;

-- --------------------------------------------------------

--
-- Structure for view `v_member_top_up`
--
DROP TABLE IF EXISTS `v_member_top_up`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_member_top_up`  AS  select `member_top_up`.`mt_member_id` AS `v_mt_member_id`,`member_top_up`.`mt_ordinal` AS `v_mt_ordinal_last`,coalesce(sum(`member_top_up`.`mt_real_amount`),0) AS `v_total_real_amount`,coalesce(sum(`member_top_up`.`mt_free_amount`),0) AS `v_total_free_amount`,coalesce(sum(`member_top_up`.`mt_debt_amount`),0) AS `v_total_debt_amount`,coalesce(sum(`member_top_up`.`mt_pay_debt`),0) AS `v_total_pay_debt`,count(`member_top_up`.`mt_bonus_id`) AS `v_get_bonus`,coalesce(sum(`bonus`.`bonus_point`),0) AS `v_total_bonus_amount`,count(`member_top_up`.`mt_promotion_id`) AS `v_get_promotion`,coalesce(sum(`promotion`.`promo_rate`),0) AS `v_total_promo_amount`,coalesce(sum(`promotion`.`promo_rate_point`),0) AS `v_total_use_bonus_amount`,coalesce(sum(`bonus`.`bonus_point`),0) - coalesce(sum(`promotion`.`promo_rate_point`),0) AS `v_remaining_bonus_amount`,coalesce(sum(`member_top_up`.`mt_real_amount`),0) + coalesce(sum(`member_top_up`.`mt_debt_amount`),0) AS `v_total_real_top_up`,coalesce(sum(`member_top_up`.`mt_free_amount`),0) + coalesce(sum(`promotion`.`promo_rate`),0) AS `v_total_free_top_up`,coalesce(sum(`member_top_up`.`mt_real_amount`),0) + coalesce(sum(`member_top_up`.`mt_debt_amount`),0) + (coalesce(sum(`member_top_up`.`mt_free_amount`),0) + coalesce(sum(`promotion`.`promo_rate`),0)) AS `v_total_top_up`,coalesce(sum(`online`.`online_use_real_amount`),0) AS `v_total_use_real_amount`,coalesce(sum(`online`.`online_use_free_amount`),0) AS `v_total_use_free_amount`,coalesce(sum(`online`.`online_use_real_amount`),0) + coalesce(sum(`online`.`online_use_free_amount`),0) AS `v_total_use_amount`,coalesce(sum(`member_top_up`.`mt_real_amount`),0) + coalesce(sum(`member_top_up`.`mt_debt_amount`),0) - coalesce(sum(`online`.`online_use_real_amount`),0) AS `v_remaining_real_amount`,coalesce(sum(`member_top_up`.`mt_free_amount`),0) + coalesce(sum(`promotion`.`promo_rate`),0) - coalesce(sum(`online`.`online_use_free_amount`),0) AS `v_remaining_free_amount`,coalesce(sum(`member_top_up`.`mt_real_amount`),0) + coalesce(sum(`member_top_up`.`mt_debt_amount`),0) - coalesce(sum(`online`.`online_use_real_amount`),0) + (coalesce(sum(`member_top_up`.`mt_free_amount`),0) + coalesce(sum(`promotion`.`promo_rate`),0) - coalesce(sum(`online`.`online_use_free_amount`),0)) AS `v_total_remaining_amount` from ((((`member_top_up` join `v_mt_ordinal` on(`v_mt_ordinal`.`v_member_id` = `member_top_up`.`mt_member_id` and `v_mt_ordinal`.`v_last_ordinal` = `member_top_up`.`mt_ordinal`)) left join `bonus` on(`bonus`.`bonus_id` = `member_top_up`.`mt_bonus_id`)) left join `promotion` on(`promotion`.`promo_id` = `member_top_up`.`mt_promotion_id`)) left join `online` on(`online`.`online_member_id` = `member_top_up`.`mt_member_id`)) group by `member_top_up`.`mt_member_id` ;

-- --------------------------------------------------------

--
-- Structure for view `v_mt_ordinal`
--
DROP TABLE IF EXISTS `v_mt_ordinal`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_mt_ordinal`  AS  select `member_top_up`.`mt_member_id` AS `v_member_id`,max(`member_top_up`.`mt_ordinal`) AS `v_last_ordinal`,max(`member_top_up`.`mt_date`) AS `v_last_date` from `member_top_up` group by `member_top_up`.`mt_member_id` ;

-- --------------------------------------------------------

--
-- Structure for view `v_online_history`
--
DROP TABLE IF EXISTS `v_online_history`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_online_history`  AS  select `online`.`online_id` AS `v_online_id`,`online`.`online_pc_id` AS `v_online_pc_id`,`computer`.`pc_name` AS `v_online_pc_name`,`computer`.`pc_ip_address` AS `v_online_pc_ip_address`,`computer`.`pc_mac_address` AS `v_online_pc_mac_address`,`online`.`online_member_id` AS `v_online_member_id`,`online`.`online_coupon_id` AS `v_online_coupon_id`,`online`.`online_status` AS `v_online_status`,`online`.`online_ordinal` AS `v_online_ordinal`,`online`.`online_s_datetime` AS `v_online_s_datetime`,substring_index(`online`.`online_s_datetime`,' ',1) AS `v_online_s_date`,substring_index(`online`.`online_s_datetime`,' ',-1) AS `v_online_s_time`,`online`.`online_e_datetime` AS `v_online_e_datetime`,substring_index(`online`.`online_e_datetime`,' ',1) AS `v_online_e_date`,substring_index(`online`.`online_e_datetime`,' ',-1) AS `v_online_e_time`,`online`.`online_use_real_amount` AS `v_online_use_real_amount`,`online`.`online_use_free_amount` AS `v_online_use_free_amount`,coalesce(`online`.`online_use_real_amount`,0) + coalesce(`online`.`online_use_free_amount`,0) AS `v_online_total_use_amount`,floor(3600 / `v_all_customer`.`v_all_group_rate` * (coalesce(`online`.`online_use_real_amount`,0) + coalesce(`online`.`online_use_free_amount`,0)) / 3600) AS `v_all_hr`,round((3600 / `v_all_customer`.`v_all_group_rate` * coalesce(`online`.`online_use_real_amount`,0) + coalesce(`online`.`online_use_free_amount`,0)) / 60 MOD 60,0) AS `v_all_mn`,`v_all_customer`.`v_member_id` AS `v_member_id`,`v_all_customer`.`v_coupon_id` AS `v_coupon_id`,`v_all_customer`.`v_all_create_by` AS `v_all_create_by`,`v_all_customer`.`v_all_username` AS `v_all_username`,`v_all_customer`.`v_all_password` AS `v_all_password`,`v_all_customer`.`v_all_c_date` AS `v_all_c_date`,`v_all_customer`.`v_all_s_date` AS `v_all_s_date`,`v_all_customer`.`v_all_e_date` AS `v_all_e_date`,`v_all_customer`.`v_op_c_id` AS `v_op_c_id`,`v_all_customer`.`v_all_group_id` AS `v_all_group_id`,`v_all_customer`.`v_all_group_name` AS `v_all_group_name`,`v_all_customer`.`v_all_group_rate` AS `v_all_group_rate`,`v_all_customer`.`v_all_type_id` AS `v_all_type_id`,`v_all_customer`.`v_all_type_name` AS `v_all_type_name`,`v_all_customer`.`v_member_name` AS `v_member_name`,`v_all_customer`.`v_member_nickname` AS `v_member_nickname`,`v_all_customer`.`v_member_lastname` AS `v_member_lastname`,`v_all_customer`.`v_member_birthday` AS `v_member_birthday`,`v_all_customer`.`v_member_address` AS `v_member_address`,`v_all_customer`.`v_member_id_card` AS `v_member_id_card`,`v_all_customer`.`v_member_tel` AS `v_member_tel`,`v_all_customer`.`v_member_email` AS `v_member_email`,`v_all_customer`.`v_member_credit_limit` AS `v_member_credit_limit`,`v_all_customer`.`v_all_ordinal_last` AS `v_all_ordinal_last`,`v_all_customer`.`v_all_total_real_amount` AS `v_all_total_real_amount`,`v_all_customer`.`v_all_total_free_amount` AS `v_all_total_free_amount`,`v_all_customer`.`v_total_debt_amount` AS `v_total_debt_amount`,`v_all_customer`.`v_total_pay_debt` AS `v_total_pay_debt`,`v_all_customer`.`v_get_bonus` AS `v_get_bonus`,`v_all_customer`.`v_total_bonus_amount` AS `v_total_bonus_amount`,`v_all_customer`.`v_get_promotion` AS `v_get_promotion`,`v_all_customer`.`v_total_promo_amount` AS `v_total_promo_amount`,`v_all_customer`.`v_total_use_bonus_amount` AS `v_total_use_bonus_amount`,`v_all_customer`.`v_remaining_bonus_amount` AS `v_remaining_bonus_amount`,`v_all_customer`.`v_total_real_top_up` AS `v_total_real_top_up`,`v_all_customer`.`v_total_free_top_up` AS `v_total_free_top_up`,`v_all_customer`.`v_all_total_top_up` AS `v_all_total_top_up`,`v_all_customer`.`v_all_use_real_amount` AS `v_all_use_real_amount`,`v_all_customer`.`v_all_use_free_amount` AS `v_all_use_free_amount`,`v_all_customer`.`v_all_total_use_amount` AS `v_all_total_use_amount`,`v_all_customer`.`v_all_remaining_real_amount` AS `v_all_remaining_real_amount`,`v_all_customer`.`v_all_remaining_free_amount` AS `v_all_remaining_free_amount`,`v_all_customer`.`v_all_remaining_amount` AS `v_all_remaining_amount` from ((`online` left join `v_all_customer` on(`v_all_customer`.`v_member_id` = `online`.`online_member_id` or `v_all_customer`.`v_coupon_id` = `online`.`online_coupon_id`)) left join `computer` on(`computer`.`pc_id` = `online`.`online_pc_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `v_online_ordinal`
--
DROP TABLE IF EXISTS `v_online_ordinal`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_online_ordinal`  AS  select `online`.`online_pc_id` AS `v_pc_id`,max(`online`.`online_ordinal`) AS `v_last_ordinal`,max(`online`.`online_s_datetime`) AS `v_last_datetime` from `online` group by `online`.`online_pc_id` ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bonus`
--
ALTER TABLE `bonus`
  ADD PRIMARY KEY (`bonus_id`);

--
-- Indexes for table `computer`
--
ALTER TABLE `computer`
  ADD PRIMARY KEY (`pc_id`);

--
-- Indexes for table `coupon`
--
ALTER TABLE `coupon`
  ADD PRIMARY KEY (`coupon_id`);

--
-- Indexes for table `coupon_top_up`
--
ALTER TABLE `coupon_top_up`
  ADD PRIMARY KEY (`ct_id`);

--
-- Indexes for table `member`
--
ALTER TABLE `member`
  ADD PRIMARY KEY (`member_id`);

--
-- Indexes for table `member_top_up`
--
ALTER TABLE `member_top_up`
  ADD PRIMARY KEY (`mt_id`);

--
-- Indexes for table `online`
--
ALTER TABLE `online`
  ADD PRIMARY KEY (`online_id`);

--
-- Indexes for table `option_coupon`
--
ALTER TABLE `option_coupon`
  ADD PRIMARY KEY (`op_c_id`);

--
-- Indexes for table `promotion`
--
ALTER TABLE `promotion`
  ADD PRIMARY KEY (`promo_id`);

--
-- Indexes for table `staff`
--
ALTER TABLE `staff`
  ADD PRIMARY KEY (`staff_id`);

--
-- Indexes for table `type`
--
ALTER TABLE `type`
  ADD PRIMARY KEY (`type_id`);

--
-- Indexes for table `user_group`
--
ALTER TABLE `user_group`
  ADD PRIMARY KEY (`group_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bonus`
--
ALTER TABLE `bonus`
  MODIFY `bonus_id` int(5) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `computer`
--
ALTER TABLE `computer`
  MODIFY `pc_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `coupon`
--
ALTER TABLE `coupon`
  MODIFY `coupon_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `coupon_top_up`
--
ALTER TABLE `coupon_top_up`
  MODIFY `ct_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `member`
--
ALTER TABLE `member`
  MODIFY `member_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `member_top_up`
--
ALTER TABLE `member_top_up`
  MODIFY `mt_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `online`
--
ALTER TABLE `online`
  MODIFY `online_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `option_coupon`
--
ALTER TABLE `option_coupon`
  MODIFY `op_c_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `promotion`
--
ALTER TABLE `promotion`
  MODIFY `promo_id` int(5) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `staff`
--
ALTER TABLE `staff`
  MODIFY `staff_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `type`
--
ALTER TABLE `type`
  MODIFY `type_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `user_group`
--
ALTER TABLE `user_group`
  MODIFY `group_id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
