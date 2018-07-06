using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public enum OrderType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 网络预支付订单
        /// </summary>
        WebOrder = 1,
        /// <summary>
        /// 车站现场支付订单
        /// </summary>
        StationOrder = 2
    }
}
