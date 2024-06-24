namespace Nexus;

public class NexusIO<T>(INexusIO.GetterDelegate getter) : INexusIO
{
    public INexusIO.GetterDelegate Getter
    {
        get => getter;
        set => getter = value;
    }

    public T Value => (T)Convert.ChangeType(Getter(), typeof(T));
    public string GetDisplayValue()
    {
        return Value.ToString();
    }

    public object GetValue()
    {
        return Value;
    }

    public void SetGetter(INexusIO.GetterDelegate getter)
    {
        Getter = getter;
    }

    public object CastToType(Type type)
    {
        if (type == typeof(T))
            return Value;
        return Convert.ChangeType(Value, type);
    }

    public Type GetGenericType()
    {
        return typeof(T);
    }
}