using Godot;
using System;

public class Player : KinematicBody2D
{

	[Export]
	float jumpHeight, riseTime, fallTime, maxSpeed, accTime, stopTime;
	float gRise, gFall, jumpSpeed, accel, decel;

	Vector2 speed;


	public override void _Ready()
	{
		// calculate jump parameters
		jumpSpeed = -jumpHeight * 2 / riseTime;
		gRise = (jumpHeight * 2) / (riseTime * riseTime);
		gFall = (jumpHeight * 2) / (fallTime * fallTime);
		// calculate motion parameters
		accel = maxSpeed / accTime;
		decel = maxSpeed / stopTime;

		GD.Print(accel.ToString() + " " + decel.ToString() + " " + jumpSpeed.ToString());
	}
	
	Vector2 gunDir = new Vector2(1,0);

	public override void _Process(float delta)
	{
		DoMovment(delta);
		DoGravity(delta);
		MoveAndSlide(speed,new Vector2(0,-1));

		if(Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left")
		|| Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down")) {
			gunDir.x = Input.GetActionStrength("ui_right") 
				- Input.GetActionStrength("ui_left");
			gunDir.y = Input.GetActionStrength("ui_down") 
				- Input.GetActionStrength("ui_up");
		}
		
		if(Input.IsActionPressed("fire"))
			Shoot(gunDir);
	}

	void DoMovment(float delta)
	{
		float movInput = Input.GetActionStrength("ui_right") 
				- Input.GetActionStrength("ui_left");

		if (Mathf.Abs(speed.x) > 50 && movInput == 0)
			speed.x -= Math.Sign(speed.x) * decel * delta;
		else if (movInput == 0)
			speed.x = 0;
		
		if (movInput != 0)
			speed.x += accel * movInput * delta;
		
		speed.x = Mathf.Clamp(speed.x, -maxSpeed, maxSpeed);
	}

	void DoGravity(float delta)
	{
		if(!IsOnFloor()) {
			if(speed.y < 0)
				speed.y += gRise * delta;
			else
				speed.y += gFall * delta;
		} else {
			if(Input.IsActionJustPressed("jump"))
				speed.y = jumpSpeed;
			
			if(speed.y > 5)
				speed.y = 5;
		}
	}

	void Shoot(Vector2 dir)
	{
		if(GetNode<Timer>("GunTimer").IsStopped()) {
			if(dir == Vector2.Zero) 
				dir = new Vector2(1,0);
			
			var projectile = GD.Load<PackedScene>("res://PlayerProjectile.tscn");
			PlayerProjectile projectileInst = projectile.Instance() as PlayerProjectile;
			
			(projectileInst as KinematicBody2D).Position = this.Position;
			projectileInst.speed = dir * 800 + new Vector2(speed.x,0);
			
			GetParent().AddChild(projectileInst as KinematicBody2D);
			GetNode<Timer>("GunTimer").Start();
		}
	}
}
