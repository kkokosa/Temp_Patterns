using System;
using ChainOfResponsibility.OrderObject.Orders;

namespace ChainOfResponsibility.OrderObject
{
    public class RegularEmployee : Unit
    {
        private const int OrderingThreshold = 1_000;
        private readonly int id;
        private readonly string name;
        private bool isLogged;

        public override string Name => name;
        public override int Id => id;
        public bool IsLogged
        {
            get => isLogged;
            set => isLogged = value;
        }

        public override uint Size => 1;
        public override void Process(IOrder order)
        {
            if (CanProcess(order))
            {
                Console.WriteLine($"Employee {id} processed an order {order.Id} with amount {order.Price}");
                return;
            }
            Parent.Process(order);
        }

        protected override bool CanProcess(IOrder order)
        {
            return order switch
            {
                ApproveOrder approveOrder => ProcessApproveOrder(approveOrder),
                _ => false
            };
        }

        private bool ProcessApproveOrder(ApproveOrder approveOrder)
        {
            // approveOrder.Comment = ...
            return approveOrder.Price < OrderingThreshold;
        }

        internal RegularEmployee(int id, string name, Unit parent) : base(parent)
        {
            this.id = id;
            this.name = name;
        }
    }

}
