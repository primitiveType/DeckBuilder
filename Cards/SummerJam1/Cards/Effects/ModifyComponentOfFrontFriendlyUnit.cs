using Api;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public abstract class ModifyComponentOfFrontFriendlyUnit<T> : SummerJam1Component, IEffect where T : Component, new()
    {
        [JsonProperty] public int Amount { get; private set; }
        protected string ReduceIncrease => Amount > 0 ? "Increase" : "Reduce";


        public bool DoEffect(IEntity target)
        {
            IEntity front = Game.Battle.GetFrontMostFriendly();
            if (front == null)
            {
                return false;
            }

            ModifyComponent(front.GetOrAddComponent<T>());


            return true;
        }

        protected abstract void ModifyComponent(T component);
    }
}