namespace VendingMachine.Core
{
    public class NoteCreditProvider : CreditProvider
    {
        private int[] _denominations = { 5, 10 };
        private decimal _total;

        public override decimal Total
        {
            get => _total;
        }

        public void InsertNote(NoteDenomination denomination)
        {
            _total += denomination.Value;
        }
    }
}
