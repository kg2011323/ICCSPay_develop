using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络预下单结果响应
    /// </summary>
    public class WebPrePayRespondVo 
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
        /// 外部订单号
        /// </summary>
        public string ExternalTradeNo;
        /// <summary>
        /// 查询订单号有效标识
        /// </summary>
        public bool IsTradeNoValid;
        /// <summary>
        /// 操作成功标识
        /// </summary>
        public bool IsSuccess;
        /// <summary>
        /// 处理步骤其他信息
        /// </summary>
        public string StepStatus;
    }
}
