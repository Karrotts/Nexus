using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using Nexus;
using Nexus.Elements.Basic;
using Nexus.Elements.Boolean;
using Nexus.Elements.Display;
using Nexus.Godot;
using Nexus.Godot.Scripts.File;
using Nexus.Godot.UI;

public partial class NodeManager : Node2D
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
    public bool IsMovingCamera { get; set; }

    private LineRenderer _lineRenderer;
    private ulong? _currentlyDraggingNode;
    private string _nodeElementPath = "res://UI/node_element.tscn";
    private PackedScene _nodeElementScene;
    private List<ConnectedIO> _connections;
    private List<NodeElement> _nodeElements;

    public override void _Ready()
    {
        _nodeElementScene = ResourceLoader.Load(_nodeElementPath) as PackedScene;
        _lineRenderer = GetNode<LineRenderer>("/root/LineRenderer");
        _connections = new List<ConnectedIO>();
        _nodeElements = new List<NodeElement>();
    }

    public override void _Process(double delta)
    {
        HandleDrawLineFromSelected();
        HandleNodeConnect();
        HandleNodeIoSelection();
        HandleLineUpdate();
    }

    public void DEMO_SAVE()
    {
        List<NexusItemInformation> itemInformations = new List<NexusItemInformation>();
        foreach (NodeElement element in _nodeElements)
        {
            Godot.Collections.Dictionary<string, Variant> staticValues = new Godot.Collections.Dictionary<string, Variant>();
            foreach (NexusStatic nexusStatic in element.Node.GetAllStatics())
            {
                //staticValues.Add(nexusStatic.Label, (Variant)nexusStatic.Value);
            }
            
            itemInformations.Add(new NexusItemInformation()
            {
                GlobalPosition = element.GlobalPosition,
                NexusOption = element.Option,
                UUID = element.Node.GetUUID(),
                //StaticStates = staticValues TODO -- need to be able to set this and retrieve this as a value even if its a list
            });   
        }
        new NexusSerializer().Save(itemInformations);
    }
    
    public void HandleNodeCreate(NexusOption option)
    {
        // should add this to a specific container but this is fine for now.
        NodeElement element = _nodeElementScene.Instantiate<NodeElement>();
        element.Position = _lineRenderer.GlobalPosition - element.Size / 2;
        switch (option)
        {
            case NexusOption.MATH: 
                element.Node = new MathNexus();
                element.Option = NexusOption.MATH;
                break;
            case NexusOption.TEXT_DISPLAY:
                element.Node = new OutputNexus();
                element.Option = NexusOption.TEXT_DISPLAY;
                break;
            case NexusOption.NUMBER_INPUT:
                element.Node = new NumberInputNexus();
                element.Option = NexusOption.NUMBER_INPUT;
                break;
            case NexusOption.BOOLEAN_INPUT:
                element.Node = new BooleanNexus();
                element.Option = NexusOption.BOOLEAN_INPUT;
                break;
            case NexusOption.LOGIC:
                element.Node = new LogicNexus();
                element.Option = NexusOption.LOGIC;
                break;
            case NexusOption.BYTE:
                element.Node = new ByteNexus();
                element.Option = NexusOption.BYTE;
                break;
        }
        AddChild(element);
        _nodeElements.Add(element);
    }

    public void RemoveAllConnectionsFromElement(NodeElement element)
    {
        List<ConnectedIO> filtered = _connections.Where(item => item.To.Element != element && item.From.Element != element).ToList();
        List<ConnectedIO> toBeRemoved = _connections.Except(filtered).ToList();

        foreach (ConnectedIO io in toBeRemoved)
        {
            _lineRenderer.RemoveLine(io.ConnectionName);
            Nexus.Nexus.DisconnectIO(io.To.Element.Node,io.To.Io.GetLabelName());
            
        }

        _nodeElements.Remove(element);
        _connections = filtered;
    }

    private void HandleDrawLineFromSelected()
    {
        if (SelectedIo != null)
        {
            HandleInputIsSelected();
            Vector2 offset = SelectedIo.Io.GetMarker().Size / 2;
            _lineRenderer.CreateOrUpdate("mouseLine", new DrawLine()
            {
                From = SelectedIo.Io.GetMarker().GlobalPosition + offset,
                To = GetGlobalMousePosition(),
                Color = GetLineColor()
            });
        }
        else
        { 
            _lineRenderer.RemoveLine("mouseLine");
        }
    }

    private void HandleInputIsSelected()
    {
        if (SelectedIo.IsInput)
            RemoveAllConnectionsToInput(SelectedIo, true);
    }

    private void HandleNodeConnect()
    {
        if (
            SelectedIo != null 
            && HoveredIo != null
            && SelectedIo != HoveredIo
            && SelectedIo.Element != HoveredIo.Element
            && !Input.IsMouseButtonPressed(MouseButton.Left)
        )
        {
            try
            {
                // handle connecting input to an output
                if (SelectedIo.IsInput && !HoveredIo.IsInput && !IsRecursiveConnection(HoveredIo, SelectedIo))
                {
                    Nexus.Nexus.ConnectIO(
                        HoveredIo.Element.Node,
                        SelectedIo.Element.Node,
                        HoveredIo.Io.GetLabelName(),
                        SelectedIo.Io.GetLabelName()
                    );
                    
                    _connections.Add(new ConnectedIO()
                    {
                        From = HoveredIo,
                        To = SelectedIo
                    });
                }
                // handle connecting output to input
                else if (!SelectedIo.IsInput && HoveredIo.IsInput && !IsRecursiveConnection(SelectedIo, HoveredIo))
                {
                    // TODO: fix bug here if connected io causes recursive loop
                    RemoveAllConnectionsToInput(HoveredIo);
                    Nexus.Nexus.ConnectIO(
                        SelectedIo.Element.Node,
                        HoveredIo.Element.Node,
                        SelectedIo.Io.GetLabelName(),
                        HoveredIo.Io.GetLabelName()
                    );
                    
                    _connections.Add(new ConnectedIO()
                    {
                        From = SelectedIo,
                        To = HoveredIo
                    });
                }
            }
            catch (Exception e)
            {
                GD.Print("UNABLE TO CONNECT NODES: " + e);
            }
        }   
    }

    private void HandleLineUpdate()
    {
        foreach (var connected in _connections)
        {
            _lineRenderer.CreateOrUpdate(connected.ConnectionName, connected.ProduceLine());
        }
    }

    private void HandleNodeIoSelection()
    {
        if (Input.IsMouseButtonPressed(MouseButton.Left) && !IsMovingCamera)
        {
            if (HoveredIo != null && SelectedIo == null)
            {
                SelectedIo = HoveredIo;
            }
            return;
        }
        SelectedIo = null;
    }
    
    private void RemoveAllConnectionsToInput(NodeIoInformation input, bool setSelectedToOutput = false)
    {
        List<ConnectedIO> connectedIos = _connections.Where(
            item => item.To.Element == input.Element && item.To.Io == input.Io
        ).ToList();

        if (connectedIos.Count > 0)
        {
            foreach (var io in connectedIos)
            {
                if (setSelectedToOutput)
                    SelectedIo = new NodeIoInformation(io.From.Element, io.From.Io, io.From.IsInput);
                
                Nexus.Nexus.DisconnectIO(io.To.Element.Node, io.To.Io.GetLabelName());
                _lineRenderer.RemoveLine(io.ConnectionName);
                _connections.Remove(io);
            }
        }
    }
    
    private bool IsRecursiveConnection(NodeIoInformation ioInfo, NodeIoInformation connectionIo)
    {
        Queue<NodeIoInformation> queue = new Queue<NodeIoInformation>();
        ConnectedIO temp = new ConnectedIO()
        {
            From = ioInfo,
            To = connectionIo
        };
        _connections.Add(temp);
        
        foreach (ConnectedIO io in GetConnectionsFrom(ioInfo))
        {
            queue.Enqueue(io.To);
        }

        while (queue.Count > 0)
        {
            NodeIoInformation ioInformation = queue.Dequeue();
            if (ioInformation.Element == ioInfo.Element)
            {
                _connections.Remove(temp);
                return true;
            };
            foreach (ConnectedIO io in GetConnectionsFrom(ioInformation))
            {
                queue.Enqueue(io.To);
            }
        }
        
        _connections.Remove(temp);
        return false;
    }

    private Color GetLineColor()
    {
        if (SelectedIo == null || HoveredIo == null) return Colors.Aqua;
        if (SelectedIo.IsInput && !HoveredIo.IsInput && !IsRecursiveConnection(HoveredIo, SelectedIo)) return Colors.Green;
        if (!SelectedIo.IsInput && HoveredIo.IsInput && !IsRecursiveConnection(SelectedIo, HoveredIo)) return Colors.Green;
        return Colors.Red;
    }

    private List<ConnectedIO> GetConnectionsFrom(NodeIoInformation ioInformation)
    {
        return _connections.Where(item => item.From.Element == ioInformation.Element).ToList();
    }
}