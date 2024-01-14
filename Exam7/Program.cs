
using System.Reflection;

namespace Exam7
{

    public enum Currency
    {
        USD,
        EUR,
        RUB
    }
    internal class Program
    {

        static Random random = new Random();

        static bool exitProgram = false;
        static void Main(string[] args)
        {
            GetNomenklature();
            Start();
        }

        private static void GetNomenklature()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames()
              .Single(str => str.EndsWith(".txt"));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (reader.Peek() != -1)
                    {
                        string str = reader.ReadLine().Trim();
                        var mas = str.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                        if (mas.Length != 2) continue;


                        var vendor = random.Next(0, 100).ToString();
                        while(Provider.Nomenclature.Any(a => a.VendorCode == vendor))
                        {
                            vendor = random.Next(0, 100).ToString();
                        }
                        

                        Provider.Nomenclature.Add(new Product { Name = mas[0], VendorCode = vendor, Price = Convert.ToDecimal(mas[1])});
                    }
                }
            }
        }

        private static void Start()
        {

            while (!exitProgram)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Доступные товары у поставщика:\n");

                var maxLenName = Provider.Nomenclature.Max(a => a.Name.Length);

                foreach(var pr in Provider.Nomenclature)
                {
                    string tab = SetOffset(maxLenName, pr.Name);

                    Console.WriteLine($"Наименование: {pr.Name + tab}\tАртикул: {pr.VendorCode}\tЦена: {pr.Price.Genitive(Currency.RUB)}");
                }


                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nЗаполнить склад: {0}", 1);
                _ = int.TryParse(Console.ReadLine(), out int str);
                if(str != 1)
                {
                    continue;
                }
                else
                {
                    var stock = new Stock();
                    for (var i = 0; i < 50; i++)
                    {
                        var index = random.Next(0, Provider.Nomenclature.Count);
                        var prod = Provider.GetProductByIndex(index);
                        if (prod != null)
                        {
                            stock.Products.Add(prod);
                        }
                        
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Поставщик доставил на склад:\n");

                    stock.Products = stock.Products.GroupBy(a => new { a.Name, a.VendorCode, a.Price }).Select(g => new Product
                    {
                        Name = g.Key.Name,
                        VendorCode = g.Key.VendorCode,
                        Price = g.Key.Price,
                        Count = g.Count()
                    }).ToList();

                    foreach (var pr in stock.Products)
                    {
                        string tab = SetOffset(maxLenName, pr.Name);
                        Console.WriteLine($"Наименование: {pr.Name + tab}\tАртикул: {pr.VendorCode}\tКоличество: {pr.Count}");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nСоздать заказ-наряд: {0}", 2);
                    _ = int.TryParse(Console.ReadLine(), out str);
                    if (str != 2)
                    {
                        continue;
                    }
                    else
                    {

                        var order = new Order<HomeDelivery>(StringExtentions.RandomString(6, random));
                        Console.WriteLine($"Заказ-наряд № {order.Number} создан!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("\nДобавить товар: {0}", 3);
                        _ = int.TryParse(Console.ReadLine(), out str);
                        if(str != 3)
                        {
                            continue;
                        }
                        else
                        {
                            bool addProduct = true;

                            while (addProduct)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Введите артикул");
                                var vendor = Console.ReadLine();

                                var product = stock[vendor];
                                if (product != null)
                                {
                                    Product prod = product--;
                                    if (product.Count <= 0)
                                    {
                                        Console.WriteLine("Товара на складе нет!");
                                    }
                                    else
                                    {
                                        Product pr;
                                        if (order.Products.Any(a => a.VendorCode == vendor))
                                        {
                                            pr = order.Products.Where(a => a.VendorCode == vendor).First();
                                            pr++;
                                        }
                                        else
                                        {
                                            order.Products.Add(new Product { Count = 1, Name = product.Name, VendorCode = product.VendorCode, Price = product.Price});
                                        }
                                        Console.WriteLine($"{product.Name} добавлен! Осталось на складе {prod.Count}");
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Еще? (Y)");
                                addProduct = Console.ReadLine() == "y" || Console.ReadLine() == "н";
                            }
                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.WriteLine($"Заказ № {order.Number} сформирован!\n");
                            foreach(var pr in order.Products)
                            {
                                string tab = SetOffset(maxLenName, pr.Name);
                                Console.WriteLine($"Наименование: {pr.Name + tab}\tАртикул: {pr.VendorCode}\tКоличество: {pr.Count}" +
                                    $"\tЦена: {pr.Price.Genitive(Currency.RUB)}\tСумма {(pr.Price * pr.Count).Genitive(Currency.RUB)}");
                               
                            }
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"\nСумма заказа {order.Summ.Genitive(Currency.RUB)}");
                            order.Delivery.GetDeliveryDetails();
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }


                }
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    exitProgram = true;
                }
            }

        }

        private static string SetOffset(int maxLenName, string name)
        {
            var offset = maxLenName - name.Length;
            var tab = "";
            for (var i = 0; i < offset; i++)
            {
                tab += " ";
            }

            return tab;
        }

        internal class Stock
        {

            public List<Product> Products = new List<Product>();
            public Product this[string vendor] => Products.FirstOrDefault(a => a.VendorCode == vendor);
        }
        private static class Provider
        {
            public static List<Product> Nomenclature { get; set; } = [];

            public static Product? GetProductByIndex(int index)
            {
                return index > 0 && index < Nomenclature.Count ? Nomenclature[index] : null;
            }
        }
        internal class Product
        {
            public string Name { get; set; } = string.Empty;
            public string VendorCode { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Count { get; set; }

            public static Product operator ++(Product product)
            {
                return new Product 
                { 
                    Count = product.Count++, 
                    Name = product.Name, 
                    VendorCode = product.VendorCode, 
                    Price = product.Price 
                };
            }
            public static Product operator --(Product product)
            {
                return new Product
                {
                    Count = product.Count > 0 ? product.Count-- : product.Count,
                    Name = product.Name,
                    VendorCode = product.VendorCode,
                    Price = product.Price
                };
            }
        }
        abstract class Delivery
        {
            public string Address;
            public void DisplayAddress()
            {
                Console.WriteLine(Address);
            }
            public abstract void GetDeliveryDetails();
        }
        class HomeDelivery : Delivery
        {
            private string courier = "Иванов Иван";
            public override void GetDeliveryDetails()
            {
                Console.WriteLine($"Товар доставит {courier}");

            }
        }
        class PickPointDelivery : Delivery
        {
            public override void GetDeliveryDetails()
            {
                throw new NotImplementedException();
            }
        }
        class ShopDelivery : Delivery
        {
            public override void GetDeliveryDetails()
            {
                throw new NotImplementedException();
            }
        }
        class Order<TDelivery> where TDelivery : new()
        {
            public Order(string number)
            {
                Number = number;
                Delivery = new TDelivery();
            }
            public string Number { get; }
            public TDelivery Delivery { get; }
            public List<Product> Products { get; } = new List<Product>();

            public decimal Summ => Products.Sum(a => a.Price * a.Count);
        }
        
    }

}
