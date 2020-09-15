using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class CancelVendTests
    {
        private readonly Product _cokeCan = Product.CokeCan;
        private readonly Product _cokeBottle = Product.CokeBottle;
        private Machine _machine;

        private void Init()
        {
            var builder = new MachineBuilder();
            _machine = builder.Build();
            _machine.Locations["A1"].Stock(_cokeCan);
        }

        [TestMethod]
        public void MachineReturnsChangeWhenCancelled()
        {
            // Arrange
            Init();

            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FiftyPence);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.TenPence);
            var change = 0M;
            _machine.ChangeGiven += (sender, e) => { change = e.Change; };

            // Act
            _machine.Cancel();

            // Assert
            Assert.AreEqual(0.60M, change);
            Assert.AreEqual(0, _machine.Credit);
            Assert.AreEqual(0, _machine.CoinCreditProvider.Total);
            Assert.AreEqual(0, _machine.CoinBank.Balance);
        }

        [TestMethod]
        public void MachineReturnsOnlyCustomerCreditWhenCancelled()
        {
            // Arrange
            Init();

            _machine.CoinBank.AddCoin(CoinDenomination.FiftyPence);

            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.TenPence);
            var change = 0M;
            _machine.ChangeGiven += (sender, e) => { change = e.Change; };

            // Act
            _machine.Cancel();

            // Assert
            Assert.AreEqual(0.10M, change);
            Assert.AreEqual(0, _machine.Credit);
            Assert.AreEqual(0, _machine.CoinCreditProvider.Total);
            Assert.AreEqual(0.50M, _machine.CoinBank.Balance);
        }
    }
}