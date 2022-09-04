using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class DungeonViewManager : TempMonoBehaviourSingleton<DungeonViewManager>
    {
        [SerializeField] private Transform DungeonViewParent;
        [SerializeField] private DungeonPreviewView DungeonViewPrefab;

        public void ViewDungeon(DungeonPile pile)
        {
            foreach (Transform child in DungeonViewParent)
            {
                Destroy(child.gameObject);
            }
            var view = Instantiate(DungeonViewPrefab, DungeonViewParent);
            view.SetModel(pile);
        }
    }
}
