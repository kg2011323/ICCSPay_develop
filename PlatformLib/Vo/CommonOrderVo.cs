using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.DB;
using PlatformLib.Util;

namespace PlatformLib.Vo
{
    public class CommonOrderVo
    {
        /// <summary>
        /// 购票方式
        /// </summary>
        private OrderType _ticketOrderType;
        /// <summary>
        /// 购票方式
        /// </summary>
        public OrderType TicketOrderType
        {
            get { return _ticketOrderType; }
            set { _ticketOrderType = value; }
        }

        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        private string _tradeNo;
        /// <summary>
        /// 后台订单号（商户订单号）
        /// </summary>
        public string TradeNo
        {
            get { return _tradeNo; }
            set { _tradeNo = value; }
        }

        /// <summary>
        /// 购票时间
        /// </summary>
        private DateTime _buyTime;
        /// <summary>
        /// 购票时间
        /// </summary>
        public DateTime BuyTime
        {
            get { return _buyTime; }
            set { _buyTime = value; }
        }

        /// <summary>
        /// 车票单价，单位为分
        /// </summary>
        private decimal _ticketPrice;
        /// <summary>
        /// 车票单价，单位为分
        /// </summary>
        public decimal TicketPrice
        {
            get { return _ticketPrice; }
            set { _ticketPrice = value; }
        }

        /// <summary>
        /// 车票数量
        /// </summary>
        private int _ticketNum;
        /// <summary>
        /// 车票数量
        /// </summary>
        public int TicketNum
        {
            get { return _ticketNum; }
            set { _ticketNum = value; }
        }

        /// <summary>
        /// 折扣，2位小数表示
        /// </summary>
        private decimal _discount;
        /// <summary>
        /// 折扣，2位小数表示
        /// </summary>
        public decimal Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        /// <summary>
        /// 实际支付金额，单位为分
        /// </summary>
        private decimal _actualFee;
        /// <summary>
        /// 实际支付金额，单位为分
        /// </summary>
        public decimal ActualFee
        {
            get { return _actualFee; }
            set { _actualFee = value; }
        }

        /// <summary>
        /// 支付通道编码
        /// </summary>
        private string _paymentVendor;
        /// <summary>
        /// 支付通道编码
        /// </summary>
        public string PaymentVendor
        {
            get { return _paymentVendor; }
            set { _paymentVendor = value; }
        }

        /// <summary>
        /// 起始AFC车站代码
        /// </summary>
        private string _oriAFCStationCode;
        /// <summary>
        /// 起始AFC车站代码
        /// </summary>
        public string OriAFCStationCode
        {
            get { return _oriAFCStationCode; }
            set { _oriAFCStationCode = value; }
        }

        /// <summary>
        /// 结束AFC车站代码
        /// </summary>
        private string _desAFCStationCode;
        /// <summary>
        /// 结束AFC车站代码
        /// </summary>
        public string DesAFCStationCode
        {
            get { return _desAFCStationCode; }
            set { _desAFCStationCode = value; }
        }

        /// <summary>
        /// 起始车站中文名称
        /// </summary>
        private string _oriStationChineseName;
        /// <summary>
        /// 起始车站中文名称
        /// </summary>
        public string OriStationChineseName
        {
            get { return _oriStationChineseName; }
            set { _oriStationChineseName = value; }
        }

        /// <summary>
        /// 结束车站中文名称
        /// </summary>
        private string _desStationChineseName;
        /// <summary>
        /// 结束车站中文名称
        /// </summary>
        public string DesStationChineseName
        {
            get { return _desStationChineseName; }
            set { _desStationChineseName = value; }
        }

        /// <summary>
        /// 车票使用目标设备类型
        /// </summary>
        private TicketTargetType _ticketTarget;
        /// <summary>
        /// 车票使用目标设备类型
        /// </summary>
        public TicketTargetType TicketTarget
        {
            get { return _ticketTarget; }
            set { _ticketTarget = value; }
        }

        /// <summary>
        /// 已使用标识
        /// </summary>
        private bool _isUsed;
        /// <summary>
        /// 已使用标识
        /// </summary>
        public bool IsUsed
        {
            get { return _isUsed; }
            set { _isUsed = value; }
        }

        /// <summary>
        /// 车站车票购票和取票设备代码，网络预购票为TVM车票取票或APMGATE验票设备代码
        /// </summary>
        private string _deviceId;
        /// <summary>
        /// 车站车票购票和取票设备代码，网络预购票为TVM车票取票或APMGATE验票设备代码
        /// </summary>
        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        /// <summary>
        /// 闸机车票过闸时间，购票机车票取票时间
        /// </summary>
        private DateTime? _useTime;
        /// <summary>
        /// 闸机车票过闸时间，购票机车票取票时间
        /// </summary>
        public DateTime? UseTime
        {
            get { return _useTime; }
            set { _useTime = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CommonOrderVo()
        { }


        public CommonOrderVo(StationOrder stationOrder)
        {
            if (null != stationOrder)
            {
                TicketOrderType = OrderType.StationOrder;
                TradeNo = stationOrder.TradeNo;
                BuyTime = stationOrder.BuyTime;
                TicketPrice = stationOrder.TicketPrice;
                TicketNum = stationOrder.TicketNum;
                Discount = stationOrder.Discount;
                if (null != stationOrder.ActualFee)
                {
                    ActualFee = stationOrder.ActualFee.Value;
                }
                PaymentVendor = stationOrder.PaymentVendor;
                OriAFCStationCode = stationOrder.OriAFCStationCode;
                DesAFCStationCode = stationOrder.DesAFCStationCode;
                OriStationChineseName = StationInfoHelper.Instance.GetAFCChineseStationName(stationOrder.OriAFCStationCode);
                DesStationChineseName = StationInfoHelper.Instance.GetAFCChineseStationName(stationOrder.DesAFCStationCode);
                TicketTarget = TicketTargetType.TVM;
                IsUsed = false;
                if (null != stationOrder.TicketTakeTime)
                {
                    IsUsed = true;
                    UseTime = stationOrder.TicketTakeTime.Value;
                }
                DeviceId = stationOrder.DeviceId;
            }
        }


        public CommonOrderVo(WebOrder webOrder)
        {
            if (null != webOrder)
            {
                TicketOrderType = OrderType.WebOrder;
                TradeNo = webOrder.TradeNo;
                BuyTime = webOrder.BuyTime;
                TicketPrice = 0;
                TicketPrice = webOrder.TicketPrice;
                TicketNum = webOrder.TicketNum;
                Discount = webOrder.Discount;
                if (null != webOrder.ActualFee)
                {
                    ActualFee = webOrder.ActualFee.Value;
                }
                PaymentVendor = webOrder.PaymentVendor;
                OriAFCStationCode = webOrder.OriAFCStationCode;
                DesAFCStationCode = webOrder.DesAFCStationCode;
                OriStationChineseName = StationInfoHelper.Instance.GetAFCChineseStationName(webOrder.OriAFCStationCode);
                DesStationChineseName = StationInfoHelper.Instance.GetAFCChineseStationName(webOrder.DesAFCStationCode);
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
                IsUsed = false;
                if (null != webOrder.TicketTakeTime)
                {
                    IsUsed = true;
                    UseTime = webOrder.TicketTakeTime.Value;
                }
                DeviceId = webOrder.DeviceId;
            }
        }
    }
}
