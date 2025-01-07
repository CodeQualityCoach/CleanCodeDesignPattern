using StructureMap;

namespace LazyFunc;

internal class ContainerFactory : IOutFactory
{
    private readonly IContainer _container;

    public ContainerFactory(IContainer container)
    {
        _container = container;
    }
    public IOut Create()
    {
        return _container.GetInstance<IOut>();
    }
}