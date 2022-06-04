using Api;
using CardsAndPiles;
using Component = Api.Component;

namespace SummerJam1
{
    public class SummerJam1Component : Component
    {
        protected SummerJam1Events Events { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Events = Entity.GetComponentInParent<SummerJam1Events>();
        }
    }

    public class Unit : SummerJam1Component, IPileItem
    {
        public bool TrySendToPile(IPile pile)
        {
            return pile is UnitSlot;
        }
    }

    public abstract class SummerJam1Card : Card
    {
        protected SummerJam1Events Events { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Events = Entity.GetComponentInParent<SummerJam1Events>();
        }
    }

    public class UnitCard : SummerJam1Card
    {
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId.Id == Entity.Id)
            {
            }
        }

        protected override bool PlayCard(IEntity target)
        {
            var slot = target.GetComponent<UnitSlot>();
            if (slot != null)
            {
                if (Entity.TrySetParent(target))
                {
                    IEntity unit = Context.CreateEntity();
                    unit.AddComponent<Unit>();
                    Events.OnUnitCreated(new UnitCreatedEventArgs(unit));
                    unit.TrySetParent(target);
                    return true;
                }
            }

            return false;
        }

     
    }
}