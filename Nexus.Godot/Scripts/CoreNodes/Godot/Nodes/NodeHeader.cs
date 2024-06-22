using Godot;
using System;


namespace Nexus.Godot.UI
{
	public partial class NodeHeader : Control
	{
		public void SetHeaderText(string text)
		{
			GetNode<Label>("Label").Text = text;
		}
	}	
}
