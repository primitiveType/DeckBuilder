using Api;

namespace SummerJam1.Cards
{
    public class StartingCard : SummerJam1Component, IAmount
    {
        public int Amount { get; set; } = 1;
    }

    public class CharacterConstraint : SummerJam1Component
    {
        public string Character { get; set; } = "Any";
    }
}
