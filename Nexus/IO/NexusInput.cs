namespace Nexus;

public class NexusInput<T>(INexusIO.GetterDelegate getter) : NexusIO<T>(getter)
{
}