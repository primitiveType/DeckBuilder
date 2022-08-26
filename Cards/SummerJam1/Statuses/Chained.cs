using Api;
using CardsAndPiles;

namespace SummerJam1.Statuses
{
    public class Chained : SummerJam1Component //todo
    {
        public IEntity ChainedTo { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
        }

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            if (args.DrawnCard == Entity || args.DrawnCard == ChainedTo)
            {
            }
        }
    }
}
