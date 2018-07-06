using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.Util;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 请求对象基类
    /// </summary>
    public class DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 请求系统日期
        /// </summary>
        private string reqSysDateString;
        /// <summary>
        /// 请求系统日期
        /// </summary>
        public String ReqSysDateString
        {
            set { reqSysDateString = value; }
        }
        /// <summary>
        /// 请求系统日期
        /// </summary>
        public DateTime? ReqSysDate
        {
            get { return TimeHelper.GetDateTimeYyyyMMddHHmmss( reqSysDateString); }
        }

        /// <summary>
        /// 接口编码
        /// </summary>
        public string OperationCode;
        /// <summary>
        /// 城市代码	
        /// </summary>
        public string CityCode;
        /// <summary>
        /// 设备编码
        /// </summary>
        private string _deviceId;
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceId
        {
            get
            {
                string rstDeviceId = String.Empty;
                if (null != _deviceId)
                {
                    rstDeviceId = _deviceId; 
                }
                return rstDeviceId;
            }
            set { _deviceId = value; }
        }
        /// <summary>
        /// 发起渠道
        /// </summary>
        public string ChannelType;
        /// <summary>
        /// 请求扩展字段
        /// </summary>
        public List<string> expandAttribute;
    }
}
