using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using log4net;
using PlatformLib.Vo;

namespace PlatformLib.BLL
{
    public class CommonOrderBo
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 通用订单查询
        /// </summary>
        /// <param name="isQueryUsedTime">查询使用时间标识，启用时查询已使用订单的使用时间，不启用查询所有订单的购买时间</param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="paymentVendor">必须为“1001”或“1002”，空字符串或null包含所有</param>
        /// <param name="ticketOrderType">购票方式，为None时同时查询车站和网络2种类型</param>
        /// <param name="ticketTarget">车票使用目标设备类型，为None时查询所有</param>
        /// <returns></returns>
        public List<CommonOrderVo> CommonOrderQuery(bool isQueryUsedTime, DateTime fromTime, DateTime toTime, string paymentVendor, OrderType ticketOrderType, TicketTargetType ticketTarget)
        {
            List<CommonOrderVo> commonOrderList = new List<CommonOrderVo>();

            try
            {
                if (String.IsNullOrEmpty(paymentVendor)
                    || paymentVendor.Equals("1001")
                    || paymentVendor.Equals("1002"))
                {
                    switch (ticketOrderType)
                    {
                        case OrderType.None:
                            {
                                StationOrderBo stationOrderBo = new StationOrderBo();
                                List<CommonOrderVo> stationCommonOrderList = stationOrderBo.CommonOrderQuery(isQueryUsedTime, fromTime, toTime, paymentVendor);
                                commonOrderList.AddRange(stationCommonOrderList);

                                WebPreOrderBo webPreOrderBo = new WebPreOrderBo();
                                List<CommonOrderVo> webCommonOrderList = webPreOrderBo.CommonOrderQuery(isQueryUsedTime, fromTime, toTime, paymentVendor, ticketTarget);
                                commonOrderList.AddRange(webCommonOrderList);

                                break;
                            }
                        case OrderType.StationOrder:
                            {
                                StationOrderBo stationOrderBo = new StationOrderBo();
                                commonOrderList = stationOrderBo.CommonOrderQuery(isQueryUsedTime, fromTime, toTime, paymentVendor);
                                
                                break;
                            }
                        case OrderType.WebOrder:
                            {
                                WebPreOrderBo webPreOrderBo = new WebPreOrderBo();
                                commonOrderList = webPreOrderBo.CommonOrderQuery(isQueryUsedTime, fromTime, toTime, paymentVendor, ticketTarget);
                                
                                break;
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

            return commonOrderList;
        }
    }
}
