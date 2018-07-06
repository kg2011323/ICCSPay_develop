using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 已有订单验证响应
    /// </summary>
    public class WebOrderVerifyRespondVo : DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string userMsisdn;
        /// <summary>
        /// 起点站代码
        /// </summary>
        public string pickupStationCode;
        /// <summary>
        /// 终点站代码
        /// </summary>
        public string getOffStationCode;
        /// <summary>
        /// 单程票价
        /// </summary>
        public string singlelTicketPrice;
        /// <summary>
        /// 购买数量
        /// </summary>
        public string singleTicketNum;
        /// <summary>
        /// 0:有起点站和终点站
        /// 1:有起点站点的固定票价
        /// </summary>
        public string singleTicketType;
    }
}
