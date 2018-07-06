using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;
using PlatformLib.DB;


namespace PlatformLib.Util
{
    public class StationInfoHelper
    {
        #region 单例模式
        private static volatile StationInfoHelper instance;
        private static object syncRoot = new Object();
        public static StationInfoHelper Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                        {
                            instance = new StationInfoHelper();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion 单例模式

        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 最新AFC车站对象列表
        /// </summary>
        private List<AFCStationCode> _latestAFCStationCodeList = new List<AFCStationCode>();

        /// <summary>
        /// key为StationCode
        /// </summary>
        private Dictionary<string, AFCStationCode> _dictAFCStationCode = new Dictionary<string,AFCStationCode>();
    
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private StationInfoHelper()
        {
            FillData();
        }

        private bool FillLatestAFCStationCodeList()
        {
            bool isSuccess = false;

            try
            {
                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                {
                    _latestAFCStationCodeList = dbContext.AFCStationCodes.Where(s => s.RecordFlag.Equals("0")).ToList();

                    if (0 < _latestAFCStationCodeList.Count)
                    {
                        isSuccess = true;
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

            return isSuccess;
        }

        private bool FillDictAFCStationCode(List<AFCStationCode> AFCStationCodeList)
        {
            bool isSuccess = false;

            try
            {
                if ((null != AFCStationCodeList)
                    && (0 < AFCStationCodeList.Count))
                {
                    _dictAFCStationCode = new Dictionary<string, AFCStationCode>();
                    foreach (AFCStationCode eachAFCStationCode in AFCStationCodeList)
                    {
                        string eachStationCode = eachAFCStationCode.LineId + eachAFCStationCode.StationId;
                        if (!_dictAFCStationCode.ContainsKey(eachStationCode))
                        {
                            _dictAFCStationCode.Add(eachStationCode, eachAFCStationCode);
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

            return isSuccess;
        }

        private bool FillData()
        {
            bool isSuccess = true;

            isSuccess &= FillLatestAFCStationCodeList();
            isSuccess &= FillDictAFCStationCode(_latestAFCStationCodeList);

            return isSuccess;
        }

        public AFCStationCode GetAFCStationCode(string stationCode)
        {
            AFCStationCode theAFCStationCode = null;
            if (_dictAFCStationCode.ContainsKey(stationCode))
            {
                theAFCStationCode = _dictAFCStationCode[stationCode];
            }
            return theAFCStationCode;
        }

        public string GetAFCChineseStationName(string stationCode)
        {
            string strAFCChineseStationName = stationCode;

            AFCStationCode theAFCStationCode = GetAFCStationCode(stationCode);
            if (null != theAFCStationCode)
            {
                strAFCChineseStationName = theAFCStationCode.StationChaineseName;
            }

            return strAFCChineseStationName;
        }

        public string GetAFCEnglishStationName(string stationCode)
        {
            string strAFCEnglishStationName = stationCode;

            AFCStationCode theAFCStationCode = GetAFCStationCode(stationCode);
            if (null != theAFCStationCode)
            {
                strAFCEnglishStationName = theAFCStationCode.StationEnglishName;
            }

            return strAFCEnglishStationName;
        }
    }
}
