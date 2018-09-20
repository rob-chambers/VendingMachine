using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Core
{
    public class CoinBank
    {
        private List<CoinDenomination> Coins { get; } = new List<CoinDenomination>();

        public decimal Balance
        {
            get
            {
                var total = Coins.Sum(x => x.Value);
                return total;
            }
        }

        public void AddCoin(CoinDenomination denomination)
        {
            if (denomination == null)
            {
                throw new ArgumentNullException(nameof(denomination));
            }

            Coins.Add(denomination);
        }
    }
}