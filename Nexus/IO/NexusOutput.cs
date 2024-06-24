namespace Nexus;

public class NexusOutput<T>(INexusIO.GetterDelegate getter) : NexusIO<T>(getter)
{
    
}