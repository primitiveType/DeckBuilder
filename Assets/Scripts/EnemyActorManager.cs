using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;

public class EnemyActorManager : MonoBehaviour
{
    [SerializeField] EnemyActorProxy ActorProxyPrefab;

    public List<EnemyActorProxy> EnemyActors = new List<EnemyActorProxy>();

    public float EnemyWidth;
    public float EnemySeperation;

    public Vector3 CenterPosition;

    private IGameEventHandler GameEventHandler => Injector.GameEventHandler;

    private void Start()
    {
        GameEventHandler.DamageDealt += OnDamageDealt;
    }

    private void OnDamageDealt(object sender, DamageDealtArgs args)
    {
        foreach(EnemyActorProxy actorProxy in EnemyActors)
        {
            if(actorProxy.GameEntity.Id == args.ActorId)
            {
                actorProxy.DamageReceived();
            }
        }
    }

    public void CreateEnemyActor(Actor enemyActor)
    {
        EnemyActorProxy enemyProxy = Instantiate(ActorProxyPrefab);
        enemyProxy.Initialize(enemyActor);
        EnemyActors.Add(enemyProxy);

        int numEnemies = EnemyActors.Count;


        float enemyOffset = ((numEnemies - 1) * EnemyWidth) / 2 + ((numEnemies - 1) * EnemySeperation) / 2;

        Vector3 startPosition = enemyOffset * Vector3.left + CenterPosition;

        for(int i = 0; i < numEnemies; i++)
        {
            EnemyActors[i].transform.position = i * (EnemyWidth + EnemySeperation) * Vector3.right + startPosition;
        }


    }
}
