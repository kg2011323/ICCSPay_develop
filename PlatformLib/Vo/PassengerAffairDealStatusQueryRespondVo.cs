using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class PassengerAffairDealStatusQueryRespondVo:DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 车票类型
        /// </summary>
        public string ticketType;
        /// <summary>
        /// 车票Id
        /// </summary>
        public string ticketId;
        /// <summary>
        /// 支付通道
        /// </summary>
        public string paymentVendor;
        /// <summary>
        /// 更新原因
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
        /// 更新金额，单位为分
        /// </summary>
        public string passengerAffairPrice;
        /// <summary>
        /// 超时超乘事务处理状态
        /// </summary>
        public string updateOrderStatus;
        
    }
}
