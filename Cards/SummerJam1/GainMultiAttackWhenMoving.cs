namespace SummerJam1
{
    public class GainMultiAttackWhenMoving : GainStatWhenMoving<MultiAttack>
    {
        protected override string StatName => nameof(MultiAttack);
    }
}