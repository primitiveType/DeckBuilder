using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public class BaseEnergy : Resource<BaseEnergy>
    {
        public override string Name => nameof(BaseEnergy);

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.BattleStarted += OnBattleStarted;
        }

        private void OnBattleStarted(object sender, BattleStartedArgs args)
        {
            ((IActor)Owner).Resources.SetResource<Energy>(Amount);
            Context.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            ((IActor)Owner).Resources.SetResource<Energy>(Amount);
        }
    }
}