using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    //响应对象基类
    public class Responce
    {
        protected string _respCode;
        protected string _respCodeMemo;
        protected List<string> _expandAttribute;
    }
}