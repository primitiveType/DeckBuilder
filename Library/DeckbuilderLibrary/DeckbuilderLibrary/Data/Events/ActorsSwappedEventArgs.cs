using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.Events
{
    public class ActorsSwappedEventArgs
    {
        public readonly IActor Actor1;
        public readonly IActor Actor2;

        public ActorsSwappedEventArgs(IActor actor1, IActor actor2)
        {
            Actor1 = actor1;
            Actor2 = actor2;
        }
    }
}