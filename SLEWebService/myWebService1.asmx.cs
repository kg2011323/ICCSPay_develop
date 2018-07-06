using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using System.Reflection;
using SLEWebService.Vo;
using System.Diagnostics;
using SLEWebService.Util;
using PlatformLib.Vo;
using PlatformLib.BLL;

namespace SLEWebService
{
    /// <summary>
    /// Summary description for myWebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class myWebService1 : System.Web.Services.WebService
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获得请求IP
        /// </summary>
        private string RequestIP
        {
            get
            {
                string strRequestIP = String.Empty;
                try
                {
                    strRequestIP = this.Context.Request.UserHostAddress;
                }
                catch (Exception)
                { }
                return strRequestIP;
            }
        }


        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderCommitReq"></param>
        /// <returns></returns>
        [WebMethod]
        public OrderCommitResp S1_001(OrderCommitReq orderCommitReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                string strOCReq = LogHelper.GetObjectMemberString(orderCommitReq);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOCReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            OrderCommitResp ocResponce = new OrderCommitResp();
            try
            {
                StationOrderPayRequestVo stationOrderPayRequestVo = new StationOrderPayRequestVo();
                stationOrderPayRequestVo.ReqSysDateString = orderCommitReq.reqSysDate;
                stationOrderPayRequestVo.operationCode = orderCommitReq.operationCode;
                stationOrderPayRequestVo.cityCode = orderCommitReq.cityCode;
                stationOrderPayRequestVo.DeviceId = orderCommitReq.deviceId;
                stationOrderPayRequestVo.channelType = orderCommitReq.channelType;
                stationOrderPayRequestVo.expandAttribute = orderCommitReq.expandAttribute;
                stationOrderPayRequestVo.paymentCode = orderCommitReq.paymentCode;
                stationOrderPayRequestVo.msisdn = orderCommitReq.msisdn;
                stationOrderPayRequestVo.iccid = orderCommitReq.iccid;
                stationOrderPayRequestVo.serviceId = orderCommitReq.serviceId;
                stationOrderPayRequestVo.paymentVendor = orderCommitReq.paymentVendor;
                stationOrderPayRequestVo.pickupStationCode = orderCommitReq.pickupStationCode;
                stationOrderPayRequestVo.getOffStationCode = orderCommitReq.getOffStationCode;
                stationOrderPayRequestVo.singlelTicketPrice = Convert.ToInt32(orderCommitReq.singleTicketPrice);
                stationOrderPayRequestVo.singlelTicketNum = Convert.ToInt32(orderCommitReq.singleTicketNum);
                stationOrderPayRequestVo.singleTicketType = orderCommitReq.singleTicketType;

                StationOrderBo bo = new StationOrderBo();
                StationOrderPayRespondVo stationOrderRespondVo = bo.OrderPay(stationOrderPayRequestVo);
                ocResponce.respCode = stationOrderRespondVo.RespondCodeString;
                ocResponce.respCodeMemo = stationOrderRespondVo.respCodeMemo;
                ocResponce.expandAttribute = stationOrderRespondVo.expandAttribute;
                ocResponce.partnerNo = stationOrderRespondVo.partnerNo;
                ocResponce.orderNo = stationOrderRespondVo.orderNo;
                ocResponce.subject = stationOrderRespondVo.subject;
                ocResponce.body = stationOrderRespondVo.body;
                ocResponce.payType = stationOrderRespondVo.payType;
                ocResponce.amount = stationOrderRespondVo.amount;
                ocResponce.account = stationOrderRespondVo.account;
                ocResponce.notifyUrl = stationOrderRespondVo.notifyUrl;
                ocResponce.merchantCert = stationOrderRespondVo.merchantCert;
                ocResponce.timeout = stationOrderRespondVo.timeout;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            try
            {
                string strOCResp = LogHelper.GetObjectMemberString(ocResponce);
                string strLog = String.Format("Resp:{{{0}}}", strOCResp);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return ocResponce;
        }

        /// <summary>
        /// 支付结果查询
        /// </summary>
        /// <param name="queryPaymentResultReq"></param>
        /// <returns></returns>
        [WebMethod]
        public QueryPaymentResultResp S1_002(QueryPaymentResultReq queryPaymentResultReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                string strQPRReq = LogHelper.GetObjectMemberString(queryPaymentResultReq);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strQPRReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            QueryPaymentResultResp qprResponce = new QueryPaymentResultResp();
            try
            {
                StationOrderPayResultRequestVo stationOrderPayResultRequestVo = new StationOrderPayResultRequestVo();
                stationOrderPayResultRequestVo.ReqSysDateString = queryPaymentResultReq.reqSysDate;
                stationOrderPayResultRequestVo.operationCode = queryPaymentResultReq.operationCode;
                stationOrderPayResultRequestVo.cityCode = queryPaymentResultReq.cityCode;
                stationOrderPayResultRequestVo.DeviceId = queryPaymentResultReq.deviceId;
                stationOrderPayResultRequestVo.channelType = queryPaymentResultReq.channelType;
                stationOrderPayResultRequestVo.expandAttribute = queryPaymentResultReq.expandAttribute;
                stationOrderPayResultRequestVo.orderNo = queryPaymentResultReq.orderNo;

                StationOrderBo bo = new StationOrderBo();

                StationOrderPayResultRespondVo stationOrderPayResultRespondVo = bo.PayResultQuery(stationOrderPayResultRequestVo);
                qprResponce.respCode = stationOrderPayResultRespondVo.RespondCodeString;
                qprResponce.respCodeMemo = stationOrderPayResultRespondVo.respCodeMemo;
                qprResponce.paymentDate = stationOrderPayResultRespondVo.paymentDateString;
                qprResponce.amount = stationOrderPayResultRespondVo.amount.ToString();
                qprResponce.paymentAccount = stationOrderPayResultRespondVo.paymentAccount;
                qprResponce.paymentResult = stationOrderPayResultRespondVo.paymentResult;
                qprResponce.paymentDesc = stationOrderPayResultRespondVo.paymentDesc;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            try
            {
                string strQPRResp = LogHelper.GetObjectMemberString(qprResponce);
                string strLog = String.Format("Resp:{{{0}}}", strQPRResp);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return qprResponce;
        }

        /// <summary>
        /// 出票认证
        /// </summary>
        /// <param name="ticketVerificationReq"></param>
        /// <returns></returns>
        [WebMethod]
        public TicketVerificationResp S1_006(TicketVerificationReq ticketVerificationReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                string strTVReq = LogHelper.GetObjectMemberString(ticketVerificationReq);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strTVReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            TicketVerificationResp tvResponce = new TicketVerificationResp();
            try
            {
                WebOrderVerifyRequestVo newWebOrderVerifyRequestVo = new WebOrderVerifyRequestVo();
                newWebOrderVerifyRequestVo.ReqSysDateString = ticketVerificationReq.reqSysDate;
                newWebOrderVerifyRequestVo.operationCode = ticketVerificationReq.operationCode;
                newWebOrderVerifyRequestVo.cityCode = ticketVerificationReq.cityCode;
                newWebOrderVerifyRequestVo.DeviceId = ticketVerificationReq.deviceId;
                newWebOrderVerifyRequestVo.channelType = ticketVerificationReq.channelType;
                newWebOrderVerifyRequestVo.expandAttribute = ticketVerificationReq.expandAttribute;
                newWebOrderVerifyRequestVo.orderNo = ticketVerificationReq.orderNo;
                newWebOrderVerifyRequestVo.orderToken = ticketVerificationReq.orderToken;

                WebPreOrderBo bo = new WebPreOrderBo();
                WebOrderVerifyRespondVo webOrderVerifyRespondVo = bo.WebOrderVerify(newWebOrderVerifyRequestVo);
                tvResponce.respCode = webOrderVerifyRespondVo.RespondCodeString;
                tvResponce.respCodeMemo = webOrderVerifyRespondVo.respCodeMemo;
                tvResponce.expandAttribute = webOrderVerifyRespondVo.expandAttribute;
                tvResponce.orderNo = webOrderVerifyRespondVo.orderNo;
                tvResponce.userMsisdn = webOrderVerifyRespondVo.userMsisdn;
                tvResponce.pickupStationCode = webOrderVerifyRespondVo.pickupStationCode;
                tvResponce.getOffStationCode = webOrderVerifyRespondVo.getOffStationCode;
                tvResponce.singleTicketPrice = webOrderVerifyRespondVo.singlelTicketPrice;
                tvResponce.singleTicketNum = webOrderVerifyRespondVo.singleTicketNum;
                tvResponce.singleTicketType = webOrderVerifyRespondVo.singleTicketType;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            try
            {
                string strObjectMemberString = LogHelper.GetObjectMemberString(tvResponce);
                string strLog = String.Format("Resp:{{{0}}}", strObjectMemberString);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return tvResponce;
        }

        /// <summary>
        /// 订单执行开始
        /// </summary>
        /// <param name="orderExecuteBeginReq"></param>
        /// <returns></returns>
        [WebMethod]
        public OrderExecuteBeginResp S1_003(OrderExecuteBeginReq orderExecuteBeginReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                string strOEBReq = LogHelper.GetObjectMemberString(orderExecuteBeginReq);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOEBReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            OrderExecuteBeginResp oebResponce = new OrderExecuteBeginResp();
            oebResponce.respCode = "0004";
            try
            {
                if (orderExecuteBeginReq.orderNo.StartsWith("w") || orderExecuteBeginReq.orderNo.StartsWith("W"))  //网络购票
                {
                    WebOrderProcessRequestVo newWebOrderProcessRequestVo = new WebOrderProcessRequestVo();
                    newWebOrderProcessRequestVo.ReqSysDateString = orderExecuteBeginReq.reqSysDate;
                    newWebOrderProcessRequestVo.operationCode = orderExecuteBeginReq.operationCode;
                    newWebOrderProcessRequestVo.cityCode = orderExecuteBeginReq.cityCode;
                    newWebOrderProcessRequestVo.DeviceId = orderExecuteBeginReq.deviceId;
                    newWebOrderProcessRequestVo.channelType = orderExecuteBeginReq.channelType;
                    newWebOrderProcessRequestVo.expandAttribute = orderExecuteBeginReq.expandAttribute;
                    newWebOrderProcessRequestVo.orderNo = orderExecuteBeginReq.orderNo;

                    WebPreOrderBo bo = new WebPreOrderBo();
                    WebOrderProcessRespondVo webOrderProcessRespondVo = bo.WebOrderProcess(newWebOrderProcessRequestVo);
                    oebResponce.respCode = webOrderProcessRespondVo.RespondCodeString;
                    oebResponce.respCodeMemo = webOrderProcessRespondVo.respCodeMemo;
                }
                else if (orderExecuteBeginReq.orderNo.StartsWith("s") || orderExecuteBeginReq.orderNo.StartsWith("S"))  //现场取票
                {
                    StationOrderProcessRequestVo stationOrderProcessRequestVo = new StationOrderProcessRequestVo();
                    stationOrderProcessRequestVo.ReqSysDateString = orderExecuteBeginReq.reqSysDate;
                    stationOrderProcessRequestVo.operationCode = orderExecuteBeginReq.operationCode;
                    stationOrderProcessRequestVo.cityCode = orderExecuteBeginReq.cityCode;
                    stationOrderProcessRequestVo.DeviceId = orderExecuteBeginReq.deviceId;
                    stationOrderProcessRequestVo.channelType = orderExecuteBeginReq.channelType;
                    stationOrderProcessRequestVo.expandAttribute = orderExecuteBeginReq.expandAttribute;
                    stationOrderProcessRequestVo.orderNo = orderExecuteBeginReq.orderNo;

                    StationOrderBo bo = new StationOrderBo();
                    StationOrderProcessRespondVo stationOrderProcessRespondVo = bo.StationOrderProcess(stationOrderProcessRequestVo);
                    oebResponce.respCode = stationOrderProcessRespondVo.RespondCodeString;
                    oebResponce.respCodeMemo = stationOrderProcessRespondVo.respCodeMemo;
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

            try
            {
                string strOEBResp = LogHelper.GetObjectMemberString(oebResponce);
                string strLog = String.Format("Resp:{{{0}}}", strOEBResp);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return oebResponce;
        }

        /// <summary>
        /// 订单执行结果
        /// </summary>
        /// <param name="orderExecuteResultReq"></param>
        /// <returns></returns>
        [WebMethod]
        public OrderExecuteResultResp S1_004(OrderExecuteResultReq orderExecuteResultReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                string strOERReq = LogHelper.GetObjectMemberString(orderExecuteResultReq);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOERReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            OrderExecuteResultResp oerResponce = new OrderExecuteResultResp();
            oerResponce.respCode = "0004";
            try
            {
                if (orderExecuteResultReq.orderNo.StartsWith("W") || orderExecuteResultReq.orderNo.StartsWith("w"))
                {
                    WebOrderTakenRequestVo newWebOrderTakenRequestVo = new WebOrderTakenRequestVo();
                    newWebOrderTakenRequestVo.ReqSysDateString = orderExecuteResultReq.reqSysDate;
                    newWebOrderTakenRequestVo.operationCode = orderExecuteResultReq.operationCode;
                    newWebOrderTakenRequestVo.cityCode = orderExecuteResultReq.cityCode;
                    newWebOrderTakenRequestVo.DeviceId = orderExecuteResultReq.deviceId;
                    newWebOrderTakenRequestVo.channelType = orderExecuteResultReq.channelType;
                    newWebOrderTakenRequestVo.expandAttribute = orderExecuteResultReq.expandAttribute;
                    newWebOrderTakenRequestVo.orderNo = orderExecuteResultReq.orderNo;
                    newWebOrderTakenRequestVo.takeSingleTicketNum = orderExecuteResultReq.takeSingleTicketNum.ToString();
                    newWebOrderTakenRequestVo.takeSingleTicketDateString = orderExecuteResultReq.takeSingleTicketDate;

                    WebPreOrderBo bo = new WebPreOrderBo();
                    WebOrderTakenRespondVo webOrderTakenRespondVo = bo.WebOrderTaken(newWebOrderTakenRequestVo);
                    oerResponce.respCode = webOrderTakenRespondVo.RespondCodeString;
                    oerResponce.respCodeMemo = webOrderTakenRespondVo.respCodeMemo;
                }
                else if (orderExecuteResultReq.orderNo.StartsWith("S") || orderExecuteResultReq.orderNo.StartsWith("s"))
                {
                    StationOrderTakenRequestVo stationOrderTakenRequestVo = new StationOrderTakenRequestVo();
                    stationOrderTakenRequestVo.ReqSysDateString = orderExecuteResultReq.reqSysDate;
                    stationOrderTakenRequestVo.operationCode = orderExecuteResultReq.operationCode;
                    stationOrderTakenRequestVo.cityCode = orderExecuteResultReq.cityCode;
                    stationOrderTakenRequestVo.DeviceId = orderExecuteResultReq.deviceId;
                    stationOrderTakenRequestVo.channelType = orderExecuteResultReq.channelType;
                    stationOrderTakenRequestVo.expandAttribute = orderExecuteResultReq.expandAttribute;
                    stationOrderTakenRequestVo.orderNo = orderExecuteResultReq.orderNo;
                    stationOrderTakenRequestVo.takeSingleTicketNum = orderExecuteResultReq.takeSingleTicketNum.ToString();
                    stationOrderTakenRequestVo.takeSingleTicketDateString = orderExecuteResultReq.takeSingleTicketDate;

                    StationOrderBo bo = new StationOrderBo();
                    StationOrderTakenRespondVo stationOrderTakenRespondVo = bo.StationOrderTaken(stationOrderTakenRequestVo);
                    oerResponce.respCode = stationOrderTakenRespondVo.RespondCodeString;
                    oerResponce.respCodeMemo = stationOrderTakenRespondVo.respCodeMemo;
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

            try
            {
                string strOERResp = LogHelper.GetObjectMemberString(oerResponce);
                string strLog = String.Format("Resp:{{{0}}}", strOERResp);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return oerResponce;
        }

        /// <summary>
        /// 订单执行故障
        /// </summary>
        /// <param name="orderExecuteFaultReq"></param>
        /// <returns></returns>
        [WebMethod]
        public OrderExecuteFaultResp S1_005(OrderExecuteFaultReq orderExecuteFaultReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                string strOEFReq = LogHelper.GetObjectMemberString(orderExecuteFaultReq);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOEFReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            OrderExecuteFaultResp oefResponce = new OrderExecuteFaultResp();
            oefResponce.respCode = "0004";
            try
            {
                if (orderExecuteFaultReq.orderNo.StartsWith("W") || orderExecuteFaultReq.orderNo.StartsWith("w"))
                {
                    WebOrderTakenErrRequestVo newWebOrderTakenErrRequestVo = new WebOrderTakenErrRequestVo();
                    newWebOrderTakenErrRequestVo.ReqSysDateString = orderExecuteFaultReq.reqSysDate;
                    newWebOrderTakenErrRequestVo.operationCode = orderExecuteFaultReq.operationCode;
                    newWebOrderTakenErrRequestVo.cityCode = orderExecuteFaultReq.cityCode;
                    newWebOrderTakenErrRequestVo.DeviceId = orderExecuteFaultReq.deviceId;
                    newWebOrderTakenErrRequestVo.channelType = orderExecuteFaultReq.channelType;
                    newWebOrderTakenErrRequestVo.expandAttribute = orderExecuteFaultReq.expandAttribute;
                    newWebOrderTakenErrRequestVo.orderNo = orderExecuteFaultReq.orderNo;
                    newWebOrderTakenErrRequestVo.takeSingleTicketNum = orderExecuteFaultReq.takeSingleTicketNum.ToString();
                    newWebOrderTakenErrRequestVo.faultOccurDateString = orderExecuteFaultReq.faultOccurDate;
                    newWebOrderTakenErrRequestVo.faultSlipSeq = orderExecuteFaultReq.faultSlipSeq;
                    newWebOrderTakenErrRequestVo.errorCode = orderExecuteFaultReq.erroCode;
                    newWebOrderTakenErrRequestVo.errorMessage = orderExecuteFaultReq.errorMessage;

                    WebPreOrderBo bo = new WebPreOrderBo();
                    WebOrderTakenErrRespondVo webOrderTakenErrRespondVo = bo.WebOrderTakenErr(newWebOrderTakenErrRequestVo);
                    oefResponce.respCode = webOrderTakenErrRespondVo.RespondCodeString;
                    oefResponce.respCodeMemo = webOrderTakenErrRespondVo.respCodeMemo;
                }
                else if (orderExecuteFaultReq.orderNo.StartsWith("S") || orderExecuteFaultReq.orderNo.StartsWith("s"))
                {
                    StationOrderTakenErrRequestVo stationOrderTakenErrRequestVo = new StationOrderTakenErrRequestVo();
                    stationOrderTakenErrRequestVo.ReqSysDateString = orderExecuteFaultReq.reqSysDate;
                    stationOrderTakenErrRequestVo.operationCode = orderExecuteFaultReq.operationCode;
                    stationOrderTakenErrRequestVo.cityCode = orderExecuteFaultReq.cityCode;
                    stationOrderTakenErrRequestVo.DeviceId = orderExecuteFaultReq.deviceId;
                    stationOrderTakenErrRequestVo.channelType = orderExecuteFaultReq.channelType;
                    stationOrderTakenErrRequestVo.expandAttribute = orderExecuteFaultReq.expandAttribute;
                    stationOrderTakenErrRequestVo.orderNo = orderExecuteFaultReq.orderNo;
                    stationOrderTakenErrRequestVo.takeSingleTicketNum = orderExecuteFaultReq.takeSingleTicketNum.ToString();
                    stationOrderTakenErrRequestVo.faultOccurDateString = orderExecuteFaultReq.faultOccurDate;
                    stationOrderTakenErrRequestVo.errorCode = orderExecuteFaultReq.erroCode;
                    stationOrderTakenErrRequestVo.errorMessage = orderExecuteFaultReq.errorMessage;

                    StationOrderBo bo = new StationOrderBo();
                    StationOrderTakenErrRespondVo stationOrderTakenErrRespondVo = bo.StationOrderTakenErr(stationOrderTakenErrRequestVo);
                    oefResponce.respCode = stationOrderTakenErrRespondVo.RespondCodeString;
                    oefResponce.respCodeMemo = stationOrderTakenErrRespondVo.respCodeMemo;

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

            try
            {
                string strOEFResp = LogHelper.GetObjectMemberString(oefResponce);
                string strLog = String.Format("Resp:{{{0}}}", strOEFResp);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return oefResponce;
        }

        /// <summary>
        /// 心跳监测
        /// </summary>
        /// <param name="heartBeatReq"></param>
        /// <returns></returns>
        [WebMethod]
        public HeartBeatResp S1_009(HeartBeatReq heartBeatReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                string strhbReq = LogHelper.GetObjectMemberString(heartBeatReq);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strhbReq);
                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            HeartBeatResp hbResponce = new HeartBeatResp();
            hbResponce.respCode = "9999";
            try
            {
                if (!String.IsNullOrEmpty(heartBeatReq.reqSysDate) && !String.IsNullOrEmpty(heartBeatReq.operationCode) && !String.IsNullOrEmpty(heartBeatReq.cityCode) && !String.IsNullOrEmpty(heartBeatReq.deviceId) && !String.IsNullOrEmpty(heartBeatReq.channelType))
                {
                    hbResponce.respCode = "0000";
                }
                else
                {
                    hbResponce.respCode = "0001";
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

            try
            {
                string strhbResp = LogHelper.GetObjectMemberString(hbResponce);
                string strLog = String.Format("Resp:{{{0}}}", strhbResp);

                _log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Debug(ts.TotalMilliseconds);

            return hbResponce;
        }
    }
}
