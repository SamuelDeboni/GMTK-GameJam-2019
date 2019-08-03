using Godot;
using System;

public class JumpySlime : BasicSlime
{
	Vector2 jumpStrength = new Vector2(200, 500);
	Node2D target = null;
	Timer jumpTimer = null;


	
	public override void _Ready()
	{
		jumpTimer = new Timer();
		jumpTimer.Autostart = true;
		jumpTimer.WaitTime = 2;
		AddChild(jumpTimer);
		jumpTimer.Connect("timeout", this, nameof(Jump));
		jumpTimer.Start();
		target = GetNode("../../Player") as Node2D;
		this.Ready();
	}

	public override void _Process(float delta)
	{
		this.Process(delta);

		if (velocity.y >= 0)
			GetNode<AnimatedSprite>("AnimatedSprite").SetAnimation("default");
		else
			GetNode<AnimatedSprite>("AnimatedSprite").SetAnimation("jump");
		
		if (dead && Position.x > 1900) {
			GetParent<SlimeBoss>().SpawnNext(Position);
			this.QueueFree();
		}
	}

	public void Jump()
	{
		if (target == null && !dead)
			target = GetNode("../../Player") as Node2D;

		int direction = 1;
		if (target != null && target.Position.x < Position.x && !dead)
			direction = -1;

		velocity = new Vector2(direction, -1) * jumpStrength;
	}

	public void _on_Area2D_body_entered(PhysicsBody2D boddy)
	{
		var b = boddy as KinematicBody2D;

		if (b != null && b is PlayerProjectile){
			this.Damage(1);
			b.QueueFree();
		}
	}
}
