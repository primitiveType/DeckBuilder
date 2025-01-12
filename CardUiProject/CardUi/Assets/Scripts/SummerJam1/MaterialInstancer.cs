using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class MaterialInstancer : MonoBehaviour
    {
        public Material Material { get; private set; }
        private void Awake()
        {
            InstanceMaterial();
        }

        private void InstanceMaterial()
        {
            var graphic = GetComponent<Graphic>();
            var oldId = graphic.material.GetInstanceID();
            graphic.material = new Material(graphic.material);
            Material = graphic.material;
            Debug.Log($"Created material {Material.GetInstanceID()} to replace {oldId}");
        }

        private void OnDestroy()
        {
            Destroy(Material);
        }
    }
}