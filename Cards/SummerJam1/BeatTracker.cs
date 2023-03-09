using Api;
using CardsAndPiles;
using JetBrains.Annotations;
using SummerJam1.Cards;

namespace SummerJam1
{
    [PublicAPI]
    public class BeatTracker : SummerJam1Component
    {
        public int CurrentBeat { get; private set; }
        public int MaxBeatsToThreshold { get; private set; } = 10;

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            BeatCost BeatCost = args.CardId.GetComponent<BeatCost>();
            int previousBeat = CurrentBeat;
            CurrentBeat = (previousBeat + BeatCost.Amount) % MaxBeatsToThreshold;
            int overload = previousBeat - MaxBeatsToThreshold;

            if (overload > 0)
            {
                Events.OnBeatOverloaded(new BeatOverloadedEventArgs(overload));
            }
        }
    }
}
