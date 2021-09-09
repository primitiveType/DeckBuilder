using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;

namespace DeckbuilderLibrary.Data
{
    internal class GameEvents : IInternalGameEvents
    {
        private Battle Battle;
        public event BattleStarted BattleStarted;

        internal void InvokeBattleStarted(object sender, BattleStartedArgs args)
        {
            BattleStarted?.Invoke(sender, args);
        }

        public int RequestDamageAmount(object sender, int baseDamage, IGameEntity owner, IGameEntity target)
        {
            return Battle.Events.RequestDamageAmount(sender, baseDamage, owner, target);
        }

        public void InvokeCardPlayed(object sender, CardPlayedEventArgs args)
        {
            Battle.Events.InvokeCardPlayed(sender, args);
        }

        public void InvokeCardMoved(object sender, CardMovedEventArgs args)
        {
            Battle.Events.InvokeCardMoved(sender, args);
        }

        public void InvokeDamageDealt(object sender, DamageDealtArgs args)
        {
            Battle.Events.InvokeDamageDealt(sender, args);
        }

        public void InvokeCardCreated(object sender, CardCreatedEventArgs args)
        {
            Battle.Events.InvokeCardCreated(sender, args);
        }

        public void InvokeActorDied(object sender, ActorDiedEventArgs args)
        {
            Battle.Events.InvokeActorDied(sender, args);
        }

        public void InvokeBattleEnded(object sender, BattleEndedEventArgs args)
        {
            Battle.Events.InvokeBattleEnded(sender, args);
        }

        public void InvokeTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Battle.Events.InvokeTurnEnded(sender, args);
        }

        public void InvokeTurnStarted(object sender, TurnStartedEventArgs args)
        {
            Battle.Events.InvokeTurnStarted(sender, args);
        }

        public void InvokeIntentChanged(object sender, IntentChangedEventArgs args)
        {
            Battle.Events.InvokeIntentChanged(sender, args);
        }

        public void InvokeActorsSwapped(object sender, ActorsSwappedEventArgs actorsSwappedEventArgs)
        {
            Battle.Events.InvokeActorsSwapped(sender, actorsSwappedEventArgs);
        }

        public event CardMovedEvent CardMoved;

        private void MediateCardMovedEvent(object sender, CardMovedEventArgs args)
        {
            CardMoved?.Invoke(sender, args);
        }

        public event CardPlayedEvent CardPlayed;

        private void MediateCardPlayedEvent(object sender, CardPlayedEventArgs args)
        {
            CardPlayed?.Invoke(sender, args);
        }

        public event CardCreatedEvent CardCreated;

        private void MediateCardCreatedEvent(object sender, CardCreatedEventArgs args)
        {
            CardCreated?.Invoke(sender, args);
        }

        public event DamageDealt DamageDealt;

        private void MediateDamageDealtEvent(object sender, DamageDealtArgs args)
        {
            DamageDealt?.Invoke(sender, args);
        }

        public event RequestDamageAmountEvent DamageAmountRequested;

        private void MediateDamageAmountRequestedEvent(object sender, RequestDamageAmountEventArgs args)
        {
            DamageAmountRequested?.Invoke(sender, args);
        }

        public event ActorDiedEvent ActorDied;

        private void MediateActorDiedEvent(object sender, ActorDiedEventArgs args)
        {
            ActorDied?.Invoke(sender, args);
        }

        public event BattleEndedEvent BattleEnded;

        private void MediateBattleEndedEvent(object sender, BattleEndedEventArgs args)
        {
            BattleEnded?.Invoke(sender, args);
        }

        public event TurnEndedEvent TurnEnded;

        private void MediateTurnEndedEvent(object sender, TurnEndedEventArgs args)
        {
            TurnEnded?.Invoke(sender, args);
        }

        public event TurnStartedEvent TurnStarted;

        private void MediateTurnStartedEvent(object sender, TurnStartedEventArgs args)
        {
            TurnStarted?.Invoke(sender, args);
        }

        public event IntentChangedEvent IntentChanged;
        public event ActorsSwappedEvent ActorsSwapped;

        private void MediateIntentChangedEvent(object sender, IntentChangedEventArgs args)
        {
            IntentChanged?.Invoke(sender, args);
        }

        void IInternalGameEvents.SetBattle(Battle newBattle)
        {
            DetachListeners();
            Battle = newBattle;
            AttachListeners();
        }

        private void AttachListeners()
        {
            Battle.Events.CardMoved += MediateCardMovedEvent;
            Battle.Events.CardPlayed += MediateCardPlayedEvent;
            Battle.Events.CardCreated += MediateCardCreatedEvent;
            Battle.Events.DamageDealt += MediateDamageDealtEvent;
            Battle.Events.DamageAmountRequested += MediateDamageAmountRequestedEvent;
            Battle.Events.ActorDied += MediateActorDiedEvent;
            Battle.Events.BattleEnded += MediateBattleEndedEvent;
            Battle.Events.TurnEnded += MediateTurnEndedEvent;
            Battle.Events.TurnStarted += MediateTurnStartedEvent;
            Battle.Events.IntentChanged += MediateIntentChangedEvent;
            Battle.Events.ActorsSwapped += MediateActorsSwappedEvent;
        }

        private void MediateActorsSwappedEvent(object sender, ActorsSwappedEventArgs args)
        {
            ActorsSwapped?.Invoke(sender, args);
        }

        private void DetachListeners()
        {
            if (Battle != null)
            {
                Battle.Events.CardMoved -= MediateCardMovedEvent;
                Battle.Events.CardPlayed -= MediateCardPlayedEvent;
                Battle.Events.CardCreated -= MediateCardCreatedEvent;
                Battle.Events.DamageDealt -= MediateDamageDealtEvent;
                Battle.Events.DamageAmountRequested -= MediateDamageAmountRequestedEvent;
                Battle.Events.ActorDied -= MediateActorDiedEvent;
                Battle.Events.BattleEnded -= MediateBattleEndedEvent;
                Battle.Events.TurnEnded -= MediateTurnEndedEvent;
                Battle.Events.TurnStarted -= MediateTurnStartedEvent;
                Battle.Events.IntentChanged -= MediateIntentChangedEvent;
                Battle.Events.ActorsSwapped -= MediateActorsSwappedEvent;
            }
        }
    }
}