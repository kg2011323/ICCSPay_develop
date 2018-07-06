using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 订单开始执行请求
    /// </summary>
    public class StationOrderProcessRequestVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
    }
}
