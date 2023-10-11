public interface IUnit
{
    string Name { get; }

    void Accept(IVisitor visitor);
}