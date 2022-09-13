using System.Linq;
using Api;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class PrefabReferenceView : View<PrefabReference>, ISetModel
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            IEntity prefabEntity = GetPrefabEntity(Model.Prefab);

            GameObject prefabGo = Instantiate(ViewFactory.Instance.GetPrefab(prefabEntity.GetComponent<IVisual>()), transform);
            
            prefabGo.GetComponent<ISetModel>().SetModel(prefabEntity);
        }

        private IEntity GetPrefabEntity(string modelPrefab)
        {
            return GameContext.Instance.Game.PrefabsContainer.Children.Where(entity =>
            {
                SourcePrefab source = entity.GetComponent<SourcePrefab>();
                return source != null && source.Prefab == Model.Prefab;
            }).FirstOrDefault();
        }
    }
}
