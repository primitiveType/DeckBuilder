using Api;

namespace Tests
{
    public class CardPlayedComponent : Component
    {
        public bool CardPlayed { get; private set; }

        [OnCardPlayedAttribute]
        private void OnCardPlayed()
        {
            CardPlayed = true;
        }
    }
    public class CardDiscardedComponent : Component
    {
        public bool CardDiscarded { get; private set; }

        [OnCardDiscardedAttribute]
        private void OnCardDiscarded()
        {
            CardDiscarded = true;
        }
    }
}