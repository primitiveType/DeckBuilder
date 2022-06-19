using System.ComponentModel;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class GameView : View<SummerJam1Game>
    {
        [SerializeField] private GameObject m_ObjectiveViewPrefab;
        [SerializeField] private Transform m_ObjectiveViewParent;
        protected override void Start()
        {
            base.Start();
            SetModel(SummerJam1Context.Instance.Context.Root);
            Model.PropertyChanged += ModelOnPropertyChanged;
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SummerJam1Game.Battle))
            {
                if (Model.Battle != null)
                {
                    Instantiate(m_ObjectiveViewPrefab, m_ObjectiveViewParent);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Model.PropertyChanged -= ModelOnPropertyChanged;
        }
    }
}