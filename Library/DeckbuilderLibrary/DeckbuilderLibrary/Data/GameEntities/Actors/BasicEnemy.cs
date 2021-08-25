namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    public class BasicEnemy : Enemy
    {
        public int Strength => 5;

        protected override void Initialize()
        {
            base.Initialize();
            if (Intent == null)
            {
                var intent = Context.CreateIntent<DamageIntent>(this);
                intent.DamageAmount = Strength;
                SetIntent(intent);
            }
        }
    }
}