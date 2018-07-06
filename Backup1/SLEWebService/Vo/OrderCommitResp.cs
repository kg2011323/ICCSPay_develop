using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class OrderCommitResp:Responce
    {
        //响应对象的字段
        private string _partnerNo;
        private string _orderNo;
        private string _subject;
        private string _body;
        private string _payType;
        private string _amount;
        private string _account;
        private string _notifyUrl;
        private string _merchantCert;
        private int _timeout;

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
        public string partnerNo
        {
            get { return _partnerNo; }
            set { _partnerNo = value; }
        }

        public string orderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }

        public string subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string body
        {
            get { return _body; }
            set { _body = value; }
        }

        public string payType
        {
            get { return _payType; }
            set { _payType = value; }
        }

        public string amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string account
        {
            get { return _account; }
            set { _account = value; }
        }

        public string notifyUrl
        {
            get { return _notifyUrl; }
            set { _notifyUrl = value; }
        }

        public string merchantCert
        {
            get { return _merchantCert; }
            set { _merchantCert = value; }
        }

        public int timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
    }
}