using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 扫码预支付链接查询响应
    /// </summary>
    public class StationSnapQRCodePrePayURLQueryRespondVo : DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string deviceId;
        /// <summary>
        /// 二维码串
        /// </summary>
        public string qrCode;
    }
}
