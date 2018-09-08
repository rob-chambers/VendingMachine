namespace VendingMachine.Core
{
    public class Product
    {
        public static Product CokeCan = new Product("CokeCan", 1.20M);
        public static Product CokeBottle = new Product("CokeBottle", 2M);
        public static Product Crisps = new Product("Crisps", 1.40M);

        public string Name { get; private set; }
        public decimal Price { get; private set; }

        private Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}