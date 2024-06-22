using Godot;
using System;
using Nexus.Godot.UI.Scripts;

namespace Nexus.Godot.UI
{
	public partial class NodeInput : Control, INodeInputOutputUI
	{
		public string SlotName;
		
		public ColorRect GetMarker()
		{
			return GetNode<ColorRect>("InputPoint");
		}

		public void SetDetails(string labelText, Type type)
		{
			SlotName = labelText;
			GetNode<Label>("InputLabel").Text = labelText;
			GetNode<ColorRect>("InputPoint").Color = TypeColors.GetTypeColor(type);
		}
	}	
}
