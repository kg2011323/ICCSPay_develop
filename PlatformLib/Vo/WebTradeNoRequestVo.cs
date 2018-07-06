using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网页订单请求
    /// </summary>
    public class WebTradeNoRequestVo
    {
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string ExternalTradeNo;
        /// <summary>
        /// 购票时间
        /// </summary>
        public DateTime BuyTime;
        /// <summary>
        /// 接口编码，未定义
        /// </summary>
        public string OperationCode;
        /// <summary>
        /// 城市代码，未定义
        /// </summary>
        public string CityCode;
        /// <summary>
        /// 设备编码，未定义
        /// </summary>
        public string DeviceId;
        /// <summary>
        /// 发起渠道，未定义
        /// </summary>
        public string ChannelType;
        /// <summary>
        /// 支付通道编码，未定义
        /// </summary>
        public string PaymentVendor;
        /// <summary>
        /// 起始AFC车站代码
        /// </summary>
        public string OriAFCStationCode;
        /// <summary>
        /// 结束AFC车站代码
        /// </summary>
        public string DesAFCStationCode;
        /// <summary>
        /// 车票单价，单位为分
        /// </summary>
        public decimal TicketPrice;
        /// <summary>
        /// 车票数量
        /// </summary>
        public int TicketNum;
        /// <summary>
        /// 折扣，2位小数表示
        /// </summary>
        public decimal Discount;
        /// <summary>
        /// 总价，单位为分
        /// </summary>
        public decimal Amount;
        /// <summary>
        /// 车票使用目标设备类型
        /// </summary>
        public TicketTargetType TicketTarget;
        /// <summary>
        /// 用户openid标识
        /// </summary>
        public string UserOpenId;
        /// <summary>
        /// 用户账户
        /// </summary>
        public string UserAccount;
    }
}
