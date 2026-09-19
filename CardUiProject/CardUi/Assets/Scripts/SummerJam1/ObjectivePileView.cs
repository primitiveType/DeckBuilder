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
            // GameContext.Instance.Game.PropertyChanged += GameOnPropertyChanged;
        }

        // private void GameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     if (e.PropertyName == nameof(Game.Battle))
        //     {
        //         Setup();
        //     }
        // }

        protected override IEntity GetEntityForView()
        {
            var entity = GameContext.Instance.Game.Battle.ObjectivesPile.Entity;
            foreach (IEntity entityChild in entity.Children)
            {
                var go = Instantiate(m_Prefab, m_Parent);
                go.GetComponent<ISetModel>().SetModel(entityChild);
            }

            return entity;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // GameContext.Instance.Game.PropertyChanged -= GameOnPropertyChanged;
        }
    }
}