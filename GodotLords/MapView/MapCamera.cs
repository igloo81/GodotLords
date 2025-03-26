using Godot;
using System;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class MapCamera : Camera2D
{
	private Vector2 zoomTarget;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.MoveLocalX(10*50);
		this.MoveLocalY(10*50);
		zoomTarget = Zoom;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var movementSpeed = 1/Zoom.Length() * 10;
		if (Input.IsActionPressed("camera_move_left"))
			this.MoveLocalX(-movementSpeed);
		if (Input.IsActionPressed("camera_move_right"))
			this.MoveLocalX(movementSpeed);
		if (Input.IsActionPressed("camera_move_up"))
			this.MoveLocalY(-movementSpeed);
		if (Input.IsActionPressed("camera_move_down"))
			this.MoveLocalY(movementSpeed);

		if (Input.IsActionJustPressed("camera_zoom_in"))
			zoomTarget *= 1.1f;
		if (Input.IsActionJustPressed("camera_zoom_out"))
			zoomTarget *= 0.9f;
		Zoom = Zoom.Slerp(zoomTarget, .5f);
	}
}
