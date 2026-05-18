using System.Linq;
using Api;
using CardsAndPiles.Components;
using Godot;
using SummerJam1;

namespace Deckbuilder;

public partial class EnemyView : Node3D
{
    private Label3D _intentLabel = null!;
    private Label3D _nameLabel = null!;
    private Label3D _statsLabel = null!;
    private MeshInstance3D _body = null!;
    private IEntity? _enemy;

    public IEntity? BoundEnemy => _enemy;

    public override void _Ready()
    {
        BuildVisuals();
        Refresh();
    }

    public void Bind(IEntity enemy)
    {
        _enemy = enemy;
        if (IsInsideTree())
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (_enemy == null || _nameLabel == null)
        {
            return;
        }

        _nameLabel.Text = _enemy.GetComponent<NameComponent>()?.Value ?? $"Enemy {_enemy.Id}";

        var health = _enemy.GetComponent<Health>();
        var strength = _enemy.GetComponent<Strength>();
        var armor = _enemy.GetComponent<Armor>();
        _statsLabel.Text = string.Join("  ", new[]
        {
            health == null ? null : $"HP {health.Amount}/{health.Max}",
            armor == null || armor.Amount <= 0 ? null : $"Armor {armor.Amount}",
            strength == null || strength.Amount == 0 ? null : $"Str {strength.Amount}"
        }.Where(value => !string.IsNullOrWhiteSpace(value)));

        var damageIntent = _enemy.GetComponents<DamageIntent>().FirstOrDefault(intent => intent.Enabled);
        var shieldIntent = _enemy.GetComponents<ShieldAllyIntent>().FirstOrDefault(intent => intent.Enabled);
        var chargeIntent = _enemy.GetComponents<ChargeIntent>().FirstOrDefault(intent => intent.Enabled);

        if (damageIntent != null)
        {
            string attacks = damageIntent.Attacks > 1 ? $" x{damageIntent.Attacks}" : "";
            _intentLabel.Text = $"Intent: {damageIntent.Amount}{attacks} damage";
        }
        else if (shieldIntent != null)
        {
            _intentLabel.Text = $"Intent: {shieldIntent.Amount} armor";
        }
        else if (chargeIntent != null)
        {
            _intentLabel.Text = "Intent: charging";
        }
        else
        {
            _intentLabel.Text = "Intent: unknown";
        }

        string assetName = _enemy.GetComponent<VisualComponent>()?.AssetName ?? "";
        if (_body.MaterialOverride is ShaderMaterial material)
        {
            material.SetShaderParameter("tint", ColorFromText(assetName + _nameLabel.Text));
        }
    }

    private void BuildVisuals()
    {
        _body = new MeshInstance3D
        {
            Mesh = new CapsuleMesh
            {
                Radius = 0.55f,
                Height = 1.65f,
                RadialSegments = 24,
                Rings = 8
            },
            Position = new Vector3(0, 0.8f, 0)
        };
        var material = new ShaderMaterial
        {
            Shader = new Shader
            {
                Code = "shader_type spatial;\nuniform vec4 tint : source_color = vec4(0.7, 0.3, 0.25, 1.0);\nvoid fragment(){ ALBEDO = tint.rgb; ROUGHNESS = 0.55; }"
            }
        };
        _body.MaterialOverride = material;
        AddChild(_body);

        _nameLabel = CreateLabel(new Vector3(0, 2.05f, 0), 48, new Color(1.0f, 0.94f, 0.80f));
        AddChild(_nameLabel);

        _statsLabel = CreateLabel(new Vector3(0, 1.75f, 0), 34, new Color(0.84f, 0.92f, 0.95f));
        AddChild(_statsLabel);

        _intentLabel = CreateLabel(new Vector3(0, 1.45f, 0), 30, new Color(1.0f, 0.72f, 0.45f));
        AddChild(_intentLabel);
    }

    private static Label3D CreateLabel(Vector3 position, int fontSize, Color color)
    {
        return new Label3D
        {
            Billboard = BaseMaterial3D.BillboardModeEnum.Enabled,
            FontSize = fontSize,
            Modulate = color,
            NoDepthTest = true,
            OutlineModulate = Colors.Black,
            OutlineSize = 8,
            Position = position,
            HorizontalAlignment = HorizontalAlignment.Center
        };
    }

    private static Color ColorFromText(string text)
    {
        uint hash = 2166136261;
        foreach (char character in text)
        {
            hash ^= character;
            hash *= 16777619;
        }

        float hue = (hash % 360) / 360.0f;
        return Color.FromHsv(hue, 0.58f, 0.78f);
    }
}
