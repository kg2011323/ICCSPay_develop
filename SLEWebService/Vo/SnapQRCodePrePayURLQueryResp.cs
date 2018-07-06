using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class SnapQRCodePrePayURLQueryResp : Responce
    {
        //响应对象的字段
        private string _orderNo;
        private string _deviceId;
        private string _qrCode;

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

        public string deviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        public string qrCode
        {
            get { return _qrCode; }
            set { _qrCode = value; }
        }
    }
}