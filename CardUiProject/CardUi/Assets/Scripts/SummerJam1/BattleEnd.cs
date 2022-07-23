using System;
using App;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class BattleEnd : View<Game>
    {
        [SerializeField] private TMP_Text Text;
        [SerializeField] private GameObject Defeat;
        [SerializeField] private GameObject Victory;

        protected override void Start()
        {
            base.Start();
            SetModel(GameContext.Instance.Game.Entity);
            Disposables.Add(GameContext.Instance.Events.SubscribeToGameEnded(OnGameEnd));
            Disposables.Add(GameContext.Instance.Events.SubscribeToGameStarted(OnGameStart));
            gameObject.SetActive(false);
        }

        private void OnGameStart(object sender, GameStartedEventArgs item)
        {
            gameObject.SetActive(false);
        }

        private void OnGameEnd(object sender, GameEndedEventArgs item)
        {
            AnimationQueue.Instance.Enqueue(() =>
            {
                gameObject.SetActive(true);
                if (item.Victory)
                {
                    Text.text = "You Win! Play again?";
                    Victory.SetActive(true);
                    Defeat.SetActive(false);
                }
                else
                {
                    Text.text = "Game over... Try again?";
                    Victory.SetActive(false);
                    Defeat.SetActive(true);
                }
            });
        }
    }
}
