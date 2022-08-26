using System.ComponentModel;
using App;
using CardsAndPiles;

namespace SummerJam1
{
    public class VisibleDuringPlayerTurnOnCurrentFloor : View<EncounterSlotPile>
    {
        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(false);
            GameContext.Instance.Game.Battle.PropertyChanged += BattleOnPropertyChanged;
            Disposables.Add(GameContext.Instance.Events.SubscribeToTurnBegan(OnTurnBegan));
        }

        private void BattleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BattleContainer.CurrentFloor))
            {
                gameObject.SetActive(!IsCurrentFloor);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameContext.Instance.Game.Battle.PropertyChanged -= BattleOnPropertyChanged;
        }

        [OnDungeonPhaseStarted]
        private void OnDungeonPhaseStarted()
        {
            gameObject.SetActive(false);
        }

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs item)
        {
            gameObject.SetActive(!IsCurrentFloor);
        }


        private bool IsCurrentFloor => GameContext.Instance.Game.Battle.EncounterSlots.Contains(Model);
    }
}
