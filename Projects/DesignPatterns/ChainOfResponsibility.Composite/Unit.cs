namespace ChainOfResponsibility.Composite
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
        protected abstract bool CanProcess(Order order);
        public abstract void Approve(Order order);
        // For many operations we could end up with
        // public abstract void Cancel(Order order);
        // public abstract void Finish(Order order);
        // ...
    }
}
