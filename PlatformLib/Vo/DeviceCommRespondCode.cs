using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 响应码表
    /// </summary>
    public enum DeviceCommRespondCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        RC0000 = 0,
        /// <summary>
        /// 参数错误
        /// </summary>
        RC0001 = 1,
        /// <summary>
        /// 未收到支付结果通知
        /// </summary>
        RC0002 = 2,
        /// <summary>
        /// 支付失败
        /// </summary>
        RC0003 = 3,
        /// <summary>
        /// 订单号码错误
        /// </summary>
        RC0004 = 4,
        /// <summary>
        /// Token错误
        /// </summary>
        RC0005 = 5,
        /// <summary>
        /// 非法设备编码
        /// </summary>
        RC0006 = 6,
        /// <summary>
        /// 订单未支付
        /// </summary>
        RC0007 = 7,
        /// <summary>
        /// 订单已取票
        /// </summary>
        RC0008 = 8,
        /// <summary>
        /// 订单锁定
        /// </summary>
        RC0009 = 9,
        /// <summary>
        /// 订单已退款
        /// </summary>
        RC0010 = 10,
        /// <summary>
        /// 文件接收成功
        /// </summary>
        RC0011 = 11,
        /// <summary>
        /// 文件长度错
        /// </summary>
        RC0012 = 12,
        /// <summary>
        /// 文件校验错
        /// </summary>
        RC0013 = 13,
        /// <summary>
        /// 文件未找到
        /// </summary>
        RC0014 = 14,
        /// <summary>
        /// 系统错误
        /// </summary>
        RC9999 = 9999
    }
}
