using Godot;
using System;
using Nexus.Godot;
using Nexus.Godot.UI;

public partial class NodeManager : Node
{
    public ulong? CurrentlyDraggingNode
    {
        get
        {
            if (SelectedIo != null) return null;
            return _currentlyDraggingNode;
        }
        set => _currentlyDraggingNode = value;
    }

    public NodeIoInformation SelectedIo { get; set; }
    public NodeIoInformation HoveredIo { get; set; }

    private LineRenderer _lineRenderer;
    private ulong? _currentlyDraggingNode;

    public override void _Ready()
    {
        _lineRenderer = GetNode<LineRenderer>("/root/LineRenderer");
    }

    public override void _Process(double delta)
    {
        HandleNodeIoSelection();
        
        // notes to future me
        // after we get the selected node we need to check if it an input
        // then if it is an input then we need to disconnect all any nodes that are currently
        // connecting it and set the selected IO to the node that it was disconnected from
        // if its an output IO then we dont need to do that.

        if (SelectedIo != null)
        {
            Vector2 offset = SelectedIo.Element.GetInput(SelectedIo.IoName).GetMarker().Size / 2;
            _lineRenderer.CreateOrUpdate("s", new DrawLine()
            {
                From = SelectedIo.Element.GetInput(SelectedIo.IoName).GetMarker().GlobalPosition + offset,
                To = GetViewport().GetMousePosition(),
                Color = Colors.Aqua,
            });
        }
    }

    private void HandleNodeIoSelection()
    {
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            if (HoveredIo != null && SelectedIo == null)
            {
                SelectedIo = HoveredIo;
            }
            return;
        }
        SelectedIo = null;
    }
    
}