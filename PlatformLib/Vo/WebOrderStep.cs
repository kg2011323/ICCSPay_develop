using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public enum WebOrderStep
    {
        /// <summary>
        /// 网页订单请求
        /// </summary>
        WebTradeNoRequest = 0,
        /// <summary>
        /// 网页订单响应
        /// </summary>
        WebTradeNoRespond = 1,
        /// <summary>
        /// 网络预下单结果请求
        /// </summary>
        WebPrePayRequest = 2,
        /// <summary>
        /// 网络预下单结果响应
        /// </summary>
        WebPrePayRespond = 3,
        /// <summary>
        /// 网络支付同步结果请求
        /// </summary>
        WebPaySyncResultRequest = 4,
        /// <summary>
        /// 网络支付同步结果响应
        /// </summary>
        WebPaySyncResultRespond = 5,
        /// <summary>
        /// 网络支付异步结果请求
        /// </summary>
        WebPayAsyncResultRequest = 6,
        /// <summary>
        /// 网络支付异步结果响应
        /// </summary>
        WebPayAsyncResultRespond = 7,

        /// <summary>
        /// 已有订单验证请求
        /// </summary>
        WebOrderVerifyRequest = 10,
        /// <summary>
        /// 已有订单验证响应
        /// </summary>
        WebOrderVerifyRespond = 11,
        /// <summary>
        /// 订单开始执行请求
        /// </summary>
        WebOrderProcessRequest = 12,
        /// <summary>
        /// 订单开始执行响应
        /// </summary>
        WebOrderProcessRespond = 13,
        /// <summary>
        /// 订单执行结果通知请求
        /// </summary>
        WebOrderTakenRequest = 14,
        /// <summary>
        /// 订单执行结果通知响应
        /// </summary>
        WebOrderTakenRespond = 15,
        /// <summary>
        /// 订单执行故障通知请求
        /// </summary>
        WebOrderTakenErrRequest = 16,
        /// <summary>
        /// 订单执行故障通知响应
        /// </summary>
        WebOrderTakenErrRespond = 17
    }
}
