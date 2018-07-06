using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using PlatformLib.Util;
using log4net;
using System.Timers;
using System.Threading;
using System.Net;
using System.IO;

namespace SLEWebService
{
    public class Global : System.Web.HttpApplication
    {
        private static ILog _log = LogManager.GetLogger("");
        public static ExceptionHelper eh = new ExceptionHelper();

        protected void Application_Start(object sender, EventArgs e)
        {

            // 在应用程序启动时运行的代码
            _log.Info(this.GetType().ToString() + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":Application Start!");

            //定义定时器  60分钟
            System.Timers.Timer myTimer = new System.Timers.Timer(3600000);

            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);

            myTimer.Enabled = true;

            myTimer.AutoReset = true;
        }

        void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {

            try
            {
                eh.ClearDict();
            }

            catch (Exception ee)
            {

                _log.Debug(this.GetType().ToString() + ee.ToString());

            }

        }


        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
            _log.Info(this.GetType().ToString() + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":Application End!");

            //下面的代码是关键，可解决IIS应用程序池自动回收的问题  

            Thread.Sleep(1000);

            //这里设置你的web地址，可以随便指向你的任意一个asmx页面甚至不存在的页面，目的是要激发Application_Start  

            string url = "http://58.63.71.41:9090/SLEServiceStressTest/StressTestWebService.asmx";

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流  

        }
    }
}