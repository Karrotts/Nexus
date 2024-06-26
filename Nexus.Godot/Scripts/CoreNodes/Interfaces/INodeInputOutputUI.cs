﻿using System;
using Godot;

namespace Nexus.Godot;

public interface INodeInputOutputUI
{
    public ColorRect GetMarker();
    public string GetLabelName();
    public void SetDetails(string labelText, Type type);
}