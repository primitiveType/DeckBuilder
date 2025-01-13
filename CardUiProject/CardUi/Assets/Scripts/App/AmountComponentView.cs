using System.ComponentModel;
using System.Threading.Tasks;
using Api;
using UnityEngine;
using IComponent = Api.IComponent;

namespace App
{
    public class AmountComponentView<T> : ComponentView<T> where T : IAmount, INotifyPropertyChanged, IComponent
    {
        [SerializeField] private bool m_HideIfZero = true;
        [SerializeField] private TMPro.TMP_Text _text;

        protected override void ComponentOnPropertyChanged()
        {
            int? amount = Component?.Amount;
            Disposables.Add(AnimationQueue.Instance.Enqueue((() => ValueChanged(amount))));
        }

        protected virtual Task ValueChanged(int? amount)
        {
            bool hide = (m_HideIfNull && amount == null) || (m_HideIfZero && amount is 0);
            if (m_HideIfNull || m_HideIfZero)
            {
                VisibilityObject.SetActive(!hide);
            }

            _text.text = GetStringForAmount(amount);
            return Task.CompletedTask;
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
    // public class AmountView<T> : View<T> where T : IAmount, INotifyPropertyChanged, IComponent
    // {
    //     [SerializeField] private bool m_HideIfZero = true;
    //     [SerializeField] private TMPro.TMP_Text _text;
    //     [SerializeField] private bool m_HideIfNull;
    //
    //     [PropertyListener]
    //     protected void ComponentOnPropertyChanged()
    //     {
    //         int? amount = Model?.Amount;
    //         Disposables.Add(AnimationQueue.Instance.Enqueue((() => ValueChanged(amount))));
    //     }
    //
    //     protected virtual Task ValueChanged(int? amount)
    //     {
    //         bool hide = (m_HideIfNull && amount == null) || (m_HideIfZero && amount is 0);
    //         if (m_HideIfNull || m_HideIfZero)
    //         {
    //             VisibilityObject.SetActive(!hide);
    //         }
    //
    //         _text.text = GetStringForAmount(amount);
    //         return Task.CompletedTask;
    //     }
    //
    //     protected virtual string GetStringForAmount(int? amount)
    //     {
    //         if (amount == null)
    //         {
    //             return "";
    //         }
    //
    //         return amount.Value.ToString();
    //     }
    // }

}
