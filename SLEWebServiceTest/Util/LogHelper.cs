using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using log4net;

namespace SLEWebServiceTest.Util
{
    public class LogHelper
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public static string GetObjectMemberString(Object obj)
        {
            string strObjectMemberString = String.Empty;

            try
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    PropertyInfo[] propertys = obj.GetType().GetProperties();
                    foreach (PropertyInfo pinfo in propertys)
                    {
                        sb.Append(String.Format(",{0}:{1}", pinfo.Name, pinfo.GetValue(obj, null)));
                    }
                }
                catch (Exception sbEx)
                {
                    _log.Warn(sbEx.Message);
                }

                strObjectMemberString = sb.ToString();
                if (strObjectMemberString.StartsWith(","))
                {
                    strObjectMemberString = strObjectMemberString.Substring(1);
                }
            }
            catch (Exception ex)
            {
                _log.Warn(ex.Message);
            }

            return strObjectMemberString;
        }
    }
}