namespace LazyFunc;

internal class OutFuncFactory : IOutFactory
{
    private readonly Func<IOut> _func;

    public OutFuncFactory(Func<IOut> @func)
    {
        _func = func;
    }
    public IOut Create()
    {
        return _func();
    }
}