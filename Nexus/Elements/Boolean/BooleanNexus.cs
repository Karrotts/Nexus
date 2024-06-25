namespace Nexus.Elements.Boolean;

public class BooleanNexus : NexusBase
{
    public NexusStatic BooleanInput;
    public NexusOutput<bool> Output { get; set; }

    public BooleanNexus()
    {
        BooleanInput = new NexusStatic(false, "Toggle");
        Output = new NexusOutput<bool>(() => BooleanInput.Value);
    }
}