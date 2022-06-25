using Api;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public abstract class ModifyComponentOfAllEnemyUnits<T> : SummerJam1Component, IEffect where T : Component, new()
    {
        [JsonProperty] public int Amount { get; private set; }
        protected string ReduceIncrease => Amount > 0 ? "Increase" : "Reduce";


        public bool DoEffect(IEntity target)
        {
            foreach (IEntity friendly in Game.Battle.GetEnemies())
            {
                ModifyComponent(friendly.GetOrAddComponent<T>());
            }

            return true;
        }

        protected abstract void ModifyComponent(T component);
    }
}