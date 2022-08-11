using System;
using System.Collections.Generic;
using App;
using App.Utility;
using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class AudioHandler : MonoBehaviourSingleton<AudioHandler>
    {
        [SerializeField] private AudioSource HitAudio;
        [SerializeField] private AudioSource CardAudio;
        [SerializeField] private AudioSource ButtonAudio;
        private List<IDisposable> Disposables = new List<IDisposable>();

        private void OnCardPlayed(object sender, CardPlayedEventArgs item)
        {
            AnimationQueue.Instance.Enqueue(() => CardAudio.PlayOneShot(CardAudio.clip));
        }

        private void OnDamageDealt(object sender, DamageDealtEventArgs item)
        {
            AnimationQueue.Instance.Enqueue(() => HitAudio.PlayOneShot(HitAudio.clip));
        }

        public void ButtonClicked()
        {
            if (ButtonAudio)
            {
                ButtonAudio.PlayOneShot(ButtonAudio.clip);
            }
        }

        protected override void SingletonAwakened()
        {
            base.SingletonAwakened();

            Disposables.Add(GameContext.Instance.Events.SubscribeToDamageDealt(OnDamageDealt));
            Disposables.Add(GameContext.Instance.Events.SubscribeToCardPlayed(OnCardPlayed));
        }
    }
}
