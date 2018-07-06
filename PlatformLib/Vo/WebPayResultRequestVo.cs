using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络支付异步结果请求，TradeNo必须为非空
    /// </summary>
    public class WebPayResultRequestVo 
    {
        ///// <summary>
        ///// 预购票订单号，只作为唯一标识不参与其他业务逻辑
        ///// </summary>
        //public Guid? WebOrderId;
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string ExternalTradeNo;
        /// <summary>
        /// 支付商支付订单号
        /// </summary>
        public string TransactionId;
        /// <summary>
        /// 支付完成时间，结构为yyyyMMddHHmmss
        /// </summary>
        public string PayEndTime;
        /// <summary>
        /// 付款银行标识
        /// </summary>
        public string BankType;
        /// <summary>
        /// 实际支付金额，单位为分
        /// </summary>
        public decimal ActualFee;
       /// <summary>
        /// 支付错误代码描述
       /// </summary>
        public string ErrCodeDes;
        /// <summary>
        /// 用户openid标识
        /// </summary>
        public string UserOpenId;
        /// <summary>
        /// 用户账户
        /// </summary>
        public string UserAccount;
        /// <summary>
        /// 操作成功标识
        /// </summary>
        public bool IsSuccess;
    }
}
