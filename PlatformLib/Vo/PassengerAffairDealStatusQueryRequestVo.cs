using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class PassengerAffairDealStatusQueryRequestVo:DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 车票Id
        /// </summary>
        public string ticketId;
    }
}
