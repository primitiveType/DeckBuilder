using System;
using System.ComponentModel;
using App;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class RoomsCleared : View<GameEndsAfter10Rooms>
    {
        private void Awake()
        {
            SetModel(SummerJam1Context.Instance.Context.Root.GetComponentInChildren<GameEndsAfter10Rooms>().Entity);
        }

        [SerializeField] private TMP_Text m_Text;

        [PropertyListener(nameof(GameEndsAfter10Rooms.RoomsCleared))]
        private void OnDescriptionChanged(object sender, PropertyChangedEventArgs args)
        {
            m_Text.text = $"{Model.RoomsCleared} / {10}";
        }
    }
}
