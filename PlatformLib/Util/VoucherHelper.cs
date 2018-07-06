using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using log4net;
using PlatformLib.DB;

namespace PlatformLib.Util
{
    /// <summary>
    /// 取票凭证处理类
    /// </summary>
    public class VoucherHelper
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxRange">最大范围M，设置后由1到M共M个</param>
        /// <param name="randomNumCount">随机数个数，必须大于0且等于小于最大范围M，否则返回结果为空</param>
        /// <returns></returns>
        public int[] GetNoRepeatRandomNums(int maxRange, int randomNumCount)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int[] result = new int[0];

            try
            {
                if ((0 < maxRange)
                    && (0 < randomNumCount)
                    && (randomNumCount <= maxRange))
                {
                    // 随机数处理字符串
                    int[] randomNums = new int[maxRange];
                    for (int index = 0; index < maxRange; index++)
                    {
                        randomNums[index] = (index + 1);
                    }

                    Random rnd = new Random();
                    int maxIndex = maxRange - 1;
                    for (int index = 0; index < randomNumCount; index++)
                    {
                        int swapIndex = rnd.Next((index + 1), maxIndex);

                        int temp = randomNums[index];
                        randomNums[index] = randomNums[swapIndex];
                        randomNums[swapIndex] = temp;

                        //// debug
                        //Console.WriteLine(String.Format("{0}:{1}", index, randomNums[index]));
                    }

                    // 返回结果            
                    result = new int[randomNumCount];
                    for (int index = 0; index < randomNumCount; index++)
                    {
                        result[index] = randomNums[index];
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


            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            //// debug
            //Console.WriteLine("SwapGenerator:{0}ms.", ts2.TotalMilliseconds);

            return result;
        }

        /// <summary>
        /// 生成指定位数和个数的随机数列表
        /// 按8位一组随机数拼接
        /// </summary>
        /// <param name="randomNumCount">随机数个数，最大为99999999</param>
        /// <param name="randomNumDigitCount">随机数位数，最大32</param>
        /// <returns></returns>
        public List<string> GetNoRepeatRandomNumList(int randomNumCount, int randomNumDigitCount)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<string> noRepeatRandomNumList = new List<string>();

            // 最大输出位数
            int maxDigitCount = 32;
            // 输出随机数最大个数，也是每组最大范围
            int maxRange = 99999999;            
            // 每组位数
            int eachGroupMaxRangeDigitCount = 8;
            // 每组格式化字符串
            string eachGroupFormatString = String.Format("{{0:D{0}}}", eachGroupMaxRangeDigitCount);

            if ((0 < randomNumCount)
                && (maxRange >= randomNumCount))
            {
                try
                {
                    // 输出个数范围位数
                    int randomNumRangeDigitCount = randomNumCount.ToString().Length;

                    if ((0 < randomNumRangeDigitCount)
                        && (randomNumRangeDigitCount <= maxDigitCount))
                    {

                        // 需生成随机数组数，用于拼接
                        int randomGroupCount = Convert.ToInt32( Math.Ceiling(randomNumDigitCount / Convert.ToDouble(eachGroupMaxRangeDigitCount)));
                       
                        // 填充每组随机数
                        int[,] randomNumGroups = new int[randomGroupCount, randomNumCount];
                        for (int groupIndex = 0; groupIndex < randomGroupCount; groupIndex++)
                        {
                            int[] currentNoRepeatRandomNums = GetNoRepeatRandomNums(maxRange, randomNumCount);
                            for (int randomNumIndex = 0; randomNumIndex < currentNoRepeatRandomNums.Length; randomNumIndex++)
                            {
                                randomNumGroups[groupIndex, randomNumIndex] = currentNoRepeatRandomNums[randomNumIndex];
                            }
                        }
                        
                        // 填充每个随机数
                        for (int randomNumIndex = 0; randomNumIndex < randomNumCount; randomNumIndex++)
                        {
                            StringBuilder sbRandomNumCombine = new StringBuilder();
                            for (int groupIndex = 0; groupIndex < randomGroupCount; groupIndex++)
                            {
                                sbRandomNumCombine.Append(String.Format(eachGroupFormatString, randomNumGroups[groupIndex, randomNumIndex]));                                
                            }
                            noRepeatRandomNumList.Add(sbRandomNumCombine.ToString().Substring(0, randomNumDigitCount));
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
            }

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            // debug
            Console.WriteLine("GetNoRepeatRandomNumList:{0}sec.", ts2.TotalSeconds);


            return noRepeatRandomNumList;
        }

        public static string VoucherCodeCombine(WebOrder webOrder, string voucher)
        {
            string voucherCode = String.Empty;

            if (null != webOrder
                && !String.IsNullOrEmpty(voucher))
            {
                try
                {
                    Dictionary<string, string> dictFlagValue = new Dictionary<string, string>();
                    dictFlagValue.Add("site_begin", webOrder.OriAFCStationCode);
                    dictFlagValue.Add("site_end", webOrder.DesAFCStationCode);
                    dictFlagValue.Add("ticket_num", ((int)webOrder.TicketNum).ToString());
                    dictFlagValue.Add("ticket_price", ((int)webOrder.TicketPrice).ToString());
                    dictFlagValue.Add("total_fee", ((int)webOrder.ActualFee).ToString());
                    dictFlagValue.Add("trade_no", webOrder.TradeNo);
                    dictFlagValue.Add("ticket_no", voucher);
                    
                    // 按二维码规则广州地铁生成的一定是gzmtr
                    dictFlagValue.Add("source", "gzmtr");
                    // 适应广电错误临时处理
                    //dictFlagValue.Add("source", "1001");
                    
                    dictFlagValue.Add("target", webOrder.TicketTarget);

                    StringBuilder sbVoucherCode = new StringBuilder();
                    foreach (KeyValuePair<string, string> eachFlagValue in dictFlagValue)
                    {
                        sbVoucherCode.Append("&");
                        sbVoucherCode.Append(eachFlagValue.Key);
                        sbVoucherCode.Append("=");
                        sbVoucherCode.Append(eachFlagValue.Value);
                    }
                    string processVoucherCode = sbVoucherCode.ToString();
                    if (processVoucherCode.StartsWith("&"))
                    {
                        voucherCode = processVoucherCode.Substring(1, (processVoucherCode.Length - 1));
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
            }

            return voucherCode;
        }
    }
}
