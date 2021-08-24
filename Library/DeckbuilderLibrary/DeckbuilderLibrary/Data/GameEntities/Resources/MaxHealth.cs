namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public class MaxHealth : Resource<MaxHealth>
    {
        public override string Name => nameof(MaxHealth);
        public override void Subtract(int amount)
        {
            base.Subtract(amount);
            //TODO: check health component and see if it needs to be reduced.
        }

        public override void Set(int amount)
        {
            base.Set(amount);
            //TODO: check health component and see if it needs to be reduced.
        }
    }
}