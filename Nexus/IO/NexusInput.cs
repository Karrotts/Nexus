namespace Nexus;

public class NexusInput<T>(NexusIO<T>.GetValue getter) : NexusIO<T>(getter)
{
}