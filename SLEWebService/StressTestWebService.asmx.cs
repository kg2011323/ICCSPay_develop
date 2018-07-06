using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using System.Reflection;
using PlatformLib.Vo;
using PlatformLib.Util;
using System.Diagnostics;
using SLEWebService.Util;

namespace SLEWebService
{
    /// <summary>
    /// Summary description for StressTestWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StressTestWebService : System.Web.Services.WebService
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获得请求IP
        /// </summary>
        private string RequestIP
        {
            get
            {
                string strRequestIP = String.Empty;
                try
                {
                    strRequestIP = this.Context.Request.UserHostAddress;
                }
                catch (Exception)
                { }
                return strRequestIP;
            }
        }

        [WebMethod]
        public List<string> HelloWorld(int orderCount, TicketTargetType ticketTarget)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();


            try
            {
                string strQPRReq = String.Format("{0},{1}", orderCount, ticketTarget.ToString());
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strQPRReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            List<string> resultList = new List<string>();

            try
            {
                StressTestHelper sth = new StressTestHelper();

                List<WebPayResultRespondVo> webPayResultRespondVoList = sth.FillPayedWebOrder(orderCount, ticketTarget);

                foreach (WebPayResultRespondVo eachWebPayResultRespondVo in webPayResultRespondVoList)
                {
                    string eachResult = String.Format("{0}|{1}", eachWebPayResultRespondVo.TradeNo, eachWebPayResultRespondVo.Voucher);
                    resultList.Add(eachResult);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            try
            {
                string strQPRResp = resultList.Count.ToString();
                string strLog = String.Format("Resp:{{{0}}}", strQPRResp);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return resultList;
        }
    }
}
