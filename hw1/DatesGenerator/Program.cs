using JayMuntzCom;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using static JayMuntzCom.HolidayCalculator;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace DatesGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var startYear = int.Parse(ConfigurationManager.AppSettings["startYear"]);
            var endYear = int.Parse(ConfigurationManager.AppSettings["endYear"]);
            var holidaysDefinition = ConfigurationManager.AppSettings["holidaysConfig"];
            var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;

            var dates = GetDaysInYears(startYear, endYear, holidaysDefinition);
            dates.Add(new Day("Unknown") { Date = new DateTime(9999, 12, 29) });
            dates.Add(new Day("Error"){ Date = new DateTime(9999, 12, 30) });
            dates.Add(new Day("Not yet happened") { Date = new DateTime(9999, 12, 31) });

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlConnection.Insert(dates);
            }
        }

        private static List<Day> GetDaysInYears(int startYear, int endYear, string holidaysDefinition)
        {
            var days = new List<Day>();

            var currYear = startYear - 1;
            while (endYear > currYear++)
            {
                var holidays = new HolidayCalculator(DateTime.Parse($"{currYear}-1-1"), holidaysDefinition).OrderedHolidays.OfType<Holiday>().ToArray();

                for (var month = 1; month <= 12; month++)
                {
                    for (var date = new DateTime(currYear, month, 1); date.Month == month; date = date.AddDays(1))
                    {

                        var holiday = holidays.FirstOrDefault(h => h.Date.Date == date.Date);
                        if (holiday != default(Holiday))
                        {
                            days.Add(new Day(date, holiday));
                        }
                        else
                        {
                            days.Add(new Day(date));
                        }
                    }
                }
            }

            return days;
        }
    }
}
