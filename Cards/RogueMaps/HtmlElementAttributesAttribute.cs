﻿using System;

namespace RogueMaps.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class HtmlElementAttributesAttribute : Attribute
    {
        public HtmlElementAttributesAttribute()
        {
        }

        public HtmlElementAttributesAttribute([NotNull] string name)
        {
            Name = name;
        }

        [CanBeNull] public string Name { get; }
    }
}
