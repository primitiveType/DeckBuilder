using Api;
using SummerJam1;

namespace App
{
    public class PrefabReferencePileOrganizer : FactoryPileOrganizer<PrefabReference>
    {
        protected override void OnItemAddedImmediate(IEntity added, IGameObject view)
        {
            //TODO: find a way to show prefabs well.
            PrefabReference component = added.GetComponent<PrefabReference>();
            if (component == null)
            {
                return;
            }
            
            ISetModel entityView = Instantiate(Prefab, Parent).GetComponentInChildren<ISetModel>();
            entityView.SetModel(component);
        }
    }
}
