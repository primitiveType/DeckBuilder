namespace SummerJam1
{
    public class GrantsStrengthToAdjacent : GrantsAmount<Strength>
    {
        public override bool EveryTurn { get; }
        public override string Name => "Strength";
    }
}
