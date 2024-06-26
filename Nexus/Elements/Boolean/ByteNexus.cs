namespace Nexus.Elements.Boolean;

public class ByteNexus : NexusBase
{
    public NexusDisplay OutputDisplay;
    public NexusOutput<double> Output { get; set; }
    public NexusInput<bool> Bit0 { get; set; }
    public NexusInput<bool> Bit1 { get; set; }
    public NexusInput<bool> Bit2 { get; set; }
    public NexusInput<bool> Bit3 { get; set; }
    public NexusInput<bool> Bit4 { get; set; }
    public NexusInput<bool> Bit5 { get; set; }
    public NexusInput<bool> Bit6 { get; set; }
    public NexusInput<bool> Bit7 { get; set; }

    public ByteNexus()
    {
        Bit0 = new NexusInput<bool>(() => false);
        Bit1 = new NexusInput<bool>(() => false);
        Bit2 = new NexusInput<bool>(() => false);
        Bit3 = new NexusInput<bool>(() => false);
        Bit4 = new NexusInput<bool>(() => false);
        Bit5 = new NexusInput<bool>(() => false);
        Bit6 = new NexusInput<bool>(() => false);
        Bit7 = new NexusInput<bool>(() => false);
        Output = new NexusOutput<double>(() => GetByteValue());
        OutputDisplay = new NexusDisplay(() => Output.Value.ToString());
    }

    private double GetByteValue()
    {
        bool[] bitArray = { Bit0.Value, Bit1.Value, Bit2.Value, Bit3.Value, Bit4.Value, Bit5.Value, Bit6.Value, Bit7.Value };
        byte result = 0;
        for (int i = 0; i < 8; i++)
        {
            if (bitArray[i])
            {
                result |= (byte)(1 << i);
            }
        }
        return result;
    }
}