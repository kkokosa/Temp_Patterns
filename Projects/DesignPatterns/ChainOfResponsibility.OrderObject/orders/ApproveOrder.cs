namespace ChainOfResponsibility.OrderObject.Orders
{
    public class ApproveOrder : IOrder
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
    }
}