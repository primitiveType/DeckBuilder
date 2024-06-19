using System;

namespace Api
{
    [NonSerializableComponent]
    public class SourcePrefab : Component
    {
        public string Prefab { get; set; }
    }
}