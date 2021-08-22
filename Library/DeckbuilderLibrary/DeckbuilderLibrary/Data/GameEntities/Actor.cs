using System;
using System.Runtime.InteropServices;
using DeckbuilderLibrary.Data;
using Newtonsoft.Json;

namespace Data
{
    //similar to a card. Triggers on turn end.
    public abstract class Intent : GameEntity
    {
        public abstract string GetDescription { get; }
        public int OwnerId { get; internal set; } = -1;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.TurnEnded += EventsOnTurnEnded;
            Context.Events.IntentChanged += EventsOnIntentChanged;
        }

        private void EventsOnIntentChanged(object sender, IntentChangedEventArgs args)
        {
            if (args.Owner.Id == OwnerId && args.Owner.Intent != this)
            {
                Context.Events.TurnEnded -= EventsOnTurnEnded;
            }
        }

        private void EventsOnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Trigger();
        }

        protected abstract void Trigger();
    }

    public class DamageIntent : Intent
    {
        public int DamageAmount { get; set; }

        public override string GetDescription =>
            Context.GetDamageAmount(this, DamageAmount, Target, Context.GetActorById(OwnerId)).ToString();

        private IActor Target => Context.GetCurrentBattle().Player;

        protected override void Trigger()
        {
            var battle = Context.GetCurrentBattle();
            if (OwnerId == battle.Player.Id)
            {
                throw new NotImplementedException("Player intents not implemented.");
            }

            if (OwnerId == -1)
            {
                throw new NotSupportedException("Intent with no owner was triggered!");
            }

            Context.TryDealDamage(this, Context.GetActorById(OwnerId), Target, DamageAmount);
        }
    }

    public abstract class Enemy : Actor
    {
        public Intent Intent { get; private set; }

        public void SetIntent(Intent intent)
        {
            Intent = intent;
            Context.Events.InvokeIntentChanged(this, new IntentChangedEventArgs(this));
        }
    }

    public class BasicEnemy : Enemy
    {
        public int Strength => 5;

        protected override void Initialize()
        {
            base.Initialize();
            if (Intent == null)
            {
                var intent = Context.CreateIntent<DamageIntent>(this);
                intent.DamageAmount = Strength;
                SetIntent(intent);
            }
        }
    }

    public abstract class Actor : GameEntity, IInternalActor
    {
        public int Health { get; internal set; }

        public int Armor { get; internal set; }


        void IInternalActor.TryDealDamage(GameEntity source, int damage, out int totalDamage, out int healthDamage)
        {
            int armorDamage = System.Math.Min(damage, Armor);
            Armor -= armorDamage;
            healthDamage = damage - armorDamage;
            healthDamage = System.Math.Min(healthDamage, Health);
            totalDamage = healthDamage + armorDamage;

            Health -= healthDamage;
            //do we want to clamp the output of damage dealt? add overkill damage as a param?
            ((IInternalGameEventHandler)Context.Events).InvokeDamageDealt(this,
                new DamageDealtArgs(Id, totalDamage, healthDamage, source));

            if (Health <= 0)
            {
                ((IInternalGameEventHandler)Context.Events).InvokeActorDied(this,
                    new ActorDiedEventArgs(this, source, source /*TODO*/));
            }
        }

        public void GainArmor(int amount)
        {
            Armor += amount;
        }

        public void ResetArmor()
        {
            Armor = 0;
        }
    }

    public class PlayerActor : Actor
    {
    }

    public interface IActor : IGameEntity
    {
        int Health { get; }
        int Armor { get; }
    }

    internal interface IInternalActor : IActor
    {
        void TryDealDamage(GameEntity source, int damage, out int totalDamage, out int healthDamage);
    }
}