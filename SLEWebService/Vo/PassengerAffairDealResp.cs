using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class PassengerAffairDealResp:Responce
    {
        //响应对象的字段
        private string _tradeNo;
        private string _ticketId;
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

        public string ticketId
        {
            get { return _ticketId; }
            set { _ticketId = value; }
        }
    }
}