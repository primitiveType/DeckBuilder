using System.ComponentModel;
using Api;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class ObjectivePileView : PileView
    {
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private Transform m_Parent;
        protected override void Start()
        {
            base.Start();
            Setup();
            SummerJam1Context.Instance.Game.PropertyChanged += GameOnPropertyChanged;
        }

        private void GameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.Battle))
            {
                Setup();
            }
        }

        private void Setup()
        {
            if (SummerJam1Context.Instance.Game.Battle?.ObjectivesPile?.Entity != null)
            {
                SetModel(SummerJam1Context.Instance.Game.Battle.ObjectivesPile.Entity);
                foreach (IEntity entityChild in Model.Entity.Children)
                {
                    var go = Instantiate(m_Prefab, m_Parent);
                    go.GetComponent<ISetModel>().SetModel(entityChild);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SummerJam1Context.Instance.Game.PropertyChanged -= GameOnPropertyChanged;
        }
    }
}
