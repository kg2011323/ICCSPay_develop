using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public enum StationOrderStep
    {
        /// <summary>
        /// 提交订单请求
        /// </summary>
        StationOrderPayRequest = 0,
        /// <summary>
        /// 提交订单响应
        /// </summary>
        StationOrderPayRespond = 1,
        

        /// <summary>
        /// 支付结果查询请求
        /// </summary>
        StationOrderPayResultRequest = 10,
        /// <summary>
        /// 支付结果查询响应
        /// </summary>
        StationOrderPayResultRespond = 11,
        /// <summary>
        /// 订单开始执行请求
        /// </summary>
        StationOrderProcessRequest = 12,
        /// <summary>
        /// 订单开始执行响应
        /// </summary>
        StationOrderProcessRespond = 13,
        /// <summary>
        /// 订单执行结果通知请求
        /// </summary>
        StationOrderTakenRequest = 14,
        /// <summary>
        /// 订单执行结果通知响应
        /// </summary>
        StationOrderTakenRespond = 15,
        /// <summary>
        /// 订单执行故障通知请求
        /// </summary>
        StationOrderTakenErrRequest = 16,
        /// <summary>
        /// 订单执行故障通知响应
        /// </summary>
        StationOrderTakenErrRespond = 17
    }
}
