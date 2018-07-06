using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class PassengerAffairDealRespondVo:DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string tradeNo;
        /// <summary>
        /// 车票Id
        /// </summary>
        public string ticketId;
    }
}
