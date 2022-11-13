using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public abstract class DungeonPile : SummerJam1Component, IVisual, IPile
    {
        public abstract string Type { get; }
        public abstract string Description { get; }
        public virtual int RequiredCards => 15;

        protected virtual int NumBoosters => 3;

        public int Difficulty { get; set; }

        protected virtual int GetMinCards(int difficulty)
        {
            return 10 + difficulty;
        }

        protected virtual int GetMaxCards(int difficulty)
        {
            return 15 + difficulty;
        }

        protected override void Initialize()
        {
            base.Initialize();

       
           
        }
    }
}
