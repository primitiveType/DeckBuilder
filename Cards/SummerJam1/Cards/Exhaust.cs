using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class Exhaust : SummerJam1Component, ITooltip
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

        public string Tooltip => "Consume- This card can only be played once per battle.";
    }
}
