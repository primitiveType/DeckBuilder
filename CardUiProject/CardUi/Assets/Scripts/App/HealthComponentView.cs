using System.ComponentModel;
using Api.Components;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class HealthComponentView : ComponentView<Health>
    {
        [SerializeField] private SimpleHealthBar HealthText;

        protected override void ComponentOnPropertyChanged()
        {
            HealthText.UpdateBar(Component.Amount, Component.Max);
        }
    }
}