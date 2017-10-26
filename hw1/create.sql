CREATE TABLE dDate(
	[DateId] int PRIMARY KEY,
	[Date] date,
	[Type] nvarchar(20),
	[DayMonthYear] varchar(20),
	[DayPart] int,
	[MonthPart] int,
	[YearPart] int,
	[Quarter] varchar(50),
	[WeekdayOrdNum] int,
	[WeekdayName] varchar(50),
	[MonthName] varchar(50),
	[IsWorkDay] varchar(50),
	[IsLastDayInMonth] varchar(50),
	[Season] varchar(50),
	[Event] nvarchar(max),
	[IsHoliday] varchar(50),
	[HolidayName] varchar(50)
);