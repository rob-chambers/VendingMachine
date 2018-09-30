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

        /// <summary>
        /// Withdraws a number of coins to the value of <paramref name="value"/> />
        /// </summary>
        /// <param name="value"></param>
        public void Withdraw(decimal value)
        {
            var coin = Coins.SingleOrDefault(c => c.Value == value);
            if (coin != null)
            {
                Coins.Remove(coin);
                return;
            }

            // Prioritise by largest denomination
            var coins = Coins.Where(c => c.Value < value).OrderByDescending(c => c.Size);                
            var combination = GetCombinationForTotal(coins, value);
            if (combination != null)
            {
                foreach (var item in combination)
                {
                    Coins.Remove(item);
                }
            }
        }

        private IEnumerable<CoinDenomination> GetCombinationForTotal(IEnumerable<CoinDenomination> coins, decimal value)
        {
            var stack = new Stack<CoinDenomination>();
            var total = 0M;

            foreach (var item in coins)
            {
                stack.Push(item);
                total += item.Value;

                var remainder = value - total;

                var coin = coins.SingleOrDefault(c => c.Value == remainder);
                if (coin != null)
                {
                    stack.Push(coin);
                    return stack;
                }

                if (total == value)
                {
                    return stack;
                }
                else if (total > value)
                {
                    total = 0;
                    stack.Clear();
                }
            }

            return null;
        }
    }
}