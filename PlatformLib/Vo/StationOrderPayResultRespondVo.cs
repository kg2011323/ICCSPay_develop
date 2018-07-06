using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.Util;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 现场车站购票支付结果查询响应
    /// </summary>
    public class StationOrderPayResultRespondVo : DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 支付时间
        /// </summary>
        private DateTime _paymentDate;
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PaymentDate
        {
            get { return _paymentDate; }
            set { _paymentDate = value; }
        }

        /// <summary>
        /// 支付时间
        /// </summary>
        public string paymentDateString
        {
            get 
            {
                string strPaymentDateString = String.Empty;
                try
                {
                    strPaymentDateString = TimeHelper.GetTimeStringYyyyMMddHHmmss(_paymentDate);
                }
                catch (Exception)
                { }
                return strPaymentDateString;
            }
        }
        /// <summary>
        /// 支付金额，以分为单位
        /// </summary>
        public int amount;
        /// <summary>
        /// 支付账号
        /// </summary>
        public string paymentAccount;
        /// <summary>
        /// 支付结果状态
        /// </summary>
        private bool _isPaymentSuccess;
        /// <summary>
        /// 支付结果状态
        /// </summary>
        public bool IsPaymentSuccess
        {
            get { return _isPaymentSuccess; }
            set { _isPaymentSuccess = value; }
        }
        /// <summary>
        /// 支付结果状态
        /// 成功：SUCCESS
        /// 失败：FAILED
        /// </summary>
        public string paymentResult
        {
            get
            {
                string strPaymentResult = _isPaymentSuccess?"SUCCESS":"FAILED";
                return strPaymentResult;
            }
        }
        /// <summary>
        /// 支付消息
        /// </summary>
        public string paymentDesc;
    }
}
