using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class OrderCommitReq : Request
    {
        //请求对象的字段
        private string _paymentCode;
        private string _msisdn;
        private string _iccid;
        private string _serviceId;
        private string _paymentVendor;
        private string _pickupStationCode;
        private string _getOffStationCode;
        private string _singleTicketPrice;
        private string _singleTicketNum;
        private string _singleTicketType;

        //操作基类属性
        public string reqSysDate
        {
            get { return _reqSysDate; }
            set { _reqSysDate = value; }
        }

        public string operationCode
        {
            get { return _operationCode; }
            set { _operationCode = value; }
        }

        public string cityCode
        {
            get { return _cityCode; }
            set { _cityCode = value; }
        }

        public string deviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        public string channelType
        {
            get { return _channelType; }
            set { _channelType = value; }
        }

        public List<string> expandAttribute
        {
            get { return _expandAttribute; }
            set { _expandAttribute = value; }
        }

        //操作自身属性
        public string paymentCode
        {
            get { return _paymentCode; }
            set { _paymentCode = value; }
        }

        public string msisdn
        {
            get { return _msisdn; }
            set { _msisdn = value; }
        }

        public string iccid
        {
            get { return _iccid; }
            set { _iccid = value; }
        }

        public string serviceId
        {
            get { return _serviceId; }
            set { _serviceId = value; }
        }

        public string paymentVendor
        {
            get { return _paymentVendor; }
            set { _paymentVendor = value; }
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
    }
}