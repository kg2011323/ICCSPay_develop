using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class PassengerAffairDealRequestVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 支付码
        /// </summary>
        public string paymentCode;
        /// <summary>
        /// 车票类型
        /// </summary>
        public string ticketType;
        /// <summary>
        /// 车票Id
        /// </summary>
        public string ticketId;
        /// <summary>
        /// 支付通道编码
        /// </summary>
        public string paymentVendor;
        /// <summary>
        /// 事务处理类型
        /// </summary>
        public string passengerAffairType;
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
        /// 事务处理金额
        /// </summary>
        public string passengerAffairPrice;
    }
}
