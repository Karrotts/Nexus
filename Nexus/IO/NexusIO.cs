namespace Nexus;

public class NexusIO<T>(NexusIO<T>.GetValue getter)
{
    public delegate T GetValue();

    public GetValue Getter => getter;
    public T Value => Getter();
}