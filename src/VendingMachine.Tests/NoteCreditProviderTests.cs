using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;
using VendingMachine.Core.CreditProviders;

namespace VendingMachine.Tests
{
    [TestClass]
    public class NoteCreditProviderTests
    {
        [TestMethod]
        public void CanInsertFivePounds()
        {
            var provider = new NoteCreditProvider();
            provider.InsertNote(NoteDenomination.FivePound);
            Assert.AreEqual(5, provider.Total);
        }

        [TestMethod]
        public void CanInsertTenPounds()
        {
            var provider = new NoteCreditProvider();
            provider.InsertNote(NoteDenomination.TenPound);
            Assert.AreEqual(10, provider.Total);
        }

        [TestMethod]
        public void CanNotInsertMultipleNotes()
        {
            var provider = new NoteCreditProvider();
            provider.InsertNote(NoteDenomination.FivePound);
            Assert.IsFalse(provider.InsertNote(NoteDenomination.FivePound));
            Assert.AreEqual(5, provider.Total);
        }
    }
}