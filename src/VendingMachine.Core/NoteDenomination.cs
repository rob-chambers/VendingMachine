namespace VendingMachine.Core
{
    public class NoteDenomination
    {
        public static NoteDenomination FivePound = new NoteDenomination(5);
        public static NoteDenomination TenPound = new NoteDenomination(10);

        private NoteDenomination(int size)
        {
            Size = size;
            Value = size;
        }

        public int Size { get; private set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return "£" + Size;
        }
    }
}