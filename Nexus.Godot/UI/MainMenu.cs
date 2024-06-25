using Godot;
using System;
using Nexus.Godot;

public partial class MainMenu : Control
{
	private MenuButton _numberDropdown;
	private MenuButton _booleanDropdown;
	private MenuButton _inputDropdown;
	private MenuButton _outputDropdown;
	private NodeManager _nodeManager;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeManager = GetNode<NodeManager>("/root/NodeManager");
		_numberDropdown = GetNode<MenuButton>("HBoxContainer/NumberDropdown");
		_booleanDropdown = GetNode<MenuButton>("HBoxContainer/BooleanDropdown");
		_inputDropdown = GetNode<MenuButton>("HBoxContainer/InputDropdown");
		_outputDropdown = GetNode<MenuButton>("HBoxContainer/OutputDropdown");
		
		_numberDropdown.GetPopup().IdPressed += (id =>
		{
			switch (id)
			{
				case 0:
					_nodeManager.HandleNodeCreate(NexusOption.MATH);
					break;
			}
		});

		_booleanDropdown.GetPopup().IdPressed += (id =>
		{
			switch (id)
			{
				case 0:
					_nodeManager.HandleNodeCreate(NexusOption.LOGIC);
					break;
				case 1:
					_nodeManager.HandleNodeCreate(NexusOption.BYTE);
					break;
			}
		});
		
		_inputDropdown.GetPopup().IdPressed += (id =>
		{
			switch (id)
			{
				case 0:
					_nodeManager.HandleNodeCreate(NexusOption.NUMBER_INPUT);
					break;
				case 1:
					_nodeManager.HandleNodeCreate(NexusOption.BOOLEAN_INPUT);
					break;
			}
		});
		
		_outputDropdown.GetPopup().IdPressed += (id =>
		{
			switch (id)
			{
				case 0:
					_nodeManager.HandleNodeCreate(NexusOption.TEXT_DISPLAY);
					break;
			}
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
