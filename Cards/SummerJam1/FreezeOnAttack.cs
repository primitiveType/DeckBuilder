using System.Linq;
using System.Runtime.ConstrainedExecution;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1
{
    public class FreezeOnAttack : EnabledWhenInEncounterSlot, ITooltip, IDescription
    {
        public string Description => Tooltip;

        public string Tooltip => "Ice Breath - when attacking, freezes a random card in your hand.";


        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                IEntity random = Game.Battle.Hand.GetRandomWithCondition((entity, _) => entity.GetComponent<Frozen>() == null);
                random?.AddComponent<Frozen>();
            }
        }
    }

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

    public class GainStrengthWhenMoving : GainStatWhenMoving<Strength>
    {
        protected override string StatName => nameof(Strength);
    }

    public class GainBlockWhenMoving : GainStatWhenMoving<Strength>
    {
        protected override string StatName => "Block";
    }

    public abstract class GainStatWhenMoving<T> : SummerJam1Component, IAmount, IDescription where T : Component, IAmount, new()
    {
        protected abstract string StatName { get; }
        public int Amount { get; set; }

        public string Description => $"Gains {Amount} {StatName} each time it moves.";

        [OnCardMoved]
        private void OnCardMoved(object sender, CardMovedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                Entity.GetOrAddComponent<T>().Amount += Amount;
            }
        }
    }

    public class GainStrengthEachTurnWhenInUpcomingSlot : SummerJam1Component, IAmount, IDescription
    {
        public int Amount { get; set; }

        public string Description => $"Gains {Amount} Strength each turn spent above the board.";


        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            if (Entity.GetComponentInParent<UpcomingEncounterSlotPile>() != null)
            {
                Entity.GetOrAddComponent<Strength>().Amount += Amount;
            }
        }
    }

    public class BurnOnAttack : EnabledWhenInEncounterSlot, ITooltip, IDescription, IAmount
    {
        public int Amount { get; set; } = 1;
        public string Description => Tooltip;


        public string Tooltip => $"Fire Breath - when attacking, adds {Amount} burn.";

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                args.EntityId.GetOrAddComponent<Burn>().Amount += Amount;
            }
        }
    }
}
