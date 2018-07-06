using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Util
{
    /// <summary>
    /// 仅用于外部调用dll生成TradeNo唯一性测试
    /// </summary>
    public class TradeNoTest
    {
        public string GetTradeNo()
        {
            return TradeNoHelper.Instance.GetTradeNo();
        }
    }
}
