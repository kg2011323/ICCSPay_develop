using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 车票使用目标设备类型
    /// </summary>
    public enum TicketTargetType
    {
        /// <summary>
        /// 空
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 云购票机
        /// </summary>
        TVM = 1,
        /// <summary>
        /// APM云闸机
        /// </summary>
        APMGATE = 2
    }
}
