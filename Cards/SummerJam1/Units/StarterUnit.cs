using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;
using SummerJam1.Statuses;

namespace SummerJam1.Units
{
    public class StarterUnit : Unit, IDraggable, IGrantsEnergy, IMonster
    {
        [JsonIgnore] public bool CanDrag => true;


        protected override bool PlayCard(IEntity target)
        {
            return true;
        }

        [OnBattleStarted]
        [OnTurnEnded]
        private void OnTurnEnded()
        {
            IsTopMonster topCheck = Entity.GetOrAddComponent<IsTopMonster>(); //could be problematic with serialization.

            if (!topCheck.Enabled)
            {
                Entity.GetOrAddComponent<FaceDown>();//will have to change if other things can cause face down.
            }
            else
            {
                Entity.RemoveComponent<FaceDown>();
            }
        }


        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            if (args.Target.GetComponent<EncounterSlotPile>() == null)
            {
                args.Blockers.Add(CardBlockers.INVALID_TARGET);
                return;
            }

            var topMonsterHealth = args.Target.Children.LastOrDefault()?.GetComponent<Health>();
            var myHealth = Entity.GetComponent<Health>();
            if (topMonsterHealth != null && topMonsterHealth.Amount <= myHealth.Amount)
            {
                args.Blockers.Add(CardBlockers.TOP_MONSTER_HAS_LESS_HEALTH);
            }
        }

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            EncounterSlotPile slot = args.Target.GetComponentInSelfOrParent<EncounterSlotPile>();

            Entity.TrySetParent(slot.Entity);
        }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Blood blood = Entity.GetComponent<Blood>();
                EncounterSlotPile slot = Entity.GetComponentInParent<EncounterSlotPile>();
                slot.Entity.GetOrAddComponent<Blood>().Amount += blood.Amount;
            }
        }
    }
}
