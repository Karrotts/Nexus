
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Nexus.Godot.Scripts.EZSaver;

namespace Nexus.Godot.Scripts.File;

public class NexusSerializer
{
    public void Save(List<NexusItemInformation> items, List<NexusConnectionInformation> connections)
    {
        EzSaver saver = new EzSaver("user://nexus.save");

        Array<global::Godot.Collections.Dictionary<string, Variant>> saveItems =
            new Array<global::Godot.Collections.Dictionary<string, Variant>>();
        
        foreach (var item in items)
        {
            saveItems.Add(item.GetAsJson());
        }

        saver.SaveValue("items",saveItems);
    }

    public List<NexusItemInformation> Load()
    {
        EzSaver saver = new EzSaver("user://nexus.save");
        saver.LoadValue("items");
        Array<global::Godot.Collections.Dictionary<string, Variant>> saveItems = 
            (Array<global::Godot.Collections.Dictionary<string, Variant>>)saver.LoadValue("items");

        List<NexusItemInformation> items = new List<NexusItemInformation>();
        foreach (var item in saveItems)
        {
            items.Add(new NexusItemInformation(item));
        }

        return items;
    }
}