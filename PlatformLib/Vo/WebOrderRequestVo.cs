using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络订单验证请求
    /// </summary>
    public class WebOrderRequestVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string ExternalTradeNo;
    }
}
