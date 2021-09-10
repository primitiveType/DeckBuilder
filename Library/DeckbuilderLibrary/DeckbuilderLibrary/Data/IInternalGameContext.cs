using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data
{
    internal interface IInternalGameContext
    {
        void ToInitializeAdd(IInternalInitialize toInit);
    }
}