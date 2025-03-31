using Godot;
using System;

namespace GodotLords.MapView;

public partial class ArmySelection : Control
{
    public void SetArmy(string armyId)
    {
        GetNode<Label>("%ArmyNameLabel").Text = armyId;
    }
}
