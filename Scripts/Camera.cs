using Godot;
using System;

public class Camera : Node2D
{
	const int SPEED = 100;

	int targetPositionX = 0;

	float shakeStrength;
	float shakeMaxStrength = 100f;
	float shakeDropoff = 0.5f;

	public override void _Ready()
	{
		GetNode("../SlimeBoss").Connect("_next_stage", this, nameof(NextLevel));
		GetNode("../SlimeBoss").Connect("_shake_camera", this, nameof(Shake));
	}

	public override void _Process(float delta)
	{
		if (targetPositionX - Position.x > SPEED * delta)
			Position += new Vector2(SPEED * delta, 0);
		else
			Position = new Vector2(targetPositionX, 0);
	}

	public override void _PhysicsProcess(float delta)
	{
		Camera2D cam = GetNode("Camera2D") as Camera2D;
		if (shakeStrength > 0.1f)
		{
			cam.Offset = RandomInsideUnitCircle() * shakeStrength;
			shakeStrength *= shakeDropoff;
		}
		else
		{
			shakeStrength = 0f;
			cam.Offset = new Vector2(0, 0);
		}
	}

	public void NextLevel()
	{
		targetPositionX += 640;
	}

	public void Shake(float intensity)
	{
		GD.Print("shake");
		shakeStrength += intensity;
		shakeStrength = Mathf.Clamp(shakeStrength, 0, shakeMaxStrength);
	}

	private Vector2 RandomInsideUnitCircle()
	{
		float r = Mathf.Sqrt((float)GD.RandRange(0, 1));
		float theta = (float)GD.RandRange(0, 1) * 2 * Mathf.Pi;
		return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * r;
	}
}
