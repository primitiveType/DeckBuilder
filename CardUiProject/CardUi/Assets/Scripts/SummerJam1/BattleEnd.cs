using System;
using App;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class BattleEnd : View<Game>
    {
        [SerializeField] private TMP_Text Text;

        protected override void Start()
        {
            base.Start();
            SetModel(SummerJam1Context.Instance.Game.Entity);
            Disposables.Add(SummerJam1Context.Instance.Events.SubscribeToGameEnded(OnGameEnd));
            Disposables.Add(SummerJam1Context.Instance.Events.SubscribeToGameStarted(OnGameStart));
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
                }
                else
                {
                    Text.text = "Game over... Try again?";
                }
            });
        }
    }
}
