using System.Linq;
using DeckbuilderLibrary.Data.Events;

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
            Owner.Resources.SetResource<Energy>(Amount);
            Context.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Owner.Resources.SetResource<Energy>(Amount);
        }
    }

    public class BaseCardDraw : Resource<BaseCardDraw>
    {
        public override string Name => nameof(BaseCardDraw);
        private IBattleDeck BattleDeck => Context.GetCurrentBattle().Deck;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.TurnStarted += OnTurnStarted;

        }
        

        private void OnTurnStarted(object sender, TurnStartedEventArgs args)
        {
            for (int i = 0; i < Amount; i++)
            {
                var card = BattleDeck.DrawPile.Cards.FirstOrDefault() ;
                if (card != null)
                {
                    Context.TrySendToPile(card.Id, PileType.HandPile);
                }
            }
        }
    }
}