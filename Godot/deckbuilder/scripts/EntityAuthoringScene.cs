using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Api;
using CardsAndPiles.Components;
using Godot;
using Newtonsoft.Json;
using SummerJam1;
using SummerJam1.Cards;
using SummerJam1.Characters;
using SummerJam1.Units;
using Component = Api.Component;

namespace Deckbuilder;

public enum AuthoringTemplate
{
    Card,
    Enemy,
    PlayerCharacter
}

public partial class EntityAuthoringScene : Control
{
    private const string DefaultContentRoot = "res://../../Cards/SummerJam1/StreamingAssets";

    private readonly List<Type> _filteredAddableTypes = new();
    private readonly List<string> _filteredPrefabPaths = new();
    private readonly List<Component> _visibleComponents = new();

    private ItemList _addComponentList = null!;
    private LineEdit _addComponentSearch = null!;
    private ItemList _componentList = null!;
    private OptionButton _createType = null!;
    private LineEdit _pathEdit = null!;
    private ItemList _prefabList = null!;
    private LineEdit _prefabSearch = null!;
    private VBoxContainer _propertyRows = null!;
    private Label _status = null!;

    private List<Type> _componentTypes = new();
    private string _contentRoot = "";
    private Context? _context;
    private IEntity? _entity;

    public override void _Ready()
    {
        Logging.Initialize(new GodotLogger(message => GD.Print(message)));
        _contentRoot = ProjectSettings.GlobalizePath(DefaultContentRoot);

        BuildUi();
        BootContext();
        RefreshPrefabList();
        RefreshAddComponentList();
        CreateNewEntity(AuthoringTemplate.Card, GetDefaultPrefabPath(AuthoringTemplate.Card));
    }

    private void BuildUi()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        var root = new VBoxContainer
        {
            OffsetLeft = 16,
            OffsetTop = 16,
            OffsetRight = -16,
            OffsetBottom = -16
        };
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.AddThemeConstantOverride("separation", 10);
        AddChild(root);

        var header = new HBoxContainer();
        header.AddThemeConstantOverride("separation", 10);
        root.AddChild(header);

        var title = new Label { Text = "Entity Authoring" };
        title.AddThemeFontSizeOverride("font_size", 26);
        header.AddChild(title);

        var backButton = AddButton(header, "Back", new Vector2(80, 36));
        backButton.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");

        var toolbar = new HFlowContainer();
        toolbar.AddThemeConstantOverride("h_separation", 8);
        toolbar.AddThemeConstantOverride("v_separation", 8);
        root.AddChild(toolbar);

        _createType = new OptionButton { CustomMinimumSize = new Vector2(170, 36) };
        foreach (AuthoringTemplate template in Enum.GetValues<AuthoringTemplate>())
        {
            _createType.AddItem(template.ToString());
        }
        _createType.ItemSelected += index =>
        {
            var template = (AuthoringTemplate)index;
            _pathEdit.Text = GetDefaultPrefabPath(template);
        };
        toolbar.AddChild(_createType);

        _pathEdit = new LineEdit
        {
            PlaceholderText = "Cards/NewCard.json",
            Text = GetDefaultPrefabPath(AuthoringTemplate.Card),
            CustomMinimumSize = new Vector2(360, 36),
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        toolbar.AddChild(_pathEdit);

        var newButton = AddButton(toolbar, "New", new Vector2(96, 36));
        newButton.Pressed += () => RunAuthoringAction(() =>
        {
            CreateNewEntity((AuthoringTemplate)_createType.Selected, _pathEdit.Text);
            SetStatus("Created new entity template.");
        });

        var saveButton = AddButton(toolbar, "Save", new Vector2(96, 36));
        saveButton.Pressed += () => RunAuthoringAction(SaveCurrentEntity);

        var loadButton = AddButton(toolbar, "Load", new Vector2(96, 36));
        loadButton.Pressed += () => RunAuthoringAction(LoadSelectedOrTypedPrefab);

        var refreshButton = AddButton(toolbar, "Refresh", new Vector2(96, 36));
        refreshButton.Pressed += RefreshPrefabList;

        _status = new Label { Text = "", AutowrapMode = TextServer.AutowrapMode.WordSmart };
        _status.AddThemeColorOverride("font_color", new Color(0.70f, 0.76f, 0.78f));
        root.AddChild(_status);

        var panes = new HSplitContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(panes);

        panes.AddChild(BuildLibraryPane());

        var editorSplit = new HSplitContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        panes.AddChild(editorSplit);
        editorSplit.AddChild(BuildComponentPane());
        editorSplit.AddChild(BuildPropertyPane());
    }

