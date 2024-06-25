using System;
using Godot;

namespace Nexus.Godot.UI.Scripts;

public static class TypeColors
{
    public static Color GetTypeColor(Type type)
    {
        if (type == typeof(string)) return Colors.Green;
        if (type == typeof(double)) return Colors.Blue;
        if (type == typeof(bool)) return Colors.Orange;
        return Colors.White;
    }
}