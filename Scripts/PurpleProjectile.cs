using Godot;
using System;

public class PurpleProjectile : KinematicBody2D
{
	public Vector2 velocity;
	public override void _Process(float delta)
	{
		MoveAndCollide(velocity * delta);
		velocity.y += 800 * delta;
	}
}
