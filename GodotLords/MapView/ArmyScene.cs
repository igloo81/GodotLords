using Godot;
using System;
using System.Linq;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class ArmyScene : Node2D
{
	public Unit[] Units { get; set; }
	private static int tileSize = 32;	// todo share..

	public override void _Ready()
	{
		var tileSheet = GD.Load<Texture2D>("res://Resources/units.png");	// todo static/share

		var unitTypeToShow = GetUnitTypeToShow(Units.Select(_ => _.unitTypeEnum).ToArray());
		var offset = GetOffsetInTileSheet(unitTypeToShow);
        var texture = new AtlasTexture() 
		{ 
			Atlas = tileSheet, 
			Region = new Rect2(offset.X * tileSize, offset.Y * tileSize, tileSize, tileSize)
		};

        var sprite  = new Sprite2D();
        sprite.Texture = texture;
        AddChild(sprite);
	}

    public override void _Draw()
    {
		DrawRect(new Rect2(-tileSize/2, -tileSize/2, 4 * Units.Length, 5), new Color(1, 0, 0));
    }


    private static Vector2I GetOffsetInTileSheet(UnitTypeEnum unitTypeEnum) => unitTypeEnum switch	// todo clean up with tile size..
    {
        UnitTypeEnum.LightInfantry => new Vector2I(0, 0),
        UnitTypeEnum.HeavyInfantry => new Vector2I(1, 0),
        UnitTypeEnum.Giant => new Vector2I(2, 0),
        UnitTypeEnum.Archer => new Vector2I(3, 0),

        UnitTypeEnum.WolfRider => new Vector2I(0, 1),
        UnitTypeEnum.Cavalry => new Vector2I(1, 1),
        UnitTypeEnum.Pegasus => new Vector2I(2, 1),
        UnitTypeEnum.Griffin => new Vector2I(3, 1),

        UnitTypeEnum.Dwarf => new Vector2I(0, 2),
        UnitTypeEnum.Navy => new Vector2I(1, 2),
        UnitTypeEnum.Ghost => new Vector2I(2, 2),
        UnitTypeEnum.Demon => new Vector2I(3, 2),

        UnitTypeEnum.Devil => new Vector2I(0, 3),
        UnitTypeEnum.Wizard => new Vector2I(1, 3),
        UnitTypeEnum.Dragon => new Vector2I(2, 3),
        UnitTypeEnum.Knight => new Vector2I(3, 3),

        _ => throw new NotImplementedException()
    };

    private static UnitTypeEnum GetUnitTypeToShow(UnitTypeEnum[] unitTypes)
    {

        if (unitTypes.Length == 0)
            throw new Exception($"Can't determine which unit to show if not unitTypes are passed");

         return unitTypes.OrderByDescending(_ => GetOffsetInTileSheet(_).X + GetOffsetInTileSheet(_).Y * 3).First(); // todo compute twice :-)
    }	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
