using Api;
using CardsAndPiles.Components;

namespace SummerJam1
{
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
}