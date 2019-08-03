using Godot;
using System;

public class BasicSlime : KinematicBody2D
{
	const float GRAVITY = 800;

	[Export]
	int hp = 10;
	protected Vector2 velocity;
	
	[Signal]
	public delegate void _on_stage_0_end();

	public bool dead = false;
	public override void _Process(float delta)
	{
		if(!IsOnFloor())
			velocity += Vector2.Down * GRAVITY * delta;
		else
			velocity.x -= velocity.x * 0.1f;
		MoveAndSlide(velocity, new Vector2(0,-1));

		if (velocity.y >= 0)
			GetNode<AnimatedSprite>("AnimatedSprite").SetAnimation("default");
		else
			GetNode<AnimatedSprite>("AnimatedSprite").SetAnimation("jump");
		
		if (dead && Position.x > 1900) {
			GetParent<SlimeBoss>().SpawnNext(Position);
			this.QueueFree();
		}
	}

	public void Damage(int damage)
	{
		this.hp -= damage;
		if(this.hp <= 0) {
			if(!dead)
				EmitSignal(nameof(_on_stage_0_end));
			dead = true;
		}
	}

}
