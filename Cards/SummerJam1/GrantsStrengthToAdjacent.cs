namespace SummerJam1
{
    public class GrantsStrengthToAdjacent : GrantsAmount<Strength>
    {
        public override bool EveryTurn => false;
        public override string Name => "Strength";
    }
}
