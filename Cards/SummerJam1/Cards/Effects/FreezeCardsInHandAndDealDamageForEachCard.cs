using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Cards.Effects
{
    public class FreezeCardsInHandAndDealDamageForEachCard : DamageUnitCard
    {
        public override string Description =>
            $"Freeze all cards in hand. Then, for each frozen card you own, all enemies take {DamageAmount} damage.";

        public override bool DoEffect(IEntity target)
        {
            foreach (IEntity card in Game.Battle.Hand.Entity.Children)
            {
                if (card == Entity)
                {
                    continue; //don't freeze this card.
                }

                card.GetOrAddComponent<Frozen>();
            }


            List<IEntity> cards = Game.Battle.GetAllPlayerCardsInPlay();
            int frozenCards = cards.Count(card => card.GetComponent<Frozen>() != null);

            int damage = frozenCards * DamageAmount;

            foreach (IEntity guy in Game.Battle.EncounterSlots.Entity.Children)
            {
                guy.GetComponent<ITakesDamage>()?.TryDealDamage(damage, Entity);
            }

            return true;
        }
    }
}