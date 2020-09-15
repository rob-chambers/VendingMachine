using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class CoinBankTests
    {
        [TestMethod]
        public void SimpleCase()
        {
            // Arrange
            var bank = new CoinBank();
            bank.AddCoin(CoinDenomination.TwentyPence);

            // Act
            bank.Withdraw(0.20M);

            // Assert
            Assert.AreEqual(0M, bank.Balance);
        }

        [TestMethod]
        public void ComplexCase()
        {
            // Arrange
            var bank = new CoinBank();
            bank.AddCoin(CoinDenomination.TwentyPence);

            bank.AddCoin(CoinDenomination.FiftyPence);
            bank.AddCoin(CoinDenomination.TwentyPence);
            bank.AddCoin(CoinDenomination.TenPence);

            // Act
            bank.Withdraw(0.80M);

            // Assert
            Assert.AreEqual(0.20M, bank.Balance);
        }
    }
}