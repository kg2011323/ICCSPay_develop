using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网页订单响应
    /// </summary>
    public class WebTradeNoRespondVo
    {
        /// <summary>
        /// 操作成功标识
        /// </summary>
        public bool IsSuccess = false;

        /// <summary>
        /// 预购票订单号，只作为唯一标识不参与其他业务逻辑
        /// </summary>
        public Guid WebOrderId;

        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;

        /// <summary>
        /// 对应订单请求有效性
        /// </summary>
        public bool IsVaild;
      
        /// <summary>
        /// 处理步骤其他信息
        /// </summary>
        public string StepStatus;
    }
}
