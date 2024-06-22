using Godot;
using Godot.Collections;

namespace GNodes;

public partial class LineRenderer : Control
{
    private Dictionary<string, DrawLine> _lines = new();
    public override void _Draw()
    {
        foreach (string key in _lines.Keys)
        {
            DrawLine line = _lines[key];
            DrawLine(line.From, line.To, line.Color, (float)line.Size);
        }
    }

    public void AddLine(string id, DrawLine line)
    {
        _lines.Add(id, line);
        QueueRedraw();
    }

    public void CreateOrUpdate(string id, DrawLine line)
    {
        if (_lines.ContainsKey(id))
        {
            UpdateLine(id, line);
            return;
        }
        AddLine(id, line);
    }

    public void UpdateLine(string id, DrawLine line)
    {
        if (!_lines.ContainsKey(id))
        {
            GD.Print("Line does not exist with key: " + id);
            return;
        }
        _lines[id] = line;
        QueueRedraw();
    }

    public void RemoveLine(string id)
    {
        if (!_lines.ContainsKey(id))
        {
            GD.Print("Line does not exist with key: " + id);
            return;
        }

        _lines.Remove(id);
        QueueRedraw();
    }
}