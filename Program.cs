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
                        _seller.ShowBalance();
                        break;

                    case CommandBuyerShowBalance:
                        _buyer.ShowBalance();
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
            int minLimitRandom = 1000;
            int maxLimitRandom = 5000;
            
            int money = UserUtils.GenerateRandomNumber(minLimitRandom, maxLimitRandom);

            return money;
        }

        public void ShowBalance()
        {
            Console.WriteLine($"Ваш баланс: {Money}");
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

            if (TryGetProduct(Console.ReadLine(), out Product product) && buyer.TryPay(product))
            {
                Money += buyer.Buy(product);
                Products.Remove(product);
            }
        }

        private bool TryGetProduct(string line, out Product product)
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
            int minLimitRandomPrice = 10;
            int maxLimitRandomPrice = 101;

            List<Product> goods = new List<Product>();

            List<string> names = new List<string>() { "молоко", "банан", "хлеб", "колбаса","сыр","мыло","йогурт","кефир","кофе","мука","яйцо",
                "соль","сахар", "перец","яблочный сок","мясо"};

            foreach (string name in names)
            {
                goods.Add(new Product(name, UserUtils.GenerateRandomNumber(minLimitRandomPrice, maxLimitRandomPrice)));
            }

            return goods;
        }
    }

    class Buyer : Trader
    {
        public Buyer()
        {
            Products = new List<Product>();
            Money = GenerateRandomMoney();
        }

        public bool TryPay(Product product)
        {
            if (Money >= product.Price)
            {
                return true;
            }
            else
            {
                Console.WriteLine("У вас не хватает денег.");
                return false;
            }
        }

        public int Buy(Product product)
        {
            Products.Add(product);
            Money -= product.Price;
            return product.Price;
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }

        public void ShowStats()
        {
            Console.WriteLine($"{Name} по цене {Price}");
        }
    }

    class UserUtils
    {
        public static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return s_random.Next(min, max);
        }
    }
}
