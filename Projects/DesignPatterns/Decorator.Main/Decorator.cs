namespace Decorator.PreMain
{
    public interface IDiscount
    {
        public decimal CalculateDiscount(decimal price);
    }

    public class NoDiscount : IDiscount
    {
        public decimal CalculateDiscount(decimal price) => price;
    }

    public class DiscountDecorator : IDiscount
    {
        private readonly IDiscount _decorated;

        public DiscountDecorator(IDiscount decorated)
        {
            _decorated = decorated;
        }

        public virtual decimal CalculateDiscount(decimal price)
        {
            return _decorated.CalculateDiscount(price);
        }
    }

    public class FlatDiscount : DiscountDecorator
    {
        private readonly decimal _amount;

        public FlatDiscount(IDiscount decorated, decimal amount) : base(decorated)
        {
            _amount = amount;
        }

        public override decimal CalculateDiscount(decimal price)
        {
            var basePrice = base.CalculateDiscount(price);
            return basePrice - _amount;
        }
    }

    public class PercentageDiscount : DiscountDecorator
    {
        private readonly decimal _percentage;

        public PercentageDiscount(IDiscount decorated, decimal percentage) : base(decorated)
        {
            _percentage = percentage;
        }

        public override decimal CalculateDiscount(decimal price)
        {
            var basePrice = base.CalculateDiscount(price);
            return basePrice * (1 - _percentage / 100);
        }
    }
}
