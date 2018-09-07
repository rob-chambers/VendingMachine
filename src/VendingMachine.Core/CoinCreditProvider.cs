namespace VendingMachine.Core
{
    public class CoinCreditProvider : CreditProvider
    {
        private decimal _total;

        public override decimal Total
        {
            get => _total;
        }

        public void InsertCoin(CoinDenomination denomination)
        {
            _total += denomination.Value;
        }
    }
}
