namespace Nexus;

public class NexusStatic(object value)
{
    public object Value = value;
    public Type Type = value.GetType();

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