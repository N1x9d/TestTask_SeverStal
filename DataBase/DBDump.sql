CREATE DATABASE  IF NOT EXISTS `mydb` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mydb`;
-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: mydb
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `delivery`
--

DROP TABLE IF EXISTS `delivery`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `delivery` (
  `Id` int NOT NULL,
  `Date` date NOT NULL,
  `ProviderId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Delivery history_providers1_idx` (`ProviderId`),
  CONSTRAINT `fk_Delivery history_providers1` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`ProviderNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `delivery`
--

LOCK TABLES `delivery` WRITE;
/*!40000 ALTER TABLE `delivery` DISABLE KEYS */;
INSERT INTO `delivery` VALUES (1,'2023-10-29',1),(2,'2023-10-29',2),(3,'2023-10-29',1);
/*!40000 ALTER TABLE `delivery` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `delivery_has_producttype`
--

DROP TABLE IF EXISTS `delivery_has_producttype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `delivery_has_producttype` (
  `DeliveryId` int NOT NULL,
  `ProductType_ProductId` int NOT NULL,
  `ProductCount` int NOT NULL,
  PRIMARY KEY (`DeliveryId`,`ProductType_ProductId`),
  KEY `fk_Delivery history_has_ProductType_ProductType1_idx` (`ProductType_ProductId`),
  KEY `fk_Delivery history_has_ProductType_Delivery history1_idx` (`DeliveryId`),
  CONSTRAINT `fk_Delivery history_has_ProductType_Delivery history1` FOREIGN KEY (`DeliveryId`) REFERENCES `delivery` (`Id`),
  CONSTRAINT `fk_Delivery history_has_ProductType_ProductType1` FOREIGN KEY (`ProductType_ProductId`) REFERENCES `producttype` (`ProductId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `delivery_has_producttype`
--

LOCK TABLES `delivery_has_producttype` WRITE;
/*!40000 ALTER TABLE `delivery_has_producttype` DISABLE KEYS */;
INSERT INTO `delivery_has_producttype` VALUES (1,1,1),(1,2,2),(1,3,56),(1,4,7),(2,1,8),(2,2,5),(2,3,4),(2,4,6),(3,1,8),(3,2,5),(3,3,4),(3,4,6);
/*!40000 ALTER TABLE `delivery_has_producttype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pricebydate`
--

DROP TABLE IF EXISTS `pricebydate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pricebydate` (
  `providers_ProviderNumber` int NOT NULL,
  `ProductType_ProductId` int NOT NULL,
  `StartOfPricingDate` date NOT NULL,
  `EndOfPricingDate` date NOT NULL,
  `Price` int DEFAULT NULL,
  PRIMARY KEY (`providers_ProviderNumber`,`ProductType_ProductId`,`StartOfPricingDate`,`EndOfPricingDate`),
  KEY `fk_providers_has_ProductType_ProductType1_idx` (`ProductType_ProductId`),
  KEY `fk_providers_has_ProductType_providers_idx` (`providers_ProviderNumber`),
  CONSTRAINT `fk_providers_has_ProductType_ProductType1` FOREIGN KEY (`ProductType_ProductId`) REFERENCES `producttype` (`ProductId`),
  CONSTRAINT `fk_providers_has_ProductType_providers` FOREIGN KEY (`providers_ProviderNumber`) REFERENCES `providers` (`ProviderNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pricebydate`
--

LOCK TABLES `pricebydate` WRITE;
/*!40000 ALTER TABLE `pricebydate` DISABLE KEYS */;
INSERT INTO `pricebydate` VALUES (1,1,'2023-09-01','2023-11-30',10),(1,2,'2023-09-01','2023-11-30',12),(1,3,'2023-09-01','2023-11-30',15),(1,4,'2023-09-01','2023-11-30',14),(2,1,'2023-09-01','2023-11-30',9),(2,2,'2023-09-01','2023-11-30',13),(2,3,'2023-09-01','2023-11-30',16),(2,4,'2023-09-01','2023-11-30',12),(3,1,'2023-09-01','2023-11-30',11),(3,2,'2023-09-01','2023-11-30',11),(3,3,'2023-09-01','2023-11-30',14),(3,4,'2023-09-01','2023-11-30',11);
/*!40000 ALTER TABLE `pricebydate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `producttype`
--

DROP TABLE IF EXISTS `producttype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `producttype` (
  `ProductId` int NOT NULL AUTO_INCREMENT,
  `ProductName` varchar(45) NOT NULL,
  PRIMARY KEY (`ProductId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producttype`
--

LOCK TABLES `producttype` WRITE;
/*!40000 ALTER TABLE `producttype` DISABLE KEYS */;
INSERT INTO `producttype` VALUES (1,'Груша 1'),(2,'Груша 2'),(3,'Яблоко 1'),(4,'Яблоко 2');
/*!40000 ALTER TABLE `producttype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `providers`
--

DROP TABLE IF EXISTS `providers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providers` (
  `ProviderNumber` int NOT NULL,
  PRIMARY KEY (`ProviderNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `providers`
--

LOCK TABLES `providers` WRITE;
/*!40000 ALTER TABLE `providers` DISABLE KEYS */;
INSERT INTO `providers` VALUES (1),(2),(3);
/*!40000 ALTER TABLE `providers` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-10-29 14:26:00
