using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.Property
{
    public class TurnProperty<T> : GameProperty<T>
    {
        public TurnProperty(T value) : base(value)
        {
            GameContext.CurrentContext.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Value = default(T);
        }

        public TurnProperty()
        {
            GameContext.CurrentContext.Events.TurnEnded += OnTurnEnded;
        }
    }
}