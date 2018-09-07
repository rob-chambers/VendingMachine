using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Core;

namespace VendingMachine.Tests
{
    [TestClass]
    public class MachineTests
    {
        Product _coke;
        Machine _machine;

        private void Init()
        {
            _coke = new Product("Coke");
            var builder = new MachineBuilder();
            _machine = builder.Build();
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
    }
}
