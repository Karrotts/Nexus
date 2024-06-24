using Godot;
using System;
using Nexus.Godot.UI.Scripts;

namespace Nexus.Godot.UI
{
    public partial class NodeOutput : Control, INodeInputOutputUI
    {
        public string SlotName;
        public ColorRect GetMarker()
        {
            return GetNode<ColorRect>("OutputPoint");
        }

        public string GetLabelName()
        {
            return SlotName;
        }

        public void SetDetails(string labelText, Type type)
        {
            SlotName = labelText;
            GetNode<Label>("OutputLabel").Text = labelText;
            GetNode<ColorRect>("OutputPoint").Color = TypeColors.GetTypeColor(type);
        }
    }	
}