namespace Strategy.Main
{
    using ConcreteContext = Context<ConcreteStrategy>;

    public class Customer
    {
        public delegate double RevenueStrategy(double price, int quantity);

        RevenueStrategy strategy;

        public Customer(RevenueStrategy strategy) => this.strategy = strategy;

        public double Use(double price, int quantity)
        {
            strategy = (price, quantity) => price * quantity;
            return 0.0;
        }
    }

    // Generics and static virtual members
    public interface IStrategy
    {
        static abstract void Execute();
    }

    public class ConcreteStrategy : IStrategy
    {
        public static void Execute()
        {
        }
    }

    public class Context<TStrategy> where TStrategy : IStrategy
    {
        public void ExecuteStrategy()
        {
            TStrategy.Execute();
        }
    }
}
