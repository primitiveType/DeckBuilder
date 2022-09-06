namespace SummerJam1
{
    public class GainStrengthWhenMoving : GainStatWhenMoving<Strength>
    {
        protected override string StatName => nameof(Strength);
    }
}