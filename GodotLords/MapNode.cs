using Godot;
using System;
using System.Collections.Generic;

public partial class MapNode : Node2D
{
	public Lazy<GameData> GameData = new Lazy<GameData>(() => InitializeMap());

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private static GameData InitializeMap()
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

		return new GameData() { Map = map };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
