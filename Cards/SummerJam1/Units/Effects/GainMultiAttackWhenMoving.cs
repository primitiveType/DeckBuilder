namespace SummerJam1.Units.Effects
{
    public class GainMultiAttackWhenMoving : GainStatWhenMoving<MultiAttack>
    {
        protected override string StatName => nameof(MultiAttack);
    }
}
