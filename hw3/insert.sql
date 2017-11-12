-------------------------------------------------------------------------------
--								     dCity  								 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[dCity]
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
INSERT INTO [dw].[dCity]
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
		  FROM [rel].[City]
-------------------------------------------------------------------------------
--								    dCustomer								 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[dCustomer]
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
INSERT INTO [dw].[dCustomer]
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
		  FROM [rel].[Customers]
-------------------------------------------------------------------------------
--								   dDiscount								 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[dDiscount]
           ([DiscountDesc])
	SELECT DISTINCT COALESCE([DiscountDesc], 'Unknown discount type') as t
	FROM [rel].[OrderItems]
	ORDER BY t DESC
-------------------------------------------------------------------------------
--							   dDiscountInterval							 --
-------------------------------------------------------------------------------
DECLARE @cnt INT = 0;
WHILE @cnt < 100
BEGIN
	INSERT INTO [dw].dDiscountInterval(
		[DiscountValue]
	   ,[Interval]
	) VALUES (cast(convert(decimal(3,2), cast(@cnt as float)/100) as varchar)
			 ,CAST(convert(decimal(3,2), cast(@cnt/10 as float)/10) AS varchar) + ' - 0.' + CAST(@cnt/10 AS varchar) + '9'
	);
	SET @cnt = @cnt + 1;
