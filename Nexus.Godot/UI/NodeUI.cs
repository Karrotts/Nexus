using Godot;
using System;
using Nexus.Godot;

public partial class NodeUI : Node2D
{
	private NodeManager _nodeManager;
	private LineRenderer _lineRenderer;
	private Vector2 _mouseWas;
	public override void _Ready()
	{
		_nodeManager = GetNode<NodeManager>("/root/NodeManager");
		_lineRenderer = GetNode<LineRenderer>("/root/LineRenderer");
	}
	
	public override void _Process(double delta)
	{
		HandleCameraDrag();
		_lineRenderer.Position = GetNode<Camera2D>("Camera2D").Position + GetViewportRect().Size / 2;

	}

	public void HandleCameraDrag()
	{
		if (_nodeManager.CurrentlyDraggingNode != null || _nodeManager.SelectedIo != null || !Input.IsMouseButtonPressed(MouseButton.Left))
		{
			_mouseWas = GetViewport().GetMousePosition();
			_nodeManager.IsMovingCamera = false;
			return;
		};
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			GetNode<Camera2D>("Camera2D").Position -= GetViewport().GetMousePosition() - _mouseWas;
			_nodeManager.IsMovingCamera = true;
			_mouseWas = GetViewport().GetMousePosition();
		}
	}
}
