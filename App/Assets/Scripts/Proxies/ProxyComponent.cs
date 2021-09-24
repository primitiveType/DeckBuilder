using DeckbuilderLibrary.Data.GameEntities;

public class ProxyComponent<T> : EntityBehaviour<T>, IProxyComponent where T : class, IGameEntity
{
    protected override void OnInitialize()
    {
    }

    public void Initialize(IGameEntity entity)
    {
        base.Initialize((T)entity);
    }
}

public interface IProxyComponent : IInitialize
{
}

public interface IProxy : IInitialize
{
}

public interface IInitialize
{
    void Initialize(IGameEntity entity);
}