using UnityEditor;
using UnityEngine;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Api;
using CardsAndPiles.Components;
using SummerJam1;

public class CustomHierarchyWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private IEntity rootEntity;
    private IEntity selectedEntity;
    int selectedPrefab = 0;
    private string[] Prefabs = { };

    private Dictionary<string, bool> entityFoldoutStates = new();

    [MenuItem("Window/Custom Hierarchy")]
    public static void ShowWindow()
    {
        GetWindow<CustomHierarchyWindow>("Custom Hierarchy");
    }

    private bool Initialize()
    {
        if (GameContext.Instance?.Game?.Entity == null)
        {
            return false;
        }

        if (rootEntity == GameContext.Instance.Game.Entity)
        {
            return true;
        }

        rootEntity = GameContext.Instance.Game.Entity;
        DirectoryInfo di = new DirectoryInfo(Context.PrefabsPath);
        List<string> prefabs = new List<string>();
        foreach (var prefab in di.EnumerateFiles("*", SearchOption.AllDirectories))
        {
            if (prefab.Extension == ".json")
            {
                string name = prefab.FullName.Replace(di.FullName, "").Replace("\\", "/");
                name = name.TrimStart('/');
                prefabs.Add(name);
            }
        }

        Prefabs = prefabs.ToArray();
        Debug.Log($"{Prefabs.Length} prefabs found at {Context.PrefabsPath}.");

        return true;
    }

    private void OnGUI()
    {
        if (!Initialize())
        {
            return;
        }

        EditorGUILayout.LabelField("Custom Hierarchy", EditorStyles.boldLabel);

        // Scrollable area for the hierarchy
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (rootEntity != null)
        {
            DrawEntity(rootEntity, 0);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Inspector", EditorStyles.boldLabel);

        if (selectedEntity != null)
        {
            DrawInspector(selectedEntity);
        }
        else
        {
            EditorGUILayout.LabelField("Select an entity to view its details.");
        }
    }

    private void DrawEntity(IEntity entity, int indentLevel)
    {
        string entityId = entity.Id.ToString();
        if (!entityFoldoutStates.ContainsKey(entityId))
        {
            entityFoldoutStates[entityId] = false; // Default to collapsed
        }

        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(indentLevel * 15); // Indentation

        bool isSelected = selectedEntity == entity;

        string name = entity.GetComponent<NameComponent>()?.Value ?? $"Entity {entity.Id}";
        if (entity.Children.Count > 0)
        {
            var style = new GUIStyle(EditorStyles.foldout);
            entityFoldoutStates[entityId] = EditorGUILayout.Foldout(entityFoldoutStates[entityId], "", style);
        }
        else
        {
            entityFoldoutStates[entityId] = false;
            
        }

        if (GUILayout.Button(name, isSelected ? EditorStyles.boldLabel : EditorStyles.label))
        {
            selectedEntity = entity;
        }


        EditorGUILayout.EndHorizontal();

        // Draw children recursively
        if (entityFoldoutStates[entityId] && entity.Children != null && entity.Children.Count > 0)
        {
            foreach (var child in entity.Children)
            {
                DrawEntity(child, indentLevel + 1);
            }
        }
    }

    private void DrawInspector(IEntity entity)
    {
        Type entityType = entity.GetType();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Child"))
        {
            GameContext.Instance.Context.CreateEntity(selectedEntity, Prefabs[selectedPrefab]);
        }

        Debug.Log($"{Prefabs.Length} prefabs.");
        Debug.Log($"{Prefabs[0]} .");
        Debug.Log($"{Prefabs[1]} .");

        selectedPrefab = EditorGUILayout.Popup("Choose an option", selectedPrefab, Prefabs);
        EditorGUILayout.EndHorizontal();

        foreach (var component in entity.Components)
        {
            var compName = component.GetType().Name;
            var key = $"{entity.Id}_{compName}";
            if (!entityFoldoutStates.ContainsKey(key))
            {
                entityFoldoutStates[key] = false; // Default to closed
            }

            entityFoldoutStates[key] = EditorGUILayout.Foldout(entityFoldoutStates[key], compName);
            if(entityFoldoutStates[key])
            {
                DrawComponent(component, component.GetType());
            }
        }
    }

    private static void DrawComponent(object component, Type entityType)
    {
        PropertyInfo[] properties =
            entityType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var fieldValue = property.GetValue(component);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(property.Name, GUILayout.Width(150));
            if (property.SetMethod != null)
            {
                string newVal = EditorGUILayout.TextField(fieldValue != null ? fieldValue.ToString() : "null");
                TrySetPropertyFromString(property, component, newVal);
            }
            else
            {
                EditorGUILayout.LabelField(fieldValue != null ? fieldValue.ToString() : "null");
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private static bool TrySetPropertyFromString(PropertyInfo property, object obj, string value)
    {
        try
        {
            if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
            {
                object parsedValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(obj, parsedValue);
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to set property '{property.Name}' with value '{value}': {ex.Message}");
        }

        return false;
    }
}