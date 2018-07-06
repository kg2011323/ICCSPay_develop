using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class OrderQueryResp:Responce
    {
        //响应对象的字段
        private string _tradeNo;
        private string _singleTicketType;
        private string _buyTime;
        private string _oriAFCStationCode;
        private string _desAFCStationCode;
        private int _ticketPrice;
        private int _ticketNum;
        private decimal _discount;
        private int _amount;
        private string _orderStatus;
        private int _ticketTakeNum;
        private string _ticketTakeTime;
        private string _entryDeviceCode;
        private string _entryTime;
        private string _exitDeviceCode;
        private string _exitTime;

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
        public string tradeNo
        {
            get { return _tradeNo; }
            set { _tradeNo = value; }
        }

        public string singleTicketType
        {
            get { return _singleTicketType; }
            set { _singleTicketType = value; }
        }

        public string buyTime
        {
            get { return _buyTime; }
            set { _buyTime = value; }
        }

        public string oriAFCStationCode
        {
            get { return _oriAFCStationCode; }
            set { _oriAFCStationCode = value; }
        }

        public string desAFCStationCode
        {
            get { return _desAFCStationCode; }
            set { _desAFCStationCode = value; }
        }

        public int ticketPrice
        {
            get { return _ticketPrice; }
            set { _ticketPrice = value; }
        }

        public int ticketNum
        {
            get { return _ticketNum; }
            set { _ticketNum = value; }
        }

        public decimal discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        public int amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string orderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }
        public int ticketTakeNum
        {
            get { return _ticketTakeNum; }
            set { _ticketTakeNum = value; }
        }
        public string ticketTakeTime
        {
            get { return _ticketTakeTime; }
            set { _ticketTakeTime = value; }
        }
        public string entryDeviceCode
        {
            get { return _entryDeviceCode; }
            set { _entryDeviceCode = value; }
        }
        public string entryTime
        {
            get { return _entryTime; }
            set { _entryTime = value; }
        }
        public string exitDeviceCode
        {
            get { return _exitDeviceCode; }
            set { _exitDeviceCode = value; }
        }
        public string exitTime
        {
            get { return _exitTime; }
            set { _exitTime = value; }
        }
    }
}