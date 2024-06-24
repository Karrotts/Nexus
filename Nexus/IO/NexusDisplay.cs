namespace Nexus;

public class NexusDisplay(NexusDisplay.Getter getter, DisplayFormat format = DisplayFormat.STRING)
{
    public delegate string Getter();
    public Getter DisplayCallback = getter;
    public string Value => FormatString(DisplayCallback(), format);

    private string FormatString(string raw, DisplayFormat format)
    {
        switch (format)
        {
            case DisplayFormat.STRING:
            default: return raw;
        }
    }
}