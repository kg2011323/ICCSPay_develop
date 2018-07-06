using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using log4net;
using PlatformLib.Vo;
using PlatformLib.DB;


namespace PlatformLib.Util
{
    /// <summary>
    /// 车票
    /// </summary>
    public class TicketPriceHepler
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 返回最新票价
        /// </summary>
        /// <returns></returns>
        public List<ODTicketPriceVo> GetLatestODTicketPriceList()
        {
            List<ODTicketPriceVo> ODTicketPriceList = new List<ODTicketPriceVo>();

            try
            {
                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                {
                    List<TicketPrice> latestTicketPriceList = dbContext.TicketPrices.Where(p => p.RecordFlag.Equals("0")).ToList();

                    foreach (TicketPrice eachTicketPrice in latestTicketPriceList)
                    {
                        ODTicketPriceVo newODTicketPriceVo = new ODTicketPriceVo()
                        {
                            OriLineId = eachTicketPrice.BeginLineId,
                            DesLineId = eachTicketPrice.EndLineId,
                            OriLineChineseName = eachTicketPrice.BeginLineChineseName,
                            DesLineChineseName = eachTicketPrice.EndLineChineseName,
                            OriLineEnglishName = eachTicketPrice.BeginLineEnglishName,
                            DesLineEnglishName = eachTicketPrice.EndLineEnglishName,
                            OriStationCode = eachTicketPrice.BeginStationCode,
                            DesStationCode = eachTicketPrice.EndStationCode,
                            OriStationChineseName = eachTicketPrice.BeginStationChineseName,
                            DesStationChineseName = eachTicketPrice.EndStationChineseName,
                            OriStationEnglishName = eachTicketPrice.BeginStationEnglishName,
                            DesStationEnglishName = eachTicketPrice.EndStationEnglishName,
                            PriceCent = (int)eachTicketPrice.FareCent
                        };

                        ODTicketPriceList.Add(newODTicketPriceVo);
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

            return ODTicketPriceList;
        }
    }
}
