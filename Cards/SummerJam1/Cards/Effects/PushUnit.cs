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
                RequestMoveUnitEventArgs tryMoveArgs = new(Entity, false, target);
                if (!tryMoveArgs.Blockers.Any())
                {
                    foreach (string blocker in tryMoveArgs.Blockers)
                    {
                        Logging.Log($"Unable to push card : {blocker}");
                    }
                    return false;
                }

                Events.OnUnitMoved(new UnitMovedEventArgs(unit, false, target)); 
                return true;
            }

            return false;
        }

        public string Tooltip => "Push - Pushes the unit to the right.";
    }
}
