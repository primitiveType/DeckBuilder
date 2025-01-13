using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class MaterialInstancer : MonoBehaviour
    {
        private static readonly int Seed = Shader.PropertyToID("_Seed");
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
            Material.name += " (instance)";
            Debug.Log($"Created material {Material.GetInstanceID()} to replace {oldId}");
            Material.SetFloat(Seed, GetInstanceID() / 1000f);
        }

        private void OnDestroy()
        {
            Destroy(Material);
        }
    }
}