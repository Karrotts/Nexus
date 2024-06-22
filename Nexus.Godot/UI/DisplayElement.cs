using Godot;
using System;

public partial class DisplayElement : Control
{
	public delegate string Getter();
	private Getter _getter;
	public void SetDisplay(Getter getter) {
		_getter += getter;
	}

	public RichTextLabel GetLabel() {
		return GetNode<RichTextLabel>("VBoxContainer/RichTextLabel");
	}

    public override void _Process(double delta)
    {
        GetLabel().Text = $"[center]{_getter()}[/center]";
    }
}
