using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.Util;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 订单执行结果通知请求
    /// </summary>
    public class OrderTakenRequestBaseVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 实际出票数量
        /// </summary>
        public string takeSingleTicketNum;
        /// <summary>
        /// 取票时间,格式YYYYMMDDHH24mmss
        /// </summary>
        public string takeSingleTicketDateString;
        /// <summary>
        /// 取票时间
        /// </summary>
        public DateTime? takeSingleTicketDate
        {
            get { return TimeHelper.GetDateTimeYyyyMMddHHmmss(takeSingleTicketDateString); }
        }
    }
}
