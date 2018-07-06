using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;


namespace PlatformLib.Util
{
    public class TradeNoHelper
    {
        #region 单例模式
        private static volatile TradeNoHelper instance;
        private static object syncRoot = new Object();
        public static TradeNoHelper Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                        {
                            instance = new TradeNoHelper();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion 单例模式

        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Object _lock = new Object();

        /// <summary>
        /// 当天记录数量
        /// </summary>
        private long _dailyRecordCount = 0;

        private string _strTodayYyyyMMddHHmmss = null;

        /// <summary>
        /// 订单号日期长度，YyyyMMddHHmmss格式长度
        /// </summary>
        private const int _tradeNoDateTimeStringLength = 14;  //正式环境
  

        /// <summary>
        /// 订单号除日期外字符串格式，格式为{0:DX}，X为长度
        /// </summary>
        private string _tradeNoOtherStringFormat = String.Empty;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private TradeNoHelper()
        {
            try
            {
                int _tradeNoOtherStringLength = Constants.TradeNoLength - _tradeNoDateTimeStringLength;  //正式环境
                _tradeNoOtherStringFormat = String.Format("{{0:D{0}}}", _tradeNoOtherStringLength);  //100服务器订单生成

                _dailyRecordCount = 1;
                _strTodayYyyyMMddHHmmss = GetNowYyyyMMddHHmmss();

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }
        }

        private string GetNowYyyyMMddHHmmss()
        {
            string strTodayYyyyMMddHHmmss = DateTime.Now.ToString("yyyyMMddHHmmss");
            return strTodayYyyyMMddHHmmss;
        }

        /// <summary>
        /// 获得内部订单号（商户号）
        /// </summary>
        /// <returns></returns>
        public string GetTradeNo()
        {
            string strTradeNo = String.Empty;

            try
            {
                lock (_lock)
                {
                    string currentTodayYyyyMMddHHmmss = GetNowYyyyMMddHHmmss();
                    if (!currentTodayYyyyMMddHHmmss.Equals(_strTodayYyyyMMddHHmmss))
                    {
                        _dailyRecordCount = 1;
                        _strTodayYyyyMMddHHmmss = currentTodayYyyyMMddHHmmss;
                    }

                    strTradeNo = String.Format("{0}{1}", _strTodayYyyyMMddHHmmss, String.Format(_tradeNoOtherStringFormat, _dailyRecordCount));
                    ++_dailyRecordCount;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            return strTradeNo;
        }
    }
}
