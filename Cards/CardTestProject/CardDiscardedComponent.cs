using Api;

namespace Tests
{
    public class CardDiscardedComponent : Component
    {
        public bool CardDiscarded { get; private set; }

        [OnCardDiscarded]
        private void OnCardDiscarded()
        {
            CardDiscarded = true;
        }
    }
}