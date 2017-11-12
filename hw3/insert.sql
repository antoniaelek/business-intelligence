-------------------------------------------------------------------------------
--								     dCity  								 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[dCity]
           ([CityDBID]
           ,[PostalCode]
           ,[CityName]
           ,[Region]
           ,[Country])
	VALUES(null,
		   'unknown',
		   'unknown',
		   'unknown',
		   'unknown')
INSERT INTO [dbo].[dCity]
           ([CityDBID]
           ,[PostalCode]
           ,[CityName]
           ,[Region]
           ,[Country])
	SELECT [CityID]
		  ,COALESCE([PostalCode], 'unknown')
		  ,COALESCE([CityName], 'unknown')
		  ,COALESCE([Region], 'unknown')
		  ,COALESCE([Country], 'unknown')
		  FROM [dbo].[City]
-------------------------------------------------------------------------------
--								    dCustomer								 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[dCustomer]
           ([CustomerDBID]
           ,[CompanyName]
           ,[ContactName]
           ,[ContactTitle]
           ,[Address]
           ,[CityDBID]
           ,[Phone]
           ,[Fax]) VALUES(
		   null,
		   'unknown',
		   'unknown',
		   'unknown',
		   'unknown',
		   null,
		   'unknown',
		   'unknown')
INSERT INTO [dbo].[dCustomer]
           ([CustomerDBID]
           ,[CompanyName]
           ,[ContactName]
           ,[ContactTitle]
           ,[Address]
           ,[CityDBID]
           ,[Phone]
           ,[Fax])
	SELECT [CustomerID]
		  ,[CompanyName]
		  ,COALESCE([ContactName], 'unknown')
		  ,COALESCE([ContactTitle], 'unknown')
		  ,COALESCE([Address], 'unknown')
		  ,[CityID]
		  ,COALESCE([Phone], 'unknown')
		  ,COALESCE([Fax], 'unknown')
		  FROM [dbo].[Customers]
-------------------------------------------------------------------------------
--								   dDiscount								 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[dDiscount]
           ([DiscountDesc])
	SELECT DISTINCT COALESCE([DiscountDesc], 'Unknown discount type') as t
	FROM [dbo].[OrderItems]
	ORDER BY t DESC
-------------------------------------------------------------------------------
--							   dDiscountInterval							 --
-------------------------------------------------------------------------------
DECLARE @cnt INT = 0;
WHILE @cnt < 100
BEGIN
	INSERT INTO dDiscountInterval(
		[DiscountValue]
	   ,[Interval]
	) VALUES (cast(convert(decimal(3,2), cast(@cnt as float)/100) as varchar)
			 ,CAST(convert(decimal(3,2), cast(@cnt/10 as float)/10) AS varchar) + ' - 0.' + CAST(@cnt/10 AS varchar) + '9'
	);
	SET @cnt = @cnt + 1;
END;
INSERT INTO dDiscountInterval([DiscountValue], [Interval]) VALUES (1.00	,'1.00');
INSERT INTO dDiscountInterval([DiscountValue], [Interval]) VALUES (-1, 'error');
-------------------------------------------------------------------------------
--								    dEmployee								 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[dEmployee]
           ([EmployeeDBID]
           ,[LastName]
           ,[FirstName]
           ,[Title]
           ,[TitleOfCourtesy]
           ,[BirthDate]
           ,[HireDate]
           ,[Address]
		   ,[CityDBID]
           ,[City]
           ,[HomePhone]
           ,[Extension]
           ,[Photo]
           ,[Notes]
           ,[ReportsTo]
		   )VALUES(
		   null,
		   'unknown',
		   'unknown',
		   'unknown',
		   'unknown',
		   null,
		   null,
		   'unknown',
		   null,
		   'unknown',
		   'unknown',
		   'unknown',
		   null,
		   'unknown',
		   null)
