using System.Collections.Generic;
using Api;

namespace CardsAndPiles
{
    public class CardPrefabPile : Pile
    {
        public Dictionary<string, IEntity> PrefabsByName { get; } = new Dictionary<string, IEntity>();

        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}
