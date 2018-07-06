using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class SnapQRCodePayResultQueryResp : Responce
    {
        //响应对象的字段
        private string _orderNo;
        private string _paymentDate;
        private string _amount;
        private string _paymentAccount;
        private string _paymentResult;
        private string _paymentDesc;
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
        public string orderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }

        public string paymentDate
        {
            get { return _paymentDate; }
            set { _paymentDate = value; }
        }

        public string amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string paymentAccount
        {
            get { return _paymentAccount; }
            set { _paymentAccount = value; }
        }

        public string paymentResult
        {
            get { return _paymentResult; }
            set { _paymentResult = value; }
        }

        public string paymentDesc
        {
            get { return _paymentDesc; }
            set { _paymentDesc = value; }
        }

        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }
    }
}