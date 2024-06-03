/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using UnityEngine;

namespace E7.Introloop
{
    internal class PositiveFloatAttribute : PropertyAttribute
    {
        public readonly string unit;

        public PositiveFloatAttribute()
        {
        }

        public PositiveFloatAttribute(string unit)
        {
            this.unit = unit;
        }
    }
}