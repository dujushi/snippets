using Demo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class PxPayTest
    {
        [TestMethod]
        public async Task Purchase()
        {
            var px = new PxPay();
            var url = await px.Purchase(NewTxnId(), 10.00m);
            Assert.IsTrue(url.StartsWith("https://sec.paymentexpress.com"));
        }

        [TestMethod]
        public async Task ProcessResponseForPurchase()
        {
            var appSettings = ConfigurationManager.AppSettings;

            var px = new PxPay();
            var response = await px.ProcessResponse(appSettings["PxPayPurchaseReponseSample"]);
            Assert.AreEqual(1, response.Success);
        }

        [TestMethod]
        public async Task Auth()
        {
            var px = new PxPay();
            var url = await px.Auth(NewTxnId(), 10.00m);
            Assert.IsTrue(url.StartsWith("https://sec.paymentexpress.com"));
        }

        [TestMethod]
        public async Task BillSetupWithPurchase()
        {
            var px = new PxPay();
            var url = await px.BillSetup(NewTxnId(), 10.00m, "", true);
            Assert.IsTrue(url.StartsWith("https://sec.paymentexpress.com"));
        }

        [TestMethod]
        public async Task BillSetupWithAuth()
        {
            var px = new PxPay();
            var url = await px.BillSetup(NewTxnId(), 10.00m);
            Assert.IsTrue(url.StartsWith("https://sec.paymentexpress.com"));
        }

        [TestMethod]
        public async Task ProcessResponseForBillSetup()
        {
            var appSettings = ConfigurationManager.AppSettings;

            var px = new PxPay();
            var response = await px.ProcessResponse(appSettings["PxPayBillSetupResponseSample"]);
            Assert.AreEqual(1, response.Success);
            Assert.AreNotEqual("", response.DpsBillingId);
        }

        [TestMethod]
        public async Task Rebill()
        {
            var appSettings = ConfigurationManager.AppSettings;

            var px = new PxPay();
            var url = await px.Rebill(NewTxnId(), 10.00m, appSettings["PxPayDpsBillingIdSample"]);
            Assert.IsTrue(url.StartsWith("https://sec.paymentexpress.com"));
        }

        private string NewTxnId()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
        }
    }
}
