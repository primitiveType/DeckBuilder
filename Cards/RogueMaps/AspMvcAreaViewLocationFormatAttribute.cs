﻿using System;

namespace RogueMaps.Annotations
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class AspMvcAreaViewLocationFormatAttribute : Attribute
    {
        public AspMvcAreaViewLocationFormatAttribute([NotNull] string format)
        {
            Format = format;
        }

        [NotNull] public string Format { get; }
    }
}
