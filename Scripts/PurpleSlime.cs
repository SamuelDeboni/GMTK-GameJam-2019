using Godot;
using System;

public class PurpleSlime : BasicSlime
{

	Vector2 jumpStrength = new Vector2(200, 300);
	Node2D target = null;
	Timer jumpTimer = null;
	Timer shootTimer = null;

	[Export]
	PackedScene slimeProjectilePck;

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
		shootTimer.WaitTime = 1f;
		AddChild(shootTimer);
		shootTimer.Connect("timeout", this, nameof(Shoot));
		shootTimer.Start();

		this.Ready();
	}

	public override void _Process(float delta)
	{
		this.Process(delta);

		if (velocity.y >= 0)
			GetNode<AnimatedSprite>("SlimeSprite").SetAnimation("default");
		else 
			GetNode<AnimatedSprite>("SlimeSprite").SetAnimation("jump");
 
		
		if (shootTimer.TimeLeft < 0.2f) {
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("shooting");
			jumpTimer.Start();
		} else if (velocity.y >= 0) {
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("default");
		} else { 
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("jump");
		}
	}

	public void Jump()
	{
		if (target == null && !dead)
			target = GetNode("../../Player") as Node2D;

		int direction = 1;
		if (target != null && target.Position.x < Position.x && !dead)
			direction = -1;

		velocity = (new Vector2(direction, -1) * jumpStrength);
	}

	public void Shoot()
	{
		KinematicBody2D sb = slimeProjectilePck.Instance() as KinematicBody2D;
		sb.Position = Position + new Vector2(-32,-32);

		int direction = 1;
		if (target != null && target.Position.x < Position.x && !dead)
			direction = -1;
		(sb as PurpleProjectile).velocity = 2*(new Vector2(direction, -1) * jumpStrength);
		GetParent().AddChild(sb);
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
