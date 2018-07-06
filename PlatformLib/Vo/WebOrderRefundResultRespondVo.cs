using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.DB;
using PlatformLib.Util;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络订单退款结果响应
    /// </summary>
    public class WebOrderRefundResultRespondVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string ExternalTradeNo;
        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundTradeNo;
        /// <summary>
        /// 查询订单号有效标识
        /// </summary>
        public bool IsTradeNoValid;
        /// <summary>
        /// 退款原因，单位为分
        /// </summary>
        public string RefundReason;
        /// <summary>
        /// 支付通道编码，未定义
        /// </summary>
        public string PaymentVendor;
        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public int RefundFee;
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public int TotalFee;
        /// <summary>
        /// 微信银行渠道
        /// </summary>
        public string BankType;
        /// <summary>
        /// 退款请求时间
        /// </summary>
        public DateTime RequestTime;
        /// <summary>
        /// 退款请求成功标识
        /// </summary>
        public bool IsRequestSuccess;
        /// <summary>
        /// 退款请求异常描述
        /// </summary>
        public string RequestErrCodeDes;
        /// <summary>
        /// 退款成功标识
        /// </summary>
        public bool IsRespondSuccess;
        /// <summary>
        /// 退款请求响应时间
        /// </summary>
        public DateTime? RespondTime;
        /// <summary>
        /// 退款响应异常描述
        /// </summary>
        public string RespondErrCodeDes;
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusType OrderStatus;

        public WebOrderRefundResultRespondVo()
        { }

        public WebOrderRefundResultRespondVo(WebOrderRefund webOrderRefund)
        {
            if (null != webOrderRefund)
            {
                TradeNo = webOrderRefund.TradeNo;
                ExternalTradeNo = webOrderRefund.ExternalTradeNo;
                RefundTradeNo = webOrderRefund.RefundTradeNo;
                IsTradeNoValid = true;
                RefundReason = webOrderRefund.RefundReason;
                PaymentVendor = webOrderRefund.PaymentVendor;
                RefundFee = 0;
                try
                {
                    RefundFee = Convert.ToInt32(webOrderRefund.RefundFee);
                }
                catch (Exception)
                { }
                TotalFee = 0;
                try
                {
                    TotalFee = Convert.ToInt32(webOrderRefund.TotalFee);
                }
                catch (Exception)
                { }
                BankType = webOrderRefund.BankType;
                RequestTime = webOrderRefund.RequestTime;
                IsRequestSuccess = webOrderRefund.IsRequestSuccess;
                RequestErrCodeDes = webOrderRefund.RequestErrCodeDes;
                IsRespondSuccess = webOrderRefund.IsRespondSuccess;
                RespondTime = webOrderRefund.RespondTime;
                RespondErrCodeDes = webOrderRefund.RespondErrCodeDes;
                OrderStatus = OrderStatusType.None;
                try
                {
                    OrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(webOrderRefund.OrderStatus);
                }
                catch (Exception)
                { }
            }
        }
    }
}

