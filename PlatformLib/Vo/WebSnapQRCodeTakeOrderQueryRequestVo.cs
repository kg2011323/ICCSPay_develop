using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 扫码支付取票订单请求
    /// </summary>
    public class WebSnapQRCodeTakeOrderQueryRequestVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 设备号
        /// </summary>
        public string deviceId;
        /// <summary>
        /// 随机因子
        /// </summary>
        public string randomFact;
    }
}
