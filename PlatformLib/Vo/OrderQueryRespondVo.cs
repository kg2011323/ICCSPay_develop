using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class OrderQueryRespondVo : DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string tradeNo;
        /// <summary>
        /// 车票类型
        /// </summary>
        public string singleTicketType;
        /// <summary>
        /// 购票时间
        /// </summary>
        public string buyTime;
        /// <summary>
        /// 起点站代码
        /// </summary>
        public string oriAFCStationCode;
        /// <summary>
        /// 终点站代码
        /// </summary>
        public string desAFCStationCode;
        /// <summary>
        /// 票价（单位分）
        /// </summary>
        public int ticketPrice;
        /// <summary>
        /// 购票数量
        /// </summary>
        public int ticketNum;
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal discount;
        /// <summary>
        /// 总票价（单位分）
        /// </summary>
        public int amount;
        /// <summary>
        /// 订单状态
        /// </summary>
        public string orderStatus;
        /// <summary>
        /// 取票数量
        /// </summary>
        public int ticketTakeNum;
        /// <summary>
        /// 取票时间
        /// </summary>
        public string ticketTakeTime;
        /// <summary>
        /// 进站设备号
        /// </summary>
        public string entryDeviceCode;
        /// <summary>
        /// 进站时间
        /// </summary>
        public string entryTime;
        /// <summary>
        /// 出站设备号
        /// </summary>
        public string exitDeviceCode;
        /// <summary>
        /// 出站时间
        /// </summary>
        public string exitTime;

    }
}
