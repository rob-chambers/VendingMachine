namespace VendingMachine.Core
{
    public class Product
    {
        public string Name { get; private set; }
        public decimal Price { get; internal set; }

        public Product(string name)
        {
            Name = name;

            // All products 50p
            Price = 0.50M;
        }
    }
}