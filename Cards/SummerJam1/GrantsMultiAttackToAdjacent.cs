namespace SummerJam1
{
    public class GrantsMultiAttackToAdjacent : GrantsAmount<MultiAttack>
    {
        public override bool EveryTurn { get; }
        public override string Name => "Multi-Attack";
    }
}
