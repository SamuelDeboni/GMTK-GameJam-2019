using Godot;
using System;

public class PurpleBall : KinematicBody2D
{
	public Vector2 velocity;
    
	public override void _Process(float delta)
	{
		MoveAndCollide(velocity * delta);
	}
}
