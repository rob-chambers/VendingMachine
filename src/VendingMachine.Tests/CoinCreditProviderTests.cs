using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class CoinCreditProviderTests
    {
        [TestMethod]
        public void SupplyingCoinsIncreasesCredit()
        {
            var provider = new CoinCreditProvider();
            provider.InsertCoin(CoinDenomination.FiftyPence);
            provider.InsertCoin(CoinDenomination.TenPence);
            provider.InsertCoin(CoinDenomination.FivePence);

            Assert.AreEqual(0.65M, provider.Total);
        }
    }
}
