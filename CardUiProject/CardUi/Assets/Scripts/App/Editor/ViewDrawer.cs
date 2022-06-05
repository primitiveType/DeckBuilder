using System.Collections;
using System.Collections.Generic;
using Api;
using CardsAndPiles;
using Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PileView))]
public class PileView : PropertyDrawer 
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var modelField = new PropertyField(property.FindPropertyRelative(nameof(Common.PileView.Model)));

        // Add fields to the container.
        container.Add(modelField);

        return container;
    }
}
