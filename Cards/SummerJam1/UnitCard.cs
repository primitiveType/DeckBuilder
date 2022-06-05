using System;
using Api;
using CardsAndPiles;
using Component = Api.Component;

namespace SummerJam1
{
    public class SummerJam1Component : Component
    {
        protected new SummerJam1Events Events => (SummerJam1Events)base.Events;
    }

    public abstract class Unit : SummerJam1Component, IPileItem
    {
        public virtual bool AcceptsParent(IEntity parent)
        {
            return parent.GetComponent<UnitSlot>() != null;
        }

        public virtual bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }


    public abstract class SummerJam1Card : Card, IDraggable
    {
        protected new SummerJam1Events Events => (SummerJam1Events)base.Events;

        public bool CanDrag { get; set; } = true;


        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.CardId.TrySetParent(Context.Root.GetComponent<SummerJam1Game>().Discard);
            }
        }
    }

    public abstract class UnitCard : SummerJam1Card
    {
        protected override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("Initialized card.");
        }


        protected override bool PlayCard(IEntity target)
        {
            FriendlyUnitSlot slot = target?.GetComponent<FriendlyUnitSlot>();
            if (slot != null)
            {
                if (Entity.TrySetParent(target))
                {
                    Unit unit = CreateUnit();
                    Events.OnUnitCreated(new UnitCreatedEventArgs(unit.Entity));
                    unit.Entity.TrySetParent(target);
                    return true;
                }
            }

            return false;
        }

        public override bool AcceptsParent(IEntity parent)
        {
            return base.AcceptsParent(parent) && parent.GetComponentInChildren<Unit>() == null;
        }

        protected abstract Unit CreateUnit();
    }

    public class StarterUnitCard : UnitCard
    {
        protected override Unit CreateUnit()
        {
            IEntity unitEntity = Context.CreateEntity(null, entity => entity.AddComponent<StarterUnit>());

            return unitEntity.GetComponent<StarterUnit>();
        }
    }

    public class StarterUnit : Unit
    {
    }
}