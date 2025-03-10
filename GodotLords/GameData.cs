using System;
using Godot;

public static class MapData
{
    public static Lazy<Map> GameMap = new Lazy<Map>(() => Map.FromImage("Resources/map.png", 2, water:Color.FromHtml("#0053c9"), road: Color.FromHtml("#555555")));
}