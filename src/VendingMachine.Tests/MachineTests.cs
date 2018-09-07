using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class MachineTests
    {        
        Machine _machine;
        private Product _coke;

        private void Init()
        {
            _coke = Product.CokeCan;
            var builder = new MachineBuilder();
            _machine = builder.Build();
        }

        private void InitWithSufficientCredit()
        {
            Init();

            _machine.Locations["A1"].Stock(_coke);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.OnePound);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FiftyPence);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.FiftyPence);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.TwentyPence);
            _machine.CoinCreditProvider.InsertCoin(CoinDenomination.TwentyPence);
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
            
            _machine.Locations["A1"].Stock(_coke);
            var result = _machine.Vend(_machine.Locations["A1"].Code);
            Assert.AreEqual(VendResult.InsufficientCredit, result);
        }

        [TestMethod]
        public void CanVendWhenStockedAndSufficientCredit()
        {
            Init();

            _machine.Locations["A1"].Stock(_coke);

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

            _machine.Locations["A1"].Stock(_coke);
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
            InitWithSufficientCredit();
            var result = _machine.Vend(_machine.Locations["A1"].Code);

            Assert.AreEqual(VendResult.Success, result);

            result = _machine.Vend(_machine.Locations["A2"].Code);

            Assert.AreEqual(VendResult.InsufficientCredit, result);
        }
    }
}
