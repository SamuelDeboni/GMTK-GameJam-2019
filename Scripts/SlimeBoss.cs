using Godot;
using System;

public class SlimeBoss : Node2D
{
	[Export]
	public PackedScene JumpySlimePck, RedSlimePck, PurpleSlimePck;

	[Signal]
	public delegate void _next_stage();
	[Signal]
	public delegate void _shake_camera(float intensity);

	int currentStage = 0;

	float lifeBarTarget = 100, lifeBarValue;

	public override void _Ready()
	{
		KinematicBody2D slime = JumpySlimePck.Instance() as KinematicBody2D;
		this.AddChild(slime);
		slime.Position = new Vector2(500,592);
		GD.Print("start");
	}

	public override void _Process(float delta)
	{
		GetNode<ProgressBar>("CanvasLayer/ProgressBar").SetValue(lifeBarValue);
		if(Math.Abs(lifeBarTarget - lifeBarValue) > 0)
			lifeBarValue += Math.Sign(lifeBarTarget - lifeBarValue) * 2;
	} 

	public void _on_stage_end()
	{
		GD.Print("Stage ended");
		if (currentStage < 2)
			EmitSignal("_next_stage");
	}

	public void _on_damage()
	{
		EmitSignal("_shake_camera", 5);
	}

	public void UpdateBar(float value) 
	{
		lifeBarTarget = value;
	}

	public void SpawnNext(Vector2 pos)
	{
		currentStage++;

		switch(currentStage) {

			case 1: {
				GD.Print("stage 1");
				KinematicBody2D slime = RedSlimePck.Instance() as KinematicBody2D;
				this.AddChild(slime);
				slime.Position = pos;
				break; 
			}
			case 2: {
				GD.Print("stage 2");
				KinematicBody2D slime = PurpleSlimePck.Instance() as KinematicBody2D;
				this.AddChild(slime);
				slime.Position = pos;
				break;
			}
			case 3: {
				GD.Print("stage 3");
				break;
			}
			default:
				
				break;
		}

	}

}
