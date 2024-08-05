using Api;
using SummerJam1;
using UnityEditor;
using UnityEngine;

namespace App
{
    [CustomEditor(typeof(EntityView))]
    [CanEditMultipleObjects]
    public class ViewEditor : UnityEditor.Editor 
    {
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Print entity"))
            {
                var debugString = Serializer.Serialize(((IView)serializedObject.targetObject).Entity);
                Debug.Log(debugString);
            }
        }
    }
}