using SummerJam1.Relics;

namespace SummerJam1.Characters
{
    public class Quartermaster : SummerJam1Component, ICharacterClass
    {
        public string Name => "Quartermaster";
        protected override void Initialize()
        {
            base.Initialize();
            //starting relic.
            Entity.AddComponent<BagOfHolding>();
        }
    }
}
