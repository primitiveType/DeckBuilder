using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;

namespace SummerJam1.Units
{
    public class BloodRequirement : SummerJam1Component, IAmount, ITooltip
    {
        [JsonProperty] public int Amount { get; set; }
        public string Tooltip => $"This card requires a slot with at least {Amount} blood.";

        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }
            EncounterSlotPile slot = args.Target.GetComponentInSelfOrParent<EncounterSlotPile>();

            if (slot == null || slot.Entity.Children.Count > 0)
            {
                args.Blockers.Add(CardBlockers.INVALID_TARGET);
            }

            if (Amount <= 0)
            {
                return;
            }

            Blood blood = slot.Entity.GetOrAddComponent<Blood>();
            if (blood.Amount < Amount)
            {
                args.Blockers.Add(CardBlockers.NOT_ENOUGH_BLOOD);
            }
        }
    }

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

    public interface IGrantsEnergy
    {
    }
}
