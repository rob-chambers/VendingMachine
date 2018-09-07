using System;

namespace VendingMachine.Core
{
    public class Location
    {
        public Location(string code)
        {
            Code = code;
        }

        public string Code { get; private set; }

        public Product Product { get; private set; }

        public void Stock(Product product)
        {
            Product = product;
        }

        public void Dispense()
        {
            if (Product == null)
                throw new InvalidOperationException("No product to be dispensed");

            // Record date

            Product = null;
        }
    }
}