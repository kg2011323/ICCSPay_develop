using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SLEWebServiceTest.Util;
using System.Diagnostics;

namespace SLEWebServiceTest.View
{
    public partial class StationOrderForm : TestBaseForm
    {
        private SLEWebService.myWebService1SoapClient _webService = null;

        public StationOrderForm()
        {
            InitializeComponent();

            tbxBaseDeviceId = tbxDeviceId;

            channelType = "01";

            tbxFaultSlipSeq.Text = Guid.NewGuid().ToString().Substring(0, 32);
        }

        private void btnS1001_Click(object sender, EventArgs e)
        {
            TestS1001();
        }

        private void btnS1002_Click(object sender, EventArgs e)
        {
            TestS1002();
        }

        private void btnS1003_Click(object sender, EventArgs e)
        {
            TestS1003();
        }

        private void btnS1004_Click(object sender, EventArgs e)
        {
            TestS1004();
        }

        private void btnS1005_Click(object sender, EventArgs e)
        {
            TestS1005();
        }

        private void TestS1001()
        {
            try
            {
                 _webService = new SLEWebService.myWebService1SoapClient(Constants.WebServiceEndPointConfigName, Constants.WebServiceURL);
                SLEWebService.OrderCommitReq req = new SLEWebService.OrderCommitReq();
                req.reqSysDate = reqSysDate;
                req.operationCode = "S1-001";
                req.cityCode = cityCode;
                req.deviceId = deviceId;
                req.channelType = channelType;
                req.paymentCode = tbxPaymentCode.Text.Trim();
                req.msisdn = String.Empty;
                req.iccid = String.Empty;
                req.serviceId = String.Empty;
                req.paymentVendor = tbxPaymentVendor.Text.Trim();
                req.pickupStationCode = tbxPickupStationCode.Text.Trim();
                req.getOffStationCode = tbxGetOffStationCode.Text.Trim();
                req.singleTicketPrice = tbxSinglelTicketPrice.Text.Trim();
                req.singleTicketNum = tbxSinglelTicketNum.Text.Trim();
                req.singleTicketType = rdbtnSingleTicketType0.Checked ? "0" : "1";

                Stopwatch swWS = new Stopwatch();
                swWS.Start(); 

                SLEWebService.OrderCommitResp resp = _webService.S1_001(req);

                swWS.Stop();
                TimeSpan tsWS = swWS.Elapsed;
                rtbS1001.AppendText(String.Format("耗时ms:{0}", ((int)tsWS.TotalMilliseconds)));
                rtbS1001.AppendText(Environment.NewLine);

                if (null != resp)
                {
                    string strResp = LogHelper.GetObjectMemberString(resp);
                    rtbS1001.AppendText(strResp);
                    rtbS1001.AppendText(Environment.NewLine);

                    tbxOrderNo.Text = resp.orderNo;
                }
            }
            catch (Exception ex)
            {
                rtbS1001.AppendText(ex.Message);
                rtbS1001.AppendText(Environment.NewLine);
            }
        }

