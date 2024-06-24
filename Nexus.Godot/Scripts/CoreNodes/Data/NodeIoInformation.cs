using Nexus.Godot.UI;

namespace Nexus.Godot;

public class NodeIoInformation(NodeElement element, INodeInputOutputUI io, bool isInput)
{
    public NodeElement Element { get; } = element;
    public INodeInputOutputUI Io { get; } = io;
    public bool IsInput { get; } = isInput;
}