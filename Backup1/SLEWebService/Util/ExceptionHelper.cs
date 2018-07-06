using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Util
{
    public class ExceptionHelper
    {
        public Dictionary<string, string> S1Dict = null;

        public Dictionary<string, string> S6Dict = null;

        public ExceptionHelper()
        {

            if (S1Dict == null)
                S1Dict = new Dictionary<string, string>();

            if (S6Dict == null)
                S6Dict = new Dictionary<string, string>();

            S1Dict.Add("beginTime", DateTime.Now.ToString("yyyyMMdd"));
            S1Dict.Add("endTime", DateTime.Now.AddDays(2).ToString("yyyyMMdd"));

            S6Dict.Add("beginTime", DateTime.Now.ToString("yyyyMMdd"));
            S6Dict.Add("endTime", DateTime.Now.AddDays(2).ToString("yyyyMMdd"));
        }

        public void ClearDict()
        {
            int nowTime = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            int S1EndTime = Convert.ToInt32(S1Dict["endTime"]);
            int S6EndTime = Convert.ToInt32(S6Dict["endTime"]);
            if (nowTime > S1EndTime)
            {
                S1Dict.Clear();
                S1Dict.Add("beginTime", DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
                S1Dict.Add("endTime", DateTime.Now.AddDays(3).ToString("yyyyMMdd"));
            }

            if (nowTime > S6EndTime)
            {
                S6Dict.Clear();
                S6Dict.Add("beginTime", DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
                S6Dict.Add("endTime", DateTime.Now.AddDays(3).ToString("yyyyMMdd"));
            }
        }
    }
}
