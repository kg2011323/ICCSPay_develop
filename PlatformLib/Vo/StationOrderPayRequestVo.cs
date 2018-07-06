using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 现场车站购票订单提交请求
    /// </summary>
    public class StationOrderPayRequestVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 设备上提供用户扫描的支付账户认证码（可变长度，最长64位）
        /// </summary>
        public string paymentCode;
        /// <summary>
        /// 客户端下单(此字段不用填)
        /// </summary>
        public string msisdn;
        /// <summary>
        /// Sim卡标识(此字段不用填)
        /// </summary>
        public string iccid;
        /// <summary>
        /// 客户端下单(此字段不用填)
        /// </summary>
        public string serviceId;
        /// <summary>
        /// 支付通道编码
        /// </summary>
        public string paymentVendor;
        /// <summary>
        /// 起始AFC车站代码
        /// </summary>
        public string pickupStationCode;
        /// <summary>
        /// 结束AFC车站代码
        /// </summary>
        public string getOffStationCode;
        /// <summary>
        /// 单程票价，单位为分
        /// </summary>
        public int singlelTicketPrice;
        /// <summary>
        /// 购买数量
        /// </summary>
        public int singlelTicketNum;       
        /// <summary>
        /// 0:有起点站和终点站
        /// 1:有起点站的固定票价
        /// </summary>
        public string singleTicketType;
    }
}
