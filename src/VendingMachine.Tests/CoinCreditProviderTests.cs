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
            var provider = new CoinCreditProvider(new CoinBank());
            provider.UpdateOutOfStockStatus(false);

            provider.InsertCoin(CoinDenomination.FiftyPence);
            provider.InsertCoin(CoinDenomination.TenPence);
            provider.InsertCoin(CoinDenomination.FivePence);

            Assert.AreEqual(0.65M, provider.Total);
        }

        [TestMethod]
        public void SupplyingCoinsAddsToCoinBank()
        {
            var bank = new CoinBank();
            Assert.AreEqual(0, bank.Balance);

            var provider = new CoinCreditProvider(bank);
            provider.UpdateOutOfStockStatus(false);
            provider.InsertCoin(CoinDenomination.FiftyPence);
            provider.InsertCoin(CoinDenomination.TenPence);
            provider.InsertCoin(CoinDenomination.FivePence);

            Assert.AreEqual(0.65M, bank.Balance);
        }

        [TestMethod]
        public void SupplyingCoinsWhenMachineOutOfStockDoesNotIncreaseCredit()
        {
            // Arrange
            var bank = new CoinBank();
            var provider = new CoinCreditProvider(bank);
            provider.UpdateOutOfStockStatus(true);

            // Act
            var result = provider.InsertCoin(CoinDenomination.FiftyPence);

            // Assert
            Assert.AreEqual(0, bank.Balance);
            Assert.IsFalse(result);
        }
    }
}
