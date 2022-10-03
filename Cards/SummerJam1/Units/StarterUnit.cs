using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;
using SummerJam1.Piles;

namespace SummerJam1.Units
{
    public class StarterUnit : Unit, IDraggable, IGrantsEnergy, IMonster
    {
        [JsonIgnore] public bool CanDrag => Entity.Parent.Children.LastOrDefault() == Entity;


        protected override bool PlayCard(IEntity target)
        {
            return true;
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

            Health topMonsterHealth = args.Target.Children.LastOrDefault()?.GetComponent<Health>();
            Health myHealth = Entity.GetComponent<Health>();
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

            if (Entity.TrySetParent(slot.Entity))
            {
                ((SummerJam1Events)Events).OnCardMoved(new CardMovedEventArgs(Entity)); //todo: introduce try move event.
            }
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
