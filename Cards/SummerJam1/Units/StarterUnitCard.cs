using Api;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class StarterUnitCard : UnitCard
    {
        [JsonProperty] public string PrefabName = "StarterUnit.json";

        protected override Unit CreateUnit()
        {
            IEntity unitEntity = Context.CreateEntity(null, PrefabName);

            return unitEntity.GetComponent<StarterUnit>();
        }

        public override string Description => "Summon an IceCream that turns into HeadCheese.";
    }
}