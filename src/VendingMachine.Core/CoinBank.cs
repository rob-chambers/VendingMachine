using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Core
{
    public class CoinBank
    {
        public decimal Balance
        {
            get
            {
                decimal total = 0;
                //total += FivePenceCoins.Sum(x => x.Value);
                //total += TenPenceCoins.Sum(x => x.Value);
                //total += TwentyPenceCoins.Sum(x => x.Value);
                //total += FiftyPenceCoins.Sum(x => x.Value);
                //total += OnePoundCoins.Sum(x => x.Value);

                total += Coins.Sum(x => x.Value);

                return total;
            }
        }

        public void AddCoin(CoinDenomination denomination)
        {
            Coins.Add(denomination);
        }

        List<CoinDenomination> Coins { get; } = new List<CoinDenomination>();
        //public List<CoinDenomination> FivePenceCoins { get; set; } = new List<CoinDenomination>();
        //public List<CoinDenomination> TenPenceCoins { get; set; } = new List<CoinDenomination>();
        //public List<CoinDenomination> TwentyPenceCoins { get; set; } = new List<CoinDenomination>();
        //public List<CoinDenomination> FiftyPenceCoins { get; set; } = new List<CoinDenomination>();
        //public List<CoinDenomination> OnePoundCoins { get; set; } = new List<CoinDenomination>();

        //public void StockTenPenceCoins(int number)
        //{
        //    var coin = CoinDenomination.TenPence;
        //    for (int i = 0; i < number; i++)
        //    {
        //        TenPenceCoins.Add(coin);
        //    }
        //}
    }
}
