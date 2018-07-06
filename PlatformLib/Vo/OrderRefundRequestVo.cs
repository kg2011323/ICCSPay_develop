using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class OrderRefundRequestVo:DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
    }
}
