CREATE TABLE dCustomer (
	[CustomerID] int identity(1,1) primary key,
	[CustomerDBID] [nvarchar](5) NULL,
	[CompanyName] [nvarchar](40) NOT NULL,
	[ContactName] [nvarchar](30) NULL,
	[ContactTitle] [nvarchar](30) NULL,
	[Address] [nvarchar](60) NOT NULL,
	[CityDBID] [int] NULL,
	[Phone] [nvarchar](24) NOT NULL,
	[Fax] [nvarchar](24) NOT NULL
)
CREATE TABLE dCity (
	[CityID] int identity(1,1) primary key,
	[CityDBID] [int] NULL,
	[PostalCode] [nvarchar](10) NOT NULL,
	[CityName] [nvarchar](15) NOT NULL,
	[Region] [nvarchar](15) NOT NULL,
	[Country] [nvarchar](15) NOT NULL
)
CREATE TABLE dDiscountInterval(
	[DiscountID] int identity(1,1) primary key,
	[DiscountValue] decimal(10,2) NOT NULL,
	[Interval] nvarchar(20) NOT NULL
)
CREATE TABLE dShipper (
	[ShipperID] int identity(1,1) primary key,
	[ShipperDBID] [int] NULL,
	[CompanyName] [nvarchar](40) NOT NULL,
	[Phone] [nvarchar](24) NOT NULL
)
CREATE TABLE dPaymentMethod (
	[PaymentMethodID] int identity(1,1) primary key,
	[PaymentMethodName] [nvarchar](60) NOT NULL
)
CREATE TABLE dFreightInterval (
	[FreightID] int identity(1,1) primary key,
	[FreightValue] decimal(10,1) NOT NULL,
	[IntervalDecimal] nvarchar(20) NOT NULL,
	[IntervalRound] nvarchar(20) NOT NULL
)
CREATE TABLE dEmployee (
	[EmployeeID] int identity(1,1) primary key,
	[EmployeeDBID] [int] NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[FirstName] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](30) NOT NULL,
	[TitleOfCourtesy] [nvarchar](25) NULL,
	[BirthDate] [smalldatetime] NULL,
	[HireDate] [smalldatetime] NULL,
	[Address] [nvarchar](60) NOT NULL,
	[City] [nvarchar](15) NOT NULL,
	[CityDBID] [int] NULL,
	[HomePhone] [nvarchar](24) NOT NULL,
	[Extension] [nvarchar](10) NOT NULL,
	[Photo] [image] NULL,
	[Notes] [nvarchar](max) NOT NULL,
	[ReportsTo] [int] NULL,
)
CREATE TABLE dDiscount(
	[DiscountID] int identity(1,1) PRIMARY KEY,
	[DiscountDesc] [nchar](30) NOT NULL,
)
CREATE TABLE dProduct(
	[ProductID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProductDBID] [int] NULL,
	[ProductName] [nvarchar](40) NOT NULL,
	[SupplierDBID] [int] NULL,
	[SupplierCompanyName] [nvarchar](40) NOT NULL,
	[CategoryDBID] [int] NULL,
	[CategoryName] [nvarchar](15) NOT NULL,
	[CountryOfOrigin] [nchar](25) NOT NULL,
	[QuantityPerUnit] [nvarchar](20) NOT NULL,
	[UnitPrice] [money] NULL,
	[UnitsInStock] [smallint] NULL
)
CREATE TABLE fOrder(
	[OrderID] int identity(1,1) primary key,
	[OrderDBID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL foreign key references dCustomer([CustomerID]),
	[EmployeeID] [int] NOT NULL foreign key references dEmployee([EmployeeID]),
	[PaymentMethodID] [int] NOT NULL foreign key references dPaymentMethod([PaymentMethodID]),
	[OrderDateID] [int] NOT NULL foreign key references dDate([DateId]),
	[RequiredDateID] [int] NOT NULL foreign key references dDate([DateId]),
	[ShippedDateID] [int] NOT NULL foreign key references dDate([DateId]),
	[OrderTimeID] [int] NOT NULL foreign key references dTimeOfDay([TimeOfDayId]),
	[RequiredTimeID] [int] NOT NULL foreign key references dTimeOfDay([TimeOfDayId]),
	[ShippedTimeID] [int] NOT NULL foreign key references dTimeOfDay([TimeOfDayId]),
	[FreightIntervalID] [int] NOT NULL foreign key references dFreightInterval([FreightID]),
	[ShipCityID] [int] NOT NULL foreign key references dCity([CityID]),
	[ShipperID] [int] NOT NULL foreign key references dShipper([ShipperID]),
	[ShipName] [nvarchar](40) NOT NULL,
	[ShipAddress] [nvarchar](60) NOT NULL,
	[Freight] [money] NULL,
	[DeliveryDays] int NULL,
	[OrderPrice] money,
	[OrderPriceWithDiscount] money
)
CREATE TABLE fOrderItem(
	[OrderItemID] int identity(1,1) primary key,
	[OrderID] [int] NOT NULL foreign key references fOrder([OrderID]),
	[ProductID] [int] NOT NULL foreign key references dProduct([ProductID]),
	[UnitPrice] [money] NOT NULL,
	[Quantity] [smallint] NOT NULL,
	[DiscountValue] [real] NOT NULL,
	[DiscountID] int not null foreign key references dDiscount([DiscountID]),
	[TotalPrice] [money] NOT NULL,
	[TotalPriceWithDiscount] [money] NOT NULL,
	[CustomerID] [int] NOT NULL foreign key references dCustomer([CustomerID]),
	[EmployeeID] [int] NOT NULL foreign key references dEmployee([EmployeeID]),
	[ShipCityID] [int] NOT NULL foreign key references dCity([CityID]),
	[PaymentMethodID] [int] NOT NULL foreign key references dPaymentMethod([PaymentMethodID]),
	[OrderDateID] [int] NOT NULL foreign key references dDate([DateID]),
	[RequiredDateID] [int] NOT NULL foreign key references dDate([DateID]),
	[ShippedDateID] [int] NOT NULL foreign key references dDate([DateID]),
	[OrderTimeID] [int] NOT NULL foreign key references dTimeOfDay([TimeOfDayId]),
	[RequiredTimeID] [int] NOT NULL foreign key references dTimeOfDay([TimeOfDayId]),
	[ShippedTimeID] [int] NOT NULL foreign key references dTimeOfDay([TimeOfDayId]),
	[ShipperID] [int] NOT NULL foreign key references dShipper([ShipperID]),
	[ShipName] [nvarchar](40) NOT NULL,
	[ShipAddress] [nvarchar](60) NOT NULL,
	[DeliveryDays] int NULL
)