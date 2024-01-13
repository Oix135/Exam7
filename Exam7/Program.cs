using System;
using static Exam7.Program;

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

        static void Main(string[] args)
        {
            FoodStok.Foods = GetFood();

            Console.ReadKey();
        }

        private static List<Food> GetFood()
        {
            return new List<Food>
            {
                new Food{ Name = "Мороженое", Articul = 00001, Price = 100}
            };
        }
    }
    abstract class Delivery
    {
        public string Address;
    }

    class HomeDelivery : Delivery
    {
        /* ... */
    }

    class PickPointDelivery : Delivery
    {
        /* ... */
    }

    class ShopDelivery : Delivery
    {
        /* ... */
    }
    class Order<TDelivery> where TDelivery : Delivery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product<int>> FoodList { get; set; }
    }
    public abstract class Product<TArticul>
    {
        public virtual required string Name { get; set; }
        public virtual TArticul Articul { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        
    }
    public class Food:Product<int>
    {
    }
    public class Manufactured : Product<string>
    {

    }
    public static class FoodStok
    {
        public static List<Food> Foods { get; set; } = new List<Food>();
    }
    public interface IProduct<TArticul>
    {
        public TArticul Articul { get; set; }
    }

    class Order<TDelivery,
    TStruct> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public int Number;

        public string Description;

        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }
    }
    public static class StringExtentions
    {
        public static string Genitive(this int count, Currency currency)
        {
            var lastDigit = count % 10;

            switch (lastDigit)
            {
                case 0:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    {
                        switch (currency)
                        {
                            case Currency.RUB:
                                return $"{count} рублей";
                            case Currency.USD:
                                return $"{count} долларов";
                            default:
                                return $"{count} евро";
                        }
                    }
                case 1:
                    {
                        switch (currency)
                        {
                            case Currency.RUB:
                                return $"{count} рубль";
                            case Currency.USD:
                                return $"{count} доллар";
                            default:
                                return $"{count} евро";
                        }
                    }
                case 2:
                case 3:
                case 4:
                    {
                        switch (currency)
                        {
                            case Currency.RUB:
                                return $"{count} рублея";
                            case Currency.USD:
                                return $"{count} доллара";
                            default:
                                return $"{count} евро";
                        }
                    }
                default:
                    return $"{count} евро";
            }
        }

    }
}
