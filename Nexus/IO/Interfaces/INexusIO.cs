namespace Nexus;

public interface INexusIO
{
    public string GetDisplayValue();
    public object GetValue();
    public void SetGetter(GetterDelegate getter);
    public object CastToType(Type type);
    public Type GetGenericType();

    public delegate object GetterDelegate();
}