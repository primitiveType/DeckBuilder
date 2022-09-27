using System.Linq;
using Api;
using CardsAndPiles.Components;
using SummerJam1.Piles;

namespace SummerJam1.Cards.Effects
{
    public class PushUnit : TargetSlotComponent, IEffect, IDescription, ITooltip
    {
        public string Description => "Push.";

        public bool DoEffect(IEntity target)
        {
            EncounterSlotPile slot = target.GetComponentInSelfOrParent<EncounterSlotPile>();

            IEntity unit = slot.Entity.Children.LastOrDefault();

            if (unit == null)
            {
                return false;
            }

            IEntity targetSlot = Game.Battle.GetSlotToRight(unit);
            if (targetSlot == null)
            {
                return false;
            }

            if (unit.TrySetParent(targetSlot))
            {
                Events.OnCardMoved(new CardMovedEventArgs(unit)); //todo: introduce try move event.
                return true;
            }

            return false;
        }

        public string Tooltip => "Push - Pushes the unit to the right.";
    }
}