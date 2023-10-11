using System;
using System.Collections.Generic;

namespace ChainOfResponsibility.Composite
{
    public class Manager : Unit
    {
        private const int OrderThreshold = 10_000;
        private List<RegularEmployee> employees = new List<RegularEmployee>();

        public override int Id { get; }
        public override string Name { get; }
        public override uint Size { get; }
        public override void Approve(Order order)
        {
            if (CanProcess(order))
            {
                Console.WriteLine($"Manager {Id} processed an order {order.Id} with amount {order.Price}");
                return;
            }
            Parent.Approve(order);
        }

        protected override bool CanProcess(Order order)
        {
            return order.Price < OrderThreshold;
        }

        public void AddEmployee(RegularEmployee employee)
        {
            employees.Add(employee);
        }

        public Manager(int id, string name, Unit parent) : base(parent)
        {
            Id = id;
            Name = name;
        }
    }
}