-- ===================================================
-- DATABASE SCRIPT FOR WAREHOUSE MANAGEMENT (CA 4)
-- Student: Lê Ngọc Tấn Tài
-- MSSV: 23DH113071
-- Database: Warehouse_23DH113071
-- ===================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Warehouse_23DH113071')
BEGIN
    CREATE DATABASE Warehouse_23DH113071;
END
GO

USE Warehouse_23DH113071;
GO

IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL
    DROP TABLE dbo.Products;
GO

-- Create Table Products
CREATE TABLE dbo.Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL
);
GO

-- Seed Data (At least 5 products, at least 2 categories, attached with student name Tài)
INSERT INTO dbo.Products (ProductName, Category, Price, Quantity) VALUES
(N'Bàn phím cơ Gaming Tài', N'Phụ kiện máy tính Tài', 100.00, 10),
(N'Chuột không dây Tài', N'Phụ kiện máy tính Tài', 50.00, 5),
(N'Tai nghe chụp tai Pro Tài', N'Thiết bị âm thanh Tài', 80.00, 3),
(N'Màn hình hiển thị 24inch Tài', N'Màn hình máy tính Tài', 250.00, 0),
(N'Loa Bluetooth Bass Tài', N'Thiết bị âm thanh Tài', 60.00, 8),
(N'Bàn phím An', N'Phụ kiện máy tính Tài', 120.00, 4);
GO

SELECT * FROM dbo.Products;
GO
