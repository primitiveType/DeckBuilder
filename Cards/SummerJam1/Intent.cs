using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;
using SummerJam1.Units;

namespace SummerJam1
{
    public class DrawNewHandCard : SummerJam1Card
    {
        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            while (Game.Hand.Discard())
            {
                //do nothing.
            }
            
            for (int i = 0; i < 5; i++)
            {
                Game.Deck.DrawCard();
            }

            return true;
        }

        public override string Description { get; } = "Discard your hand and draw 5 cards.";
    } 
    public class HealUnit : SummerJam1Card
    {
        [JsonProperty] public int HealAmount { get; private set; }

        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            IHealable unit = target.GetComponentInChildren<IHealable>();

            if (unit == null)
            {
                return false;
            }


            unit.TryHeal(HealAmount, Entity);
            return true;
        }

        public override string Description { get; }
    }

    public class Draw2Cards : SummerJam1Card
    {
        private int NumToDraw { get; } = 2;
        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            for (int i = 0; i < NumToDraw; i++)
            {
                Game.Deck.DrawCard();
            }

            return true;
        }

        public override string Description { get; }
    } 
    public class DamageUnitCard : SummerJam1Card
    {
        [JsonProperty] public int DamageAmount { get; private set; }

        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(DamageAmount, Entity);
            return true;
        }

        [JsonIgnore] public override string Description => $"Deal {DamageAmount} damage to target Unit.";
    }

    public class EnemyUnitSlotConstraint : Component, IParentConstraint
    {
        public bool AcceptsParent(IEntity parent)
        {
            return parent.GetComponent<EnemyUnitSlot>() != null;
        }

        public bool AcceptsChild(IEntity child)
        {
            return false;
        }
    }

    public abstract class Intent : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            List<Intent> intents = Entity.GetComponents<Intent>();
            foreach (Intent intent in intents)
            {
                if (intent != this)
                {
                    Entity.RemoveComponent(intent);
                }
            }
        }

        [OnTurnEnded]
        private void OnTurnEnded()
        {
            DoIntent();
        }

        protected abstract void DoIntent();
    }

    public class DamageIntent : Intent, IAmount
    {
        [JsonIgnore] private int Amount => Entity.GetComponent<Strength>()?.Amount ?? 0;

        protected override void DoIntent()
        {
            var isFriendly = Entity.GetComponentInParent<FriendlyUnitSlot>() != null;


            UnitSlot targetSlot;
            if (isFriendly)
            {
                targetSlot = Context.Root.GetComponentsInChildren<EnemyUnitSlot>()
                    .FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() != null);
            }
            else
            {
                targetSlot = Context.Root.GetComponentsInChildren<FriendlyUnitSlot>()
                    .FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() != null);
            }

            if (targetSlot == null)
            {
                return;
            }

            Events.OnIntentStarted(new IntentStartedEventArgs(Entity));

            foreach (ITakesDamage componentsInChild in targetSlot.Entity.GetComponentsInChildren<ITakesDamage>())
            {
                componentsInChild.TryDealDamage(Amount, Entity);
            }
        }
    }

    public class Strength : SummerJam1Component
    {
        [JsonProperty] public int Amount { get; set; }
    }

    public interface IAmount
    {
    }
}