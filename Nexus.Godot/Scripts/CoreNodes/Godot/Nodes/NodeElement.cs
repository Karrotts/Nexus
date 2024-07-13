using Godot;
using System;
using System.Collections.Generic;
using Nexus;
using Nexus.Elements.Basic;
using Nexus.Elements.Boolean;
using Nexus.Elements.Display;

namespace Nexus.Godot.UI
{
	public partial class NodeElement : PanelContainer
	{
		[Export] 
		public PackedScene NodeHeader;
		[Export] 
		public PackedScene NodeInput;
		[Export] 
		public PackedScene NodeOutput;
		[Export] 
		public PackedScene SelectListElement;
		[Export] 
		public PackedScene DisplayElement;
		[Export] 
		public PackedScene NumberInput;
		[Export] 
		public PackedScene BooleanInput;

		private LineRenderer _lineRenderer;
		private NodeManager _nodeManager;
		private bool _headerHovered;
		private bool _isDragging;
		private Vector2 _mouseOffset;
		private List<NodeInput> _inputs;
		private List<NodeOutput> _outputs;
	
		public INexus Node;
		public NexusOption Option;
		
		public override void _Ready()
		{
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
			CreateDisplays(container);
			CreateStatics(container);
			CreateOutputs(container);
			CreateInputs(container);
		}
	
		public override void _Process(double delta)
		{
			HandleDrag();
			HandleDelete();
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

		private void HandleDelete()
		{
			if (_headerHovered && Input.IsActionPressed("ui_cancel"))
			{
				_nodeManager.RemoveAllConnectionsFromElement(this);
				QueueFree();
			}
		}

		private void HandleDrag()
		{
			if (_nodeManager.IsMovingCamera) return;
			Vector2 mousePos = GetGlobalMousePosition();
			if (_headerHovered && Input.IsMouseButtonPressed(MouseButton.Left) && !_isDragging && _nodeManager.CurrentlyDraggingNode == null)
			{
				_isDragging = true;
				_mouseOffset = mousePos - GlobalPosition;
				_nodeManager.CurrentlyDraggingNode = GetInstanceId();
			}

			if (Input.IsMouseButtonPressed(MouseButton.Left) && _isDragging && _nodeManager.CurrentlyDraggingNode == GetInstanceId())
			{
				GlobalPosition = mousePos - _mouseOffset;
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
				input.GetMarker().MouseEntered += () => _nodeManager.HoveredIo = new NodeIoInformation(this, input, true);
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
				output.GetMarker().MouseEntered += () => _nodeManager.HoveredIo = new NodeIoInformation(this, output, false);
				output.GetMarker().MouseExited += () => _nodeManager.HoveredIo = null;
				container.AddChild(output);
			}
		}

		private void CreateStatics(VBoxContainer container)
		{
			foreach (NexusStatic staticItem in Node.GetAllStatics())
			{
				if (staticItem.IsListType)
				{
					CreateListStatic(staticItem.GetList(), container);
					continue;
				}

				if (staticItem.Type == typeof(double))
				{
					NumberInput numberInput = NumberInput.Instantiate<NumberInput>();
					numberInput.GetSpinBox().ValueChanged += value => staticItem.Value = value;
					container.AddChild(numberInput);
				}

				if (staticItem.Type == typeof(bool))
				{
					BooleanInput booleanInput = BooleanInput.Instantiate<BooleanInput>();
					booleanInput.GetCheckButton().Toggled += (on => staticItem.Value = on);
					booleanInput.GetCheckButton().Text = staticItem.Label;
					container.AddChild(booleanInput);
				}
			}
		}

		private void CreateDisplays(VBoxContainer container)
		{
			foreach (var display in Node.GetAllDisplays())
			{
				DisplayElement displayElement = DisplayElement.Instantiate<DisplayElement>();
				displayElement.SetDisplay(() => display.Value);
				container.AddChild(displayElement);	
			}
		}

		private void CreateListStatic(ISelectableList selectableList, VBoxContainer container)
		{
			SelectListElement selectListElement = SelectListElement.Instantiate<SelectListElement>();
			selectListElement.SetOptions(selectableList.GetOptions(), selectableList.GetIndexOfSelected());
			selectListElement.GetOptionsButton().ItemSelected += (selected) => selectableList.SetSelected((int)selected);
			container.AddChild(selectListElement);
		}
	}
	
	public class NodeElementBuilder(NodeElement element)
	{
		private NexusOption _option = NexusOption.MATH;
		private Vector2 _position = new Vector2();

		public NodeElementBuilder SetOption(NexusOption option)
		{
			_option = option;
			return this;
		}

		public NodeElementBuilder SetPosition(Vector2 position)
		{
			_position = position;
			return this;
		}

		public void Build()
		{
			element.Position = _position;
			switch (_option)
			{
				case NexusOption.MATH: 
					element.Node = new MathNexus();
					element.Option = _option;
					break;
				case NexusOption.TEXT_DISPLAY:
					element.Node = new OutputNexus();
					element.Option = _option;
					break;
				case NexusOption.NUMBER_INPUT:
					element.Node = new NumberInputNexus();
					element.Option = _option;
					break;
				case NexusOption.BOOLEAN_INPUT:
					element.Node = new BooleanNexus();
					element.Option = _option;
					break;
				case NexusOption.LOGIC:
					element.Node = new LogicNexus();
					element.Option = _option;
					break;
				case NexusOption.BYTE:
					element.Node = new ByteNexus();
					element.Option = _option;
					break;
			}
		}
	}
}
