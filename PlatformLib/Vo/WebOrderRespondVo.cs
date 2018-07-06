using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.DB;
using PlatformLib.Util;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 已有订单查询响应
    /// </summary>
    public class WebOrderRespondVo
    {
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo;        
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string ExternalTradeNo;
        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime BuyTime;
        /// <summary>
        /// 订单号效标识
        /// </summary>
        public bool IsWebOrderVaild;
        /// <summary>
        /// 支付通道编码
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
        /// 起始车站中文名称
        /// </summary>
        public string OriStationChineseName;
        /// <summary>
        /// 结束车站中文名称
        /// </summary>
        public string DesStationChineseName;
        /// <summary>
        /// 起始车站英文名称
        /// </summary>
        public string OriStationEnglishName;
        /// <summary>
        /// 结束车站英文名称
        /// </summary>
        public string DesStationEnglishName;
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
        /// 实际支付金额，单位为分
        /// </summary>
        public decimal ActualFee;
        /// <summary>
        /// 车票使用目标设备类型
        /// </summary>
        public TicketTargetType TicketTarget;
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string TransactionId;
        /// <summary>
        /// 支付时间，没支付为null
        /// </summary>
        public DateTime? PayEndTime;
        /// <summary>
        /// 过期时间，为隔天凌晨，没支付以buytime计算，已支付按PayEndTime计算
        /// </summary>
        public DateTime ExpiryTime
        {
            get 
            {
                DateTime dtExpiryTime = new DateTime();

                DateTime calTime = BuyTime;
                if (null != PayEndTime)
                {
                    calTime = PayEndTime.Value;
                }
                dtExpiryTime = new DateTime(calTime.Year, calTime.Month, calTime.Day);
                dtExpiryTime = dtExpiryTime.AddDays(2);

                return dtExpiryTime;
            }
        }
        /// <summary>
        /// 取票凭证
        /// </summary>
        public string Voucher;
        /// <summary>
        /// 已使用标识
        /// </summary>
        public bool IsUsed;
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UsedTime;
        /// <summary>
        /// 车票数量
        /// </summary>
        public int TicketTakeNum;
        /// <summary>
        /// 车票数量
        /// </summary>
        public DateTime? TicketTakeTime;
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusType OrderStatus;
        /// <summary>
        /// 订单步骤
        /// </summary>
        public WebOrderStep OrderStep;

        public WebOrderRespondVo()
        { }

        public WebOrderRespondVo(WebOrder webOrder)
        {
            if (null != webOrder)
            {
                TradeNo = webOrder.TradeNo;
                ExternalTradeNo = webOrder.ExternalTradeNo;
                BuyTime = webOrder.BuyTime;
                IsWebOrderVaild = true;
                PaymentVendor = webOrder.PaymentVendor;
                OriAFCStationCode = webOrder.OriAFCStationCode;
                DesAFCStationCode = webOrder.DesAFCStationCode;
                OriStationChineseName = String.Empty;
                DesStationChineseName = String.Empty;
                OriStationEnglishName = String.Empty;
                DesStationEnglishName = String.Empty;
                TicketPrice = webOrder.TicketPrice;
                TicketNum = webOrder.TicketNum;
                Discount = webOrder.Discount;
                ActualFee = 0;
                if (null != webOrder.ActualFee)
                {
                    ActualFee = webOrder.ActualFee.Value;
                }
                TicketTarget = TicketTargetType.NONE;
                if (!String.IsNullOrEmpty(webOrder.TicketTarget))
                {
                    try
                    {
                        TicketTarget = EnumHelper.GetTicketTargetType(webOrder.TicketTarget);
                    }
                    catch (Exception)
                    { }
                }
                TransactionId = webOrder.TransactionId;
                PayEndTime = webOrder.PayEndTime;
                //// 临时处理
                //ExpiryTime = new DateTime();
                Voucher = String.Empty;
                TicketTakeNum = 0;
                if (null != webOrder.TicketTakeNum)
                {
                    TicketTakeNum = webOrder.TicketTakeNum.Value;
                    IsUsed = true;
                }
                UsedTime = webOrder.TicketTakeTime;
                OrderStatus = OrderStatusType.None;
                try
                {
                    OrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(webOrder.OrderStatus);
                }
                catch (Exception)
                { }
                OrderStep = EnumHelper.GetWebOrderStep(webOrder.Step);
            }
        }
    }
}
