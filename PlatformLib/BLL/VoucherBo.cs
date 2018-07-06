using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using log4net;
using PlatformLib.DB;
using PlatformLib.Util;
using System.Diagnostics;


namespace PlatformLib.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class VoucherBo
    {
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public bool UnlockAllVoucher()
        {
            bool isSuccess = false;

            try
            {
                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                {
                    var lockedVouchers = dbContext.VoucherLists.Where(v => v.IsLocked.Equals(true));
                    foreach (VoucherList eachVoucher in lockedVouchers)
                    {
                        eachVoucher.IsLocked = false;
                    }

                    dbContext.SaveChanges();

                    isSuccess = true;
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

            return isSuccess;
        }

        /// <summary>
        /// 生成可用取票凭证
        /// </summary>
        /// <param name="fillCount">范围为1到1,000,000</param>
        /// <param name="effectiveTime"></param>
        /// <param name="expiredTime"></param>
        /// <returns></returns>
        public int FillNewVoucher(int fillCount, DateTime effectiveTime, DateTime expiredTime)
        {
            string strLog = String.Format("Generate new voucher:{0}, from {1} to {2}"
                , fillCount
                , TimeHelper.GetTimeStringYyyyMMddHHmmss(effectiveTime)
                , TimeHelper.GetTimeStringYyyyMMddHHmmss(expiredTime));
            _log.Info(strLog);

            if ((fillCount < 1)
                || (fillCount > 1000000))
            {
                throw new ArgumentOutOfRangeException("fillCount");
            }
            if (0 < effectiveTime.CompareTo(expiredTime))
            {
                throw new ArgumentException("expiredTime must larger then effectiveTime");
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            int filledCount = 0;


            try
            {
                VoucherHelper voucherHelper = new VoucherHelper();
                // 为免重复生成双倍随机数
                int doublefillCount = fillCount * 2;
                // 不重复随机数
                List<string> noRepeatRandomNumList = voucherHelper.GetNoRepeatRandomNumList(doublefillCount, Constants.VoucherLength);
                // 已有随机数字典
                Dictionary<string, string> dictExistVoucherCode = new Dictionary<string, string>();
                // 需添加的新随机数列表，与已有随机数不重复
                List<VoucherList> toAddVoucherList = new List<VoucherList>();
                using (MobilePayDBEntities dbContext = new MobilePayDBEntities())
                {
                    // 填充 已有随机数字典
                    foreach (VoucherList eachVoucher in dbContext.VoucherLists.ToList())
                    {
                        string eachExistVoucherCode = eachVoucher.VoucherCode;
                        dictExistVoucherCode.Add(eachExistVoucherCode, eachExistVoucherCode);
                    }

                    // 填充 需添加的新随机数列表
                    int newVoucherCodeCount = 0;
                    foreach (string eachNewVoucherCode in noRepeatRandomNumList)
                    {
                        if (!dictExistVoucherCode.ContainsKey(eachNewVoucherCode))
                        {
                            VoucherList newVoucherList = new VoucherList();
                            newVoucherList.VoucherId = Guid.NewGuid();
                            newVoucherList.VoucherCode = eachNewVoucherCode;
                            newVoucherList.EffectiveTime = effectiveTime;
                            newVoucherList.ExpiredTime = expiredTime;
                            newVoucherList.IsValid = true;
                            newVoucherList.IsUsed = false;
                            newVoucherList.IsLocked = false;

                            toAddVoucherList.Add(newVoucherList);

                            newVoucherCodeCount++;
                            if (fillCount <= newVoucherCodeCount)
                            {
                                break;
                            }
                        }
                    }

                    DateTime dtGenerateTime = DateTime.Now;
                    foreach (VoucherList eachNewVoucher in toAddVoucherList)
                    {
                        eachNewVoucher.GenerateTime = dtGenerateTime;
                        dbContext.VoucherLists.AddObject(eachNewVoucher);
                    }
                    filledCount = dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            _log.Info(ts.TotalMilliseconds);

            return filledCount;
        }


        
    }
}
