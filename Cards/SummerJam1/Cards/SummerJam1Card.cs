using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public interface IEffect
    {
        bool DoEffect(IEntity target);
    }
    public class SummerJam1Card : Card, IDraggable
    {
        [JsonIgnore] public bool CanDrag => Entity.Parent == Game.Battle?.Hand.Entity; //can only drag while in hand.
        protected SummerJam1Game Game { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Game = Context.Root.GetComponent<SummerJam1Game>();
        }

        protected override bool PlayCard(IEntity target)
        {
            bool played = false;
            foreach (IEffect effect in Entity.GetComponents<IEffect>())
            {
                played |= effect.DoEffect(target);
            }

            return played;
        }
    }

    public class Exhaust : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.RemoveComponent<Discard>();
        }

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.CardId.TrySetParent(Context.Root.GetComponent<SummerJam1Game>().Battle.Exhaust);
            }
        }
    }
    internal class Retain : SummerJam1Component
    {
    }
    public class Discard : SummerJam1Component
    {
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.CardId.TrySetParent(Context.Root.GetComponent<SummerJam1Game>().Battle.Discard);
            }
        }
    }
}
