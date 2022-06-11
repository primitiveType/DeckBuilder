using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace App.Editor
{
    [CustomPropertyDrawer(typeof(PileView))]
    public class PileView : PropertyDrawer 
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var modelField = new PropertyField(property.FindPropertyRelative(nameof(App.PileView.Model)));

            // Add fields to the container.
            container.Add(modelField);

            return container;
        }
    }
}