END;
INSERT INTO [dw].dDiscountInterval([DiscountValue], [Interval]) VALUES (1.00,'1.00');
INSERT INTO [dw].dDiscountInterval([DiscountValue], [Interval]) VALUES (-1, 'unknown');
INSERT INTO [dw].dDiscountInterval([DiscountValue], [Interval]) VALUES (-1, 'error');
-------------------------------------------------------------------------------
--								    dEmployee								 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[dEmployee]
           ([EmployeeDBID]
           ,[LastName]
           ,[FirstName]
           ,[Title]
           ,[TitleOfCourtesy]
           ,[BirthDate]
           ,[HireDate]
           ,[Address]
		   ,[CityDBID]
           ,[rel].[City]
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
INSERT INTO [dw].[dEmployee]
           ([EmployeeDBID]
           ,[LastName]
           ,[FirstName]
           ,[Title]
           ,[TitleOfCourtesy]
           ,[BirthDate]
           ,[HireDate]
           ,[Address]
		   ,[CityDBID]
           ,[rel].[City]
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
		  ,[rel].[Employees].[CityID]
		  ,COALESCE([CityName], 'unknown')
		  ,COALESCE([HomePhone], 'unknown')
		  ,COALESCE([Extension], 'unknown')
		  ,[Photo]
		  ,COALESCE([Notes], '')
		  ,[ReportsTo]
		  FROM [rel].[Employees] LEFT OUTER JOIN [rel].[City] ON [rel].[Employees].[CityID] = [rel].[City].[CityID]
-------------------------------------------------------------------------------
--								dFreightInterval							 --
-------------------------------------------------------------------------------
DECLARE @cntF decimal(10,1) = 0.0;
WHILE @cntF < 1000
BEGIN
	INSERT INTO [dw].[dFreightInterval] 
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
INSERT INTO [dw].[dFreightInterval] 
		   ([FreightValue]
           ,[IntervalDecimal]
		   ,[IntervalRound]
		) VALUES (
			1000,
			'1000.0 - *',
			'1000 - *'
		)
INSERT INTO [dw].[dFreightInterval] 
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
INSERT INTO [dw].[dPaymentMethod]
           ([PaymentMethodName])
	SELECT DISTINCT COALESCE([PaymentMethod], 'unknown') FROM [rel].[Orders]
-------------------------------------------------------------------------------
--								    dProduct								 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[dProduct]
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
INSERT INTO [dw].[dProduct]
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
		   ,[rel].[Products].[SupplierID]
		   ,COALESCE([rel].[Suppliers].[CompanyName], 'unknown')
		   ,[rel].[Products].[CategoryID]
		   ,COALESCE([rel].[Categories].[CategoryName], 'unknown')
		   ,COALESCE([CountryOfOrigin], 'unknown')
		   ,COALESCE([QuantityPerUnit], 'unknown')
		   ,[UnitPrice]
		   ,[UnitsInStock]
		   FROM [rel].[Products] LEFT OUTER JOIN [rel].[Suppliers] ON [rel].[Products].[SupplierID] = [rel].[Suppliers].[SupplierID]
								 LEFT OUTER JOIN [rel].[Categories] ON [rel].[Products].[CategoryID] = [rel].[Categories].[CategoryID]
-------------------------------------------------------------------------------
--								  dShipper									 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[dShipper]
           ([ShipperDBID]
           ,[CompanyName]
           ,[Phone])
	VALUES(null,
		   'unknown',
		   'unknown')
INSERT INTO [dw].[dShipper]
           ([ShipperDBID]
           ,[CompanyName]
           ,[Phone])
	SELECT [ShipperID]
		  ,COALESCE([CompanyName], 'unknown')
		  ,COALESCE([Phone], 'unknown')
	FROM [rel].[Shippers]

-------------------------------------------------------------------------------
--								  fOrder									 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[fOrder]
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
	SELECT [rel].[Orders].[OrderID]
		  ,[dw].[dCustomer].[CustomerID]
		  ,[dw].[dEmployee].[EmployeeID]
		  ,COALESCE([dw].[dPaymentMethod].[PaymentMethodID], 1)
		  ,COALESCE([dw].[dOrderDate].[DateId], 99991229)
		  ,COALESCE([dw].[dRequiredDate].[DateId], 99991229)
		  ,COALESCE([dw].[dShippedDate].[DateId], 99991229)
		  ,COALESCE([dw].[dOrderTime].[TimeOfDayId] ,86401)
		  ,COALESCE([dw].[dRequiredTime].[TimeOfDayId] ,86401)
		  ,COALESCE([dw].[dShippedTime].[TimeOfDayId] ,86401)
		  ,COALESCE([dw].[dFreightInterval].[FreightID], 1001)
		  ,COALESCE([dw].[dShipCity].[CityID], 1)
		  ,COALESCE([dw].[dShipper].[ShipperID], 1)
		  ,COALESCE([rel].[Orders].[ShipName], 'unknown')
		  ,COALESCE([rel].[Orders].[ShipAddress], 'unknown')
		  ,[rel].[Orders].[Freight]
		  ,DATEDIFF(day, [rel].[Orders].[OrderDate], [rel].[Orders].[ShippedDate])
		  ,sums.[OrderSum]
		  ,sums.[OrderSumWithDiscount]
		  FROM [rel].[Orders] JOIN [dw].[dCustomer] ON [rel].[Orders].[CustomerID] = [dw].[dCustomer].[CustomerDBID]
						JOIN [dw].[dEmployee] ON [rel].[Orders].[EmployeeID] = [dw].[dEmployee].[EmployeeDBID]
						LEFT OUTER JOIN [dw].[dPaymentMethod] ON [rel].[Orders].[PaymentMethod] = [dw].[dPaymentMethod].[PaymentMethodName]
						LEFT OUTER JOIN [dw].[dOrderDate] ON  cast([rel].[Orders].[OrderDate] as date) = [dw].[dOrderDate].[Date]
						LEFT OUTER JOIN [dw].[dRequiredDate] ON cast([rel].[Orders].[RequiredDate] as date) = [dw].[dRequiredDate].[Date]
						LEFT OUTER JOIN [dw].[dShippedDate] ON cast([rel].[Orders].[ShippedDate] as date) = [dw].[dShippedDate].[Date]
						LEFT OUTER JOIN [dw].[dOrderTime] ON cast([rel].[Orders].[OrderDate] as time) = [dw].[dOrderTime].[Time]
						LEFT OUTER JOIN [dw].[dRequiredTime] ON cast([rel].[Orders].[RequiredDate] as time) = [dw].[dRequiredTime].[Time]
						LEFT OUTER JOIN [dw].[dShippedTime] ON cast([rel].[Orders].[ShippedDate] as time) = [dw].[dShippedTime].[Time]
						LEFT OUTER JOIN [dw].[dShipCity] ON [rel].[Orders].[ShipCityId] = [dw].[dShipCity].[CityDBID]
						LEFT OUTER JOIN [dw].[dShipper] ON [rel].[Orders].[ShipVia] = [dw].[dShipper].[ShipperDBID]
						LEFT OUTER JOIN [dw].[dFreightInterval] ON ROUND([rel].[Orders].[Freight], 1) = [dw].[dFreightInterval].[FreightValue]
						LEFT OUTER JOIN (SELECT SUM([UnitPrice]*[Quantity]) AS OrderSum
											   ,SUM(([UnitPrice]-[UnitPrice]*[Discount])*[Quantity]) AS OrderSumWithDiscount
											   ,[OrderID]
										FROM [rel].[OrderItems]
										GROUP BY [OrderID]
									) sums ON sums.[OrderID] = [rel].[Orders].OrderID
-------------------------------------------------------------------------------
--								fOrderItem									 --
-------------------------------------------------------------------------------
INSERT INTO [dw].[fOrderItem]
           ([OrderID]
           ,[ProductID]
           ,[UnitPrice]
           ,[Quantity]
           ,[DiscountValue]
           ,[DiscountID]
		   ,[DiscountIntervalID]
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
	SELECT [dw].[fOrder].[OrderID]
		  ,[dw].[dProduct].[ProductID]
		  ,[rel].[OrderItems].[UnitPrice]
		  ,[rel].[OrderItems].[Quantity]
		  ,[rel].[OrderItems].[Discount]
		  ,COALESCE([dw].[dDiscount].[DiscountID], 1) as DiscountID
		  ,COALESCE([dw].[dDiscountInterval].[DiscountID], 102)
		  ,[rel].[OrderItems].[UnitPrice]*[rel].[OrderItems].[Quantity] as TotalPrice
		  ,([rel].[OrderItems].[UnitPrice]-[rel].[OrderItems].[UnitPrice]*[Discount])*[rel].[OrderItems].[Quantity] as TotalPriceWithDiscount
		  ,[dw].[fOrder].[CustomerID]
		  ,[dw].[fOrder].[EmployeeID]
		  ,[dw].[fOrder].[ShipCityID]
		  ,[dw].[fOrder].[PaymentMethodID]
		  ,[dw].[fOrder].[OrderDateID]
		  ,[dw].[fOrder].[RequiredDateID]
		  ,[dw].[fOrder].[ShippedDateID]
		  ,[dw].[fOrder].[OrderTimeID]
		  ,[dw].[fOrder].[RequiredTimeID]
		  ,[dw].[fOrder].[ShippedTimeID]
		  ,[dw].[fOrder].[ShipperID]
		  ,[dw].[fOrder].[ShipName]
		  ,[dw].[fOrder].[ShipAddress]
		  ,[dw].[fOrder].[DeliveryDays]
		  FROM [rel].[OrderItems] JOIN [dw].[fOrder] ON [rel].[OrderItems].[OrderID] = [dw].[fOrder].[OrderDBID] 
								  JOIN [dw].[dProduct] ON [rel].[OrderItems].[ProductID] = [dw].[dProduct].[ProductDBID]
								  LEFT OUTER JOIN [dw].[dDiscount] ON [rel].[OrderItems].[DiscountDesc] = [dw].[dDiscount].[DiscountDesc]
								  LEFT OUTER JOIN [dw].[dDiscountInterval] ON round([rel].[OrderItems].[Discount],2) = round([dw].[dDiscountInterval].[DiscountValue],2)