using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;

using PlatformLib.Vo;
using PlatformLib.DB;
using PlatformLib.Util;
using ICCSPayAPI;
using System.Threading;
using System.Collections.Concurrent;


namespace PlatformLib.BLL
{
    /// <summary>
    /// 车站现场逻辑层
    /// </summary>
    public class StationOrderBo
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void TestDelegate(QRCStationOrder order, string subject, string body);
        /// <summary>
        /// 现场车站购票（付款）订单提交
        /// </summary>
        /// <param name="stationOrderPayRequestVo"></param>
        /// <returns></returns>
        public StationOrderPayRespondVo OrderPay(StationOrderPayRequestVo stationOrderPayRequestVo, ConcurrentDictionary<string, string> S1_001Dict)
        {
            StationOrderPayRespondVo result = new StationOrderPayRespondVo();
            // 临时处理
            result.partnerNo = String.Empty;
            result.notifyUrl = String.Empty;
            result.merchantCert = String.Empty;
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;
            result.timeout = 1;

            string subject = String.Empty;
            string body = String.Empty;

            try
            {
                if (!S1_001Dict.ContainsKey(stationOrderPayRequestVo.DeviceId + stationOrderPayRequestVo.pickupStationCode + stationOrderPayRequestVo.getOffStationCode + stationOrderPayRequestVo.singlelTicketNum + stationOrderPayRequestVo.ReqSysDate))  //gaoke 20160719  允许同一设备同一付款码重复发S1_001请求
                {

                    if (null != stationOrderPayRequestVo)
                    {
                        string newTradeNo = String.Format("{0}{1}", Constants.OrderNoQRCStation, TradeNoHelper.Instance.GetTradeNo());

                        //newTradeNo = newTradeNo.Substring(0, 18) + "100" + newTradeNo.Substring(21, 9);  //旧一、二号线试点

                        //newTradeNo = newTradeNo.Substring(0, 18) + "200" + newTradeNo.Substring(21, 9);  //新科样机

                        newTradeNo = newTradeNo.Substring(0, 18) + "S31" + newTradeNo.Substring(21, 9);  //测试环境  CS1广电   CS2新科   CS3三星   CS4中软   S4  第四套生产环境

                        QRCStationOrder newStationOrder = new QRCStationOrder();
                        newStationOrder.QRCStationOrderId = Guid.NewGuid();
                        newStationOrder.TradeNo = newTradeNo;
                        DateTime? dtReqSysDate = stationOrderPayRequestVo.ReqSysDate;
                        if (null == dtReqSysDate)
                        {
                            newStationOrder.BuyTime = DateTime.Now;
                        }
                        else
                        {
                            newStationOrder.BuyTime = dtReqSysDate.Value;
                        }
                        newStationOrder.OperationCode = stationOrderPayRequestVo.OperationCode;
                        newStationOrder.CityCode = stationOrderPayRequestVo.CityCode;
                        newStationOrder.DeviceId = stationOrderPayRequestVo.DeviceId;
                        newStationOrder.ChannelType = stationOrderPayRequestVo.ChannelType;
                        newStationOrder.PaymentVendor = stationOrderPayRequestVo.paymentVendor;
                        newStationOrder.PaymentCode = stationOrderPayRequestVo.paymentCode;

                        newStationOrder.OriAFCStationCode = stationOrderPayRequestVo.pickupStationCode;
                        newStationOrder.DesAFCStationCode = stationOrderPayRequestVo.getOffStationCode;
                        newStationOrder.TicketPrice = stationOrderPayRequestVo.singlelTicketPrice;
                        newStationOrder.TicketNum = stationOrderPayRequestVo.singlelTicketNum;
                        newStationOrder.SingleTicketType = stationOrderPayRequestVo.singleTicketType;
                        newStationOrder.Discount = 1;
                        newStationOrder.Amount = newStationOrder.TicketPrice * newStationOrder.TicketNum;

                        newStationOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderPayRequest);
                        newStationOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.WaitBuyerPay);
                        newStationOrder.IsValid = true;


                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            //需增加重复增加相同订单的情况 20170711  gaoke
                            QRCStationOrder existOrder = dbContext.QRCStationOrders.FirstOrDefault(wo => wo.TradeNo.Equals(newStationOrder.TradeNo));
                            if (existOrder == null)
                            {
                                dbContext.QRCStationOrders.AddObject(newStationOrder);

                                if (0 < dbContext.SaveChanges())
                                {
                                    //gaoke 20160719  将新订单加入到字典并响应S1_001请求
                                    S1_001Dict.TryAdd(stationOrderPayRequestVo.DeviceId + stationOrderPayRequestVo.pickupStationCode + stationOrderPayRequestVo.getOffStationCode + stationOrderPayRequestVo.singlelTicketNum + stationOrderPayRequestVo.ReqSysDate, newTradeNo);

                                    result.orderNo = newTradeNo;
                                    result.RespondCode = DeviceCommRespondCode.RC0000;  //gaoke 20160719  响应0000
                                    subject = "TVM扫码支付现场购票支付";
                                    result.subject = subject;

                                    #region 组装支付详细信息
                                    string oriStaitonName = StationInfoHelper.Instance.GetAFCChineseStationName(newStationOrder.OriAFCStationCode);
                                    if (String.IsNullOrEmpty(oriStaitonName))
                                    {
                                        oriStaitonName = newStationOrder.OriAFCStationCode;
                                    }
                                    string desStaitonName = StationInfoHelper.Instance.GetAFCChineseStationName(newStationOrder.DesAFCStationCode);
                                    if (String.IsNullOrEmpty(desStaitonName))
                                    {
                                        desStaitonName = newStationOrder.DesAFCStationCode;
                                    }

                                    if (String.IsNullOrEmpty(desStaitonName))
                                    {
                                        body = String.Format("起始站为{0}的{1}元单程票", oriStaitonName, (newStationOrder.Amount / 100));
                                    }
                                    else
                                    {
                                        body = String.Format("{0}到{1}的{2}元单程票", oriStaitonName, desStaitonName, (newStationOrder.Amount / 100));
                                    }
                                    result.body = body;

                                    #endregion 组装支付详细信息

                                    TestDelegate d = Test;

                                    d.BeginInvoke(newStationOrder, subject, body, null, null);


                                }
                            }
                        }

                    }
                }
                else
                {
                    result.RespondCode = DeviceCommRespondCode.RC0000;
                    result.orderNo = S1_001Dict[stationOrderPayRequestVo.DeviceId + stationOrderPayRequestVo.pickupStationCode + stationOrderPayRequestVo.getOffStationCode + stationOrderPayRequestVo.singlelTicketNum + stationOrderPayRequestVo.ReqSysDate];
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

            return result;
        }

        static void Test(QRCStationOrder newStationOrder, string subject, string body)
        {

            // 实际需支付金额，单位为分
            decimal toActualPay = newStationOrder.Amount;

            using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
            {
                var stationOrder = dbContext.QRCStationOrders.FirstOrDefault(m => m.TradeNo == newStationOrder.TradeNo);

                bool isSuccess = false;
                bool isRespondTimeout = false;
                string outTransactionId = String.Empty;
                string outPayEndTimeRaw = String.Empty;
                DateTime? outPayEndTime = null;
                int outActualfee = -1;
                string outBankType = String.Empty;
                string outUserAccount = String.Empty;
                string outUserOpenId = String.Empty;
                string outPayErrCodeDes = String.Empty;
                string userPaying = String.Empty;  //gaoke 20161012

                #region 正常支付调用

                String payURL = String.Empty;
                switch (newStationOrder.PaymentVendor)
                {
                    case "0001":
                        {
                            #region 银联支付

                            try
                            {
                                ICCSPayAPIHandle ap = new ICCSPayAPIHandle();
                                payURL = ap.ICCSPay_Natvie2_QRCode(
                                   newStationOrder.PaymentVendor
                                   , newStationOrder.TradeNo
                                   , subject
                                   , ((int)toActualPay));
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex.Message);
                                if (null != ex.InnerException)
                                {
                                    _log.Error(ex.InnerException.Message);
                                }
                            }

                            #endregion 银联支付

                            break;
                        }
                    case "1001":
                        {
                            #region 支付宝支付

                            try
                            {
                                ICCSPayAPIHandle ap = new ICCSPayAPIHandle();
                                payURL = ap.ICCSPay_Natvie2_QRCode(
                                   newStationOrder.PaymentVendor
                                   , newStationOrder.TradeNo
                                   , subject
                                   , ((int)toActualPay));
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex.Message);
                                if (null != ex.InnerException)
                                {
                                    _log.Error(ex.InnerException.Message);
                                }
                            }

                            #endregion 支付宝支付

                            break;
                        }
                    case "1002":
                        {
                            #region 微信支付

                            try
                            {
                                ICCSPayAPIHandle ap = new ICCSPayAPIHandle();
                                payURL = ap.ICCSPay_Natvie2_QRCode(
                                   newStationOrder.PaymentVendor
                                   , newStationOrder.TradeNo
                                   , subject
                                   , ((int)toActualPay));
                            }
                            catch (Exception ex)
                            {
                                _log.Error(ex.Message);
                                if (null != ex.InnerException)
                                {
                                    _log.Error(ex.InnerException.Message);
                                }
                            }

                            #endregion 微信支付

                            break;
                        }
                }

                if (!String.IsNullOrEmpty(payURL))
                {
                    stationOrder.ActualFee = toActualPay;
                    stationOrder.BankType = outBankType;
                    stationOrder.UserOpenId = outUserOpenId;
                    stationOrder.UserAccount = outUserAccount;
                    stationOrder.QRCode = payURL;


                    stationOrder.PayErrCodeDes = outPayErrCodeDes;
                    stationOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderPayRespond);  //gaoke 20161026  临时处理
                    dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
                #endregion 正常支付调用
            }
        }


        /// <summary>
        /// 扫码预支付链接查询
        /// </summary>
        /// <param name="stationSnapQRCodePrePayURLQueryRequestVo">orderNo为查询依据必须为非空</param>
        /// <returns></returns>
        public StationSnapQRCodePrePayURLQueryRespondVo SnapQRCodePrePayURLQuery(StationSnapQRCodePrePayURLQueryRequestVo stationSnapQRCodePrePayURLQueryRequestVo)
        {
            StationSnapQRCodePrePayURLQueryRespondVo result = new StationSnapQRCodePrePayURLQueryRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationSnapQRCodePrePayURLQueryRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationSnapQRCodePrePayURLQueryRequestVo.orderNo;


                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            QRCStationOrder existOrder = dbContext.QRCStationOrders.FirstOrDefault(wo => wo.TradeNo.Equals(inputTradeNo));
                            //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                            if (null != existOrder)
                            {
                                if (!existOrder.DeviceId.Equals(stationSnapQRCodePrePayURLQueryRequestVo.DeviceId))
                                {
                                    // 订单存在但已锁定
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
                                }
                                else
                                {
                                    result.orderNo = inputTradeNo;
                                    result.deviceId = existOrder.TradeNo;
                                    while(true)
                                    {
                                        if (existOrder.QRCode == "")
                                        {
                                            Thread.Sleep(1000);
                                            existOrder = dbContext.QRCStationOrders.FirstOrDefault(wo => wo.TradeNo.Equals(inputTradeNo));
                                            _log.Info("获取不到支付链接！");
                                        }
                                        else
                                        {
                                            result.qrCode = existOrder.QRCode;
                                            result.RespondCode = DeviceCommRespondCode.RC0000;
                                            dbContext.SaveChanges();
                                            _log.Info("获取到支付链接！");
                                            break;
                                        }
                                    }
                                }
                            }
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

            return result;
        }



        /// <summary>
        /// 扫码支付结果查询
        /// </summary>
        /// <param name="stationSnapQRCodePayResultQueryRequestVo">orderNo为查询依据必须为非空</param>
        /// <returns></returns>
        public StationSnapQRCodePayResultQueryRespondVo SnapQRCodePayResultQuery(StationSnapQRCodePayResultQueryRequestVo stationSnapQRCodePayResultQueryRequestVo)
        {
            StationSnapQRCodePayResultQueryRespondVo result = new StationSnapQRCodePayResultQueryRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationSnapQRCodePayResultQueryRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationSnapQRCodePayResultQueryRequestVo.orderNo;


                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            QRCStationOrder existOrder = dbContext.QRCStationOrders.FirstOrDefault(wo => wo.TradeNo.Equals(inputTradeNo));
                            IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                            if (null != existOrder)
                            {
                                // 订单存在
                                result.orderNo = existOrder.TradeNo;

                                if (!existOrder.DeviceId.Equals(stationSnapQRCodePayResultQueryRequestVo.DeviceId))
                                {
                                    // 订单存在但已锁定
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
                                }
                                else
                                {
                                    if (null != existOrder.PayEndTime)
                                    {
                                        result.paymentDate = existOrder.PayEndTime.Value.ToString();
                                    }
                                    else
                                    {
                                        result.paymentDate = existOrder.PayEndTimeRaw;
                                    }
                                    if (null != existOrder.ActualFee)
                                    {
                                        try
                                        {
                                            result.amount = existOrder.Amount.ToString();//Convert.ToInt32(existOrder.ActualFee.Value);
                                        }
                                        catch (Exception)
                                        { }
                                    }
                                    // 临时处理
                                    result.paymentAccount = existOrder.UserAccount;

                                    bool isOrderStatusVaild = false;
                                    #region 判断订单状态
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existOrder.OrderStatus);
                                        if (existOrderStatus == OrderStatusType.WaitBuyerPay)
                                        {
                                            ICCSPayAPIResultVO payResultVo = null;
                                            try
                                            {
                                                ICCSPayAPIHandle ap = new ICCSPayAPIHandle();
                                                payResultVo = ap.ICCSPay_Orderquery(existOrder.PaymentVendor, existOrder.TradeNo);
                                            }
                                            catch (Exception ex)
                                            { }
                                            if (payResultVo != null && payResultVo.is_success)
                                            {
                                                if ((existOrder.PaymentVendor == "1001" && payResultVo.result_code == "TRADE_SUCCESS" && payResultVo.ActualFee > 1 && payResultVo.ActualFee * 100 == (int)existOrder.Amount) || (existOrder.PaymentVendor == "1002" && payResultVo.result_code == "OK" && payResultVo.ActualFee > 100 && payResultVo.ActualFee == (int)existOrder.Amount) || (existOrder.PaymentVendor == "0001" && payResultVo.result_code == "SUCCESS" && payResultVo.ActualFee > 100 && payResultVo.ActualFee == (int)existOrder.Amount))
                                                {
                                                    #region 支付成功
                                                    existOrder.TransactionId = payResultVo.transactionId;
                                                    existOrder.PayEndTimeRaw = payResultVo.returnEndTimeRaw;
                                                    DateTime? dtPayEndTime = TimeHelper.GetDateTimeYyyyMMddHHmmss(payResultVo.returnEndTimeRaw);

                                                    if (null != dtPayEndTime)
                                                    {
                                                        existOrder.PayEndTime = dtPayEndTime.Value;
                                                        //_log.Info("支付结束时间：" + existOrder.PayEndTime);
                                                    }
                                                    else
                                                    {
                                                        existOrder.PayEndTime = DateTime.Now;
                                                        //_log.Info("支付结束时间为空！");
                                                    }

                                                    if (existOrder.PaymentVendor == "1001")
                                                    {
                                                        existOrder.ActualFee = payResultVo.ActualFee * 100;
                                                    }
                                                    else if (existOrder.PaymentVendor == "1002" || existOrder.PaymentVendor == "0001")
                                                    {
                                                        existOrder.ActualFee = payResultVo.ActualFee;
                                                    }
                                                    existOrder.BankType = payResultVo.BankType;
                                                    existOrder.UserOpenId = payResultVo.UserOpenId;
                                                    existOrder.UserAccount = payResultVo.buyer_logon_id;


                                                    existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TradeSuccess);
                                                    //dbContext.SaveChanges();
                                                    #endregion 支付成功
                                                    existOrderStatus = OrderStatusType.TradeSuccess;
                                                    //_log.Info("订单 " + existOrder.TradeNo + " 查询支付结果为成功！");
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    { }
                                    switch (existOrderStatus)
                                    {
                                        case OrderStatusType.None:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC9999;
                                                result.paymentResult = "FAILED";
                                                break;
                                            }
                                        case OrderStatusType.WaitBuyerPay:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0002;
                                                result.paymentResult = "PROCEDING";
                                                break;
                                            }
                                        case OrderStatusType.TradeTimeout:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;
                                                result.paymentResult = "FAILED";
                                                break;
                                            }
                                        case OrderStatusType.TradeSuccess:
                                            {
                                                isOrderStatusVaild = true;
                                                result.paymentResult = "SUCCESS";
                                                break;
                                            }
                                        case OrderStatusType.TradeFaild:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;
                                                result.paymentResult = "FAILED";
                                                break;
                                            }
                                        case OrderStatusType.TicketOut:
                                            {
                                                //result.RespondCode = DeviceCommRespondCode.RC0008;
                                                result.RespondCode = DeviceCommRespondCode.RC0000;  //gaoke 20160720  已出票的情况下还是响应0000，由设备控制只出一次票
                                                result.paymentResult = "SUCCESS";
                                                break;
                                            }
                                        case OrderStatusType.TicketException:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;
                                                result.paymentResult = "FAILED";
                                                break;
                                            }
                                        case OrderStatusType.RefundProcessing:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;
                                                result.paymentResult = "SUCCESS";
                                                break;
                                            }
                                        case OrderStatusType.RefundSuccess:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0010;
                                                result.paymentResult = "SUCCESS";
                                                break;
                                            }
                                        case OrderStatusType.RefundFail:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;
                                                result.paymentResult = "SUCCESS";
                                                break;
                                            }
                                        case OrderStatusType.TradeInvalid:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;
                                                result.paymentResult = "FAILED";
                                                break;
                                            }
                                        case OrderStatusType.TradeClosed:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;
                                                result.paymentResult = "FAILED";
                                                break;
                                            }
                                    }
                                    #endregion 判断订单状态

                                    if (isOrderStatusVaild)
                                    {
                                        bool isOrderStepVaild = false;

                                        result.RespondCode = DeviceCommRespondCode.RC0009;

                                        #region 判断订单步骤
                                        try
                                        {
                                            int existStepFlag = Convert.ToInt32(existOrder.Step);
                                            int newStepFlag = EnumHelper.GetStationOrderStepFlag(StationOrderStep.StationOrderPayResultRespond);

                                            if (newStepFlag >= existStepFlag)  //允许重发S1_002请求
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderPayResultRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            result.paymentResult = "SUCCESS";

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderPayResultRespond);
                                        }
                                    }

                                    // 支付消息
                                    result.paymentDesc = existOrder.PayErrCodeDes;
                                    result.userName = existOrder.UserAccount;
                                }

                                dbContext.SaveChanges();
                            }
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

            return result;
        }


        /// <summary>
        /// 现场车站购票支付结果查询
        /// </summary>
        /// <param name="stationOrderPayResultRequestVo">orderNo为查询依据必须为非空</param>
        /// <returns></returns>
        public StationOrderPayResultRespondVo PayResultQuery(StationOrderPayResultRequestVo stationOrderPayResultRequestVo)
        {
            StationOrderPayResultRespondVo result = new StationOrderPayResultRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationOrderPayResultRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationOrderPayResultRequestVo.orderNo;


                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            StationOrder existOrder = dbContext.StationOrders.FirstOrDefault(wo => wo.TradeNo.Equals(inputTradeNo));
                            IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                            if (null != existOrder)
                            {
                                // 订单存在

                                if (!existOrder.DeviceId.Equals(stationOrderPayResultRequestVo.DeviceId))
                                {
                                    // 订单存在但已锁定
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
                                }
                                else
                                {
                                    result.PaymentDate = new DateTime();
                                    if (null != existOrder.PayEndTime)
                                    {
                                        result.PaymentDate = existOrder.PayEndTime.Value;
                                    }
                                    else
                                    {
                                        result.PaymentDate = DateTime.ParseExact(existOrder.PayEndTimeRaw, "yyyyMMddHHmmss", format);  //string型时间转成DateTime型
                                    }
                                    if (null != existOrder.ActualFee)
                                    {
                                        try
                                        {
                                            result.amount = Convert.ToInt32(existOrder.ActualFee.Value);
                                        }
                                        catch (Exception)
                                        { }
                                    }
                                    // 临时处理
                                    result.paymentAccount = existOrder.UserAccount;

                                    bool isOrderStatusVaild = false;
                                    #region 判断订单状态
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existOrder.OrderStatus);
                                    }
                                    catch (Exception)
                                    { }
                                    switch (existOrderStatus)
                                    {
                                        case OrderStatusType.None:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC9999;

                                                break;
                                            }
                                        case OrderStatusType.WaitBuyerPay:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0002;

                                                break;
                                            }
                                        case OrderStatusType.TradeTimeout:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;

                                                break;
                                            }
                                        case OrderStatusType.TradeSuccess:
                                            {
                                                isOrderStatusVaild = true;

                                                break;
                                            }
                                        case OrderStatusType.TradeFaild:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;

                                                break;
                                            }
                                        case OrderStatusType.TicketOut:
                                            {
                                                //result.RespondCode = DeviceCommRespondCode.RC0008;
                                                result.RespondCode = DeviceCommRespondCode.RC0000;  //gaoke 20160720  已出票的情况下还是响应0000，由设备控制只出一次票

                                                break;
                                            }
                                        case OrderStatusType.TicketException:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundProcessing:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundSuccess:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0010;

                                                break;
                                            }
                                        case OrderStatusType.RefundFail:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeInvalid:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeClosed:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                    }
                                    #endregion 判断订单状态

                                    if (isOrderStatusVaild)
                                    {
                                        bool isOrderStepVaild = false;

                                        result.RespondCode = DeviceCommRespondCode.RC0009;

                                        #region 判断订单步骤
                                        try
                                        {
                                            int existStepFlag = Convert.ToInt32(existOrder.Step);
                                            int newStepFlag = EnumHelper.GetStationOrderStepFlag(StationOrderStep.StationOrderPayResultRespond);

                                            if (newStepFlag >= existStepFlag)  //允许重发S1_002请求
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderPayResultRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            result.IsPaymentSuccess = true;

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderPayResultRespond);
                                        }
                                    }

                                    // 支付消息
                                    result.paymentDesc = existOrder.PayErrCodeDes;
                                }

                                dbContext.SaveChanges();
                            }
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

            return result;
        }



        /// <summary>
        /// 订单开始执行
        /// </summary>
        /// <param name="stationOrderProcessRequestVo"></param>
        /// <returns></returns>
        public StationOrderProcessRespondVo StationOrderProcess(StationOrderProcessRequestVo stationOrderProcessRequestVo)
        {
            StationOrderProcessRespondVo result = new StationOrderProcessRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationOrderProcessRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationOrderProcessRequestVo.orderNo;
                    string inputDeviceId = stationOrderProcessRequestVo.DeviceId;

                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            StationOrder existOrder = dbContext.StationOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

                            if (null != existOrder)
                            {
                                // 订单存在
                                if (!existOrder.DeviceId.Equals(inputDeviceId))
                                {
                                    // 订单存在但已锁定
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
                                }
                                else
                                {
                                    bool isOrderStatusVaild = false;
                                    #region 判断订单状态
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existOrder.OrderStatus);
                                    }
                                    catch (Exception)
                                    { }

                                    switch (existOrderStatus)
                                    {
                                        case OrderStatusType.None:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC9999;

                                                break;
                                            }
                                        case OrderStatusType.WaitBuyerPay:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0007;

                                                break;
                                            }
                                        case OrderStatusType.TradeTimeout:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0002;

                                                break;
                                            }
                                        case OrderStatusType.TradeSuccess:
                                            {
                                                isOrderStatusVaild = true;

                                                break;
                                            }
                                        case OrderStatusType.TradeFaild:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;

                                                break;
                                            }
                                        case OrderStatusType.TicketOut:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0008;

                                                break;
                                            }
                                        case OrderStatusType.TicketException:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundProcessing:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundSuccess:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0010;

                                                break;
                                            }
                                        case OrderStatusType.RefundFail:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeInvalid:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeClosed:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                    }
                                    #endregion 判断订单状态

                                    if (isOrderStatusVaild)
                                    {
                                        bool isOrderStepVaild = false;

                                        result.RespondCode = DeviceCommRespondCode.RC0009;

                                        #region 判断订单步骤
                                        try
                                        {
                                            int existStepFlag = Convert.ToInt32(existOrder.Step);
                                            int newStepFlag = EnumHelper.GetStationOrderStepFlag(StationOrderStep.StationOrderProcessRespond);

                                            if (newStepFlag >= existStepFlag)
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderProcessRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderProcessRespond);
                                        }
                                    }

                                    dbContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.RespondCode = DeviceCommRespondCode.RC9999;

                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 订单执行结果通知
        /// </summary>
        /// <param name="stationOrderTakenRequestVo"></param>
        /// <returns></returns>
        public StationOrderTakenRespondVo StationOrderTaken(StationOrderTakenRequestVo stationOrderTakenRequestVo)
        {
            StationOrderTakenRespondVo result = new StationOrderTakenRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationOrderTakenRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationOrderTakenRequestVo.orderNo;
                    string inputDeviceId = stationOrderTakenRequestVo.DeviceId;

                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {

                            StationOrder existOrder = dbContext.StationOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

                            if (null != existOrder)
                            {
                                // 订单存在
                                if (!existOrder.DeviceId.Equals(inputDeviceId))
                                {
                                    // 订单存在但已锁定
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
                                }
                                else
                                {
                                    bool isOrderStatusVaild = false;
                                    #region 判断订单状态
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existOrder.OrderStatus);
                                    }
                                    catch (Exception)
                                    { }
                                    switch (existOrderStatus)
                                    {
                                        case OrderStatusType.None:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC9999;

                                                break;
                                            }
                                        case OrderStatusType.WaitBuyerPay:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0007;

                                                break;
                                            }
                                        case OrderStatusType.TradeTimeout:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0002;

                                                break;
                                            }
                                        case OrderStatusType.TradeSuccess:
                                            {
                                                isOrderStatusVaild = true;

                                                break;
                                            }
                                        case OrderStatusType.TradeFaild:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;

                                                break;
                                            }
                                        case OrderStatusType.TicketOut:
                                            {
                                                //result.RespondCode = DeviceCommRespondCode.RC0008;
                                                isOrderStatusVaild = true;   //gaoke  20160719   取票成功也返回0000

                                                break;
                                            }
                                        case OrderStatusType.TicketException:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundProcessing:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundSuccess:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0010;

                                                break;
                                            }
                                        case OrderStatusType.RefundFail:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeInvalid:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeClosed:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                    }
                                    #endregion 判断订单状态

                                    if (isOrderStatusVaild)
                                    {
                                        bool isOrderStepVaild = false;

                                        result.RespondCode = DeviceCommRespondCode.RC0009;

                                        #region 判断订单步骤
                                        try
                                        {
                                            int existStepFlag = Convert.ToInt32(existOrder.Step);
                                            int newStepFlag = EnumHelper.GetStationOrderStepFlag(StationOrderStep.StationOrderTakenRespond);

                                            //gaoke 20160707  允许重发S1_004请求
                                            if (newStepFlag >= existStepFlag)
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            // 未取票
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            string inputTakeTicketNum = stationOrderTakenRequestVo.takeSingleTicketNum;
                                            int takeTicketNum = 0;
                                            if (!int.TryParse(inputTakeTicketNum, out takeTicketNum))
                                            {

                                            }
                                            existOrder.TicketTakeNum = takeTicketNum;

                                            DateTime? dtTakeTicketDate = stationOrderTakenRequestVo.takeSingleTicketDate;
                                            if (null == dtTakeTicketDate)
                                            {
                                                dtTakeTicketDate = DateTime.Now;
                                            }
                                            existOrder.TicketTakeTime = dtTakeTicketDate.Value;

                                            if (existOrder.TicketTakeNum.Equals(existOrder.TicketNum))
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketOut);
                                            }
                                            else
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketException);
                                            }
                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenRespond);
                                        }

                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.RespondCode = DeviceCommRespondCode.RC9999;

                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 订单执行结果通知
        /// </summary>
        /// <param name="stationOrderTakenRequestVo"></param>
        /// <returns></returns>
        public StationOrderTakenRespondVo QRCStationOrderTaken(StationOrderTakenRequestVo stationOrderTakenRequestVo)
        {
            StationOrderTakenRespondVo result = new StationOrderTakenRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationOrderTakenRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationOrderTakenRequestVo.orderNo;
                    string inputDeviceId = stationOrderTakenRequestVo.DeviceId;

                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {

                            QRCStationOrder existOrder = dbContext.QRCStationOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

                            if (null != existOrder)
                            {
                                // 订单存在
                                if (!existOrder.DeviceId.Equals(inputDeviceId))
                                {
                                    // 订单存在但已锁定
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
                                }
                                else
                                {
                                    bool isOrderStatusVaild = false;
                                    #region 判断订单状态
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existOrder.OrderStatus);
                                    }
                                    catch (Exception)
                                    { }
                                    switch (existOrderStatus)
                                    {
                                        case OrderStatusType.None:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC9999;

                                                break;
                                            }
                                        case OrderStatusType.WaitBuyerPay:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0007;

                                                break;
                                            }
                                        case OrderStatusType.TradeTimeout:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0002;

                                                break;
                                            }
                                        case OrderStatusType.TradeSuccess:
                                            {
                                                isOrderStatusVaild = true;

                                                break;
                                            }
                                        case OrderStatusType.TradeFaild:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;

                                                break;
                                            }
                                        case OrderStatusType.TicketOut:
                                            {
                                                //result.RespondCode = DeviceCommRespondCode.RC0008;
                                                isOrderStatusVaild = true;   //gaoke  20160719   取票成功也返回0000

                                                break;
                                            }
                                        case OrderStatusType.TicketException:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundProcessing:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundSuccess:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0010;

                                                break;
                                            }
                                        case OrderStatusType.RefundFail:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeInvalid:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeClosed:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                    }
                                    #endregion 判断订单状态

                                    if (isOrderStatusVaild)
                                    {
                                        bool isOrderStepVaild = false;

                                        result.RespondCode = DeviceCommRespondCode.RC0009;

                                        #region 判断订单步骤
                                        try
                                        {
                                            int existStepFlag = Convert.ToInt32(existOrder.Step);
                                            int newStepFlag = EnumHelper.GetStationOrderStepFlag(StationOrderStep.StationOrderTakenRespond);

                                            //gaoke 20160707  允许重发S1_004请求
                                            if (newStepFlag >= existStepFlag)
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            // 未取票
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            string inputTakeTicketNum = stationOrderTakenRequestVo.takeSingleTicketNum;
                                            int takeTicketNum = 0;
                                            if (!int.TryParse(inputTakeTicketNum, out takeTicketNum))
                                            {

                                            }
                                            existOrder.TicketTakeNum = takeTicketNum;

                                            DateTime? dtTakeTicketDate = stationOrderTakenRequestVo.takeSingleTicketDate;
                                            if (null == dtTakeTicketDate)
                                            {
                                                dtTakeTicketDate = DateTime.Now;
                                            }
                                            existOrder.TicketTakeTime = dtTakeTicketDate.Value;

                                            if (existOrder.TicketTakeNum.Equals(existOrder.TicketNum))
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketOut);
                                            }
                                            else
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketException);
                                            }
                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenRespond);
                                        }

                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.RespondCode = DeviceCommRespondCode.RC9999;

                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 网络订单执行故障通知
        /// </summary>
        /// <param name="stationOrderTakenErrRequestVo"></param>
        /// <returns></returns>
        public StationOrderTakenErrRespondVo StationOrderTakenErr(StationOrderTakenErrRequestVo stationOrderTakenErrRequestVo)
        {
            StationOrderTakenErrRespondVo result = new StationOrderTakenErrRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationOrderTakenErrRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationOrderTakenErrRequestVo.orderNo;
                    string inputDeviceId = stationOrderTakenErrRequestVo.DeviceId;

                    if (!String.IsNullOrEmpty(inputTradeNo) && inputTradeNo.Length == 30)
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            StationOrder existOrder = dbContext.StationOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

                            if (null != existOrder)
                            {
                                // 订单存在
                                if (!existOrder.DeviceId.Equals(inputDeviceId))
                                {
                                    // 订单存在但已锁定
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
                                }
                                else
                                {
                                    bool isOrderStatusVaild = false;
                                    #region 判断订单状态
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existOrder.OrderStatus);
                                    }
                                    catch (Exception)
                                    { }

                                    switch (existOrderStatus)
                                    {
                                        case OrderStatusType.None:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC9999;

                                                break;
                                            }
                                        case OrderStatusType.WaitBuyerPay:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0007;

                                                break;
                                            }
                                        case OrderStatusType.TradeTimeout:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0002;

                                                break;
                                            }
                                        case OrderStatusType.TradeSuccess:
                                            {
                                                isOrderStatusVaild = true;

                                                break;
                                            }
                                        case OrderStatusType.TradeFaild:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0003;

                                                break;
                                            }
                                        case OrderStatusType.TicketOut:
                                            {
                                                isOrderStatusVaild = true;

                                                break;
                                            }
                                        case OrderStatusType.TicketException:
                                            {
                                                isOrderStatusVaild = true;

                                                break;
                                            }
                                        case OrderStatusType.RefundProcessing:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.RefundSuccess:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0010;

                                                break;
                                            }
                                        case OrderStatusType.RefundFail:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeInvalid:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                        case OrderStatusType.TradeClosed:
                                            {
                                                result.RespondCode = DeviceCommRespondCode.RC0009;

                                                break;
                                            }
                                    }
                                    #endregion 判断订单状态

                                    if (isOrderStatusVaild)
                                    {
                                        result.RespondCode = DeviceCommRespondCode.RC0000;

                                        string inputTakeTicketNum = stationOrderTakenErrRequestVo.takeSingleTicketNum;
                                        int takeTicketNum = 0;
                                        if (!int.TryParse(inputTakeTicketNum, out takeTicketNum))
                                        {

                                        }
                                        existOrder.TicketTakeNum = takeTicketNum;

                                        DateTime? dtTakeTicketErrDate = stationOrderTakenErrRequestVo.faultOccurDate;
                                        if (null == dtTakeTicketErrDate)
                                        {
                                            dtTakeTicketErrDate = DateTime.Now;
                                        }
                                        existOrder.DeviceErrTime = dtTakeTicketErrDate.Value;
                                        existOrder.DeviceErrCode = stationOrderTakenErrRequestVo.errorCode;
                                        existOrder.DeviceErrMessage = stationOrderTakenErrRequestVo.errorMessage;
                                        existOrder.DeviceErrSlipSeq = stationOrderTakenErrRequestVo.faultSlipSeq;

                                        existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketException);
                                        existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenErrRespond);

                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    else if (!String.IsNullOrEmpty(inputTradeNo) && inputTradeNo.Length != 30)   //gaoke 20161020  增加处理S1_005接口请求无订单号的情况
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            StationOrder existOrder = dbContext.StationOrders.FirstOrDefault(wo => (wo.PaymentCode.Equals(inputTradeNo) && wo.OrderStatus.Equals("3")));
                            if (existOrder != null && stationOrderTakenErrRequestVo.errorCode == "0007" && !String.IsNullOrEmpty(existOrder.TradeNo))
                            {
                                //将订单退款情况记录到退款表中
                                WebOrderRefund newWebOrderRefund = new WebOrderRefund();   //gaoke 20170213  
                                newWebOrderRefund.RequestTime = DateTime.Now;
                                //如果支付成功后无订单号，自动退款
                                ICCSPayAPIHandle ap = new ICCSPayAPIHandle();
                                ICCSPayAPIResultVO refundVO = ap.ICCSPay_Refund(existOrder.PaymentVendor, existOrder.TransactionId, existOrder.TradeNo, Convert.ToInt32(existOrder.Amount), Convert.ToInt32(existOrder.ActualFee));

                                existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenErrRespond);

                                newWebOrderRefund.WebOrderRefundId = Guid.NewGuid();
                                newWebOrderRefund.WebOrderId = Guid.NewGuid();
                                newWebOrderRefund.ExternalTradeNo = Guid.NewGuid().ToString();
                                newWebOrderRefund.TradeNo = existOrder.TradeNo;
                                newWebOrderRefund.RefundTradeNo = existOrder.TransactionId;//待完善
                                newWebOrderRefund.RefundReason = "设备申请退款";
                                newWebOrderRefund.PaymentVendor = existOrder.PaymentVendor;
                                newWebOrderRefund.RefundFee = (decimal)existOrder.ActualFee;
                                newWebOrderRefund.TotalFee = existOrder.Amount;
                                newWebOrderRefund.BankType = existOrder.BankType;

                                newWebOrderRefund.IsRequestSuccess = true;
                                newWebOrderRefund.IsRespondSuccess = true;
                                newWebOrderRefund.RespondTime = DateTime.Now;
                                if (refundVO.is_success)
                                {
                                    existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.RefundSuccess);
                                    newWebOrderRefund.OrderStatus = "8";
                                }
                                else
                                {
                                    existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.RefundProcessing);
                                    newWebOrderRefund.OrderStatus = "7";
                                }
                                dbContext.WebOrderRefunds.AddObject(newWebOrderRefund);

                                dbContext.SaveChanges();

                                result.RespondCode = DeviceCommRespondCode.RC0000;  //gaoke 20161020
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.RespondCode = DeviceCommRespondCode.RC9999;

                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 网络订单执行故障通知
        /// </summary>
        /// <param name="stationOrderTakenErrRequestVo"></param>
        /// <returns></returns>
        public StationOrderTakenErrRespondVo QRCStationOrderTakenErr(StationOrderTakenErrRequestVo stationOrderTakenErrRequestVo)
        {
            StationOrderTakenErrRespondVo result = new StationOrderTakenErrRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != stationOrderTakenErrRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = stationOrderTakenErrRequestVo.orderNo;
                    string inputDeviceId = stationOrderTakenErrRequestVo.DeviceId;

                    if (!String.IsNullOrEmpty(inputTradeNo) && inputTradeNo.Length == 30)
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            QRCStationOrder existOrder = dbContext.QRCStationOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));


                            if (null != existOrder)
                            {
                                existOrder.DeviceErrCode = stationOrderTakenErrRequestVo.errorCode;
                                dbContext.SaveChanges();
                                // 撤销订单
                                if (stationOrderTakenErrRequestVo.errorCode == "0007")
                                {
                                    existOrder.DeviceErrCode = stationOrderTakenErrRequestVo.errorCode;
                                    dbContext.SaveChanges();
                                    result.RespondCode = DeviceCommRespondCode.RC0000;  //gaoke 20161020
                                    //如果支付成功后无订单号，自动退款
                                    ICCSPayAPIResultVO payResultVo = null;
                                    ICCSPayAPIHandle ap = new ICCSPayAPIHandle();

                                    payResultVo = ap.ICCSPay_Orderquery(existOrder.PaymentVendor, existOrder.TradeNo);

                                    if (existOrder.OrderStatus.Equals("3") || (payResultVo != null && payResultVo.is_success && payResultVo.ActualFee > 100 && existOrder.PaymentVendor == "0001" && payResultVo.result_code == "SUCCESS") || (payResultVo != null && payResultVo.is_success && payResultVo.ActualFee > 1 && existOrder.PaymentVendor == "1001" && payResultVo.result_code == "TRADE_SUCCESS") || (payResultVo != null && payResultVo.is_success && payResultVo.ActualFee > 100 && existOrder.PaymentVendor == "1002" && payResultVo.result_code == "OK"))
                                    {
                                        //将订单退款情况记录到退款表中
                                        WebOrderRefund newWebOrderRefund = new WebOrderRefund();   //gaoke 20170213  
                                        newWebOrderRefund.RequestTime = DateTime.Now;
                                        ICCSPayAPIResultVO refundVo = ap.ICCSPay_Refund(existOrder.PaymentVendor, existOrder.TransactionId, existOrder.TradeNo, Convert.ToInt32(existOrder.ActualFee), Convert.ToInt32(existOrder.ActualFee));
                                        _log.Info("订单 " + existOrder.TradeNo + " 的退款结果为总金额： " + refundVo.ActualFee + ",退款金额： " + refundVo.refund_fee + ", 退款结果： " + refundVo.is_success);
                                        if (refundVo.is_success)   //20170607  gaoke  增加退款结果判断逻辑
                                        {
                                            existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.RefundSuccess);
                                            newWebOrderRefund.OrderStatus = "8";
                                        }
                                        else
                                        {
                                            existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.RefundProcessing);
                                            newWebOrderRefund.OrderStatus = "7";
                                        }

                                        existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenErrRespond);
                                        //existOrder.DeviceErrCode = stationOrderTakenErrRequestVo.errorCode;

                                        newWebOrderRefund.WebOrderRefundId = Guid.NewGuid();
                                        newWebOrderRefund.WebOrderId = Guid.NewGuid();
                                        newWebOrderRefund.ExternalTradeNo = Guid.NewGuid().ToString();
                                        newWebOrderRefund.TradeNo = existOrder.TradeNo;
                                        newWebOrderRefund.RefundTradeNo = existOrder.TransactionId;//待完善
                                        newWebOrderRefund.RefundReason = "设备申请退款";
                                        newWebOrderRefund.PaymentVendor = existOrder.PaymentVendor;
                                        newWebOrderRefund.RefundFee = (decimal)existOrder.ActualFee;
                                        newWebOrderRefund.TotalFee = existOrder.Amount;
                                        newWebOrderRefund.BankType = existOrder.BankType;

                                        newWebOrderRefund.IsRequestSuccess = true;
                                        newWebOrderRefund.IsRespondSuccess = true;
                                        newWebOrderRefund.RespondTime = DateTime.Now;
                                        WebOrderRefund webOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));
                                        if (webOrderRefund == null)//避免对同一订单号进行多次插入退款表操作   gaoke  20170619
                                        {
                                            dbContext.WebOrderRefunds.AddObject(newWebOrderRefund);
                                            dbContext.SaveChanges();
                                        }
                                        else
                                        {
                                            _log.Info("订单 " + existOrder.TradeNo + " 已存在在退款表中！");
                                        }
                                    }
                                    else
                                    {
                                        existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenErrRespond);
                                    }
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    // 订单存在
                                    if (!existOrder.DeviceId.Equals(inputDeviceId))
                                    {
                                        // 订单存在但已锁定
                                        result.RespondCode = DeviceCommRespondCode.RC0009;
                                    }
                                    else
                                    {
                                        bool isOrderStatusVaild = false;
                                        #region 判断订单状态
                                        OrderStatusType existOrderStatus = OrderStatusType.None;
                                        try
                                        {
                                            existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existOrder.OrderStatus);
                                        }
                                        catch (Exception)
                                        { }

                                        switch (existOrderStatus)
                                        {
                                            case OrderStatusType.None:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC9999;

                                                    break;
                                                }
                                            case OrderStatusType.WaitBuyerPay:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0007;

                                                    break;
                                                }
                                            case OrderStatusType.TradeTimeout:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0002;

                                                    break;
                                                }
                                            case OrderStatusType.TradeSuccess:
                                                {
                                                    isOrderStatusVaild = true;

                                                    break;
                                                }
                                            case OrderStatusType.TradeFaild:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0003;

                                                    break;
                                                }
                                            case OrderStatusType.TicketOut:
                                                {
                                                    isOrderStatusVaild = true;

                                                    break;
                                                }
                                            case OrderStatusType.TicketException:
                                                {
                                                    isOrderStatusVaild = true;

                                                    break;
                                                }
                                            case OrderStatusType.RefundProcessing:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0009;

                                                    break;
                                                }
                                            case OrderStatusType.RefundSuccess:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0010;

                                                    break;
                                                }
                                            case OrderStatusType.RefundFail:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0009;

                                                    break;
                                                }
                                            case OrderStatusType.TradeInvalid:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0009;

                                                    break;
                                                }
                                            case OrderStatusType.TradeClosed:
                                                {
                                                    result.RespondCode = DeviceCommRespondCode.RC0009;

                                                    break;
                                                }
                                        }
                                        #endregion 判断订单状态

                                        if (isOrderStatusVaild)
                                        {
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            string inputTakeTicketNum = stationOrderTakenErrRequestVo.takeSingleTicketNum;
                                            int takeTicketNum = 0;
                                            if (!int.TryParse(inputTakeTicketNum, out takeTicketNum))
                                            {

                                            }
                                            existOrder.TicketTakeNum = takeTicketNum;

                                            DateTime? dtTakeTicketErrDate = stationOrderTakenErrRequestVo.faultOccurDate;
                                            if (null == dtTakeTicketErrDate)
                                            {
                                                dtTakeTicketErrDate = DateTime.Now;
                                            }
                                            existOrder.DeviceErrTime = dtTakeTicketErrDate.Value;
                                            existOrder.DeviceErrCode = stationOrderTakenErrRequestVo.errorCode;
                                            existOrder.DeviceErrMessage = stationOrderTakenErrRequestVo.errorMessage;
                                            existOrder.DeviceErrSlipSeq = stationOrderTakenErrRequestVo.faultSlipSeq;

                                            if (existOrder.QRCode == null)
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TradeClosed);
                                            }
                                            else
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketException);
                                            }

                                            existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenErrRespond);

                                            dbContext.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (!String.IsNullOrEmpty(inputTradeNo) && inputTradeNo.Length != 30)   //gaoke 20161020  增加处理S1_005接口请求无订单号的情况
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            QRCStationOrder existOrder = dbContext.QRCStationOrders.FirstOrDefault(wo => (wo.PaymentCode.Equals(inputTradeNo)));
                            WebOrderRefund webOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => wo.TradeNo.Equals(dbContext.QRCStationOrders.FirstOrDefault(wo1 => wo1.PaymentCode.Equals(inputTradeNo))));
                            if (existOrder != null && !String.IsNullOrEmpty(existOrder.TradeNo) && stationOrderTakenErrRequestVo.errorCode == "0007")
                            {
                                ICCSPayAPIResultVO payResultVo = null;
                                ICCSPayAPIHandle ap = new ICCSPayAPIHandle();

                                payResultVo = ap.ICCSPay_Orderquery(existOrder.PaymentVendor, existOrder.TradeNo);
                                if (existOrder.OrderStatus.Equals("3") || (payResultVo != null && payResultVo.is_success && payResultVo.ActualFee > 100 && existOrder.PaymentVendor == "0001" && payResultVo.result_code == "SUCCESS") || (payResultVo != null && payResultVo.is_success && payResultVo.ActualFee > 1 && existOrder.PaymentVendor == "1001" && payResultVo.result_code == "TRADE_SUCCESS") || (payResultVo != null && payResultVo.is_success && payResultVo.ActualFee > 100 && existOrder.PaymentVendor == "1002" && payResultVo.result_code == "OK"))
                                {
                                    //将订单退款情况记录到退款表中
                                    WebOrderRefund newWebOrderRefund = new WebOrderRefund();   //gaoke 20170213  
                                    newWebOrderRefund.RequestTime = DateTime.Now;
                                    //如果支付成功后无订单号，自动退款
                                    ICCSPayAPIResultVO refundVO = ap.ICCSPay_Refund(existOrder.PaymentVendor, existOrder.TransactionId, existOrder.TradeNo, Convert.ToInt32(existOrder.ActualFee), Convert.ToInt32(existOrder.ActualFee));
                                    _log.Info("订单 " + existOrder.TradeNo + " 的退款结果为总金额： " + refundVO.ActualFee + ",退款金额： " + refundVO.refund_fee + ", 退款结果： " + refundVO.is_success);

                                    existOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenErrRespond);
                                    existOrder.DeviceErrCode = stationOrderTakenErrRequestVo.errorCode;
                                    dbContext.SaveChanges();
                                    newWebOrderRefund.WebOrderRefundId = Guid.NewGuid();
                                    newWebOrderRefund.WebOrderId = Guid.NewGuid();
                                    newWebOrderRefund.ExternalTradeNo = Guid.NewGuid().ToString();
                                    newWebOrderRefund.TradeNo = existOrder.TradeNo;
                                    newWebOrderRefund.RefundTradeNo = existOrder.TransactionId;//待完善
                                    newWebOrderRefund.RefundReason = "设备申请退款";
                                    newWebOrderRefund.PaymentVendor = existOrder.PaymentVendor;
                                    newWebOrderRefund.RefundFee = (decimal)existOrder.ActualFee;
                                    newWebOrderRefund.TotalFee = existOrder.Amount;
                                    newWebOrderRefund.BankType = existOrder.BankType;

                                    newWebOrderRefund.IsRequestSuccess = true;
                                    newWebOrderRefund.IsRespondSuccess = true;
                                    newWebOrderRefund.RespondTime = DateTime.Now;
                                    if (refundVO.is_success)  //20170607  gaoke 增加退款结果判断逻辑
                                    {
                                        existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.RefundSuccess);
                                        newWebOrderRefund.OrderStatus = "8";
                                    }
                                    else
                                    {
                                        existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.RefundProcessing);
                                        newWebOrderRefund.OrderStatus = "7";
                                    }
                                    webOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.TradeNo.Equals(existOrder.TradeNo)));
                                    if (webOrderRefund == null)  //避免对同一订单号进行多次插入退款表操作
                                    {
                                        dbContext.WebOrderRefunds.AddObject(newWebOrderRefund);
                                        dbContext.SaveChanges();
                                    }
                                    else
                                    {
                                        _log.Info("订单 " + existOrder.TradeNo + " 已存在在退款表中！");
                                    }
                                }
                            }
                            result.RespondCode = DeviceCommRespondCode.RC0000;  //gaoke 20161117  确保S1_005的响应码为0000
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.RespondCode = DeviceCommRespondCode.RC0000;//原为9999

                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            return result;
        }

        ///// <summary>
        ///// ApplePay交易记录录入
        ///// </summary>
        ///// <param name="applePayDealInsertRequestVo"></param>
        ///// <returns></returns>
        //public ApplePayDealInsertRespondVo ApplePayDealInsert(ApplePayDealInsertRequestVo applePayDealInsertRequestVo)
        //{
        //    ApplePayDealInsertRespondVo result = new ApplePayDealInsertRespondVo();
        //    // 临时处理
        //    result.RespondCode = DeviceCommRespondCode.RC9999;
        //    //result.respCodeMemo = "9999";

        //    result.conversationId = String.Empty;

        //    string subject = String.Empty;

        //    try
        //    {
        //        ApplePayDeal newApplePayDeal = new ApplePayDeal();
        //        if (null != applePayDealInsertRequestVo)
        //        {
        //            newApplePayDeal.ApplePayDealId = Guid.NewGuid();
        //            DateTime? dtReqSysDate = applePayDealInsertRequestVo.ReqSysDate;
        //            if (null == dtReqSysDate)
        //            {
        //                newApplePayDeal.ReqSysDate = DateTime.Now;
        //            }
        //            else
        //            {
        //                newApplePayDeal.ReqSysDate = dtReqSysDate.Value;
        //            }
        //            newApplePayDeal.OperationCode = applePayDealInsertRequestVo.OperationCode;
        //            newApplePayDeal.CityCode = applePayDealInsertRequestVo.CityCode;
        //            newApplePayDeal.DeviceId = applePayDealInsertRequestVo.DeviceId;
        //            newApplePayDeal.ChannelType = applePayDealInsertRequestVo.ChannelType;
        //            newApplePayDeal.ConversationId = applePayDealInsertRequestVo.ConversationId;
        //            newApplePayDeal.PAN = applePayDealInsertRequestVo.PAN;
        //            newApplePayDeal.TransactionAmount = applePayDealInsertRequestVo.TransactionAmount;
        //            newApplePayDeal.TransactionCurrencyCode = applePayDealInsertRequestVo.TransactionCurrencyCode;
        //            newApplePayDeal.TransactionTimeRaw = String.Empty;
        //            if (null != applePayDealInsertRequestVo.TransactionTimeStringMMddhhmmss)
        //            {
        //                newApplePayDeal.TransactionTimeRaw = applePayDealInsertRequestVo.TransactionTimeStringMMddhhmmss;

        //                //string型时间转成DateTime型
        //                DateTime? transactionTime = TimeHelper.GetDateTimeYyyyMMddHHmmss(DateTime.Now.Year.ToString("YYYY") + applePayDealInsertRequestVo.TransactionTimeStringMMddhhmmss);
        //                if (null != transactionTime)
        //                {
        //                    newApplePayDeal.TransactionTime = transactionTime.Value;
        //                }
        //            }
        //            newApplePayDeal.AuthorizationResponseIdentificationCode = applePayDealInsertRequestVo.AuthorizationResponseIdentificationCode;
        //            newApplePayDeal.RetrievalReferNumber = applePayDealInsertRequestVo.RetrievalReferNumber;
        //            newApplePayDeal.TerminalNo = applePayDealInsertRequestVo.TerminalNo;
        //            newApplePayDeal.MerchantCodeId = applePayDealInsertRequestVo.MerchantCodeId;
        //            newApplePayDeal.ApplicationCryptogram = applePayDealInsertRequestVo.ApplicationCryptogram;
        //            newApplePayDeal.InputmodeCode = applePayDealInsertRequestVo.InputmodeCode;
        //            newApplePayDeal.CardserialNumber = applePayDealInsertRequestVo.CardserialNumber;
        //            newApplePayDeal.TerminalReadability = applePayDealInsertRequestVo.TerminalReadability;
        //            newApplePayDeal.CardconditionCode = applePayDealInsertRequestVo.CardconditionCode;
        //            newApplePayDeal.TerminalPerformance = applePayDealInsertRequestVo.TerminalPerformance;
        //            newApplePayDeal.TerminalVerificationResults = applePayDealInsertRequestVo.TerminalVerificationResults;
        //            newApplePayDeal.UnpredictableNumber = applePayDealInsertRequestVo.UnpredictableNumber;
        //            newApplePayDeal.InterfaceEquipmentSerialNumber = applePayDealInsertRequestVo.InterfaceEquipmentSerialNumber;
        //            newApplePayDeal.IssuerapplicationData = applePayDealInsertRequestVo.IssuerapplicationData;
        //            newApplePayDeal.ApplicationTradeCounter = applePayDealInsertRequestVo.ApplicationTradeCounter;
        //            newApplePayDeal.ApplicationInterchangeProfile = applePayDealInsertRequestVo.ApplicationInterchangeProfile;
        //            newApplePayDeal.TransactionDate = applePayDealInsertRequestVo.TransactionDate;
        //            newApplePayDeal.TerminalCountryCode = applePayDealInsertRequestVo.TradingCurrencyCode;
        //            newApplePayDeal.ResponseCode = applePayDealInsertRequestVo.ResponseCode;
        //            newApplePayDeal.TransactionType = applePayDealInsertRequestVo.TransactionType;
        //            newApplePayDeal.AuthorizeAmount = applePayDealInsertRequestVo.AuthorizeAmount;
        //            newApplePayDeal.TradingCurrencyCode = applePayDealInsertRequestVo.TradingCurrencyCode;
        //            newApplePayDeal.CipherCheckResult = applePayDealInsertRequestVo.CipherCheckResult;
        //            newApplePayDeal.CardValidPeriod = applePayDealInsertRequestVo.CardValidPeriod;
        //            newApplePayDeal.CryptogramInformationData = applePayDealInsertRequestVo.CryptogramInformationData;
        //            newApplePayDeal.OtherAmount = applePayDealInsertRequestVo.OtherAmount;
        //            newApplePayDeal.CardholderVerificationMethod = applePayDealInsertRequestVo.CardholderVerificationMethod;
        //            newApplePayDeal.TerminalType = applePayDealInsertRequestVo.TerminalType;
        //            newApplePayDeal.ProfessFilename = applePayDealInsertRequestVo.ProfessFilename;
        //            newApplePayDeal.ApplicationVersion = applePayDealInsertRequestVo.ApplicationVersion;
        //            newApplePayDeal.TradingSequenceCounter = applePayDealInsertRequestVo.TradingSequenceCounter;
        //            newApplePayDeal.EcissueAuthorizationCode = applePayDealInsertRequestVo.EcissueAuthorizationCode;
        //            newApplePayDeal.ProductIdentificationInformation = applePayDealInsertRequestVo.ProductIdentificationInformation;
        //            newApplePayDeal.CardType = applePayDealInsertRequestVo.CardType;
        //            newApplePayDeal.PaymentCode = applePayDealInsertRequestVo.PaymentCode;
        //            using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
        //            {
        //                dbContext.ApplePayDeals.AddObject(newApplePayDeal);

        //                if (0 < dbContext.SaveChanges())
        //                {
        //                    result.RespondCode = DeviceCommRespondCode.RC0000;
        //                    result.conversationId = newApplePayDeal.ConversationId;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error(ex.Message);
        //        if (null != ex.InnerException)
        //        {
        //            _log.Error(ex.InnerException.Message);
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 通用订单查询
        /// </summary>
        /// <param name="isQueryUsedTime">查询使用时间标识，启用时查询已使用订单的使用时间，不启用查询所有订单的购买时间</param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="paymentVendor">必须为“1001”或“1002”，空字符串或null包含所有</param>
        /// <returns></returns>
        public List<CommonOrderVo> CommonOrderQuery(bool isQueryUsedTime, DateTime fromTime, DateTime toTime, string paymentVendor)
        {
            List<CommonOrderVo> commonOrderList = new List<CommonOrderVo>();

            try
            {
                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                {
                    dbContext.CommandTimeout = 300;


                    var result = dbContext.StationOrders.AsQueryable();

                    if (isQueryUsedTime)
                    {
                        result = result.Where(o => ((null != o.TicketTakeTime)
                            && (0 <= o.TicketTakeTime.Value.CompareTo(fromTime))
                            && (0 >= o.TicketTakeTime.Value.CompareTo(toTime))));
                    }
                    else
                    {
                        result = result.Where(o => ((0 <= o.BuyTime.CompareTo(fromTime))
                                                   && (0 >= o.BuyTime.CompareTo(toTime))));
                    }

                    if (!String.IsNullOrEmpty(paymentVendor))
                    {
                        result = result.Where(o => o.PaymentVendor.Equals(paymentVendor));
                    }

                    if ((null != result)
                        && (0 < result.Count()))
                    {
                        foreach (StationOrder eachStationOrders in result)
                        {
                            commonOrderList.Add(new CommonOrderVo(eachStationOrders));
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
