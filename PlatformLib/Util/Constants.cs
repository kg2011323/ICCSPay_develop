using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Util
{
    public class Constants
    {
        /// <summary>
        /// 商户订单号长度
        /// </summary>
        public static readonly int TradeNoLength = 28;
        
        /// <summary>
        /// 取票凭证长度
        /// </summary>
        public static readonly int VoucherLength = 20;

        /// <summary>
        /// 车站订单支付直接返回成功，用于测试
        /// </summary>
        public const bool IsStationOrderPayAlwaysSuccess = false;

        /// <summary>
        /// 车站订单支付价格调整为百分之一，用于测试
        /// </summary>
        public const bool IsStationOrderPriceOnePercentage = false;  

        /// <summary>
        /// 网络预购订单前缀
        /// </summary>
        public const string OrderNoPrefixWeb = "W";
        /// <summary>
        /// 车站现场订单前缀
        /// </summary>
        public const string OrderNoPrefixStation = "S";
        /// <summary>
        /// 扫码支付网络预购订单前缀
        /// </summary>
        public const string OrderNoQRCWeb = "QW";
        /// <summary>
        /// 扫码支付车站现场订单前缀
        /// </summary>
        public const string OrderNoQRCStation = "QS";
    }
}
