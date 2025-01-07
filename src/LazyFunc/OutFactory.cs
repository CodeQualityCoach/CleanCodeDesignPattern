namespace LazyFunc;

internal class OutFactory : IOutFactory
{
    public IOut Create()
    {
        return new ConsoleOut();
    }
}