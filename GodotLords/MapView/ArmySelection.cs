using Godot;
using GodotLords.Engine;
using System;
using System.Linq;

namespace GodotLords.MapView;

public partial class ArmySelection : Control
{
    public void SetArmy(Unit[] units)
    {
        if (units.Length == 0)
            Set("", "", "", "");
        else if (units.Length == 1)
            Set("playerName", units[0].Id, units[0].MovesLeft.ToString(), units[0].Strength.ToString());
        else
            Set("playerName", "Group", units.Min(_ => _.MovesLeft).ToString(), "-");
    }

    public void Set(string playerName, string armyName, string movesLeft, string strength)
    {
        GetNode<Label>("%PlayerNameLabel").Text = playerName;
        GetNode<Label>("%MoveCountLabel").Text = movesLeft;
        GetNode<Label>("%ArmyNameLabel").Text = armyName;
        GetNode<Label>("%StrengthLabel").Text = strength;
    }
}
