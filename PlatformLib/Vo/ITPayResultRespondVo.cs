using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 信息服务网络支付结果响应
    /// </summary>
    public class ITPayResultRespondVo
    {        
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;       
        /// <summary>
        /// 取票凭证
        /// </summary>
        public string Voucher;
        ///// <summary>
        ///// 异常字段列表
        ///// </summary>
        //public string ErrFlags;
        /// <summary>
        /// 异常描述
        /// </summary>
        public string ErrStatus
;
    }
}
