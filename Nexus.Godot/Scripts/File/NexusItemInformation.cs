using System;
using Godot;
using Godot.Collections;

namespace Nexus.Godot.Scripts.File;
public class NexusItemInformation
{
    public NexusItemInformation() {}
    public NexusItemInformation(Dictionary<string, Variant> item)
    {
        UUID = (string)item["UUID"];
        GlobalPosition = new Vector2((float)item["GlobalPosition.X"], (float)item["GlobalPosition.Y"]);
        if (NexusOption.TryParse((string)item["NexusOption"], out NexusOption option))
        {
            NexusOption = option;
        }
        else
        {
            NexusOption = NexusOption.TEXT_DISPLAY;
        }
    }

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