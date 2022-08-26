using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class StarterUnit : Unit, IDraggable, IGrantsEnergy, IMonster
    {
        [JsonIgnore] public bool CanDrag => Entity.Parent.GetComponent<HandPile>() != null; //can only drag while in hand.

        protected override bool PlayCard(IEntity target)
        {
            return true;
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
