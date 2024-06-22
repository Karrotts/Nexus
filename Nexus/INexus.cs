namespace Nexus;

public interface INexus
{
    void SetInstanceProperty<TPropertyType>(string propertyName, TPropertyType value);
    object? GetPropValue(string propName);
    Dictionary<string, Type> GetInputs();
    Dictionary<string, Type> GetOutputs();
    List<NexusStatic> GetAllStatics();

}