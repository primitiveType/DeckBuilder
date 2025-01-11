using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class MaterialInstancer : MonoBehaviour
    {
        private Material _createdMaterial;
        private void Awake()
        {
            var graphic = GetComponent<Graphic>();
            var oldId = graphic.material.GetInstanceID();
            graphic.material = new Material(graphic.material);
            _createdMaterial = graphic.material;
            Debug.Log($"Created material {_createdMaterial.GetInstanceID()} to replace {oldId}");
        }

        private void OnDestroy()
        {
            Destroy(_createdMaterial);
        }
    }
}