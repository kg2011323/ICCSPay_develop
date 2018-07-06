using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Reflection;
using System.Diagnostics;

using log4net;
using SLEWebService.Vo;
using SLEWebService.Util;
using PlatformLib.Vo;
using PlatformLib.BLL;

namespace SLEWebService
{
    /// <summary>
    /// Summary description for SLEWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SLEWebService : System.Web.Services.WebService
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string _respondCodeParaErr = String.Format("{0:D4}", ((int)DeviceCommRespondCode.RC0001));
        private static readonly string _respondCodeOrderNoInvalid = String.Format("{0:D4}", ((int)DeviceCommRespondCode.RC0004));
        private static readonly string _respondCodeDeviceInvalid = String.Format("{0:D4}", ((int)DeviceCommRespondCode.RC0006));
        private static readonly string _respondCodeDefault = String.Format("{0:D4}", ((int)DeviceCommRespondCode.RC9999));
        
        /// <summary>
        /// 网络预购订单前缀
        /// </summary>        
        private static readonly string _orderNoPrefixWeb = PlatformLib.Util.Constants.OrderNoPrefixWeb;
        /// <summary>
        /// 车站现场订单前缀
        /// </summary>
        private static readonly string _orderNoPrefixStation = PlatformLib.Util.Constants.OrderNoPrefixStation;

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
                    strRequestIP = String.Format("IP:[{0}]", this.Context.Request.UserHostAddress);
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }

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
            ocResponce.respCode = _respondCodeDefault;
            try
            {
                if (null == orderCommitReq)
                {
                    ocResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(orderCommitReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            ocResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        StationOrderPayRequestVo stationOrderPayRequestVo = new StationOrderPayRequestVo();
                        stationOrderPayRequestVo.ReqSysDateString = orderCommitReq.reqSysDate;
                        stationOrderPayRequestVo.OperationCode = orderCommitReq.operationCode;
                        stationOrderPayRequestVo.CityCode = orderCommitReq.cityCode;
                        stationOrderPayRequestVo.DeviceId = orderCommitReq.deviceId;
                        stationOrderPayRequestVo.ChannelType = orderCommitReq.channelType;
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
                        StationOrderPayRespondVo stationOrderRespondVo = bo.OrderPay(stationOrderPayRequestVo, Global.eh.S1Dict);
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
            qprResponce.respCode = _respondCodeDefault;
            try
            {
                if (null == queryPaymentResultReq)
                {
                    qprResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(queryPaymentResultReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            qprResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        StationOrderPayResultRequestVo stationOrderPayResultRequestVo = new StationOrderPayResultRequestVo();
                        stationOrderPayResultRequestVo.ReqSysDateString = queryPaymentResultReq.reqSysDate;
                        stationOrderPayResultRequestVo.OperationCode = queryPaymentResultReq.operationCode;
                        stationOrderPayResultRequestVo.CityCode = queryPaymentResultReq.cityCode;
                        stationOrderPayResultRequestVo.DeviceId = queryPaymentResultReq.deviceId;
                        stationOrderPayResultRequestVo.ChannelType = queryPaymentResultReq.channelType;
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

        ///// <summary>
        ///// 订单执行开始
        ///// </summary>
        ///// <param name="orderExecuteBeginReq"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public OrderExecuteBeginResp S1_003(OrderExecuteBeginReq orderExecuteBeginReq)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    try
        //    {
        //        string strOEBReq = LogHelper.GetObjectMemberString(orderExecuteBeginReq);
        //        string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOEBReq);
        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    OrderExecuteBeginResp oebResponce = new OrderExecuteBeginResp();
        //    oebResponce.respCode = _respondCodeDefault;
        //    try
        //    {
        //        if (null == orderExecuteBeginReq)
        //        {
        //            oebResponce.respCode = _respondCodeParaErr;
        //        }
        //        else
        //        {
        //            bool isDeviceIdValid = true;

        //            if (Constants.IsCheckDeviceId)
        //            {
        //                if (!DeviceHelper.Instance.IsValidDeviceId(orderExecuteBeginReq.deviceId))
        //                {
        //                    isDeviceIdValid = false;
        //                    oebResponce.respCode = _respondCodeDeviceInvalid;
        //                }
        //            }

        //            if (isDeviceIdValid)
        //            {
        //                oebResponce.respCode = _respondCodeOrderNoInvalid;

        //                string strOrderNoToUpper = orderExecuteBeginReq.orderNo.ToUpper().Trim();
        //                if (strOrderNoToUpper.StartsWith(_orderNoPrefixWeb)) 
        //                {
        //                    #region 网络购票
        //                    WebOrderProcessRequestVo newWebOrderProcessRequestVo = new WebOrderProcessRequestVo();
        //                    newWebOrderProcessRequestVo.ReqSysDateString = orderExecuteBeginReq.reqSysDate;
        //                    newWebOrderProcessRequestVo.OperationCode = orderExecuteBeginReq.operationCode;
        //                    newWebOrderProcessRequestVo.CityCode = orderExecuteBeginReq.cityCode;
        //                    newWebOrderProcessRequestVo.DeviceId = orderExecuteBeginReq.deviceId;
        //                    newWebOrderProcessRequestVo.ChannelType = orderExecuteBeginReq.channelType;
        //                    newWebOrderProcessRequestVo.expandAttribute = orderExecuteBeginReq.expandAttribute;
        //                    newWebOrderProcessRequestVo.orderNo = orderExecuteBeginReq.orderNo;

        //                    WebPreOrderBo bo = new WebPreOrderBo();
        //                    WebOrderProcessRespondVo webOrderProcessRespondVo = bo.WebOrderProcess(newWebOrderProcessRequestVo);
        //                    oebResponce.respCode = webOrderProcessRespondVo.RespondCodeString;
        //                    oebResponce.respCodeMemo = webOrderProcessRespondVo.respCodeMemo;
        //                    #endregion 网络购票
        //                }
        //                else if (strOrderNoToUpper.StartsWith(_orderNoPrefixStation))
        //                {
        //                    #region 现场取票
        //                    StationOrderProcessRequestVo stationOrderProcessRequestVo = new StationOrderProcessRequestVo();
        //                    stationOrderProcessRequestVo.ReqSysDateString = orderExecuteBeginReq.reqSysDate;
        //                    stationOrderProcessRequestVo.OperationCode = orderExecuteBeginReq.operationCode;
        //                    stationOrderProcessRequestVo.CityCode = orderExecuteBeginReq.cityCode;
        //                    stationOrderProcessRequestVo.DeviceId = orderExecuteBeginReq.deviceId;
        //                    stationOrderProcessRequestVo.ChannelType = orderExecuteBeginReq.channelType;
        //                    stationOrderProcessRequestVo.expandAttribute = orderExecuteBeginReq.expandAttribute;
        //                    stationOrderProcessRequestVo.orderNo = orderExecuteBeginReq.orderNo;

        //                    StationOrderBo bo = new StationOrderBo();
        //                    StationOrderProcessRespondVo stationOrderProcessRespondVo = bo.StationOrderProcess(stationOrderProcessRequestVo);
        //                    oebResponce.respCode = stationOrderProcessRespondVo.RespondCodeString;
        //                    oebResponce.respCodeMemo = stationOrderProcessRespondVo.respCodeMemo;
        //                    #endregion 现场取票
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


        //    try
        //    {
        //        string strOEBResp = LogHelper.GetObjectMemberString(oebResponce);
        //        string strLog = String.Format("Resp:{{{0}}}", strOEBResp);

        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    sw.Stop();
        //    TimeSpan ts = sw.Elapsed;
        //    _log.Debug(ts.TotalMilliseconds);

        //    return oebResponce;
        //}

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
            oerResponce.respCode = _respondCodeDefault;
            try
            {
                if (null == orderExecuteResultReq)
                {
                    oerResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(orderExecuteResultReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            oerResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        oerResponce.respCode = _respondCodeOrderNoInvalid;

                        string strOrderNoToUpper = orderExecuteResultReq.orderNo.ToUpper().Trim();
                        if (strOrderNoToUpper.StartsWith(_orderNoPrefixWeb))
                        {
                            #region 网络购票
                            WebOrderTakenRequestVo newWebOrderTakenRequestVo = new WebOrderTakenRequestVo();
                            newWebOrderTakenRequestVo.ReqSysDateString = orderExecuteResultReq.reqSysDate;
                            newWebOrderTakenRequestVo.OperationCode = orderExecuteResultReq.operationCode;
                            newWebOrderTakenRequestVo.CityCode = orderExecuteResultReq.cityCode;
                            newWebOrderTakenRequestVo.DeviceId = orderExecuteResultReq.deviceId;
                            newWebOrderTakenRequestVo.ChannelType = orderExecuteResultReq.channelType;
                            newWebOrderTakenRequestVo.expandAttribute = orderExecuteResultReq.expandAttribute;
                            newWebOrderTakenRequestVo.orderNo = orderExecuteResultReq.orderNo;
                            newWebOrderTakenRequestVo.takeSingleTicketNum = orderExecuteResultReq.takeSingleTicketNum.ToString();
                            newWebOrderTakenRequestVo.takeSingleTicketDateString = orderExecuteResultReq.takeSingleTicketDate;

                            WebPreOrderBo bo = new WebPreOrderBo();
                            WebOrderTakenRespondVo webOrderTakenRespondVo = bo.WebOrderTaken(newWebOrderTakenRequestVo);
                            oerResponce.respCode = webOrderTakenRespondVo.RespondCodeString;
                            oerResponce.respCodeMemo = webOrderTakenRespondVo.respCodeMemo;
                            #endregion 网络购票
                        }
                        else if (strOrderNoToUpper.StartsWith(_orderNoPrefixStation))
                        {
                            #region 现场取票
                            StationOrderTakenRequestVo stationOrderTakenRequestVo = new StationOrderTakenRequestVo();
                            stationOrderTakenRequestVo.ReqSysDateString = orderExecuteResultReq.reqSysDate;
                            stationOrderTakenRequestVo.OperationCode = orderExecuteResultReq.operationCode;
                            stationOrderTakenRequestVo.CityCode = orderExecuteResultReq.cityCode;
                            stationOrderTakenRequestVo.DeviceId = orderExecuteResultReq.deviceId;
                            stationOrderTakenRequestVo.ChannelType = orderExecuteResultReq.channelType;
                            stationOrderTakenRequestVo.expandAttribute = orderExecuteResultReq.expandAttribute;
                            stationOrderTakenRequestVo.orderNo = orderExecuteResultReq.orderNo;
                            stationOrderTakenRequestVo.takeSingleTicketNum = orderExecuteResultReq.takeSingleTicketNum.ToString();
                            stationOrderTakenRequestVo.takeSingleTicketDateString = orderExecuteResultReq.takeSingleTicketDate;

                            StationOrderBo bo = new StationOrderBo();
                            StationOrderTakenRespondVo stationOrderTakenRespondVo = bo.StationOrderTaken(stationOrderTakenRequestVo);
                            oerResponce.respCode = stationOrderTakenRespondVo.RespondCodeString;
                            oerResponce.respCodeMemo = stationOrderTakenRespondVo.respCodeMemo;
                            #endregion 现场取票
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
            oefResponce.respCode = _respondCodeDefault;
            try
            {
                if (null == orderExecuteFaultReq)
                {
                    oefResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(orderExecuteFaultReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            oefResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        oefResponce.respCode = _respondCodeOrderNoInvalid;

                        string strOrderNoToUpper = orderExecuteFaultReq.orderNo.ToUpper().Trim();
                        if (strOrderNoToUpper.StartsWith(_orderNoPrefixWeb))
                        {
                            #region 网络购票
                            WebOrderTakenErrRequestVo newWebOrderTakenErrRequestVo = new WebOrderTakenErrRequestVo();
                            newWebOrderTakenErrRequestVo.ReqSysDateString = orderExecuteFaultReq.reqSysDate;
                            newWebOrderTakenErrRequestVo.OperationCode = orderExecuteFaultReq.operationCode;
                            newWebOrderTakenErrRequestVo.CityCode = orderExecuteFaultReq.cityCode;
                            newWebOrderTakenErrRequestVo.DeviceId = orderExecuteFaultReq.deviceId;
                            newWebOrderTakenErrRequestVo.ChannelType = orderExecuteFaultReq.channelType;
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
                            #endregion 网络购票
                        }
                        else if (strOrderNoToUpper.StartsWith(_orderNoPrefixStation))
                        {
                            #region 现场取票
                            StationOrderTakenErrRequestVo stationOrderTakenErrRequestVo = new StationOrderTakenErrRequestVo();
                            stationOrderTakenErrRequestVo.ReqSysDateString = orderExecuteFaultReq.reqSysDate;
                            stationOrderTakenErrRequestVo.OperationCode = orderExecuteFaultReq.operationCode;
                            stationOrderTakenErrRequestVo.CityCode = orderExecuteFaultReq.cityCode;
                            stationOrderTakenErrRequestVo.DeviceId = orderExecuteFaultReq.deviceId;
                            stationOrderTakenErrRequestVo.ChannelType = orderExecuteFaultReq.channelType;
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
                            #endregion 现场取票
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
            tvResponce.respCode = _respondCodeDefault;
            try
            {
                if (null == ticketVerificationReq)
                {
                    tvResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(ticketVerificationReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            tvResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        WebOrderVerifyRequestVo newWebOrderVerifyRequestVo = new WebOrderVerifyRequestVo();
                        newWebOrderVerifyRequestVo.ReqSysDateString = ticketVerificationReq.reqSysDate;
                        newWebOrderVerifyRequestVo.OperationCode = ticketVerificationReq.operationCode;
                        newWebOrderVerifyRequestVo.CityCode = ticketVerificationReq.cityCode;
                        newWebOrderVerifyRequestVo.DeviceId = ticketVerificationReq.deviceId;
                        newWebOrderVerifyRequestVo.ChannelType = ticketVerificationReq.channelType;
                        newWebOrderVerifyRequestVo.expandAttribute = ticketVerificationReq.expandAttribute;
                        newWebOrderVerifyRequestVo.orderNo = ticketVerificationReq.orderNo;
                        newWebOrderVerifyRequestVo.orderToken = ticketVerificationReq.orderToken;

                        WebPreOrderBo bo = new WebPreOrderBo();
                        WebOrderVerifyRespondVo webOrderVerifyRespondVo = bo.WebOrderVerify(newWebOrderVerifyRequestVo, Global.eh.S6Dict);
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
            hbResponce.respCode = _respondCodeDefault;
            try
            {
                if (null == heartBeatReq)
                {
                    hbResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(heartBeatReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            hbResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        if (!String.IsNullOrEmpty(heartBeatReq.reqSysDate)
                            && !String.IsNullOrEmpty(heartBeatReq.operationCode)
                            && !String.IsNullOrEmpty(heartBeatReq.cityCode)
                            && !String.IsNullOrEmpty(heartBeatReq.deviceId)
                            && !String.IsNullOrEmpty(heartBeatReq.channelType))
                        {
                            hbResponce.respCode = "0000";
                        }
                        else
                        {
                            hbResponce.respCode = "0001";
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


        ///// <summary>
        ///// 订单查询
        ///// </summary>
        ///// <param name="orderQueryReq"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public OrderQueryResp S1_011(OrderQueryReq orderQueryReq)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    try
        //    {
        //        string strTVReq = LogHelper.GetObjectMemberString(orderQueryReq);
        //        string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strTVReq);
        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    OrderQueryResp oqResponce = new OrderQueryResp();
        //    oqResponce.respCode = _respondCodeDefault;
        //    try
        //    {
        //        if (null == orderQueryReq)
        //        {
        //            oqResponce.respCode = _respondCodeParaErr;
        //        }
        //        else
        //        {
        //            bool isDeviceIdValid = true;

        //            if (Constants.IsCheckDeviceId)
        //            {
        //                if (!DeviceHelper.Instance.IsValidDeviceId(orderQueryReq.deviceId))
        //                {
        //                    isDeviceIdValid = false;
        //                    oqResponce.respCode = _respondCodeDeviceInvalid;
        //                }
        //            }

        //            if (isDeviceIdValid)
        //            {
        //                OrderQueryRequestVo newOrderQueryRequestVo = new OrderQueryRequestVo();
        //                newOrderQueryRequestVo.ReqSysDateString = orderQueryReq.reqSysDate;
        //                newOrderQueryRequestVo.OperationCode = orderQueryReq.operationCode;
        //                newOrderQueryRequestVo.CityCode = orderQueryReq.cityCode;
        //                newOrderQueryRequestVo.DeviceId = orderQueryReq.deviceId;
        //                newOrderQueryRequestVo.ChannelType = orderQueryReq.channelType;
        //                newOrderQueryRequestVo.expandAttribute = orderQueryReq.expandAttribute;
        //                newOrderQueryRequestVo.orderNo = orderQueryReq.orderNo;

        //                WebPreOrderBo bo = new WebPreOrderBo();
        //                OrderQueryRespondVo orderQueryRespondVo = bo.OrderQuery(newOrderQueryRequestVo);
        //                oqResponce.respCode = orderQueryRespondVo.RespondCodeString;
        //                oqResponce.respCodeMemo = orderQueryRespondVo.respCodeMemo;
        //                oqResponce.expandAttribute = orderQueryRespondVo.expandAttribute;
        //                oqResponce.tradeNo = orderQueryRespondVo.tradeNo;
        //                oqResponce.singleTicketType = orderQueryRespondVo.singleTicketType;
        //                oqResponce.buyTime = orderQueryRespondVo.buyTime;
        //                oqResponce.oriAFCStationCode = orderQueryRespondVo.oriAFCStationCode;
        //                oqResponce.desAFCStationCode = orderQueryRespondVo.desAFCStationCode;
        //                oqResponce.ticketPrice = orderQueryRespondVo.ticketPrice;
        //                oqResponce.ticketNum = orderQueryRespondVo.ticketNum;
        //                oqResponce.discount = orderQueryRespondVo.discount;
        //                oqResponce.amount = orderQueryRespondVo.amount;
        //                oqResponce.orderStatus = orderQueryRespondVo.orderStatus;
        //                oqResponce.ticketTakeNum = orderQueryRespondVo.ticketNum;
        //                oqResponce.ticketTakeTime = orderQueryRespondVo.ticketTakeTime;
        //                oqResponce.entryDeviceCode = orderQueryRespondVo.entryDeviceCode;
        //                oqResponce.entryTime = orderQueryRespondVo.entryTime;
        //                oqResponce.exitDeviceCode = orderQueryRespondVo.exitDeviceCode;
        //                oqResponce.exitTime = orderQueryRespondVo.exitTime;
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

        //    try
        //    {
        //        string strObjectMemberString = LogHelper.GetObjectMemberString(oqResponce);
        //        string strLog = String.Format("Resp:{{{0}}}", strObjectMemberString);

        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    sw.Stop();
        //    TimeSpan ts = sw.Elapsed;
        //    _log.Debug(ts.TotalMilliseconds);

        //    return oqResponce;
        //}


        ///// <summary>
        ///// 订单状态更新
        ///// </summary>
        ///// <param name="orderStatusUpdateReq"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public OrderStatusUpdateResp S1_012(OrderStatusUpdateReq orderStatusUpdateReq)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    try
        //    {
        //        string strTVReq = LogHelper.GetObjectMemberString(orderStatusUpdateReq);
        //        string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strTVReq);
        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    OrderStatusUpdateResp osuResponce = new OrderStatusUpdateResp();
        //    osuResponce.respCode = _respondCodeDefault;
        //    try
        //    {
        //        if (null == orderStatusUpdateReq)
        //        {
        //            osuResponce.respCode = _respondCodeParaErr;
        //        }
        //        else
        //        {
        //            bool isDeviceIdValid = true;

        //            if (Constants.IsCheckDeviceId)
        //            {
        //                if (!DeviceHelper.Instance.IsValidDeviceId(orderStatusUpdateReq.deviceId))
        //                {
        //                    isDeviceIdValid = false;
        //                    osuResponce.respCode = _respondCodeDeviceInvalid;
        //                }
        //            }

        //            if (isDeviceIdValid)
        //            {
        //                OrderStatusUpdateRequestVo newOrderStatusUpdateRequestVo = new OrderStatusUpdateRequestVo();
        //                newOrderStatusUpdateRequestVo.ReqSysDateString = orderStatusUpdateReq.reqSysDate;
        //                newOrderStatusUpdateRequestVo.OperationCode = orderStatusUpdateReq.operationCode;
        //                newOrderStatusUpdateRequestVo.CityCode = orderStatusUpdateReq.cityCode;
        //                newOrderStatusUpdateRequestVo.DeviceId = orderStatusUpdateReq.deviceId;
        //                newOrderStatusUpdateRequestVo.ChannelType = orderStatusUpdateReq.channelType;
        //                newOrderStatusUpdateRequestVo.expandAttribute = orderStatusUpdateReq.expandAttribute;
        //                newOrderStatusUpdateRequestVo.orderNo = orderStatusUpdateReq.orderNo;
        //                newOrderStatusUpdateRequestVo.pickupStationCode = orderStatusUpdateReq.pickupStationCode;
        //                newOrderStatusUpdateRequestVo.pickupStationTime = orderStatusUpdateReq.pickupStationTime;
        //                newOrderStatusUpdateRequestVo.getOffStationCode = orderStatusUpdateReq.getOffStationCode;
        //                newOrderStatusUpdateRequestVo.getOffStationTime = orderStatusUpdateReq.pickupStationTime;
        //                newOrderStatusUpdateRequestVo.updateReason = orderStatusUpdateReq.updateReason;
        //                newOrderStatusUpdateRequestVo.updateFee = orderStatusUpdateReq.updateFee;
        //                newOrderStatusUpdateRequestVo.paymentChannel = orderStatusUpdateReq.paymentChannel;

        //                WebPreOrderBo bo = new WebPreOrderBo();
        //                OrderStatusUpdateRespondVo orderStatusUpdateRespondVo = bo.OrderStatusUpdate(newOrderStatusUpdateRequestVo);
        //                osuResponce.respCode = orderStatusUpdateRespondVo.RespondCodeString;
        //                osuResponce.respCodeMemo = orderStatusUpdateRespondVo.respCodeMemo;
        //                osuResponce.expandAttribute = orderStatusUpdateRespondVo.expandAttribute;
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

        //    try
        //    {
        //        string strObjectMemberString = LogHelper.GetObjectMemberString(osuResponce);
        //        string strLog = String.Format("Resp:{{{0}}}", strObjectMemberString);

        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    sw.Stop();
        //    TimeSpan ts = sw.Elapsed;
        //    _log.Debug(ts.TotalMilliseconds);

        //    return osuResponce;
        //}


        ///// <summary>
        ///// 订单退款
        ///// </summary>
        ///// <param name="orderRefundReq"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public OrderRefundResp S1_013(OrderRefundReq orderRefundReq)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    try
        //    {
        //        string strTVReq = LogHelper.GetObjectMemberString(orderRefundReq);
        //        string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strTVReq);
        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    OrderRefundResp orfResponce = new OrderRefundResp();
        //    orfResponce.respCode = _respondCodeDefault;
        //    try
        //    {
        //        if (null == orderRefundReq)
        //        {
        //            orfResponce.respCode = _respondCodeParaErr;
        //        }
        //        else
        //        {
        //            bool isDeviceIdValid = true;

        //            if (Constants.IsCheckDeviceId)
        //            {
        //                if (!DeviceHelper.Instance.IsValidDeviceId(orderRefundReq.deviceId))
        //                {
        //                    isDeviceIdValid = false;
        //                    orfResponce.respCode = _respondCodeDeviceInvalid;
        //                }
        //            }

        //            if (isDeviceIdValid)
        //            {
        //                OrderRefundRequestVo newOrderRefundRequestVo = new OrderRefundRequestVo();
        //                newOrderRefundRequestVo.ReqSysDateString = orderRefundReq.reqSysDate;
        //                newOrderRefundRequestVo.OperationCode = orderRefundReq.operationCode;
        //                newOrderRefundRequestVo.CityCode = orderRefundReq.cityCode;
        //                newOrderRefundRequestVo.DeviceId = orderRefundReq.deviceId;
        //                newOrderRefundRequestVo.ChannelType = orderRefundReq.channelType;
        //                newOrderRefundRequestVo.expandAttribute = orderRefundReq.expandAttribute;
        //                newOrderRefundRequestVo.orderNo = orderRefundReq.orderNo;

        //                WebPreOrderBo bo = new WebPreOrderBo();
        //                OrderRefundRespondVo orderRefundRespondVo = bo.OrderRefund(newOrderRefundRequestVo);
        //                orfResponce.respCode = orderRefundRespondVo.RespondCodeString;
        //                orfResponce.respCodeMemo = orderRefundRespondVo.respCodeMemo;
        //                orfResponce.expandAttribute = orderRefundRespondVo.expandAttribute;
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

        //    try
        //    {
        //        string strObjectMemberString = LogHelper.GetObjectMemberString(orfResponce);
        //        string strLog = String.Format("Resp:{{{0}}}", strObjectMemberString);

        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    sw.Stop();
        //    TimeSpan ts = sw.Elapsed;
        //    _log.Debug(ts.TotalMilliseconds);

        //    return orfResponce;
        //}


        ///// <summary>
        ///// 乘客事务处理（超乘、超时、余额不足）
        ///// </summary>
        ///// <param name="passengerAffairDealReq"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public PassengerAffairDealResp S1_016(PassengerAffairDealReq passengerAffairDealReq)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    try
        //    {
        //        string strTVReq = LogHelper.GetObjectMemberString(passengerAffairDealReq);
        //        string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strTVReq);
        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    PassengerAffairDealResp padResponce = new PassengerAffairDealResp();
        //    padResponce.respCode = _respondCodeDefault;
        //    try
        //    {
        //        if (null == passengerAffairDealReq)
        //        {
        //            padResponce.respCode = _respondCodeParaErr;
        //        }
        //        else
        //        {
        //            bool isDeviceIdValid = true;

        //            if (Constants.IsCheckDeviceId)
        //            {
        //                if (!DeviceHelper.Instance.IsValidDeviceId(passengerAffairDealReq.deviceId))
        //                {
        //                    isDeviceIdValid = false;
        //                    padResponce.respCode = _respondCodeDeviceInvalid;
        //                }
        //            }

        //            if (isDeviceIdValid)
        //            {
        //                PassengerAffairDealRequestVo newPassengerAffairDealRequestVo = new PassengerAffairDealRequestVo();
        //                newPassengerAffairDealRequestVo.ReqSysDateString = passengerAffairDealReq.reqSysDate;
        //                newPassengerAffairDealRequestVo.OperationCode = passengerAffairDealReq.operationCode;
        //                newPassengerAffairDealRequestVo.CityCode = passengerAffairDealReq.cityCode;
        //                newPassengerAffairDealRequestVo.DeviceId = passengerAffairDealReq.deviceId;
        //                newPassengerAffairDealRequestVo.ChannelType = passengerAffairDealReq.channelType;
        //                newPassengerAffairDealRequestVo.expandAttribute = passengerAffairDealReq.expandAttribute;
        //                newPassengerAffairDealRequestVo.paymentCode = passengerAffairDealReq.paymentCode;
        //                newPassengerAffairDealRequestVo.ticketType = passengerAffairDealReq.ticketType;
        //                newPassengerAffairDealRequestVo.ticketId = passengerAffairDealReq.ticketId;
        //                newPassengerAffairDealRequestVo.paymentVendor = passengerAffairDealReq.paymentVendor;
        //                newPassengerAffairDealRequestVo.passengerAffairType = passengerAffairDealReq.passengerAffairType;
        //                newPassengerAffairDealRequestVo.pickupStationCode = passengerAffairDealReq.pickupStationCode;
        //                newPassengerAffairDealRequestVo.pickupStationTime = passengerAffairDealReq.pickupStationTime;
        //                newPassengerAffairDealRequestVo.getOffStationCode = passengerAffairDealReq.getOffStationCode;
        //                newPassengerAffairDealRequestVo.getOffStationTime = passengerAffairDealReq.getOffStationTime;
        //                newPassengerAffairDealRequestVo.passengerAffairPrice = passengerAffairDealReq.passengerAffairPrice;

        //                WebPreOrderBo bo = new WebPreOrderBo();
        //                PassengerAffairDealRespondVo newPassengerAffairDealRespondVo = bo.PassengerAffairDeal(newPassengerAffairDealRequestVo);
        //                padResponce.respCode = newPassengerAffairDealRespondVo.RespondCodeString;
        //                padResponce.respCodeMemo = newPassengerAffairDealRespondVo.respCodeMemo;
        //                padResponce.expandAttribute = newPassengerAffairDealRespondVo.expandAttribute;
        //                padResponce.tradeNo = newPassengerAffairDealRespondVo.tradeNo;
        //                padResponce.ticketId = newPassengerAffairDealRespondVo.ticketId;
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

        //    try
        //    {
        //        string strObjectMemberString = LogHelper.GetObjectMemberString(padResponce);
        //        string strLog = String.Format("Resp:{{{0}}}", strObjectMemberString);

        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    sw.Stop();
        //    TimeSpan ts = sw.Elapsed;
        //    _log.Debug(ts.TotalMilliseconds);

        //    return padResponce;
        //}


        ///// <summary>
        ///// 乘客事务处理（超乘、超时、余额不足）状态查询
        ///// </summary>
        ///// <param name="passengerAffairDealStatusQueryReq"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public PassengerAffairDealStatusQueryResp S1_017(PassengerAffairDealStatusQueryReq passengerAffairDealStatusQueryReq)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    try
        //    {
        //        string strTVReq = LogHelper.GetObjectMemberString(passengerAffairDealStatusQueryReq);
        //        string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strTVReq);
        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    PassengerAffairDealStatusQueryResp padSQResponce = new PassengerAffairDealStatusQueryResp();
        //    padSQResponce.respCode = _respondCodeDefault;
        //    try
        //    {
        //        if (null == passengerAffairDealStatusQueryReq)
        //        {
        //            padSQResponce.respCode = _respondCodeParaErr;
        //        }
        //        else
        //        {
        //            bool isDeviceIdValid = true;

        //            if (Constants.IsCheckDeviceId)
        //            {
        //                if (!DeviceHelper.Instance.IsValidDeviceId(passengerAffairDealStatusQueryReq.deviceId))
        //                {
        //                    isDeviceIdValid = false;
        //                    padSQResponce.respCode = _respondCodeDeviceInvalid;
        //                }
        //            }

        //            if (isDeviceIdValid)
        //            {
        //                PassengerAffairDealStatusQueryRequestVo newPassengerAffairDealStatusQueryRequestVo = new PassengerAffairDealStatusQueryRequestVo();
        //                newPassengerAffairDealStatusQueryRequestVo.ReqSysDateString = passengerAffairDealStatusQueryReq.reqSysDate;
        //                newPassengerAffairDealStatusQueryRequestVo.OperationCode = passengerAffairDealStatusQueryReq.operationCode;
        //                newPassengerAffairDealStatusQueryRequestVo.CityCode = passengerAffairDealStatusQueryReq.cityCode;
        //                newPassengerAffairDealStatusQueryRequestVo.DeviceId = passengerAffairDealStatusQueryReq.deviceId;
        //                newPassengerAffairDealStatusQueryRequestVo.ChannelType = passengerAffairDealStatusQueryReq.channelType;
        //                newPassengerAffairDealStatusQueryRequestVo.expandAttribute = passengerAffairDealStatusQueryReq.expandAttribute;
        //                newPassengerAffairDealStatusQueryRequestVo.orderNo = passengerAffairDealStatusQueryReq.orderNo;
        //                newPassengerAffairDealStatusQueryRequestVo.ticketId = passengerAffairDealStatusQueryReq.ticketId;

        //                WebPreOrderBo bo = new WebPreOrderBo();
        //                PassengerAffairDealStatusQueryRespondVo newPassengerAffairDealStatusQueryRespondVo = bo.PassengerAffairDealStatueQuery(newPassengerAffairDealStatusQueryRequestVo);
        //                padSQResponce.respCode = newPassengerAffairDealStatusQueryRespondVo.RespondCodeString;
        //                padSQResponce.respCodeMemo = newPassengerAffairDealStatusQueryRespondVo.respCodeMemo;
        //                padSQResponce.expandAttribute = newPassengerAffairDealStatusQueryRespondVo.expandAttribute;
        //                padSQResponce.ticketType = newPassengerAffairDealStatusQueryRespondVo.ticketType;
        //                padSQResponce.ticketId = newPassengerAffairDealStatusQueryRespondVo.ticketId;
        //                padSQResponce.paymentVendor = newPassengerAffairDealStatusQueryRespondVo.paymentVendor;
        //                padSQResponce.ticketId = newPassengerAffairDealStatusQueryRespondVo.ticketId;
        //                padSQResponce.passengerAffairType = newPassengerAffairDealStatusQueryRespondVo.passengerAffairType;
        //                padSQResponce.pickupStationCode = newPassengerAffairDealStatusQueryRespondVo.pickupStationCode;
        //                padSQResponce.pickupStationTime = newPassengerAffairDealStatusQueryRespondVo.pickupStationTime;
        //                padSQResponce.getOffStationCode = newPassengerAffairDealStatusQueryRespondVo.getOffStationCode;
        //                padSQResponce.getOffStationTime = newPassengerAffairDealStatusQueryRespondVo.getOffStationTime;
        //                padSQResponce.passengerAffairPrice = newPassengerAffairDealStatusQueryRespondVo.passengerAffairPrice;
        //                padSQResponce.updateOrderStatus = newPassengerAffairDealStatusQueryRespondVo.updateOrderStatus;
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

        //    try
        //    {
        //        string strObjectMemberString = LogHelper.GetObjectMemberString(padSQResponce);
        //        string strLog = String.Format("Resp:{{{0}}}", strObjectMemberString);

        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    sw.Stop();
        //    TimeSpan ts = sw.Elapsed;
        //    _log.Debug(ts.TotalMilliseconds);

        //    return padSQResponce;
        //}

        ///// <summary>
        ///// 保存ApplePay交易记录
        ///// </summary>
        ///// <param name="applePayDealInsert"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public ApplePayDealInsertResp S1_015(ApplePayDealInsertReq apdiReq)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    try
        //    {
        //        string strOCReq = LogHelper.GetObjectMemberString(apdiReq);
        //        string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOCReq);
        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    ApplePayDealInsertResp apdiResp = new ApplePayDealInsertResp();
        //    apdiResp.conversationId = _respondCodeDefault;
        //    try
        //    {
        //        if (null == apdiReq)
        //        {
        //            apdiResp.conversationId = _respondCodeParaErr;
        //        }
        //        else
        //        {
        //            bool isDeviceIdValid = true;

        //            if (Constants.IsCheckDeviceId)
        //            {
        //                if (!DeviceHelper.Instance.IsValidDeviceId(apdiReq.deviceId))
        //                {
        //                    isDeviceIdValid = false;
        //                    apdiResp.conversationId = _respondCodeDeviceInvalid;
        //                }
        //            }

        //            if (isDeviceIdValid)
        //            {
        //                ApplePayDealInsertRequestVo newApplePayDealInsertRequestVo = new ApplePayDealInsertRequestVo();
        //                newApplePayDealInsertRequestVo.ReqSysDateString = apdiReq.reqSysDate;
        //                newApplePayDealInsertRequestVo.OperationCode = apdiReq.operationCode;
        //                newApplePayDealInsertRequestVo.CityCode = apdiReq.cityCode;
        //                newApplePayDealInsertRequestVo.DeviceId = apdiReq.deviceId;
        //                newApplePayDealInsertRequestVo.ChannelType = apdiReq.channelType;
        //                newApplePayDealInsertRequestVo.expandAttribute = apdiReq.expandAttribute;
        //                newApplePayDealInsertRequestVo.ConversationId = apdiReq.conversationId;
        //                newApplePayDealInsertRequestVo.PAN = apdiReq.PAN;
        //                newApplePayDealInsertRequestVo.TransactionAmount = apdiReq.transactionAmount;
        //                newApplePayDealInsertRequestVo.TransactionCurrencyCode = apdiReq.transactionCurrencyCode;
        //                newApplePayDealInsertRequestVo.TransactionTimeStringMMddhhmmss = apdiReq.transactionTime;
        //                newApplePayDealInsertRequestVo.AuthorizationResponseIdentificationCode = apdiReq.authorizationResponseIdentificationCode;
        //                newApplePayDealInsertRequestVo.RetrievalReferNumber = apdiReq.retrievalReferNumber;
        //                newApplePayDealInsertRequestVo.TerminalNo = apdiReq.terminalNo;
        //                newApplePayDealInsertRequestVo.MerchantCodeId = apdiReq.merchantCodeId;
        //                newApplePayDealInsertRequestVo.ApplicationCryptogram = apdiReq.applicationCryptogram;
        //                newApplePayDealInsertRequestVo.InputmodeCode = apdiReq.inputModeCode;
        //                newApplePayDealInsertRequestVo.CardserialNumber = apdiReq.cardSerialNumber;
        //                newApplePayDealInsertRequestVo.TerminalReadability = apdiReq.terminalReadAbility;
        //                newApplePayDealInsertRequestVo.CardconditionCode = apdiReq.cardConditionCode;
        //                newApplePayDealInsertRequestVo.TerminalPerformance = apdiReq.terminalPerformance;
        //                newApplePayDealInsertRequestVo.TerminalVerificationResults = apdiReq.terminalVerificationResults;
        //                newApplePayDealInsertRequestVo.UnpredictableNumber = apdiReq.unpredictableNumber;
        //                newApplePayDealInsertRequestVo.InterfaceEquipmentSerialNumber = apdiReq.interfaceEquipmentSerialNumber;
        //                newApplePayDealInsertRequestVo.IssuerapplicationData = apdiReq.issuerApplicationData;
        //                newApplePayDealInsertRequestVo.ApplicationTradeCounter = apdiReq.applicationTradeCounter;
        //                newApplePayDealInsertRequestVo.ApplicationInterchangeProfile = apdiReq.applicationInterchangeProfile;
        //                newApplePayDealInsertRequestVo.TransactionDate = apdiReq.transactionDate;
        //                newApplePayDealInsertRequestVo.terminalcountryCode = apdiReq.terminalCountryCode;
        //                newApplePayDealInsertRequestVo.ResponseCode = apdiReq.responseCode;
        //                newApplePayDealInsertRequestVo.TransactionType = apdiReq.transactionType;
        //                newApplePayDealInsertRequestVo.AuthorizeAmount = apdiReq.authorizeAmount;
        //                newApplePayDealInsertRequestVo.TradingCurrencyCode = apdiReq.tradingCurrencyCode;
        //                newApplePayDealInsertRequestVo.CipherCheckResult = apdiReq.cipherCheckResult;
        //                newApplePayDealInsertRequestVo.CardValidPeriod = apdiReq.cardValidPeriod;
        //                newApplePayDealInsertRequestVo.CryptogramInformationData = apdiReq.cryptogramInformationData;
        //                newApplePayDealInsertRequestVo.OtherAmount = apdiReq.otherAmount;
        //                newApplePayDealInsertRequestVo.CardholderVerificationMethod = apdiReq.cardHolderVerificationMethod;
        //                newApplePayDealInsertRequestVo.TerminalType = apdiReq.terminalType;
        //                newApplePayDealInsertRequestVo.ProfessFilename = apdiReq.professFileName;
        //                newApplePayDealInsertRequestVo.ApplicationVersion = apdiReq.applicationVersion;
        //                newApplePayDealInsertRequestVo.TradingSequenceCounter = apdiReq.tradingSequenceCounter;
        //                newApplePayDealInsertRequestVo.EcissueAuthorizationCode = apdiReq.ecissueAuthorizationCode;
        //                newApplePayDealInsertRequestVo.ProductIdentificationInformation = apdiReq.productIdentificationInformation;
        //                newApplePayDealInsertRequestVo.CardType = apdiReq.cardType;
        //                newApplePayDealInsertRequestVo.PaymentCode = apdiReq.paymentVendor;

        //                StationOrderBo bo = new StationOrderBo();
        //                ApplePayDealInsertRespondVo apdiRespVo = bo.ApplePayDealInsert(newApplePayDealInsertRequestVo);
        //                apdiResp.respCode = apdiRespVo.RespondCodeString;
        //                apdiResp.respCodeMemo = apdiRespVo.respCodeMemo;
        //                apdiResp.expandAttribute = apdiRespVo.expandAttribute;
        //                apdiResp.conversationId = apdiRespVo.conversationId;
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

        //    try
        //    {
        //        string strOCResp = LogHelper.GetObjectMemberString(apdiResp);
        //        string strLog = String.Format("Resp:{{{0}}}", strOCResp);

        //        _log.Info(strLog);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
        //        _log.Error(strLogErr);
        //    }

        //    sw.Stop();
        //    TimeSpan ts = sw.Elapsed;
        //    _log.Debug(ts.TotalMilliseconds);
        //    return apdiResp;
        //}
    }
}
