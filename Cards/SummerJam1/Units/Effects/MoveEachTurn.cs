using Api;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public class MoveEachTurn : EnabledWhenAtTopOfEncounterSlot, IDescription
    {
        protected override void Initialize()
        {
            Logging.Log("Init move each turn.");
            base.Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateComponents();
        }


        [OnAttackPhaseEnded]
        private void OnAttackPhaseEnded(object sender, AttackPhaseEndedEventArgs args)
        {
            Logging.Log("Updating Move components.");
            UpdateComponents();
        }


        private void UpdateComponents()
        {
            if (Game.Battle == null)
            {
                return;
            }
            if (Entity.HasComponent<MoveRightEachTurn>())
            {
                CheckIfLeftComponentNeeded();
            }
            else if (Entity.HasComponent<MoveLeftEachTurn>())
            {
                CheckIfRightComponentNeeded();
            }
            else
            {
                Logging.Log("Adding move right because none was found.");

                Entity.AddComponent<MoveRightEachTurn>();
                UpdateComponents();
            }
        }

        private void CheckIfRightComponentNeeded()
        {
            IEntity leftSlot = Game.Battle.GetSlotToLeft(Entity);

            if (leftSlot == null)
            {
                Logging.Log("Adding move right.");

                Entity.RemoveComponent<MoveLeftEachTurn>();
                Entity.AddComponent<MoveRightEachTurn>();
            }
        }

        private void CheckIfLeftComponentNeeded()
        {
            IEntity rightSlot = Game.Battle.GetSlotToRight(Entity);

            if (rightSlot == null)
            {
                Logging.Log("Adding move left.");

                Entity.RemoveComponent<MoveRightEachTurn>();
                Entity.AddComponent<MoveLeftEachTurn>();
            }
        }

        public string Description => "Moves each turn.";
    }
}
