using UnityEngine;

namespace App.Utility
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Recursively sets GameObject's layer to newLayer
        /// </summary>
        /// <param name="newLayer">The new layer</param>
        /// <param name="trans">Start transform</param>
        public static void SetLayerRecursively(this Transform trans, int newLayer)
        {
            trans.gameObject.layer = newLayer;
            foreach (Transform child in trans)
            {
                child.gameObject.layer = newLayer;
                if (child.childCount > 0)
                {
                    SetLayerRecursively(child.transform, newLayer);
                }
            }
        }
    }
}
