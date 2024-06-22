namespace Nexus.Elements.Display;

public class OutputNexus : NexusBase
{
    public readonly NexusStatic OutputDisplay;
    public NexusInput<Double> DisplayInput { get; set; }
    public NexusOutput<Double> Output { get; set; }

    public OutputNexus()
    {
        DisplayInput = new NexusInput<Double>(() => 0);
        Output = new NexusOutput<Double>(() => DisplayInput.Value);
        OutputDisplay = new NexusStatic(DisplayInput.Value.ToString());
    }
}