namespace Nexus;

public class NexusOutput<T>(NexusIO<T>.GetValue getter) : NexusIO<T>(getter)
{
    
}