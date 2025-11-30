USE `fashion_sales`;

-- Bảng danh mục sản phẩm
CREATE TABLE IF NOT EXISTS `Categories` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(200) NOT NULL,
  `Slug` VARCHAR(200) NOT NULL,
  `ParentId` INT NULL,
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
  `DisplayOrder` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  CONSTRAINT `FK_Categories_Parent`
    FOREIGN KEY (`ParentId`) REFERENCES `Categories` (`Id`)
    ON DELETE RESTRICT
) ENGINE=InnoDB;

-- Bảng sản phẩm
CREATE TABLE IF NOT EXISTS `Products` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NOT NULL,
  `Slug` VARCHAR(255) NOT NULL,
  `CategoryId` INT NOT NULL,
  `Description` TEXT NULL,
  `Price` DECIMAL(18,2) NOT NULL,
  `DiscountPrice` DECIMAL(18,2) NULL,
  `StockQuantity` INT NOT NULL DEFAULT 0,
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
  `CreatedAt` DATETIME NOT NULL,
  `MainImageUrl` VARCHAR(1000) NOT NULL,
  PRIMARY KEY (`Id`),
  CONSTRAINT `FK_Products_Categories`
    FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`)
    ON DELETE RESTRICT
) ENGINE=InnoDB;

-- Đơn hàng
CREATE TABLE IF NOT EXISTS `Orders` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `OrderCode` VARCHAR(50) NOT NULL,
  `UserId` VARCHAR(450) NOT NULL,
  `OrderDate` DATETIME NOT NULL,
  `TotalAmount` DECIMAL(18,2) NOT NULL,
  `Status` INT NOT NULL,
  `ShippingAddress` VARCHAR(500) NOT NULL,
  `Note` VARCHAR(1000) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Orders_OrderCode` (`OrderCode`),
  CONSTRAINT `FK_Orders_AspNetUsers`
    FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`)
    ON DELETE RESTRICT
) ENGINE=InnoDB;

-- Chi tiết đơn hàng
CREATE TABLE IF NOT EXISTS `OrderItems` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `OrderId` INT NOT NULL,
  `ProductId` INT NOT NULL,
  `Quantity` INT NOT NULL,
  `UnitPrice` DECIMAL(18,2) NOT NULL,
  `TotalPrice` DECIMAL(18,2) NOT NULL,
  PRIMARY KEY (`Id`),
  CONSTRAINT `FK_OrderItems_Orders`
    FOREIGN KEY (`OrderId`) REFERENCES `Orders` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_OrderItems_Products`
    FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`)
    ON DELETE RESTRICT
) ENGINE=InnoDB;

-- Thống kê theo ngày
CREATE TABLE IF NOT EXISTS `StatisticsDaily` (
  `StatDate` DATE NOT NULL,
  `TotalRevenue` DECIMAL(18,2) NOT NULL DEFAULT 0,
  `OrderCount` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`StatDate`)
) ENGINE=InnoDB;


