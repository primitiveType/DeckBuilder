namespace SummerJam1.Units.Effects
{
    public class AfterMoveCardBelowIsGivenStrength : AfterMoveCardBelowIsGiven<Strength>
    {
        protected override string GetEffectName()
        {
            return nameof(Strength);
        }
    }
}