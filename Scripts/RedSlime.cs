using Godot;
using System;

public class RedSlime : BasicSlime
{	
	Vector2 jumpStrength = new Vector2(200, 300);
	Node2D target = null;
	Timer jumpTimer = null;
	Timer spawnTimer = null;

	[Export]
	public PackedScene minion;

	public override void _Ready()
	{
		jumpTimer = new Timer();
		jumpTimer.Autostart = true;
		jumpTimer.WaitTime = 2;
		AddChild(jumpTimer);
		jumpTimer.Connect("timeout", this, nameof(Jump));
		jumpTimer.Start();

		spawnTimer = new Timer();
		spawnTimer.Autostart = true;
		spawnTimer.WaitTime = 5f;
		AddChild(spawnTimer);
		spawnTimer.Connect("timeout", this, nameof(Spawn));
		spawnTimer.Start();

		target = GetNode("../../Player") as Node2D;
		this.Ready();
	}

	public override void _Process(float delta)
	{
		this.Process(delta);

		if (dead && Position.x > 3200) {
			GetParent<SlimeBoss>().SpawnNext(Position);
			this.QueueFree();
		}

		if (velocity.y >= 0 && spawnTimer.TimeLeft > 0.5f)
			GetNode<AnimatedSprite>("AnimatedSprite").SetAnimation("default");
		else if (spawnTimer.TimeLeft > 0.5f)
			GetNode<AnimatedSprite>("AnimatedSprite").SetAnimation("jump");
		else
			GetNode<AnimatedSprite>("AnimatedSprite").SetAnimation("spawning");
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

	public void Spawn()
	{
		if(!dead) {
			JumpySlime slime = minion.Instance() as JumpySlime;
			slime.Position = Position + new Vector2(-32,-32);
			(slime as BasicSlime).isMinion = true;
			(slime as BasicSlime).hp = 5;
			GetParent().AddChild(slime);
		}
	}
}
