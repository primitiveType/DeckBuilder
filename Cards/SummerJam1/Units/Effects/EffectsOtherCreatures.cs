using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public abstract class EffectsOtherCreatures : EnabledWhenAtTopOfEncounterSlot, IDescription
    {
        public abstract bool EveryTurn { get; }

        public abstract string Description { get; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            if (EveryTurn)
            {
                var neighbors = Game.Battle.EncounterSlots.Entity.Children;

                foreach (IEntity neighbor in neighbors)
                {
                    foreach (IEntity neighborChild in neighbor.Children)
                    {
                        ProcessAdjacentAdded(neighborChild);
                    }
                }
            }
        }


        protected abstract void ProcessAdjacentAdded(IEntity newItem);
        
      
    }
}
