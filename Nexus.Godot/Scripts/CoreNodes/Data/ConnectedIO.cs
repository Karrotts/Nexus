using Godot;

namespace Nexus.Godot;

public class ConnectedIO
{
    public NodeIoInformation To;
    public NodeIoInformation From;

    public string ConnectionName => $"{From.Io.GetLabelName()}({From.Element.Node.GetUUID()})-To-{To.Io.GetLabelName()}({To.Element.Node.GetUUID()})";

    public DrawLine ProduceLine()
    {
        return new DrawLine()
        {
            From = From.Io.GetMarker().GlobalPosition + (From.Io.GetMarker().Size / 2),
            To = To.Io.GetMarker().GlobalPosition + (To.Io.GetMarker().Size / 2),
            Color = Colors.White
        };
    }
}