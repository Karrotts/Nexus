using System;
using Godot;
using Godot.Collections;

namespace Nexus.Godot.Scripts.File;
public class NexusItemInformation
{
    public Vector2 GlobalPosition { get; set; }
    public string  UUID { get; set; }
    public NexusOption NexusOption { get; set; }
    public Dictionary<string, Variant> StaticStates { get; set; }

    public Dictionary<string, Variant> GetAsJson()
    {
        return new Dictionary<string, Variant>()
        {
            { "UUID", UUID },
            { "GlobalPosition.X", GlobalPosition.X },
            { "GlobalPosition.Y", GlobalPosition.Y },
            { "NexusOption", NexusOption.ToString()},
            { "StaticStates", StaticStates}
        };
    }
}