using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 现场车站购票订单提交响应
    /// </summary>
    public class StationOrderPayRespondVo : DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 提供给接入方的唯一标识
        /// </summary>
        public string partnerNo;
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 商品名称(可变长最大255位)
        /// </summary>
        public string subject;
        /// <summary>
        /// 商品描述(可变长最大255位)
        /// </summary>
        public string body;
        /// <summary>
        /// 支付类型
        /// </summary>
        public string payType;
        /// <summary>
        /// 金额，单位为分
        /// </summary>
        public string amount;
        /// <summary>
        /// 用户账号(可变长最大32位)
        /// </summary>
        public string account;
        /// <summary>
        /// 服务器异步通知页面路径(可变长最大255位)
        /// </summary>
        public string notifyUrl;
        /// <summary>
        /// 支付通道密钥（可变最大128）
        /// </summary>
        public string merchantCert;
        /// <summary>
        /// 未支付的超时时间
        /// 默认2分钟，取值范围：1～60。
        /// 一旦超时，该笔交易就会自动被关闭。
        /// </summary>
        public int timeout;
    }
}
