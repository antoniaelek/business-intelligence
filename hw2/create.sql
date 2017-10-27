CREATE TABLE [dbo].[dTimeOfDay](
	[TimeOfDayId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](20) NULL,
	[MinutesCount] [int] NULL,
	[SecondsCount] [int] NULL,
	[Time] [time] NULL,
	[HoursPart] [int] NULL,
	[MinutesPart] [int] NULL,
	[SecondsPart] [int] NULL,
	[Period] [varchar](50) NULL,
);

DECLARE @time DATETIME = '00:00:00.00';
WHILE @time <= '23:59:59.00'
BEGIN
	INSERT INTO dTimeOfDay(
		  [Type]
		, [MinutesCount]
		, [SecondsCount]
		, [Time]
		, [HoursPart]
		, [MinutesPart]
		, [SecondsPart]  
		, [Period]
	) VALUES (
		  'Time'
		, DATEPART(hh, @time) * 60 + DATEPART(n, @time)
		, DATEPART(hh, @time) * 3600 + DATEPART(n, @time) * 60 + DATEPART(ss, @time)
		, @time
		, DATEPART(hh, @time)
		, DATEPART(n, @time)
		, DATEPART(ss, @time)
		, null
	);
	SET @time = DATEADD(ss, 1, @time);
END;

UPDATE [dbo].[dTimeOfDay] SET [Period] = 'Night' WHERE HoursPart IN (0,1,2,3,4,22,23);
UPDATE [dbo].[dTimeOfDay] SET [Period] = 'Morning' WHERE HoursPart IN (5,6,7,8,9,10);
UPDATE [dbo].[dTimeOfDay] SET [Period] = 'Lunch' WHERE HoursPart IN (11,12);
UPDATE [dbo].[dTimeOfDay] SET [Period] = 'Afternoon' WHERE HoursPart IN (13,14,15,16,17);
UPDATE [dbo].[dTimeOfDay] SET [Period] = 'Evening' WHERE HoursPart IN (18,19,20,21);

INSERT INTO dTimeOfDay([Type], [Period]) VALUES ('Unknown', 'N/A'),('Not yet happened', 'N/A'),('Error', 'N/A');
