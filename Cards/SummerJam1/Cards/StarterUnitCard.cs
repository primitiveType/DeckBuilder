using System.IO;
using Api;
using Newtonsoft.Json;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public class StarterUnitCard : UnitCard
    {
        [JsonProperty] public string PrefabName = "StarterUnit.json";

        protected override Unit CreateUnit()
        {
            IEntity unitEntity = Context.CreateEntity(null, Path.Combine("Units", PrefabName));

            return unitEntity.GetComponent<StarterUnit>();
        }
    }
}
