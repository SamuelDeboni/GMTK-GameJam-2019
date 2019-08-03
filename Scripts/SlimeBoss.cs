using Godot;
using System;

public class SlimeBoss : Node2D
{
	[Export]
	public PackedScene JumpySlimePck, RedSlimePck;

	[Signal]
	public delegate void _next_stage();

	int currentStage = 0;

	public override void _Ready()
	{
		KinematicBody2D slime = JumpySlimePck.Instance() as KinematicBody2D;
		this.AddChild(slime);
		slime.Position = new Vector2(992,592);
		GD.Print("start");
	}

	public void _on_stage_end()
	{
		GD.Print("Stage ended");
		EmitSignal("_next_stage");
	}

	public void SpawnNext(Vector2 pos)
	{
		currentStage++;

		switch(currentStage) {

			case 1:
				GD.Print("stage 1");
				KinematicBody2D slime = RedSlimePck.Instance() as KinematicBody2D;
				this.AddChild(slime);
				slime.Position = pos;
				break;

			case 2:
				GD.Print("stage 2");
				break;
			
			default:

				break;
		}

	}

}
