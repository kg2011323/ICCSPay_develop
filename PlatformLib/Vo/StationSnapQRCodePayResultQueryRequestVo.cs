using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 扫码支付结果查询请求
    /// </summary>
    public class StationSnapQRCodePayResultQueryRequestVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 二维码串
        /// </summary>
        public string qrCode;
    }
}
