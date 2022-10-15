namespace SummerJam1.Units.Effects
{
    public class CreaturesMovedOnTopAreGivenStrength : CreaturesMovedOnTopAreGiven<Strength>
    {
        protected override string GetEffectName()
        {
            return nameof(Strength);
        }
    }
}
