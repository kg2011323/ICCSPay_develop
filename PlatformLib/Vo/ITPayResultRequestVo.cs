using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 信息服务部网络支付结果请求
    /// </summary>
    public class ITPayResultRequestVo 
    {
        /// <summary>
        /// 后台订单号（商户订单号），必须为非空
        /// </summary>
        public string TradeNo;
        /// <summary>
        /// 支付商支付订单号
        /// </summary>
        public string TransactionId;
        /// <summary>
        /// 用户openid标识
        /// </summary>
        public string UserOpenId;
        /// <summary>
        /// 起始AFC车站代码
        /// </summary>
        public string OriAFCStationCode;
        /// <summary>
        /// 结束AFC车站代码
        /// </summary>
        public string DesAFCStationCode;
        /// <summary>
        /// 车票单价，单位为分
        /// </summary>
        public string TicketPrice;
        /// <summary>
        /// 车票数量
        /// </summary>
        public string TicketNum;       
        /// <summary>
        /// 实际支付金额，单位为分
        /// </summary>
        public string ActualFee;
        /// <summary>
        /// 支付完成时间，结构为yyyyMMddHHmmss
        /// </summary>
        public string PayEndTime;
        /// <summary>
        /// 支付运营商
        /// </summary>
        public string PayOperator;
        /// <summary>
        /// 付款银行标识
        /// </summary>
        public string BankType;
        /// <summary>
        /// 使用设备
        /// </summary>
        public string Target;
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string ErrCodeDes;
    }
}
