using Api;
using Newtonsoft.Json;
using SummerJam1.Statuses;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public class AddRegenToUnit : SummerJam1Component, IEffect
    {
        [JsonProperty] public int Amount { get; set; }

        public bool DoEffect(IEntity target)
        {
            IEntity unit = target.GetComponentInChildren<Unit>()?.Entity;
            if (unit == null)
            {
                return false;
            }

            Regen regen = unit.GetOrAddComponent<Regen>();
            regen.Amount += Amount;
            return true;
        }
    }

    public class AddRegenToAllFriendlyUnits : SummerJam1Component, IEffect
    {
        [JsonProperty] public int Amount { get; set; }

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
