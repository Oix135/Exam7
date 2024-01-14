namespace Exam7
{
    private class OrderBase<TDelivery> where TDelivery : new()
    {
        public TDelivery Delivery { get; }
        public string Number { get; }
        public List<Product> Products { get; } = new List<Product>();

        public decimal Summ => Products.Sum(a => a.Price * a.Count);
    }
}