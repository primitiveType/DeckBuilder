using System;
using System.Linq;
using Api;
using Random = Api.Random;

namespace CardsAndPiles
{
    public abstract class Pile : Component, IPile, IParentConstraint

    {
        public virtual bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public abstract bool AcceptsChild(IEntity child);

        public IEntity GetRandom()
        {
            if (Entity.Children.Count == 0)
            {
                return null;
            }

            int index = Context.Root.GetComponent<Random>().SystemRandom.Next(0, Entity.Children.Count);
            IEntity card = Entity.Children.ElementAt(index);
            return card;
        }

        public IEntity GetRandomWithCondition(Func<IEntity, int, bool> condition)
        {
            if (Entity.Children.Count == 0)
            {
                return null;
            }

            var matchingChildren = Entity.Children.Where(condition).ToList();
            IEntity card = null;
            if (matchingChildren.Count > 0)
            {
                int index = Context.Root.GetComponent<Random>().SystemRandom.Next(0, matchingChildren.Count);
                card = matchingChildren.ElementAt(index);
            }

            return card;
        }
    }
}
