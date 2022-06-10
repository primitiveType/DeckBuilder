using CardsAndPiles;

namespace SummerJam1.Cards
{
    public abstract class SummerJam1Card : Card, IDraggable
    {
        protected new SummerJam1Events Events => (SummerJam1Events)base.Events;

        public bool CanDrag => Entity.Parent == Game.Hand.Entity;//can only drag while in hand.
        protected SummerJam1Game Game { get; private set; }        

        protected override void Initialize()
        {
            base.Initialize();
            Game = Context.Root.GetComponent<SummerJam1Game>();
        }


        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.CardId.TrySetParent(Context.Root.GetComponent<SummerJam1Game>().Discard);
            }
        }
    }
}