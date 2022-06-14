using Api;
using SummerJam1;
using UnityEngine;

namespace App
{
    public class PrizePileOrganizer : PileOrganizer
    {
        protected override void OnItemAdded(IEntity added)
        {
            base.OnItemAdded(added);
            GameObject entityGO = added.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null)
            {
                entityGO.AddComponent<CardInPrizePool>();
            }
        }

        protected override void OnItemRemoved(IEntity removed)
        {
            base.OnItemRemoved(removed);
            GameObject entityGO = removed.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null)
            {
                var com = entityGO.GetComponent<CardInPrizePool>();
                Destroy(com);
            }
        }
    }
}