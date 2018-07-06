using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络支付同步结果请求，WebOrderId和TradeNo其中一个必须为非空
    /// </summary>
    public class WebPaySyncResultRequestVo 
    {
        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string PrepayId;
        /// <summary>
        /// 时间戳，自1970年以来的秒数 
        /// </summary>
        public string TimeStamp;
    }
}
