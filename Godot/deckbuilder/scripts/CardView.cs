using System;
using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Godot;
using SummerJam1.Cards;

namespace Deckbuilder;

public partial class CardView : PanelContainer
{
    public const float BaseWidth = 190.0f;
    public const float BaseHeight = 250.0f;

    private IEntity? _card;
    private Label _costLabel = null!;
    private Label _descriptionLabel = null!;
    private Label _detailsLabel = null!;
    private Label _nameLabel = null!;
    private Label _typeLabel = null!;
    private VBoxContainer _rootLayout = null!;
    private HBoxContainer _headerLayout = null!;
    private float _presentationScale = 1.0f;

    public IEntity? BoundCard => _card;

    public override void _Ready()
    {
        BuildLayout();
        Refresh();
    }

    public void Bind(IEntity card)
    {
        _card = card;
        if (IsInsideTree())
        {
            Refresh();
        }
    }

    public void ApplyPresentationScale(float scale)
    {
        _presentationScale = Mathf.Clamp(scale, 0.55f, 1.8f);
        if (_costLabel == null)
        {
            return;
        }

        Vector2 cardSize = new(BaseWidth * _presentationScale, BaseHeight * _presentationScale);
        CustomMinimumSize = cardSize;
        Size = cardSize;
        Scale = Vector2.One;
        PivotOffset = cardSize / 2.0f;

        AddThemeStyleboxOverride("panel", CreatePanelStyle(_presentationScale));
        _rootLayout.AddThemeConstantOverride("separation", ScaledInt(8));
        _headerLayout.AddThemeConstantOverride("separation", ScaledInt(8));

        float costSize = 34.0f * _presentationScale;
        _costLabel.CustomMinimumSize = new Vector2(costSize, costSize);
        _costLabel.AddThemeFontSizeOverride("font_size", ScaledInt(18));
        _costLabel.AddThemeStyleboxOverride("normal", CreateCostStyle(_presentationScale));

        _nameLabel.AddThemeFontSizeOverride("font_size", ScaledInt(18));
        _typeLabel.AddThemeFontSizeOverride("font_size", ScaledInt(12));
        _descriptionLabel.AddThemeFontSizeOverride("font_size", ScaledInt(13));
        _detailsLabel.AddThemeFontSizeOverride("font_size", ScaledInt(11));
    }

    private void BuildLayout()
    {
        SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        SizeFlagsVertical = SizeFlags.ShrinkBegin;
        MouseFilter = MouseFilterEnum.Ignore;
        ClipContents = true;

        _rootLayout = new VBoxContainer();
        AddChild(_rootLayout);

        _headerLayout = new HBoxContainer();
        _rootLayout.AddChild(_headerLayout);

        _costLabel = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        _costLabel.AddThemeColorOverride("font_color", new Color(0.14f, 0.12f, 0.10f));
        _headerLayout.AddChild(_costLabel);

        _nameLabel = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            ClipText = true,
            MaxLinesVisible = 2,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        _headerLayout.AddChild(_nameLabel);

        _typeLabel = new Label();
        _typeLabel.AddThemeColorOverride("font_color", new Color(0.72f, 0.68f, 0.58f));
        _rootLayout.AddChild(_typeLabel);

        _descriptionLabel = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            ClipText = true,
            MaxLinesVisible = 5,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            VerticalAlignment = VerticalAlignment.Top
        };
        _rootLayout.AddChild(_descriptionLabel);

