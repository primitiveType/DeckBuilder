﻿using System;

namespace RogueMaps.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AspChildControlTypeAttribute : Attribute
    {
        public AspChildControlTypeAttribute([NotNull] string tagName, [NotNull] Type controlType)
        {
            TagName = tagName;
            ControlType = controlType;
        }

        [NotNull] public string TagName { get; }

        [NotNull] public Type ControlType { get; }
    }
}