        private void TestS1002()
        {
            try
            {
                _webService = new SLEWebService.myWebService1SoapClient(Constants.WebServiceEndPointConfigName, Constants.WebServiceURL);
                SLEWebService.QueryPaymentResultReq req = new SLEWebService.QueryPaymentResultReq();
                req.reqSysDate = reqSysDate;
                req.operationCode = "S1-002";
                req.cityCode = cityCode;
                req.deviceId = deviceId;
                req.channelType = channelType;
                req.orderNo = tbxOrderNo.Text.Trim();

                Stopwatch swWS = new Stopwatch();
                swWS.Start();

                SLEWebService.QueryPaymentResultResp resp = _webService.S1_002(req);

                swWS.Stop();
                TimeSpan tsWS = swWS.Elapsed;
                rtbS1002.AppendText(String.Format("耗时ms:{0}", ((int)tsWS.TotalMilliseconds)));
                rtbS1002.AppendText(Environment.NewLine);

                if (null != resp)
                {
                    string strResp = LogHelper.GetObjectMemberString(resp);
                    rtbS1002.AppendText(strResp);
                    rtbS1002.AppendText(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                rtbS1002.AppendText(ex.Message);
                rtbS1002.AppendText(Environment.NewLine);
            }
        }

        private void TestS1003()
        {
            try
            {
                _webService = new SLEWebService.myWebService1SoapClient(Constants.WebServiceEndPointConfigName, Constants.WebServiceURL);
                SLEWebService.OrderExecuteBeginReq req = new SLEWebService.OrderExecuteBeginReq();
                req.reqSysDate = reqSysDate;
                req.operationCode = "S1-003";
                req.cityCode = cityCode;
                req.deviceId = deviceId;
                req.channelType = channelType;
                req.orderNo = tbxOrderNo.Text.Trim();

                Stopwatch swWS = new Stopwatch();
                swWS.Start();

                SLEWebService.OrderExecuteBeginResp resp = _webService.S1_003(req);

                swWS.Stop();
                TimeSpan tsWS = swWS.Elapsed;
                rtbS1003.AppendText(String.Format("耗时ms:{0}", ((int)tsWS.TotalMilliseconds)));
                rtbS1003.AppendText(Environment.NewLine);

                if (null != resp)
                {
                    string strResp = LogHelper.GetObjectMemberString(resp);
                    rtbS1003.AppendText(strResp);
                    rtbS1003.AppendText(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                rtbS1003.AppendText(ex.Message);
                rtbS1003.AppendText(Environment.NewLine);
            }
        }

        private void TestS1004()
        {
            try
            {
                _webService = new SLEWebService.myWebService1SoapClient(Constants.WebServiceEndPointConfigName, Constants.WebServiceURL);
                SLEWebService.OrderExecuteResultReq req = new SLEWebService.OrderExecuteResultReq();
                req.reqSysDate = reqSysDate;
                req.operationCode = "S1-004";
                req.cityCode = cityCode;
                req.deviceId = deviceId;
                req.channelType = channelType;
                req.orderNo = tbxOrderNo.Text.Trim();
                req.takeSingleTicketNum = Convert.ToInt32(tbxTakeSingleTicketNum004.Text.Trim());
                req.takeSingleTicketDate = reqSysDate;

                Stopwatch swWS = new Stopwatch();
                swWS.Start();

                SLEWebService.OrderExecuteResultResp resp = _webService.S1_004(req);

                swWS.Stop();
                TimeSpan tsWS = swWS.Elapsed;
                rtbS1004.AppendText(String.Format("耗时ms:{0}", ((int)tsWS.TotalMilliseconds)));
                rtbS1004.AppendText(Environment.NewLine);

                if (null != resp)
                {
                    string strResp = LogHelper.GetObjectMemberString(resp);
                    rtbS1004.AppendText(strResp);
                    rtbS1004.AppendText(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                rtbS1004.AppendText(ex.Message);
                rtbS1004.AppendText(Environment.NewLine);
            }
        }

        private void TestS1005()
        {
            try
            {
                _webService = new SLEWebService.myWebService1SoapClient(Constants.WebServiceEndPointConfigName, Constants.WebServiceURL);
                SLEWebService.OrderExecuteFaultReq req = new SLEWebService.OrderExecuteFaultReq();
                req.reqSysDate = reqSysDate;
                req.operationCode = "S1-005";
                req.cityCode = cityCode;
                req.deviceId = deviceId;
                req.channelType = channelType;
                req.orderNo = tbxOrderNo.Text.Trim();
                req.takeSingleTicketNum = Convert.ToInt32(tbxTakeSingleTicketNum005.Text.Trim());
                req.faultOccurDate = reqSysDate;
                req.faultSlipSeq = tbxFaultSlipSeq.Text.Trim();
                req.erroCode = tbxErrorCode.Text.Trim();
                req.errorMessage = tbxErrorMessage.Text.Trim();

                Stopwatch swWS = new Stopwatch();
                swWS.Start();

                SLEWebService.OrderExecuteFaultResp resp = _webService.S1_005(req);

                swWS.Stop();
                TimeSpan tsWS = swWS.Elapsed;
                rtbS1005.AppendText(String.Format("耗时ms:{0}", ((int)tsWS.TotalMilliseconds)));
                rtbS1005.AppendText(Environment.NewLine);

                if (null != resp)
                {
                    string strResp = LogHelper.GetObjectMemberString(resp);
                    rtbS1005.AppendText(strResp);
                    rtbS1005.AppendText(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                rtbS1005.AppendText(ex.Message);
                rtbS1005.AppendText(Environment.NewLine);
            }
        }

        
    }
}
