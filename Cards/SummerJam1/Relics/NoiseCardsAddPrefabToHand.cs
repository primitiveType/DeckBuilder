using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Relics
{
    public class NoiseCardsAddPrefabToHand : SummerJam1Component, ITooltip, IDescription
    {
        public string Prefab { get; set; }
        public string PrefabPrettyName { get; set; }

        public string Description => Tooltip;
        public string Tooltip => $"Whenever you play a noise card, add a {PrefabPrettyName} card to your hand.";

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId.GetComponent<Noisy>() != null)
            {
                Context.CreateEntity(Game.Battle.Hand.Entity, Prefab);
            }
        }
    }
}
