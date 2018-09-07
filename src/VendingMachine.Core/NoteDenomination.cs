namespace VendingMachine.Core
{
    public class NoteDenomination
    {
        public NoteDenomination(int size)
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