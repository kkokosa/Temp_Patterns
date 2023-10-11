using System;
using System.Collections.Generic;
using System.Linq;
using ChainOfResponsibility.OrderObject.Orders;

namespace ChainOfResponsibility.OrderObject
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

        public override void Process(IOrder order)
            => ProcessOrder(order as dynamic);

        private void ProcessOrder(ApproveOrder approveOrder)
        {
            approveOrder.Comment = $"Approved by {Id} department";
            Console.WriteLine($"Department {Id} processed an order {approveOrder.Id} with amount {approveOrder.Price}");
        }

        private void ProcessOrder(CancelOrder cancelOrder)
        {
            cancelOrder.Reason = CancelOrder.ReasonType.NotNeeded;
            Console.WriteLine($"Department {Id} processed an order {cancelOrder.Id} with amount {cancelOrder.Price}");
        }

        protected override bool CanProcess(IOrder order)
        {
            return true;
        }

        public IReadOnlyCollection<Unit> Subunits => subunits;

        public Department(int id, string name, Unit parent) : base(parent)
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
