using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLEWebService.Vo
{
    public class ApplePayDealInsertReq:Request
    {
        //请求对象的字段
        private string _conversationId;
        private string _PAN;
        private string _transactionAmount;
        private string _transactionCurrencyCode;
        private string _transactionTime;
        private string _authorizationResponseIdentificationCode;
        private string _retrievalReferNumber;
        private string _terminalNo;
        private string _merchantCodeId;
        private string _applicationCryptogram;
        private string _inputModeCode;
        private string _cardSerialNumber;
        private string _terminalReadAbility;
        private string _cardConditionCode;
        private string _terminalPerformance;
        private string _terminalVerificationResults;
        private string _unpredictableNumber;
        private string _interfaceEquipmentSerialNumber;
        private string _issuerApplicationData;
        private string _applicationTradeCounter;
        private string _applicationInterchangeProfile;
        private string _transactionDate;
        private string _terminalCountryCode;
        private string _responseCode;
        private string _transactionType;
        private string _authorizeAmount;
        private string _tradingCurrencyCode;
        private string _cipherCheckResult;
        private string _cardValidPeriod;
        private string _cryptogramInformationData;
        private string _otherAmount;
        private string _cardHolderVerificationMethod;
        private string _terminalType;
        private string _professFileName;
        private string _applicationVersion;
        private string _tradingSequenceCounter;
        private string _ecissueAuthorizationCode;
        private string _productIdentificationInformation;
        private string _cardType;
        private string _paymentVendor;



        //操作基类属性
        public string reqSysDate
        {
            get { return _reqSysDate; }
            set { _reqSysDate = value; }
        }

        public string operationCode
        {
            get { return _operationCode; }
            set { _operationCode = value; }
        }

        public string cityCode
        {
            get { return _cityCode; }
            set { _cityCode = value; }
        }

        public string deviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        public string channelType
        {
            get { return _channelType; }
            set { _channelType = value; }
        }

        public List<string> expandAttribute
        {
            get { return _expandAttribute; }
            set { _expandAttribute = value; }
        }

        //操作自身属性
        public string conversationId
        {
            get { return _conversationId; }
            set { _conversationId = value; }
        }

        public string PAN
        {
            get { return _PAN; }
            set { _PAN = value; }
        }

        public string transactionAmount
        {
            get { return _transactionAmount; }
            set { _transactionAmount = value; }
        }
        public string transactionCurrencyCode
        {
            get { return _transactionCurrencyCode; }
            set { _transactionCurrencyCode = value; }
        }
        public string transactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }
        public string authorizationResponseIdentificationCode
        {
            get { return _authorizationResponseIdentificationCode; }
            set { _authorizationResponseIdentificationCode = value; }
        }
        public string retrievalReferNumber
        {
            get { return _retrievalReferNumber; }
            set { _retrievalReferNumber = value; }
        }
        public string terminalNo
        {
            get { return _terminalNo; }
            set { _terminalNo = value; }
        }
        public string merchantCodeId
        {
            get { return _merchantCodeId; }
            set { _merchantCodeId = value; }
        }
        public string applicationCryptogram
        {
            get { return _applicationCryptogram; }
            set { _applicationCryptogram = value; }
        }
        public string inputModeCode
        {
            get { return _inputModeCode; }
            set { _inputModeCode = value; }
        }
        public string cardSerialNumber
        {
            get { return _cardSerialNumber; }
            set { _cardSerialNumber = value; }
        }
        public string terminalReadAbility
        {
            get { return _terminalReadAbility; }
            set { _terminalReadAbility = value; }
        }
        public string cardConditionCode
        {
            get { return _cardConditionCode; }
            set { _cardConditionCode = value; }
        }
        public string terminalPerformance
        {
            get { return _terminalPerformance; }
            set { _terminalPerformance = value; }
        }
        public string terminalVerificationResults
        {
            get { return _terminalVerificationResults; }
            set { _terminalVerificationResults = value; }
        }
        public string unpredictableNumber
        {
            get { return _unpredictableNumber; }
            set { _unpredictableNumber = value; }
        }
        public string interfaceEquipmentSerialNumber
        {
            get { return _interfaceEquipmentSerialNumber; }
            set { _interfaceEquipmentSerialNumber = value; }
        }
        public string issuerApplicationData
        {
            get { return _issuerApplicationData; }
            set { _issuerApplicationData = value; }
        }
        public string applicationTradeCounter
        {
            get { return _applicationTradeCounter; }
            set { _applicationTradeCounter = value; }
        }
        public string applicationInterchangeProfile
        {
            get { return _applicationInterchangeProfile; }
            set { _applicationInterchangeProfile = value; }
        }
        public string transactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
        }
        public string terminalCountryCode
        {
            get { return _terminalCountryCode; }
            set { _terminalCountryCode = value; }
        }
        public string responseCode
        {
            get { return _responseCode; }
            set { _responseCode = value; }
        }
        public string transactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }
        public string authorizeAmount
        {
            get { return _authorizeAmount; }
            set { _authorizeAmount = value; }
        }
        public string tradingCurrencyCode
        {
            get { return _tradingCurrencyCode; }
            set { _tradingCurrencyCode = value; }
        }
        public string cipherCheckResult
        {
            get { return _cipherCheckResult; }
            set { _cipherCheckResult = value; }
        }
        public string cardValidPeriod
        {
            get { return _cardValidPeriod; }
            set { _cardValidPeriod = value; }
        }
        public string cryptogramInformationData
        {
            get { return _cryptogramInformationData; }
            set { _cryptogramInformationData = value; }
        }
        public string otherAmount
        {
            get { return _otherAmount; }
            set { _otherAmount = value; }
        }
        public string cardHolderVerificationMethod
        {
            get { return _cardHolderVerificationMethod; }
            set { _cardHolderVerificationMethod = value; }
        }
        public string terminalType
        {
            get { return _terminalType; }
            set { _terminalType = value; }
        }
        public string professFileName
        {
            get { return _professFileName; }
            set { _professFileName = value; }
        }
        public string applicationVersion
        {
            get { return _applicationVersion; }
            set { _applicationVersion = value; }
        }
        public string tradingSequenceCounter
        {
            get { return _tradingSequenceCounter; }
            set { _tradingSequenceCounter = value; }
        }
        public string ecissueAuthorizationCode
        {
            get { return _ecissueAuthorizationCode; }
            set { _ecissueAuthorizationCode = value; }
        }
        public string productIdentificationInformation
        {
            get { return _productIdentificationInformation; }
            set { _productIdentificationInformation = value; }
        }
        public string cardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }
        public string paymentVendor
        {
            get { return _paymentVendor; }
            set { _paymentVendor = value; }
        }
    }
}