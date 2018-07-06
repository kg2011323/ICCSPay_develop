using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusType
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None = 0,
        /// <summary>
        /// 待支付状态
        /// </summary>
        WaitBuyerPay = 1,
        /// <summary>
        /// 支付响应超时
        /// </summary>
        TradeTimeout = 2,
        /// <summary>
        /// 支付成功未取票状态
        /// </summary>
        TradeSuccess = 3,
        /// <summary>
        /// 支付失败
        /// </summary>
        TradeFaild = 4,
        /// <summary>
        /// 取票成功状态
        /// </summary>
        TicketOut = 5,
        /// <summary>
        /// 取票异常状态
        /// </summary>
        TicketException = 6,
        /// <summary>
        /// 退款中
        /// </summary>
        RefundProcessing = 7,
        /// <summary>
        /// 退款成功
        /// </summary>
        RefundSuccess = 8,
        /// <summary>
        /// 退款失败
        /// </summary>
        RefundFail = 9,
        /// <summary>
        /// 订单失效（前台不再处理，超时未支付等情况）
        /// </summary>
        TradeInvalid = 10,
        /// <summary>
        /// 订单关闭（后台不再处理）
        /// </summary>
        TradeClosed = 11
    }
}
