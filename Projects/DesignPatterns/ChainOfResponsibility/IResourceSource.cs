namespace ChainOfResponsibility
{
    public interface IResourceSource
    {
        string Acquire(string handle);
    }
}