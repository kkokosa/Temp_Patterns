using System;

namespace ChainOfResponsibility.Composite
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
        public override void Approve(Order order)
        {
            if (CanProcess(order))
            {
                Console.WriteLine($"Employee {id} processed an order {order.Id} with amount {order.Price}");
                return;
            }
            Parent.Approve(order);
        }

        protected override bool CanProcess(Order order)
        {
            return order.Price < OrderingThreshold;
        }

        internal RegularEmployee(int id, string name, Unit parent) : base(parent)
        {
            this.id = id;
            this.name = name;
        }
    }

}
