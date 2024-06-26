using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Api;
using SummerJam1.Units;
using UnityEngine;

namespace App
{
    public class AmountComponentView<T> : ComponentView<T> where T : IAmount, INotifyPropertyChanged
    {
        [SerializeField] private bool m_HideIfZero = true;
        [SerializeField] private TMPro.TMP_Text _text;

        protected override void ComponentOnPropertyChanged()
        {
            int? amount = Component?.Amount;
            Disposables.Add(AnimationQueue.Instance.Enqueue((() => ValueChanged(amount))));
        }

        protected virtual async Task ValueChanged(int? amount)
        {
            bool hide = (m_HideIfNull && amount == null) || (m_HideIfZero && amount is 0);
            VisibilityObject.SetActive(!hide);

            _text.text = GetStringForAmount(amount);
        }

        protected virtual string GetStringForAmount(int? amount)
        {
            if (amount == null)
            {
                return "";
            }

            return amount.Value.ToString();
        }
    }
}
