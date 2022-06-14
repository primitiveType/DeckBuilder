using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public abstract class SummerJam1Card : Card, IDraggable
    {
        protected new SummerJam1Events Events => (SummerJam1Events)base.Events;

        [JsonIgnore] public bool CanDrag => Entity.Parent == Game.Battle?.Hand.Entity; //can only drag while in hand.
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
                args.CardId.TrySetParent(SendToAfterPlaying());
            }
        }

        private IEntity SendToAfterPlaying()
        {
            if (Entity.GetComponent<Exhaust>() != null)
            {
                return Context.Root.GetComponent<SummerJam1Game>().Battle.Exhaust;
            }
            return Context.Root.GetComponent<SummerJam1Game>().Battle.Discard;
        }
    }

    public class Exhaust : SummerJam1Component
    {
        
    }
    
}