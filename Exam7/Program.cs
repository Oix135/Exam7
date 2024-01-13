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
}
