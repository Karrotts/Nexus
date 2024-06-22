using System;
using Godot;

namespace GNodes;

public interface INodeInputOutputUI
{
    public ColorRect GetMarker();
    public void SetDetails(string labelText, Type type);
}