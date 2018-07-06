using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.Util;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 订单执行故障通知请求
    /// </summary>
    public class OrderTakenErrRequestBaseVo : DeviceCommRequestBaseVo
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
        /// 故障时间,格式YYYYMMDDHH24mmss
        /// </summary>
        public string faultOccurDateString;
        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime? faultOccurDate
        {
            get { return TimeHelper.GetDateTimeYyyyMMddHHmmss(faultOccurDateString); }
        }
        /// <summary>
        /// 故障凭条号
        /// </summary>
        public string faultSlipSeq;
        /// <summary>
        /// 错误代码
        /// </summary>
        public string errorCode;
        /// <summary>
        /// 执行错误信息
        /// </summary>
        public string errorMessage;
    }
}
