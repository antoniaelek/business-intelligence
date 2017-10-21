using System;
using static DatesGenerator.Utils;
using static JayMuntzCom.HolidayCalculator;

namespace DatesGenerator
{
    public class Day
    {
        public Day(DateTime dt)
        {
            DateId = $"{dt.Year}{dt.Month.ToString("00")}{dt.Day.ToString("00")}";
            Date = dt;
            Type = "Date";
            DayMonthYear = ($"{dt.Day.ToString("00")}.{dt.Month.ToString("00")}.{dt.Year}.");
            DayPart = dt.Day;
            MonthPart = dt.Month;
            YearPart = dt.Year;
            Quarter = ((dt.Month - (dt.Month % 3)) / 3) + 1;
            WeekdayOrdNum = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek;
            WeekdayName = dt.DayOfWeek.ToString();
            MonthName = dt.ToString("MMMM");
            IsWorkDay = WeekdayOrdNum < 6 ? YesOrNo.Yes : YesOrNo.No;
            IsLastDayInMonth = dt.Day == DateTime.DaysInMonth(dt.Year, dt.Month) ? YesOrNo.Yes : YesOrNo.No;
            Season = dt.GetSeason();
            IsHoliday = YesOrNo.No;
        }

        public Day(DateTime dt, Holiday holiday) : this(dt)
        {
            IsWorkDay = YesOrNo.No;
            IsHoliday = YesOrNo.Yes;
            HolidayName = holiday.Name;
        }

        public string DateId { get; }
        public DateTime Date { get; }
        public string Type { get; set; }
        public string DayMonthYear { get; }
        public int DayPart { get; }
        public int MonthPart { get; }
        public int YearPart { get; }
        public int Quarter { get; }
        public int WeekdayOrdNum { get; set; }
        public string WeekdayName { get; set; }
        public string MonthName { get; set; }
        public YesOrNo IsWorkDay { get; set; }
        public YesOrNo IsLastDayInMonth { get; set; }
        public Season Season { get; set; }
        public string Event { get; set; } = string.Empty;
        public YesOrNo IsHoliday { get; set; }
        public string HolidayName { get; set; } = string.Empty;

    }
}
