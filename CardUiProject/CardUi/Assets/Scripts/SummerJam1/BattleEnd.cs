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
            Disposables.Add(GameContext.Instance.Events.SubscribeToBattleEnded(OnBattleEnd));
            gameObject.SetActive(false);
        }
        
        private void OnBattleEnd(object sender, BattleEndedEventArgs item)
        {
            AnimationQueue.Instance.Enqueue(() =>
            {
                gameObject.SetActive(true);
                if (item.Victory)
                {
                    Text.text = "Victory!";
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