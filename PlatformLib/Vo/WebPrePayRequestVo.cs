using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络预下单结果请求
    /// </summary>
    public class WebPrePayRequestVo 
    {
        /// <summary>
        /// 预购票订单号，只作为唯一标识不参与其他业务逻辑
        /// </summary>
        public Guid? WebOrderId;
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;
        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string PrepayId;
    }
}
