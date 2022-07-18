using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Api;
using UnityEngine;

namespace App
{
    public class AmountComponentView<T> : ComponentView<T> where T : IAmount, INotifyPropertyChanged
    {
        [SerializeField] private bool m_HideIfZero = true;
        [SerializeField] private bool m_HideIfNull = true;
        [SerializeField] private GameObject m_VisibilityObject;
        [SerializeField] private TMPro.TMP_Text _text;

        protected override void ComponentOnPropertyChanged()
        {
            int? amount = Component?.Amount;
            Disposables.Add(AnimationQueue.Instance.Enqueue((() => ValueChanged(amount))));
        }

        protected virtual async Task ValueChanged(int? amount)
        {
            bool hide = (m_HideIfNull && amount == null) || (m_HideIfZero && amount is 0);
            m_VisibilityObject.SetActive(!hide);

            if (amount == null)
            {
                _text.text = "";
                return;
            }


            _text.text = amount.Value.ToString();
        }
    }
}
