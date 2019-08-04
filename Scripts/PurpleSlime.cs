using Godot;
using System;

public class PurpleSlime : BasicSlime
{

	Vector2 jumpStrength = new Vector2(200, 300);
	Node2D target = null;
	Timer jumpTimer = null;
	Timer shootTimer = null;
	Timer shootBallTimer = null;

	bool ballAnimationRan = false;

	[Export]
	PackedScene slimeProjectilePck;
	[Export]
	PackedScene slimeBallPck;

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
		shootTimer.WaitTime = 3;
		AddChild(shootTimer);
		shootTimer.Connect("timeout", this, nameof(Shoot));
		shootTimer.Start();

		shootBallTimer = new Timer();
		shootBallTimer.Autostart = true;
		shootBallTimer.WaitTime = 5;
		AddChild(shootBallTimer);
		shootBallTimer.Connect("timeout", this, nameof(ShootBall));
		shootBallTimer.Start();

		this.Ready();
	}

	public override void _Process(float delta)
	{
		this.Process(delta);

		if (velocity.y >= 0)
			GetNode<AnimatedSprite>("SlimeSprite").SetAnimation("default");
		else 
			GetNode<AnimatedSprite>("SlimeSprite").SetAnimation("jump");
 
		if (shootTimer.TimeLeft < 0.2f) 
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("shooting");
		 else if (velocity.y >= 0) 
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("default");
		 else 
			GetNode<AnimatedSprite>("ArmorSprite").SetAnimation("jump");

		if (shootBallTimer.TimeLeft < 0.8f && !ballAnimationRan)
		{
			GetNode<AnimatedSprite>("BallSprite").SetAnimation("default");
			GetNode<AnimatedSprite>("BallSprite").Frame = 0;
			int direction = target.Position.x > Position.x ? 1 : -1;
			GetNode<AnimatedSprite>("BallSprite").FlipH = direction == -1;
			ballAnimationRan = true;
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
		{
			KinematicBody2D sb = slimeProjectilePck.Instance() as KinematicBody2D;
			sb.Position = Position + new Vector2(-32,-32);

			int direction = 1;
			if (target != null && target.Position.x < Position.x && !dead)
				direction = -1;
			(sb as PurpleProjectile).velocity = 2*(new Vector2(direction, -1) * jumpStrength);
			GetParent().AddChild(sb);
		}

		{
			KinematicBody2D sb = slimeProjectilePck.Instance() as KinematicBody2D;
			sb.Position = Position + new Vector2(32,-32);

			int direction = 1;
			if (target != null && target.Position.x < Position.x && !dead)
				direction = -1;
			(sb as PurpleProjectile).velocity = 2*(new Vector2(direction, -1) * jumpStrength);
			GetParent().AddChild(sb);
		}
	}

	public void ShootBall()
	{		
		if (target != null && !dead)
		{
			int direction = target.Position.x > Position.x ? 1 : -1;
			GetNode<AnimatedSprite>("BallSprite").FlipH = direction == -1;
			ballAnimationRan = false;
			KinematicBody2D sb = slimeBallPck.Instance() as KinematicBody2D;
			sb.Position = Position + new Vector2(36 * direction, 11);
			(sb as PurpleBall).velocity = (target.Position - sb.Position).Normalized() * 200;
			GetParent().AddChild(sb);
		}
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
