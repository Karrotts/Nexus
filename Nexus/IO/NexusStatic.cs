namespace Nexus;

public class NexusStatic(object value, string label = "")
{
    public object Value = value;
    public Type Type = value.GetType();
    public string Label = label;

    public bool IsListType
    {
        get => Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(NexusSelectableList<>);
    }

    public ISelectableList GetList()
    {
        if (IsListType) return (ISelectableList)value;
        throw new Exception("Static element is not a selectable list type!");
    }
}