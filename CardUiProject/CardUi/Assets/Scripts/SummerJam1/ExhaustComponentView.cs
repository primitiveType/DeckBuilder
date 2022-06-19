using SummerJam1.Cards;
using TMPro;
using UnityEngine;

namespace App
{
    public class ExhaustComponentView : ComponentView<Exhaust>
    {
        protected override void ComponentOnPropertyChanged()
        {
            bool enable = Component != null;
            Disposables.Add(AnimationQueue.Instance.Enqueue(( ()=>Enable(enable))));
        }

        private void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}
