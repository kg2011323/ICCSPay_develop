using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 已有订单验证请求
    /// </summary>
    public class WebOrderVerifyRequestVo: DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 二维码全部信息
        /// </summary>
        public string orderToken;
    }
}
