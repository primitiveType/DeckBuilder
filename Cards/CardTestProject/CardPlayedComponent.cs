using System.ComponentModel;
using JetBrains.Annotations;
using Component = Api.Component;

namespace Tests
{
    public class CardPlayedComponent : Component
    {
        public bool CardPlayed { get; private set; }

        [OnCardPlayed]
        private void OnCardPlayed()
        {
            CardPlayed = true;
        }
    }
}