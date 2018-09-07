using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Core
{
    public class Machine
    {
        public Machine() : this(25)
        {
        }

        private Machine(int numberLocations)
        {
            CoinCreditProvider = new CoinCreditProvider();
            NoteCreditProvider = new NoteCreditProvider();
            FillLocations(numberLocations);
        }

        private void FillLocations(int numberLocations)
        {
            Locations = new Dictionary<string, Location>();
            for (int i = 0; i < numberLocations; i++)
            {
                int row, col;

                row = i / 5;
                col = i % 5 + 1;
                var rowChar = (byte)(65 + row);
                char[] location = Encoding.ASCII.GetChars(new byte[] { rowChar });
                var code = location[0] + col.ToString();

                Locations.Add(code, new Location(code));
            }
        }

        public Dictionary<string, Location> Locations { get; private set; }

        public CoinCreditProvider CoinCreditProvider { get; set; }

        public NoteCreditProvider NoteCreditProvider { get; set; }

        public decimal Credit
        {
            get
            {
                return CoinCreditProvider.Total + NoteCreditProvider.Total;
            }
        }

        public VendResult Vend(string code)
        {
            if (!Locations.ContainsKey(code))
            {
                throw new InvalidLocationException(code);
            }

            if (Locations[code].Product == null)
            {
                return VendResult.ProductNotAvailable;
            }

            var price = Locations[code].Product.Price;
            if (Credit < price)
            {
                return VendResult.InsufficientCredit;
            }

            // TODO: Reduce customer credit
            Locations[code].Dispense();

            return VendResult.Success;            
        }

        public void Stock()
        {
            throw new NotImplementedException();
        }
    }
}