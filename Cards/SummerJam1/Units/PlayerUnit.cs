using Api;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class PlayerUnit : Unit
    {
        [JsonProperty]
        public string UnitName { get; private set; }

        public PlayerUnit()
        {
            Logging.Log("Created player unit.");
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}