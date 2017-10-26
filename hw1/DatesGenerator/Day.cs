using Dapper.Contrib.Extensions;
using System;
using static DatesGenerator.Utils;
using static JayMuntzCom.HolidayCalculator;

namespace DatesGenerator
{
    [Table("dDate")]
    public class Day
    {
        public Day(string specialEntryType)
        {
            Type = specialEntryType;
        }

        public Day(DateTime dt)
        {
            Date = dt;
            Type = "Date";
            DayMonthYear = ($"{dt.Day.ToString("00")}.{dt.Month.ToString("00")}.{dt.Year}.");
            DayPart = dt.Day;
            MonthPart = dt.Month;
            YearPart = dt.Year;
            Quarter = (dt.Month + 2) / 3;
            WeekdayOrdNum = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek;
            WeekdayName = dt.DayOfWeek.ToString();
            MonthName = dt.ToString("MMMM");
            IsWorkDay = WeekdayOrdNum < 6 ? YesOrNo.Yes.ToString() : YesOrNo.No.ToString();
            IsHoliday = YesOrNo.No.ToString();
            IsLastDayInMonth = dt.Day == DateTime.DaysInMonth(dt.Year, dt.Month) ? YesOrNo.Yes.ToString() : YesOrNo.No.ToString();
            Season = dt.GetSeason().ToString();
            Event = "No event";
        }

        public Day(DateTime dt, Holiday holiday) : this(dt)
        {
            IsWorkDay = YesOrNo.No.ToString();
            IsHoliday = YesOrNo.Yes.ToString();
            HolidayName = holiday.Name;
        }

        [ExplicitKey]
        public string DateId { get { return $"{Date.Year}{Date.Month.ToString("00")}{Date.Day.ToString("00")}"; } }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string DayMonthYear { get; } = "N/A";
        public int DayPart { get; } = -1;
        public int MonthPart { get; } = -1;
        public int YearPart { get; } = -1;
        public int Quarter { get; } = -1;
        public int WeekdayOrdNum { get; set; } = -1;
        public string WeekdayName { get; set; } = "N/A";
        public string MonthName { get; set; } = "N/A";
        public string IsWorkDay { get; set; } = "N/A";
        public string IsLastDayInMonth { get; set; } = "N/A";
        public string Season { get; set; } = "N/A";
        public string Event { get; set; } = "N/A";
        public string IsHoliday { get; set; } = "N/A";
        public string HolidayName { get; set; } = "N/A";

    }
}
