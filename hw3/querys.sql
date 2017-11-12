SELECT COUNT(*) FROM [rel].[Orders]
SELECT COUNT(*) FROM [dw].[fOrder]
SELECT COUNT(*) FROM [rel].[OrderItems]
SELECT COUNT(*) FROM [dw].[fOrderItem]

SELECT [dw].[dProduct].[ProductName]
	 , COUNT([dw].[dProduct].[ProductID]) AS ProductInOrders
FROM [dw].[fOrderItem]
JOIN [dw].[dProduct] ON [dw].[dProduct].[ProductID] = [dw].[fOrderItem].[ProductID]
GROUP BY [dw].[dProduct].[ProductID], [dw].[dProduct].[ProductName]
ORDER BY ProductInOrders DESC

SELECT [dw].[dProduct].[CategoryName]
	 , COUNT([dw].[dProduct].[CategoryName]) AS CategoryInOrders
FROM [dw].[fOrderItem]
JOIN [dw].[dProduct] ON [dw].[dProduct].[ProductID] = [dw].[fOrderItem].[ProductID]
GROUP BY [dw].[dProduct].[CategoryName]
ORDER BY CategoryInOrders DESC

