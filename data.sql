USE [master];
GO
/****** Object:  Database [CarSalesDB]    Script Date: 10/21/2025 ******/

-- ✅ Tạo database tương thích với mọi máy
IF DB_ID('CarSalesDB') IS NOT NULL
    DROP DATABASE [CarSalesDB];
GO

CREATE DATABASE [CarSalesDB]
 CONTAINMENT = NONE
 WITH CATALOG_COLLATION = DATABASE_DEFAULT;
GO

-- ✅ Đặt mức tương thích phù hợp (SQL Server 2019 trở về sau)
ALTER DATABASE [CarSalesDB] SET COMPATIBILITY_LEVEL = 150;
GO

-- ✅ Bật full-text nếu có
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
BEGIN
    EXEC [CarSalesDB].[dbo].[sp_fulltext_database] @action = 'enable';
END
GO

-- ✅ Các thiết lập cơ bản, tương thích toàn phiên bản
ALTER DATABASE [CarSalesDB] SET ANSI_NULL_DEFAULT OFF;
ALTER DATABASE [CarSalesDB] SET ANSI_NULLS OFF;
ALTER DATABASE [CarSalesDB] SET ANSI_PADDING OFF;
ALTER DATABASE [CarSalesDB] SET ANSI_WARNINGS OFF;
ALTER DATABASE [CarSalesDB] SET ARITHABORT OFF;
ALTER DATABASE [CarSalesDB] SET AUTO_CLOSE ON;
ALTER DATABASE [CarSalesDB] SET AUTO_SHRINK OFF;
ALTER DATABASE [CarSalesDB] SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE [CarSalesDB] SET CURSOR_CLOSE_ON_COMMIT OFF;
ALTER DATABASE [CarSalesDB] SET CURSOR_DEFAULT  GLOBAL;
ALTER DATABASE [CarSalesDB] SET CONCAT_NULL_YIELDS_NULL OFF;
ALTER DATABASE [CarSalesDB] SET NUMERIC_ROUNDABORT OFF;
ALTER DATABASE [CarSalesDB] SET QUOTED_IDENTIFIER OFF;
ALTER DATABASE [CarSalesDB] SET RECURSIVE_TRIGGERS OFF;
ALTER DATABASE [CarSalesDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF;
ALTER DATABASE [CarSalesDB] SET DATE_CORRELATION_OPTIMIZATION OFF;
ALTER DATABASE [CarSalesDB] SET TRUSTWORTHY OFF;
ALTER DATABASE [CarSalesDB] SET PARAMETERIZATION SIMPLE;
ALTER DATABASE [CarSalesDB] SET READ_COMMITTED_SNAPSHOT ON;
ALTER DATABASE [CarSalesDB] SET RECOVERY SIMPLE;
ALTER DATABASE [CarSalesDB] SET MULTI_USER;
ALTER DATABASE [CarSalesDB] SET PAGE_VERIFY CHECKSUM;
ALTER DATABASE [CarSalesDB] SET TARGET_RECOVERY_TIME = 60 SECONDS;
ALTER DATABASE [CarSalesDB] SET DELAYED_DURABILITY = DISABLED;
GO

USE [CarSalesDB];
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[customerId] [int] NOT NULL,
	[dealerId] [int] NOT NULL,
	[fullName] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[phone] [varchar](20) NULL,
	[birthday] [date] NULL,
 CONSTRAINT [PK__Customer__B611CB7D0049E38E] PRIMARY KEY CLUSTERED 
(
	[customerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer_Temp]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer_Temp](
	[customerId] [int] NOT NULL,
	[dealerId] [int] NOT NULL,
	[fullName] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[phone] [varchar](20) NULL,
	[birthday] [date] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dealer]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dealer](
	[dealerId] [int] NOT NULL,
	[fullName] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[phone] [varchar](20) NULL,
	[transactionId] [int] NULL,
 CONSTRAINT [PK__Dealer__5A9E9D961C30970D] PRIMARY KEY CLUSTERED 
(
	[dealerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DealerContract]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DealerContract](
	[dealerContractId] [int] NOT NULL,
	[dealerId] [int] NOT NULL,
	[manufacturerId] [int] NOT NULL,
	[targetSales] [decimal](12, 2) NULL,
	[creditLimit] [decimal](12, 2) NULL,
	[signedDate] [date] NULL,
 CONSTRAINT [PK__DealerCo__5D9704E720573FF3] PRIMARY KEY CLUSTERED 
(
	[dealerContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[feedbackId] [int] NOT NULL,
	[customerId] [int] NOT NULL,
	[content] [varchar](500) NULL,
	[createdAt] [datetime] NULL,
	[FeedbackDate] [datetime2](7) NULL,
	[Rating] [int] NULL,
 CONSTRAINT [PK__Feedback__2613FD24ACBA58F5] PRIMARY KEY CLUSTERED 
(
	[feedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturer]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturer](
	[manufacturerId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[country] [varchar](100) NULL,
	[address] [varchar](200) NULL,
 CONSTRAINT [PK__Manufact__02B55389ED519028] PRIMARY KEY CLUSTERED 
(
	[manufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[orderId] [int] NOT NULL,
	[dealerId] [int] NOT NULL,
	[customerId] [int] NOT NULL,
	[variantId] [int] NOT NULL,
	[status] [varchar](50) NOT NULL,
	[orderDate] [date] NULL,
 CONSTRAINT [PK__Order__0809335DA5E72A85] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[paymentId] [int] IDENTITY(1,1) NOT NULL,
	[orderId] [int] NOT NULL,
	[amount] [decimal](12, 2) NULL,
	[method] [varchar](50) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[paymentDate] [datetime2](7) NULL,
	[Status] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[paymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promotion]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotion](
	[promotionId] [int] NOT NULL,
	[orderId] [int] NOT NULL,
	[discountAmount] [decimal](12, 2) NOT NULL,
	[promotionCode] [varchar](50) NULL,
	[validUntil] [date] NULL,
 CONSTRAINT [PK__Promotio__99EB696E2E1B6388] PRIMARY KEY CLUSTERED 
(
	[promotionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Quotation]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotation](
	[quotationId] [int] NOT NULL,
	[customerId] [int] NOT NULL,
	[variantId] [int] NOT NULL,
	[dealerId] [int] NOT NULL,
	[price] [decimal](12, 2) NOT NULL,
	[createdAt] [datetime] NULL,
	[status] [varchar](50) NULL,
 CONSTRAINT [PK__Quotatio__7536E352113ABA0A] PRIMARY KEY CLUSTERED 
(
	[quotationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesContract]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesContract](
	[saleContractId] [int] NOT NULL,
	[orderId] [int] NOT NULL,
	[signedDate] [date] NULL,
	[ContractDate] [date] NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[Terms] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
 CONSTRAINT [PK__SalesCon__BBA78B0BB1B2B884] PRIMARY KEY CLUSTERED 
(
	[saleContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestDrive]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestDrive](
	[testDriveId] [int] NOT NULL,
	[customerId] [int] NOT NULL,
	[variantId] [int] NOT NULL,
	[scheduledDate] [date] NULL,
	[status] [varchar](50) NULL,
	[ScheduledTime] [time](7) NULL,
 CONSTRAINT [PK__TestDriv__1BEFF08411737214] PRIMARY KEY CLUSTERED 
(
	[testDriveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userId] [int] NOT NULL,
	[fullName] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[phone] [varchar](20) NULL,
	[password] [varchar](100) NOT NULL,
	[role] [varchar](50) NOT NULL,
	[dealerId] [int] NULL,
	[manufacturerId] [int] NULL,
 CONSTRAINT [PK__User__CB9A1CFF85902E89] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleModel]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleModel](
	[vehicleModelId] [int] IDENTITY(1,1) NOT NULL,
	[manufacturerId] [int] NOT NULL,
	[name] [varchar](100) NOT NULL,
	[category] [varchar](50) NULL,
	[imageUrl] [varchar](500) NULL,
 CONSTRAINT [PK__VehicleM__DF4B1849AF5DCC0A] PRIMARY KEY CLUSTERED 
(
	[vehicleModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleVariant]    Script Date: 10/19/2025 2:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleVariant](
	[variantId] [int] NOT NULL,
	[vehicleModelId] [int] NOT NULL,
	[version] [varchar](50) NOT NULL,
	[color] [varchar](50) NULL,
	[productYear] [int] NULL,
	[price] [decimal](12, 2) NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK__VehicleV__69E44B2D7D537419] PRIMARY KEY CLUSTERED 
(
	[variantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250930130419_InitialCreate', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250930130940_AddImageUrlToVehicleModel', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20251002112215_AddQuantityToVehicleVariant', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20251002113239_AddScheduledTimeToTestDrive', N'8.0.8')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20251002172949_FixFeedbackIdIdentity', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20251002180815_UpdateIdentityColumns', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20251002184613_EnsureIdentityColumns', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20251019040059_FixPaymentIdIdentity', N'8.0.5')
GO
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (1, 1, N'Nguyen Van A', N'a@gmail.com', N'0911111111', CAST(N'1990-01-01' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (2, 1, N'Tran Thi B', N'b@gmail.com', N'0911111112', CAST(N'1991-02-02' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (3, 2, N'Le Van C', N'c@gmail.com', N'0911111113', CAST(N'1992-03-03' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (4, 2, N'Pham Thi D', N'd@gmail.com', N'0911111114', CAST(N'1993-04-04' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (5, 3, N'Hoang Van E', N'e@gmail.com', N'0911111115', CAST(N'1994-05-05' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (6, 3, N'Do Thi F', N'f@gmail.com', N'0911111116', CAST(N'1995-06-06' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (7, 4, N'Bui Van G', N'g@gmail.com', N'0911111117', CAST(N'1996-07-07' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (8, 4, N'Ngo Thi H', N'h@gmail.com', N'0911111118', CAST(N'1997-08-08' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (9, 5, N'Pham Van I', N'i@gmail.com', N'0911111119', CAST(N'1998-09-09' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (10, 5, N'Tran Thi J', N'j@gmail.com', N'0911111120', CAST(N'1999-10-10' AS Date))
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (11, 1, N'th?ng', N'admin@evm.com', N'01209102212', NULL)
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (12, 1, N'th?ng', N'admin@evm.com', N'01209102212', NULL)
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (13, 1, N'my', N'cust1@carsales.com', N'01209102212', NULL)
INSERT [dbo].[Customer] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (14, 1, N'Pham Nhat Thong', N'phamnhatthong1712@gmail.com', N'0366993031', CAST(N'2004-12-17' AS Date))
GO
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (1, 1, N'Nguyen Van A', N'a@gmail.com', N'0911111111', CAST(N'1990-01-01' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (2, 1, N'Tran Thi B', N'b@gmail.com', N'0911111112', CAST(N'1991-02-02' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (3, 2, N'Le Van C', N'c@gmail.com', N'0911111113', CAST(N'1992-03-03' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (4, 2, N'Pham Thi D', N'd@gmail.com', N'0911111114', CAST(N'1993-04-04' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (5, 3, N'Hoang Van E', N'e@gmail.com', N'0911111115', CAST(N'1994-05-05' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (6, 3, N'Do Thi F', N'f@gmail.com', N'0911111116', CAST(N'1995-06-06' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (7, 4, N'Bui Van G', N'g@gmail.com', N'0911111117', CAST(N'1996-07-07' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (8, 4, N'Ngo Thi H', N'h@gmail.com', N'0911111118', CAST(N'1997-08-08' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (9, 5, N'Pham Van I', N'i@gmail.com', N'0911111119', CAST(N'1998-09-09' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (10, 5, N'Tran Thi J', N'j@gmail.com', N'0911111120', CAST(N'1999-10-10' AS Date))
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (11, 1, N'th?ng', N'admin@evm.com', N'01209102212', NULL)
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (12, 1, N'th?ng', N'admin@evm.com', N'01209102212', NULL)
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (13, 1, N'my', N'cust1@carsales.com', N'01209102212', NULL)
INSERT [dbo].[Customer_Temp] ([customerId], [dealerId], [fullName], [email], [phone], [birthday]) VALUES (14, 1, N'Pham Nhat Thong', N'phamnhatthong1712@gmail.com', N'0366993031', CAST(N'2004-12-17' AS Date))
GO
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (1, N'Auto World Dealer', N'dealer1@carsales.com', N'dealer123', N'0901111111', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (2, N'Premium Motors', N'dealer2@carsales.com', N'dealer123', N'0902222222', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (3, N'City Car Dealer', N'dealer3@carsales.com', N'dealer123', N'0903333333', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (4, N'Luxury Auto', N'dealer4@carsales.com', N'dealer123', N'0904444444', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (5, N'Global Cars', N'dealer5@carsales.com', N'dealer123', N'0905555555', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (6, N'Metro Motors', N'dealer6@carsales.com', N'dealer123', N'0906666666', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (7, N'Highway Auto', N'dealer7@carsales.com', N'dealer123', N'0907777777', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (8, N'Speedy Cars', N'dealer8@carsales.com', N'dealer123', N'0908888888', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (9, N'Green Car Dealer', N'dealer9@carsales.com', N'dealer123', N'0909999999', NULL)
INSERT [dbo].[Dealer] ([dealerId], [fullName], [email], [password], [phone], [transactionId]) VALUES (10, N'Future Auto', N'dealer10@carsales.com', N'dealer123', N'0910000000', NULL)
GO
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (1, 1, 1, CAST(100.00 AS Decimal(12, 2)), CAST(1000000.00 AS Decimal(12, 2)), CAST(N'2024-01-01' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (2, 2, 2, CAST(120.00 AS Decimal(12, 2)), CAST(1200000.00 AS Decimal(12, 2)), CAST(N'2024-01-02' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (3, 3, 3, CAST(150.00 AS Decimal(12, 2)), CAST(1500000.00 AS Decimal(12, 2)), CAST(N'2024-01-03' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (4, 4, 4, CAST(200.00 AS Decimal(12, 2)), CAST(2000000.00 AS Decimal(12, 2)), CAST(N'2024-01-04' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (5, 5, 5, CAST(180.00 AS Decimal(12, 2)), CAST(1800000.00 AS Decimal(12, 2)), CAST(N'2024-01-05' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (6, 6, 6, CAST(160.00 AS Decimal(12, 2)), CAST(1600000.00 AS Decimal(12, 2)), CAST(N'2024-01-06' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (7, 7, 7, CAST(140.00 AS Decimal(12, 2)), CAST(1400000.00 AS Decimal(12, 2)), CAST(N'2024-01-07' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (8, 8, 8, CAST(220.00 AS Decimal(12, 2)), CAST(2200000.00 AS Decimal(12, 2)), CAST(N'2024-01-08' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (9, 9, 9, CAST(90.00 AS Decimal(12, 2)), CAST(900000.00 AS Decimal(12, 2)), CAST(N'2024-01-09' AS Date))
INSERT [dbo].[DealerContract] ([dealerContractId], [dealerId], [manufacturerId], [targetSales], [creditLimit], [signedDate]) VALUES (10, 10, 10, CAST(130.00 AS Decimal(12, 2)), CAST(1300000.00 AS Decimal(12, 2)), CAST(N'2024-01-10' AS Date))
GO
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (1, 1, N'Great service!', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (2, 2, N'Good experience.', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-02T00:00:00.0000000' AS DateTime2), 4)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (3, 3, N'Average service.', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-03T00:00:00.0000000' AS DateTime2), 3)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (4, 4, N'Not satisfied.', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-04T00:00:00.0000000' AS DateTime2), 2)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (5, 5, N'Excellent staff!', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-05T00:00:00.0000000' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (6, 6, N'Fast delivery.', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-06T00:00:00.0000000' AS DateTime2), 4)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (7, 7, N'Car was clean.', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-07T00:00:00.0000000' AS DateTime2), 4)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (8, 8, N'Helpful dealer.', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-08T00:00:00.0000000' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (9, 9, N'Paperwork slow.', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-09T00:00:00.0000000' AS DateTime2), 3)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (10, 10, N'Great car!', CAST(N'2025-10-01T11:05:14.267' AS DateTime), CAST(N'2025-01-10T00:00:00.0000000' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (11, 14, N'ok', CAST(N'2025-10-03T00:33:05.727' AS DateTime), CAST(N'2025-10-03T00:33:05.7266667' AS DateTime2), 3)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (12, 14, N'ok', CAST(N'2025-10-03T13:13:01.637' AS DateTime), CAST(N'2025-10-03T13:13:01.6366667' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (13, 14, N'xe ok', CAST(N'2025-10-03T14:28:18.123' AS DateTime), CAST(N'2025-10-03T14:28:18.1233333' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (14, 14, N'ok', CAST(N'2025-10-08T14:30:33.880' AS DateTime), CAST(N'2025-10-08T14:30:33.8800000' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (15, 14, N'ok', CAST(N'2025-10-08T14:34:17.837' AS DateTime), CAST(N'2025-10-08T14:34:17.8366667' AS DateTime2), 5)
INSERT [dbo].[Feedback] ([feedbackId], [customerId], [content], [createdAt], [FeedbackDate], [Rating]) VALUES (16, 14, N'x?n', CAST(N'2025-10-19T14:36:34.433' AS DateTime), CAST(N'2025-10-19T14:36:34.4333333' AS DateTime2), 5)
GO
SET IDENTITY_INSERT [dbo].[Manufacturer] ON 

INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (1, N'Toyota', N'Japan', N'Toyota City, Aichi, Japan')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (2, N'Honda', N'Japan', N'Minato, Tokyo, Japan')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (3, N'Ford', N'USA', N'Dearborn, Michigan, USA')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (4, N'BMW', N'Germany', N'Munich, Germany')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (5, N'Mercedes-Benz', N'Germany', N'Stuttgart, Germany')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (6, N'Hyundai', N'South Korea', N'Seoul, South Korea')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (7, N'Kia', N'South Korea', N'Seoul, South Korea')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (10, N'Chevrolet', N'USA', N'Detroit, Michigan, USA')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (11, N'Toyota', N'Japan', N'Toyota City, Aichi, Japan')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (12, N'Honda', N'Japan', N'Minato, Tokyo, Japan')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (13, N'Ford', N'USA', N'Dearborn, Michigan, USA')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (14, N'BMW', N'Germany', N'Munich, Germany')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (15, N'Mercedes-Benz', N'Germany', N'Stuttgart, Germany')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (16, N'Hyundai', N'South Korea', N'Seoul, South Korea')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (17, N'Kia', N'South Korea', N'Seoul, South Korea')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (20, N'Chevrolet', N'USA', N'Detroit, Michigan, USA')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (24, N'Tesla', N'USA', N'3500 Deer Creek Road, Palo Alto, CA')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (25, N'VinFast', N'Vietnam', N'Hai Phong, Vietnam')
INSERT [dbo].[Manufacturer] ([manufacturerId], [name], [country], [address]) VALUES (26, N'BYD', N'China', N'Shenzhen, Guangdong, China')
SET IDENTITY_INSERT [dbo].[Manufacturer] OFF
GO
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (0, 1, 1, 2, N'Pending', CAST(N'2025-10-01' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (1, 1, 1, 1, N'Pending', CAST(N'2025-01-01' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (2, 1, 2, 2, N'Confirmed', CAST(N'2025-01-02' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (3, 2, 3, 3, N'Shipped', CAST(N'2025-01-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (4, 2, 4, 4, N'Delivered', CAST(N'2025-01-04' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (5, 3, 5, 5, N'Cancelled', CAST(N'2025-01-05' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (6, 3, 6, 6, N'Pending', CAST(N'2025-01-06' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (7, 4, 7, 7, N'Confirmed', CAST(N'2025-01-07' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (8, 4, 8, 8, N'Shipped', CAST(N'2025-01-08' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (9, 5, 9, 9, N'Delivered', CAST(N'2025-01-09' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (10, 5, 10, 10, N'Pending', CAST(N'2025-01-10' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (11, 1, 1, 1, N'Pending', CAST(N'2025-10-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (12, 1, 14, 2, N'Completed', CAST(N'2025-10-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (13, 1, 14, 8, N'Completed', CAST(N'2025-10-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (14, 1, 14, 4, N'Completed', CAST(N'2025-10-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (15, 1, 14, 3, N'Completed', CAST(N'2025-10-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (16, 1, 14, 1, N'Completed', CAST(N'2025-10-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (17, 1, 14, 2, N'Completed', CAST(N'2025-10-03' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (18, 1, 11, 4, N'Pending', CAST(N'2025-10-10' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (19, 1, 11, 4, N'Pending', CAST(N'2025-10-10' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (20, 1, 14, 1, N'Completed', CAST(N'2025-10-14' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (21, 1, 14, 1, N'Completed', CAST(N'2025-10-17' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (22, 1, 14, 1, N'Completed', CAST(N'2025-10-18' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (23, 1, 14, 1, N'Completed', CAST(N'2025-10-19' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (24, 1, 14, 7, N'Completed', CAST(N'2025-10-19' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (25, 1, 14, 1, N'Completed', CAST(N'2025-10-19' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (26, 1, 14, 1, N'Pending', CAST(N'2025-10-19' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (27, 1, 14, 9, N'Pending', CAST(N'2025-10-19' AS Date))
INSERT [dbo].[Order] ([orderId], [dealerId], [customerId], [variantId], [status], [orderDate]) VALUES (28, 1, 14, 1, N'Pending', CAST(N'2025-10-19' AS Date))
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (1, 1, CAST(30000.00 AS Decimal(12, 2)), N'Credit Card', N'Visa', CAST(N'2025-01-02T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (2, 2, CAST(35000.00 AS Decimal(12, 2)), N'Cash', N'Cash', CAST(N'2025-01-03T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (3, 3, CAST(20000.00 AS Decimal(12, 2)), N'Bank Transfer', N'Transfer', CAST(N'2025-01-04T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (4, 4, CAST(25000.00 AS Decimal(12, 2)), N'Credit Card', N'MasterCard', CAST(N'2025-01-05T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (5, 5, CAST(35000.00 AS Decimal(12, 2)), N'Cash', N'Cash', CAST(N'2025-01-06T00:00:00.0000000' AS DateTime2), N'Refunded')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (6, 6, CAST(40000.00 AS Decimal(12, 2)), N'Bank Transfer', N'Transfer', CAST(N'2025-01-07T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (7, 7, CAST(55000.00 AS Decimal(12, 2)), N'Credit Card', N'Visa', CAST(N'2025-01-08T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (8, 8, CAST(65000.00 AS Decimal(12, 2)), N'Cash', N'Cash', CAST(N'2025-01-09T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (9, 9, CAST(70000.00 AS Decimal(12, 2)), N'Bank Transfer', N'Transfer', CAST(N'2025-01-10T00:00:00.0000000' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (10, 10, CAST(45000.00 AS Decimal(12, 2)), N'Credit Card', N'Visa', CAST(N'2025-01-11T00:00:00.0000000' AS DateTime2), N'Pending')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (11, 14, CAST(25000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-03T12:58:23.6207039' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (12, 12, CAST(35000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-03T13:01:57.7642295' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (13, 15, CAST(20000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-03T13:10:09.2704312' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (14, 16, CAST(30000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-03T14:18:44.8280689' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (15, 17, CAST(25000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-03T14:21:12.7450659' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (16, 17, CAST(10000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-03T14:21:52.5895462' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (17, 21, CAST(30000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-19T09:26:11.4803129' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (18, 20, CAST(30000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-19T10:36:52.3397494' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (19, 13, CAST(65000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-19T11:02:27.0912571' AS DateTime2), N'Received')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (20, 24, CAST(55000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-19T11:03:53.9238321' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (21, 23, CAST(30000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-19T11:17:56.9496618' AS DateTime2), N'Completed')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (22, 25, CAST(30000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-19T11:20:18.8676307' AS DateTime2), N'Received')
INSERT [dbo].[Payment] ([paymentId], [orderId], [amount], [method], [PaymentMethod], [paymentDate], [Status]) VALUES (23, 22, CAST(30000.00 AS Decimal(12, 2)), NULL, N'Cash', CAST(N'2025-10-19T11:25:08.0833289' AS DateTime2), N'Completed')
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (1, 1, CAST(500.00 AS Decimal(12, 2)), N'PROMO500', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (2, 2, CAST(1000.00 AS Decimal(12, 2)), N'PROMO1000', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (3, 3, CAST(700.00 AS Decimal(12, 2)), N'PROMO700', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (4, 4, CAST(800.00 AS Decimal(12, 2)), N'PROMO800', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (5, 5, CAST(900.00 AS Decimal(12, 2)), N'PROMO900', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (6, 6, CAST(600.00 AS Decimal(12, 2)), N'PROMO600', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (7, 7, CAST(1500.00 AS Decimal(12, 2)), N'PROMO1500', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (8, 8, CAST(2000.00 AS Decimal(12, 2)), N'PROMO2000', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (9, 9, CAST(1200.00 AS Decimal(12, 2)), N'PROMO1200', CAST(N'2025-12-31' AS Date))
INSERT [dbo].[Promotion] ([promotionId], [orderId], [discountAmount], [promotionCode], [validUntil]) VALUES (10, 10, CAST(300.00 AS Decimal(12, 2)), N'PROMO300', CAST(N'2025-12-31' AS Date))
GO
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (1, 1, 1, 1, CAST(30000.00 AS Decimal(12, 2)), CAST(N'2025-01-01T00:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (2, 2, 2, 1, CAST(35000.00 AS Decimal(12, 2)), CAST(N'2025-01-02T00:00:00.000' AS DateTime), N'Accepted')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (3, 3, 3, 2, CAST(20000.00 AS Decimal(12, 2)), CAST(N'2025-01-03T00:00:00.000' AS DateTime), N'Rejected')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (4, 4, 4, 2, CAST(25000.00 AS Decimal(12, 2)), CAST(N'2025-01-04T00:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (5, 5, 5, 3, CAST(35000.00 AS Decimal(12, 2)), CAST(N'2025-01-05T00:00:00.000' AS DateTime), N'Accepted')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (6, 6, 6, 3, CAST(40000.00 AS Decimal(12, 2)), CAST(N'2025-01-06T00:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (7, 7, 7, 4, CAST(55000.00 AS Decimal(12, 2)), CAST(N'2025-01-07T00:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (8, 8, 8, 4, CAST(65000.00 AS Decimal(12, 2)), CAST(N'2025-01-08T00:00:00.000' AS DateTime), N'Rejected')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (9, 9, 9, 5, CAST(70000.00 AS Decimal(12, 2)), CAST(N'2025-01-09T00:00:00.000' AS DateTime), N'Accepted')
INSERT [dbo].[Quotation] ([quotationId], [customerId], [variantId], [dealerId], [price], [createdAt], [status]) VALUES (10, 10, 10, 5, CAST(45000.00 AS Decimal(12, 2)), CAST(N'2025-01-10T00:00:00.000' AS DateTime), N'Sent')
GO
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (1, 1, CAST(N'2025-01-02' AS Date), CAST(N'2025-01-02' AS Date), CAST(30000.00 AS Decimal(18, 2)), N'Full Payment', N'Active')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (2, 2, CAST(N'2025-01-03' AS Date), CAST(N'2025-01-03' AS Date), CAST(35000.00 AS Decimal(18, 2)), N'Installment', N'Active')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (3, 3, CAST(N'2025-01-04' AS Date), CAST(N'2025-01-04' AS Date), CAST(20000.00 AS Decimal(18, 2)), N'Full Payment', N'Closed')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (4, 4, CAST(N'2025-01-05' AS Date), CAST(N'2025-01-05' AS Date), CAST(25000.00 AS Decimal(18, 2)), N'Installment', N'Active')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (5, 5, CAST(N'2025-01-06' AS Date), CAST(N'2025-01-06' AS Date), CAST(35000.00 AS Decimal(18, 2)), N'Full Payment', N'Cancelled')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (6, 6, CAST(N'2025-01-07' AS Date), CAST(N'2025-01-07' AS Date), CAST(40000.00 AS Decimal(18, 2)), N'Installment', N'Active')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (7, 7, CAST(N'2025-01-08' AS Date), CAST(N'2025-01-08' AS Date), CAST(55000.00 AS Decimal(18, 2)), N'Full Payment', N'Active')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (8, 8, CAST(N'2025-01-09' AS Date), CAST(N'2025-01-09' AS Date), CAST(65000.00 AS Decimal(18, 2)), N'Installment', N'Active')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (9, 9, CAST(N'2025-01-10' AS Date), CAST(N'2025-01-10' AS Date), CAST(70000.00 AS Decimal(18, 2)), N'Full Payment', N'Closed')
INSERT [dbo].[SalesContract] ([saleContractId], [orderId], [signedDate], [ContractDate], [TotalAmount], [Terms], [Status]) VALUES (10, 10, CAST(N'2025-01-11' AS Date), CAST(N'2025-01-11' AS Date), CAST(45000.00 AS Decimal(18, 2)), N'Installment', N'Pending')
GO
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (1, 1, 1, CAST(N'2025-02-01' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (2, 2, 2, CAST(N'2025-02-02' AS Date), N'Completed', CAST(N'10:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (3, 3, 3, CAST(N'2025-02-03' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (4, 4, 4, CAST(N'2025-02-04' AS Date), N'Cancelled', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (5, 5, 5, CAST(N'2025-02-05' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (6, 6, 6, CAST(N'2025-02-06' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (7, 7, 7, CAST(N'2025-02-07' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (8, 8, 8, CAST(N'2025-02-08' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (9, 9, 9, CAST(N'2025-02-09' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (10, 10, 10, CAST(N'2025-02-10' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (11, 11, 2, CAST(N'2025-10-04' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (12, 12, 1, CAST(N'2025-10-04' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (13, 13, 5, CAST(N'2025-10-04' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (14, 13, 1, CAST(N'2025-10-04' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (15, 1, 1, CAST(N'2025-10-04' AS Date), N'Cancelled', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (16, 2, 4, CAST(N'2025-10-04' AS Date), N'Completed', NULL)
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (17, 1, 1, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (20, 1, 1, CAST(N'2025-10-03' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (21, 2, 2, CAST(N'2025-10-03' AS Date), N'Completed', CAST(N'10:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (22, 1, 2, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (25, 3, 1, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'14:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (26, 1, 10, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (27, 2, 6, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (32, 1, 7, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (34, 1, 5, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (35, 1, 9, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (36, 1, 8, CAST(N'2025-10-04' AS Date), N'Cancelled', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (37, 1, 4, CAST(N'2025-10-04' AS Date), N'Cancelled', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (41, 1, 2, CAST(N'2025-10-04' AS Date), N'Scheduled', CAST(N'11:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (42, 1, 2, CAST(N'2025-10-04' AS Date), N'Scheduled', CAST(N'12:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (43, 1, 2, CAST(N'2025-10-04' AS Date), N'Confirmed', CAST(N'13:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (44, 1, 1, CAST(N'2025-10-04' AS Date), N'Confirmed', CAST(N'12:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (45, 14, 2, CAST(N'2025-10-04' AS Date), N'Cancelled', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (46, 14, 2, CAST(N'2025-10-04' AS Date), N'Completed', CAST(N'10:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (47, 14, 2, CAST(N'2025-10-05' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (48, 14, 1, CAST(N'2025-10-05' AS Date), N'Completed', CAST(N'08:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (49, 14, 1, CAST(N'2025-10-09' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (50, 14, 9, CAST(N'2025-10-11' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (51, 14, 4, CAST(N'2025-10-11' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (52, 14, 6, CAST(N'2025-10-11' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (53, 14, 5, CAST(N'2025-10-11' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (54, 14, 8, CAST(N'2025-10-11' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (55, 14, 3, CAST(N'2025-10-11' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (56, 14, 6, CAST(N'2025-10-20' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
INSERT [dbo].[TestDrive] ([testDriveId], [customerId], [variantId], [scheduledDate], [status], [ScheduledTime]) VALUES (57, 14, 5, CAST(N'2025-10-20' AS Date), N'Completed', CAST(N'09:00:00' AS Time))
GO
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (0, N'Pham Nhat Thong', N'phamnhatthong1712@gmail.com', N'0366993031', N'123456', N'customer', NULL, NULL)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (4, N'Manufacturer Manager 1', N'manu1@carsales.com', N'0933333333', N'manu123', N'Manufacturer', NULL, 1)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (5, N'Manufacturer Manager 2', N'manu2@carsales.com', N'0934444444', N'manu123', N'Manufacturer', NULL, 2)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (6, N'Customer User 1', N'cust1@carsales.com', N'0945555555', N'cust123', N'Customer', NULL, NULL)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (7, N'Customer User 2', N'cust2@carsales.com', N'0946666666', N'cust123', N'Customer', NULL, NULL)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (8, N'Dealer Manager 3', N'dealeruser3@carsales.com', N'0903333333', N'dealer123', N'Dealer', 3, NULL)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (9, N'Manufacturer Manager 3', N'manu3@carsales.com', N'0937777777', N'manu123', N'Manufacturer', NULL, 3)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (10, N'Customer User 3', N'cust3@carsales.com', N'0947777777', N'cust123', N'Customer', NULL, NULL)
INSERT [dbo].[User] ([userId], [fullName], [email], [phone], [password], [role], [dealerId], [manufacturerId]) VALUES (11, N'Dealer', N'dealer1@carsales.com', N'0123456789', N'dealer123', N'Dealer', 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[VehicleModel] ON 

INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (1, 1, N'Toyota Camry', N'Sedan', N'https://bizweb.dktcdn.net/thumb/grande/100/388/878/products/2b7c6e87fc3672f4509e84d5d7f00c1f.png?v=1729671432427')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (2, 1, N'Toyota Corolla', N'Sedan', N'https://bizweb.dktcdn.net/thumb/1024x1024/100/388/878/products/toyota-corolla-cross-2024-moi-nhat-2.png?v=1715158367267')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (3, 2, N'Honda Civic', N'Sedan', N'https://hondaotodongthap.com.vn/wp-content/uploads/2023/09/Honda-Civic-Type-R-do_.png')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (4, 2, N'Honda CR-V', N'SUV', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT6iGh4j-BRp2Ue79hP3qGfP100toK3RFbpUw&s')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (5, 3, N'Ford Ranger', N'Pickup', N'https://www.centralmotorgroup.co.nz/wp-content/uploads/2023/06/Ranger-Raptor-Arctic-White-1.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (6, 3, N'Ford Mustang', N'Sports', N'https://d2qldpouxvc097.cloudfront.net/image-by-path?bucket=a5-gallery-serverless-prod-chromebucket-1iz9ffi08lwxm&key=439073%2Ffront34%2Flg%2Fa0222d')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (7, 4, N'BMW X5', N'SUV', N'https://bmw-hanoi.vn/wp-content/uploads/2024/01/e274135665c6ce9897d7-removebg-preview.png')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (8, 5, N'Mercedes C-Class', N'Sedan', N'https://images3.kingautos.net/2024/36/5/qaHNz6DCp9Wf1J_Z0g.webp')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (9, 8, N'Tesla Model 3', N'EV Sedan', N'https://xehay.vn/uploads/images/2023/9/01/xehay-tesla-200823-13.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (10, 9, N'VinFast VF9', N'EV SUV', N'https://vinfasttimescity.vn/wp-content/uploads/2022/01/vf9-05.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (18, 24, N'Model S', N'Sedan', N'https://digitalassets.tesla.com/tesla-contents/image/upload/f_auto,q_auto/Model-S-Main-Hero-Desktop-LHD.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (19, 24, N'Model 3', N'Sedan', N'https://digitalassets.tesla.com/tesla-contents/image/upload/f_auto,q_auto/Model-3-Main-Hero-Desktop-LHD.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (20, 24, N'Model Y', N'SUV', N'https://digitalassets.tesla.com/tesla-contents/image/upload/f_auto,q_auto/Model-Y-Main-Hero-Desktop-Global.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (21, 25, N'VF 8', N'SUV', N'https://vinfastauto.com/sites/default/files/2023-05/VF8%20Hero%20Desktop_0.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (22, 25, N'VF 9', N'SUV', N'https://vinfastauto.com/sites/default/files/2023-05/VF9%20Hero%20Desktop_0.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (23, 26, N'Atto 3', N'SUV', N'https://www.byd.com/content/dam/byd/global/vehicles/atto3/hero/atto3-hero-desktop.jpg')
INSERT [dbo].[VehicleModel] ([vehicleModelId], [manufacturerId], [name], [category], [imageUrl]) VALUES (24, 26, N'Han EV', N'Sedan', N'https://www.byd.com/content/dam/byd/global/vehicles/han/hero/han-hero-desktop.jpg')
SET IDENTITY_INSERT [dbo].[VehicleModel] OFF
GO
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (1, 1, N'2.0G', N'Black', 2024, CAST(30000.00 AS Decimal(12, 2)), 10)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (2, 1, N'2.5Q', N'White', 2024, CAST(35000.00 AS Decimal(12, 2)), 10)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (3, 2, N'1.8E', N'Silver', 2024, CAST(20000.00 AS Decimal(12, 2)), 10)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (4, 3, N'1.5 Turbo', N'Blue', 2024, CAST(25000.00 AS Decimal(12, 2)), 10)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (5, 4, N'2.0 Turbo', N'Red', 2024, CAST(35000.00 AS Decimal(12, 2)), 10)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (6, 5, N'Wildtrak', N'Gray', 2024, CAST(40000.00 AS Decimal(12, 2)), 15)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (7, 6, N'GT', N'Yellow', 2024, CAST(55000.00 AS Decimal(12, 2)), 15)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (8, 7, N'xDrive40i', N'Black', 2024, CAST(65000.00 AS Decimal(12, 2)), 15)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (9, 8, N'C300 AMG', N'White', 2024, CAST(70000.00 AS Decimal(12, 2)), 15)
INSERT [dbo].[VehicleVariant] ([variantId], [vehicleModelId], [version], [color], [productYear], [price], [Quantity]) VALUES (10, 9, N'Standard Range', N'Blue', 2024, CAST(45000.00 AS Decimal(12, 2)), 15)
GO
/****** Object:  Index [IX_Customer_dealerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Customer_dealerId] ON [dbo].[Customer]
(
	[dealerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DealerContract_dealerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_DealerContract_dealerId] ON [dbo].[DealerContract]
(
	[dealerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DealerContract_manufacturerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_DealerContract_manufacturerId] ON [dbo].[DealerContract]
(
	[manufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Feedback_customerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Feedback_customerId] ON [dbo].[Feedback]
(
	[customerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_customerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_customerId] ON [dbo].[Order]
(
	[customerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_dealerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_dealerId] ON [dbo].[Order]
(
	[dealerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_variantId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_variantId] ON [dbo].[Order]
(
	[variantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payment_orderId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payment_orderId] ON [dbo].[Payment]
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Promotion_orderId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Promotion_orderId] ON [dbo].[Promotion]
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quotation_customerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Quotation_customerId] ON [dbo].[Quotation]
(
	[customerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quotation_dealerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Quotation_dealerId] ON [dbo].[Quotation]
(
	[dealerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quotation_variantId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Quotation_variantId] ON [dbo].[Quotation]
(
	[variantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SalesContract_orderId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_SalesContract_orderId] ON [dbo].[SalesContract]
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TestDrive_customerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_TestDrive_customerId] ON [dbo].[TestDrive]
(
	[customerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TestDrive_variantId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_TestDrive_variantId] ON [dbo].[TestDrive]
(
	[variantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_User_dealerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_dealerId] ON [dbo].[User]
(
	[dealerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_User_manufacturerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_manufacturerId] ON [dbo].[User]
(
	[manufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VehicleModel_manufacturerId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_VehicleModel_manufacturerId] ON [dbo].[VehicleModel]
(
	[manufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VehicleVariant_vehicleModelId]    Script Date: 10/19/2025 2:55:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_VehicleVariant_vehicleModelId] ON [dbo].[VehicleVariant]
(
	[vehicleModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VehicleVariant] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Customer]  WITH NOCHECK ADD  CONSTRAINT [FK__Customer__dealer__6FE99F9F] FOREIGN KEY([dealerId])
REFERENCES [dbo].[Dealer] ([dealerId])
GO
ALTER TABLE [dbo].[Customer] NOCHECK CONSTRAINT [FK__Customer__dealer__6FE99F9F]
GO
ALTER TABLE [dbo].[DealerContract]  WITH NOCHECK ADD  CONSTRAINT [FK__DealerCon__deale__6C190EBB] FOREIGN KEY([dealerId])
REFERENCES [dbo].[Dealer] ([dealerId])
GO
ALTER TABLE [dbo].[DealerContract] NOCHECK CONSTRAINT [FK__DealerCon__deale__6C190EBB]
GO
ALTER TABLE [dbo].[DealerContract]  WITH NOCHECK ADD  CONSTRAINT [FK__DealerCon__manuf__6D0D32F4] FOREIGN KEY([manufacturerId])
REFERENCES [dbo].[Manufacturer] ([manufacturerId])
GO
ALTER TABLE [dbo].[DealerContract] NOCHECK CONSTRAINT [FK__DealerCon__manuf__6D0D32F4]
GO
ALTER TABLE [dbo].[Feedback]  WITH NOCHECK ADD  CONSTRAINT [FK__Feedback__custom__02FC7413] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Feedback] NOCHECK CONSTRAINT [FK__Feedback__custom__02FC7413]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK__Order__customerI__797309D9] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK__Order__customerI__797309D9]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK__Order__dealerId__787EE5A0] FOREIGN KEY([dealerId])
REFERENCES [dbo].[Dealer] ([dealerId])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK__Order__dealerId__787EE5A0]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK__Order__variantId__7A672E12] FOREIGN KEY([variantId])
REFERENCES [dbo].[VehicleVariant] ([variantId])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK__Order__variantId__7A672E12]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK__Payment__orderId__7D439ABD] FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK__Payment__orderId__7D439ABD]
GO
ALTER TABLE [dbo].[Promotion]  WITH NOCHECK ADD  CONSTRAINT [FK__Promotion__order__00200768] FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
GO
ALTER TABLE [dbo].[Promotion] NOCHECK CONSTRAINT [FK__Promotion__order__00200768]
GO
ALTER TABLE [dbo].[Quotation]  WITH NOCHECK ADD  CONSTRAINT [FK__Quotation__custo__0C85DE4D] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Quotation] NOCHECK CONSTRAINT [FK__Quotation__custo__0C85DE4D]
GO
ALTER TABLE [dbo].[Quotation]  WITH NOCHECK ADD  CONSTRAINT [FK__Quotation__deale__0E6E26BF] FOREIGN KEY([dealerId])
REFERENCES [dbo].[Dealer] ([dealerId])
GO
ALTER TABLE [dbo].[Quotation] NOCHECK CONSTRAINT [FK__Quotation__deale__0E6E26BF]
GO
ALTER TABLE [dbo].[Quotation]  WITH NOCHECK ADD  CONSTRAINT [FK__Quotation__varia__0D7A0286] FOREIGN KEY([variantId])
REFERENCES [dbo].[VehicleVariant] ([variantId])
GO
ALTER TABLE [dbo].[Quotation] NOCHECK CONSTRAINT [FK__Quotation__varia__0D7A0286]
GO
ALTER TABLE [dbo].[SalesContract]  WITH NOCHECK ADD  CONSTRAINT [FK__SalesCont__order__05D8E0BE] FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
GO
ALTER TABLE [dbo].[SalesContract] NOCHECK CONSTRAINT [FK__SalesCont__order__05D8E0BE]
GO
ALTER TABLE [dbo].[TestDrive]  WITH NOCHECK ADD  CONSTRAINT [FK__TestDrive__custo__08B54D69] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[TestDrive] NOCHECK CONSTRAINT [FK__TestDrive__custo__08B54D69]
GO
ALTER TABLE [dbo].[TestDrive]  WITH NOCHECK ADD  CONSTRAINT [FK__TestDrive__varia__09A971A2] FOREIGN KEY([variantId])
REFERENCES [dbo].[VehicleVariant] ([variantId])
GO
ALTER TABLE [dbo].[TestDrive] NOCHECK CONSTRAINT [FK__TestDrive__varia__09A971A2]
GO
ALTER TABLE [dbo].[User]  WITH NOCHECK ADD  CONSTRAINT [FK__User__dealerId__68487DD7] FOREIGN KEY([dealerId])
REFERENCES [dbo].[Dealer] ([dealerId])
GO
ALTER TABLE [dbo].[User] NOCHECK CONSTRAINT [FK__User__dealerId__68487DD7]
GO
ALTER TABLE [dbo].[User]  WITH NOCHECK ADD  CONSTRAINT [FK__User__manufactur__693CA210] FOREIGN KEY([manufacturerId])
REFERENCES [dbo].[Manufacturer] ([manufacturerId])
GO
ALTER TABLE [dbo].[User] NOCHECK CONSTRAINT [FK__User__manufactur__693CA210]
GO
ALTER TABLE [dbo].[VehicleModel]  WITH NOCHECK ADD  CONSTRAINT [FK__VehicleMo__manuf__72C60C4A] FOREIGN KEY([manufacturerId])
REFERENCES [dbo].[Manufacturer] ([manufacturerId])
GO
ALTER TABLE [dbo].[VehicleModel] NOCHECK CONSTRAINT [FK__VehicleMo__manuf__72C60C4A]
GO
ALTER TABLE [dbo].[VehicleVariant]  WITH NOCHECK ADD  CONSTRAINT [FK__VehicleVa__vehic__75A278F5] FOREIGN KEY([vehicleModelId])
REFERENCES [dbo].[VehicleModel] ([vehicleModelId])
GO
ALTER TABLE [dbo].[VehicleVariant] NOCHECK CONSTRAINT [FK__VehicleVa__vehic__75A278F5]
GO
USE [master]
GO
ALTER DATABASE [CarSalesDB] SET  READ_WRITE 
GO
