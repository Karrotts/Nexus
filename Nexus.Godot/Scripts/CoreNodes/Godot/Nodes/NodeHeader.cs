using Godot;
using System;


namespace GNodes.UI
{
	public partial class NodeHeader : Control
	{
		public void SetHeaderText(string text)
		{
			GetNode<Label>("Label").Text = text;
		}
	}	
}
