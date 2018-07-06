using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;

namespace PlatformLib.Util
{
    public class TimeHelper
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static DateTime GetDayDateTime(DateTime time)
        {
            DateTime dtDay = new DateTime(time.Year, time.Month, time.Day);
            return dtDay;
        }

        public static DateTime? GetDateTimeYyyyMMddHHmmss(string timeYyyyMMddHHmmss)
        {
            DateTime? dtTime = null;

            try
            {
                dtTime = DateTime.ParseExact(timeYyyyMMddHHmmss, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            return dtTime;
        }

        public static string GetTimeStringYyyyMMddHHmmss(DateTime time)
        {
            string timeString = time.ToString("yyyyMMddHHmmss");
            return timeString;
        }
    }
}
