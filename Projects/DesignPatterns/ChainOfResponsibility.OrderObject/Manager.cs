using System;
using System.Collections.Generic;
using ChainOfResponsibility.OrderObject.Orders;

namespace ChainOfResponsibility.OrderObject
{
    public class Manager : Unit
    {
        private const int OrderThreshold = 10_000;
        private List<RegularEmployee> employees = new List<RegularEmployee>();

        public override int Id { get; }
        public override string Name { get; }
        public override uint Size { get; }

        /*
         * Separate CanProcess and Process - maybe good, maybe not
         */
        public override void Process(IOrder order)
        {
            if (CanProcess(order))
            {
                ProcessInternal(order);
                return;
            }
            Parent.Process(order);
        }

        private void ProcessInternal(IOrder order)
        {
            switch (order)
            {
                case ApproveOrder approveOrder: ProcessApproveOrder(approveOrder);
                    break;
                case CancelOrder cancelOrder: ProcessCancelOrder(cancelOrder);
                    break;
                default:
                    Parent.Process(order);
                    break;
            }
        }

        private void ProcessApproveOrder(ApproveOrder approveOrder)
        {
            approveOrder.Comment = $"Approved by {Id}";
            Console.WriteLine($"Manager {Id} processed an order {approveOrder.Id} with amount {approveOrder.Price}");
        }

        private void ProcessCancelOrder(CancelOrder cancelOrder)
        {
            cancelOrder.Reason = CancelOrder.ReasonType.NotNeeded;
            Console.WriteLine($"Manager {Id} processed an order {cancelOrder.Id} with amount {cancelOrder.Price}");
        }

        protected override bool CanProcess(IOrder order)
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