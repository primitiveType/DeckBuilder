namespace SummerJam1.Units.Effects
{
    public class GrantsArmorToAdjacent : GrantsAmount<MultiAttack>
    {
        public override bool EveryTurn { get; } = true;
        public override string Name => "Armor";
    }
}