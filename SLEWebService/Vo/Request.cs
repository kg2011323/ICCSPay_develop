using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    //请求对象基类
    public class Request  
    {
        protected string _reqSysDate;
        protected string _operationCode;
        protected string _cityCode;
        protected string _deviceId;
        protected string _channelType;
        protected List<string> _expandAttribute;
    }
}