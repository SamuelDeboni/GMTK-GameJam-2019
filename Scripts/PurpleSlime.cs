using Godot;
using System;

public class PurpleSlime : BasicSlime
{

	Vector2 jumpStrength = new Vector2(200, 300);
	Node2D target = null;
	Timer jumpTimer = null;
	Timer shootTimer = null;

	public override void _Ready()
	{
		

		jumpTimer = new Timer();
		jumpTimer.Autostart = true;
		jumpTimer.WaitTime = 2;
		AddChild(jumpTimer);
		jumpTimer.Connect("timeout", this, nameof(Jump));
		jumpTimer.Start();

		shootTimer = new Timer();
		shootTimer.Autostart = true;
		shootTimer.WaitTime = 5f;
		AddChild(shootTimer);
		shootTimer.Connect("timeout", this, nameof(Shoot));
		shootTimer.Start();

		this.Ready();
	}

	public override void _Process(float delta)
	{
		this.Process(delta);
	}

	public void Jump()
	{
		if (target == null && !dead)
			target = GetNode("../../Player") as Node2D;

		int direction = 1;
		if (target != null && target.Position.x < Position.x && !dead)
			direction = -1;

		velocity = new Vector2(direction, -1) * jumpStrength;

		if (velocity.y >= 0 && shootTimer.TimeLeft > 0.5f) {
			GetNode<AnimatedSprite>("SlimeSprite").SetAnimation("default");
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("default");
		} else if (shootTimer.TimeLeft > 0.5f) {
			GetNode<AnimatedSprite>("SlimeSprite").SetAnimation("jump");
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("jump");
		} else {
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("shooting");
			jumpTimer.Start();
		}

		
	}

	public void Shoot()
	{

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
