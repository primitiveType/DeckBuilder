using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public abstract class GainStatWhenMoving<T> : SummerJam1Component, IAmount, IDescription where T : Component, IAmount, new()
    {
        protected abstract string StatName { get; }
        public int Amount { get; set; }
        
        public bool AnyMovementCounts { get; set; }
        public bool ResetEachTurn { get; set; }
        private int AmountThisTurn { get; set; }

        public string Description
        {
            get
            {
                string untilEndOfTurn = ResetEachTurn ? " until end of turn" : "";
                if (AnyMovementCounts)
                {
                    return $"Gains {Amount} {StatName}{untilEndOfTurn} each time any creature is moved.";
                }
                return $"Gains {Amount} {StatName}{untilEndOfTurn} each time it moves.";
            }
        }

        [OnCardMoved]
        private void OnCardMoved(object sender, CardMovedEventArgs args)
        {
            if (AnyMovementCounts || args.CardId == Entity)
            {
                Entity.GetOrAddComponent<T>().Amount += Amount;
                AmountThisTurn += Amount;
            }
        }

        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            if (AmountThisTurn > 0)
            {
                Entity.GetOrAddComponent<T>().Amount -= AmountThisTurn;
                AmountThisTurn = 0;
            }
        }
    }
}
