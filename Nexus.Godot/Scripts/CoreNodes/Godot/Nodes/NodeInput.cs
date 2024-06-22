using Godot;
using System;
using GNodes.UI.Scripts;

namespace GNodes.UI
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
