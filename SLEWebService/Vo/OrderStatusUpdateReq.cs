using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class OrderStatusUpdateReq:Request
    {
        //响应对象的字段
        private string _orderNo;
        private string _pickupStationCode;
        private string _pickupStationTime;
        private string _getOffStationCode;
        private string _getOffStationTime;
        private string _updateReason;
        private string _updateFee;
        private string _paymentChannel;

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
        public string updateReason
        {
            get { return _updateReason; }
            set { _updateReason = value; }
        }
        public string updateFee
        {
            get { return _updateFee; }
            set { _updateFee = value; }
        }
        public string paymentChannel
        {
            get { return _paymentChannel; }
            set { _paymentChannel = value; }
        }
    }
}