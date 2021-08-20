using System.Collections.Generic;
using UnityEngine;
using Data;

public class EnemyActorManager : MonoBehaviour
{
    [SerializeField] EnemyActorProxy ActorProxyPrefab;

    public List<EnemyActorProxy> EnemyActors = new List<EnemyActorProxy>();

    public float EnemyWidth;
    public float EnemySeperation;

    public Vector3 CenterPosition;


    public void CreateEnemyActor(Actor enemyActor)
    {
        EnemyActorProxy enemyProxy = Instantiate(ActorProxyPrefab);
        enemyProxy.Initialize(enemyActor);
        EnemyActors.Add(enemyProxy);

        int numEnemies = EnemyActors.Count;


        float enemyOffset = ((numEnemies - 1) * EnemyWidth) / 2 + ((numEnemies - 1) * EnemySeperation) / 2;

        Vector3 startPosition = enemyOffset * Vector3.left + CenterPosition;

        for (int i = 0; i < numEnemies; i++)
        {
            EnemyActors[i].transform.position = i * (EnemyWidth + EnemySeperation) * Vector3.right + startPosition;
        }
    }
}