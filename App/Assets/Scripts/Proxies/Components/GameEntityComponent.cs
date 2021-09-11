using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public abstract class GameEntityComponent : MonoBehaviour
{
    protected IGameEntity GameEntity => EntityProperty.GameEntity;
    private IGameEntityProperty EntityProperty { get; set; }

    protected IContext Context => GameEntity.Context;
    private void Awake()
    {
        EntityProperty = GetComponentInParent<IGameEntityProperty>();
    }
}