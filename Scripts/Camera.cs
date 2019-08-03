using Godot;
using System;

public class Camera : Node2D
{
	const int SPEED = 200;
	int targetPositionX = 0;

	public override void _Process(float delta)
	{
		if (targetPositionX - Position.x > SPEED * delta)
			Position += new Vector2(SPEED * delta, 0);
		else
			Position = new Vector2(targetPositionX, 0);

		if (Input.IsActionJustPressed("test"))
			this.NextLevel();
	}

	public void NextLevel()
	{
		targetPositionX += 1280;
	}
}
