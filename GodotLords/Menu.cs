using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotLords.MapView;

using GodotLords.Engine;
using GodotLords.GameWindow;

public partial class Menu : Control
{
	[Export] public Color HoverColor = new Color(0.2f, 0.2f, 0.2f); // Lighter color on hover

	public override void _Ready()
	{
		var startButton = GetNode<Button>("Menu/Start");
		startButton.Pressed += OnStartPressed;

		var quitButton = GetNode<Button>("Menu/Quit");
		quitButton.Pressed += OnQuitPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnStartPressed()
    {
        var gameData = CreateTestGameData();
        var mapScene = (GameWindow)((PackedScene)ResourceLoader.Load("res://GameScreen.tscn")).Instantiate();
        mapScene.GameData = gameData;
        GetTree().Root.AddChild(mapScene);
        GetTree().CurrentScene = mapScene;  // todo does this keep adding extra scenes?
		GetTree().Root.RemoveChild(this);
    }

    private static GameData CreateTestGameData()
    {
        var map = Map.FromImage(
            "Resources/map.png", 2,
            new Dictionary<Color, TerrainType>{
                { Color.FromHtml("#0053c9"), TerrainType.Water},
                { Color.FromHtml("#217725"), TerrainType.Grass},
                { Color.FromHtml("#555555"), TerrainType.Road},
                { Color.FromHtml("#9a5600"), TerrainType.Hill},
                { Color.FromHtml("#693500"), TerrainType.Mountain},
                { Color.FromHtml("#005500"), TerrainType.Forest},
            }
            );

        var units = new List<Unit> {
                new Unit("1st unit", 10, 10, "joost", 4, UnitTypeEnum.Knight, 1 ),
                new Unit("2nd unit", 10, 10, "joost", 4, UnitTypeEnum.LightInfantry,  1 ),
                new Unit("3rd unit", 10, 10, "joost", 4, UnitTypeEnum.Dwarf, 1 )
                };

        var unitsOnMap = new Dictionary<Vector2I, string[]>{
                        { new Vector2I(10, 10), new string[] { "1st unit" }},
                        { new Vector2I(12, 10), new string[] { "2nd unit", "3rd unit" }}
                    };

        AddOneOfEachUnitType(units, unitsOnMap);

        var gameData = new GameData()
        {
            Map = map,
            Units = units,
            UnitsOnMap = unitsOnMap,
            Cities = new List<City> { new City("test city", 10, 10) }
        };
        return gameData;
    }


    private static void AddOneOfEachUnitType(List<Unit> units, Dictionary<Vector2I, string[]> unitsOnMap)
    {
        var index = 0;
        foreach (var a in Enum.GetValues<UnitTypeEnum>())
        {
            var unit = new Unit($"test sprites, unit type {a}", 10, 10, $"test {a}", 5, a, 1);
            units.Add(unit);
            unitsOnMap[new Vector2I(2 + index % 4, 2 + index / 4)] = new string[] { unit.Id };
            index++;
        }
    }

    public void OnQuitPressed()
	{
		GetTree().Quit();
	}	
}
