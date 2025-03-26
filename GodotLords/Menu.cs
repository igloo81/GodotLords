using Godot;
using System;
using System.Collections.Generic;

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
		var mapScene = (MapNode)((PackedScene)ResourceLoader.Load("res://node_2d.tscn")).Instantiate();
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
		mapScene.GameData = new GameData() { 
			Map = map,
			Units = new List<Unit> { 
				new Unit { Id = "1st unit", MovesLeft = 10, MovesMaximum = 10, PlayerId = "joost", Strength = 4, unitTypeEnum = UnitTypeEnum.Knight, Upkeep = 1 },
				new Unit { Id = "2nd unit", MovesLeft = 10, MovesMaximum = 10, PlayerId = "joost", Strength = 4, unitTypeEnum = UnitTypeEnum.Demon, Upkeep = 1 },
				new Unit { Id = "3rd unit", MovesLeft = 10, MovesMaximum = 10, PlayerId = "joost", Strength = 4, unitTypeEnum = UnitTypeEnum.Dwarf, Upkeep = 1 }
				},
			UnitsOnMap = new Dictionary<Vector2I, string[]>{
						{ new Vector2I(10, 10), new string[] { "1st unit" }},
						{ new Vector2I(12, 10), new string[] { "2nd unit", "3rd unit" }}
					}
			};
		GetTree().Root.AddChild(mapScene);
		GetTree().CurrentScene = mapScene;	// todo does this keep adding extra scenes?
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}	
}
