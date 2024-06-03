namespace SummerJam1.Units.Effects
{
    public class GrantsStrengthToAdjacent : GrantsAmount<Strength>
    {
        public override bool EveryTurn => false;
        public override string Name => "Strength";
    }
}
