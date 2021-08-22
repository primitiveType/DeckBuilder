using System.Collections.Generic;
using Data;

namespace DeckbuilderLibrary.Data
{
    internal class GameEventHandler : IInternalGameEventHandler
    {
        public event CardMovedEvent CardMoved;

        public event RequestDamageAmountEvent RequestDamageAmount;
        public event ActorDiedEvent ActorDied;
        public event BattleEndedEvent BattleEnded;
        public event TurnEndedEvent TurnEnded;
        public event IntentChangedEvent IntentChanged;

        public void InvokeIntentChanged(object sender, IntentChangedEventArgs args)
        {
            IntentChanged?.Invoke(sender, args);
        }
        public void InvokeTurnEnded(object sender, TurnEndedEventArgs args)
        {
            TurnEnded?.Invoke(sender, args);
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

        public int RequestDamage(object sender, int baseDamage, IGameEntity target)
        {
            var args = new RequestDamageAmountEventArgs(target);
            args.AddModifier(new DamageAmountModifier { AdditiveModifier = baseDamage });
            RequestDamageAmount?.Invoke(sender, args);
            return args.GetResult();
        }
    }

    public delegate void BattleEndedEvent(object sender, BattleEndedEventArgs args);

    public class BattleEndedEventArgs
    {
        public bool IsVictory { get; }

        public BattleEndedEventArgs(bool isVictory)
        {
            IsVictory = isVictory;
        }
    }

    public delegate void ActorDiedEvent(object sender, ActorDiedEventArgs args);

    public class ActorDiedEventArgs
    {
        public ActorDiedEventArgs(Actor actor, IGameEntity causeOfDeath, IGameEntity causeOfDeathOwner)
        {
            Actor = actor;
            CauseOfDeath = causeOfDeath;
            CauseOfDeathOwner = causeOfDeathOwner;
        }

        public Actor Actor { get; }
        public IGameEntity CauseOfDeath { get; }
        public IGameEntity CauseOfDeathOwner { get; }
    }


    public delegate void RequestDamageAmountEvent(object sender, RequestDamageAmountEventArgs args);

    public class RequestDamageAmountEventArgs
    {
        public IGameEntity Target { get; }
        private List<DamageAmountModifier> Modifiers { get; } = new List<DamageAmountModifier>();


        public void AddModifier(DamageAmountModifier mod)
        {
            Modifiers.Add(mod);
        }

        public RequestDamageAmountEventArgs(IGameEntity target)
        {
            Target = target;
        }

        public int GetResult()
        {
            int total = 0;
            foreach (var mod in Modifiers)
            {
                total += mod.AdditiveModifier;
            }

            float totalPercentMod = 1;
            foreach (var mod in Modifiers)
            {
                totalPercentMod += mod.MultiplicativeModifier;
            }

            return (int)(total * totalPercentMod);
        }
    }

    public class DamageAmountModifier
    {
        public int AdditiveModifier { get; set; }
        public float MultiplicativeModifier { get; set; }
    }
}