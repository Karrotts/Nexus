using Nexus.Godot.UI;

namespace Nexus.Godot;

public class NodeIoInformation(NodeElement element, string ioName, bool isInput)
{
    public NodeElement Element { get; } = element;
    public string IoName { get; } = ioName;
    public bool IsInput { get; } = isInput;
}