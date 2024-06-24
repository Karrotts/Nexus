using Godot;
using System;

public partial class NodeUI : Node2D
{
	private NodeManager _nodeManager;
	private Vector2 _mouseWas;
	public override void _Ready()
	{
		_nodeManager = GetNode<NodeManager>("/root/NodeManager");
	}
	
	public override void _Process(double delta)
	{
		HandleCameraDrag();
		GD.Print($"Camera Position: {GetNode<Camera2D>("Camera2D").Position}, Mouse Position: {GetViewport().GetMousePosition()} Mouse Was: {_mouseWas}, Diff: {GetViewport().GetMousePosition() - _mouseWas}");
		
	}

	public void HandleCameraDrag()
	{
		if (_nodeManager.CurrentlyDraggingNode != null || _nodeManager.SelectedIo != null || !Input.IsMouseButtonPressed(MouseButton.Left))
		{
			_mouseWas = GetViewport().GetMousePosition();
			return;
		};
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			GetNode<Camera2D>("Camera2D").Position -= GetViewport().GetMousePosition() - _mouseWas;
			_mouseWas = GetViewport().GetMousePosition();
		}
	}
}
