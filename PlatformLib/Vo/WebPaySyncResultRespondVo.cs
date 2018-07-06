using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 网络支付结果响应
    /// </summary>
    public class WebPaySyncResultRespondVo
    {
        /// <summary>
        /// 预购票订单号，只作为唯一标识不参与其他业务逻辑
        /// </summary>
        public Guid? WebOrderId;
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;
        /// <summary>
        /// 起始车站AFC代码
        /// </summary>
        public string OriAFCStationCode;
        /// <summary>
        /// 结束车站AFC代码
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
        /// 取票凭证
        /// </summary>
        public string Voucher;
        /// <summary>
        /// 加密取票凭证，预留
        /// </summary>
        //public string VoucherEncrypted;
        /// <summary>
        /// 处理步骤其他信息
        /// </summary>
        public string StepStatus;
        /// <summary>
        /// 操作成功标识
        /// </summary>
        public bool IsSuccess;
    }
}
