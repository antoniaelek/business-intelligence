SELECT COUNT(*) FROM [dbo].[Orders]
SELECT COUNT(*) FROM [dbo].[fOrder]
SELECT COUNT(*) FROM [dbo].[OrderItems]
SELECT COUNT(*) FROM [dbo].[fOrderItem]

SELECT [dbo].[dProduct].[ProductName]
	 , COUNT([dbo].[dProduct].[ProductID]) AS ProductInOrders
FROM [dbo].[fOrderItem]
JOIN [dbo].[dProduct] ON [dbo].[dProduct].[ProductID] = [dbo].[fOrderItem].[ProductID]
GROUP BY [dbo].[dProduct].[ProductID], [dbo].[dProduct].[ProductName]
ORDER BY ProductInOrders DESC

SELECT [dbo].[dProduct].[CategoryName]
	 , COUNT([dbo].[dProduct].[CategoryName]) AS CategoryInOrders
FROM [dbo].[fOrderItem]
JOIN [dbo].[dProduct] ON [dbo].[dProduct].[ProductID] = [dbo].[fOrderItem].[ProductID]
GROUP BY [dbo].[dProduct].[CategoryName]
ORDER BY CategoryInOrders DESC

