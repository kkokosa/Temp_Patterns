namespace ChainOfResponsibility.OrderObject.Orders
{
    public class CancelOrder : IOrder
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public ReasonType Reason { get; set; }

        public enum ReasonType
        {
            Boring,
            NotNeeded,
            TooExpensive
        }
    }
}