INSERT INTO [dbo].[dEmployee]
           ([EmployeeDBID]
           ,[LastName]
           ,[FirstName]
           ,[Title]
           ,[TitleOfCourtesy]
           ,[BirthDate]
           ,[HireDate]
           ,[Address]
		   ,[CityDBID]
           ,[City]
           ,[HomePhone]
           ,[Extension]
           ,[Photo]
           ,[Notes]
           ,[ReportsTo])
	SELECT [EmployeeID]
		  ,[LastName]
		  ,[FirstName]
		  ,COALESCE([Title], 'unknown')
		  ,COALESCE([TitleOfCourtesy], 'unknown')
		  ,[BirthDate]
		  ,[HireDate]
		  ,COALESCE([Address], 'unknown')
		  ,[dbo].[Employees].[CityID]
		  ,COALESCE([CityName], 'unknown')
		  ,COALESCE([HomePhone], 'unknown')
		  ,COALESCE([Extension], 'unknown')
		  ,[Photo]
		  ,COALESCE([Notes], '')
		  ,[ReportsTo]
		  FROM [dbo].[Employees] LEFT OUTER JOIN [dbo].[City] ON [dbo].[Employees].[CityID] = [dbo].[City].[CityID]
-------------------------------------------------------------------------------
--								dFreightInterval							 --
-------------------------------------------------------------------------------
DECLARE @cntF decimal(10,1) = 0.0;
WHILE @cntF < 1000
BEGIN
	INSERT INTO [dbo].[dFreightInterval] 
		   ([FreightValue]
           ,[IntervalDecimal]
		   ,[IntervalRound]
		) VALUES (
			@cntF,
			CASE 
				WHEN FLOOR(@cntF) = @cntF THEN CAST(FLOOR(@cntF) AS varchar) + '.0 - ' + CAST(FLOOR(@cntF) AS varchar) + '.9'
				WHEN CEILING(@cntF) = @cntF THEN CAST(CEILING(@cntF) AS varchar) + '.0 - ' + CAST(CEILING(@cntF) AS varchar) + '.9'
				ELSE CAST(FLOOR(@cntF) AS varchar) + '.0 - ' + CAST(FLOOR(@cntF) AS varchar) + '.9'
			END,
			CAST(FLOOR(@cntF/10)*10 AS varchar) + ' - ' + CAST(FLOOR(@cntF/10)*10+9 AS varchar)
		);
	SET @cntF = @cntF + 0.1;
END;
INSERT INTO [dbo].[dFreightInterval] 
		   ([FreightValue]
           ,[IntervalDecimal]
		   ,[IntervalRound]
		) VALUES (
			1000,
			'1000.0 - *',
			'1000 - *'
		)
INSERT INTO [dbo].[dFreightInterval] 
		   ([FreightValue]
           ,[IntervalDecimal]
		   ,[IntervalRound]
		) VALUES (
			-1,
			'unknown',
			'unknown'
		)
-------------------------------------------------------------------------------
--								 dPaymentMethod								 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[dPaymentMethod]
           ([PaymentMethodName])
	SELECT DISTINCT COALESCE([PaymentMethod], 'unknown') FROM [dbo].[Orders]
-------------------------------------------------------------------------------
--								    dProduct								 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[dProduct]
           ([ProductDBID]
           ,[ProductName]
           ,[SupplierDBID]
           ,[SupplierCompanyName]
           ,[CategoryDBID]
           ,[CategoryName]
           ,[CountryOfOrigin]
           ,[QuantityPerUnit]
           ,[UnitPrice]
           ,[UnitsInStock]
		   )
		   VALUES
		   (null
		   ,'unknown'
		   ,null
		   ,'unknown'
		   ,null
		   ,'unknown'
		   ,'unknown'
		   ,'unknown'
		   ,null
		   ,null
		   )
