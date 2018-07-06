using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    /// <summary>
    /// OD票价对象
    /// </summary>
    public class ODTicketPriceVo
    {
        /// <summary>
        /// 起始线路代码
        /// </summary>
        public string OriLineId;
        /// <summary>
        /// 结束线路代码
        /// </summary>
        public string DesLineId;
        /// <summary>
        /// 起始线路中文名称
        /// </summary>
        public string OriLineChineseName;
        /// <summary>
        /// 结束线路中文名称
        /// </summary>
        public string DesLineChineseName;
        /// <summary>
        /// 起始线路英文名称
        /// </summary>
        public string OriLineEnglishName;
        /// <summary>
        /// 结束线路英文名称
        /// </summary>
        public string DesLineEnglishName;

        /// <summary>
        /// 起始车站代码
        /// </summary>
        public string OriStationCode;
        /// <summary>
        /// 结束车站代码
        /// </summary>
        public string DesStationCode;
        /// <summary>
        /// 起始车站中文名称
        /// </summary>
        public string OriStationChineseName;
        /// <summary>
        /// 结束车站中文名称
        /// </summary>
        public string DesStationChineseName;
        /// <summary>
        /// 起始车站英文名称
        /// </summary>
        public string OriStationEnglishName;
        /// <summary>
        /// 结束车站英文名称
        /// </summary>
        public string DesStationEnglishName;
        /// <summary>
        /// 票价，单位为分
        /// </summary>
        public int PriceCent;
    }
}