        _detailsLabel = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            ClipText = true,
            MaxLinesVisible = 3,
            VerticalAlignment = VerticalAlignment.Bottom
        };
        _detailsLabel.AddThemeColorOverride("font_color", new Color(0.70f, 0.76f, 0.78f));
        _rootLayout.AddChild(_detailsLabel);

        ApplyPresentationScale(_presentationScale);
    }

    private int ScaledInt(float value)
    {
        return Mathf.Max(1, Mathf.RoundToInt(value * _presentationScale));
    }

    private static StyleBoxFlat CreatePanelStyle(float scale)
    {
        int border = Mathf.Max(1, Mathf.RoundToInt(2.0f * scale));
        int radius = Mathf.Max(1, Mathf.RoundToInt(8.0f * scale));
        int margin = Mathf.Max(1, Mathf.RoundToInt(10.0f * scale));
        return new StyleBoxFlat
        {
            BgColor = new Color(0.13f, 0.12f, 0.10f),
            BorderColor = new Color(0.72f, 0.62f, 0.42f),
            BorderWidthBottom = border,
            BorderWidthLeft = border,
            BorderWidthRight = border,
            BorderWidthTop = border,
            CornerRadiusBottomLeft = radius,
            CornerRadiusBottomRight = radius,
            CornerRadiusTopLeft = radius,
            CornerRadiusTopRight = radius,
            ContentMarginBottom = margin,
            ContentMarginLeft = margin,
            ContentMarginRight = margin,
            ContentMarginTop = margin
        };
    }

    private static StyleBoxFlat CreateCostStyle(float scale)
    {
        int radius = Mathf.Max(1, Mathf.RoundToInt(17.0f * scale));
        return new StyleBoxFlat
        {
            BgColor = new Color(0.88f, 0.72f, 0.30f),
            CornerRadiusBottomLeft = radius,
            CornerRadiusBottomRight = radius,
            CornerRadiusTopLeft = radius,
            CornerRadiusTopRight = radius
        };
    }

    private void Refresh()
    {
        if (_card == null || _nameLabel == null)
        {
            return;
        }

        _nameLabel.Text = GetCardName(_card);
        _costLabel.Text = GetCostText(_card);
        _typeLabel.Text = GetPrimaryCardType(_card);
        _descriptionLabel.Text = GetDescription(_card);
        _detailsLabel.Text = GetDetails(_card);
    }

    private static string GetCardName(IEntity card)
    {
        return card.GetComponent<NameComponent>()?.Value ?? $"Card #{card.Id}";
    }

    private static string GetCostText(IEntity card)
    {
        var energyCost = card.GetComponent<EnergyCost>();
        if (energyCost != null)
        {
            return energyCost.Amount.ToString();
        }

        var cost = card.Components
            .OfType<IAmount>()
            .FirstOrDefault(component => component.GetType().Name.Contains("Cost", StringComparison.OrdinalIgnoreCase));

        return cost?.Amount.ToString() ?? "-";
    }

    private static string GetPrimaryCardType(IEntity card)
    {
        var cardComponent = card.GetComponent<Card>();
        return cardComponent?.GetType().Name ?? "Card";
    }

    private static string GetDescription(IEntity card)
    {
        List<string> descriptions = card.Components
            .OfType<IDescription>()
            .Select(description => description.Description)
            .Where(description => !string.IsNullOrWhiteSpace(description))
            .Distinct()
            .ToList();

        if (descriptions.Count == 0)
        {
            return "No description.";
        }

        return string.Join("\n", descriptions);
    }

    private static string GetDetails(IEntity card)
    {
        List<string> amounts = card.Components
            .OfType<IAmount>()
            .Where(component => component is not EnergyCost)
            .Select(component => $"{component.GetType().Name}: {component.Amount}")
            .ToList();

        List<string> tooltips = card.Components
            .OfType<ITooltip>()
            .Select(tooltip => tooltip.Tooltip)
            .Where(tooltip => !string.IsNullOrWhiteSpace(tooltip))
            .Distinct()
            .ToList();

        List<string> componentNames = card.Components
            .Select(component => component.GetType().Name)
            .Where(name => name is not nameof(NameComponent) and not nameof(DescriptionComponent))
            .Distinct()
            .Take(6)
            .ToList();

        var groups = new List<string>();
        if (amounts.Count > 0)
        {
            groups.Add(string.Join(" | ", amounts));
        }

        if (tooltips.Count > 0)
        {
            groups.Add(string.Join(" | ", tooltips));
        }

        if (componentNames.Count > 0)
        {
            groups.Add(string.Join(", ", componentNames));
        }

        return string.Join("\n", groups);
    }
}
