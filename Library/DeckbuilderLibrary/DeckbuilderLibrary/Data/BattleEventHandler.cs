using System;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data
{
    internal class BattleEventHandler : IInternalBattleEventHandler
    {
        public event CardMovedEvent CardMoved;

        public event RequestDamageAmountEvent DamageAmountRequested;
        public event ActorDiedEvent ActorDied;
        public event BattleEndedEvent BattleEnded;
        public event TurnEndedEvent TurnEnded;
        public event TurnStartedEvent TurnStarted;
        public event IntentChangedEvent IntentChanged;

        public void InvokeIntentChanged(object sender, IntentChangedEventArgs args)
        {
            IntentChanged?.Invoke(sender, args);
        }
        public void InvokeTurnEnded(object sender, TurnEndedEventArgs args)
        {
            TurnEnded?.Invoke(sender, args);
        }
        public void InvokeTurnStarted(object sender, TurnStartedEventArgs args)
        {
            TurnStarted?.Invoke(sender, args);
        }
        public void InvokeBattleEnded(object sender, BattleEndedEventArgs args) 
        {
            BattleEnded?.Invoke(sender, args);
        }

        public void InvokeActorDied(object sender, ActorDiedEventArgs args)
        {
            ActorDied?.Invoke(sender, args);
        }
        public void InvokeCardMoved(object sender, CardMovedEventArgs args)
        {
            CardMoved?.Invoke(sender, args);
        }

        public event CardPlayedEvent CardPlayed;

        public void InvokeCardPlayed(object sender, CardPlayedEventArgs args)
        {
            CardPlayed?.Invoke(sender, args);
        }

        public event CardCreatedEvent CardCreated;

        public void InvokeCardCreated(object sender, CardCreatedEventArgs args)
        {
            CardCreated?.Invoke(sender, args);
        }

        public event DamageDealt DamageDealt;

        public void InvokeDamageDealt(object sender, DamageDealtArgs args)
        {
            DamageDealt?.Invoke(sender, args);
        }

        public int RequestDamageAmount(object sender, int baseDamage, IGameEntity owner, IGameEntity target)
        {
            var args = new RequestDamageAmountEventArgs(owner, target);
            args.AddModifier(new DamageAmountModifier { AdditiveModifier = baseDamage });
            DamageAmountRequested?.Invoke(sender, args);
            return args.GetResult();
        }
    }
}