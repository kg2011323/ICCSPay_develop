using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.Vo;
using log4net;
using System.Reflection;
using PlatformLib.BLL;
using PlatformLib.DB;

namespace PlatformLib.Util
{
    public class StressTestHelper
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 填充已支付网络订单
        /// </summary>
        /// <param name="orderCount"></param>
        /// <param name="ticketTarget"></param>
        /// <returns></returns>
        public List<WebPayResultRespondVo> FillPayedWebOrder(int orderCount, TicketTargetType ticketTarget)
        {
            List<WebPayResultRespondVo> result = new List<WebPayResultRespondVo>();

            string strUserOpenId = "UserOpenId";
            string strUserAccount = "UserAccount";

            try
            {
                //VoucherBo vb = new VoucherBo();
                //vb.FillNewVoucher(orderCount, DateTime.Now, DateTime.Now.AddYears(1));

                WebPreOrderBo wbo = new WebPreOrderBo();
                string webTradeNo = String.Empty;
                string webExternalTradeNo = Guid.NewGuid().ToString();


                for (int orderIndex = 1; orderIndex <= orderCount; orderIndex++)
                {
                    webExternalTradeNo = Guid.NewGuid().ToString();

                    #region 获得 商家订单号
                    

                    int amount = 400;
                    WebTradeNoRequestVo newWebTradeNoRequestVo = new WebTradeNoRequestVo()
                    {
                        ExternalTradeNo = webExternalTradeNo,
                        BuyTime = DateTime.Now,
                        OperationCode = String.Empty,
                        CityCode = "020",
                        DeviceId = String.Empty,
                        ChannelType = String.Empty,
                        PaymentVendor = String.Empty,
                        OriAFCStationCode = "0101",
                        DesAFCStationCode = "0102",
                        TicketPrice = 200,
                        TicketNum = 2,
                        Discount = 1,
                        Amount = amount,
                        TicketTarget = ticketTarget,
                        UserOpenId = strUserOpenId,
                        UserAccount = strUserAccount
                    };

                    //Console.WriteLine("WebTradeNoRespondVo");
                    WebTradeNoRespondVo webTradeNoRespondVo = wbo.GetTradeNo(newWebTradeNoRequestVo);
                    //Console.WriteLine(webTradeNoRespondVo.IsSuccess);
                    //Console.WriteLine(webTradeNoRespondVo.WebOrderId);
                    //Console.WriteLine(webTradeNoRespondVo.TradeNo);
                    //Console.WriteLine(webTradeNoRespondVo.IsVaild);
                    //Console.WriteLine(webTradeNoRespondVo.StepStatus);

                    webTradeNo = webTradeNoRespondVo.TradeNo;

                    #endregion 获得 商家订单号

                    #region 记录 预支付交易会话标识
                    string testPrepayId = Guid.NewGuid().ToString();
                    WebPrePayRequestVo webPrePayRequestVo = new WebPrePayRequestVo();
                    webPrePayRequestVo.WebOrderId = null;
                    webPrePayRequestVo.TradeNo = webTradeNo;
                    webPrePayRequestVo.PrepayId = testPrepayId;

                    //Console.WriteLine("WebPrePayRespondVo");
                    WebPrePayRespondVo webPrePayRespondVo = wbo.PrePayRecord(webPrePayRequestVo);
                    //Console.WriteLine(webPrePayRespondVo.IsSuccess);
                    //Console.WriteLine(webPrePayRespondVo.WebOrderId);
                    //Console.WriteLine(webPrePayRespondVo.TradeNo);
                    //Console.WriteLine(webPrePayRespondVo.ExternalTradeNo);
                    //Console.WriteLine(webPrePayRespondVo.StepStatus);

                    #endregion 记录 预支付交易会话标识


                    #region 支付异步结果记录
                    WebPayResultRequestVo webPayResultRequestVo = new WebPayResultRequestVo();
                    webPayResultRequestVo.TradeNo = webTradeNo;
                    webPayResultRequestVo.TransactionId = Guid.NewGuid().ToString();
                    webPayResultRequestVo.PayEndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    webPayResultRequestVo.BankType = "BankType";
                    webPayResultRequestVo.ActualFee = amount;
                    webPayResultRequestVo.ErrCodeDes = String.Empty;
                    webPayResultRequestVo.IsSuccess = true;
                    webPayResultRequestVo.UserOpenId = strUserOpenId;
                    webPayResultRequestVo.UserAccount = strUserAccount;

                    //Console.WriteLine("WebPayResultRequestVo");
                    WebPayResultRespondVo webPayResultRespondVo = wbo.PayResultRecord(webPayResultRequestVo);
                    //Console.WriteLine(webPayResultRespondVo.IsSuccess);
                    //Console.WriteLine(webPayResultRespondVo.WebOrderId);
                    //Console.WriteLine(webPayResultRespondVo.TradeNo);
                    //Console.WriteLine(webPayResultRespondVo.ExternalTradeNo);
                    //Console.WriteLine(webPayResultRespondVo.OriAFCStationCode);
                    //Console.WriteLine(webPayResultRespondVo.DesAFCStationCode);
                    //Console.WriteLine(webPayResultRespondVo.TicketPrice);
                    //Console.WriteLine(webPayResultRespondVo.TicketNum);
                    //Console.WriteLine(webPayResultRespondVo.Discount);
                    //Console.WriteLine(webPayResultRespondVo.PayEndTime);
                    //Console.WriteLine(webPayResultRespondVo.TicketTarget.ToString());
                    //Console.WriteLine(webPayResultRespondVo.Voucher);
                    //Console.WriteLine(webPayResultRespondVo.StepStatus);

                    #endregion 支付异步结果记录

                    result.Add(webPayResultRespondVo);
                    _log.Debug(result.Count);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 填充已取票车站订单
        /// </summary>
        /// <param name="orderCount"></param>
        /// <param name="ticketTarget"></param>
        /// <returns></returns>
        public int FillCompleteedStationOrder(int orderCount)
        {
            int resultCount = 0;

            string stationOrderNo = String.Empty;
            string strCityCode = "020";
            string strDeviceId = "WebLocalTest";
            string stationChannelType = "04";
            string inputPaymentCode = "130481063942121998";

            try
            {
                //VoucherBo vb = new VoucherBo();
                //vb.FillNewVoucher(orderCount, DateTime.Now, DateTime.Now.AddYears(1));

                StationOrderBo sbo = new StationOrderBo();

                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                {
                    for (int orderIndex = 1; orderIndex <= orderCount; orderIndex++)
                    {

                        //#region 现场车站购票（付款）订单提交

                        //StationOrderPayRequestVo newStationOrderPayRequestVo = new StationOrderPayRequestVo()
                        //{
                        //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        //    operationCode = "S1-001",
                        //    cityCode = strCityCode,
                        //    DeviceId = strDeviceId,
                        //    channelType = stationChannelType,
                        //    expandAttribute = new List<string>(),
                        //    paymentCode = inputPaymentCode,
                        //    msisdn = String.Empty,
                        //    iccid = String.Empty,
                        //    serviceId = String.Empty,
                        //    paymentVendor = "1002",
                        //    pickupStationCode = "0101",
                        //    getOffStationCode = "0102",
                        //    singlelTicketPrice = 200,
                        //    singlelTicketNum = 2,
                        //    singleTicketType = "0"
                        //};
                        ////Console.WriteLine("StationOrderPayRespondVo");
                        //StationOrderPayRespondVo stationOrderPayRespondVo = sbo.OrderPay(newStationOrderPayRequestVo);
                        ////Console.WriteLine(stationOrderPayRespondVo.RespondCodeString);
                        ////Console.WriteLine(stationOrderPayRespondVo.partnerNo);
                        ////Console.WriteLine(stationOrderPayRespondVo.orderNo);
                        ////Console.WriteLine(stationOrderPayRespondVo.subject);
                        ////Console.WriteLine(stationOrderPayRespondVo.body);
                        ////Console.WriteLine(stationOrderPayRespondVo.payType);
                        ////Console.WriteLine(stationOrderPayRespondVo.amount);
                        ////Console.WriteLine(stationOrderPayRespondVo.account);
                        ////Console.WriteLine(stationOrderPayRespondVo.notifyUrl);
                        ////Console.WriteLine(stationOrderPayRespondVo.merchantCert);
                        ////Console.WriteLine(stationOrderPayRespondVo.timeout);
                        //stationOrderNo = stationOrderPayRespondVo.orderNo;
                        //#endregion 现场车站购票（付款）订单提交


                        //#region 现场车站购票支付结果查询
                        //StationOrderPayResultRequestVo newStationOrderPayResultRequestVo = new StationOrderPayResultRequestVo()
                        //{
                        //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        //    operationCode = "S1-002",
                        //    cityCode = strCityCode,
                        //    DeviceId = strDeviceId,
                        //    channelType = stationChannelType,
                        //    expandAttribute = new List<string>(),
                        //    orderNo = stationOrderNo
                        //};
                        ////Console.WriteLine("StationOrderPayResultRespondVo");
                        //StationOrderPayResultRespondVo stationOrderPayResultRespondVo = sbo.PayResultQuery(newStationOrderPayResultRequestVo);
                        ////Console.WriteLine(stationOrderPayResultRespondVo.RespondCodeString);
                        ////Console.WriteLine(stationOrderPayResultRespondVo.paymentDateString);
                        ////Console.WriteLine(stationOrderPayResultRespondVo.amount);
                        ////Console.WriteLine(stationOrderPayResultRespondVo.paymentAccount);
                        ////Console.WriteLine(stationOrderPayResultRespondVo.paymentResult);
                        ////Console.WriteLine(stationOrderPayResultRespondVo.paymentDesc);
                        //#endregion 现场车站购票支付结果查询

                        //#region 车站订单开始执行通知
                        //StationOrderProcessRequestVo newStationOrderProcessRequestVo = new StationOrderProcessRequestVo()
                        //{
                        //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        //    operationCode = "S1-003",
                        //    cityCode = strCityCode,
                        //    DeviceId = strDeviceId,
                        //    channelType = stationChannelType,
                        //    expandAttribute = null,
                        //    orderNo = stationOrderNo
                        //};

                        ////Console.WriteLine("WebOrderProcessRespondVo");
                        //StationOrderProcessRespondVo stationOrderProcessRespondVo = sbo.StationOrderProcess(newStationOrderProcessRequestVo);
                        ////Console.WriteLine(stationOrderProcessRespondVo.RespondCodeString);
                        ////Console.WriteLine(stationOrderProcessRespondVo.respCodeMemo);
                        //#endregion 车站订单开始执行通知

                        //#region 车站订单执行结果通知
                        //StationOrderTakenRequestVo newStationOrderTakenRequestVo = new StationOrderTakenRequestVo()
                        //{
                        //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        //    operationCode = "S1-004",
                        //    cityCode = strCityCode,
                        //    DeviceId = strDeviceId,
                        //    channelType = stationChannelType,
                        //    expandAttribute = null,
                        //    orderNo = stationOrderNo,
                        //    takeSingleTicketNum = 1.ToString(),
                        //    takeSingleTicketDateString = DateTime.Now.ToString("yyyyMMddHHmmss")
                        //};
                        ////Console.WriteLine("WebOrderTakenRespondVo");
                        //StationOrderTakenRespondVo stationOrderTakenRespondVo = sbo.StationOrderTaken(newStationOrderTakenRequestVo);
                        ////Console.WriteLine(stationOrderTakenRespondVo.RespondCodeString);
                        ////Console.WriteLine(stationOrderTakenRespondVo.respCodeMemo);
                        //#endregion 车站订单执行结果通知

                        StationOrder newStationOrder = new StationOrder();
                        newStationOrder.StationOrderId = Guid.NewGuid();
                        newStationOrder.TradeNo = String.Format("S{0}", TradeNoHelper.Instance.GetTradeNo());
                        newStationOrder.BuyTime = DateTime.Now;
                       
                        newStationOrder.OperationCode = String.Empty;
                        newStationOrder.CityCode = "020";
                        newStationOrder.DeviceId = "DeviceId";
                        newStationOrder.ChannelType = "04";
                        newStationOrder.PaymentVendor = "1002";
                        newStationOrder.PaymentCode = "130481063942121998";

                        newStationOrder.OriAFCStationCode = "0101";
                        newStationOrder.DesAFCStationCode = "0102";
                        newStationOrder.TicketPrice = 100;
                        newStationOrder.TicketNum = 1;
                        newStationOrder.SingleTicketType = "0";
                        newStationOrder.Discount = 1;
                        newStationOrder.Amount = newStationOrder.TicketPrice * newStationOrder.TicketNum;
                         newStationOrder.TransactionId = Guid.NewGuid().ToString();
                        newStationOrder.PayEndTime = DateTime.Now;
                        newStationOrder.PayEndTimeRaw = TimeHelper.GetTimeStringYyyyMMddHHmmss(DateTime.Now);
                        newStationOrder.ActualFee = newStationOrder.Amount;
                        newStationOrder.BankType = "TestBankType";
                        newStationOrder.UserAccount = "UserAccount";
                        newStationOrder.UserOpenId = "UserOpenId";

                        newStationOrder.Step = EnumHelper.GetStationOrderStepFlagString(StationOrderStep.StationOrderTakenRespond);
                        newStationOrder.OrderStatus = EnumHelper.GetOrderStatusTypeFlagString(OrderStatusType.TicketOut);
                        newStationOrder.IsValid = true;
                        newStationOrder.TicketTakeNum = newStationOrder.TicketNum;
                        newStationOrder.TicketTakeTime = DateTime.Now;
                        dbContext.StationOrders.AddObject(newStationOrder);

                        resultCount++;
                        _log.Debug(resultCount);
                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return resultCount;
        }
    }
}
