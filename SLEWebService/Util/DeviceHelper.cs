using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

using log4net;

namespace SLEWebService.Util
{
    /// <summary>
    /// 设备处理类
    /// </summary>
    public class DeviceHelper
    {
        #region 单例模式
        private static volatile DeviceHelper instance;
        private static object syncRoot = new Object();
        public static DeviceHelper Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                        {
                            instance = new DeviceHelper();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion 单例模式

        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 合法设备ID字典
        /// </summary>
        private Dictionary<string, string> _dictValidDeviceId = new Dictionary<string, string>();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private DeviceHelper()
        {
            try
            {
                string[] validDeviceIds = Constants.ValidDeviceIdList.Split(',');

                foreach (string eachDeviceId in validDeviceIds)
                {
                    string eachDeviceIdTrim = eachDeviceId.Trim();

                    if (!String.IsNullOrEmpty(eachDeviceIdTrim))
                    {
                        if (!_dictValidDeviceId.ContainsKey(eachDeviceIdTrim))
                        {
                            _dictValidDeviceId.Add(eachDeviceIdTrim, eachDeviceIdTrim);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// 合法设备ID判断
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool IsValidDeviceId(string deviceId)
        {
            bool isValidDeviceId = false;

            try
            {
                if (!String.IsNullOrEmpty(deviceId))
                {
                    if (_dictValidDeviceId.ContainsKey(deviceId))
                    {
                        isValidDeviceId = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return isValidDeviceId;
        }
    }
}