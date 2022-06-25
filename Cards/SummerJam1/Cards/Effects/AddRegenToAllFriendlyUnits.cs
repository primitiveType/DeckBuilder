using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Statuses;

namespace SummerJam1.Cards
{
    public class AddRegenToAllFriendlyUnits : SummerJam1Component, IEffect
    {
        [JsonProperty] public int Amount { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Amount} regen to all friendly units.";
            }
        }

        public bool DoEffect(IEntity target)
        {
            var friendlies = Game.Battle.GetFriendlies();
            foreach (IEntity friendly in friendlies)
            {
                Regen regen = friendly.GetOrAddComponent<Regen>();
                regen.Amount += Amount;
            }

            return true;
        }
    }
}