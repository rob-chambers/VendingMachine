namespace VendingMachine.Core
{
    public class CoinDenomination
    {
        public static CoinDenomination FivePence = new CoinDenomination(5);
        public static CoinDenomination TenPence = new CoinDenomination(10);
        public static CoinDenomination TwentyPence = new CoinDenomination(20);
        public static CoinDenomination FiftyPence = new CoinDenomination(50);
        public static CoinDenomination OnePound = new CoinDenomination(100);

        private CoinDenomination(int size)
        {
            Size = size;
            Value = System.Convert.ToDecimal(size) / 100;
        }

        public int Size { get; private set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return Size < 100 
                ? Size + "p" 
                : "£1";
        }
    }
}