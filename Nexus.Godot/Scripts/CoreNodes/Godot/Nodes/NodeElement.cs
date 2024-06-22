using Godot;
using System;
using System.Collections.Generic;
using Nodes;
using Nodes.nodes.basic;

namespace GNodes.UI
{
	public partial class NodeElement : PanelContainer
	{
		[Export] 
		public PackedScene NodeHeader;
		[Export] 
		public PackedScene NodeInput;
		[Export] 
		public PackedScene NodeOutput;

		private LineRenderer _lineRenderer;
		private NodeManager _nodeManager;
		private bool _headerHovered;
		private bool _isDragging;
		private Vector2 _mouseOffset;
		private List<NodeInput> _inputs;
		private List<NodeOutput> _outputs;
	
		public INode Node;
		public override void _Ready()
		{
			Node = new AdderNode<int>();
			
			_headerHovered = false;
			_isDragging = false;
			_inputs = new();
			_outputs = new();
			_lineRenderer = GetNode<LineRenderer>("/root/LineRenderer");
			_nodeManager = GetNode<NodeManager>("/root/NodeManager");
			
			if (Node == null)
			{
				GD.Print("Node element is null, must provide a node class to the Node Element");
				return;
			}
			
			VBoxContainer container = GetNode<VBoxContainer>("Container");
			CreateHeader(container);
			CreateOutputs(container);
			CreateInputs(container);
		}
	
		public override void _Process(double delta)
		{
			HandleDrag();
		}

		public NodeInput GetInput(string inputLabel)
		{
			foreach (NodeInput input in _inputs)
			{
				if (input.SlotName == inputLabel) return input;
			}
			throw new Exception("Input not found with label: " + inputLabel);
		}
		
		public NodeOutput GetOutput(string outputLabel)
		{
			foreach (NodeOutput output in _outputs)
			{
				if (output.SlotName == outputLabel) return output;
			}
			throw new Exception("Output not found with label: " + outputLabel);
		}

		private void HandleDrag()
		{
			Vector2 mousePos = GetViewport().GetMousePosition();
			if (_headerHovered && Input.IsMouseButtonPressed(MouseButton.Left) && !_isDragging && _nodeManager.CurrentlyDraggingNode == null)
			{
				_isDragging = true;
				_mouseOffset = mousePos - Position;
				_nodeManager.CurrentlyDraggingNode = GetInstanceId();
			}

			if (Input.IsMouseButtonPressed(MouseButton.Left) && _isDragging && _nodeManager.CurrentlyDraggingNode == GetInstanceId())
			{
				Position = mousePos - _mouseOffset;
				return;
			}

			_nodeManager.CurrentlyDraggingNode = _nodeManager.CurrentlyDraggingNode == GetInstanceId() ? null : _nodeManager.CurrentlyDraggingNode;
			_isDragging = false;
		}

		private void CreateHeader(VBoxContainer container)
		{
			NodeHeader header = (NodeHeader) NodeHeader.Instantiate();
			header.SetHeaderText(Node.GetType().Name);
			header.MouseEntered += () => _headerHovered = true;
			header.MouseExited += () => _headerHovered = false;
			container.AddChild(header);
		}

		private void CreateInputs(VBoxContainer container)
		{
			foreach (String s in Node.GetInputs().Keys) 
			{
				NodeInput input = NodeInput.Instantiate<NodeInput>();
				input.SetDetails(s, Node.GetInputs()[s]);
				_inputs.Add(input);
				input.GetMarker().MouseEntered += () => _nodeManager.HoveredIo = new NodeIoInformation(this, s, true);
				input.GetMarker().MouseExited += () => _nodeManager.HoveredIo = null;
				container.AddChild(input);
			}
		}

		private void CreateOutputs(VBoxContainer container)
		{
			foreach (String s in Node.GetOutputs().Keys) 
			{
				NodeOutput output = NodeOutput.Instantiate<NodeOutput>();
				output.SetDetails(s, Node.GetOutputs()[s]);
				_outputs.Add(output);
				output.GetMarker().MouseEntered += () => _nodeManager.HoveredIo = new NodeIoInformation(this, s, false);
				output.GetMarker().MouseExited += () => _nodeManager.HoveredIo = null;
				container.AddChild(output);
			}
		}
	}
}
