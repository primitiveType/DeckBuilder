using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;

namespace CardsAndPiles
{
    public class CardPrefabPile : Pile
    {
        public Dictionary<string, IEntity> PrefabsByName { get; } = new Dictionary<string, IEntity>();

        protected override void Initialize()
        {
            base.Initialize();
   
        }

        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}
