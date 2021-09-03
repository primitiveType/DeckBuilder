using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data
{
    internal interface IInternalBattleEventHandler : IBattleEventHandler
    {
        //To request damage, cards should use the game context.
        int RequestDamageAmount(object sender, int baseDamage, IGameEntity owner, IGameEntity target);
        void InvokeCardPlayed(object sender, CardPlayedEventArgs args);
        void InvokeCardMoved(object sender, CardMovedEventArgs args);
        void InvokeDamageDealt(object sender, DamageDealtArgs args);
        void InvokeCardCreated(object sender, CardCreatedEventArgs args);
        void InvokeActorDied(object sender, ActorDiedEventArgs args);
        void InvokeBattleEnded(object sender, BattleEndedEventArgs args);
        
        void InvokeTurnEnded(object sender, TurnEndedEventArgs args);
        void InvokeTurnStarted(object sender, TurnStartedEventArgs args);
        void InvokeIntentChanged(object sender, IntentChangedEventArgs args);
        void InvokeActorsSwapped(object battle, ActorsSwappedEventArgs actorsSwappedEventArgs);
    }
}