namespace Nexus.Elements.Display;

public class OutputNexus : NexusBase
{
    public readonly NexusDisplay OutputDisplay;
    public NexusInput<string> DisplayInput { get; set; }

    public OutputNexus()
    {
        DisplayInput = new NexusInput<string>(() => "");
        OutputDisplay = new NexusDisplay(() => DisplayInput.Value);
    }
}