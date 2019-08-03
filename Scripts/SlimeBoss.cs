using Godot;
using System;

public class SlimeBoss : Node2D
{
	PackedScene JumpySlime = GD.Load<PackedScene>("res://JumpySlime.tscn");

	[Signal]
	public delegate void _next_stage();

	public override void _Ready()
	{
		KinematicBody2D slime = JumpySlime.Instance() as KinematicBody2D;
		this.AddChild(slime);
		slime.Position = new Vector2(992,592);
		GD.Print("start");
	}

	public void _on_stage_0_end()
	{
		GD.Print("Stage 0 ended");
		EmitSignal("_next_stage");
	}

}
