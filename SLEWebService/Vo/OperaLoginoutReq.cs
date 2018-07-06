using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class OperaLoginoutReq:Request
    {
        //请求对象的字段
        private string _UserID;
        private string _Password;
        private string _LoginType;

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
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public string LoginType
        {
            get { return _LoginType; }
            set { _LoginType = value; }
        }
    }
}