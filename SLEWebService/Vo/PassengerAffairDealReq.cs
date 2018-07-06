using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class PassengerAffairDealReq:Request
    {
        //请求对象的字段
        private string _paymentCode;
        private string _ticketType;
        private string _ticketId;
        private string _paymentVendor;
        private string _passengerAffairType;
        private string _pickupStationCode;
        private string _pickupStationTime;
        private string _getOffStationCode;
        private string _getOffStationTime;
        private string _passengerAffairPrice;


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
        public string ticketType
        {
            get { return _ticketType; }
            set { _ticketType = value; }
        }
        public string ticketId
        {
            get { return _ticketId; }
            set { _ticketId = value; }
        }
        public string paymentVendor
        {
            get { return _paymentVendor; }
            set { _paymentVendor = value; }
        }
        public string passengerAffairType
        {
            get { return _passengerAffairType; }
            set { _passengerAffairType = value; }
        }
        public string pickupStationCode
        {
            get { return _pickupStationCode; }
            set { _pickupStationCode = value; }
        }
        public string pickupStationTime
        {
            get { return _pickupStationTime; }
            set { _pickupStationTime = value; }
        }
        public string getOffStationCode
        {
            get { return _getOffStationCode; }
            set { _getOffStationCode = value; }
        }
        public string getOffStationTime
        {
            get { return _getOffStationTime; }
            set { _getOffStationTime = value; }
        }
        public string passengerAffairPrice
        {
            get { return _passengerAffairPrice; }
            set { _passengerAffairPrice = value; }
        }
    }
}