    private Control BuildLibraryPane()
    {
        var root = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(300, 0),
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddThemeConstantOverride("separation", 8);

        var prefabsHeader = new Label { Text = "Prefabs" };
        prefabsHeader.AddThemeFontSizeOverride("font_size", 18);
        root.AddChild(prefabsHeader);

        _prefabSearch = new LineEdit { PlaceholderText = "Filter prefabs" };
        _prefabSearch.TextChanged += _ => RefreshPrefabList();
        root.AddChild(_prefabSearch);

        _prefabList = new ItemList
        {
            SizeFlagsVertical = SizeFlags.ExpandFill,
            AutoHeight = false
        };
        _prefabList.ItemActivated += _ => RunAuthoringAction(LoadSelectedOrTypedPrefab);
        _prefabList.ItemSelected += index =>
        {
            if (index >= 0 && index < _filteredPrefabPaths.Count)
            {
                _pathEdit.Text = _filteredPrefabPaths[(int)index];
            }
        };
        root.AddChild(_prefabList);

        var addHeader = new Label { Text = "Add Component" };
        addHeader.AddThemeFontSizeOverride("font_size", 18);
        root.AddChild(addHeader);

        _addComponentSearch = new LineEdit { PlaceholderText = "Filter components" };
        _addComponentSearch.TextChanged += _ => RefreshAddComponentList();
        root.AddChild(_addComponentSearch);

        _addComponentList = new ItemList
        {
            CustomMinimumSize = new Vector2(0, 220),
            AutoHeight = false
        };
        _addComponentList.ItemActivated += index => RunAuthoringAction(() => AddComponent((int)index));
        root.AddChild(_addComponentList);

        return root;
    }

    private Control BuildComponentPane()
    {
        var root = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(280, 0),
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddThemeConstantOverride("separation", 8);

        var header = new Label { Text = "Current Components" };
        header.AddThemeFontSizeOverride("font_size", 18);
        root.AddChild(header);

        _componentList = new ItemList
        {
            SizeFlagsVertical = SizeFlags.ExpandFill,
            AutoHeight = false
        };
        _componentList.ItemSelected += index => RefreshProperties((int)index);
        root.AddChild(_componentList);

        var removeButton = AddButton(root, "Remove Selected", new Vector2(0, 36));
        removeButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        removeButton.Pressed += () => RunAuthoringAction(RemoveSelectedComponent);

        return root;
    }

    private Control BuildPropertyPane()
    {
        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(440, 0),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };

        var scroll = new ScrollContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        panel.AddChild(scroll);

        _propertyRows = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        _propertyRows.AddThemeConstantOverride("separation", 8);
        scroll.AddChild(_propertyRows);

