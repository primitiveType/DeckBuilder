﻿using Api;

namespace SummerJam1
{
    public class PrefabReference : SummerJam1Component, IVisual
    {
        public string Prefab { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
