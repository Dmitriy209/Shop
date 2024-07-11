using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Shop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Shop shop = new Shop();
            shop.Work();
        }
    }

    class Shop
    {
        private Seller _seller = new Seller();
        private Buyer _buyer = new Buyer();

        public void Work()
        {
            const string CommandSellerShowGoods = "1";
            const string CommandBuyerShowGoods = "2";
            const string CommandSellerSale = "3";
            const string CommandSellerShowBalance = "4";
            const string CommandBuyerShowBalance = "5";
            const string CommandExit = "exit";

            bool isRunning = true;

            Console.WriteLine("Вы зашли в магазин.");

            while (isRunning)
            {
                Console.WriteLine($"Введите {CommandSellerShowGoods}, чтобы продавец показал товары.\n" +
                    $"Введите {CommandBuyerShowGoods}, чтобы покупатель показал товары.\n" +
                    $"Введите {CommandSellerSale}, чтобы торговать.\n" +
                    $"Введите {CommandSellerShowBalance}, чтобы продавец показал баланс.\n" +
                    $"Введите {CommandBuyerShowBalance}, чтобы покупатель показал баланс.\n" +
                    $"Введите {CommandExit}, чтобы выйти.");
                string userInput = Console.ReadLine();

                Console.Clear();

                switch (userInput)
                {
                    case CommandSellerShowGoods:
                        _seller.ShowGoods();
                        break;

                    case CommandBuyerShowGoods:
                        _buyer.ShowGoods();
                        break;

                    case CommandSellerSale:
                        _seller.Sale(_buyer);
                        break;

                    case CommandSellerShowBalance:
                        break;

                    case CommandBuyerShowBalance:
                        break;

                    case CommandExit:
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Такой команды нет.");
                        break;
                }
            }

            Console.WriteLine("Вы вышли из магазина.");
        }
    }

    class Trader
    {
        protected List<Product> Products;
        protected int Money;

        public Trader()
        {

        }

        public void ShowGoods()
        {
            if (Products.Count != 0)
                foreach (var good in Products)
                    good.ShowStats();
            else
                Console.WriteLine("Товаров нет.");
        }

        public int GenerateRandomMoney()
        {
            Random random = new Random();

            int minLimitRandom = 1000;
            int maxLimitRandom = 5000;

            int money = random.Next(minLimitRandom, maxLimitRandom);

            return money;
        }
    }

    class Seller : Trader
    {
        public Seller()
        {
            Products = CreateProducts();
            Money = 0;
        }

        public void Sale(Buyer buyer)
        {
            ShowGoods();

            Console.WriteLine("\nВведите название товара:");

            if (HasProduct(Console.ReadLine(), out Product product) && buyer.TryPayForOne(product))
            {
                if (TryReadAmount(product, out int amount))
                {
                    if (buyer.TryPay(product, amount, out int priceForAmount))
                    {
                        buyer.Buy(product, amount, priceForAmount);
                    }
                }
            }

        }

        private bool TryReadAmount(Product product, out int amount)
        {
            Console.WriteLine("Введите количество:");
            amount = ReadInt();

            if (amount > 0 && amount <= product.Amount)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Товара в таком количестве нет.");
                return false;
            }
        }

        private int ReadInt()
        {
            int number;

            while (int.TryParse(Console.ReadLine(), out number) == false)
                Console.WriteLine("Это не число.");

            return number;
        }

        private bool HasProduct(string line, out Product product)
        {
            product = null;

            foreach (var goods in Products)
            {
                if (line == goods.Name)
                {
                    product = goods;
                    return true;
                }
            }

            Console.WriteLine("Такого товара не найдено.");
            return false;
        }

        private List<Product> CreateProducts()
        {
            Random random = new Random();

            int minLimitRandomAmount = 1;
            int maxLimitRandomAmount = 20;

            int minLimitRandomPrice = 10;
            int maxLimitRandomPrice = 101;

            List<Product> goods = new List<Product>();

            List<string> names = new List<string>() { "молоко", "банан", "хлеб", "колбаса","сыр","мыло","йогурт","кефир","кофе","мука","яйцо",
                "соль","сахар", "перец","яблочный сок","мясо"};

            foreach (string name in names)
                goods.Add(new Product(name, random.Next(minLimitRandomPrice, maxLimitRandomPrice), random.Next(minLimitRandomAmount, maxLimitRandomAmount)));

            return goods;
        }
    }

    class Buyer : Trader
    {
        public Buyer()
        {
            List<Product> Goods = new List<Product>();
            Money = GenerateRandomMoney();
        }

        public bool TryPayForOne(Product product)
        {
            if (Money >= product.Price)
            {
                return true;
            }
            else
            {
                Console.WriteLine("У вас не хватает денег даже на единицу товара.");
                return false;
            }
        }

        public bool TryPay(Product product, int amount, out int priceForAmount)
        {
            priceForAmount = product.Price * amount;

            if (Money >= priceForAmount)
            {
                return true;
            }
            else
            {
                Console.WriteLine("У вас не хватает денег.");
                return false;
            }
        }

        public int Buy(Product product, int amount, int priceForAmount)
        {
            Products.Add(product);
            Money -= priceForAmount;
            return priceForAmount;
        }
    }

    class Product
    {
        public Product(string name, int price, int amount)
        {
            Name = name;
            Price = price;
            Amount = amount;
        }

        public string Name { get; private set; }
        public int Amount { get; private set; }
        public int Price { get; private set; }

        public void ShowStats()
        {
            Console.WriteLine($"{Name} в количестве: {Amount}, по цене за штуку: {Price}");
        }
    }
}
