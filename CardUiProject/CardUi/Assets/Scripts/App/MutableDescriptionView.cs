using System.ComponentModel;
using CardsAndPiles.Components;
using TMPro;
using UnityEngine;

namespace App
{
    public class MutableDescriptionView : View<IDescription>
    {
        [SerializeField] private TMP_InputField m_Text;

        private void Awake()
        {
            m_Text.onValueChanged.AddListener(OnInputChanged);
        }

        private void OnInputChanged(string arg0)
        {
            // Model.Description = arg0;
        }

        [PropertyListener(nameof(IDescription.Description))]
        private void OnDescriptionChanged(object sender, PropertyChangedEventArgs args)
        {
            m_Text.text = Model.Description;
        }
    }
}
