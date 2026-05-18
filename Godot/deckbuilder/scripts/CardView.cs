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
    private IEntity? _card;
    private Label _costLabel = null!;
    private Label _descriptionLabel = null!;
    private Label _detailsLabel = null!;
    private Label _nameLabel = null!;
    private Label _typeLabel = null!;

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

    private void BuildLayout()
    {
        CustomMinimumSize = new Vector2(190, 250);
        SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        SizeFlagsVertical = SizeFlags.ShrinkBegin;
        MouseFilter = MouseFilterEnum.Ignore;
        PivotOffset = CustomMinimumSize / 2.0f;

        AddThemeStyleboxOverride("panel", new StyleBoxFlat
        {
            BgColor = new Color(0.13f, 0.12f, 0.10f),
            BorderColor = new Color(0.72f, 0.62f, 0.42f),
            BorderWidthBottom = 2,
            BorderWidthLeft = 2,
            BorderWidthRight = 2,
            BorderWidthTop = 2,
            CornerRadiusBottomLeft = 8,
            CornerRadiusBottomRight = 8,
            CornerRadiusTopLeft = 8,
            CornerRadiusTopRight = 8,
            ContentMarginBottom = 10,
            ContentMarginLeft = 10,
            ContentMarginRight = 10,
            ContentMarginTop = 10
        });

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 8);
        AddChild(root);

        var header = new HBoxContainer();
        header.AddThemeConstantOverride("separation", 8);
        root.AddChild(header);

        _costLabel = new Label
        {
            CustomMinimumSize = new Vector2(34, 34),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        _costLabel.AddThemeColorOverride("font_color", new Color(0.14f, 0.12f, 0.10f));
        _costLabel.AddThemeFontSizeOverride("font_size", 18);
        _costLabel.AddThemeStyleboxOverride("normal", new StyleBoxFlat
        {
            BgColor = new Color(0.88f, 0.72f, 0.30f),
            CornerRadiusBottomLeft = 17,
            CornerRadiusBottomRight = 17,
            CornerRadiusTopLeft = 17,
            CornerRadiusTopRight = 17
        });
        header.AddChild(_costLabel);

        _nameLabel = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        _nameLabel.AddThemeFontSizeOverride("font_size", 18);
        header.AddChild(_nameLabel);

        _typeLabel = new Label();
        _typeLabel.AddThemeColorOverride("font_color", new Color(0.72f, 0.68f, 0.58f));
        _typeLabel.AddThemeFontSizeOverride("font_size", 12);
        root.AddChild(_typeLabel);

        _descriptionLabel = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            VerticalAlignment = VerticalAlignment.Top
        };
        _descriptionLabel.AddThemeFontSizeOverride("font_size", 13);
        root.AddChild(_descriptionLabel);

        _detailsLabel = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            VerticalAlignment = VerticalAlignment.Bottom
        };
        _detailsLabel.AddThemeColorOverride("font_color", new Color(0.70f, 0.76f, 0.78f));
        _detailsLabel.AddThemeFontSizeOverride("font_size", 11);
        root.AddChild(_detailsLabel);
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
