using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using log4net;
using System.Reflection;

namespace PlatformLib.Util
{
    public class ExceptionHelper
    {
        public ConcurrentDictionary<string, string> S1Dict = null;

        public ConcurrentDictionary<string, string> S6Dict = null;

        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public ExceptionHelper()
        {

            if (S1Dict == null)
                S1Dict = new ConcurrentDictionary<string, string>();

            if (S6Dict == null)
                S6Dict = new ConcurrentDictionary<string, string>();

            S1Dict.TryAdd("beginTime", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            S1Dict.TryAdd("endTime", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd" + " 00:00:00"));

            S6Dict.TryAdd("beginTime", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            S6Dict.TryAdd("endTime", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd" + " 00:00:00"));
        }

        public void ClearDict()
        {
            try
            {
                string S1EndTime = S1Dict["endTime"];
                string S6EndTime = S6Dict["endTime"];
                _log.Info("字典S1记录条数：" + S1Dict.Count);
                _log.Info("当前时间为： " + DateTime.Now.ToString() + " 上次终止时间： " + S1EndTime);
                if (DateTime.Compare(DateTime.Now,Convert.ToDateTime(S1EndTime))>0)
                {
                    _log.Info("S1_Clear");
                    S1Dict.Clear();
                    S1Dict.TryAdd("beginTime", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                    S1Dict.TryAdd("endTime", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd" + " 00:00:00"));
                }
                _log.Info("字典S6记录条数：" + S6Dict.Count);
                if (DateTime.Compare(DateTime.Now, Convert.ToDateTime(S6EndTime)) > 0)
                {
                    _log.Info("S6_Clear");
                    S6Dict.Clear();
                    S6Dict.TryAdd("beginTime", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                    S6Dict.TryAdd("endTime", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd" + " 00:00:00"));
                }
            }
            catch(Exception e)
            {
                _log.Error(e.Message);
            }  
        }
    }
}
