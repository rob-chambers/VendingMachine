using System;
using System.ComponentModel;

namespace VendingMachine.Core
{
    public class Location
    {
        private Product _product;

        public Location(string code)
        {
            Code = code;
        }

        public event PropertyChangedEventHandler ProductChanged;

        public string Code { get; private set; }

        public Product Product
        {
            get => _product;
            private set
            {
                if (_product == value) return;
                _product = value;
                RaiseStockChange();
            }
        }

        private void RaiseStockChange()
        {
            var handler = ProductChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(nameof(Product)));
        }

        public bool OutOfStock
        {
            get
            {
                return Product == null;
            }
        }

        public void Stock(Product product)
        {
            Product = product;
        }

        public void Dispense()
        {
            if (OutOfStock)
                throw new InvalidOperationException("No product to be dispensed");

            // Record date

            Product = null;
        }
    }
}