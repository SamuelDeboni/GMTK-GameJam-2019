using Godot;
using System;

public class BasicSlime : KinematicBody2D
{
	const float GRAVITY = 800;

	int hp;
	protected Vector2 velocity;
	
	public override void _Process(float delta)
	{
		velocity += Vector2.Down * GRAVITY * delta;
		velocity = MoveAndSlide(velocity);
	}
}
