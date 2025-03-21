using Godot;
using System.Collections.Generic;

public partial class UnitsMapLayer : TileMapLayer
{

    public override void _Ready()
    {
		this.SetCell(new Vector2I(10, 10), 0, new Vector2I(0, 0));
    }   
} 
