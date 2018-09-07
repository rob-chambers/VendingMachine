namespace VendingMachine.Core
{
    public class NoteCreditProvider : CreditProvider
    {
        private decimal _total;

        public override decimal Total
        {
            get => _total;
        }

        public bool InsertNote(NoteDenomination denomination)
        {
            // Assume we can only accept one note
            if (Total > 0)
            {
                return false;
            }

            _total += denomination.Value;
            return true;
        }
    }
}
