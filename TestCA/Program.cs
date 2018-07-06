using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.Util;
using PlatformLib.BLL;
using PlatformLib.Vo;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TestCA
{
    class Program
    {
        private static string strCityCode = "020";
        private static string strDeviceId = "WebLocalTest";
        private static string stationChannelType = "04";
        private static string webChannelType = "01";

        static void Main(string[] args)
        {
            //DateTime dtToday = new DateTime(2016,1,1);
            //DateTime dt1YearLater = dtToday.AddYears(1);
            //VoucherBo vb = new VoucherBo();
            //int addCount = vb.FillNewVoucher(100000, dtToday, dt1YearLater);
            //Console.WriteLine(addCount);

            //VoucherHelper vh = new VoucherHelper();
            //List<string> rl = vh.GetNoRepeatRandomNumList(1000, 30);
            //foreach (var e in rl)
            //{
            //    Console.WriteLine(e);
            //}
            //Console.WriteLine(rl.Count);

            //#region 获得最新票价
            //TicketPriceHepler tph = new TicketPriceHepler();
            //var pl = tph.GetLatestODTicketPriceList();
            //Console.WriteLine(pl.Count);
            //#endregion 获得最新票价


            //#region 获得内部订单号（商户号）

            #region 订单号唯一性测试（串行）

            BlockingCollection<string> tradeNoList = null;
            Dictionary<string, string> dictTradeNo = null;
            int testTradeNoCount = 1000;

            //Console.WriteLine("订单号唯一性测试（串行）");

            //tradeNoList = new BlockingCollection<string>();
            //dictTradeNo = new Dictionary<string, string>();

            //for (int i = 0; i < testTradeNoCount; i++)
            //{
            //    tradeNoList.Add(TradeNoHelper.Instance.GetTradeNo());
            //}

            //foreach (string eachTradeNo in tradeNoList)
            //{
            //    if (dictTradeNo.ContainsKey(eachTradeNo))
            //    {
            //        Console.WriteLine(String.Format("{0} more than one", eachTradeNo));
            //    }
            //    else
            //    {
            //        dictTradeNo.Add(eachTradeNo, eachTradeNo);
            //        Console.WriteLine(eachTradeNo);
            //    }
            //}

            //Console.WriteLine(tradeNoList.Count);
            //Console.WriteLine(dictTradeNo.Count);

            #endregion 订单号唯一性测试（串行）

            #region 订单号唯一性测试（并行）

            //Console.WriteLine("订单号唯一性测试（并行）");

            //tradeNoList = new BlockingCollection<string>();
            //dictTradeNo = new Dictionary<string, string>();

       
            //Parallel.For(0, testTradeNoCount, i =>
            //{
            //    TradeNoTest tradeNoTestInstance = new TradeNoTest();
            //    tradeNoList.Add(tradeNoTestInstance.GetTradeNo());

            //    //tradeNoList.Add(TradeNoHelper.Instance.GetTradeNo());

            //    i++;
            //});

            //foreach (string eachTradeNo in tradeNoList)
            //{
            //    if (dictTradeNo.ContainsKey(eachTradeNo))
            //    {
            //        Console.WriteLine(String.Format("{0} more than one", eachTradeNo));
            //    }
            //    else
            //    {
            //        dictTradeNo.Add(eachTradeNo, eachTradeNo);
            //        Console.WriteLine(eachTradeNo);
            //    }
            //}

            //Console.WriteLine(tradeNoList.Count);
            //Console.WriteLine(dictTradeNo.Count);

            #endregion 订单号唯一性测试（并行）

            ////string tradeNo1 = TradeNoHelper.Instance.GetTradeNo();
            ////Console.WriteLine(tradeNo1);
            ////string tradeNo2 = TradeNoHelper.Instance.GetTradeNo();
            ////Console.WriteLine(tradeNo2);




            //#endregion 获得内部订单号（商户号）


            StationOrderBo sbo = new StationOrderBo();
            string stationOrderNo = String.Empty;


            #region 现场车站购票（付款）订单提交
            string inputPaymentCode = "130057041971350303";
            StationOrderPayRequestVo newStationOrderPayRequestVo = new StationOrderPayRequestVo()
            {
                ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
                OperationCode = "S1-001",
                CityCode = strCityCode,
                DeviceId = strDeviceId,
                ChannelType = stationChannelType,
                expandAttribute = new List<string>(),
                paymentCode = inputPaymentCode,
                msisdn = String.Empty,
                iccid = String.Empty,
                serviceId = String.Empty,
                paymentVendor = "1002",
                pickupStationCode = "0101",
                getOffStationCode = "0102",
                singlelTicketPrice = 200,
                singlelTicketNum = 1,
                singleTicketType = "0"
            };
            Console.WriteLine("StationOrderPayRespondVo");
            //StationOrderPayRespondVo stationOrderPayRespondVo = sbo.OrderPay(newStationOrderPayRequestVo);
            //Console.WriteLine(stationOrderPayRespondVo.RespondCodeString);
            //Console.WriteLine(stationOrderPayRespondVo.partnerNo);
            //Console.WriteLine(stationOrderPayRespondVo.orderNo);
            //Console.WriteLine(stationOrderPayRespondVo.subject);
            //Console.WriteLine(stationOrderPayRespondVo.body);
            //Console.WriteLine(stationOrderPayRespondVo.payType);
            //Console.WriteLine(stationOrderPayRespondVo.amount);
            //Console.WriteLine(stationOrderPayRespondVo.account);
            //Console.WriteLine(stationOrderPayRespondVo.notifyUrl);
            //Console.WriteLine(stationOrderPayRespondVo.merchantCert);
            //Console.WriteLine(stationOrderPayRespondVo.timeout);
            //stationOrderNo = stationOrderPayRespondVo.orderNo;
            #endregion 现场车站购票（付款）订单提交


            #region 现场车站购票支付结果查询
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
            //Console.WriteLine("StationOrderPayResultRespondVo");
            //StationOrderPayResultRespondVo stationOrderPayResultRespondVo = sbo.PayResultQuery(newStationOrderPayResultRequestVo);
            //Console.WriteLine(stationOrderPayResultRespondVo.RespondCodeString);
            //Console.WriteLine(stationOrderPayResultRespondVo.paymentDateString);
            //Console.WriteLine(stationOrderPayResultRespondVo.amount);
            //Console.WriteLine(stationOrderPayResultRespondVo.paymentAccount);
            //Console.WriteLine(stationOrderPayResultRespondVo.paymentResult);
            //Console.WriteLine(stationOrderPayResultRespondVo.paymentDesc);
            ////// 重复调用测试
            ////Console.WriteLine("StationOrderPayResultRespondVo");
            ////stationOrderPayResultRespondVo = sbo.PayResultQuery(newStationOrderPayResultRequestVo);
            ////Console.WriteLine(stationOrderPayResultRespondVo.RespondCodeString);
            ////Console.WriteLine(stationOrderPayResultRespondVo.paymentDateString);
            ////Console.WriteLine(stationOrderPayResultRespondVo.amount);
            ////Console.WriteLine(stationOrderPayResultRespondVo.paymentAccount);
            ////Console.WriteLine(stationOrderPayResultRespondVo.paymentResult);
            ////Console.WriteLine(stationOrderPayResultRespondVo.paymentDesc);
            #endregion 现场车站购票支付结果查询

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

            //Console.WriteLine("WebOrderProcessRespondVo");
            //StationOrderProcessRespondVo stationOrderProcessRespondVo = sbo.StationOrderProcess(newStationOrderProcessRequestVo);
            //Console.WriteLine(stationOrderProcessRespondVo.RespondCodeString);
            //Console.WriteLine(stationOrderProcessRespondVo.respCodeMemo);
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
            //Console.WriteLine("WebOrderTakenRespondVo");
            //StationOrderTakenRespondVo stationOrderTakenRespondVo = sbo.StationOrderTaken(newStationOrderTakenRequestVo);
            //Console.WriteLine(stationOrderTakenRespondVo.RespondCodeString);
            //Console.WriteLine(stationOrderTakenRespondVo.respCodeMemo);
            //#endregion 车站订单执行结果通知

            //#region 车站订单执行故障通知
            ////stationOrderNo = "S20160122110434000000000000001";
            //StationOrderTakenErrRequestVo newStationOrderTakenErrRequestVo = new StationOrderTakenErrRequestVo()
            //{
            //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    operationCode = "S1-005",
            //    cityCode = strCityCode,
            //    DeviceId = strDeviceId,
            //    channelType = String.Empty,
            //    expandAttribute = null,
            //    orderNo = stationOrderNo,
            //    takeSingleTicketNum = 0.ToString(),
            //    faultOccurDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    faultSlipSeq = Guid.NewGuid().ToString().Substring(0, 32),
            //    errorCode = "errorCode",
            //    errorMessage = "errorMessage"
            //};

            //Console.WriteLine("StationOrderTakenErrRespondVo");
            //StationOrderTakenErrRespondVo stationOrderTakenErrRespondVo = sbo.StationOrderTakenErr(newStationOrderTakenErrRequestVo);
            //Console.WriteLine(stationOrderTakenErrRespondVo.RespondCodeString);
            //Console.WriteLine(stationOrderTakenErrRespondVo.respCodeMemo);
            //#endregion 车站订单执行故障通知

            #region 填充已支付网络订单

            StressTestHelper webOrderStressTestHelper = new StressTestHelper();


            //TicketTargetType targetTypeTVM = TicketTargetType.TVM;
            //int orderCountTVM = 20;
            //Console.WriteLine(String.Format("FillPayedWebOrder({0}, {1})", orderCountTVM, targetTypeTVM.ToString()));
            //List<WebPayResultRespondVo> resultRespTVMList = webOrderStressTestHelper.FillPayedWebOrder(orderCountTVM, targetTypeTVM);
            //Console.WriteLine(String.Format("resultCount:{0}", resultRespTVMList.Count));

            //TicketTargetType targetTypeAGM = TicketTargetType.APMGATE;
            //int orderCountAGM = 10;
            //Console.WriteLine(String.Format("FillPayedWebOrder({0}, {1})", orderCountAGM, targetTypeAGM.ToString()));
            //List<WebPayResultRespondVo> resultRespAGMList = webOrderStressTestHelper.FillPayedWebOrder(orderCountAGM, targetTypeAGM);
            //Console.WriteLine(String.Format("resultCount:{0}", resultRespAGMList.Count));

            #endregion 填充已支付网络订单


            WebPreOrderBo wbo = new WebPreOrderBo();
            string webTradeNo = String.Empty;
            string webExternalTradeNo = Guid.NewGuid().ToString();

            string strUserOpenId = "UserOpenId";
            string strUserAccount = "UserAccount";

            int amount = 0;

            #region 获得 商家订单号
            

            
            //WebTradeNoRequestVo newWebTradeNoRequestVo = new WebTradeNoRequestVo()
            //    {
            //        ExternalTradeNo = webExternalTradeNo,
            //        BuyTime = DateTime.Now,
            //        OperationCode = String.Empty,
            //        CityCode = String.Empty,
            //        DeviceId = String.Empty,
            //        ChannelType = String.Empty,
            //        PaymentVendor = String.Empty,
            //        OriAFCStationCode = "6804",
            //        DesAFCStationCode = "6804",
            //        TicketPrice = 200,
            //        TicketNum = 1,
            //        Discount = 1,
            //        Amount = amount,
            //        TicketTarget = TicketTargetType.APMGATE,
            //        UserOpenId = strUserOpenId,
            //        UserAccount = strUserAccount
            //    };
            //amount = Convert.ToInt32(newWebTradeNoRequestVo.TicketPrice) * newWebTradeNoRequestVo.TicketNum;

            //Console.WriteLine("WebTradeNoRespondVo");
            //WebTradeNoRespondVo webTradeNoRespondVo = wbo.GetTradeNo(newWebTradeNoRequestVo);
            //Console.WriteLine(webTradeNoRespondVo.IsSuccess);
            //Console.WriteLine(webTradeNoRespondVo.WebOrderId);
            //Console.WriteLine(webTradeNoRespondVo.TradeNo);
            //Console.WriteLine(webTradeNoRespondVo.IsVaild);
            //Console.WriteLine(webTradeNoRespondVo.StepStatus);

            //webTradeNo = webTradeNoRespondVo.TradeNo;

            #endregion 获得 商家订单号

            #region 记录 预支付交易会话标识

            //string testPrepayId = Guid.NewGuid().ToString();
            //WebPrePayRequestVo webPrePayRequestVo = new WebPrePayRequestVo();
            //webPrePayRequestVo.WebOrderId = null;
            //webPrePayRequestVo.TradeNo = webTradeNo;
            //webPrePayRequestVo.PrepayId = testPrepayId;

            //Console.WriteLine("WebPrePayRespondVo");
            //WebPrePayRespondVo webPrePayRespondVo = wbo.PrePayRecord(webPrePayRequestVo);
            //Console.WriteLine(webPrePayRespondVo.IsSuccess);
            //Console.WriteLine(webPrePayRespondVo.WebOrderId);
            //Console.WriteLine(webPrePayRespondVo.TradeNo);
            //Console.WriteLine(webPrePayRespondVo.ExternalTradeNo);
            //Console.WriteLine(webPrePayRespondVo.StepStatus);

            #endregion 记录 预支付交易会话标识


            #region 支付异步结果记录
            
            //WebPayResultRequestVo webPayResultRequestVo = new WebPayResultRequestVo();
            //webPayResultRequestVo.TradeNo = webTradeNo;
            //webPayResultRequestVo.TransactionId = Guid.NewGuid().ToString();
            //webPayResultRequestVo.PayEndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //webPayResultRequestVo.BankType = "BankType";
            //webPayResultRequestVo.ActualFee = amount;
            //webPayResultRequestVo.ErrCodeDes = String.Empty;
            //webPayResultRequestVo.IsSuccess = true;
            //webPayResultRequestVo.UserOpenId = strUserOpenId;
            //webPayResultRequestVo.UserAccount = strUserAccount;

            //Console.WriteLine("WebPayResultRequestVo");
            //WebPayResultRespondVo webPayResultRespondVo = wbo.PayResultRecord(webPayResultRequestVo);
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

            #region 获得 2016六一嘉年华订单


            //WebTradeNoRequestVo newWebTradeNoRequestVo20160601 = new WebTradeNoRequestVo()
            //    {
            //        ExternalTradeNo = webExternalTradeNo,
            //        BuyTime = DateTime.Now,
            //        OperationCode = String.Empty,
            //        CityCode = String.Empty,
            //        DeviceId = String.Empty,                    
            //        ChannelType = String.Empty,
            //        // 固定不能变字段
            //        PaymentVendor = "1000",
            //        OriAFCStationCode = "6809",
            //        DesAFCStationCode = "6809",
            //        // 固定不能变字段
            //        TicketPrice = 200,
            //        // 固定不能变字段
            //        TicketNum = 1,
            //        // 固定不能变字段
            //        Discount = 0,
            //        // 固定不能变字段
            //        Amount = 200,
            //        // 固定不能变字段
            //        TicketTarget = TicketTargetType.APMGATE,
            //        UserOpenId = strUserOpenId,
            //        UserAccount = strUserAccount
            //    };
            //Console.WriteLine("WebPayResultRequestVo");
            //WebPayResultRespondVo webPayResultRespondVo201601010 = wbo.FreeAPMTicketFor20160601(newWebTradeNoRequestVo20160601);
            //Console.WriteLine(webPayResultRespondVo201601010.IsSuccess);
            //Console.WriteLine(webPayResultRespondVo201601010.WebOrderId);
            //Console.WriteLine(webPayResultRespondVo201601010.TradeNo);
            //Console.WriteLine(webPayResultRespondVo201601010.ExternalTradeNo);
            //Console.WriteLine(webPayResultRespondVo201601010.OriAFCStationCode);
            //Console.WriteLine(webPayResultRespondVo201601010.DesAFCStationCode);
            //Console.WriteLine(webPayResultRespondVo201601010.TicketPrice);
            //Console.WriteLine(webPayResultRespondVo201601010.TicketNum);
            //Console.WriteLine(webPayResultRespondVo201601010.Discount);
            //Console.WriteLine(webPayResultRespondVo201601010.PayEndTime);
            //Console.WriteLine(webPayResultRespondVo201601010.TicketTarget.ToString());
            //Console.WriteLine(webPayResultRespondVo201601010.Voucher);
            //Console.WriteLine(webPayResultRespondVo201601010.StepStatus);

            //webTradeNo = webPayResultRespondVo201601010.TradeNo;

            #endregion 获得 2016六一嘉年华订单

            #region 网络订单查询

            //WebOrderRequestVo webOrderRequestVo = new WebOrderRequestVo();
            //webOrderRequestVo.TradeNo = webTradeNo;
            ////webOrderRequestVo.UserOpenId = strUserOpenId;

            //Console.WriteLine("WebOrderRespondVo");
            //WebOrderRespondVo webOrderRespondVo = wbo.WebOrderQuery(webOrderRequestVo);
            //Console.WriteLine(webOrderRespondVo.TradeNo);
            //Console.WriteLine(webOrderRespondVo.ExternalTradeNo);
            //Console.WriteLine(webOrderRespondVo.BuyTime);
            //Console.WriteLine(webOrderRespondVo.IsWebOrderVaild);
            //Console.WriteLine(webOrderRespondVo.PaymentVendor);
            //Console.WriteLine(webOrderRespondVo.OriAFCStationCode);
            //Console.WriteLine(webOrderRespondVo.DesAFCStationCode);
            //Console.WriteLine(webOrderRespondVo.OriStationChineseName);
            //Console.WriteLine(webOrderRespondVo.DesStationChineseName);
            //Console.WriteLine(webOrderRespondVo.OriStationEnglishName);
            //Console.WriteLine(webOrderRespondVo.DesStationEnglishName);
            //Console.WriteLine(webOrderRespondVo.TicketPrice);
            //Console.WriteLine(webOrderRespondVo.TicketNum);
            //Console.WriteLine(webOrderRespondVo.Discount);
            //Console.WriteLine(webOrderRespondVo.ActualFee);
            //Console.WriteLine(webOrderRespondVo.TicketTarget.ToString());
            //Console.WriteLine(webOrderRespondVo.TransactionId);
            //Console.WriteLine(webOrderRespondVo.PayEndTime);
            //Console.WriteLine(webOrderRespondVo.ExpiryTime);
            //Console.WriteLine(webOrderRespondVo.Voucher);
            //Console.WriteLine(webOrderRespondVo.IsUsed);
            //Console.WriteLine(webOrderRespondVo.UsedTime);
            //Console.WriteLine(webOrderRespondVo.TicketTakeNum);
            //Console.WriteLine(webOrderRespondVo.TicketTakeTime);
            //Console.WriteLine(webOrderRespondVo.OrderStatus.ToString());
            //Console.WriteLine(webOrderRespondVo.OrderStep.ToString());
            
            #endregion 网络订单查询

            #region 网络订单列表查询

            //DateTime payTimeStart = new DateTime(2015, 1, 1);
            //DateTime payTimeEnd = new DateTime(2017, 1, 1);
            //string webOrderListQueryPaymentVendor = "1001";
            //OrderStatusType webOrderListQueryOrderStatusType = OrderStatusType.TicketException;
            //List<WebOrderRespondVo> webOrderRespondList = wbo.WebOrderListQuery(payTimeStart, payTimeEnd, webOrderListQueryPaymentVendor, webOrderListQueryOrderStatusType);
            //Console.WriteLine("WebOrderRespondVoList");
            //foreach (WebOrderRespondVo eachWebOrderRespondVo in webOrderRespondList)
            //{
            //    Console.WriteLine(eachWebOrderRespondVo.TradeNo);
            //    Console.WriteLine(eachWebOrderRespondVo.ExternalTradeNo);
            //    Console.WriteLine(eachWebOrderRespondVo.IsWebOrderVaild);
            //    Console.WriteLine(eachWebOrderRespondVo.PaymentVendor);
            //    Console.WriteLine(eachWebOrderRespondVo.OriAFCStationCode);
            //    Console.WriteLine(eachWebOrderRespondVo.DesAFCStationCode);
            //    Console.WriteLine(eachWebOrderRespondVo.OriStationChineseName);
            //    Console.WriteLine(eachWebOrderRespondVo.DesStationChineseName);
            //    Console.WriteLine(eachWebOrderRespondVo.OriStationEnglishName);
            //    Console.WriteLine(eachWebOrderRespondVo.DesStationEnglishName);
            //    Console.WriteLine(eachWebOrderRespondVo.TicketPrice);
            //    Console.WriteLine(eachWebOrderRespondVo.TicketNum);
            //    Console.WriteLine(eachWebOrderRespondVo.Discount);
            //    Console.WriteLine(eachWebOrderRespondVo.ActualFee);
            //    Console.WriteLine(eachWebOrderRespondVo.TicketTarget.ToString());
            //    Console.WriteLine(eachWebOrderRespondVo.TransactionId);
            //    Console.WriteLine(eachWebOrderRespondVo.ExpiryTime.ToString("yyyyMMddHHmmss"));
            //    Console.WriteLine(eachWebOrderRespondVo.Voucher);
            //    Console.WriteLine(eachWebOrderRespondVo.IsUsed);
            //    Console.WriteLine(eachWebOrderRespondVo.UsedTime);
            //    Console.WriteLine(eachWebOrderRespondVo.TicketTakeNum);
            //    Console.WriteLine(eachWebOrderRespondVo.TicketTakeTime);
            //    Console.WriteLine(eachWebOrderRespondVo.OrderStatus.ToString());
            //    Console.WriteLine(eachWebOrderRespondVo.OrderStep.ToString());
            //}

            #endregion 网络订单列表查询

            /* * 
             * 
             * 

            string urlr = "http://172.20.27.16/MobilePay/index.aspx";
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(urlr); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            Console.WriteLine(sb);
            Console.Read();*/

            //#region 信息服务部网络支付结果记录
            //WebPreOrderBo bo = new WebPreOrderBo();
            //ITPayResultRequestVo itPayResultRequestVo = new ITPayResultRequestVo();
            //itPayResultRequestVo.TradeNo = TradeNoHelper.Instance.GetTradeNo();
            //itPayResultRequestVo.TransactionId = Guid.NewGuid().ToString().Substring(0, 30);
            //itPayResultRequestVo.UserOpenId = "UserOpenId";
            //itPayResultRequestVo.OriAFCStationCode = "0101";
            //itPayResultRequestVo.DesAFCStationCode = "0102";
            //itPayResultRequestVo.TicketPrice = "200";
            //itPayResultRequestVo.TicketNum = "2";
            //itPayResultRequestVo.ActualFee = "400";
            //itPayResultRequestVo.PayEndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //itPayResultRequestVo.PayOperator = "weixin";
            //itPayResultRequestVo.BankType = "BankType";
            //itPayResultRequestVo.Target = "TVM";
            //itPayResultRequestVo.ErrCodeDes = String.Empty;

            //Console.WriteLine("ITPayResultRespondVo");
            //ITPayResultRespondVo itPayResultRespondVo = bo.ITPayResultRecord(itPayResultRequestVo);
            //Console.WriteLine(itPayResultRespondVo.TradeNo);
            //Console.WriteLine(itPayResultRespondVo.Voucher);
            //Console.WriteLine(itPayResultRespondVo.ErrStatus);
            //#endregion 信息服务部网络支付结果记录




            #region 网络订单出票认证
            //WebOrderVerifyRequestVo newWebOrderVerifyRequestVo = new WebOrderVerifyRequestVo()
            //{
            //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    operationCode = "S1-006",
            //    cityCode = strCityCode,
            //    DeviceId = strDeviceId,
            //    channelType = stationChannelType,
            //    expandAttribute = null,
            //    orderNo = webTradeNo,
            //    orderToken = webOrderRespondVo.Voucher
            //};

            //Console.WriteLine("WebOrderVerifyRespondVo");
            //WebOrderVerifyRespondVo webOrderVerifyRespondVo = wbo.WebOrderVerify(newWebOrderVerifyRequestVo);
            //Console.WriteLine(webOrderVerifyRespondVo.RespondCodeString);
            //Console.WriteLine(webOrderVerifyRespondVo.respCodeMemo);
            //Console.WriteLine(webOrderVerifyRespondVo.orderNo);
            //Console.WriteLine(webOrderVerifyRespondVo.userMsisdn);
            //Console.WriteLine(webOrderVerifyRespondVo.pickupStationCode);
            //Console.WriteLine(webOrderVerifyRespondVo.getOffStationCode);
            //Console.WriteLine(webOrderVerifyRespondVo.singlelTicketPrice);
            //Console.WriteLine(webOrderVerifyRespondVo.singleTicketNum);
            //Console.WriteLine(webOrderVerifyRespondVo.singleTicketType);
            #endregion 网络订单出票认证

            #region 网络订单执行结果通知
            //WebOrderProcessRequestVo newWebOrderProcessRequestVo = new WebOrderProcessRequestVo()
            //{
            //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    operationCode = "S1-003",
            //    cityCode = strCityCode,
            //    DeviceId = strDeviceId,
            //    channelType = stationChannelType,
            //    expandAttribute = null,
            //    orderNo = webTradeNo
            //};

            //Console.WriteLine("WebOrderProcessRespondVo");
            //WebOrderProcessRespondVo webOrderProcessRespondVo = wbo.WebOrderProcess(newWebOrderProcessRequestVo);
            //Console.WriteLine(webOrderProcessRespondVo.RespondCodeString);
            //Console.WriteLine(webOrderProcessRespondVo.respCodeMemo);
            #endregion 网络订单执行结果通知

            #region 网络订执行结果通知
            //WebOrderTakenRequestVo newWebOrderTakenRequestVo = new WebOrderTakenRequestVo()
            //{
            //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    operationCode = "S1-004",
            //    cityCode = strCityCode,
            //    DeviceId = strDeviceId,
            //    channelType = stationChannelType,
            //    expandAttribute = null,
            //    orderNo = webTradeNo,
            //    takeSingleTicketNum = webOrderRespondVo.TicketNum.ToString(),
            //    takeSingleTicketDateString = DateTime.Now.ToString("yyyyMMddHHmmss")
            //};

            //Console.WriteLine("WebOrderTakenRespondVo");
            //WebOrderTakenRespondVo webOrderTakenRespondVo = wbo.WebOrderTaken(newWebOrderTakenRequestVo);
            //Console.WriteLine(webOrderTakenRespondVo.RespondCodeString);
            //Console.WriteLine(webOrderTakenRespondVo.respCodeMemo);
            #endregion 网络订执行结果通知

            //#region 网络订单执行故障通知
            //WebOrderTakenErrRequestVo newWebOrderTakenErrRequestVo = new WebOrderTakenErrRequestVo()
            //{
            //    ReqSysDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    operationCode = "S1-005",
            //    cityCode = strCityCode,
            //    DeviceId = strDeviceId,
            //    channelType = stationChannelType,
            //    expandAttribute = null,
            //    orderNo = webTradeNo,
            //    takeSingleTicketNum = (webOrderRespondVo.TicketNum - 1).ToString(),
            //    faultOccurDateString = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    faultSlipSeq = Guid.NewGuid().ToString().Substring(0, 32),
            //    errorCode = "errorCode",
            //    errorMessage = "errorMessage"
            //};

            //Console.WriteLine("WebOrderTakenErrRespondVo");
            //WebOrderTakenErrRespondVo webOrderTakenErrRespondVo = wbo.WebOrderTakenErr(newWebOrderTakenErrRequestVo);
            //Console.WriteLine(webOrderTakenErrRespondVo.RespondCodeString);
            //Console.WriteLine(webOrderTakenErrRespondVo.respCodeMemo);
            //#endregion 网络订单执行故障通知

            //#region 网络订单退款记录请求
            //WebOrderRefundRecordRequestVo newWebOrderRefundRecordRequestVo = new WebOrderRefundRecordRequestVo()
            //{
            //    TradeNo = webTradeNo,
            //    ExternalTradeNo = webExternalTradeNo,
            //    RefundTradeNo = webTradeNo,
            //    RefundReason = "RefundReason",
            //    RefundFee = 50,
            //    RequestTime = DateTime.Now,
            //    IsRequestSuccess = true,
            //    RequestErrCodeDes = "RequestErrCodeDes",
            //    IsRespondSuccess = false,
            //    RespondTime = null,
            //    RespondErrCodeDes = "RespondErrCodeDes",
            //    OrderStatus = OrderStatusType.RefundProcessing
            //};

            //Console.WriteLine("WebOrderRefundRecordRespondVo");
            //WebOrderRefundRecordRespondVo webOrderRefundRecordRespondVo = wbo.WebOrderRefundRecord(newWebOrderRefundRecordRequestVo);
            //Console.WriteLine(webOrderRefundRecordRespondVo.IsSuccess);
            //Console.WriteLine(webOrderRefundRecordRespondVo.OrderStatus.ToString());
            //Console.WriteLine(webOrderRefundRecordRespondVo.IsTradeNoValid);
            //#endregion 网络订单退款记录请求

            //#region 网络订单退款记录请求
            //WebOrderRefundUpdateRequestVo newWebOrderRefundUpdateRequestVo = new WebOrderRefundUpdateRequestVo()
            //{
            //    TradeNo = webTradeNo,
            //    ExternalTradeNo = webExternalTradeNo,
            //    RefundTradeNo = webTradeNo,
            //    IsRespondSuccess = false,
            //    RespondTime = DateTime.Now,
            //    RespondErrCodeDes = "RespondErrCodeDes",
            //    OrderStatus = OrderStatusType.RefundFail
            //};

            //Console.WriteLine("WebOrderRefundUpdateRespondVo");
            //WebOrderRefundUpdateRespondVo webOrderRefundUpdateRespondVo = wbo.WebOrderRefundUpdate(newWebOrderRefundUpdateRequestVo);
            //Console.WriteLine(webOrderRefundUpdateRespondVo.IsSuccess);
            //Console.WriteLine(webOrderRefundUpdateRespondVo.IsTradeNoValid);
            //Console.WriteLine(webOrderRefundUpdateRespondVo.IsOrderStatusValid);
            //Console.WriteLine(webOrderRefundUpdateRespondVo.OrderStatus.ToString());
            //#endregion 网络订单退款记录请求

            //#region 网络订单退款记录查询
            //WebOrderRefundResultRequestVo newWebOrderRefundResultRequestVo = new WebOrderRefundResultRequestVo()
            //{
            //    TradeNo = webTradeNo,
            //    ExternalTradeNo = webExternalTradeNo,
            //    RefundTradeNo = webTradeNo
            //};
            //Console.WriteLine("WebOrderRefundResultRespondVo");
            //WebOrderRefundResultRespondVo webOrderRefundResultRespondVo = wbo.WebOrderRefundQuery(newWebOrderRefundResultRequestVo);
            //Console.WriteLine(webOrderRefundResultRespondVo.IsTradeNoValid);
            //Console.WriteLine(webOrderRefundResultRespondVo.TradeNo);
            //Console.WriteLine(webOrderRefundResultRespondVo.ExternalTradeNo);
            //Console.WriteLine(webOrderRefundResultRespondVo.RefundTradeNo);
            //Console.WriteLine(webOrderRefundResultRespondVo.RefundReason);
            //Console.WriteLine(webOrderRefundResultRespondVo.PaymentVendor);
            //Console.WriteLine(webOrderRefundResultRespondVo.RefundFee);
            //Console.WriteLine(webOrderRefundResultRespondVo.TotalFee);
            //Console.WriteLine(webOrderRefundResultRespondVo.BankType);
            //Console.WriteLine(webOrderRefundResultRespondVo.RequestTime);
            //Console.WriteLine(webOrderRefundResultRespondVo.IsRequestSuccess);
            //Console.WriteLine(webOrderRefundResultRespondVo.RequestErrCodeDes);
            //Console.WriteLine(webOrderRefundResultRespondVo.IsRespondSuccess);
            //Console.WriteLine(webOrderRefundResultRespondVo.RespondTime);
            //Console.WriteLine(webOrderRefundResultRespondVo.RespondErrCodeDes);
            //Console.WriteLine(webOrderRefundResultRespondVo.OrderStatus.ToString());
            //#endregion 网络订单退款记录查询

            //#region 网络订单退款记录查询
            //string paymentVendor1 = "1001";
            //OrderStatusType orderStatus1 = OrderStatusType.RefundFail;
            //Console.WriteLine("WebOrderRefundResultRespondList");
            //List<WebOrderRefundResultRespondVo> webOrderRefundResultRespondList = wbo.WebOrderRefundListQuery(paymentVendor1, orderStatus1);
            //foreach (WebOrderRefundResultRespondVo eachWebOrderRefundResultRespondVo in webOrderRefundResultRespondList)
            //{
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.IsTradeNoValid);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.TradeNo);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.ExternalTradeNo);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.RefundTradeNo);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.RefundReason);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.PaymentVendor);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.RefundFee);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.TotalFee);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.BankType);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.RequestTime);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.IsRequestSuccess);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.RequestErrCodeDes);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.IsRespondSuccess);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.RespondTime);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.RespondErrCodeDes);
            //    Console.WriteLine(eachWebOrderRefundResultRespondVo.OrderStatus.ToString());
            //}
            //#endregion 网络订单退款记录查询

            //#region 获得订单过期时间
            //DateTime dtOrderExpiryTime = wbo.GetOrderExpiryTime(DateTime.Now);
            //Console.WriteLine(dtOrderExpiryTime);
            //#endregion 获得订单过期时间

            #region 填充已取票车站订单

            StressTestHelper staitonOrderStressTestHelper = new StressTestHelper();

            //for (int time = 1; time <= 100; time++)
            //{
            //    int stationOrderCountTVM = 10000;
            //    Console.WriteLine(String.Format("FillCompleteedStationOrder({0})", stationOrderCountTVM));
            //    int resultCount = staitonOrderStressTestHelper.FillCompleteedStationOrder(stationOrderCountTVM);
            //    Console.WriteLine(String.Format("resultCount:{0}", resultCount));
            //}

            #endregion 填充已取票车站订单

            //#region 通用订单查询
            //CommonOrderBo commonOrderBo = new CommonOrderBo();
            //DateTime dtFromTime = new DateTime(2016,1,28,9,0,0);
            //DateTime dtToTime = new DateTime(2016, 1, 28, 10, 20, 0);
            //string commonOrderQueryPaymentVendor = null;
            //List<CommonOrderVo> commonOrderList = commonOrderBo.CommonOrderQuery(false, dtFromTime, dtToTime, commonOrderQueryPaymentVendor, OrderType.StationOrder, TicketTargetType.TVM);
            //Console.WriteLine("CommonOrderList");
            //foreach (CommonOrderVo eachCommonOrderVo in commonOrderList)
            //{
            //    Console.WriteLine(eachCommonOrderVo.TicketOrderType.ToString());
            //    Console.WriteLine(eachCommonOrderVo.TradeNo);
            //    Console.WriteLine(TimeHelper.GetTimeStringYyyyMMddHHmmss( eachCommonOrderVo.BuyTime));
            //    Console.WriteLine(eachCommonOrderVo.TicketPrice);
            //    Console.WriteLine(eachCommonOrderVo.TicketNum);
            //    Console.WriteLine(eachCommonOrderVo.Discount);
            //    Console.WriteLine(eachCommonOrderVo.ActualFee);
            //    Console.WriteLine(eachCommonOrderVo.PaymentVendor);
            //    Console.WriteLine(eachCommonOrderVo.OriAFCStationCode);
            //    Console.WriteLine(eachCommonOrderVo.OriAFCStationCode);
            //    Console.WriteLine(eachCommonOrderVo.OriStationChineseName);
            //    Console.WriteLine(eachCommonOrderVo.DesStationChineseName);
            //    Console.WriteLine(eachCommonOrderVo.TicketTarget.ToString());
            //    Console.WriteLine(eachCommonOrderVo.IsUsed);
            //    Console.WriteLine(eachCommonOrderVo.DeviceId);
            //    Console.WriteLine(eachCommonOrderVo.UseTime);
            //}
            //#endregion 通用订单查询
        }

        public static void PrintObjectAllMembers(Object obj)
        {
            if (null != obj)
            {
                PropertyInfo[] propertys = obj.GetType().GetProperties();
                foreach (PropertyInfo pinfo in propertys)
                {
                    Console.WriteLine(String.Format("{0}:{1}", pinfo.Name, pinfo.GetValue(obj, null)));
                }
            }
        }
    }
}
