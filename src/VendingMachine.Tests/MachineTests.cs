using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class MachineTests
    {                
        private readonly Product _cokeCan = Product.CokeCan;
        private readonly Product _cokeBottle = Product.CokeBottle;
        private Machine _machine;

        private void Init()
        {
            var builder = new MachineBuilder();
            _machine = builder.Build();
        }

        private void InitWithSufficientCredit()
        {
            Init();

            _machine.Locations["A1"].Stock(_cokeCan);
            _machine.Locations["A2"].Stock(_cokeBottle);

            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.OnePound);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.OnePound);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.OnePound);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FiftyPence);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FiftyPence);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.TwentyPence);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.TwentyPence);
        }

        private void InitWithCokeCanCredit()
        {
            Init();
            _machine.Locations["A1"].Stock(_cokeCan);            

            do
            {
                _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FivePence);
            }
            while (_machine.CoinCreditProvider.Total < _cokeCan.Price);            
        }

        [TestMethod]
        public void CannotVendWhenNoStock()
        {
            Init();
            foreach (var location in _machine.Locations.Values)
            {
                Assert.AreEqual(null, location.Product);
            }
        }

        [TestMethod]
        public void CannotVendWhenProductNotAvailable()
        {
            Init();
            var result = _machine.Vend(_machine.Locations["A1"].Code);
            Assert.AreEqual(VendResult.ProductNotAvailable, result);
        }

        [TestMethod]
        public void CannotVendWhenStockedButNoCredit()
        {
            Init();
            
            _machine.Locations["A1"].Stock(_cokeCan);
            var result = _machine.Vend(_machine.Locations["A1"].Code);
            Assert.AreEqual(VendResult.InsufficientCredit, result);
        }

        [TestMethod]
        public void CanVendWhenStockedAndSufficientCredit()
        {
            Init();

            _machine.Locations["A1"].Stock(_cokeCan);

            for (int i = 0; i < 10; i++)
            {
                _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FiftyPence);
            }
            
            var result = _machine.Vend(_machine.Locations["A1"].Code);
            Assert.AreEqual(VendResult.Success, result);
        }

        [TestMethod]
        public void BadVendingLocationThrowsException()
        {
            Init();

            var pass = false;
            string exception = string.Empty;

            try
            {
                _machine.Vend("bad");
            }
            catch (InvalidLocationException ex)
            {
                exception = ex.Message;
                pass = true;
            }

            Assert.AreEqual("bad does not exist", exception);
            Assert.IsTrue(pass);
        }

        [TestMethod]
        public void CannotVendWhenStockedButInsufficientCredit()
        {
            Init();

            _machine.Locations["A1"].Stock(_cokeCan);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FivePence);
            var result = _machine.Vend(_machine.Locations["A1"].Code);
            Assert.AreEqual(VendResult.InsufficientCredit, result);
        }

        [TestMethod]
        public void VendingRemovesProduct()
        {
            InitWithSufficientCredit();
            var result = _machine.Vend(_machine.Locations["A1"].Code);

            Assert.AreEqual(VendResult.Success, result);

            result = _machine.Vend(_machine.Locations["A1"].Code);

            Assert.AreEqual(VendResult.ProductNotAvailable, result);
        }

        [TestMethod]
        public void VendingDrainsCredit()
        {
            InitWithCokeCanCredit();
            _machine.Locations["A2"].Stock(_cokeBottle);

            var result = _machine.Vend(_machine.Locations["A1"].Code);

            Assert.AreEqual(VendResult.Success, result);

            result = _machine.Vend(_machine.Locations["A2"].Code);

            Assert.AreEqual(VendResult.InsufficientCredit, result);
        }

        [TestMethod]
        public void MachineInitiallyOutOfStock()
        {
            Init();
            Assert.IsTrue(_machine.IsOutOfStock);
        }

        [TestMethod]
        public void MachineNotOutOfStockAfterStocking()
        {
            // Arrange
            Init();            
            var outOfStock = _machine.IsOutOfStock;
            _machine.IsOutOfStockChanged += (sender, args) => outOfStock = (sender as Machine).IsOutOfStock;

            // Act
            _machine.Locations["A1"].Stock(_cokeCan);

            // Assert
            Assert.IsFalse(_machine.IsOutOfStock);
            Assert.IsFalse(outOfStock);
        }

        [TestMethod]
        public void MachineOutOfStockAfterVendingLastProduct()
        {
            // Arrange
            InitWithCokeCanCredit();
            var outOfStock = _machine.IsOutOfStock;
            _machine.IsOutOfStockChanged += (sender, args) => outOfStock = (sender as Machine).IsOutOfStock;

            // Act
            _machine.Vend(_machine.Locations["A1"].Code);            
            
            // Assert
            Assert.IsTrue(_machine.IsOutOfStock);
            Assert.IsTrue(outOfStock);
        }

        [TestMethod]
        public void MachineDoesNotAcceptCoinsWhenOutOfStock()
        {
            Init();
            var result = _machine.CoinCreditProvider.InsertCoin(CoinDenomination.OnePound);

            Assert.IsFalse(result);
            Assert.AreEqual(0, _machine.CoinCreditProvider.Total);
        }

        [TestMethod]
        public void MachineDispensesChange()
        {
            // Arrange
            Init();
            _machine.Locations["A1"].Stock(_cokeCan);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.OnePound);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FiftyPence);

            var change = 0M;
            _machine.ChangeGiven += (sender, e) => { change = e.Change; };

            // Act
            var result = _machine.Vend(_machine.Locations["A1"].Code);

            // Assert
            Assert.AreEqual(VendResult.Success, result);

            var expected = 1.50M - _cokeCan.Price;
            Assert.AreEqual(expected, change);
        }

        [TestMethod]
        public void MachineDispensesNoChangeWhenCreditMatchesPrice()
        {
            // Arrange
            Init();
            _machine.Locations["A1"].Stock(_cokeCan);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.OnePound);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.TwentyPence);

            var fired = false;
            _machine.ChangeGiven += (sender, e) => { fired = true; };

            // Act
            var result = _machine.Vend(_machine.Locations["A1"].Code);

            // Assert
            Assert.AreEqual(VendResult.Success, result);
            Assert.IsFalse(fired);
        }
    }
}
