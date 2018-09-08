using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class CoinDenominationTests
    {
        [TestMethod]
        public void FivePenceCorrect()
        {
            var denomination = CoinDenomination.FivePence;
            Assert.AreEqual(0.05M, denomination.Value);
            Assert.AreEqual("5p", denomination.ToString());
        }

        [TestMethod]
        public void OnePoundCorrect()
        {
            var denomination = CoinDenomination.OnePound;
            Assert.AreEqual(1, denomination.Value);
            Assert.AreEqual("£1", denomination.ToString());
        }
    }
}