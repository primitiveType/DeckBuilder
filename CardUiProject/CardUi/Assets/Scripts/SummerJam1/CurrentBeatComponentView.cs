using System;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class CurrentBeatComponentView : ComponentView<BeatTracker>
    {
        [SerializeField] private TMPro.TMP_Text m_Text;

        protected override void ComponentOnPropertyChanged()
        {
            m_Text.text = $"{Component.CurrentBeat+1}/{Component.MaxBeatsToThreshold}";
        }
    }
}
