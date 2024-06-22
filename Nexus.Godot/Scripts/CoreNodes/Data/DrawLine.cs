using Godot;

namespace Nexus.Godot;

public partial class DrawLine : GodotObject
{
    public Vector2 From;
    public Vector2 To;
    public Color Color;
    public double Size = 4.0f;
}