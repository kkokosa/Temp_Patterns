namespace ChainOfResponsibility.OrderObject
{
    public abstract class Unit
    {
        protected  Unit Parent { get; private set; }

        public Unit(Unit parent)
        {
            Parent = parent;
        }
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract uint Size { get; }
        protected abstract bool CanProcess(IOrder order);
        public abstract void Process(IOrder order);
    }
}
