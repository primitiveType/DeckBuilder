using System.Linq;
using Api;

namespace SummerJam1.Rules
{
    public class HandleMovementPhase : SummerJam1Component
    {
        [OnMovementPhaseBegan]
        private void OnMovementPhaseBegan(object sender, MovementPhaseBeganEventArgs args)
        {
            for (int i = 0; i < BattleContainer.NumEncounterSlotsPerFloor; i++)
            {
                //slide them down.
                if (!Game.Battle.EncounterSlots[i].Entity.Children.Any())
                {
                    var child = Game.Battle.EncounterSlotsUpcoming[i].Entity.Children.FirstOrDefault();
                    if (child != null)
                    {
                        child.TrySetParent(Game.Battle.EncounterSlots[i].Entity);
                        Events.OnCardMoved(new CardMovedEventArgs(child));
                    }
                }
            }

            for (int i = BattleContainer.NumEncounterSlotsPerFloor - 1; i >= 0; i -= 1)
            {
                if (!Game.Battle.EncounterSlotsUpcoming[i].Entity.Children.Any())
                {
                    //upper slot is empty, slide to the right.

                    int sourceSlot = i - 1;
                    bool found = false;
                    while (!found)
                    {
                        if (sourceSlot < 0)
                        {
                            Game.Battle.EncounterDrawPile.DrawCardInto(Game.Battle.EncounterSlotsUpcoming[i].Entity);
                            found = true;
                        }
                        else
                        {
                            IEntity source = Game.Battle.EncounterSlotsUpcoming[sourceSlot].Entity.Children.FirstOrDefault();
                            if (source != null)
                            {
                                found = source.TrySetParent(Game.Battle.EncounterSlotsUpcoming[i].Entity);
                                Events.OnCardMoved(new CardMovedEventArgs(source));
                            }
                        }

                        sourceSlot--;
                    }
                }
            }
        }
    }
}