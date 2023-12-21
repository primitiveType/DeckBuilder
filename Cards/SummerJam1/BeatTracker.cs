using System;
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
        public int CurrentBeatPreview { get; set; }
        public int MaxBeatsToThreshold { get; set; }
    }

    public interface IBeatTracker : INotifyPropertyChanged
    {
        int CurrentBeat { get; }
        int CurrentBeatPreview { get; }
        int MaxBeatsToThreshold { get; }
    }

    [PublicAPI]
    public class BeatTracker : SummerJam1Component, IBeatTracker
    {
        public int CurrentBeat { get; private set; }
        public int CurrentBeatPreview { get; private set; }
        public int MaxBeatsToThreshold { get; private set; } = 10;
        
        public void AdvanceBeats(int BeatCost)
        {
            int previousBeat = CurrentBeat;
            bool didOverload = previousBeat + BeatCost >= MaxBeatsToThreshold;
            var currentBeat = (previousBeat + BeatCost) % MaxBeatsToThreshold;
            int overload = previousBeat + BeatCost - MaxBeatsToThreshold;

            if (didOverload)
            {
                CurrentBeat = 0;
            }
            else
            {
                CurrentBeat = currentBeat;
            }

            Events.OnBeatMoved(new BeatMovedEventArgs(previousBeat, CurrentBeat, didOverload, overload));
            Events.OnAfterBeatMoved(new AfterBeatMovedEventArgs(previousBeat, CurrentBeat, didOverload, overload));
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
        
        public IDisposable GetPreview(int beatCost)
        {
            CurrentBeatPreview += beatCost;
            return new PreviewHandle(beatCost, this);
        }

        private void DisposePreview(PreviewHandle handle)
        {
            CurrentBeatPreview -= handle.BeatCost;
        }


        private class PreviewHandle : IDisposable
        {
            public BeatTracker Model { get; }
            public int BeatCost { get; }

            public PreviewHandle(int beatCost, BeatTracker model)
            {
                Model = model;
                BeatCost = beatCost;
            }

            public void Dispose()
            {
                // TODO release managed resources here
                Model.DisposePreview(this);
            }
        }
    }
}
