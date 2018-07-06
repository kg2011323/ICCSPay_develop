using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class PassengerAffairDealStatusQueryResp:Responce
    { //响应对象的字段
        private string _ticketType;
        private string _ticketId;
        private string _paymentVendor;
        private string _passengerAffairType;
        private string _pickupStationCode;
        private string _pickupStationTime;
        private string _getOffStationCode;
        private string _getOffStationTime;
        private string _passengerAffairPrice;
        private string _updateOrderStatus;

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

        public string updateOrderStatus
        {
            get { return _updateOrderStatus; }
            set { _updateOrderStatus = value; }
        }
    }
}