using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 支付渠道代码
    /// </summary>
    public enum PaymentVendorType
    {
        /// <summary>
        /// 银联支付
        /// </summary>
        FREE = 1000,
        /// <summary>
        /// 银联支付
        /// </summary>
        UnionPay = 1,
        /// <summary>
        /// 支付宝
        /// </summary>
        AliPay = 1001,
        /// <summary>
        /// 微信支付
        /// </summary>
        WeixinPay = 1002
    }
}
