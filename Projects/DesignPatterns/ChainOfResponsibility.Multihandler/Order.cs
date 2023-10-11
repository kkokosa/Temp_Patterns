using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainOfResponsibility.Multihandler
{
    public class Order
    {
        private decimal initialPrice;
        private decimal currentPrice;
        private List<OrderItem> orderItems;
        private List<Guid> appliedDiscountIds;

        public decimal CurrentPrice
        {
            get => currentPrice;
            set => currentPrice = value;
        }

        public int TotalItems => orderItems.Sum(x => x.Count);

        public Order()
        {
            this.initialPrice = 0.0m;
            this.currentPrice = 0.0m;
            this.orderItems = new List<OrderItem>();
            this.appliedDiscountIds = new List<Guid>();
        }

        public void AddOrderItem(string name, decimal unitPrice, int count)
        {
            orderItems.Add(new OrderItem()
            {
                Count =  count,
                Name = name,
                UnitPrice = unitPrice
            });
            initialPrice += unitPrice * count;
            currentPrice = initialPrice;
        }

        public void TallyDiscount(Guid appliedDiscountId)
        {
            appliedDiscountIds.Add(appliedDiscountId);
        }
    }
}