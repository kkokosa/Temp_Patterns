namespace ChainOfResponsibility._Intro
{
    public interface IResourceSource
    {
        string Acquire(string handle);
    }
}