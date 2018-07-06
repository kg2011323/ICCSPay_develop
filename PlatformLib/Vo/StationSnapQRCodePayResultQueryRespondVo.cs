using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 扫码支付结果查询响应
    /// </summary>
    public class StationSnapQRCodePayResultQueryRespondVo : DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 支付时间
        /// </summary>
        public string paymentDate;
        /// <summary>
        /// 支付金额
        /// </summary>
        public string amount;
        /// <summary>
        /// 支付账号
        /// </summary>
        public string paymentAccount;
        /// <summary>
        /// 支付结果
        /// </summary>
        public string paymentResult;
        /// <summary>
        /// 支付描述
        /// </summary>
        public string paymentDesc;
        /// <summary>
        /// 用户信息
        /// </summary>
        public string userName;
    }
}
