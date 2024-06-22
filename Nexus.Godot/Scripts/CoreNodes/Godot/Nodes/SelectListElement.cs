using Godot;
using System;
using System.Collections.Generic;
using Nexus;

public partial class SelectListElement : Control
{
    public void SetOptions(List<String> options, int defaultSelected)
    {
        OptionButton optionButton = GetNode<OptionButton>("Container/OptionButton");
        foreach (var option in options)
        {
            optionButton.AddItem(option);
        }
        optionButton.Selected = defaultSelected;
    }

    public OptionButton GetOptionsButton()
    {
        return GetNode<OptionButton>("Container/OptionButton");
    }
}
