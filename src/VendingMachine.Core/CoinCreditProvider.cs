using System;

namespace VendingMachine.Core
{
    public class CoinCreditProvider : CreditProvider
    {
        private readonly CoinBank _coinBank;
        private decimal _total;
        private bool _isOutOfStock = true;

        public CoinCreditProvider(CoinBank coinBank)
        {
            _coinBank = coinBank;
        }

        public override decimal Total
        {
            get => _total;
        }

        public bool InsertCoin(CoinDenomination denomination)
        {
            if (_isOutOfStock) return false;

            _total += denomination.Value;
            _coinBank.AddCoin(denomination);
            return true;
        }

        public void ReduceCredit(decimal price)
        {
            _total -= price;
        }

        public void UpdateOutOfStockStatus(bool isOutOfStock)
        {
            _isOutOfStock = isOutOfStock;
        }
    }
}
