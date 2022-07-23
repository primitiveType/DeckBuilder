using System.ComponentModel;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class GameView : View<Game>
    {
        [SerializeField] private GameObject m_ObjectiveViewPrefab;
        [SerializeField] private Transform m_ObjectiveViewParent;

        protected override void Start()
        {
            base.Start();
            SetModel(GameContext.Instance.Context.Root);
            Model.PropertyChanged += ModelOnPropertyChanged;
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.Battle))
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
            if (Model != null)
            {
                Model.PropertyChanged -= ModelOnPropertyChanged;
            }
            Debug.Log("Game view destroyed!");
        }
    }
}
