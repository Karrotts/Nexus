using Godot;
using System;

public partial class NumberInput : Control
{
    public SpinBox GetSpinBox()
    {
        return GetNode<SpinBox>("MarginContainer/SpinBox");
    }
}
