﻿using System;
using Godot;

namespace GNodes.UI.Scripts;

public static class TypeColors
{
    public static Color GetTypeColor(Type type)
    {
        if (type == typeof(int)) return Colors.Aqua;
        if (type == typeof(string)) return Colors.Green;
        if (type == typeof(float)) return Colors.Aquamarine;
        if (type == typeof(long)) return Colors.Orange;
        return Colors.White;
    }
}