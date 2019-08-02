using Godot;
using System;

public class PlayerProjectile : KinematicBody2D
{
	public Vector2 speed;

	public override void _Process(float delta)
	{
		MoveAndCollide(speed * delta);
	}

	public void Die()
	{
		this.QueueFree();
	}
}