        return panel;
    }

    private Button AddButton(Control parent, string text, Vector2 minimumSize)
    {
        var button = new Button
        {
            Text = text,
            CustomMinimumSize = minimumSize
        };
        parent.AddChild(button);
        return button;
    }

    private void BootContext()
    {
        _context = new Context(new SummerJam1Events());
        _context.SetPrefabsDirectory(_contentRoot);
        _context.Root.AddComponent<Game>();

        _componentTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Where(type => typeof(IComponent).IsAssignableFrom(type))
            .Where(type => typeof(Component).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract && !type.IsInterface && !type.IsGenericType)
            .OrderBy(type => type.FullName)
            .ToList();
    }

    private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            return exception.Types.Where(type => type != null)!;
        }
    }

    private void CreateNewEntity(AuthoringTemplate template, string prefabPath)
    {
        var context = RequireContext();
        _entity?.Destroy();
        _entity = context.CreateEntity(null);

        switch (template)
        {
            case AuthoringTemplate.Card:
                CreateCardTemplate(_entity);
                break;
            case AuthoringTemplate.Enemy:
                CreateEnemyTemplate(_entity);
                break;
            case AuthoringTemplate.PlayerCharacter:
                CreatePlayerCharacterTemplate(_entity);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(template), template, null);
        }

        _pathEdit.Text = NormalizePrefabPath(prefabPath);
        RefreshCurrentComponents();
    }

    private void CreateCardTemplate(IEntity entity)
    {
        entity.AddComponent<PlayerCard>();
        entity.AddComponent<DescriptionComponent>().Description = "";
        entity.AddComponent<VisualComponent>().AssetName = "";
        entity.AddComponent<Discard>();
        entity.AddComponent<NameComponent>().Value = "New Card";
        entity.AddComponent<EnergyCost>().Amount = 1;
    }

    private void CreateEnemyTemplate(IEntity entity)
    {
        entity.AddComponent<StarterUnit>();
        entity.AddComponent<NameComponent>().Value = "New Enemy";
        entity.AddComponent<VisualComponent>().AssetName = "";
        var health = entity.AddComponent<Health>();
        health.Max = 10;
        health.Amount = 10;
        var intent = entity.AddComponent<DamageIntent>();
        intent.Amount = 5;
        intent.Attacks = 1;
        entity.AddComponent<RandomIntentHandler>();
    }

    private void CreatePlayerCharacterTemplate(IEntity entity)
    {
        entity.AddComponent<PlayerUnit>();
        entity.AddComponent<NameComponent>().Value = "New Character";
        entity.AddComponent<VisualComponent>().AssetName = "";
        var health = entity.AddComponent<Health>();
        health.Max = 10;
        health.Amount = 10;
        entity.AddComponent<Strength>().Amount = 0;
        entity.AddComponent<PartyMember>().DisplayName = "New Character";
        entity.AddComponent<EquipmentLoadout>();
    }

    private void LoadSelectedOrTypedPrefab()
    {
        var path = _pathEdit.Text;
        int selected = _prefabList.GetSelectedItems().FirstOrDefault(-1);
        if (selected >= 0 && selected < _filteredPrefabPaths.Count)
        {
            path = _filteredPrefabPaths[selected];
        }

        LoadPrefab(path);
    }

    private void LoadPrefab(string prefabPath)
    {
        var context = RequireContext();
        string normalized = NormalizePrefabPath(prefabPath);
        _entity?.Destroy();
        _entity = context.CreateEntity(null, normalized);
        _entity.GetOrAddComponent<SourcePrefab>().Prefab = normalized;
        _pathEdit.Text = normalized;
        RefreshCurrentComponents();
        SetStatus($"Loaded {normalized}.");
    }

    private void SaveCurrentEntity()
    {
        if (_entity == null)
        {
            throw new InvalidOperationException("No entity is currently loaded.");
        }

        string relativePath = NormalizePrefabPath(_pathEdit.Text);
        _entity.GetOrAddComponent<SourcePrefab>().Prefab = relativePath;

        string fullPath = Path.Combine(_contentRoot, "Prefabs", relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? _contentRoot);
        File.WriteAllText(fullPath, Serializer.SerializeWithoutIds(_entity));

        _pathEdit.Text = relativePath;
        RefreshPrefabList();
        SetStatus($"Saved {relativePath}.");
    }

    private void AddComponent(int index)
    {
        if (_entity == null || index < 0 || index >= _filteredAddableTypes.Count)
        {
            return;
        }

        _entity.AddComponent(_filteredAddableTypes[index]);
        RefreshCurrentComponents();
    }

    private void RemoveSelectedComponent()
    {
        if (_entity == null)
        {
            return;
        }

        int selected = _componentList.GetSelectedItems().FirstOrDefault(-1);
        if (selected < 0 || selected >= _visibleComponents.Count)
        {
            return;
        }

        _entity.RemoveComponent(_visibleComponents[selected]);
        RefreshCurrentComponents();
    }

    private void RefreshPrefabList()
    {
        _filteredPrefabPaths.Clear();
        _prefabList?.Clear();

        string prefabsRoot = Path.Combine(_contentRoot, "Prefabs");
        if (!Directory.Exists(prefabsRoot))
        {
            SetStatus($"Prefab directory not found: {prefabsRoot}");
            return;
        }

        string filter = _prefabSearch?.Text ?? "";
        foreach (string file in Directory.EnumerateFiles(prefabsRoot, "*.json", SearchOption.AllDirectories).OrderBy(path => path))
        {
            string relative = Path.GetRelativePath(prefabsRoot, file);
            if (!MatchesFilter(relative, filter))
            {
                continue;
            }

            _filteredPrefabPaths.Add(relative);
            _prefabList?.AddItem(relative);
        }
    }

    private void RefreshAddComponentList()
    {
        _filteredAddableTypes.Clear();
        _addComponentList?.Clear();

        string filter = _addComponentSearch?.Text ?? "";
        foreach (Type type in _componentTypes.Where(type => MatchesFilter(type.FullName ?? type.Name, filter)))
        {
            _filteredAddableTypes.Add(type);
            _addComponentList?.AddItem(type.FullName ?? type.Name);
        }
    }

    private void RefreshCurrentComponents()
    {
        _visibleComponents.Clear();
        _componentList.Clear();
        ClearProperties("Select a component to edit its public values.");

        if (_entity == null)
        {
            return;
        }

        foreach (Component component in _entity.Components)
        {
            _visibleComponents.Add(component);
            _componentList.AddItem(component.GetType().Name);
        }

        if (_visibleComponents.Count > 0)
        {
            _componentList.Select(0);
            RefreshProperties(0);
        }
    }

    private void RefreshProperties(int componentIndex)
    {
        if (componentIndex < 0 || componentIndex >= _visibleComponents.Count)
        {
            ClearProperties("Select a component to edit its public values.");
            return;
        }

        Component component = _visibleComponents[componentIndex];
        _propertyRows.QueueFreeChildren();

        var header = new Label { Text = component.GetType().FullName };
        header.AddThemeFontSizeOverride("font_size", 18);
        _propertyRows.AddChild(header);

        List<PropertyInfo> properties = component.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(IsEditableProperty)
            .OrderBy(property => property.Name)
            .ToList();

        if (properties.Count == 0)
        {
            _propertyRows.AddChild(new Label { Text = "No editable public values." });
            return;
        }

        foreach (PropertyInfo property in properties)
        {
            AddPropertyEditor(component, property);
        }
    }

    private bool IsEditableProperty(PropertyInfo property)
    {
        if (property.GetIndexParameters().Length > 0 || property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
        {
            return false;
        }

        if (property.SetMethod == null)
        {
            return false;
        }

        Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        return type == typeof(string) || type == typeof(int) || type == typeof(float) || type == typeof(double) ||
               type == typeof(bool) || type.IsEnum;
    }

    private void AddPropertyEditor(Component component, PropertyInfo property)
    {
        var row = new HBoxContainer();
        row.AddThemeConstantOverride("separation", 8);
        _propertyRows.AddChild(row);

        var label = new Label
        {
            Text = property.Name,
            CustomMinimumSize = new Vector2(150, 0),
            TooltipText = property.PropertyType.Name
        };
        row.AddChild(label);

        Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        object? value = property.GetValue(component);

        if (type == typeof(bool))
        {
            var checkBox = new CheckBox { ButtonPressed = value is true };
            checkBox.Toggled += toggled => SetProperty(component, property, toggled);
            row.AddChild(checkBox);
            return;
        }

        if (type == typeof(int) || type == typeof(float) || type == typeof(double))
        {
            var spinBox = new SpinBox
            {
                MinValue = -999999,
                MaxValue = 999999,
                Step = type == typeof(int) ? 1 : 0.1,
                Value = Convert.ToDouble(value ?? 0),
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            spinBox.ValueChanged += number => SetProperty(component, property, ConvertNumber(number, type));
            row.AddChild(spinBox);
            return;
        }

        if (type.IsEnum)
        {
            var option = new OptionButton { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            Array values = Enum.GetValues(type);
            for (int i = 0; i < values.Length; i++)
            {
                object enumValue = values.GetValue(i)!;
                option.AddItem(enumValue.ToString());
                if (Equals(enumValue, value))
                {
                    option.Selected = i;
                }
            }
            option.ItemSelected += index => SetProperty(component, property, values.GetValue((int)index));
            row.AddChild(option);
            return;
        }

        var lineEdit = new LineEdit
        {
            Text = value?.ToString() ?? "",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        lineEdit.TextSubmitted += text => SetProperty(component, property, text);
        lineEdit.FocusExited += () => SetProperty(component, property, lineEdit.Text);
        row.AddChild(lineEdit);
    }

    private object ConvertNumber(double value, Type type)
    {
        if (type == typeof(int))
        {
            return (int)Math.Round(value);
        }

        if (type == typeof(float))
        {
            return (float)value;
        }

        return value;
    }

    private void SetProperty(Component component, PropertyInfo property, object? value)
    {
        property.SetValue(component, value);
        SetStatus($"Updated {component.GetType().Name}.{property.Name}.");
    }

    private void ClearProperties(string message)
    {
        _propertyRows.QueueFreeChildren();
        _propertyRows.AddChild(new Label { Text = message });
    }

    private string GetDefaultPrefabPath(AuthoringTemplate template)
    {
        return template switch
        {
            AuthoringTemplate.Card => "Cards/NewCard.json",
            AuthoringTemplate.Enemy => "Units/Standard/1/NewEnemy.json",
            AuthoringTemplate.PlayerCharacter => "Units/Player/NewCharacter.json",
            _ => "NewPrefab.json"
        };
    }

    private string NormalizePrefabPath(string path)
    {
        string normalized = string.IsNullOrWhiteSpace(path) ? "NewPrefab.json" : path.Trim();
        normalized = normalized.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        if (!normalized.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            normalized += ".json";
        }

        return normalized;
    }

    private bool MatchesFilter(string value, string filter)
    {
        return string.IsNullOrWhiteSpace(filter) ||
               value.Contains(filter, StringComparison.OrdinalIgnoreCase);
    }

    private Context RequireContext()
    {
        return _context ?? throw new InvalidOperationException("Authoring context is not booted.");
    }

    private void RunAuthoringAction(Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            SetStatus($"ERROR: {exception.Message}");
            GD.PushError(exception.ToString());
        }
    }

    private void SetStatus(string message)
    {
        _status.Text = message;
    }
}

public static class NodeExtensions
{
    public static void QueueFreeChildren(this Node node)
    {
        foreach (Node child in node.GetChildren())
        {
            child.QueueFree();
        }
    }
}
