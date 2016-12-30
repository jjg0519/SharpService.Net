using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheService.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime DateFormat(this DateTime date, string format)
        {
            return Convert.ToDateTime($"{date.ToString("yyyy-MM-dd")} {format}");
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string TimeStamp(this DateTime date)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

    }
}
