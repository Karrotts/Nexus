using System.Linq;
using Godot;
using Godot.Collections;

namespace Nexus.Godot;

public partial class LineRenderer : Control
{
    private Dictionary<string, DrawLine> _lines = new();

    public override void _Ready()
    {
        // DEBUG CUBE --- enable this if for some reason the lines disappear, if this cube is offscreen the lines
        // will not render at all due to the visibility of the control node.
        
        // ColorRect debugRect = new ColorRect();
        // debugRect.Size = new Vector2(10, 10);
        // AddChild(debugRect);
    }

    public override void _Draw()
    {
        Dictionary<Color, Array<Vector2>> lines = new Dictionary<Color, Array<Vector2>>();
        foreach (string key in _lines.Keys)
        {
            DrawLine line = _lines[key];
            if (lines.TryGetValue(line.Color, out Array<Vector2> points))
            {
                points.Add(line.From - Position);
                points.Add(line.To - Position);
                lines[line.Color] = points;
            }
            else
            {
                points = new Array<Vector2>();
                points.Add(line.From - Position);
                points.Add(line.To - Position);
                lines[line.Color] = points;
            }
        }

        foreach (var multiline in lines.Keys)
        {
            DrawMultiline(lines[multiline].ToArray(), multiline, 4f);
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
        if (!_lines.ContainsKey(id)) return;
        _lines.Remove(id);
        QueueRedraw();
    }
}