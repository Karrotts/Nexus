namespace Nexus.Elements.Basic;

public class NumberInputNexus : NexusBase
{
    public NexusStatic InputNumber;
    public NexusOutput<double> NumberOutput { get; set; }

    public NumberInputNexus()
    {
        InputNumber = new NexusStatic(0D);
        NumberOutput = new NexusOutput<double>(() => (double)InputNumber.Value);
    }
}