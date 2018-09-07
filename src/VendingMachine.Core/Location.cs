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
    }
}