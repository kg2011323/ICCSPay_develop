using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SLEWebService.Util
{
    public class Constants
    {
        /// <summary>
        /// 设备ID检查开关，1为启用，0为关闭，无效设备响应“非法设备编码”
        /// </summary>
        public static readonly bool IsCheckDeviceId = ConfigurationManager.AppSettings["IsCheckDeviceId"].Trim().Equals("1") ? true : false;

        /// <summary>
        /// 有效设备ID列表，以“,”分隔
        /// </summary>
        public static readonly string ValidDeviceIdList = ConfigurationManager.AppSettings["ValidDeviceIdList"].Trim().ToString();
    }
}