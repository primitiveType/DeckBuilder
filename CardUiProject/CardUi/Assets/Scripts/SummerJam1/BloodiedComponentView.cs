using SummerJam1.Statuses;
using UnityEngine;

namespace SummerJam1
{
    public class BloodiedComponentView : ShaderPropertyComponentView<Bloodied>
    {
        private readonly int _property = Shader.PropertyToID("_Bloodied");

        protected override int GetShaderProperty()
        {
            return _property;
        }
    }
}