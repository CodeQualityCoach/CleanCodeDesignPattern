namespace LazyFunc;

internal class OutLazyFactory : IOutFactory
{
    private readonly Lazy<IOut> _lazy;

    public OutLazyFactory(Lazy<IOut> lazy)
    {
        _lazy = lazy;
    }
    public IOut Create()
    {
        return _lazy.Value;
    }
}