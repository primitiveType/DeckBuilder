/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using UnityEditor;
using UnityEngine;

namespace E7.Introloop.Editor
{
    [CustomPropertyDrawer(typeof(PositiveFloatAttribute))]
    internal class PositiveFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            PositiveFloatAttribute attr = (PositiveFloatAttribute)attribute;
            string modifiedLabel = label.text;
            if (attr.unit != null)
            {
                modifiedLabel += " (" + attr.unit + ")";
            }


            EditorGUI.BeginChangeCheck();
            float val = EditorGUI.FloatField(position, modifiedLabel, prop.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                if (val < 0)
                    val = 0;
                prop.floatValue = val;
            }
        }
    }
}