INSERT INTO [dbo].[dProduct]
           ([ProductDBID]
           ,[ProductName]
           ,[SupplierDBID]
           ,[SupplierCompanyName]
           ,[CategoryDBID]
           ,[CategoryName]
           ,[CountryOfOrigin]
           ,[QuantityPerUnit]
           ,[UnitPrice]
           ,[UnitsInStock])
     SELECT [ProductID]
		   ,[ProductName]
		   ,[dbo].[Products].[SupplierID]
		   ,COALESCE([dbo].[Suppliers].[CompanyName], 'unknown')
		   ,[dbo].[Products].[CategoryID]
		   ,COALESCE([dbo].[Categories].[CategoryName], 'unknown')
		   ,COALESCE([CountryOfOrigin], 'unknown')
		   ,COALESCE([QuantityPerUnit], 'unknown')
		   ,[UnitPrice]
		   ,[UnitsInStock]
		   FROM [dbo].[Products] LEFT OUTER JOIN [dbo].[Suppliers] ON [dbo].[Products].[SupplierID] = [dbo].[Suppliers].[SupplierID]
								 LEFT OUTER JOIN [dbo].[Categories] ON [dbo].[Products].[CategoryID] = [dbo].[Categories].[CategoryID]
-------------------------------------------------------------------------------
--								  dShipper									 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[dShipper]
           ([ShipperDBID]
           ,[CompanyName]
           ,[Phone])
	VALUES(null,
		   'unknown',
		   'unknown')
INSERT INTO [dbo].[dShipper]
           ([ShipperDBID]
           ,[CompanyName]
           ,[Phone])
	SELECT [ShipperID]
		  ,COALESCE([CompanyName], 'unknown')
		  ,COALESCE([Phone], 'unknown')
	FROM [dbo].[Shippers]

-------------------------------------------------------------------------------
--								  fOrder									 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[fOrder]
           ([OrderDBID]
           ,[CustomerID]
           ,[EmployeeID]
           ,[PaymentMethodID]
           ,[OrderDateID]
           ,[RequiredDateID]
           ,[ShippedDateID]
           ,[OrderTimeID]
           ,[RequiredTimeID]
           ,[ShippedTimeID]
           ,[FreightIntervalID]
           ,[ShipCityID]
           ,[ShipperID]
           ,[ShipName]
           ,[ShipAddress]
           ,[Freight]
           ,[DeliveryDays]
           ,[OrderPrice]
		   ,[OrderPriceWithDiscount]
		   )
	SELECT [Orders].[OrderID]
		  ,[dCustomer].[CustomerID]
		  ,[dEmployee].[EmployeeID]
		  ,COALESCE([dPaymentMethod].[PaymentMethodID], 1)
		  ,COALESCE([dOrderDate].[DateId], 99991229)
		  ,COALESCE([dRequiredDate].[DateId], 99991229)
		  ,COALESCE([dShippedDate].[DateId], 99991229)
		  ,COALESCE([dOrderTime].[TimeOfDayId] ,86401)
		  ,COALESCE([dRequiredTime].[TimeOfDayId] ,86401)
		  ,COALESCE([dShippedTime].[TimeOfDayId] ,86401)
		  ,COALESCE([dFreightInterval].[FreightID], 1001)
		  ,COALESCE([dShipCity].[CityID], 1)
		  ,COALESCE([dShipper].[ShipperID], 1)
		  ,COALESCE([Orders].[ShipName], 'unknown')
		  ,COALESCE([Orders].[ShipAddress], 'unknown')
		  ,[Orders].[Freight]
		  ,DATEDIFF(day, [Orders].[OrderDate], [Orders].[ShippedDate])
		  ,sums.[OrderSum]
		  ,sums.[OrderSumWithDiscount]
		  FROM [Orders] JOIN [dCustomer] ON [Orders].[CustomerID] = [dCustomer].[CustomerDBID]
						JOIN [dEmployee] ON [Orders].[EmployeeID] = [dEmployee].[EmployeeDBID]
						LEFT OUTER JOIN [dPaymentMethod] ON [Orders].[PaymentMethod] = [dPaymentMethod].[PaymentMethodName]
						LEFT OUTER JOIN [dOrderDate] ON  cast([Orders].[OrderDate] as date) = [dOrderDate].[Date]
						LEFT OUTER JOIN [dRequiredDate] ON cast([Orders].[RequiredDate] as date) = [dRequiredDate].[Date]
						LEFT OUTER JOIN [dShippedDate] ON cast([Orders].[ShippedDate] as date) = [dShippedDate].[Date]
						LEFT OUTER JOIN [dOrderTime] ON cast([Orders].[OrderDate] as time) = [dOrderTime].[Time]
						LEFT OUTER JOIN [dRequiredTime] ON cast([Orders].[RequiredDate] as time) = [dRequiredTime].[Time]
						LEFT OUTER JOIN [dShippedTime] ON cast([Orders].[ShippedDate] as time) = [dShippedTime].[Time]
						LEFT OUTER JOIN [dShipCity] ON [Orders].[ShipCityId] = [dShipCity].[CityDBID]
						LEFT OUTER JOIN [dShipper] ON [Orders].[ShipVia] = [dShipper].[ShipperDBID]
						LEFT OUTER JOIN [dFreightInterval] ON ROUND([Orders].[Freight], 1) = [dFreightInterval].[FreightValue]
						LEFT OUTER JOIN (SELECT SUM([UnitPrice]*[Quantity]) AS OrderSum
											   ,SUM(([UnitPrice]-[UnitPrice]*[Discount])*[Quantity]) AS OrderSumWithDiscount
											   ,[OrderID]
										FROM [OrderItems]
										GROUP BY [OrderID]
									) sums ON sums.[OrderID] = [Orders].OrderID
