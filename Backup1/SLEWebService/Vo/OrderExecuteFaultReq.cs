using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class OrderExecuteFaultReq:Request
    {
        //请求对象的字段
        private string _orderNo;
        private int _takeSingleTicketNum;
        private string _faultOccurDate;
        private string _faultSlipSeq;
        private string _errorCode;
        private string _errorMessage;

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

        public int takeSingleTicketNum
        {
            get { return _takeSingleTicketNum; }
            set { _takeSingleTicketNum = value; }
        }

        public string faultOccurDate
        {
            get { return _faultOccurDate; }
            set { _faultOccurDate = value; }
        }

        public string faultSlipSeq
        {
            get { return _faultSlipSeq; }
            set { _faultSlipSeq = value; }
        }

        public string erroCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public string errorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

    }
}