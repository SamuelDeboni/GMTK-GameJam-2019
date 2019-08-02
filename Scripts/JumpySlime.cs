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
		jumpTimer.WaitTime = 3;
		AddChild(jumpTimer);
		jumpTimer.Connect("timeout", this, nameof(Jump));
		jumpTimer.Start();

		target = GetNode("../Player") as Node2D;
	}

	public void Jump()
	{
		GD.Print(velocity);
		if (target == null)
			target = GetNode("../Player") as Node2D;

		int direction = 1;
		if (target != null && target.Position.x < Position.x)
			direction = -1;

		velocity += new Vector2(direction, -1) * jumpStrength;
	}
}
