using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class OrderStatusUpdateRequestVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 起点站代码
        /// </summary>
        public string pickupStationCode;
        /// <summary>
        /// 起点站时间
        /// </summary>
        public string pickupStationTime;
        /// <summary>
        /// 终点站代码
        /// </summary>
        public string getOffStationCode;
        /// <summary>
        /// 终点站时间
        /// </summary>
        public string getOffStationTime;
        /// <summary>
        /// 订单状态更新原因
        /// </summary>
        public string updateReason;
        /// <summary>
        /// 单位为分
        /// </summary>
        public string updateFee;
        /// <summary>
        /// 支付通道
        /// </summary>
        public string paymentChannel;
    }
}
