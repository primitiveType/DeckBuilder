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


            RequestMoveUnitEventArgs tryMoveArgs = new(Entity, true, args.Target);
            ((SummerJam1Events)Events).OnRequestMoveUnit(tryMoveArgs);
            if (tryMoveArgs.Blockers.Any())
            {
                args.Blockers.Add(CardBlockers.UNIT_CANT_MOVE);

                foreach (string blocker in tryMoveArgs.Blockers)
                {
                    Logging.Logger.Log($"Unable to move unit : {blocker}.");
                }
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
                ((SummerJam1Events)Events).OnUnitMoved(new UnitMovedEventArgs(Entity, true, slot.Entity));
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
