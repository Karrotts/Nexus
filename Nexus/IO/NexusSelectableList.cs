using Nexus.Exceptions;

namespace Nexus;

public class NexusSelectableList<T> : ISelectableList
{
    public T Selected
    {
        get => _selected;
        set
        {
            if (Options.Contains(value))
            {
                _selected = value;
                return;
            }

            throw new NexusIOValueDoesNotExistException($"Selected value \"{value}\"` does not exist in options");
        }
    }

    public List<T> Options
    {
        get => _options;
    }

    private T _selected;
    private List<T> _options;
    
    public NexusSelectableList(List<T> options)
    {
        SetOptions(options);
        Selected = options[0];
    }

    public NexusSelectableList(List<T> options, T defaultSelected)
    {
        SetOptions(options);
        Selected = defaultSelected;
    }

    public List<string?> OptionsAsStringList()
    {
        return Options.Select(obj =>
        {
            if (obj is Enum)
            {
                return Enum.GetName(obj.GetType(), obj); // Get the enum name
            }
            return obj?.ToString(); // Get the string representation
        }).Where(obj => obj != null).ToList();
    }

    private void SetOptions(List<T> options)
    {
        if (options.Count == 0)
            throw new Exception("Selectable list must contain at least one option, instead found none. Did you initialize this list?");
        _options = options;
        
    }

    public List<string> GetOptions()
    {
        return OptionsAsStringList();
    }

    public int GetIndexOfSelected()
    {
        return Options.IndexOf(Selected);
    }

    public void SetSelected(string value)
    {
        foreach (var option in Options)
        {
            if (Enum.GetName(typeof(T), option) == value)
            {
                Selected = option;
                return;
            }

            if (option.ToString() == value)
            {
                Selected = (T)Convert.ChangeType(value, typeof(T));
                return;
            }
        }
    }
    
    public void SetSelected(int index)
    {
        if (index < 0 || index >= Options.Count)
            throw new Exception("Option was not in the selectable range.");
        Selected = Options[index];
    }

    public string GetSelected()
    {
        if (Selected is Enum)
        {
            return Enum.GetName(Selected.GetType(), Selected);
        }
        return Selected.ToString();
    }
}