-------------------------------------------------------------------------------
--								fOrderItem									 --
-------------------------------------------------------------------------------
INSERT INTO [dbo].[fOrderItem]
           ([OrderID]
           ,[ProductID]
           ,[UnitPrice]
           ,[Quantity]
           ,[DiscountValue]
           ,[DiscountID]
           ,[TotalPrice]
           ,[TotalPriceWithDiscount]
           ,[CustomerID]
           ,[EmployeeID]
           ,[ShipCityID]
           ,[PaymentMethodID]
           ,[OrderDateID]
           ,[RequiredDateID]
           ,[ShippedDateID]
           ,[OrderTimeID]
           ,[RequiredTimeID]
           ,[ShippedTimeID]
           ,[ShipperID]
           ,[ShipName]
           ,[ShipAddress]
		   ,[DeliveryDays])
	SELECT [dbo].[fOrder].[OrderID]
		  ,[dbo].[dProduct].[ProductID]
		  ,[dbo].[OrderItems].[UnitPrice]
		  ,[dbo].[OrderItems].[Quantity]
		  ,[dbo].[OrderItems].[Discount]
		  ,COALESCE([dbo].[dDiscount].[DiscountID], 1) as DiscountID
		  ,[dbo].[OrderItems].[UnitPrice]*[dbo].[OrderItems].[Quantity] as TotalPrice
		  ,([dbo].[OrderItems].[UnitPrice]-[dbo].[OrderItems].[UnitPrice]*[Discount])*[dbo].[OrderItems].[Quantity] as TotalPriceWithDiscount
		  ,[dbo].[fOrder].[CustomerID]
		  ,[dbo].[fOrder].[EmployeeID]
		  ,[dbo].[fOrder].[ShipCityID]
		  ,[dbo].[fOrder].[PaymentMethodID]
		  ,[dbo].[fOrder].[OrderDateID]
		  ,[dbo].[fOrder].[RequiredDateID]
		  ,[dbo].[fOrder].[ShippedDateID]
		  ,[dbo].[fOrder].[OrderTimeID]
		  ,[dbo].[fOrder].[RequiredTimeID]
		  ,[dbo].[fOrder].[ShippedTimeID]
		  ,[dbo].[fOrder].[ShipperID]
		  ,[dbo].[fOrder].[ShipName]
		  ,[dbo].[fOrder].[ShipAddress]
		  ,[dbo].[fOrder].[DeliveryDays]
		  FROM [dbo].[OrderItems] JOIN [dbo].[fOrder] ON [dbo].[OrderItems].[OrderID] = [dbo].[fOrder].[OrderDBID] 
								  JOIN [dbo].[dProduct] ON [dbo].[OrderItems].[ProductID] = [dbo].[dProduct].[ProductDBID]
								  LEFT OUTER JOIN [dbo].[dDiscount] ON [dbo].[OrderItems].[DiscountDesc] = [dbo].[dDiscount].[DiscountDesc]