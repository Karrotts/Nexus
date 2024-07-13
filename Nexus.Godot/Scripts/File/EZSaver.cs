using Godot;
using Godot.Collections;

namespace Nexus.Godot.Scripts.EZSaver;

public class EzSaver
{
    private string _savePath;
    private Dictionary<string, Variant> _saveState;

    public EzSaver(string savePath)
    {
        _savePath = savePath;
        LoadSaveState();
        _saveState ??= new Dictionary<string, Variant>();
    }

    public bool SaveValue(string key, Variant value)
    {
        using var save = FileAccess.Open(_savePath, FileAccess.ModeFlags.Write);
        _saveState[key] = value;
        var jsonString = Json.Stringify(_saveState);
        save.StoreLine(jsonString);
        return true;
    }

    public Variant? LoadValue(string key)
    {
        LoadSaveState();
        if (_saveState.TryGetValue(key, out Variant value))
            return value;
        return null;
    }

    public bool HasKey(string key)
    {
        return _saveState.ContainsKey(key);
    }

    private void LoadSaveState()
    {
        using var save = FileAccess.Open(_savePath, FileAccess.ModeFlags.Read);
        if (save.GetPosition() < save.GetLength())
        {
            var json = new Json();
            var parseResult = json.Parse(save.GetLine());
            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in at line {json.GetErrorLine()}");
                return;
            }
            _saveState = new Dictionary<string, Variant>((Dictionary)json.Data);
        }
    }
}