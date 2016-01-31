using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Demo.Services
{
    public class PxPay
    {
        public enum TxnType { Auth, Purchase };
        private Uri Uri => new Uri(GetAppSetting("PxPayUri"));
        private string UserId => GetAppSetting("PxPayUserId");
        private string Key => GetAppSetting("PxPayKey");
        private string UrlSuccess => GetAppSetting("PxPayUrlSuccess");
        private string UrlFail => GetAppSetting("PxPayUrlFail");
        private string _CurrentInput;
        public string CurrencyInput
        {
            get
            {
                if (_CurrentInput == null)
                {
                    _CurrentInput = GetAppSetting("PxPayDefaultCurrencyInput");
                }

                return _CurrentInput;
            }
            set
            {
                _CurrentInput = value;
            }
        }

        public class Response
        {
            public int Success { get; set; }
            public string TxnType { get; set; }
            public string TxnId { get; set; }
            public string DpsTxnRef { get; set; }
            public string DpsBillingId { get; set; }
            public string CurrencyInput { get; set; } 
            public string MerchantReference { get; set; }
            public string AuthCode { get; set; }
            public string CardName { get; set; }
            public string CardHolderName { get; set; }
            public string CardNumber { get; set; }
            public string DateExpiry { get; set; }
            public string ClientInfo { get; set; }
            public decimal AmountSettlement { get; set; }
            public string CurrencySettlement { get; set; }
            public string DateSettlement { get; set; }
            public string TxnMac { get; set; }
            public string ResponseText { get; set; }
            public string CardNumber2 { get; set; }
            public string Cvc2ResultCode { get; set; }
        }

        public PxPay() {}

        public PxPay(string currencyInput)
        {
            CurrencyInput = currencyInput;
        }

        public async Task<string> GenerateRequest(TxnType txnType, string txnId, decimal amountInput, 
            string merchantReference, bool enableAddBillCard, string dpsBillingId)
        {
            var data = new Dictionary<string, string>
            {
                {"TxnType", txnType.ToString()},
                {"AmountInput", amountInput.ToString()},
                {"TxnId", txnId},
                {"DpsBillingId", dpsBillingId},
                {"EnableAddBillCard", enableAddBillCard ? "1" : "0"},
                {"MerchantReference", HttpUtility.HtmlEncode(merchantReference)},
                {"CurrencyInput", CurrencyInput},
                {"UrlSuccess", UrlSuccess}, 
                {"UrlFail", UrlFail}
            };

            var xml = await SendRequest("GenerateRequest", data);

            if (xml.Element("URI") == null)
            {
                Trace.TraceError($"GenerateRequest Error. Response: {xml}");
                throw new PxPayException("GenerateRequest Error", xml.Element("Reco").Value, xml.Element("ResponseText").Value);
            } else if (xml.Attribute("valid").Value != "1")
            {
                Trace.TraceError($"GenerateRequest Invalid. Response: {xml}");
                throw new PxPayException("GenerateRequest Invalid", "", xml.Element("URI").Value);
            }

            return xml.Element("URI").Value;
        }

        /*
         * for purchase and bill setup
         */
        public async Task<string> Purchase(string txnId, decimal amountInput, string merchantReference = "", 
            bool enableAddBillCard = false)
        {
            return await GenerateRequest(TxnType.Purchase, txnId, amountInput, merchantReference, false, "");
        }

        /*
         * for rebill
        */
        public async Task<string> Purchase(string txnId, decimal amountInput, string dpsBillingId, 
            string merchantReference = "")
        {
            return await GenerateRequest(TxnType.Purchase, txnId, amountInput, merchantReference, false, dpsBillingId);
        }

        public async Task<string> Auth(string txnId, decimal amountInput, string merchantReference = "", 
            bool enableAddBillCard = false)
        {
            return await GenerateRequest(TxnType.Auth, txnId, amountInput, merchantReference, enableAddBillCard, "");
        }

        public async Task<string> BillSetup(string txnId, decimal amountInput, string merchantReference = "", bool withPurchase = false)
        {
            if (withPurchase)
            {
                return await Purchase(txnId, amountInput, merchantReference, true);
            }
            else
            {
                return await Auth(txnId, amountInput, merchantReference, true);
            }
            
        }

        public async Task<string> Rebill(string txnId, decimal amountInput, string dpsBillingId, 
            string merchantReference = "")
        {
            return await Purchase(txnId, amountInput, dpsBillingId, merchantReference);
        }

        public async Task<Response> ProcessResponse(string response)
        {
            var data = new Dictionary<string, string>
            {
                {"Response", response}
            };

            var xml = await SendRequest("ProcessResponse", data);
            if (xml.Attribute("valid").Value != "1")
            {
                Trace.TraceError($"ProcessResponse Invalid. Response: {xml}");
                throw new PxPayException("ProcessResponse Invalid", xml.Element("Reco").Value, xml.Element("ResponseText").Value);
            }
            var serializer = new XmlSerializer(typeof(Response));
            return (Response)serializer.Deserialize(xml.CreateReader());
        }

        private async Task<XElement> SendRequest(string type, Dictionary<string, string> data)
        {
            XElement responseXml = null;

            using (var client = new HttpClient())
            {
                data["PxPayUserId"] = UserId;
                data["PxPayKey"] = Key;

                var requestXml = new XElement(type, data.Select(d => new XElement(d.Key, d.Value)).ToArray());
                var requestContent = new StringContent(requestXml.ToString(), Encoding.UTF8, "application/xml");
                HttpResponseMessage response = await client.PostAsync(Uri, requestContent);
                if (!response.IsSuccessStatusCode)
                {
                    Trace.TraceError($"Request failed. Response: {response}");
                    throw new PxPayException("Request failed");
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    responseXml = XElement.Parse(responseContent);
                }
            }

            return responseXml;
        }

        private string GetAppSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            if (string.IsNullOrEmpty(appSettings[key]))
            {
                throw new PxPayException("AppSettings Error", "", key);
            }
            return appSettings[key];
        }
    }

    public class PxPayException: Exception
    {
        public string Reco { get; set; }
        public string ResponseText { get; set; }

        public PxPayException() : base() { }
        public PxPayException(string message) : base(message) {}
        public PxPayException(string message, string reco, string responseText) : base(message) {
            Reco = reco;
            ResponseText = responseText;
        }
        public PxPayException(string message, Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return $"{Message}. Reco: {Reco}. ResponseText: {ResponseText}.";
        }
    }
}