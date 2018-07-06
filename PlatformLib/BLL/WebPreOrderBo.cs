using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;

using PlatformLib.Vo;
using PlatformLib.DB;
using PlatformLib.Util;
using ResultNofity;

namespace PlatformLib.BLL
{
    /// <summary>
    /// 网络预购票逻辑层
    /// </summary>
    public class WebPreOrderBo
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获得商家订单号
        /// </summary>
        /// <param name="webTradeNoRequestVo"></param>
        /// <returns></returns>
        public WebTradeNoRespondVo GetTradeNo(WebTradeNoRequestVo webTradeNoRequestVo)
        {
            WebTradeNoRespondVo result = new WebTradeNoRespondVo();

            string newTradeNo = String.Empty;

            try
            {
                if (null != webTradeNoRequestVo)
                {
                    Guid newWebOrderId = Guid.NewGuid();

                    newTradeNo = String.Format("{0}{1}", Constants.OrderNoQRCWeb, TradeNoHelper.Instance.GetTradeNo());
                    string inputExternalTradeNo = webTradeNoRequestVo.ExternalTradeNo;

                    if (!String.IsNullOrEmpty(inputExternalTradeNo))
                    {
                        WebOrder newWebOrder = new WebOrder();
                        newWebOrder.WebOrderId = newWebOrderId;
                        newWebOrder.TradeNo = newTradeNo;
                        newWebOrder.ExternalTradeNo = inputExternalTradeNo;
                        newWebOrder.PrepayId = String.Empty;
                        newWebOrder.TransactionId = String.Empty;
                        newWebOrder.PayEndTime = null;
                        newWebOrder.ActualFee = 0;
                        newWebOrder.BankType = String.Empty;
                        newWebOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebTradeNoRequest);
                        newWebOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.WaitBuyerPay);

                        // 后续处理 
                        newWebOrder.StepStatus = String.Empty;
                        newWebOrder.IsValid = true;

                        newWebOrder.BuyTime = webTradeNoRequestVo.BuyTime;
                        newWebOrder.OperationCode = webTradeNoRequestVo.OperationCode;
                        newWebOrder.CityCode = webTradeNoRequestVo.CityCode;
                        // DeviceId用于取票
                        newWebOrder.DeviceId = String.Empty;
                        newWebOrder.ChannelType = webTradeNoRequestVo.ChannelType;
                        newWebOrder.PaymentVendor = webTradeNoRequestVo.PaymentVendor;
                        newWebOrder.OriAFCStationCode = webTradeNoRequestVo.OriAFCStationCode;
                        newWebOrder.DesAFCStationCode = webTradeNoRequestVo.DesAFCStationCode;
                        newWebOrder.TicketPrice = webTradeNoRequestVo.TicketPrice;
                        newWebOrder.TicketNum = webTradeNoRequestVo.TicketNum;
                        newWebOrder.Discount = webTradeNoRequestVo.Discount;
                        newWebOrder.Amount = webTradeNoRequestVo.Amount;
                        newWebOrder.TicketTarget = webTradeNoRequestVo.TicketTarget.ToString();
                        // 临时处理只出闸机票
                        //newWebOrder.TicketTarget = newWebOrder.TicketTarget = "APMGATE";
                        newWebOrder.UserOpenId = webTradeNoRequestVo.UserOpenId;
                        newWebOrder.UserAccount = webTradeNoRequestVo.UserAccount;

                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            dbContext.WebOrders.AddObject(newWebOrder);
                            if (0 < dbContext.SaveChanges())
                            {
                                result.WebOrderId = newWebOrderId;
                                result.TradeNo = newTradeNo;
                                result.IsVaild = true;
                                result.StepStatus = String.Empty;

                                result.IsSuccess = true;
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
                _log.Debug(newTradeNo);
            }

            return result;
        }


        /// <summary>
        /// 预支付结果记录
        /// </summary>
        /// <param name="webPrePayRequestVo">WebOrderId和TradeNo为查询依据，必须其中一个为非空</param>
        /// <returns></returns>
        public WebPrePayRespondVo PrePayRecord(WebPrePayRequestVo webPrePayRequestVo)
        {
            WebPrePayRespondVo result = new WebPrePayRespondVo();

            try
            {
                if (null != webPrePayRequestVo)
                {
                    Guid? existWebOrderId = webPrePayRequestVo.WebOrderId;
                    string existTradeNo = webPrePayRequestVo.TradeNo;
                    string prepayId = webPrePayRequestVo.PrepayId;

                    if ((null != existWebOrderId)
                        || (!String.IsNullOrEmpty(existTradeNo)))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrder existWebOrder = null;
                            if (null != existWebOrderId)
                            {
                                existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => wo.WebOrderId.Equals(existWebOrderId.Value));
                            }
                            else
                            {
                                existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => wo.TradeNo.Equals(existTradeNo));
                            }

                            if (null != existWebOrder)
                            {
                                result.WebOrderId = existWebOrder.WebOrderId;
                                result.TradeNo = existWebOrder.TradeNo;
                                result.ExternalTradeNo = existWebOrder.ExternalTradeNo;
                                result.IsTradeNoValid = true;

                                existWebOrder.PrepayId = prepayId;
                                existWebOrder.Step = ((int)WebOrderStep.WebPrePayRespond).ToString();
                                existWebOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.WaitBuyerPay);

                                if (0 < dbContext.SaveChanges())
                                {
                                    result.StepStatus = String.Empty;

                                    result.IsSuccess = true;
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
        /// 支付同步结果记录，成功才记录生成二维码
        /// </summary>
        /// <param name="webPaySyncResultRequestVo"></param>
        /// <returns></returns>
        [Obsolete]
        public WebPaySyncResultRespondVo PaySyncResultRecord(WebPaySyncResultRequestVo webPaySyncResultRequestVo)
        {
            WebPaySyncResultRespondVo result = new WebPaySyncResultRespondVo();

            throw new Exception("not allow use");

            try
            {
                result.StepStatus = ((int)WebOrderStep.WebPaySyncResultRequest).ToString();

                if (null != webPaySyncResultRequestVo)
                {
                    string inputPrepayId = webPaySyncResultRequestVo.PrepayId;

                    if (!String.IsNullOrEmpty(inputPrepayId))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrder existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => wo.PrepayId.Equals(inputPrepayId));


                            if (null != existWebOrder)
                            {
                                result.WebOrderId = existWebOrder.WebOrderId;
                                result.TradeNo = existWebOrder.TradeNo;
                                result.OriAFCStationCode = existWebOrder.OriAFCStationCode;
                                result.DesAFCStationCode = existWebOrder.DesAFCStationCode;
                                result.TicketPrice = existWebOrder.TicketPrice;
                                result.TicketNum = existWebOrder.TicketNum;
                                result.Discount = existWebOrder.Discount;


                                #region 时间戳处理
                                // 自1970年以来的秒数 
                                string inputTimeStamp = webPaySyncResultRequestVo.TimeStamp;
                                long timeStamp = 0;
                                if (long.TryParse(inputTimeStamp, out timeStamp))
                                {
                                    try
                                    {
                                        DateTime dt1970 = new DateTime(1970, 0, 0);

                                        DateTime dtPayEndTime = dt1970.AddSeconds(timeStamp);

                                        existWebOrder.PayEndTime = dtPayEndTime;
                                    }
                                    catch (Exception)
                                    { }
                                }
                                #endregion 时间戳处理



                                VoucherList freeVoucher = dbContext.VoucherLists.FirstOrDefault(v => (v.IsValid.Equals(true)
                                    && v.IsUsed.Equals(false)
                                    && v.IsLocked.Equals(false)));
                                if (null != freeVoucher)
                                {
                                    freeVoucher.IsLocked = true;
                                    if (0 < dbContext.SaveChanges())
                                    {
                                        freeVoucher.IsUsed = true;
                                        freeVoucher.IsLocked = false;

                                        #region 建立WebOrderVoucher
                                        WebOrderVoucher newWebOrderVoucher = new WebOrderVoucher();
                                        newWebOrderVoucher.WebOrderVoucherId = Guid.NewGuid();
                                        newWebOrderVoucher.WebOrderId = existWebOrder.WebOrderId;
                                        newWebOrderVoucher.TradeNo = existWebOrder.TradeNo;
                                        newWebOrderVoucher.VoucherId = freeVoucher.VoucherId;
                                        newWebOrderVoucher.VoucherCode = freeVoucher.VoucherCode;
                                        newWebOrderVoucher.OrderVoucherCode = VoucherHelper.VoucherCodeCombine(existWebOrder, freeVoucher.VoucherCode);
                                        newWebOrderVoucher.IsUsed = false;
                                        newWebOrderVoucher.IsValid = true;
                                        newWebOrderVoucher.UsedTime = null;
                                        newWebOrderVoucher.CreateTime = DateTime.Now;
                                        dbContext.WebOrderVouchers.AddObject(newWebOrderVoucher);
                                        #endregion 建立WebOrderVoucher

                                        existWebOrder.Step = ((int)WebOrderStep.WebPaySyncResultRespond).ToString();
                                        existWebOrder.StepStatus = String.Empty;

                                        if (0 < dbContext.SaveChanges())
                                        {
                                            // 临时处理
                                            result.Voucher = newWebOrderVoucher.OrderVoucherCode;

                                            result.StepStatus = ((int)WebOrderStep.WebPaySyncResultRespond).ToString();

                                            result.IsSuccess = true;
                                        }
                                    }
                                }
                            }

                            dbContext.SaveChanges();
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
        /// 支付异步结果记录，只记录状态，如是成功结果重新返回二维码
        /// </summary>
        /// <param name="webPayResultRequestVo">TradeNo和ExternalTradeNo为查询依据，其中一个必须为非空</param>
        /// <returns></returns>
        public WebPayResultRespondVo PayResultRecord(WebPayResultRequestVo webPayResultRequestVo)
        {
            WebPayResultRespondVo result = new WebPayResultRespondVo();

            try
            {
                if (null != webPayResultRequestVo)
                {
                    string inputTradeNo = webPayResultRequestVo.TradeNo;
                    string inputExternalTradeNo = webPayResultRequestVo.ExternalTradeNo;


                    if ((!String.IsNullOrEmpty(inputTradeNo))
                        || (!String.IsNullOrEmpty(inputExternalTradeNo)))
                    {
                        string transactionId = webPayResultRequestVo.TransactionId;

                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrder existWebOrder = null;
                            if (!String.IsNullOrEmpty(inputTradeNo))
                            {
                                existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => wo.TradeNo.Equals(inputTradeNo));
                            }
                            else
                            {
                                existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => wo.ExternalTradeNo.Equals(inputExternalTradeNo));
                            }

                            if (null != existWebOrder)
                            {
                                result.WebOrderId = existWebOrder.WebOrderId;
                                result.TradeNo = existWebOrder.TradeNo;
                                result.ExternalTradeNo = existWebOrder.ExternalTradeNo;
                                result.IsTradeNoValid = true;
                                result.OriAFCStationCode = existWebOrder.OriAFCStationCode;
                                result.DesAFCStationCode = existWebOrder.DesAFCStationCode;
                                result.TicketPrice = existWebOrder.TicketPrice;
                                result.TicketNum = existWebOrder.TicketNum;
                                result.Discount = existWebOrder.Discount;
                                result.PayEndTime = webPayResultRequestVo.PayEndTime;
                                // 设备类型
                                result.TicketTarget = TicketTargetType.NONE;
                                if (!String.IsNullOrEmpty(existWebOrder.TicketTarget))
                                {
                                    try
                                    {
                                        result.TicketTarget = EnumHelper.GetTicketTargetType(existWebOrder.TicketTarget);
                                    }
                                    catch (Exception)
                                    { }
                                }

                                existWebOrder.TransactionId = transactionId;
                                existWebOrder.PayEndTimeRaw = webPayResultRequestVo.PayEndTime;
                                existWebOrder.PayEndTime = TimeHelper.GetDateTimeYyyyMMddHHmmss(webPayResultRequestVo.PayEndTime);
                                existWebOrder.ActualFee = webPayResultRequestVo.ActualFee;
                                existWebOrder.BankType = webPayResultRequestVo.BankType;
                                existWebOrder.PayErrCodeDes = webPayResultRequestVo.ErrCodeDes;
                                // 临时处理，这里不再修改UserOpenId，在GetTradeNo已有赋值
                                //existWebOrder.UserOpenId = webPayResultRequestVo.UserOpenId;
                                existWebOrder.UserAccount = webPayResultRequestVo.UserAccount;
                                if (webPayResultRequestVo.IsSuccess)
                                {
                                    existWebOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TradeSuccess);
                                }
                                else
                                {
                                    existWebOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TradeFaild);
                                }
                                existWebOrder.Step = ((int)WebOrderStep.WebPayAsyncResultRequest).ToString();


                                VoucherList freeVoucher = dbContext.VoucherLists.FirstOrDefault(v => (v.IsValid.Equals(true)
                                    && v.IsUsed.Equals(false)
                                    && v.IsLocked.Equals(false)));
                                if (null != freeVoucher)
                                {
                                    freeVoucher.IsLocked = true;
                                    if (0 < dbContext.SaveChanges())
                                    {
                                        freeVoucher.IsUsed = true;
                                        freeVoucher.IsLocked = false;

                                        #region 建立WebOrderVoucher
                                        WebOrderVoucher newWebOrderVoucher = new WebOrderVoucher();
                                        newWebOrderVoucher.WebOrderVoucherId = Guid.NewGuid();
                                        newWebOrderVoucher.WebOrderId = existWebOrder.WebOrderId;
                                        newWebOrderVoucher.TradeNo = existWebOrder.TradeNo;
                                        newWebOrderVoucher.VoucherId = freeVoucher.VoucherId;
                                        newWebOrderVoucher.VoucherCode = freeVoucher.VoucherCode;
                                        // 获得完整二维码
                                        newWebOrderVoucher.OrderVoucherCode = VoucherHelper.VoucherCodeCombine(existWebOrder, freeVoucher.VoucherCode);
                                        newWebOrderVoucher.IsUsed = false;
                                        newWebOrderVoucher.IsValid = true;
                                        newWebOrderVoucher.UsedTime = null;
                                        newWebOrderVoucher.CreateTime = DateTime.Now;
                                        dbContext.WebOrderVouchers.AddObject(newWebOrderVoucher);
                                        #endregion 建立WebOrderVoucher

                                        existWebOrder.Step = ((int)WebOrderStep.WebPayAsyncResultRespond).ToString();
                                        existWebOrder.StepStatus = String.Empty;

                                        if (0 < dbContext.SaveChanges())
                                        {
                                            // 临时处理
                                            result.Voucher = newWebOrderVoucher.OrderVoucherCode;

                                            result.IsSuccess = true;
                                        }
                                    }
                                }
                            }

                            dbContext.SaveChanges();
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
        /// 生成免费车票订单
        /// </summary>
        /// <param name="webTradeNoRequestVo">PaymentVendor必须为"1000"，TicketPrice必须为200，TicketNum必须为1，Discount必须为0，Amount必须为200，TicketTarget必须为APMGATE</param>
        /// <returns></returns>
        public WebPayResultRespondVo FreeAPMTicketFor20160601(WebTradeNoRequestVo webTradeNoRequestVo)
        {
            WebPayResultRespondVo result = new WebPayResultRespondVo();
            result.IsSuccess = false;
            result.IsTradeNoValid = true;
            result.StepStatus = "生成订单失败";

            try
            {
                if ((null != webTradeNoRequestVo)
                    // 强制活动条件
                    && (webTradeNoRequestVo.PaymentVendor.Equals("1000"))
                    && (200 == webTradeNoRequestVo.TicketPrice)
                    && (1 == webTradeNoRequestVo.TicketNum)
                    && (0 == webTradeNoRequestVo.Discount)
                    && (200 == webTradeNoRequestVo.Amount)
                    && (TicketTargetType.APMGATE == webTradeNoRequestVo.TicketTarget)
                    )
                {
                    string inputExternalTradeNo = webTradeNoRequestVo.ExternalTradeNo;

                    result.ExternalTradeNo = inputExternalTradeNo;
                    result.OriAFCStationCode = webTradeNoRequestVo.OriAFCStationCode;
                    result.DesAFCStationCode = webTradeNoRequestVo.DesAFCStationCode;
                    result.TicketPrice = webTradeNoRequestVo.TicketPrice;
                    result.TicketNum = webTradeNoRequestVo.TicketNum;
                    result.Discount = webTradeNoRequestVo.Discount;
                    result.TicketTarget = webTradeNoRequestVo.TicketTarget;

                    using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                    {
                        Guid newWebOrderId = Guid.NewGuid();


                        string newTradeNo = String.Format("{0}{1}", Constants.OrderNoPrefixWeb, TradeNoHelper.Instance.GetTradeNo());

                        if (String.IsNullOrEmpty(inputExternalTradeNo))
                        {
                            result.StepStatus = "外部订单号为空";
                        }
                        else
                        {
                            WebOrder newWebOrder = new WebOrder();
                            newWebOrder.WebOrderId = newWebOrderId;
                            newWebOrder.TradeNo = newTradeNo;
                            newWebOrder.ExternalTradeNo = inputExternalTradeNo;
                            newWebOrder.BuyTime = webTradeNoRequestVo.BuyTime;
                            newWebOrder.PrepayId = String.Empty;

                            newWebOrder.OperationCode = webTradeNoRequestVo.OperationCode;
                            newWebOrder.CityCode = webTradeNoRequestVo.CityCode;
                            // DeviceId用于取票
                            newWebOrder.DeviceId = String.Empty;
                            newWebOrder.ChannelType = webTradeNoRequestVo.ChannelType;
                            newWebOrder.PaymentVendor = webTradeNoRequestVo.PaymentVendor;
                            newWebOrder.OriAFCStationCode = webTradeNoRequestVo.OriAFCStationCode;
                            newWebOrder.DesAFCStationCode = webTradeNoRequestVo.DesAFCStationCode;
                            newWebOrder.TicketPrice = webTradeNoRequestVo.TicketPrice;
                            newWebOrder.TicketNum = webTradeNoRequestVo.TicketNum;
                            newWebOrder.Discount = 0;
                            newWebOrder.Amount = webTradeNoRequestVo.Amount;
                            newWebOrder.TicketTarget = webTradeNoRequestVo.TicketTarget.ToString();
                            newWebOrder.TransactionId = String.Empty;
                            newWebOrder.PayEndTimeRaw = null;
                            newWebOrder.PayEndTime = null;
                            newWebOrder.ActualFee = 0;
                            newWebOrder.BankType = String.Empty;
                            newWebOrder.UserOpenId = webTradeNoRequestVo.UserOpenId;
                            newWebOrder.UserAccount = webTradeNoRequestVo.UserAccount;
                            newWebOrder.PayErrCodeDes = null;

                            newWebOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TradeSuccess);
                            newWebOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebPayAsyncResultRequest);
                            newWebOrder.StepStatus = String.Empty;
                            // 2016年5月28日0点至2016年5月29日24点期间车票有效
                            if ((0 < DateTime.Now.CompareTo(new DateTime(2016, 5, 28)))
                                && (0 > DateTime.Now.CompareTo(new DateTime(2016, 5, 30))))
                            {
                                newWebOrder.IsValid = true;
                            }
                            else
                            {
                                newWebOrder.IsValid = false;
                            }
                            dbContext.WebOrders.AddObject(newWebOrder);

                            VoucherList freeVoucher = dbContext.VoucherLists.FirstOrDefault(v => (v.IsValid.Equals(true)
                                        && v.IsUsed.Equals(false)
                                        && v.IsLocked.Equals(false)));
                            if (null != freeVoucher)
                            {
                                freeVoucher.IsLocked = true;
                                if (0 < dbContext.SaveChanges())
                                {
                                    result.StepStatus = "订单获得二维码失败";

                                    freeVoucher.IsUsed = true;
                                    freeVoucher.IsLocked = false;

                                    #region 建立WebOrderVoucher
                                    WebOrderVoucher newWebOrderVoucher = new WebOrderVoucher();
                                    newWebOrderVoucher.WebOrderVoucherId = Guid.NewGuid();
                                    newWebOrderVoucher.WebOrderId = newWebOrder.WebOrderId;
                                    newWebOrderVoucher.TradeNo = newWebOrder.TradeNo;
                                    newWebOrderVoucher.VoucherId = freeVoucher.VoucherId;
                                    newWebOrderVoucher.VoucherCode = freeVoucher.VoucherCode;
                                    // 获得完整二维码
                                    newWebOrderVoucher.OrderVoucherCode = VoucherHelper.VoucherCodeCombine(newWebOrder, freeVoucher.VoucherCode);
                                    newWebOrderVoucher.IsUsed = false;
                                    newWebOrderVoucher.IsValid = true;
                                    newWebOrderVoucher.UsedTime = null;
                                    newWebOrderVoucher.CreateTime = DateTime.Now;
                                    dbContext.WebOrderVouchers.AddObject(newWebOrderVoucher);
                                    #endregion 建立WebOrderVoucher

                                    newWebOrder.Step = ((int)WebOrderStep.WebPayAsyncResultRespond).ToString();

                                    if (0 < dbContext.SaveChanges())
                                    {
                                        result.WebOrderId = newWebOrderId;
                                        result.TradeNo = newTradeNo;

                                        result.PayEndTime = TimeHelper.GetTimeStringYyyyMMddHHmmss(DateTime.Now);

                                        result.Voucher = newWebOrderVoucher.OrderVoucherCode;

                                        result.StepStatus = String.Empty;

                                        result.IsSuccess = true;
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
        /// 信息服务部网络支付结果记录
        /// </summary>
        /// <param name="itPayResultRequestVo">信息服务部网络支付结果请求，TradeNo为查询依据必须为非空</param>
        /// <returns></returns>
        [Obsolete]
        public ITPayResultRespondVo ITPayResultRecord(ITPayResultRequestVo itPayResultRequestVo)
        {
            ITPayResultRespondVo result = new ITPayResultRespondVo();

            string strErrStatus = String.Empty;

            try
            {
                string inputTradeNo = itPayResultRequestVo.TradeNo;
                result.TradeNo = inputTradeNo;

                List<string> ticketTargetTypeNameList = EnumHelper.GetTicketTargetTypeNameList();
                string inputTarget = itPayResultRequestVo.Target;

                if (null != itPayResultRequestVo)
                {
                    if (String.IsNullOrEmpty(inputTradeNo))
                    {
                        strErrStatus += "trade_no not exist ";
                    }
                    else if (30 < inputTradeNo.Length)
                    {
                        strErrStatus += "trade_no err ";
                    }
                    else if (String.IsNullOrEmpty(inputTarget))
                    {
                        strErrStatus += "target not exist ";
                    }
                    else if (!ticketTargetTypeNameList.Contains(inputTarget))
                    {
                        strErrStatus += "target err ";
                    }
                    else
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            // 商户订单重复检查
                            int existWebOrderCount = dbContext.WebOrders.Where(wo => wo.TradeNo.Equals(inputTradeNo)).Count();
                            if (0 < existWebOrderCount)
                            {
                                strErrStatus += "trade_no duplicate ";
                            }
                            else
                            {
                                WebOrder newWebOrder = new WebOrder();
                                newWebOrder.WebOrderId = Guid.NewGuid();
                                newWebOrder.TradeNo = inputTradeNo;
                                // 先预赋值
                                newWebOrder.BuyTime = new DateTime();
                                newWebOrder.PrepayId = String.Empty;
                                newWebOrder.CityCode = "020";
                                // DeviceId用于取票
                                newWebOrder.DeviceId = String.Empty;
                                newWebOrder.ChannelType = "GZMTR";
                                newWebOrder.Discount = 1;
                                newWebOrder.TicketTarget = inputTarget;
                                newWebOrder.Step = ((int)WebOrderStep.WebPaySyncResultRequest).ToString();

                                newWebOrder.IsValid = true;

                                #region 校验ITPayResultRequestVo字段，填充WebOrder
                                string transactionId = itPayResultRequestVo.TransactionId;
                                if (String.IsNullOrEmpty(transactionId))
                                {
                                    strErrStatus += "transaction_id not exist ";
                                    transactionId = String.Empty;

                                }
                                else if (30 < transactionId.Length)
                                {
                                    strErrStatus += "transaction_id err ";
                                    transactionId = transactionId.Substring(0, 30);
                                }
                                newWebOrder.TransactionId = transactionId;

                                string userOpenId = itPayResultRequestVo.UserOpenId;
                                if (String.IsNullOrEmpty(userOpenId))
                                {
                                    strErrStatus += "user_id not exist ";
                                    userOpenId = String.Empty;
                                }
                                else if (128 < userOpenId.Length)
                                {
                                    strErrStatus += "user_id err ";
                                    userOpenId = userOpenId.Substring(0, 128);
                                }
                                newWebOrder.UserOpenId = userOpenId;


                                string beginStation = itPayResultRequestVo.OriAFCStationCode;
                                if (String.IsNullOrEmpty(beginStation))
                                {
                                    strErrStatus += "begin_station not exist ";
                                    beginStation = String.Empty;
                                }
                                else if (4 < beginStation.Length)
                                {
                                    strErrStatus += "begin_station err ";
                                    beginStation = beginStation.Substring(0, 4);
                                }
                                newWebOrder.OriAFCStationCode = beginStation;

                                string endStation = itPayResultRequestVo.DesAFCStationCode;
                                if (String.IsNullOrEmpty(endStation))
                                {
                                    strErrStatus += "end_station not exist ";
                                    endStation = String.Empty;
                                }
                                else if (4 < endStation.Length)
                                {
                                    strErrStatus += "end_station err ";
                                    endStation = endStation.Substring(0, 4);
                                }
                                newWebOrder.DesAFCStationCode = endStation;

                                string inputTicket_price = itPayResultRequestVo.TicketPrice;
                                decimal ticketPrice = 0;
                                if (String.IsNullOrEmpty(inputTicket_price))
                                {
                                    strErrStatus += "ticket_price not exist ";
                                }
                                else if (4 < inputTicket_price.Length)
                                {
                                    strErrStatus += "ticket_price err ";
                                }
                                else
                                {
                                    if (decimal.TryParse(inputTicket_price, out ticketPrice))
                                    {
                                        if (ticketPrice < 0)
                                        {
                                            strErrStatus += "ticket_price err ";
                                        }
                                    }
                                    else
                                    {
                                        strErrStatus += "ticket_price not number";
                                    }
                                }
                                newWebOrder.TicketPrice = ticketPrice;

                                string inputTicket_num = itPayResultRequestVo.TicketNum;
                                int ticketNum = 0;
                                if (String.IsNullOrEmpty(inputTicket_num))
                                {
                                    strErrStatus += "ticket_num not exist ";

                                }
                                else if (3 < inputTicket_num.Length)
                                {
                                    strErrStatus += "ticket_num err ";
                                }
                                else
                                {
                                    if (int.TryParse(inputTicket_num, out ticketNum))
                                    {
                                        if (ticketNum < 1)
                                        {
                                            strErrStatus += "ticket_num err ";
                                        }
                                    }
                                    else
                                    {
                                        strErrStatus += "ticket_num not number";
                                    }
                                }
                                newWebOrder.TicketNum = ticketNum;

                                string inputTotal_fee = itPayResultRequestVo.ActualFee;
                                decimal totalFee = 0;
                                if (String.IsNullOrEmpty(inputTotal_fee))
                                {
                                    strErrStatus += "total_fee not exist ";
                                }
                                else if (7 < inputTotal_fee.Length)
                                {
                                    strErrStatus += "total_fee err ";
                                }
                                else
                                {
                                    if (decimal.TryParse(inputTotal_fee, out totalFee))
                                    {
                                        if (totalFee < 0)
                                        {
                                            strErrStatus += "total_fee err ";
                                        }
                                    }
                                    else
                                    {
                                        strErrStatus += "total_fee not number";
                                    }
                                }
                                newWebOrder.Amount = totalFee;
                                newWebOrder.ActualFee = totalFee;

                                string inputPayEndTime = itPayResultRequestVo.PayEndTime;
                                DateTime? dtPayEndTime = null;
                                if (String.IsNullOrEmpty(inputPayEndTime))
                                {
                                    strErrStatus += "pay_time not exist ";
                                }
                                else if (14 < inputPayEndTime.Length)
                                {
                                    strErrStatus += "pay_time err ";
                                }
                                else
                                {
                                    dtPayEndTime = TimeHelper.GetDateTimeYyyyMMddHHmmss(inputPayEndTime);

                                    if (null == dtPayEndTime)
                                    {
                                        strErrStatus += "pay_time err ";
                                    }
                                }
                                newWebOrder.PayEndTimeRaw = inputPayEndTime;
                                newWebOrder.PayEndTime = dtPayEndTime;
                                if (null != dtPayEndTime)
                                {
                                    newWebOrder.BuyTime = dtPayEndTime.Value;
                                }

                                string payOperator = itPayResultRequestVo.PayOperator;
                                if (String.IsNullOrEmpty(payOperator))
                                {
                                    strErrStatus += "pay_operator not exist ";
                                    payOperator = String.Empty;
                                }
                                else if (10 < payOperator.Length)
                                {
                                    strErrStatus += "pay_time err ";
                                    payOperator = payOperator.Substring(0, 10);
                                }
                                else if (!payOperator.Equals("weixin")
                                    && !payOperator.Equals("alipay"))
                                {
                                    strErrStatus += "pay_time err ";
                                    payOperator = String.Empty;
                                }
                                newWebOrder.PaymentVendor = payOperator;

                                string bankType = itPayResultRequestVo.BankType;
                                if (String.IsNullOrEmpty(bankType))
                                {
                                    strErrStatus += "bank_type not exist ";
                                    bankType = String.Empty;
                                }
                                else if (20 < bankType.Length)
                                {
                                    strErrStatus += "bank_type err ";
                                    bankType = payOperator.Substring(0, 20);
                                }
                                newWebOrder.BankType = bankType;

                                string errCodeDes = itPayResultRequestVo.ErrCodeDes;
                                if (!String.IsNullOrEmpty(errCodeDes))
                                {
                                    if (300 < bankType.Length)
                                    {
                                        strErrStatus += "err_code_des err ";
                                        errCodeDes = payOperator.Substring(0, 20);
                                    }
                                }
                                newWebOrder.PayErrCodeDes = errCodeDes;

                                newWebOrder.StepStatus = strErrStatus;

                                dbContext.WebOrders.AddObject(newWebOrder);

                                #endregion 校验ITPayResultRequestVo字段，填充WebOrder

                                VoucherList freeVoucher = dbContext.VoucherLists.FirstOrDefault(v => (v.IsValid.Equals(true)
                                    && v.IsUsed.Equals(false)
                                    && v.IsLocked.Equals(false)));
                                if (null != freeVoucher)
                                {
                                    freeVoucher.IsLocked = true;
                                    if (0 < dbContext.SaveChanges())
                                    {
                                        if (null != freeVoucher)
                                        {
                                            freeVoucher.IsUsed = true;
                                            freeVoucher.IsLocked = false;

                                            #region 建立WebOrderVoucher
                                            WebOrderVoucher newWebOrderVoucher = new WebOrderVoucher();
                                            newWebOrderVoucher.WebOrderVoucherId = Guid.NewGuid();
                                            newWebOrderVoucher.WebOrderId = newWebOrder.WebOrderId;
                                            newWebOrderVoucher.TradeNo = newWebOrder.TradeNo;
                                            newWebOrderVoucher.VoucherId = freeVoucher.VoucherId;
                                            newWebOrderVoucher.VoucherCode = freeVoucher.VoucherCode;
                                            // 临时处理
                                            newWebOrderVoucher.OrderVoucherCode = VoucherHelper.VoucherCodeCombine(newWebOrder, freeVoucher.VoucherCode);
                                            newWebOrderVoucher.IsUsed = false;
                                            newWebOrderVoucher.IsValid = true;
                                            newWebOrderVoucher.UsedTime = null;
                                            newWebOrderVoucher.CreateTime = DateTime.Now;
                                            dbContext.WebOrderVouchers.AddObject(newWebOrderVoucher);

                                            // 
                                            newWebOrder.Step = ((int)WebOrderStep.WebPayAsyncResultRespond).ToString();

                                            #endregion 建立WebOrderVoucher

                                            if (0 < dbContext.SaveChanges())
                                            {
                                                // 临时处理
                                                result.Voucher = newWebOrderVoucher.OrderVoucherCode;


                                            }
                                        }
                                    }
                                }

                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strErrStatus += "inner process err ";

                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }
            finally
            {
                result.ErrStatus = strErrStatus;
            }

            return result;
        }


        /// <summary>
        /// 网络订单查询
        /// </summary>
        /// <param name="webOrderRequestVo">TradeNo必须非空</param>
        /// <returns></returns>
        public WebOrderRespondVo WebOrderQuery(WebOrderRequestVo webOrderRequestVo)
        {
            WebOrderRespondVo result = new WebOrderRespondVo();
            result.IsWebOrderVaild = false;

            try
            {
                if (null != webOrderRequestVo)
                {
                    string inputExternalTradeNo = webOrderRequestVo.ExternalTradeNo;
                    string inputTradeNo = webOrderRequestVo.TradeNo;


                    if ((!String.IsNullOrEmpty(inputTradeNo))
                        || (!String.IsNullOrEmpty(inputExternalTradeNo)))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrder existWebOrder = null;
                            if (!String.IsNullOrEmpty(inputTradeNo))
                            {
                                existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));
                            }
                            else
                            {
                                existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.ExternalTradeNo.Equals(inputExternalTradeNo)));
                            }

                            if (null != existWebOrder)
                            {
                                result.IsWebOrderVaild = true;
                                result.TradeNo = existWebOrder.TradeNo;
                                result.ExternalTradeNo = existWebOrder.ExternalTradeNo;
                                result.BuyTime = existWebOrder.BuyTime;
                                result.PaymentVendor = existWebOrder.PaymentVendor;
                                result.OriAFCStationCode = existWebOrder.OriAFCStationCode;
                                result.DesAFCStationCode = existWebOrder.DesAFCStationCode;
                                // 获得实际车站名称
                                result.OriStationChineseName = StationInfoHelper.Instance.GetAFCChineseStationName(existWebOrder.OriAFCStationCode);
                                result.DesStationChineseName = StationInfoHelper.Instance.GetAFCChineseStationName(existWebOrder.DesAFCStationCode);
                                result.OriStationEnglishName = StationInfoHelper.Instance.GetAFCEnglishStationName(existWebOrder.OriAFCStationCode);
                                result.DesStationEnglishName = StationInfoHelper.Instance.GetAFCEnglishStationName(existWebOrder.DesAFCStationCode);
                                result.TicketPrice = existWebOrder.TicketPrice;
                                result.TicketNum = existWebOrder.TicketNum;
                                result.Discount = existWebOrder.Discount;
                                result.ActualFee = 0;
                                if (null != existWebOrder.ActualFee)
                                {
                                    result.ActualFee = existWebOrder.ActualFee.Value;
                                }
                                result.TransactionId = existWebOrder.TransactionId;
                                // 设备类型
                                result.TicketTarget = TicketTargetType.NONE;
                                if (!String.IsNullOrEmpty(existWebOrder.TicketTarget))
                                {
                                    try
                                    {
                                        result.TicketTarget = EnumHelper.GetTicketTargetType(existWebOrder.TicketTarget);
                                    }
                                    catch (Exception)
                                    { }
                                }
                                result.PayEndTime = existWebOrder.PayEndTime;
                                //// 临时处理，当天有效
                                //DateTime dtExpiryTime = new DateTime();
                                //if (null == existWebOrder.PayEndTime)
                                //{
                                //    DateTime dtNow = DateTime.Now;
                                //    dtExpiryTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
                                //    dtExpiryTime = dtExpiryTime.AddDays(1).AddSeconds(-1);
                                //}
                                //else
                                //{
                                //    DateTime dtPayEndTime = existWebOrder.PayEndTime.Value;
                                //    dtExpiryTime = new DateTime(dtPayEndTime.Year, dtPayEndTime.Month, dtPayEndTime.Day);
                                //    dtExpiryTime = dtExpiryTime.AddDays(1).AddSeconds(-1);
                                //}
                                //result.ExpiryTime = dtExpiryTime;
                                result.TicketTakeNum = 0;
                                if (null != existWebOrder.TicketTakeNum)
                                {
                                    result.TicketTakeNum = existWebOrder.TicketTakeNum.Value;
                                }
                                result.TicketTakeTime = existWebOrder.TicketTakeTime;
                                result.OrderStatus = OrderStatusType.None;
                                try
                                {
                                    result.OrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existWebOrder.OrderStatus);
                                }
                                catch (Exception)
                                { }
                                result.OrderStep = EnumHelper.GetWebOrderStep(existWebOrder.Step);

                                WebOrderVoucher existWebOrderVoucher = dbContext.WebOrderVouchers.FirstOrDefault(wov => (wov.WebOrderId.Equals(existWebOrder.WebOrderId)));
                                if (null != existWebOrderVoucher)
                                {
                                    result.IsUsed = existWebOrderVoucher.IsUsed;
                                    result.UsedTime = existWebOrderVoucher.UsedTime;
                                    if (!result.IsUsed)
                                    {
                                        result.Voucher = existWebOrderVoucher.OrderVoucherCode;
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
        /// 网络订单列表查询
        /// </summary>
        /// <param name="payTimeStart">支付时间起始时间</param>
        /// <param name="payTimeEnd">支付时间结束时间</param>
        /// <param name="paymentVendor">必须为“1001”或“1002”</param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public List<WebOrderRespondVo> WebOrderListQuery(DateTime payTimeStart, DateTime payTimeEnd, string paymentVendor, OrderStatusType orderStatus)
        {
            List<WebOrderRespondVo> webOrderRespondList = new List<WebOrderRespondVo>();

            try
            {
                if (0 >= payTimeStart.CompareTo(payTimeEnd))
                {
                    if ((!String.IsNullOrEmpty(paymentVendor))
                       && (paymentVendor.Equals("1001") || paymentVendor.Equals("1002")))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            string orderStatusFlagString = EnumHelper.GetOrderStatusTypeFlagString(orderStatus);
                            List<WebOrder> webOrderList = dbContext.WebOrders.Where(wo => (wo.PaymentVendor.Equals(paymentVendor)
                                && wo.OrderStatus.Equals(orderStatusFlagString))
                                && null != wo.PayEndTime
                                && 0 <= wo.PayEndTime.Value.CompareTo(payTimeStart)
                                && 0 >= wo.PayEndTime.Value.CompareTo(payTimeEnd)).ToList();
                            foreach (WebOrder eachWebOrder in webOrderList)
                            {
                                webOrderRespondList.Add(new WebOrderRespondVo(eachWebOrder));
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

            return webOrderRespondList;
        }

        /// <summary>
        /// 网络订单出票认证
        /// </summary>
        /// <param name="webOrderRequestVo">orderNo和orderToken2个字段都必须非空</param>
        /// <returns></returns>
        public WebOrderVerifyRespondVo WebOrderVerify(WebOrderVerifyRequestVo webOrderRequestVo, Dictionary<string, string> S1_006Dict)
        {
            WebOrderVerifyRespondVo result = new WebOrderVerifyRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC0005;
            // 临时处理
            result.userMsisdn = String.Empty;

            bool isOrderValid = false;

            try
            {
                if (!S1_006Dict.ContainsKey(webOrderRequestVo.orderNo + webOrderRequestVo.DeviceId))
                {
                    isOrderValid = true;
                    S1_006Dict.Add(webOrderRequestVo.orderNo + webOrderRequestVo.DeviceId, webOrderRequestVo.ReqSysDate.ToString());
                }
                else if (DateTime.Now.Subtract(Convert.ToDateTime(S1_006Dict[webOrderRequestVo.orderNo + webOrderRequestVo.DeviceId])).TotalSeconds > 15)
                {
                    isOrderValid = true;
                    S1_006Dict.Clear();
                }
                else
                {
                    S1_006Dict[webOrderRequestVo.orderNo + webOrderRequestVo.DeviceId] = DateTime.Now.ToString();
                    result.RespondCode = DeviceCommRespondCode.RC0009;
                }
                if (null != webOrderRequestVo && isOrderValid)
                {
                    // 订单不存在
                    result.RespondCode = DeviceCommRespondCode.RC0004;


                    string inputTradeNo = webOrderRequestVo.orderNo;
                    string inputOrderVoucherCode = webOrderRequestVo.orderToken;

                    string inputDeviceId = webOrderRequestVo.DeviceId;


                    if ((!String.IsNullOrEmpty(inputTradeNo))
                        && (!String.IsNullOrEmpty(inputOrderVoucherCode)))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            // 不按inputOrderVoucherCode查询提高速度，按inputTradeNo查到了再匹配
                            WebOrderVoucher existWebOrderVoucher = dbContext.WebOrderVouchers.FirstOrDefault(wov => wov.TradeNo.Equals(inputTradeNo));

                            if ((null != existWebOrderVoucher)
                                && existWebOrderVoucher.OrderVoucherCode.Equals(inputOrderVoucherCode))
                            {
                                // 订单存在
                                result.orderNo = existWebOrderVoucher.TradeNo;

                                // 订单存在
                                WebOrder existOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.WebOrderId.Equals(existWebOrderVoucher.WebOrderId)));

                                if (null == existOrder)
                                {
                                    result.RespondCode = DeviceCommRespondCode.RC9999;

                                    _log.Error(String.Format("WebOrderId {0} has no WebOrder", existWebOrderVoucher.WebOrderId));
                                }
                                // 优先判断订单有效性
                                else if (!existOrder.IsValid)
                                {
                                    // 订单无效
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
                                            int newStepFlag = EnumHelper.GetWebOrderStepFlag(WebOrderStep.WebOrderVerifyRespond);

                                            //// 这里不允许重复验证发送没有等号
                                            //gaoke 20160707  允许重发S1_006请求
                                            if (newStepFlag >= existStepFlag)
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderVerifyRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            result.pickupStationCode = existOrder.OriAFCStationCode;
                                            result.getOffStationCode = existOrder.DesAFCStationCode;
                                            result.singlelTicketPrice = ((int)(existOrder.TicketPrice * existOrder.Discount)).ToString();
                                            result.singleTicketNum = existOrder.TicketNum.ToString();
                                            result.singleTicketType = "0";

                                            if (String.IsNullOrEmpty(existOrder.DeviceId))
                                            {
                                                existOrder.DeviceId = inputDeviceId;
                                            }

                                            if (!existOrder.DeviceId.Equals(inputDeviceId))
                                            {
                                                // 订单存在但已锁定
                                                result.RespondCode = DeviceCommRespondCode.RC0009;
                                            }
                                            else
                                            {
                                                existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderVerifyRespond);
                                            }
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
        /// 订单查询
        /// </summary>
        /// <param name="orderQueryRequestVo">orderNo必须非空</param>
        /// <returns></returns>
        public OrderQueryRespondVo OrderQuery(OrderQueryRequestVo orderQueryRequestVo)
        {
            OrderQueryRespondVo result = new OrderQueryRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC0004;

            bool isOrderValid = false;

            try
            {

                if (null != orderQueryRequestVo)
                {
                    // 订单不存在
                    result.RespondCode = DeviceCommRespondCode.RC0004;


                    string inputTradeNo = orderQueryRequestVo.orderNo;


                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            // 订单存在
                            WebOrder existOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

                            if (null == existOrder)
                            {
                                result.RespondCode = DeviceCommRespondCode.RC0004;

                                _log.Error(String.Format("WebOrderId {0} has no WebOrder", inputTradeNo));
                            }
                            // 优先判断订单有效性
                            else if (!existOrder.IsValid)
                            {
                                // 订单无效
                                result.RespondCode = DeviceCommRespondCode.RC0009;
                            }
                            else
                            {
                                // 订单查询成功
                                result.RespondCode = DeviceCommRespondCode.RC0000;

                                result.tradeNo = existOrder.TradeNo;
                                if (existOrder.ChannelType == "00")
                                {
                                    result.singleTicketType = existOrder.ChannelType;
                                }
                                else
                                {
                                    result.singleTicketType = "01";
                                }
                                result.buyTime = existOrder.BuyTime.ToString();
                                result.oriAFCStationCode = existOrder.OriAFCStationCode;
                                result.desAFCStationCode = existOrder.DesAFCStationCode;
                                result.ticketPrice = (int)(existOrder.TicketPrice);
                                result.discount = existOrder.Discount;
                                result.amount = (int)(existOrder.TicketNum * existOrder.TicketPrice * existOrder.Discount);
                                result.orderStatus = existOrder.OrderStatus;
                                result.ticketTakeNum = (int)existOrder.TicketTakeNum;
                                result.ticketTakeTime = existOrder.TicketTakeTime.ToString();
                                result.entryDeviceCode = existOrder.DeviceId;
                                if (result.singleTicketType == "00")
                                {
                                    result.entryTime = existOrder.TicketTakeTime.ToString();
                                }
                                else
                                {
                                    result.ticketTakeTime = "";
                                }
                                if (result.singleTicketType == "01")
                                {
                                    result.exitDeviceCode = "0000000000";
                                }
                                else
                                {
                                    result.exitDeviceCode = result.entryDeviceCode;
                                }
                                result.exitTime = "";
                                dbContext.SaveChanges();
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
        /// 订单状态更新
        /// </summary>
        /// <param name="orderStatusUpdateRequestVo"></param>
        /// <returns></returns>
        public OrderStatusUpdateRespondVo OrderStatusUpdate(OrderStatusUpdateRequestVo orderStatusUpdateRequestVo)
        {
            OrderStatusUpdateRespondVo result = new OrderStatusUpdateRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC0004;

            bool isOrderValid = false;

            try
            {

                if (null != orderStatusUpdateRequestVo)
                {
                    //需要明确将这些更新的信息保存到数据库的什么表里面？

                    // 响应0000
                    result.RespondCode = DeviceCommRespondCode.RC0000;
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
        /// 订单退款
        /// </summary>
        /// <param name="orderRefundRequestVo">orderNo必须非空</param>
        /// <returns></returns>
        public OrderRefundRespondVo OrderRefund(OrderRefundRequestVo orderRefundRequestVo)
        {
            OrderRefundRespondVo result = new OrderRefundRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC0004;

            bool isOrderValid = false;

            try
            {

                if (null != orderRefundRequestVo)
                {
                    // 订单不存在
                    result.RespondCode = DeviceCommRespondCode.RC0004;


                    string inputTradeNo = orderRefundRequestVo.orderNo;


                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            // 订单存在
                            WebOrder existOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

                            if (null == existOrder)
                            {
                                result.RespondCode = DeviceCommRespondCode.RC0004;

                                _log.Error(String.Format("WebOrderId {0} has no WebOrder", inputTradeNo));
                            }
                            // 优先判断订单有效性
                            else if (!existOrder.IsValid)
                            {
                                // 订单无效
                                result.RespondCode = DeviceCommRespondCode.RC0009;
                            }
                            else
                            {
                                // 订单查询成功
                                result.RespondCode = DeviceCommRespondCode.RC0000;
                                //需要调用退款接口，根据退款结果返回相应的响应码
                                dbContext.SaveChanges();
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
        /// 乘客事务处理
        /// </summary>
        /// <param name="passengerAffairDealRequestVo"></param>
        /// <returns></returns>
        public PassengerAffairDealRespondVo PassengerAffairDeal(PassengerAffairDealRequestVo passengerAffairDealRequestVo)
        {
            PassengerAffairDealRespondVo result = new PassengerAffairDealRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC0004;

            bool isOrderValid = false;

            try
            {

                if (null != passengerAffairDealRequestVo)
                {
                    //确定乘客事务处理结果保存到哪里

                    // 响应0000
                    result.RespondCode = DeviceCommRespondCode.RC0000;
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
        /// 乘客事务处理
        /// </summary>
        /// <param name="passengerAffairDealRequestVo"></param>
        /// <returns></returns>
        public PassengerAffairDealStatusQueryRespondVo PassengerAffairDealStatueQuery(PassengerAffairDealStatusQueryRequestVo passengerAffairDealStatusQueryRequestVo)
        {
            PassengerAffairDealStatusQueryRespondVo result = new PassengerAffairDealStatusQueryRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC0004;

            bool isOrderValid = false;

            try
            {

                if (null != passengerAffairDealStatusQueryRequestVo)
                {
                    //从乘客事务处理记录数据库中查询相应的记录

                    // 响应0000
                    result.RespondCode = DeviceCommRespondCode.RC0000;
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

        ///// <summary>
        ///// 网络订单验证
        ///// </summary>
        ///// <param name="webOrderProcessRequestVo">2个字段都必须非空</param>
        ///// <returns></returns>
        //public WebOrderProcessRespondVo WebOrderVerify(WebOrderProcessRequestVo webOrderProcessRequestVo)
        //{
        //    WebOrderProcessRespondVo result = new WebOrderProcessRespondVo();
        //    result.RespondCode = DeviceCommRespondCode.RC9999;


        //    try
        //    {
        //        if (null != webOrderProcessRequestVo)
        //        {
        //            string inputTradeNo = webOrderProcessRequestVo.orderNo;
        //            string inputDeviceId = webOrderProcessRequestVo.DeviceId;
        //            if (null == inputDeviceId)
        //            {
        //                inputDeviceId = String.Empty;
        //            }

        //            if (!String.IsNullOrEmpty(inputTradeNo))
        //            {
        //                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
        //                {

        //                    WebOrder existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

        //                    if (null == existWebOrder)
        //                    {
        //                        // 订单无效
        //                        result.RespondCode = DeviceCommRespondCode.RC0003;
        //                    }
        //                    else
        //                    {
        //                        // 订单存在
        //                        if (!existWebOrder.DeviceId.Equals(inputDeviceId))
        //                        {
        //                            // 订单存在但已锁定
        //                            result.RespondCode = DeviceCommRespondCode.RC0009;
        //                        }
        //                        else
        //                        {
        //                            existWebOrder.DeviceId = inputDeviceId;

        //                            if (null != existWebOrder.TicketTakeTime)
        //                            {
        //                                // 已取票
        //                                result.RespondCode = DeviceCommRespondCode.RC0008;
        //                            }
        //                            else
        //                            {
        //                                // 已取票
        //                                result.RespondCode = DeviceCommRespondCode.RC0000;

        //                                existWebOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderProcessRespond);

        //                                if (0 < dbContext.SaveChanges())
        //                                {

        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.RespondCode = DeviceCommRespondCode.RC9999;

        //        _log.Error(ex.Message);
        //        if (null != ex.InnerException)
        //        {
        //            _log.Error(ex.InnerException.Message);
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 网络订单开始执行
        /// </summary>
        /// <param name="webOrderProcessRequestVo"></param>
        /// <returns></returns>
        public WebOrderProcessRespondVo WebOrderProcess(WebOrderProcessRequestVo webOrderProcessRequestVo)
        {
            WebOrderProcessRespondVo result = new WebOrderProcessRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != webOrderProcessRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = webOrderProcessRequestVo.orderNo;
                    string inputDeviceId = webOrderProcessRequestVo.DeviceId;

                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrder existOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

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
                                            int newStepFlag = EnumHelper.GetWebOrderStepFlag(WebOrderStep.WebOrderProcessRespond);

                                            if (newStepFlag >= existStepFlag)
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderProcessRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderProcessRespond);
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
        /// 网络订执行结果通知
        /// </summary>
        /// <param name="webOrderTakenRequestVo"></param>
        /// <returns></returns>
        public WebOrderTakenRespondVo WebOrderTaken(WebOrderTakenRequestVo webOrderTakenRequestVo)
        {
            WebOrderTakenRespondVo result = new WebOrderTakenRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;

            try
            {
                if (null != webOrderTakenRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0003;

                    string inputTradeNo = webOrderTakenRequestVo.orderNo.Trim();
                    string inputDeviceId = webOrderTakenRequestVo.DeviceId;

                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrder existOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

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
                                                isOrderStatusVaild = true;  //gaoke 20160718   取票成功也返回0000

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
                                            int newStepFlag = EnumHelper.GetWebOrderStepFlag(WebOrderStep.WebOrderTakenRespond);

                                            //gaoke 20160707  允许重发S1_004请求
                                            if (newStepFlag >= existStepFlag)
                                            {
                                                isOrderStepVaild = true;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error(ex.Message);

                                            existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderTakenRequest);
                                        }
                                        #endregion 判断订单步骤

                                        if (isOrderStepVaild)
                                        {
                                            _log.Info("订单： " + existOrder.TradeNo + "的订单执行步骤合法！");
                                            // 未取票
                                            result.RespondCode = DeviceCommRespondCode.RC0000;

                                            string inputTakeTicketNum = webOrderTakenRequestVo.takeSingleTicketNum;
                                            int takeTicketNum = 0;
                                            if (!int.TryParse(inputTakeTicketNum, out takeTicketNum))
                                            {
                                                _log.Error(String.Format("inputTakeTicketNum Error {0}", inputTakeTicketNum));
                                            }
                                            existOrder.TicketTakeNum = takeTicketNum;


                                            DateTime? dtTakeTicketDate = webOrderTakenRequestVo.takeSingleTicketDate;
                                            if (null == dtTakeTicketDate)
                                            {
                                                dtTakeTicketDate = DateTime.Now;
                                            }
                                            existOrder.TicketTakeTime = dtTakeTicketDate.Value;

                                            if (existOrder.TicketTakeNum.Equals(existOrder.TicketNum))
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketOut);
                                                existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderTakenRespond);
                                            }
                                            else
                                            {
                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketException);
                                                existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderTakenErrRespond);
                                            }
                                            dbContext.SaveChanges();
                                            // 取票通知
                                            if (TicketTakeNotify(existOrder) && existOrder.OrderStatus == EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketOut) || existOrder.OrderStatus == EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketException))
                                            {
                                                _log.Info("通知IT订单：" + existOrder.TradeNo + " 已经取票！");
                                            }
                                            else if (!TicketTakeNotify(existOrder))
                                            {
                                                _log.Info("未能通知IT订单：" + existOrder.TradeNo + " 出票结果！");
                                            }
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
        /// 扫码支付取票订单查询
        /// </summary>
        /// <param name="webSnapQRCodeTakeOrderQueryRequestVo"></param>
        /// <returns></returns>
        public WebSnapQRCodeTakeOrderQueryRespondVo SnapQRCodeTakeOrderQuery(WebSnapQRCodeTakeOrderQueryRequestVo webOrderTakenRequestVo, ConcurrentDictionary<string, string> S1_006Dict)
        {
            WebSnapQRCodeTakeOrderQueryRespondVo result = new WebSnapQRCodeTakeOrderQueryRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC0005; 

            bool isOrderValid = false;

            try
            {
                if (null != webOrderTakenRequestVo)
                {
                    // 订单不存在
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputRandomFact = webOrderTakenRequestVo.randomFact;

                    string inputDeviceId = webOrderTakenRequestVo.DeviceId;

                    _log.Debug("Request data: " + webOrderTakenRequestVo.ReqSysDate + " Request operateCode: " + webOrderTakenRequestVo.OperationCode + " Request cityCode: " + webOrderTakenRequestVo.CityCode + " Request channelType: " + webOrderTakenRequestVo.ChannelType + " Request DeviceId: " + webOrderTakenRequestVo.DeviceId + " Request RandomFact: " + webOrderTakenRequestVo.randomFact);

                    if (!String.IsNullOrEmpty(inputRandomFact))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrder qrcWebOrder = dbContext.WebOrders.FirstOrDefault(wov => wov.RandomFact.Equals(inputRandomFact) && wov.DeviceId.Equals(inputDeviceId));

                            if ((null != qrcWebOrder)
                                && qrcWebOrder.RandomFact.Equals(inputRandomFact))
                            {
                                if (qrcWebOrder.DeviceId == inputDeviceId)
                                {
                                    if (qrcWebOrder.OrderStatus == "3" && qrcWebOrder.Step == "11" || qrcWebOrder.Step == "17")
                                    {
                                        result.orderNo = qrcWebOrder.TradeNo;
                                        result.RespondCode = DeviceCommRespondCode.RC0000;

                                        result.pickupStationCode = qrcWebOrder.OriAFCStationCode;
                                        result.getOffStationCode = qrcWebOrder.DesAFCStationCode;
                                        result.singleTicketPrice = ((int)(qrcWebOrder.TicketPrice * qrcWebOrder.Discount)).ToString();
                                        result.singleTicketNum = qrcWebOrder.TicketNum.ToString();
                                        if (!String.IsNullOrEmpty(result.getOffStationCode))
                                        {
                                            result.singleTicketType = "0";
                                        }
                                        else
                                        {
                                            result.singleTicketType = "1";
                                        }
                                        result.deviceId = qrcWebOrder.DeviceId;

                                        //gaoke 20161215 对用户信息进行部分隐藏
                                        if (String.IsNullOrEmpty(qrcWebOrder.UserAccount))
                                        {
                                            _log.Info("开始拼接用户名（为空）！");
                                            result.userName = qrcWebOrder.UserAccount;
                                        }
                                        else if (qrcWebOrder.UserAccount.Contains("@"))
                                        {
                                            _log.Info("开始拼接用户名（邮箱）！");
                                            string str = "";
                                            for (int i = 0; i < qrcWebOrder.UserAccount.IndexOf("@") - 3; i++)
                                            {
                                                str = str + "*";
                                            }
                                            result.userName = qrcWebOrder.UserAccount.Substring(0, 3) + str + qrcWebOrder.UserAccount.Substring(qrcWebOrder.UserAccount.IndexOf("@"), qrcWebOrder.UserAccount.Length - qrcWebOrder.UserAccount.IndexOf("@"));
                                            _log.Info("用户名为： " + result.userName);
                                        }
                                        else if (qrcWebOrder.UserAccount.Length == 11)
                                        {
                                            _log.Info("开始拼接用户名（手机号）！");
                                            result.userName = qrcWebOrder.UserAccount.Substring(0, 3) + "****" + qrcWebOrder.UserAccount.Substring(7, 4);
                                            _log.Info("用户名为： " + result.userName);
                                        }
                                        else
                                        {
                                            result.userName = qrcWebOrder.UserAccount;
                                        }

                                        // 验票通知
                                        TicketTakeNotify(qrcWebOrder);
                                        _log.Info("通知IT订单：" + qrcWebOrder.TradeNo + " 验票！");
                                    }
                                    else
                                    {
                                        result.RespondCode = DeviceCommRespondCode.RC0004;
                                    }
                                }
                                else
                                {
                                    result.RespondCode = DeviceCommRespondCode.RC0009;
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
        /// <param name="webOrderTakenErrRequestVo"></param>
        /// <returns></returns>
        public WebOrderTakenErrRespondVo WebOrderTakenErr(WebOrderTakenErrRequestVo webOrderTakenErrRequestVo)
        {
            WebOrderTakenErrRespondVo result = new WebOrderTakenErrRespondVo();
            result.respCodeMemo = String.Empty;
            result.RespondCode = DeviceCommRespondCode.RC9999;
            _log.Info("网络订单异常处理！");
            try
            {
                if (null != webOrderTakenErrRequestVo)
                {
                    // 订单无效
                    result.RespondCode = DeviceCommRespondCode.RC0004;

                    string inputTradeNo = webOrderTakenErrRequestVo.orderNo;
                    string inputDeviceId = webOrderTakenErrRequestVo.DeviceId.Trim();

                    if (!String.IsNullOrEmpty(inputTradeNo))
                    {

                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            if (webOrderTakenErrRequestVo.errorCode == "0008" && inputTradeNo.Length == 10)
                            {
                                _log.Info("将设备号当做订单号进行撤销关联！");
                                WebOrder existOrder1 = dbContext.WebOrders.FirstOrDefault(wo => wo.DeviceId.Equals(inputTradeNo) && wo.RandomFact.Equals(webOrderTakenErrRequestVo.faultSlipSeq));
                                if (null != existOrder1)
                                {
                                    existOrder1.DeviceId = "";
                                    existOrder1.RandomFact = "";
                                    existOrder1.OrderStatus = "3";
                                    existOrder1.Step = "7";
                                    result.RespondCode = DeviceCommRespondCode.RC0000;
                                    _log.Info("已对根据设备号查找到的订单： " + existOrder1.TradeNo + " 进行了撤销关联操作！");
                                    dbContext.SaveChanges();
                                }
                                else
                                { 
                                    //gaoke 20161221   增加无效随机因子的记录
                                    InvalidRandomFact newRandomFact = new InvalidRandomFact();
                                    newRandomFact.RandomFactId = Guid.NewGuid();
                                    newRandomFact.DeviceId = webOrderTakenErrRequestVo.orderNo;
                                    newRandomFact.RandomFact = webOrderTakenErrRequestVo.faultSlipSeq;
                                    using (MobilePayDBEntities dbContext1 = new MobilePayDBEntities())
                                    {
                                        InvalidRandomFact randomFact = dbContext1.InvalidRandomFacts.FirstOrDefault(wo => wo.RandomFact.Equals(webOrderTakenErrRequestVo.faultSlipSeq));
                                        if (randomFact == null)
                                        {
                                            dbContext1.InvalidRandomFacts.AddObject(newRandomFact);
                                            dbContext1.SaveChanges();
                                            _log.Info("已将随机因子" + webOrderTakenErrRequestVo.faultSlipSeq + "作为无效随机因子！");  
                                        }
                                        result.RespondCode = DeviceCommRespondCode.RC0000;
                                    }
                                }
                            }
                            else if (inputTradeNo.Length == 30)
                            {
                                WebOrder existOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));

                                if (null != existOrder)
                                {
                                    _log.Info("按订单号进行撤销关联！");
                                    if (webOrderTakenErrRequestVo.errorCode == "0008")
                                    {
                                        existOrder.DeviceId = "";
                                        existOrder.RandomFact = "";
                                        existOrder.OrderStatus = "3";
                                        existOrder.Step = "7";
                                        result.RespondCode = DeviceCommRespondCode.RC0000;
                                        _log.Info("已按订单号更改订单" + existOrder.TradeNo + "状态！");
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


                                                string inputTakeTicketNum = webOrderTakenErrRequestVo.takeSingleTicketNum;
                                                if (string.IsNullOrEmpty(inputTakeTicketNum) || inputTakeTicketNum == "0")
                                                {
                                                    existOrder.TicketTakeNum = 0;
                                                }
                                                else
                                                {
                                                    existOrder.TicketTakeNum = Convert.ToInt32(inputTakeTicketNum);
                                                }

                                                DateTime? dtTakeTicketErrDate = webOrderTakenErrRequestVo.faultOccurDate;
                                                if (null == dtTakeTicketErrDate)
                                                {
                                                    dtTakeTicketErrDate = DateTime.Now;
                                                }
                                                existOrder.DeviceErrTime = dtTakeTicketErrDate.Value;
                                                existOrder.TicketTakeTime = dtTakeTicketErrDate.Value;
                                                existOrder.DeviceErrCode = webOrderTakenErrRequestVo.errorCode;
                                                existOrder.DeviceErrMessage = webOrderTakenErrRequestVo.errorMessage;
                                                existOrder.DeviceErrSlipSeq = webOrderTakenErrRequestVo.faultSlipSeq;

                                                existOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketException);
                                                existOrder.Step = EnumHelper.GetWebOrderStepFlagString(WebOrderStep.WebOrderTakenErrRespond);

                                                // 取票通知
                                                TicketTakeNotify(existOrder);
                                                _log.Info("通知IT订单：" + existOrder.TradeNo + " 已经取票！");

                                                dbContext.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result.RespondCode = DeviceCommRespondCode.RC0000;
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
        /// 网络订单退款记录请求，用于原来没有退款记录创建订单记录
        /// </summary>
        /// <param name="webOrderRefundRecordRequestVo"></param>
        /// <returns></returns>
        public WebOrderRefundRecordRespondVo WebOrderRefundRecord(WebOrderRefundRecordRequestVo webOrderRefundRecordRequestVo)
        {
            WebOrderRefundRecordRespondVo result = new WebOrderRefundRecordRespondVo();

            try
            {
                if (null != webOrderRefundRecordRequestVo)
                {
                    string inputTradeNo = webOrderRefundRecordRequestVo.TradeNo;
                    string inputExternalTradeNo = webOrderRefundRecordRequestVo.ExternalTradeNo;
                    string inputRefundTradeNo = webOrderRefundRecordRequestVo.RefundTradeNo;

                    if (webOrderRefundRecordRequestVo.OrderStatus.Equals(OrderStatusType.RefundProcessing)
                        || webOrderRefundRecordRequestVo.OrderStatus.Equals(OrderStatusType.RefundSuccess)
                        || webOrderRefundRecordRequestVo.OrderStatus.Equals(OrderStatusType.RefundFail))
                    {
                        // 传入状态有效
                        result.IsOrderStatusValid = true;

                        if ((!String.IsNullOrEmpty(inputTradeNo) || !String.IsNullOrEmpty(inputExternalTradeNo))
                            && !String.IsNullOrEmpty(inputRefundTradeNo))
                        {
                            using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                            {
                                WebOrder existWebOrder = null;
                                if (!String.IsNullOrEmpty(inputTradeNo))
                                {
                                    existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));
                                }
                                else
                                {
                                    existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.ExternalTradeNo.Equals(inputExternalTradeNo)));
                                }

                                if (null != existWebOrder)
                                {
                                    // 订单存在
                                    result.IsTradeNoValid = true;
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existWebOrder.OrderStatus);
                                    }
                                    catch (Exception)
                                    { }

                                    if (existOrderStatus.Equals(OrderStatusType.None)
                                        || existOrderStatus.Equals(OrderStatusType.WaitBuyerPay)
                                        || existOrderStatus.Equals(OrderStatusType.TradeFaild)
                                        || existOrderStatus.Equals(OrderStatusType.TicketOut)
                                        || existOrderStatus.Equals(OrderStatusType.RefundProcessing)
                                        || existOrderStatus.Equals(OrderStatusType.RefundSuccess)
                                        || existOrderStatus.Equals(OrderStatusType.TradeInvalid)
                                        || existOrderStatus.Equals(OrderStatusType.TradeClosed)
                                        )
                                    {
                                        result.OrderStatus = existOrderStatus;
                                    }
                                    else
                                    {
                                        WebOrderRefund newWebOrderRefund = new WebOrderRefund();
                                        newWebOrderRefund.WebOrderRefundId = Guid.NewGuid();
                                        newWebOrderRefund.WebOrderId = existWebOrder.WebOrderId;
                                        newWebOrderRefund.TradeNo = existWebOrder.TradeNo;
                                        newWebOrderRefund.ExternalTradeNo = existWebOrder.ExternalTradeNo;
                                        newWebOrderRefund.RefundTradeNo = inputRefundTradeNo;
                                        newWebOrderRefund.RefundReason = String.Empty;
                                        if (null != webOrderRefundRecordRequestVo.RefundReason)
                                        {
                                            newWebOrderRefund.RefundReason = webOrderRefundRecordRequestVo.RefundReason;
                                        }
                                        newWebOrderRefund.PaymentVendor = existWebOrder.PaymentVendor;
                                        newWebOrderRefund.RefundFee = webOrderRefundRecordRequestVo.RefundFee;
                                        newWebOrderRefund.TotalFee = existWebOrder.ActualFee.Value;
                                        newWebOrderRefund.BankType = existWebOrder.BankType;
                                        newWebOrderRefund.RequestTime = webOrderRefundRecordRequestVo.RequestTime;
                                        newWebOrderRefund.IsRequestSuccess = webOrderRefundRecordRequestVo.IsRequestSuccess;
                                        newWebOrderRefund.RequestErrCodeDes = webOrderRefundRecordRequestVo.RequestErrCodeDes;
                                        newWebOrderRefund.IsRespondSuccess = webOrderRefundRecordRequestVo.IsRespondSuccess;
                                        newWebOrderRefund.RespondTime = webOrderRefundRecordRequestVo.RespondTime;
                                        newWebOrderRefund.RespondErrCodeDes = webOrderRefundRecordRequestVo.RespondErrCodeDes;
                                        newWebOrderRefund.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(webOrderRefundRecordRequestVo.OrderStatus);

                                        dbContext.WebOrderRefunds.AddObject(newWebOrderRefund);

                                        existWebOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(webOrderRefundRecordRequestVo.OrderStatus);

                                        if (0 < dbContext.SaveChanges())
                                        {
                                            result.IsSuccess = true;
                                            result.OrderStatus = OrderStatusType.RefundProcessing;
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
        /// 网络订单退款记录更新请求，原有订单状态必须为“退款中”或“退款失败”
        /// </summary>
        /// <param name="webOrderRefundUpdateRequestVo">TradeNo、ExternalTradeNo、RefundTradeNo</param>
        /// <returns></returns>
        public WebOrderRefundUpdateRespondVo WebOrderRefundUpdate(WebOrderRefundUpdateRequestVo webOrderRefundUpdateRequestVo)
        {
            WebOrderRefundUpdateRespondVo result = new WebOrderRefundUpdateRespondVo();
            result.IsTradeNoValid = false;
            result.IsOrderStatusValid = false;
            result.IsSuccess = false;
            result.OrderStatus = OrderStatusType.None;

            try
            {
                if (null != webOrderRefundUpdateRequestVo)
                {
                    string inputTradeNo = webOrderRefundUpdateRequestVo.TradeNo;
                    string inputExternalTradeNo = webOrderRefundUpdateRequestVo.ExternalTradeNo;
                    string inputRefundTradeNo = webOrderRefundUpdateRequestVo.RefundTradeNo;

                    if (webOrderRefundUpdateRequestVo.OrderStatus.Equals(OrderStatusType.RefundProcessing)
                        || webOrderRefundUpdateRequestVo.OrderStatus.Equals(OrderStatusType.RefundSuccess)
                        || webOrderRefundUpdateRequestVo.OrderStatus.Equals(OrderStatusType.RefundFail))
                    {
                        // 传入状态有效
                        result.IsOrderStatusValid = true;

                        if (!String.IsNullOrEmpty(inputTradeNo)
                            && !String.IsNullOrEmpty(inputExternalTradeNo)
                            && !String.IsNullOrEmpty(inputRefundTradeNo))
                        {
                            using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                            {
                                WebOrderRefund existWebOrderRefund = null;
                                WebOrder existWebOrder = null;
                                if (!String.IsNullOrEmpty(inputTradeNo))
                                {
                                    existWebOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));
                                    existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));
                                }
                                else if (!String.IsNullOrEmpty(inputExternalTradeNo))
                                {
                                    existWebOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.ExternalTradeNo.Equals(inputExternalTradeNo)));
                                    existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.ExternalTradeNo.Equals(inputExternalTradeNo)));
                                }
                                else
                                {
                                    existWebOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.RefundTradeNo.Equals(inputRefundTradeNo)));
                                    existWebOrder = dbContext.WebOrders.FirstOrDefault(wo => (wo.WebOrderId.Equals(existWebOrderRefund.WebOrderId)));
                                }
                                if (null != existWebOrder)
                                {
                                    // 订单存在
                                    result.IsTradeNoValid = true;
                                    // 以existWebOrder更行订单状态
                                    OrderStatusType existOrderStatus = OrderStatusType.None;
                                    try
                                    {
                                        existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existWebOrder.OrderStatus);
                                    }
                                    catch (Exception)
                                    { }
                                    result.OrderStatus = existOrderStatus;

                                    if (null != existWebOrderRefund)
                                    {
                                        // 以existWebOrderRefund更行订单状态
                                        try
                                        {
                                            existOrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existWebOrderRefund.OrderStatus);
                                        }
                                        catch (Exception)
                                        { }
                                        result.OrderStatus = existOrderStatus;

                                        // 原有订单状态必须为“退款中”或“退款失败”
                                        if (existOrderStatus.Equals(OrderStatusType.RefundProcessing)
                                            || existOrderStatus.Equals(OrderStatusType.RefundFail))
                                        {
                                            existWebOrderRefund.IsRespondSuccess = webOrderRefundUpdateRequestVo.IsRespondSuccess;
                                            existWebOrderRefund.RespondTime = webOrderRefundUpdateRequestVo.RespondTime;
                                            existWebOrderRefund.RespondErrCodeDes = webOrderRefundUpdateRequestVo.RespondErrCodeDes;
                                            // 同时更行退款订单和原订单状态
                                            existWebOrderRefund.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(webOrderRefundUpdateRequestVo.OrderStatus);
                                            existWebOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(webOrderRefundUpdateRequestVo.OrderStatus);

                                            if (0 < dbContext.SaveChanges())
                                            {
                                                result.IsSuccess = true;
                                                result.OrderStatus = webOrderRefundUpdateRequestVo.OrderStatus;
                                            }
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
        /// 网络订单退款记录查询
        /// </summary>
        /// <param name="webOrderRefundResultRequestVo">TradeNo、ExternalTradeNo、RefundTradeNo为查询条件，其中1个必须Wie非空</param>
        /// <returns></returns>
        public WebOrderRefundResultRespondVo WebOrderRefundQuery(WebOrderRefundResultRequestVo webOrderRefundResultRequestVo)
        {
            WebOrderRefundResultRespondVo result = new WebOrderRefundResultRespondVo();

            try
            {
                if (null != webOrderRefundResultRequestVo)
                {
                    string inputTradeNo = webOrderRefundResultRequestVo.TradeNo;
                    string inputExternalTradeNo = webOrderRefundResultRequestVo.ExternalTradeNo;
                    string inputRefundTradeNo = webOrderRefundResultRequestVo.RefundTradeNo;

                    if (!String.IsNullOrEmpty(inputTradeNo)
                        || !String.IsNullOrEmpty(inputExternalTradeNo)
                        || !String.IsNullOrEmpty(inputRefundTradeNo))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            WebOrderRefund existWebOrderRefund = null;
                            if (!String.IsNullOrEmpty(inputTradeNo))
                            {
                                existWebOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.TradeNo.Equals(inputTradeNo)));
                            }
                            else if (!String.IsNullOrEmpty(inputExternalTradeNo))
                            {
                                existWebOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.ExternalTradeNo.Equals(inputExternalTradeNo)));
                            }
                            else
                            {
                                existWebOrderRefund = dbContext.WebOrderRefunds.FirstOrDefault(wo => (wo.RefundTradeNo.Equals(inputRefundTradeNo)));
                            }

                            if (null != existWebOrderRefund)
                            {
                                // 订单存在
                                result.IsTradeNoValid = true;
                                result.TradeNo = existWebOrderRefund.TradeNo;
                                result.ExternalTradeNo = existWebOrderRefund.ExternalTradeNo;
                                result.RefundTradeNo = existWebOrderRefund.RefundTradeNo;
                                result.RefundReason = existWebOrderRefund.RefundReason;

                                result.PaymentVendor = existWebOrderRefund.PaymentVendor;

                                result.RefundFee = 0;
                                try
                                {
                                    result.RefundFee = Convert.ToInt32(existWebOrderRefund.RefundFee);
                                }
                                catch (Exception)
                                { }

                                result.TotalFee = 0;
                                try
                                {
                                    result.TotalFee = Convert.ToInt32(existWebOrderRefund.TotalFee);
                                }
                                catch (Exception)
                                { }

                                result.BankType = existWebOrderRefund.BankType;
                                result.RequestTime = existWebOrderRefund.RequestTime;
                                result.IsRequestSuccess = existWebOrderRefund.IsRequestSuccess;
                                result.RequestErrCodeDes = existWebOrderRefund.RequestErrCodeDes;
                                result.IsRespondSuccess = existWebOrderRefund.IsRespondSuccess;
                                result.RespondTime = existWebOrderRefund.RespondTime;
                                result.RespondErrCodeDes = existWebOrderRefund.RespondErrCodeDes;

                                result.OrderStatus = OrderStatusType.None;
                                try
                                {
                                    result.OrderStatus = EnumHelper.GetOrderStatusTypeByFlagString(existWebOrderRefund.OrderStatus);
                                }
                                catch (Exception)
                                { }
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
        /// 网络订单退款记录列表查询
        /// </summary>
        /// <param name="paymentVendor">必须为“1001”或“1002”</param>
        /// <param name="orderStatus">必须为“退款中”、“退款成功”、“退款失败”其中一种</param>
        /// <returns></returns>
        public List<WebOrderRefundResultRespondVo> WebOrderRefundListQuery(string paymentVendor, OrderStatusType orderStatus)
        {
            List<WebOrderRefundResultRespondVo> webOrderRefundResultRespondList = new List<WebOrderRefundResultRespondVo>();

            try
            {
                if ((!String.IsNullOrEmpty(paymentVendor))
                    && (paymentVendor.Equals("1001") || paymentVendor.Equals("1002")))
                {
                    if (orderStatus.Equals(OrderStatusType.RefundProcessing)
                        || orderStatus.Equals(OrderStatusType.RefundSuccess)
                        || orderStatus.Equals(OrderStatusType.RefundFail))
                    {
                        using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                        {
                            string orderStatusFlagString = EnumHelper.GetOrderStatusTypeFlagString(orderStatus);
                            List<WebOrderRefund> webOrderRefundList = dbContext.WebOrderRefunds.Where(wor => (wor.PaymentVendor.Equals(paymentVendor)
                                && wor.OrderStatus.Equals(orderStatusFlagString))).ToList();
                            foreach (WebOrderRefund eachWebOrderRefund in webOrderRefundList)
                            {
                                webOrderRefundResultRespondList.Add(new WebOrderRefundResultRespondVo(eachWebOrderRefund));
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

            return webOrderRefundResultRespondList;
        }

        /// <summary>
        /// 取票通知
        /// </summary>
        /// <param name="webOrder"></param>
        /// <returns></returns>
        private bool TicketTakeNotify(WebOrder webOrder)
        {
            bool isSuccess = false;

            if (null != webOrder)
            {
                try
                {
                    TicketProduceFinishNotify notify = new TicketProduceFinishNotify();
                    TicketFinishNotifyVO notifyVo = new TicketFinishNotifyVO();
                    _log.Info("取票通知中订单车票数量： " + notifyVo.tk_num);
                    notifyVo.tk_num = webOrder.TicketNum;
                    if (webOrder.OrderStatus == "3" && webOrder.Step == "11")
                    {
                        notifyVo.notify_type = "01";
                    }
                    else if (webOrder.OrderStatus == "5" && webOrder.Step == "15")
                    {
                        notifyVo.notify_type = "02";
                    }
                    else if (webOrder.OrderStatus == "6" && webOrder.Step == "17")
                    {
                        notifyVo.notify_type = "02";
                        notifyVo.tk_num = (int)webOrder.TicketTakeNum;
                    }
                    _log.Info("取票通知中订单状态为： " + webOrder.OrderStatus + " 订单步骤为：" + webOrder.Step);
                    notifyVo.trade_no = webOrder.TradeNo;

                    notifyVo.finish_num = webOrder.TicketTakeNum.Value;
                    notifyVo.notify_time = webOrder.TicketTakeTime.Value;
                    _log.Info("取票通知中取票数量： " + notifyVo.finish_num + "取票通知中取票时间： " + notifyVo.notify_time + "取票通知中的通知类型为： " + notifyVo.notify_type);
                    notify.ticket_finish_notifyIT(notifyVo);
                    _log.Info("已通知IT，未见异常！");
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    _log.Error(String.Format("ResultNofity Exception:", ex.Message));
                }
            }

            return isSuccess;
        }


        ///// <summary>
        ///// 取票通知
        ///// </summary>
        ///// <param name="webOrder"></param>
        ///// <returns></returns>
        //private bool TicketTakeNotify(QRCWebOrder qrcWebOrder)
        //{
        //    bool isSuccess = false;

        //    if (null != qrcWebOrder)
        //    {
        //        try
        //        {
        //            TicketProduceFinishNotify notify = new TicketProduceFinishNotify();
        //            TicketFinishNotifyVO notifyVo = new TicketFinishNotifyVO();
        //            notifyVo.trade_no = qrcWebOrder.TradeNo;
        //            notifyVo.tk_num = qrcWebOrder.TicketNum;
        //            notifyVo.finish_num = qrcWebOrder.TicketTakeNum.Value;
        //            notifyVo.notify_time = qrcWebOrder.TicketTakeTime.Value;
        //            notify.ticket_finish_notifyIT(notifyVo);

        //            isSuccess = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            _log.Error(String.Format("ResultNofity Exception:", ex.Message));
        //        }
        //    }

        //    return isSuccess;
        //}

        /// <summary>
        /// 通用订单查询
        /// </summary>
        /// <param name="isQueryUsedTime">查询使用时间标识，启用时查询已使用订单的使用时间，不启用查询所有订单的购买时间</param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="paymentVendor">必须为“1001”或“1002”，空字符串或null包含所有</param>
        /// <param name="ticketOrderType">购票方式</param>
        /// <param name="ticketTarget">车票使用目标设备类型，为None时查询所有</param>
        /// <returns></returns>
        public List<CommonOrderVo> CommonOrderQuery(bool isQueryUsedTime, DateTime fromTime, DateTime toTime, string paymentVendor, TicketTargetType ticketTarget)
        {
            List<CommonOrderVo> commonOrderList = new List<CommonOrderVo>();

            try
            {
                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                {
                    dbContext.CommandTimeout = 300;


                    var result = dbContext.WebOrders.AsQueryable();

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

                    if (!ticketTarget.Equals(TicketTargetType.NONE))
                    {
                        string strTicketTarget = ticketTarget.ToString();
                        result = result.Where(o => o.TicketTarget.Equals(strTicketTarget));
                    }

                    if ((null != result)
                        && (0 < result.Count()))
                    {
                        foreach (WebOrder eachWebOrder in result)
                        {
                            commonOrderList.Add(new CommonOrderVo(eachWebOrder));
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

        ///// <summary>
        ///// 获得订单过期时间，目前是购买时间的隔天凌晨
        ///// </summary>
        ///// <param name="buyTime"></param>
        ///// <returns></returns>
        //public DateTime GetOrderExpiryTime(DateTime buyTime)
        //{
        //    DateTime dtExpiryTime = buyTime;

        //    try
        //    {
        //        dtExpiryTime = new DateTime(buyTime.Year, buyTime.Month, buyTime.Day);
        //        dtExpiryTime = dtExpiryTime.AddDays(2);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error(ex.Message);
        //    }

        //    return dtExpiryTime;
        //}
    }
}
