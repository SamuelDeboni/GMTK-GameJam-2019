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
	
	public override void _Process(float delta)
	{
		// movment logic --------------------------------------
		float movInput = Input.GetActionStrength("ui_right") 
				- Input.GetActionStrength("ui_left");

		if (Mathf.Abs(speed.x) > 50 && movInput == 0)
			speed.x -= Math.Sign(speed.x) * decel * delta;
		else if (movInput == 0)
			speed.x = 0;
		
		if (movInput != 0)
			speed.x += accel * movInput * delta;
		
		speed.x = Mathf.Clamp(speed.x, -maxSpeed, maxSpeed);
		
		MoveAndSlide(speed,new Vector2(0,-1));

		// gravity logic --------------------------------------
		if(!IsOnFloor()){
			if(speed.y > 0)
				speed.y += gRise * delta;
			else
				speed.y += gFall * delta;
		} else {
			if(Input.IsActionJustPressed("jump"))
				speed.y = jumpSpeed;
			
			if(speed.y > 5)
				speed.y = 5;
		}

		GD.Print(movInput);
	}
}
