
CREATE DATABASE NuocHoaShop
GO 

USE NuocHoaShop
GO

CREATE TABLE Provider
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100)
)

CREATE TABLE Category
(
	ID INT IDENTITY PRIMARY KEY,
	CategoryName NVARCHAR(100)
)


CREATE TABLE Product
(
	ID INT IDENTITY PRIMARY KEY,
	IDCategory INT REFERENCES Category (ID), 
	IDProvider int REFERENCES Provider(ID),
	ProductName NVARCHAR(100),
	Price FLOAT,
	Amount INT,
	Poppular bit ,
	Sale bit,
	New bit,
	Image varbinary (MAX),
	UrlImage nvarchar(MAX),
	DESCRIPTION nvarchar(MAX)
)


CREATE TABLE UserWeb
(
	ID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100),
	UserName VARCHAR(100),
	Password VARCHAR(100),
	ReenterPassword VARCHAR(100),
	Phone  INT,
	Email VARCHAR(100),
	Reenteremail VARCHAR(100),
	Address NVARCHAR(100),
	TypeUser INT DEFAULT 0 REFERENCES TypeUser(ID)
)


CREATE TABLE Bill
(
	ID INT IDENTITY PRIMARY KEY,
	IDUser INT REFERENCES dbo.UserWeb (ID),  
	SDT INT,
	Ten_khach_hang NVARCHAR(100),
	Dia_chi NVARCHAR(100),
	Tong_tien INT ,
	Ngay_xuat DATE
)

CREATE TABLE OrderProduct
(
	ID INT IDENTITY PRIMARY KEY,
	OrderDay DATE,
	ShipStatus NVARCHAR(20),
	DeliveryDate DATE,
	PayStatus BIT,
	IDUser INT REFERENCES dbo.UserWeb(ID),
	Discout INT ,
	Cancel bit
)

CREATE TABLE FeedBack
(
	ID INT PRIMARY KEY,
	IDUser INT REFERENCES dbo.UserWeb(ID),
	IDProduct INT REFERENCES Product (ID),
	Noi_Dung NVARCHAR(1000)
)


CREATE TABLE OrderDetails
(
	ID INT IDENTITY PRIMARY KEY,
	IDOrder INT REFERENCES dbo.OrderProduct(ID),
	IDProduct INT REFERENCES dbo.Product(ID),
	ProductName NVARCHAR(100),
	Amount INT ,
	Price FLOAT
)

CREATE TABLE TypeUser
(
	ID INT IDENTITY PRIMARY KEY ,
	Name NVARCHAR(20),
	Disscount INT 
)

CREATE TABLE Permission
(
	ID INT PRIMARY KEY,
	Name NVARCHAR(100)
)

CREATE TABLE UserPermission
(
	IDPermission INT ,
	IDTypeUser INT,
	Note NVARCHAR(50),
	PRIMARY KEY(IDPermission,IDTypeUser)
)

