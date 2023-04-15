using UnityEditor;
using UnityEngine;

namespace App.Editor
{
    [CustomEditor(typeof(ComponentViewBase), true)]
    public class ComponentViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ComponentViewBase component = serializedObject.targetObject as ComponentViewBase;
            serializedObject.Update();
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Component Type");
                GUILayout.TextField(component.ComponentObject?.GetType().Name);
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Entity");
                GUILayout.TextField(component.Entity?.Id.ToString());
            }
            // EditorGUILayout.PropertyField(serializedObject.FindProperty("_view"), true);
        }
    }
}
