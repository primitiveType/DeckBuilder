namespace SummerJam1
{
    public class GrantsArmorToAdjacent : GrantsAmount<MultiAttack>
    {
        public override bool EveryTurn { get; } = true;
        public override string Name => "Armor";
    }
}