using Godot;
using System;
using GNodes.UI.Scripts;

namespace GNodes.UI
{
    public partial class NodeOutput : Control, INodeInputOutputUI
    {
        public string SlotName;
        public ColorRect GetMarker()
        {
            return GetNode<ColorRect>("OutputPoint");
        }

        public void SetDetails(string labelText, Type type)
        {
            SlotName = labelText;
            GetNode<Label>("OutputLabel").Text = labelText;
            GetNode<ColorRect>("OutputPoint").Color = TypeColors.GetTypeColor(type);
        }
    }	
}