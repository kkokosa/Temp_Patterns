using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainOfResponsibility.Composite
{
    public class Department : Unit
    {
        private const int OrderThreshold = 100_000;
        private readonly int id;
        private readonly string name;
        private readonly List<Unit> subunits = new List<Unit>();

        public override string Name => name;
        public override int Id => id;

        public override uint Size
        {
            get
            {
                return 1 + (uint)subunits.Sum(x => x.Size);
            }
        }

        public override void Approve(Order order)
        {
            if (CanProcess(order))
            {
                Console.WriteLine($"Department {id} processed an order {order.Id} with amount {order.Price}");
                return;
            }
            Parent.Approve(order);
        }

        protected override bool CanProcess(Order order)
        {
            return order.Price < OrderThreshold;
        }

        public IReadOnlyCollection<Unit> Subunits => subunits;

        internal Department(int id, string name, Unit parent) : base(parent)
        {
            this.id = id;
            this.name = name;
        }

        public void AddSubunit(Unit unit)
        {
            subunits.Add(unit);
        }
    }

}
