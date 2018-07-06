using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class DeviceVerificationResp:Responce
    {
        //响应对象的字段
        private string _ftpIpAddr;
        private string _ftpUserName;
        private string _ftpUserPwd;
        private string _ntpIpAddr;
        private string _retryCnt;


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
        public string ftpIpAddr
        {
            get { return _ftpIpAddr; }
            set { _ftpIpAddr = value; }
        }

        public string ftpUserName
        {
            get { return _ftpUserName; }
            set { _ftpUserName = value; }
        }

        public string ftpUserPwd
        {
            get { return _ftpUserPwd; }
            set { _ftpUserPwd = value; }
        }

        public string ntpIpAddr
        {
            get { return _ntpIpAddr; }
            set { _ntpIpAddr = value; }
        }

        public string retryCnt
        {
            get { return _retryCnt; }
            set { _retryCnt = value; }
        }
    }
}