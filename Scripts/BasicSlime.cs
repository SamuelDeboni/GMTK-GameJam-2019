using Godot;
using System;

public class BasicSlime : KinematicBody2D
{
	const float GRAVITY = 800;

	[Export]
	int hp = 10;
	protected Vector2 velocity;
	
	[Signal]
	public delegate void _on_stage_end();

	public bool dead = false;

	public void Ready()
	{
		this.Connect("_on_stage_end",GetParent(),nameof(_on_stage_end));
	}

	public void Process(float delta)
	{
		if(!IsOnFloor())
			velocity += Vector2.Down * GRAVITY * delta;
		else
			velocity.x -= velocity.x * 0.1f;
		MoveAndSlide(velocity, new Vector2(0,-1));
	}

	public void Damage(int damage)
	{
		this.hp -= damage;
		if(this.hp <= 0) {
			if(!dead)
				EmitSignal(nameof(_on_stage_end));
			dead = true;
		}
	}

}
