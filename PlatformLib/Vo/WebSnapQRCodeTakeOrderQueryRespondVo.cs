using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 扫码支付取票订单响应
    /// </summary>
    public class WebSnapQRCodeTakeOrderQueryRespondVo : DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 支付账号
        /// </summary>
        public string paymentAccount;
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
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
        public string singleTicketPrice;
        /// <summary>
        /// 购票数量
        /// </summary>
        public string singleTicketNum;
        /// <summary>
        /// 单程票类型
        /// </summary>
        public string singleTicketType;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string deviceId;
        /// <summary>
        /// 用户信息
        /// </summary>
        public string userName;
    }
}
