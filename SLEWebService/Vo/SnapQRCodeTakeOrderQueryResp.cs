using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class SnapQRCodeTakeOrderQueryResp:Responce
    {
        //响应对象的字段
        private string _paymentAccount;
        private string _orderNo;
        private string _pickupStationCode;
        private string _getOffStationCode;
        private string _singleTicketPrice;
        private string _singleTicketNum;
        private string _singleTicketType;
        private string _deviceId;
        private string _userName;

        //操作基类属性
        public string respCode
        {
            get { return _respCode; }
            set { _respCode = value; }
        }

        public string respCodeMemo
        {
            get { return _respCodeMemo; }
            set { _respCodeMemo = value; }
        }

        public List<string> expandAttribute
        {
            get { return _expandAttribute; }
            set { _expandAttribute = value; }
        }

        //操作自身属性
        public string paymentAccount
        {
            get { return _paymentAccount; }
            set { _paymentAccount = value; }
        }

        public string orderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }

        public string pickupStationCode
        {
            get { return _pickupStationCode; }
            set { _pickupStationCode = value; }
        }

        public string getOffStationCode
        {
            get { return _getOffStationCode; }
            set { _getOffStationCode = value; }
        }

        public string singleTicketPrice
        {
            get { return _singleTicketPrice; }
            set { _singleTicketPrice = value; }
        }

        public string singleTicketNum
        {
            get { return _singleTicketNum; }
            set { _singleTicketNum = value; }
        }

        public string singleTicketType
        {
            get { return _singleTicketType; }
            set { _singleTicketType = value; }
        }

        public string deviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }
    }
}