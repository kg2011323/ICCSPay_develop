using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// 响应对象基类
    /// </summary>
    public class DeviceCommRespondBaseVo
    {
        /// <summary>
        /// 响应码
        /// </summary>
        private DeviceCommRespondCode _respondCode = DeviceCommRespondCode.RC9999;
        /// <summary>
        /// 响应码
        /// </summary>
        public DeviceCommRespondCode RespondCode
        {
            set { _respondCode = value; }
        }
        /// <summary>
        /// 响应码
        /// </summary>
        public string RespondCodeString
        {
            get
            {
                return String.Format("{0:D4}", ((int)_respondCode));
            }
        }
        /// <summary>
        /// 响应码描述
        /// </summary>
        public string respCodeMemo = String.Empty;
        /// <summary>
        /// 应答扩展字段
        /// </summary>
        public List<string> expandAttribute = new List<string>();
    }
}
