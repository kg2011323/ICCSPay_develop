using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络订单退款记录更行响应
    /// </summary>
    public class WebOrderRefundUpdateRespondVo
    {
        /// <summary>
        /// 查询订单号有效标识
        /// </summary>
        public bool IsTradeNoValid;
        /// <summary>
        /// 传入订单号有效标识，必须为退款中、退款成功、退款失败其中一种
        /// </summary>
        public bool IsOrderStatusValid;
        /// <summary>
        /// 操作成功标识
        /// </summary>
        public bool IsSuccess = false;
        /// <summary>
        /// 订单状态，失败时表示原有订单状态，成功返回传入订单状态
        /// </summary>
        public OrderStatusType OrderStatus;
    }
}
