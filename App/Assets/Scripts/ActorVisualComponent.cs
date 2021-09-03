using System;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using UnityEngine;

public class ActorVisualComponent : GameEntityComponent
{
    [SerializeField] private GameObject m_EnemyVisual;
    [SerializeField] private GameObject m_PlayerVisual;

    private void Start()
    {
        switch (GameEntity)
        {
            case BasicEnemy basicEnemy:
                Instantiate(m_EnemyVisual, transform).transform.localPosition = Vector3.zero;
                break;
            case PlayerActor playerActor:
                Instantiate(m_PlayerVisual, transform).transform.localPosition = Vector3.zero;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(GameEntity));
        }
    }
}