using System;

namespace Api
{
    [DontSerialize]
    public class SourcePrefab : Component
    {
        public string Prefab { get; set; }
    }
}