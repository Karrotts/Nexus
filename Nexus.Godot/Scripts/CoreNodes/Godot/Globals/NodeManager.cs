using Godot;
using System;
using Nexus;
using Nexus.Elements.Basic;
using Nexus.Elements.Display;
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
    private string _nodeElementPath = "res://UI/node_element.tscn";
    private PackedScene _nodeElementScene;

    public override void _Ready()
    {
        _nodeElementScene = ResourceLoader.Load(_nodeElementPath) as PackedScene;
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

        if (Input.IsActionJustPressed("ui_accept"))
        {
            HandleNodeCreate();
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

    private void HandleNodeCreate()
    {
        // should add this to a specific container but this is fine for now.
        NodeElement element = _nodeElementScene.Instantiate<NodeElement>();
        element.Node = new MathNexus()
        {
            InputA = new NexusInput<double>(() => 42),
            InputB = new NexusInput<double>(() => 23)
        };
        AddChild(element);
        
        NodeElement element2 = _nodeElementScene.Instantiate<NodeElement>();
        element2.Node = new OutputNexus()
        {
            DisplayInput = new NexusInput<double>(() => 0)
        };
        AddChild(element2);
        
        Nexus.Nexus.ConnectOutputToInput(element.Node, element2.Node, "OutputC", "DisplayInput");
    }
    
}