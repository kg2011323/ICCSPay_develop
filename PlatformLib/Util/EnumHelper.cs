using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformLib.Vo;

namespace PlatformLib.Util
{
    public class EnumHelper
    {
        public static List<string> GetTicketTargetTypeNameList()
        {
            List<string> ticketTargetTypeNameList = new List<string>();

            foreach (var eachTicketTargetType in Enum.GetValues(typeof(TicketTargetType)))
            {
                ticketTargetTypeNameList.Add(eachTicketTargetType.ToString());
            }

            return ticketTargetTypeNameList;
        }

        public static TicketTargetType GetTicketTargetType(string ticketTargetTypeString)
        {
            TicketTargetType ticketTargetType = (TicketTargetType)Enum.Parse(typeof(TicketTargetType), ticketTargetTypeString);

            return ticketTargetType;
        }

        public static int GetWebOrderStepFlag(WebOrderStep webOrderStep)
        {
            int webOrderStepFlag = (int)webOrderStep;

            return webOrderStepFlag;
        }

        public static string GetWebOrderStepFlagString(WebOrderStep webOrderStep)
        {
            string webOrderStepFlagString = ((int)webOrderStep).ToString();

            return webOrderStepFlagString;
        }

        public static WebOrderStep GetWebOrderStep(string webOrderStepFlagString)
        {
            WebOrderStep ticketTargetType = WebOrderStep.WebTradeNoRequest;

            try
            {
                ticketTargetType = (WebOrderStep)Enum.ToObject(typeof(WebOrderStep), int.Parse(webOrderStepFlagString));
            }
            catch (Exception)
            { }

            return ticketTargetType;
        }

        public static int GetStationOrderStepFlag(StationOrderStep stationOrderStep)
        {
            int stationOrderStepFlag = (int)stationOrderStep;

            return stationOrderStepFlag;
        }

        public static string GetStationOrderStepFlagString(StationOrderStep stationOrderStep)
        {
            string stationOrderStepFlagString = ((int)stationOrderStep).ToString();

            return stationOrderStepFlagString;
        }

        public static string GetOrderStatusTypeFlagString(OrderStatusType orderStatusType)
        {
            string orderStatusTypeFlagString = ((int)orderStatusType).ToString();

            return orderStatusTypeFlagString;
        }

        public static OrderStatusType GetOrderStatusTypeByFlagString(string orderStatusTypeFlagString)
        {
            OrderStatusType orderStatusType = (OrderStatusType)Enum.ToObject(typeof(TicketTargetType), int.Parse(orderStatusTypeFlagString));

            return orderStatusType;
        }

        public static string GetPaymentVendorCode(PaymentVendorType paymentVendorType)
        {
            return String.Format("{0:D4}", ((int)paymentVendorType));
        }
    }
}
