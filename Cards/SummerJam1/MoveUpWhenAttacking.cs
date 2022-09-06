using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class MoveUpWhenAttacking : SummerJam1Component, IDescription
    {
        public string Description => "After attacking, swaps with the unit above it.";

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                EncounterSlotPile currentSlot = Entity.GetComponentInParent<EncounterSlotPile>();
                if (currentSlot != null)
                {
                    IEntity newSlot = Game.Battle.GetSlotAboveOrBelow(currentSlot);

                    IEntity toSwapWith  = newSlot.Children.FirstOrDefault();
                    //unparent ourselves first.
                    Entity.TrySetParent(null);

                    //try parent other to our old parent.
                    if (toSwapWith == null || toSwapWith.TrySetParent(currentSlot.Entity))
                    {
                        //try parent ourselves to our new parent 
                        if (Entity.TrySetParent(newSlot))
                        {
                            Events.OnCardMoved(new CardMovedEventArgs(Entity));
                            Events.OnCardMoved(new CardMovedEventArgs(toSwapWith));
                        }
                        else//our new slot was not valid, undo things.
                        {
                            Logging.Log("Unable to swap because this unit could not go in new slot.");
                            toSwapWith?.TrySetParent(newSlot);
                            Entity.TrySetParent(currentSlot.Entity);
                        }
                    }
                    else
                    {
                        Logging.Log("Unable to swap because other unit could not go in our slot.");
                        Entity.TrySetParent(currentSlot.Entity);
                    }
                }
            }
        }
    }
}