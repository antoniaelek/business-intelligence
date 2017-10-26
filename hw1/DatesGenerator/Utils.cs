using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatesGenerator
{
    public static class Utils
    {
        public enum YesOrNo
        {
            Yes,
            No
        }

        public enum Season
        {
            Spring,
            Summer,
            Autumn,
            Winter
        }

        public static Season GetSeason(this DateTime date)
        {
            float value = (float)date.Month + (float)date.Day / 100;   // <month>.<day(2 digit)>
            if (value < 3.21 || value >= 12.22) return Season.Winter;
            if (value < 6.21) return Season.Spring;
            if (value < 9.23) return Season.Summer;
            return Season.Autumn;
        }
    }
}
