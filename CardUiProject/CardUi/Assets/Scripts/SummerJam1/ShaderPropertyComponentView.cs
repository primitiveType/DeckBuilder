using Api;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public abstract class ShaderPropertyComponentView<T> : ComponentView<T> where T : IComponent
    {
        protected abstract int GetShaderProperty();
        [SerializeField] private MaterialInstancer materialRenderer;
        protected override bool m_DisableComponentIfNull => true;

        protected override void ComponentOnPropertyChanged()
        {
        }

        private void OnEnable()
        {
            Debug.Log($"Setting frozen on {materialRenderer.Material.GetInstanceID()}");
            materialRenderer.Material.SetFloat(GetShaderProperty(), 1.0f);
        }

        private void OnDisable()
        {
            materialRenderer.Material.SetFloat(GetShaderProperty(), 0.0f);
        }
    }
}