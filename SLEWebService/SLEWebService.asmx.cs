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

        private Random random = new Random();  //20180310

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
        /// 扫码支付网络预购订单前缀
        /// </summary>        
        private static readonly string _orderNoQRCWeb = PlatformLib.Util.Constants.OrderNoQRCWeb;
        /// <summary>
        /// 扫码支付车站现场订单前缀
        /// </summary>
        private static readonly string _orderNoQRCStation = PlatformLib.Util.Constants.OrderNoQRCStation;

        private static int systemErrorCount = 0;

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
            string strReqLog = "", strRespLog = "";
            try
            {
                //string strOCReq = LogHelper.GetObjectMemberString(orderCommitReq);
                //string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOCReq);
                //_log.Info(strLog);
                if (null != orderCommitReq)
                {
                    string strOCReq = "reqSysDate:" + orderCommitReq.reqSysDate + ",deviceId:" + orderCommitReq.deviceId + ",channelType:" + orderCommitReq.channelType + ",paymentCode:" + orderCommitReq.paymentCode
                    + ",paymentVendor:" + orderCommitReq.paymentVendor + ",pickupStationCode:" + orderCommitReq.pickupStationCode + ",getOffStationCode:" + orderCommitReq.getOffStationCode + ",singleTicketPrice:" + orderCommitReq.singleTicketPrice + ",singleTicketNum:" + orderCommitReq.singleTicketNum;//LogHelper.GetObjectMemberString(orderCommitReq);
                    //strReqLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOCReq);
                    strReqLog = String.Format("Req:{0}:{1}", RequestIP, strOCReq);
                }   
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            OrderCommitResp ocResponce = new OrderCommitResp();
            ocResponce.respCode = _respondCodeDefault;

            StationOrderBo bo = new StationOrderBo();
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
                //if (bo.ExceptionRespondAdd(DateTime.Now, "S1_001"))
                //{
                //    _log.Info(DateTime.Now.ToString() + "S1_001发生9999异常！");
                //}
            }

            try
            {
                string strOCResp = "respCode:" + ocResponce.respCode + ",orderNo:" + ocResponce.orderNo;
                strRespLog = String.Format("Resp:{{{0}}}", strOCResp);
                //if (strOCResp == "9999")
                //{
                //    if (bo.ExceptionRespondAdd(DateTime.Now, "S1_001"))
                //    {
                //        _log.Info(DateTime.Now.ToString() + "S1_001响应9999！");
                //    }
                //}

                //_log.Info(strLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            //ocResponce.respCode = "9999";  //测试界面优化用  gaoke
            if (ocResponce.respCode == "0000")  //正常响应时只记录订单号，否则记录详细情况
            {
                _log.Info(strRespLog);
            }
            else
            {
                _log.Info(strReqLog);
                _log.Info(strRespLog);
                _log.Debug(ts.TotalMilliseconds);
            }
            return ocResponce;
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
            string strReqLog = "", strRespLog = "";
            try
            {
                if (null != orderExecuteResultReq)
                {
                    string strOERReq = "reqSysDate:" + orderExecuteResultReq.reqSysDate + ",deviceId:" + orderExecuteResultReq.deviceId + ",channelType:" + orderExecuteResultReq.channelType + ",orderNo:" + orderExecuteResultReq.orderNo
                         + ",takeSingleTicketNum:" + orderExecuteResultReq.takeSingleTicketNum + ",takeSingleTicketDate:" + orderExecuteResultReq.takeSingleTicketDate;
                    //strReqLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOERReq);
                    strReqLog = String.Format("Req:{0}:{1}", RequestIP, strOERReq);
                }
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            OrderExecuteResultResp oerResponce = new OrderExecuteResultResp();
            oerResponce.respCode = _respondCodeDefault;
            StationOrderBo bo = new StationOrderBo();
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
                            WebReference.SLEWebService aa = new WebReference.SLEWebService();
                            WebReference.OrderExecuteResultReq oerReq = new WebReference.OrderExecuteResultReq();
                            if (null != orderExecuteResultReq)
                            {
                                oerReq.reqSysDate = orderExecuteResultReq.reqSysDate;
                                oerReq.operationCode = orderExecuteResultReq.operationCode;
                                oerReq.cityCode = orderExecuteResultReq.cityCode;
                                oerReq.deviceId = orderExecuteResultReq.deviceId;
                                oerReq.channelType = orderExecuteResultReq.channelType;
                                oerReq.expandAttribute = orderExecuteResultReq.expandAttribute.ToArray();
                                oerReq.orderNo = orderExecuteResultReq.orderNo;
                                oerReq.takeSingleTicketNum = orderExecuteResultReq.takeSingleTicketNum;
                                oerReq.takeSingleTicketDate = orderExecuteResultReq.takeSingleTicketDate;
                            }
                            WebReference.OrderExecuteResultResp oerResp = aa.S1_004(oerReq);
                            if (null != oerResp)
                            {
                                oerResponce.respCode = oerResp.respCode;
                                oerResponce.respCodeMemo = oerResp.respCodeMemo;
                            }                            
                            #endregion 网络购票
                        }
                        else if (strOrderNoToUpper.StartsWith(_orderNoPrefixStation))
                        {
                            #region 云购票机现场购票
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

                            StationOrderBo bo1 = new StationOrderBo();
                            StationOrderTakenRespondVo stationOrderTakenRespondVo = bo1.StationOrderTaken(stationOrderTakenRequestVo);
                            oerResponce.respCode = stationOrderTakenRespondVo.RespondCodeString;
                            oerResponce.respCodeMemo = stationOrderTakenRespondVo.respCodeMemo;
                            #endregion 云购票机现场购票
                        }
                        //else if (strOrderNoToUpper.StartsWith(_orderNoQRCWeb))
                        //{
                        //    #region 扫码支付网络购票
                        //    WebOrderTakenRequestVo newWebOrderTakenRequestVo = new WebOrderTakenRequestVo();
                        //    newWebOrderTakenRequestVo.ReqSysDateString = orderExecuteResultReq.reqSysDate;
                        //    newWebOrderTakenRequestVo.OperationCode = orderExecuteResultReq.operationCode;
                        //    newWebOrderTakenRequestVo.CityCode = orderExecuteResultReq.cityCode;
                        //    newWebOrderTakenRequestVo.DeviceId = orderExecuteResultReq.deviceId;
                        //    newWebOrderTakenRequestVo.ChannelType = orderExecuteResultReq.channelType;
                        //    newWebOrderTakenRequestVo.expandAttribute = orderExecuteResultReq.expandAttribute;
                        //    newWebOrderTakenRequestVo.orderNo = orderExecuteResultReq.orderNo;
                        //    newWebOrderTakenRequestVo.takeSingleTicketNum = orderExecuteResultReq.takeSingleTicketNum.ToString();
                        //    newWebOrderTakenRequestVo.takeSingleTicketDateString = orderExecuteResultReq.takeSingleTicketDate;

                        //    WebPreOrderBo bo = new WebPreOrderBo();
                        //    WebOrderTakenRespondVo webOrderTakenRespondVo = bo.WebOrderTaken(newWebOrderTakenRequestVo);
                        //    oerResponce.respCode = webOrderTakenRespondVo.RespondCodeString;
                        //    oerResponce.respCodeMemo = webOrderTakenRespondVo.respCodeMemo;
                        //    #endregion 扫码支付网络购票
                        //}
                        else if (strOrderNoToUpper.StartsWith(_orderNoQRCStation))
                        {
                            #region 扫码支付现场购票
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

                            //StationOrderBo bo = new StationOrderBo();
                            StationOrderTakenRespondVo stationOrderTakenRespondVo = bo.QRCStationOrderTaken(stationOrderTakenRequestVo);
                            oerResponce.respCode = stationOrderTakenRespondVo.RespondCodeString;
                            oerResponce.respCodeMemo = stationOrderTakenRespondVo.respCodeMemo;
                            #endregion 扫码支付现场购票
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                systemErrorCount++;
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
                //if (bo.ExceptionRespondAdd(DateTime.Now, "S1_004"))
                //{
                //    _log.Info(DateTime.Now.ToString() + "S1_004发生9999异常！");
                //}
            }

            try
            {
                string strOERResp = "respCode:" + oerResponce.respCode;
                strRespLog = String.Format("Resp:{{{0}}}", strOERResp);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            if (oerResponce.respCode == "0000")
            {
                _log.Info(strRespLog);
            }
            else
            {
                _log.Info(strReqLog);
                _log.Info(strRespLog);
                _log.Debug(ts.TotalMilliseconds);
            }

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
            string strReqLog = "", strRespLog = "";
            try
            {
                if (null != orderExecuteFaultReq)
                {
                    string strOEFReq = "reqSysDate:" + orderExecuteFaultReq.reqSysDate + ",deviceId:" + orderExecuteFaultReq.deviceId + ",channelType:" + orderExecuteFaultReq.channelType + ",orderNo:" + orderExecuteFaultReq.orderNo
                         + ",takeSingleTicketNum:" + orderExecuteFaultReq.takeSingleTicketNum + ",faultOccurDate:" + orderExecuteFaultReq.faultOccurDate + ",erroCode:" + orderExecuteFaultReq.erroCode;
                    strReqLog = String.Format("Req:{0}:{1}", RequestIP, strOEFReq);

                    _log.Debug(strReqLog);//测试环境用
                }
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            OrderExecuteFaultResp oefResponce = new OrderExecuteFaultResp();
            oefResponce.respCode = _respondCodeDefault;
            StationOrderBo bo = new StationOrderBo();
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
                            WebReference.SLEWebService aa = new WebReference.SLEWebService();
                            WebReference.OrderExecuteFaultReq oefReq = new WebReference.OrderExecuteFaultReq();
                            if (null != orderExecuteFaultReq)
                            {
                                oefReq.reqSysDate = orderExecuteFaultReq.reqSysDate;
                                oefReq.operationCode = orderExecuteFaultReq.operationCode;
                                oefReq.cityCode = orderExecuteFaultReq.cityCode;
                                oefReq.deviceId = orderExecuteFaultReq.deviceId;
                                oefReq.channelType = orderExecuteFaultReq.channelType;
                                oefReq.expandAttribute = orderExecuteFaultReq.expandAttribute.ToArray();
                                oefReq.orderNo = orderExecuteFaultReq.orderNo;
                                oefReq.takeSingleTicketNum = orderExecuteFaultReq.takeSingleTicketNum;
                                oefReq.faultOccurDate = orderExecuteFaultReq.faultOccurDate;
                                oefReq.faultSlipSeq = orderExecuteFaultReq.faultSlipSeq;
                                oefReq.erroCode = orderExecuteFaultReq.erroCode;
                                oefReq.errorMessage = orderExecuteFaultReq.errorMessage;
                            }
                            WebReference.OrderExecuteFaultResp oefResp = aa.S1_005(oefReq);
                            if (null != oefResp)
                            {
                                oefResponce.respCode = oefResp.respCode;
                                oefResponce.respCodeMemo = oefResp.respCodeMemo;
                            }
                            #endregion 网络购票
                        }
                        else if (strOrderNoToUpper.StartsWith(_orderNoPrefixStation))
                        {
                            #region 云购票机现场购票
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

                            StationOrderBo bo1 = new StationOrderBo();
                            StationOrderTakenErrRespondVo stationOrderTakenErrRespondVo = bo1.StationOrderTakenErr(stationOrderTakenErrRequestVo);
                            oefResponce.respCode = stationOrderTakenErrRespondVo.RespondCodeString;
                            oefResponce.respCodeMemo = stationOrderTakenErrRespondVo.respCodeMemo;
                            #endregion 云购票机现场购票
                        }
                        //else if (strOrderNoToUpper.StartsWith(_orderNoQRCWeb))
                        //{
                        //    #region 扫码支付网络购票
                        //    WebOrderTakenErrRequestVo newWebOrderTakenErrRequestVo = new WebOrderTakenErrRequestVo();
                        //    newWebOrderTakenErrRequestVo.ReqSysDateString = orderExecuteFaultReq.reqSysDate;
                        //    newWebOrderTakenErrRequestVo.OperationCode = orderExecuteFaultReq.operationCode;
                        //    newWebOrderTakenErrRequestVo.CityCode = orderExecuteFaultReq.cityCode;
                        //    newWebOrderTakenErrRequestVo.DeviceId = orderExecuteFaultReq.deviceId;
                        //    newWebOrderTakenErrRequestVo.ChannelType = orderExecuteFaultReq.channelType;
                        //    newWebOrderTakenErrRequestVo.expandAttribute = orderExecuteFaultReq.expandAttribute;
                        //    newWebOrderTakenErrRequestVo.orderNo = orderExecuteFaultReq.orderNo;
                        //    newWebOrderTakenErrRequestVo.takeSingleTicketNum = orderExecuteFaultReq.takeSingleTicketNum.ToString();
                        //    newWebOrderTakenErrRequestVo.faultOccurDateString = orderExecuteFaultReq.faultOccurDate;
                        //    newWebOrderTakenErrRequestVo.faultSlipSeq = orderExecuteFaultReq.faultSlipSeq;
                        //    newWebOrderTakenErrRequestVo.errorCode = orderExecuteFaultReq.erroCode;
                        //    newWebOrderTakenErrRequestVo.errorMessage = orderExecuteFaultReq.errorMessage;

                        //    WebPreOrderBo bo = new WebPreOrderBo();
                        //    WebOrderTakenErrRespondVo webOrderTakenErrRespondVo = bo.WebOrderTakenErr(newWebOrderTakenErrRequestVo);
                        //    oefResponce.respCode = webOrderTakenErrRespondVo.RespondCodeString;
                        //    oefResponce.respCodeMemo = webOrderTakenErrRespondVo.respCodeMemo;
                        //    #endregion 扫码支付网络购票
                        //}
                        else if (strOrderNoToUpper.StartsWith(_orderNoQRCStation))
                        {
                            #region 扫码支付现场购票
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

                            //StationOrderBo bo = new StationOrderBo();
                            StationOrderTakenErrRespondVo stationOrderTakenErrRespondVo = bo.QRCStationOrderTakenErr(stationOrderTakenErrRequestVo);
                            oefResponce.respCode = stationOrderTakenErrRespondVo.RespondCodeString;
                            oefResponce.respCodeMemo = stationOrderTakenErrRespondVo.respCodeMemo;
                            #endregion 扫码支付现场购票
                        }
                        else if (orderExecuteFaultReq.orderNo.Length == 10)
                        {
                            #region 扫码取票
                            WebReference.SLEWebService aa = new WebReference.SLEWebService();
                            WebReference.OrderExecuteFaultReq oefReq = new WebReference.OrderExecuteFaultReq();
                            if (null != orderExecuteFaultReq)
                            {
                                oefReq.reqSysDate = orderExecuteFaultReq.reqSysDate;
                                oefReq.operationCode = orderExecuteFaultReq.operationCode;
                                oefReq.cityCode = orderExecuteFaultReq.cityCode;
                                oefReq.deviceId = orderExecuteFaultReq.deviceId;
                                oefReq.channelType = orderExecuteFaultReq.channelType;
                                oefReq.expandAttribute = orderExecuteFaultReq.expandAttribute.ToArray();
                                oefReq.orderNo = orderExecuteFaultReq.orderNo;
                                oefReq.takeSingleTicketNum = orderExecuteFaultReq.takeSingleTicketNum;
                                oefReq.faultOccurDate = orderExecuteFaultReq.faultOccurDate;
                                oefReq.faultSlipSeq = orderExecuteFaultReq.faultSlipSeq;
                                oefReq.erroCode = orderExecuteFaultReq.erroCode;
                                oefReq.errorMessage = orderExecuteFaultReq.errorMessage;
                            }
                            WebReference.OrderExecuteFaultResp oefResp = aa.S1_005(oefReq);
                            if (null != oefResp)
                            {
                                oefResponce.respCode = oefResp.respCode;
                                oefResponce.respCodeMemo = oefResp.respCodeMemo;
                            }
                            #endregion 扫码取票
                        }
                        else
                        {
                            oefResponce.respCode = "0000";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                systemErrorCount++;
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
                //if (bo.ExceptionRespondAdd(DateTime.Now, "S1_005"))
                //{
                //    _log.Info(DateTime.Now.ToString() + "S1_005发生9999异常！");
                //}
            }

            try
            {
                string strOEFResp = "respCode:" + oefResponce.respCode;
                strRespLog = String.Format("Resp:{{{0}}}", strOEFResp);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            if (oefResponce.respCode == "0000")
            {
                _log.Info(strRespLog);
            }
            else
            {
                _log.Info(strReqLog);
                _log.Info(strRespLog);
                _log.Debug(ts.TotalMilliseconds);
            }
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
                //string strhbReq = LogHelper.GetObjectMemberString(heartBeatReq);
                //string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, heartBeatReq.deviceId);
                string strLog = String.Format("Req:{0}:{{{1}}}", RequestIP, heartBeatReq.deviceId);
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
                            StationOrderBo bo = new StationOrderBo();
                            //判断扫码购票流程是否正常  gaoke 20170825   //判断业务处理过程中响应9999的次数是否超过报警阀值
                            //int a = bo.ICCSPayCheck();
                            //if (a == 0 && systemErrorCount < 1)
                            //{
                            //    _log.Info("检查异常结果为： " + a);
                            //    hbResponce.respCode = "0000";
                            //}
                            //else
                            //{
                            //    _log.Info("检查异常结果为： " + a);
                            //    hbResponce.respCode = "1001";
                            //}

                            //hbResponce.respCode = "1001";
                            hbResponce.respCode = "0000";


                            //if (systemErrorCount > 20)
                            //{
                            //    _log.Info("系统错误数为： " + systemErrorCount);
                            //    hbResponce.respCode = "1001";
                            //}
                            //else
                            //{
                            //    _log.Info("系统错误数为： " + systemErrorCount);
                            //    hbResponce.respCode = "0000";
                            //    systemErrorCount = 0;
                            //}
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
                //string strhbResp = LogHelper.GetObjectMemberString(hbResponce);
                string strLog = String.Format("Resp:{{{0}}}", hbResponce.respCode);

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


        /// <summary>
        /// 扫码预支付链接查询接口
        /// </summary>
        /// <param name="orderExecuteBeginReq"></param>
        /// <returns></returns>
        [WebMethod]
        public SnapQRCodePrePayURLQueryResp S1_018(SnapQRCodePrePayURLQueryReq snapQRCodePrePayURLQueryReq)
        {
            if (random.Next(100) <= 35)
            {
                //20180310  增加随机响应判断
                SnapQRCodePrePayURLQueryResp sqrCodePrePayURLQueryResponce2 = new SnapQRCodePrePayURLQueryResp();
                sqrCodePrePayURLQueryResponce2.orderNo = snapQRCodePrePayURLQueryReq.orderNo;
                sqrCodePrePayURLQueryResponce2.deviceId = snapQRCodePrePayURLQueryReq.deviceId;
                sqrCodePrePayURLQueryResponce2.qrCode = "";
                sqrCodePrePayURLQueryResponce2.respCode = "0000";
                sqrCodePrePayURLQueryResponce2.respCodeMemo = "";
                return sqrCodePrePayURLQueryResponce2;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            string strReqLog = "", strRespLog = "";

            try
            {
                if (null != snapQRCodePrePayURLQueryReq)
                {
                    string strOEBReq = "reqSysDate:" + snapQRCodePrePayURLQueryReq.reqSysDate + ",deviceId:" + snapQRCodePrePayURLQueryReq.deviceId +  ",orderNo:" + snapQRCodePrePayURLQueryReq.orderNo;
                    strReqLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOEBReq);
                }
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            SnapQRCodePrePayURLQueryResp sqrCodePrePayURLQueryResponce = new SnapQRCodePrePayURLQueryResp();
            sqrCodePrePayURLQueryResponce.respCode = _respondCodeDefault;

            StationOrderBo bo = new StationOrderBo();
            try
            {
                if (null == snapQRCodePrePayURLQueryReq)
                {
                    sqrCodePrePayURLQueryResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(snapQRCodePrePayURLQueryReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            sqrCodePrePayURLQueryResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        sqrCodePrePayURLQueryResponce.respCode = _respondCodeOrderNoInvalid;

                        string strOrderNoToUpper = snapQRCodePrePayURLQueryReq.orderNo.ToUpper().Trim();
                        if (strOrderNoToUpper.StartsWith(_orderNoQRCStation))
                        {
                            # region TVM拍码支付
                            StationSnapQRCodePrePayURLQueryRequestVo stationSnapQRCodePrePayURLQueryRequestVo = new StationSnapQRCodePrePayURLQueryRequestVo();
                            stationSnapQRCodePrePayURLQueryRequestVo.ReqSysDateString = snapQRCodePrePayURLQueryReq.reqSysDate;
                            stationSnapQRCodePrePayURLQueryRequestVo.OperationCode = snapQRCodePrePayURLQueryReq.operationCode;
                            stationSnapQRCodePrePayURLQueryRequestVo.CityCode = snapQRCodePrePayURLQueryReq.cityCode;
                            stationSnapQRCodePrePayURLQueryRequestVo.DeviceId = snapQRCodePrePayURLQueryReq.deviceId;
                            stationSnapQRCodePrePayURLQueryRequestVo.ChannelType = snapQRCodePrePayURLQueryReq.channelType;
                            stationSnapQRCodePrePayURLQueryRequestVo.expandAttribute = snapQRCodePrePayURLQueryReq.expandAttribute;
                            stationSnapQRCodePrePayURLQueryRequestVo.orderNo = snapQRCodePrePayURLQueryReq.orderNo;

                            //StationOrderBo bo = new StationOrderBo();
                            StationSnapQRCodePrePayURLQueryRespondVo stationSnapQRCodePrePayURLQueryRespondVo = bo.SnapQRCodePrePayURLQuery(stationSnapQRCodePrePayURLQueryRequestVo);
                            sqrCodePrePayURLQueryResponce.orderNo = stationSnapQRCodePrePayURLQueryRespondVo.orderNo;
                            sqrCodePrePayURLQueryResponce.deviceId = stationSnapQRCodePrePayURLQueryRespondVo.deviceId;
                            sqrCodePrePayURLQueryResponce.qrCode = stationSnapQRCodePrePayURLQueryRespondVo.qrCode;
                            sqrCodePrePayURLQueryResponce.respCode = stationSnapQRCodePrePayURLQueryRespondVo.RespondCodeString;
                            sqrCodePrePayURLQueryResponce.respCodeMemo = stationSnapQRCodePrePayURLQueryRespondVo.respCodeMemo;

                            # endregion TVM拍码支付
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                systemErrorCount++;
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
                //if (bo.ExceptionRespondAdd(DateTime.Now, "S1_018"))
                //{
                //    _log.Info(DateTime.Now.ToString() + "S1_018发生9999异常！");
                //}
            }


            try
            {
                string strOEBResp = "respCode:" + sqrCodePrePayURLQueryResponce.respCode + ",orderNo:" + sqrCodePrePayURLQueryResponce.orderNo + ",qrCode:" + sqrCodePrePayURLQueryResponce.qrCode;
                strRespLog = String.Format("Resp:{{{0}}}", strOEBResp);
                //if (strOEBResp == "9999")
                //{
                //    if (bo.ExceptionRespondAdd(DateTime.Now, "S1_018"))
                //    {
                //        _log.Info(DateTime.Now.ToString() + "S1_018响应9999！");
                //    }
                //}
                //_log.Info(strRespLog);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            if (sqrCodePrePayURLQueryResponce.respCode == "0000" && sqrCodePrePayURLQueryResponce.qrCode != "")  //只有在查询到支付链接后才写日志
            {
                _log.Info(strReqLog);
                _log.Info(strRespLog);
                _log.Debug(ts.TotalMilliseconds);
            }
            else
            {
                _log.Info(sqrCodePrePayURLQueryResponce.orderNo + "查询支付链接未成功！");
            }

            return sqrCodePrePayURLQueryResponce;
        }

        /// <summary>
        /// 扫码支付结果查询接口
        /// </summary>
        /// <param name="orderExecuteBeginReq"></param>
        /// <returns></returns>
        [WebMethod]
        public SnapQRCodePayResultQueryResp S1_019(SnapQRCodePayResultQueryReq snapQRCodePayResultQueryReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string strReqLog = "", strRespLog = "";
            try
            {
                if (null != snapQRCodePayResultQueryReq)
                {
                    string strOEBReq = "reqSysDate:" + snapQRCodePayResultQueryReq.reqSysDate + ",deviceId:" + snapQRCodePayResultQueryReq.deviceId + ", orderNo:" + snapQRCodePayResultQueryReq.orderNo;
                    strReqLog = String.Format("Req:{0}:{{{1}}}", RequestIP, strOEBReq);
                }
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            SnapQRCodePayResultQueryResp sqrCodePayResultQueryResponce = new SnapQRCodePayResultQueryResp();
            sqrCodePayResultQueryResponce.respCode = _respondCodeDefault;
            StationOrderBo bo = new StationOrderBo();
            try
            {
                if (null == snapQRCodePayResultQueryReq)
                {
                    sqrCodePayResultQueryResponce.respCode = _respondCodeParaErr;
                }
                else
                {
                    bool isDeviceIdValid = true;

                    if (Constants.IsCheckDeviceId)
                    {
                        if (!DeviceHelper.Instance.IsValidDeviceId(snapQRCodePayResultQueryReq.deviceId))
                        {
                            isDeviceIdValid = false;
                            sqrCodePayResultQueryResponce.respCode = _respondCodeDeviceInvalid;
                        }
                    }

                    if (isDeviceIdValid)
                    {
                        sqrCodePayResultQueryResponce.respCode = _respondCodeOrderNoInvalid;

                        string strOrderNoToUpper = snapQRCodePayResultQueryReq.orderNo.ToUpper().Trim();
                        if (strOrderNoToUpper.StartsWith(_orderNoQRCStation))
                        {
                            # region TVM拍码支付
                            StationSnapQRCodePayResultQueryRequestVo stationSnapQRCodePayResultQueryRequestVo = new StationSnapQRCodePayResultQueryRequestVo();
                            stationSnapQRCodePayResultQueryRequestVo.ReqSysDateString = snapQRCodePayResultQueryReq.reqSysDate;
                            stationSnapQRCodePayResultQueryRequestVo.OperationCode = snapQRCodePayResultQueryReq.operationCode;
                            stationSnapQRCodePayResultQueryRequestVo.CityCode = snapQRCodePayResultQueryReq.cityCode;
                            stationSnapQRCodePayResultQueryRequestVo.DeviceId = snapQRCodePayResultQueryReq.deviceId;
                            stationSnapQRCodePayResultQueryRequestVo.ChannelType = snapQRCodePayResultQueryReq.channelType;
                            stationSnapQRCodePayResultQueryRequestVo.expandAttribute = snapQRCodePayResultQueryReq.expandAttribute;
                            stationSnapQRCodePayResultQueryRequestVo.orderNo = snapQRCodePayResultQueryReq.orderNo;

                            //StationOrderBo bo = new StationOrderBo();
                            StationSnapQRCodePayResultQueryRespondVo stationSnapQRCodePayResultQueryRespondVo = bo.SnapQRCodePayResultQuery(stationSnapQRCodePayResultQueryRequestVo);
                            sqrCodePayResultQueryResponce.orderNo = stationSnapQRCodePayResultQueryRespondVo.orderNo;
                            sqrCodePayResultQueryResponce.paymentDate = stationSnapQRCodePayResultQueryRespondVo.paymentDate;
                            sqrCodePayResultQueryResponce.amount = stationSnapQRCodePayResultQueryRespondVo.amount;
                            sqrCodePayResultQueryResponce.paymentAccount = stationSnapQRCodePayResultQueryRespondVo.paymentAccount;
                            sqrCodePayResultQueryResponce.paymentResult = stationSnapQRCodePayResultQueryRespondVo.paymentResult;
                            sqrCodePayResultQueryResponce.paymentDesc = stationSnapQRCodePayResultQueryRespondVo.paymentDesc;
                            sqrCodePayResultQueryResponce.userName = stationSnapQRCodePayResultQueryRespondVo.userName;
                            sqrCodePayResultQueryResponce.respCode = stationSnapQRCodePayResultQueryRespondVo.RespondCodeString;
                            sqrCodePayResultQueryResponce.respCodeMemo = stationSnapQRCodePayResultQueryRespondVo.respCodeMemo;

                            # endregion TVM拍码支付
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                systemErrorCount++;
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }

            try
            {
                string strOEBResp = "respCode:" + sqrCodePayResultQueryResponce.respCode + ",paymentDate:" + sqrCodePayResultQueryResponce.paymentDate + ",amount:" + sqrCodePayResultQueryResponce.amount + ",userName:" + sqrCodePayResultQueryResponce.userName;
                strRespLog = String.Format("Resp:{{{0}}}", strOEBResp);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            if (sqrCodePayResultQueryResponce.respCode == "0000")//只有在查询到支付成功后才写日志
            {
                _log.Info(strReqLog);
                _log.Info(strRespLog);
                _log.Debug(ts.TotalMilliseconds);
            }
            else
            {
                _log.Info(sqrCodePayResultQueryResponce.orderNo + "查询支付结果未成功！");
            }
            return sqrCodePayResultQueryResponce;
        }

        /// <summary>
        /// 扫码取票订单查询接口
        /// </summary>
        /// <param name="orderExecuteBeginReq"></param>
        /// <returns></returns>
        [WebMethod]
        public SnapQRCodeTakeOrderQueryResp S1_020(SnapQRCodeTakeOrderQueryReq snapQRCodeTakeOrderQueryReq)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string strReqLog = "", strRespLog = "";
            try
            {
                if (null != snapQRCodeTakeOrderQueryReq)
                {
                    string strTVReq = "reqSysDate:" + snapQRCodeTakeOrderQueryReq.reqSysDate + ",deviceId:" + snapQRCodeTakeOrderQueryReq.deviceId + ",channelType:" + snapQRCodeTakeOrderQueryReq.channelType + ",orderNo:" + snapQRCodeTakeOrderQueryReq.randomFact;
                    strReqLog = String.Format("Req:{0}:{1}", RequestIP, strTVReq);
                }
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Req Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            SnapQRCodeTakeOrderQueryResp sqrCodeTakeOrderQueryResponce = new SnapQRCodeTakeOrderQueryResp();
            sqrCodeTakeOrderQueryResponce.respCode = _respondCodeDefault;
            //WebPreOrderBo bo = new WebPreOrderBo();
            try
            {
                WebReference.SLEWebService aa = new WebReference.SLEWebService();
                WebReference.SnapQRCodeTakeOrderQueryReq sqrcTOQReq = new WebReference.SnapQRCodeTakeOrderQueryReq();
                if (null != snapQRCodeTakeOrderQueryReq)
                {
                    sqrcTOQReq.reqSysDate = snapQRCodeTakeOrderQueryReq.reqSysDate;
                    sqrcTOQReq.operationCode = snapQRCodeTakeOrderQueryReq.operationCode;
                    sqrcTOQReq.cityCode = snapQRCodeTakeOrderQueryReq.cityCode;
                    sqrcTOQReq.deviceId = snapQRCodeTakeOrderQueryReq.deviceId;
                    sqrcTOQReq.channelType = snapQRCodeTakeOrderQueryReq.channelType;
                    sqrcTOQReq.expandAttribute = snapQRCodeTakeOrderQueryReq.expandAttribute.ToArray();
                    sqrcTOQReq.randomFact = snapQRCodeTakeOrderQueryReq.randomFact;
                }
                WebReference.SnapQRCodeTakeOrderQueryResp sqrcTOQResp = aa.S1_020(sqrcTOQReq);
                if (null != sqrcTOQResp)
                {
                    sqrCodeTakeOrderQueryResponce.paymentAccount = sqrcTOQResp.paymentAccount;
                    sqrCodeTakeOrderQueryResponce.orderNo = sqrcTOQResp.orderNo;
                    sqrCodeTakeOrderQueryResponce.pickupStationCode = sqrcTOQResp.pickupStationCode;
                    sqrCodeTakeOrderQueryResponce.getOffStationCode = sqrcTOQResp.getOffStationCode;
                    sqrCodeTakeOrderQueryResponce.singleTicketPrice = sqrcTOQResp.singleTicketPrice;
                    sqrCodeTakeOrderQueryResponce.singleTicketNum = sqrcTOQResp.singleTicketNum;
                    sqrCodeTakeOrderQueryResponce.singleTicketType = sqrcTOQResp.singleTicketType;
                    sqrCodeTakeOrderQueryResponce.deviceId = sqrcTOQResp.deviceId;
                    sqrCodeTakeOrderQueryResponce.userName = sqrcTOQResp.userName;
                    sqrCodeTakeOrderQueryResponce.respCode = sqrcTOQResp.respCode;
                    sqrCodeTakeOrderQueryResponce.respCodeMemo = sqrcTOQResp.respCodeMemo;
                }
            }
            catch (Exception ex)
            {
                systemErrorCount++;
                _log.Error(ex.Message);
                if (null != ex.InnerException)
                {
                    _log.Error(ex.InnerException.Message);
                }
            }


            try
            {
                string strOEBResp = "respCode:" + sqrCodeTakeOrderQueryResponce.respCode + ",orderNo:" + sqrCodeTakeOrderQueryResponce.orderNo + ",pickupStationCode:" + sqrCodeTakeOrderQueryResponce.pickupStationCode + ",getOffStationCode:" + sqrCodeTakeOrderQueryResponce.getOffStationCode + ",singleTicketPrice:" + sqrCodeTakeOrderQueryResponce.singleTicketPrice + ",singleTicketNum:" + sqrCodeTakeOrderQueryResponce.singleTicketNum;
                strRespLog = String.Format("Resp:{{{0}}}", strOEBResp);
            }
            catch (Exception ex)
            {
                string strLogErr = String.Format("Resp Log Exception:{0}", ex.Message);
                _log.Error(strLogErr);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            if (sqrCodeTakeOrderQueryResponce.respCode == "0000")
            {
                _log.Info(strRespLog);
            }
            else
            {
                _log.Info(strReqLog);
                _log.Info(strRespLog);
                _log.Debug(ts.TotalMilliseconds);
            }

            return sqrCodeTakeOrderQueryResponce;
        }

    }
}
