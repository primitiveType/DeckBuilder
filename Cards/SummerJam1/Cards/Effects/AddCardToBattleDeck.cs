using Api;

namespace SummerJam1.Cards.Effects
{
    public class AddCardToBattleDeck : SummerJam1Component, IEffect
    {
        public string Prefab { get; set; }
        public int Amount { get; set; } = 1;

        public bool DoEffect(IEntity target)
        {
            for (int i = 0; i < Amount; i++)
            {
                Context.CreateEntity(Game.Battle.BattleDeck.Entity, Prefab);
            }


            return true;
        }
    }
}