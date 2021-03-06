﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using VendingMachine.Core.CreditProviders;

namespace VendingMachine.Core
{
    public class Machine
    {
        private bool _isOutOfStock;

        public Machine() : this(25)
        {
        }

        private Machine(int locationCount)
        {
            CoinCreditProvider = new CoinCreditProvider(CoinBank);
            FillLocations(locationCount);
            DetermineOutOfStockStatus();
        }

        public event EventHandler<ChangeGivenEventArgs> ChangeGiven;

        public event PropertyChangedEventHandler IsOutOfStockChanged;

        public CoinBank CoinBank { get; } = new CoinBank();

        public CoinCreditProvider CoinCreditProvider { get; set; }

        public decimal Credit
        {
            get
            {
                return CoinCreditProvider.Total + NoteCreditProvider.Total;
            }
        }

        public bool IsOutOfStock
        {
            get => _isOutOfStock;
            private set
            {
                if (_isOutOfStock == value)
                {
                    return;
                }

                _isOutOfStock = value;
                RaiseOutOfStockChangeEvent();

                CoinCreditProvider.UpdateOutOfStockStatus(value);
            }
        }

        public Dictionary<string, Location> Locations { get; private set; }

        public NoteCreditProvider NoteCreditProvider { get; set; } = new NoteCreditProvider();

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

            Locations[code].Dispense();

            DispenseChange(price);
            CoinCreditProvider.ReduceCredit(price);

            DetermineOutOfStockStatus();

            return VendResult.Success;
        }

        protected void RaiseChangeGivenEvent(decimal change)
        {
            var handler = ChangeGiven;
            if (handler == null)
            {
                return;
            }

            handler(this, new ChangeGivenEventArgs(change));
        }

        protected void RaiseOutOfStockChangeEvent()
        {
            var handler = IsOutOfStockChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, new PropertyChangedEventArgs(nameof(IsOutOfStock)));
        }

        private void DetermineOutOfStockStatus()
        {
            IsOutOfStock = Locations.All(x => x.Value.OutOfStock);
        }

        private void DispenseChange(decimal price)
        {
            // Note, we don't currently determine whether there is enough change available
            // This is an excercise for extension / enhancement
            if (Credit > price)
            {
                RaiseChangeGivenEvent(Credit - price);
            }
        }

        private void FillLocations(int numberLocations)
        {
            Locations = new Dictionary<string, Location>();
            for (var i = 0; i < numberLocations; i++)
            {
                int row, col;

                row = i / 5;
                col = i % 5 + 1;
                var rowChar = (byte)(65 + row);
                var rowChars = Encoding.ASCII.GetChars(new byte[] { rowChar });
                var code = rowChars[0] + col.ToString();

                var location = new Location(code);
                location.ProductChanged += (sender, e) => { DetermineOutOfStockStatus(); };

                Locations.Add(code, location);
            }
        }

        public void Cancel()
        {
            var credit = Credit;
            if (NoteCreditProvider.Total > 0)
            {
                NoteCreditProvider.ReduceCredit(credit);
            }
            else
            {
                CoinCreditProvider.ReturnCoins(credit);
            }

            RaiseChangeGivenEvent(credit);
        }
    }
}