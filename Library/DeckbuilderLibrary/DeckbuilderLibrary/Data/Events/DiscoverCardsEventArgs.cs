using System;
using System.Collections.Generic;
using System.Text;

namespace DeckbuilderLibrary.Data.Events
{
    public class DiscoverCardsEventArgs
    {
        public int NumOptions;

        public PileType DestionationPile;
        public DiscoverCardsEventArgs(int numOptions, PileType destionationPile)
        {
            NumOptions = numOptions;
            DestionationPile = destionationPile;
     
        }
    }
}
