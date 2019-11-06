-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 12, 2019 at 06:50 AM
-- Server version: 10.1.39-MariaDB
-- PHP Version: 7.3.5

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `internet_cafe`
--

-- --------------------------------------------------------

--
-- Table structure for table `admin`
--

CREATE TABLE `admin` (
  `admin_id` int(11) NOT NULL,
  `admin_username` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `admin_password` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `admin_name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `admin_lastname` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `group_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `admin`
--

INSERT INTO `admin` (`admin_id`, `admin_username`, `admin_password`, `admin_name`, `admin_lastname`, `group_id`) VALUES
(1, 'admin', 'admin', 'Thornthan', 'Onnuan', 2);

-- --------------------------------------------------------

--
-- Table structure for table `bonus`
--

CREATE TABLE `bonus` (
  `bonus_id` int(11) NOT NULL,
  `bonus_top_up` float NOT NULL,
  `bonus_point` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `coupon`
--

CREATE TABLE `coupon` (
  `coupon_id` int(11) NOT NULL,
  `coupon_username` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `coupon_password` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `group_id` int(11) NOT NULL,
  `coupon_top_up` float DEFAULT NULL,
  `coupon_rr_amount` float DEFAULT NULL,
  `coupon_s_date` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `coupon_e_date` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `coupon_create_by` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `coupon_create_date` varchar(255) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `member`
--

CREATE TABLE `member` (
  `member_id` int(11) NOT NULL,
  `member_username` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `member_password` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `member_name` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_nickname` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_lastname` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_birthday` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_address` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_identification_number` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_tel` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_email` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_s_date` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_e_date` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `member_credit_limit` float DEFAULT NULL,
  `member_debt` float DEFAULT NULL,
  `member_top_up` float DEFAULT NULL,
  `member_u_amount` float DEFAULT NULL,
  `member_rr_amount` float DEFAULT NULL,
  `member_tf_amount` float DEFAULT NULL,
  `member_rf_amount` float DEFAULT NULL,
  `member_t_amount` float DEFAULT NULL,
  `member_rb_point` float DEFAULT NULL,
  `group_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `member`
--

INSERT INTO `member` (`member_id`, `member_username`, `member_password`, `member_name`, `member_nickname`, `member_lastname`, `member_birthday`, `member_address`, `member_identification_number`, `member_tel`, `member_email`, `member_s_date`, `member_e_date`, `member_credit_limit`, `member_debt`, `member_top_up`, `member_u_amount`, `member_rr_amount`, `member_tf_amount`, `member_rf_amount`, `member_t_amount`, `member_rb_point`, `group_id`) VALUES
(1, 'bas14223', 'bas9352', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4);

-- --------------------------------------------------------

--
-- Table structure for table `option_cafe_shutdown`
--

CREATE TABLE `option_cafe_shutdown` (
  `ocs_id` int(11) NOT NULL,
  `ocs_set` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `ocs_time` varchar(255) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `option_change`
--

CREATE TABLE `option_change` (
  `op_ch_id` int(11) NOT NULL,
  `op_ch_add_remove_pro` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_administrative_tool` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_cd_rom` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_close_client` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_close_pro` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_date_time` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_lock_pc` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_lockoff_pc` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_network_connection` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_on_off_cafe` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_regional_language` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_registry_editer` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_set_time` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_shutdown_restart` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_swicth_user` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_system` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_task_manager` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_ch_user_account` varchar(255) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `option_coupon`
--

CREATE TABLE `option_coupon` (
  `op_c_id` int(11) NOT NULL,
  `op_c_name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `op_c_hr_rate` float NOT NULL,
  `op_c_rr_amount` float DEFAULT NULL,
  `op_c_tf_amount` float DEFAULT NULL,
  `op_c_s_date` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `op_c_e_date` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `group_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `staff`
--

CREATE TABLE `staff` (
  `staff_id` int(11) NOT NULL,
  `staff_username` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `staff_password` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `staff_name` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_nickname` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_lastname` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_birthday` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_address` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_identification_number` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_tel` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_email` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_create_date` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `staff_s_date` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `staff_e_date` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `group_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `staff`
--

INSERT INTO `staff` (`staff_id`, `staff_username`, `staff_password`, `staff_name`, `staff_nickname`, `staff_lastname`, `staff_birthday`, `staff_address`, `staff_identification_number`, `staff_tel`, `staff_email`, `staff_create_date`, `staff_s_date`, `staff_e_date`, `group_id`) VALUES
(1, 'bas14223', 'bas9352', 'thornthan', 'bas', 'onnuan', '1996-12-29', '343', '1909801117648', '0612812641', 'thornthan@gmail.com', '1996-12-29 18:01:50', '', '1996-12-29 18:01:50', 3),
(4, 'nureesun', 'nureesun', 'นูรีซัน', 'ซัน', 'ยะโอ๊ะ', '', '', '', '0000000000', '', '2019-07-04 15:57:54', NULL, '2019-07-31 15:57:54', 3),
(13, 'bas14223', 'bas2539', '', '', '', '', '', '', '', '', '2019-07-12 11:12:28', '', '2562-07-12 11:12:28', 0);

-- --------------------------------------------------------

--
-- Table structure for table `type`
--

CREATE TABLE `type` (
  `type_id` int(11) NOT NULL,
  `type_name` varchar(255) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `type`
--

INSERT INTO `type` (`type_id`, `type_name`) VALUES
(1, 'guest'),
(2, 'admin'),
(3, 'staff'),
(4, 'member'),
(5, 'coupon');

-- --------------------------------------------------------

--
-- Table structure for table `user_group`
--

CREATE TABLE `user_group` (
  `group_id` int(11) NOT NULL,
  `group_name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `group_bonus` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `group_rate` int(255) NOT NULL,
  `type_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `user_group`
--

INSERT INTO `user_group` (`group_id`, `group_name`, `group_bonus`, `group_rate`, `type_id`) VALUES
(1, 'guest', '0', 15, 1),
(2, 'admin', '0', 0, 2),
(3, 'staff', '0', 0, 3),
(4, 'member', '1', 10, 4),
(5, 'coupon', '0', 10, 5);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `admin`
--
ALTER TABLE `admin`
  ADD PRIMARY KEY (`admin_id`),
  ADD KEY `group_id` (`group_id`);

--
-- Indexes for table `bonus`
--
ALTER TABLE `bonus`
  ADD PRIMARY KEY (`bonus_id`);

--
-- Indexes for table `coupon`
--
ALTER TABLE `coupon`
  ADD PRIMARY KEY (`coupon_id`);

--
-- Indexes for table `member`
--
ALTER TABLE `member`
  ADD PRIMARY KEY (`member_id`);

--
-- Indexes for table `option_cafe_shutdown`
--
ALTER TABLE `option_cafe_shutdown`
  ADD PRIMARY KEY (`ocs_id`);

--
-- Indexes for table `option_change`
--
ALTER TABLE `option_change`
  ADD PRIMARY KEY (`op_ch_id`);

--
-- Indexes for table `option_coupon`
--
ALTER TABLE `option_coupon`
  ADD PRIMARY KEY (`op_c_id`);

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
  ADD PRIMARY KEY (`group_id`),
  ADD KEY `fk_type_id` (`type_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `admin`
--
ALTER TABLE `admin`
  MODIFY `admin_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `bonus`
--
ALTER TABLE `bonus`
  MODIFY `bonus_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `coupon`
--
ALTER TABLE `coupon`
  MODIFY `coupon_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `member`
--
ALTER TABLE `member`
  MODIFY `member_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `option_cafe_shutdown`
--
ALTER TABLE `option_cafe_shutdown`
  MODIFY `ocs_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `option_coupon`
--
ALTER TABLE `option_coupon`
  MODIFY `op_c_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `staff`
--
ALTER TABLE `staff`
  MODIFY `staff_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `type`
--
ALTER TABLE `type`
  MODIFY `type_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `user_group`
--
ALTER TABLE `user_group`
  MODIFY `group_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `admin`
--
ALTER TABLE `admin`
  ADD CONSTRAINT `admin_ibfk_1` FOREIGN KEY (`group_id`) REFERENCES `user_group` (`group_id`);

--
-- Constraints for table `coupon`
--
ALTER TABLE `coupon`
  ADD CONSTRAINT `coupon_fk_group_id` FOREIGN KEY (`group_id`) REFERENCES `user_group` (`group_id`);

--
-- Constraints for table `member`
--
ALTER TABLE `member`
  ADD CONSTRAINT `member_fk_group_id` FOREIGN KEY (`group_id`) REFERENCES `user_group` (`group_id`);

--
-- Constraints for table `option_coupon`
--
ALTER TABLE `option_coupon`
  ADD CONSTRAINT `oc_fk_group_id` FOREIGN KEY (`group_id`) REFERENCES `user_group` (`group_id`);

--
-- Constraints for table `user_group`
--
ALTER TABLE `user_group`
  ADD CONSTRAINT `fk_type_id` FOREIGN KEY (`type_id`) REFERENCES `type` (`type_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
