namespace Decorator.PreMain.Tests
{
    public class DecoratorTests
    {
        [Fact]
        public void WhenNoDiscount()
        {
            IDiscount discount = new NoDiscount();

            var price = discount.CalculateDiscount(100);

            Assert.Equal(100, price);
        }

        [Fact]
        public void WhenFlatDiscount()
        {
            IDiscount discount = new NoDiscount();
            discount = new FlatDiscount(discount, 20);

            var price = discount.CalculateDiscount(100);

            Assert.Equal(80, price);
        }

        [Fact]
        public void WhenPercentageDiscount()
        {
            IDiscount discount = new NoDiscount();
            discount = new PercentageDiscount(discount, 50);

            var price = discount.CalculateDiscount(100);

            Assert.Equal(50, price);
        }

        [Fact]
        public void WhenFlatFollowedByPercentageDiscount()
        {
            IDiscount discount = new NoDiscount();
            discount = new PercentageDiscount(discount, 50);
            discount = new FlatDiscount(discount, 20);

            var price = discount.CalculateDiscount(100);

            Assert.Equal(30, price);
        }

        [Fact]
        public void WhenPercentageFollowedByFlatDiscount()
        {
            IDiscount discount = new NoDiscount();
            discount = new FlatDiscount(discount, 20);
            discount = new PercentageDiscount(discount, 50);

            var price = discount.CalculateDiscount(100);

            Assert.Equal(40, price);
        }

    }
}