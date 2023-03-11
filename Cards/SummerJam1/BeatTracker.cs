using System.ComponentModel;
using Api;
using CardsAndPiles;
using JetBrains.Annotations;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class MockBeatTracker : IBeatTracker
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int CurrentBeat { get; set; }
        public int MaxBeatsToThreshold { get; set; }
    }
    public interface IBeatTracker : INotifyPropertyChanged
    {
        int CurrentBeat { get; }
        int MaxBeatsToThreshold { get; }
    }

    [PublicAPI]
    public class BeatTracker : SummerJam1Component, IBeatTracker
    {
        public int CurrentBeat { get; private set; }
        public int MaxBeatsToThreshold { get; private set; } = 10;
        

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            BeatCost BeatCost = args.CardId.GetComponent<BeatCost>();
            int previousBeat = CurrentBeat;
            bool didOverload = previousBeat + BeatCost.Amount >= MaxBeatsToThreshold;
            CurrentBeat = (previousBeat + BeatCost.Amount) % MaxBeatsToThreshold;
            int overload = previousBeat + BeatCost.Amount - MaxBeatsToThreshold;

            Events.OnBeatMoved(new BeatMovedEventArgs(previousBeat, CurrentBeat, didOverload, overload));
        }

        public void RegisterIntent(Intent intent)
        {
            intent.PropertyChanged += IntentOnPropertyChanged;
        }

        private void IntentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Intent intent = ((Intent)sender);
            if (e.PropertyName == nameof(State) && intent.State == LifecycleState.Destroyed)
            {
                intent.PropertyChanged -= IntentOnPropertyChanged;
                return;
            }

            if (e.PropertyName == nameof(intent.TargetBeat))
            {
                //doing it this way probably doesn't make sense.
                //instead, just have the intent itself keep track of where it should be.
            }
        }
    }
}
