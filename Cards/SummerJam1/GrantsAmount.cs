using Api;

namespace SummerJam1
{
    public abstract class GrantsAmount<T> : EffectsAdjacentCreatures, IAmount where T : Component, IAmount, new()
    {
        public override string Description
        {
            get
            {
                if (EveryTurn)
                {
                    return $"Grants {Amount} {Name} to adjacent units every turn.";
                }

                return $"Grants {Amount} {Name} to adjacent units.";
            }
        }

        public abstract string Name { get; }

        public int Amount { get; set; }

        protected override void ProcessAdjacentRemoved(IEntity eOldItem)
        {
            if (eOldItem.GetComponent<IMonster>() != null)
            {
                eOldItem.GetComponent<T>().Amount -= Amount;
            }
        }

        protected override void ProcessAdjacentAdded(IEntity newItem)
        {
            if (newItem.GetComponent<IMonster>() != null)
            {
                newItem.GetOrAddComponent<T>().Amount += Amount;
            }
        }
    }
}
