using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data.Events
{
    public class IntentChangedEventArgs
    {
        public Enemy Owner { get; }

        public IntentChangedEventArgs(Enemy owner)
        {
            Owner = owner;
        }
    }
}