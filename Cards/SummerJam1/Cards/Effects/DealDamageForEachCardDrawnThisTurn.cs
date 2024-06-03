using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class DealDamageForEachCardDrawnThisTurn : SummerJam1Component, IEffect, IDescription
    {
        [JsonProperty] public int Amount { get; set; }
        [JsonProperty] public int Multiplier { get; set; } = 1;

        public string Description => $"Deal {Multiplier} damage for each card drawn this turn. ({Amount * Multiplier})";

        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(Amount * Multiplier, Entity);
            return true;
        }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Amount = 0;
        }

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            if (args.IsHandDraw)
            {
                return;
            }


            Amount++;
        }
    }
}