using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.Property
{
    public class BattleProperty<T> : GameProperty<T>
    {
        public BattleProperty(T value) : base(value)
        {
            GameContext.CurrentContext.Events.BattleEnded += OnBattleEnded;
        }

        public BattleProperty()
        {
            GameContext.CurrentContext.Events.BattleEnded += OnBattleEnded;
        }

        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            Value = default(T);
        }
    }
}