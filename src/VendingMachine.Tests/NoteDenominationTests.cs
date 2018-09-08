using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class NoteDenominationTests
    {
        [TestMethod]
        public void FivePoundCorrect()
        {
            var denomination = NoteDenomination.FivePound;
            Assert.AreEqual(5, denomination.Value);
            Assert.AreEqual("£5", denomination.ToString());
        }

        [TestMethod]
        public void TenPoundCorrect()
        {
            var denomination = NoteDenomination.TenPound;
            Assert.AreEqual(10, denomination.Value);
            Assert.AreEqual("£10", denomination.ToString());
        }
    }
}