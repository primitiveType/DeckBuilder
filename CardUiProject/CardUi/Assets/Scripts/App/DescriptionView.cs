using System.ComponentModel;
using CardsAndPiles.Components;
using TMPro;
using UnityEngine;

namespace App
{
    public class DescriptionView : View<IDescription>
    {
        [SerializeField] private TMP_Text m_Text;
        
        [PropertyListener(nameof(IDescription.Description))]
        private void OnDescriptionChanged(object sender, PropertyChangedEventArgs args)
        {
            m_Text.text = Model.Description;
        }
    }
}