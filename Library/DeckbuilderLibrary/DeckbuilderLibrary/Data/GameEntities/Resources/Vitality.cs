namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public class Vitality : Resource<Vitality>
    {
        public override int Amount
        {
            get => 0;
            protected set { }
        }

        public override string Name => nameof(Vitality);
    }
}