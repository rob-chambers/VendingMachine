using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Core
{
    public class CoinBank
    {
        List<CoinDenomination> Coins { get; } = new List<CoinDenomination>();

        public decimal Balance
        {
            get
            {
                decimal total = Coins.Sum(x => x.Value); ;
                return total;
            }
        }        

        public void AddCoin(CoinDenomination denomination)
        {
            Coins.Add(denomination);
        }        
    }
}
