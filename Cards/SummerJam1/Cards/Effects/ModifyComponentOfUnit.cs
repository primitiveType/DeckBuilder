using Api;
using Newtonsoft.Json;
using SummerJam1.Units;

namespace SummerJam1.Cards.Effects
{
    public abstract class ModifyComponentOfUnit<T> : SummerJam1Component, IEffect where T : Component, new()
    {
        [JsonProperty] public int Amount { get; private set; }
        protected string ReduceIncrease => Amount > 0 ? "Increase" : "Reduce";


        public bool DoEffect(IEntity target)
        {
            Unit unit = target.GetComponentInChildren<Unit>();

            if (unit == null)
            {
                return false;
            }

            ModifyComponent(unit.Entity.GetOrAddComponent<T>());
            return true;
        }

        protected abstract void ModifyComponent(T component);
    }
}