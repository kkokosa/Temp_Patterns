using System;

namespace ChainOfResponsibility.Multihandler
{
    public interface IDiscountRule
    {
        Guid Id { get; }
        
        bool Apply(Order order);
        bool CanApply(Order order);
    }
}