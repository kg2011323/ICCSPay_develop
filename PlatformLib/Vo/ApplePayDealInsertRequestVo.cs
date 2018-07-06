using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformLib.Vo
{
    public class ApplePayDealInsertRequestVo : DeviceCommRequestBaseVo
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string ConversationId;
        /// <summary>
        /// 主账号
        /// </summary>
        public string PAN;
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TransactionAmount;
        /// <summary>
        /// 交易货币代码
        /// </summary>
        public string TransactionCurrencyCode;
        /// <summary>
        /// 交易时间，格式为MMddhhmmss
        /// </summary>
        public string TransactionTimeStringMMddhhmmss;
        /// <summary>
        /// 授权应答标识码
        /// </summary>
        public string AuthorizationResponseIdentificationCode;
        /// <summary>
        /// 检索参考号
        /// </summary>
        public string RetrievalReferNumber;
        /// <summary>
        /// 受卡机终端标识码
        /// </summary>
        public string TerminalNo;
        /// <summary>
        /// 受卡方标识码
        /// </summary>
        public string MerchantCodeId;
        /// <summary>
        /// 应用密文
        /// </summary>
        public string ApplicationCryptogram;
        /// <summary>
        /// 服务点输入方式码
        /// </summary>
        public string InputmodeCode;
        /// <summary>
        /// 卡片序列号
        /// </summary>
        public string CardserialNumber;
        /// <summary>
        /// 终端读取能力
        /// </summary>
        public string TerminalReadability;
        /// <summary>
        /// IC卡条件代码
        /// </summary>
        public string CardconditionCode;
        /// <summary>
        /// 终端性能
        /// </summary>
        public string TerminalPerformance;
        /// <summary>
        /// 终端验证结果
        /// </summary>
        public string TerminalVerificationResults;
        /// <summary>
        /// 不可预知数
        /// </summary>
        public string UnpredictableNumber;
        /// <summary>
        /// 接口设备序列号
        /// </summary>
        public string InterfaceEquipmentSerialNumber;
        /// <summary>
        /// 发卡行应用数据
        /// </summary>
        public string IssuerapplicationData;
        /// <summary>
        /// 应用交易记数器
        /// </summary>
        public string ApplicationTradeCounter;
        /// <summary>
        /// 应用交互特征
        /// </summary>
        public string ApplicationInterchangeProfile;
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TransactionDate;
        /// <summary>
        /// 终端国家代码
        /// </summary>
        public string terminalcountryCode;
        /// <summary>
        /// 交易响应码
        /// </summary>
        public string ResponseCode;
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TransactionType;
        /// <summary>
        /// 授权金额
        /// </summary>
        public string AuthorizeAmount;
        /// <summary>
        /// 交易币种代码
        /// </summary>
        public string TradingCurrencyCode;
        /// <summary>
        /// 应用密文校验结果
        /// </summary>
        public string CipherCheckResult;
        /// <summary>
        /// 卡有效期
        /// </summary>
        public string CardValidPeriod;
        /// <summary>
        /// 密文信息数据
        /// </summary>
        public string CryptogramInformationData;
        /// <summary>
        /// 其它金额
        /// </summary>
        public string OtherAmount;
        /// <summary>
        /// 持卡人验证方法结果
        /// </summary>
        public string CardholderVerificationMethod;
        /// <summary>
        /// 终端类型
        /// </summary>
        public string TerminalType;
        /// <summary>
        /// 专用文件名称
        /// </summary>
        public string ProfessFilename;
        /// <summary>
        /// 应用版本号
        /// </summary>
        public string ApplicationVersion;
        /// <summary>
        /// 交易序列计数器
        /// </summary>
        public string TradingSequenceCounter;
        /// <summary>
        /// 电子现金发卡行授权码
        /// </summary>
        public string EcissueAuthorizationCode;
        /// <summary>
        /// 卡产品标识信息
        /// </summary>
        public string ProductIdentificationInformation;
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardType;
        /// <summary>
        /// 支付通道
        /// </summary>
        public string PaymentCode;
    }
}
