INSERT INTO [dbo].[tablica] ([nazTablica], [nazSQLTablica], [sifTipTablica]) values('dOrderDate', 'dOrderDate', 2);			--114
INSERT INTO [dbo].[tablica] ([nazTablica], [nazSQLTablica], [sifTipTablica]) values('dOrderTime', 'dOrderTime', 2);			--115
INSERT INTO [dbo].[tablica] ([nazTablica], [nazSQLTablica], [sifTipTablica]) values('dRequiredDate', 'dRequiredDate', 2);	--116
INSERT INTO [dbo].[tablica] ([nazTablica], [nazSQLTablica], [sifTipTablica]) values('dRequiredTime', 'dRequiredTime', 2);	--117
INSERT INTO [dbo].[tablica] ([nazTablica], [nazSQLTablica], [sifTipTablica]) values('dShipCity', 'dShipCity', 2);			--118
INSERT INTO [dbo].[tablica] ([nazTablica], [nazSQLTablica], [sifTipTablica]) values('dShippedDate', 'dShippedDate', 2);		--119
INSERT INTO [dbo].[tablica] ([nazTablica], [nazSQLTablica], [sifTipTablica]) values('dShippedTime', 'dShippedTime', 2);		--120

-- date views
SELECT * INTO #ta1 FROM tabAtribut WHERE siftablica = 102
UPDATE #ta1 SET siftablica = 114;
INSERT INTO tabAtribut SELECT * FROM #ta1;
DROP TABLE #ta1

SELECT * INTO #ta1 FROM tabAtribut WHERE siftablica = 102
UPDATE #ta1 SET siftablica = 116;
INSERT INTO tabAtribut SELECT * FROM #ta1;
DROP TABLE #ta1

SELECT * INTO #ta1 FROM tabAtribut WHERE siftablica = 102
UPDATE #ta1 SET siftablica = 119;
INSERT INTO tabAtribut SELECT * FROM #ta1;
DROP TABLE #ta1

-- time views
SELECT * INTO #ta1 FROM tabAtribut WHERE siftablica = 103
UPDATE #ta1 SET siftablica = 115;
INSERT INTO tabAtribut SELECT * FROM #ta1;
DROP TABLE #ta1

SELECT * INTO #ta1 FROM tabAtribut WHERE siftablica = 103
UPDATE #ta1 SET siftablica = 117;
INSERT INTO tabAtribut SELECT * FROM #ta1;
DROP TABLE #ta1

SELECT * INTO #ta1 FROM tabAtribut WHERE siftablica = 103
UPDATE #ta1 SET siftablica = 120;
INSERT INTO tabAtribut SELECT * FROM #ta1;
DROP TABLE #ta1

-- shipCity view
SELECT * INTO #ta1 FROM tabAtribut WHERE siftablica = 105
UPDATE #ta1 SET siftablica = 118;
INSERT INTO tabAtribut SELECT * FROM #ta1;
DROP TABLE #ta1

-- set date view ids
UPDATE [dbo].[dimCinj]
SET sifdimtablica = 114
where sifdimtablica = 102 and rbrCinj = 15

UPDATE [dbo].[dimCinj]
SET sifdimtablica = 116
where sifdimtablica = 102 and rbrCinj = 16

UPDATE [dbo].[dimCinj]
SET sifdimtablica = 119
where sifdimtablica = 102 and rbrCinj = 17

-- set time view ids
UPDATE [dbo].[dimCinj]
SET sifdimtablica = 115
where sifdimtablica = 103 and rbrCinj in (18,9)

UPDATE [dbo].[dimCinj]
SET sifdimtablica = 117
where sifdimtablica = 103 and rbrCinj in (19,10)

UPDATE [dbo].[dimCinj]
SET sifdimtablica = 120
where sifdimtablica = 103 and rbrCinj in (20,11)

-- set shipcity view ids
UPDATE [dbo].[dimCinj]
SET sifdimtablica = 118
where sifdimtablica = 105 and rbrCinj = 13