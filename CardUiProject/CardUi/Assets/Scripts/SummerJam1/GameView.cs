using System.ComponentModel;
using Api;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class GameView : View<Game>
    {
        [SerializeField] private GameObject m_ObjectiveViewPrefab;
        [SerializeField] private Transform m_ObjectiveViewParent;

        protected void Awake()
        {
            SetModel(GameContext.Instance.Context.Root);
            Model.PropertyChanged += ModelOnPropertyChanged;
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.Battle))
            {
                if (Model.Battle != null && m_ObjectiveViewPrefab != null)
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
            Logging.Log("Game view destroyed!");
        }
    }
}
