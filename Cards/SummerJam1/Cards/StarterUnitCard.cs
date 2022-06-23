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

    public class DrawOnFirstTurn : SummerJam1Component
    {
        [OnBattleStarted]
        private void BattleStarted()
        {
            if (Entity.GetComponentInParent<BattleContainer>() != null)
            {
                Entity.TrySetParent(Game.Battle.Hand.Entity);
            }
        }
    }

    public class Spicy : SummerJam1Component
    {
    }

    public class Sweet : SummerJam1Component
    {
    }

    public class Savory : SummerJam1Component
    {
    }
}
