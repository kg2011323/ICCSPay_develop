using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PlatformLib.Vo;
using PlatformLib.BLL;

namespace InfoWebInterface
{
    public partial class InfoWebInterface : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (0 < Request.QueryString.Count)
                {
                    ITPayResultRequestVo itPayResultRequestVo = new ITPayResultRequestVo();
                    itPayResultRequestVo.TradeNo = Request.QueryString["trade_no"];
                    itPayResultRequestVo.TransactionId = Request.QueryString["transaction_id"];
                    itPayResultRequestVo.UserOpenId = Request.QueryString["user_id"];
                    itPayResultRequestVo.OriAFCStationCode = Request.QueryString["begin_station"];
                    itPayResultRequestVo.DesAFCStationCode = Request.QueryString["end_station"];
                    itPayResultRequestVo.TicketPrice = Request.QueryString["ticket_price"];
                    itPayResultRequestVo.TicketNum = Request.QueryString["ticket_num"];
                    itPayResultRequestVo.ActualFee = Request.QueryString["total_fee"];
                    itPayResultRequestVo.PayEndTime = Request.QueryString["pay_time"];
                    itPayResultRequestVo.PayOperator = Request.QueryString["pay_operator"];
                    itPayResultRequestVo.BankType = Request.QueryString["bank_type"];
                    itPayResultRequestVo.Target = Request.QueryString["target"];
                    itPayResultRequestVo.ErrCodeDes = Request.QueryString["err_code_des"];

                    WebPreOrderBo bo = new WebPreOrderBo();
                    ITPayResultRespondVo itPayResultRespondVo = bo.ITPayResultRecord(itPayResultRequestVo);

                    string json = String.Format("{{\"trade_no\":\"{0}\",\"voucher\":\"{1}\",\"err_status\":\"{2}\"}}"
                        , itPayResultRespondVo.TradeNo
                        , itPayResultRespondVo.Voucher
                        , itPayResultRespondVo.ErrStatus);
                    Response.Clear();
                    Response.ContentType = "application/json; charset=utf-8";
                    Response.Write(json);
                    Response.End();
                }
            }
            catch (Exception ex)
            { }
        }
    }
}