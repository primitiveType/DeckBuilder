using SummerJam1.Statuses;
using UnityEngine;

namespace SummerJam1
{
    public class FrozenComponentView : ShaderPropertyComponentView<Frozen>
    {
        private readonly int _property = Shader.PropertyToID("_Frozen");

        protected override int GetShaderProperty()
        {
            return _property;
        }
    }
}