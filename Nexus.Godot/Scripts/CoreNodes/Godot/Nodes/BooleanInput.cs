using Godot;
using System;

public partial class BooleanInput : Control
{
    public CheckButton GetCheckButton()
    {
        return GetNode<CheckButton>("CheckButton");
    }
}
