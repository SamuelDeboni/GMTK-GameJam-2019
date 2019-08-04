using Godot;
using System;

public class BasicSlime : KinematicBody2D
{
	const float GRAVITY = 800;

	[Export]
	public int Maxhp = 10;

	public int hp;
	protected Vector2 velocity;
	
	[Signal]
	public delegate void _on_stage_end();
	[Signal]
	public delegate void _on_damage();

	[Signal]
	public delegate void UpdateBar(float value);

	public bool dead = false;

	public bool isMinion = false;

	public void Ready()
	{
		hp = Maxhp;
		this.Connect("_on_stage_end",GetParent(),nameof(_on_stage_end));
		this.Connect("_on_damage", GetParent(), nameof(_on_damage));

		if (!isMinion)
			this.Connect("UpdateBar",GetParent(),nameof(UpdateBar));
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
		EmitSignal(nameof(_on_damage));

		if (!isMinion && !dead)
			EmitSignal(nameof(UpdateBar),100*((float)hp/(float)Maxhp));

		if(this.hp <= 0) {
			if(!dead & !isMinion) {
				EmitSignal(nameof(_on_stage_end));
				EmitSignal(nameof(UpdateBar),100);
			}
			dead = true;
			if(isMinion)
				this.QueueFree();
		}
	}

}
