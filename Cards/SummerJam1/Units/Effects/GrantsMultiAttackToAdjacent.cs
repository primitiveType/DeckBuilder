namespace SummerJam1.Units.Effects
{
    public class GrantsMultiAttackToAdjacent : GrantsAmount<MultiAttack>
    {
        public override bool EveryTurn => false;
        public override string Name => "Multi-Attack";
    }
}
