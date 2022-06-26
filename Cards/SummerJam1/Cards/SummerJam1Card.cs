using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public class SummerJam1Card : Card, IDraggable
    {
        protected SummerJam1Game Game { get; private set; }
        [JsonIgnore] public bool CanDrag => Entity.Parent == Game.Battle?.Hand.Entity; //can only drag while in hand.

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
}
