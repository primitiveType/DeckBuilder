using Godot;

namespace Deckbuilder;

public partial class TargetingArrow : Control
{
    private bool _visible;
    private Vector2 _from;
    private Vector2 _to;

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        ZIndex = 2000;
        SetAnchorsPreset(LayoutPreset.FullRect);
        OffsetLeft = 0;
        OffsetTop = 0;
        OffsetRight = 0;
        OffsetBottom = 0;
    }

    public void ShowArrow(Vector2 from, Vector2 to)
    {
        _visible = true;
        _from = from;
        _to = to;
        QueueRedraw();
    }

    public void HideArrow()
    {
        _visible = false;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (!_visible)
        {
            return;
        }

        Vector2 delta = _to - _from;
        if (delta.LengthSquared() < 16.0f)
        {
            return;
        }

        Vector2 control = (_from + _to) * 0.5f + new Vector2(0, -Mathf.Min(140.0f, delta.Length() * 0.35f));
        var points = new Vector2[28];
        for (int i = 0; i < points.Length; i++)
        {
            float t = i / (float)(points.Length - 1);
            points[i] = Quadratic(_from, control, _to, t);
        }

        var shadow = new Color(0.05f, 0.02f, 0.01f, 0.75f);
        var color = new Color(1.0f, 0.58f, 0.18f, 0.95f);
        DrawPolyline(points, shadow, 8.0f, true);
        DrawPolyline(points, color, 4.0f, true);

        Vector2 direction = (_to - points[^2]).Normalized();
        Vector2 normal = direction.Orthogonal();
        Vector2 tip = _to;
        Vector2 left = tip - direction * 22.0f + normal * 10.0f;
        Vector2 right = tip - direction * 22.0f - normal * 10.0f;
        DrawColoredPolygon(new[] { tip, left, right }, color);
    }

    private static Vector2 Quadratic(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        float inverse = 1.0f - t;
        return inverse * inverse * a + 2.0f * inverse * t * b + t * t * c;
    }
}
